using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;
using Amazon.CloudWatch.EMF.Logger;
using Amazon.CloudWatch.EMF.Model;
using Microsoft.Extensions.Logging.Abstractions;
using Unit = Amazon.CloudWatch.EMF.Model.Unit;

namespace Defra.TradeImportsDataApi.Api.Metrics;

[ExcludeFromCodeCoverage]
public static class EmfExportExtensions
{
    public static IApplicationBuilder UseEmfExporter(this IApplicationBuilder builder)
    {
        var config = builder.ApplicationServices.GetRequiredService<IConfiguration>();
        var enabled = config.GetValue("AWS_EMF_ENABLED", true);

        if (enabled)
        {
            var ns = config.GetValue<string>("AWS_EMF_NAMESPACE");
            EmfExporter.Init(builder.ApplicationServices.GetRequiredService<ILoggerFactory>(), ns!);
        }

        return builder;
    }
}

[ExcludeFromCodeCoverage]
public static class EmfExporter
{
    private static readonly MeterListener MeterListener = new();
    private static ILogger _logger = null!;
    private static ILoggerFactory _loggerFactory = NullLoggerFactory.Instance;
    private static string? _awsNamespace;

    public static void Init(ILoggerFactory loggerFactory, string? awsNamespace)
    {
        _logger = loggerFactory.CreateLogger(nameof(EmfExporter));
        _loggerFactory = loggerFactory;
        _awsNamespace = awsNamespace;
        MeterListener.InstrumentPublished = (instrument, listener) =>
        {
            if (instrument.Meter.Name is RequestMetrics.MeterName)
            {
                listener.EnableMeasurementEvents(instrument);
            }
        };

        MeterListener.SetMeasurementEventCallback<int>(OnMeasurementRecorded);
        MeterListener.SetMeasurementEventCallback<long>(OnMeasurementRecorded);
        MeterListener.SetMeasurementEventCallback<double>(OnMeasurementRecorded);
        MeterListener.Start();
    }

    private static void OnMeasurementRecorded<T>(
        Instrument instrument,
        T measurement,
        ReadOnlySpan<KeyValuePair<string, object?>> tags,
        object? state
    )
    {
        try
        {
            using var metricsLogger = new MetricsLogger(_loggerFactory);

            metricsLogger.SetNamespace(_awsNamespace);
            var dimensionSet = new DimensionSet();
            foreach (var tag in tags)
            {
                if (string.IsNullOrWhiteSpace(tag.Value?.ToString()))
                    continue;
                dimensionSet.AddDimension(tag.Key, tag.Value?.ToString());
            }

            metricsLogger.SetDimensions(dimensionSet);
            var name = instrument.Name;

            metricsLogger.PutMetric(name, Convert.ToDouble(measurement), Enum.Parse<Unit>(instrument.Unit!));
            metricsLogger.Flush();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to push EMF metric");
        }
    }
}
