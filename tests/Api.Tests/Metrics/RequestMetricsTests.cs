using System.Diagnostics.Metrics;
using Defra.TradeImportsDataApi.Api.Metrics;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Metrics.Testing;

namespace Defra.TradeImportsDataApi.Api.Tests.Metrics;

internal sealed class DummyMeterFactory : IMeterFactory
{
    public Meter Create(MeterOptions options) => new Meter(options);

    public void Dispose() { }
}

public class RequestMetricsTests
{
    [Fact]
    public void When_message_received_Then_counter_is_incremented()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddMetrics();
        IMeterFactory mf = serviceCollection.BuildServiceProvider().GetRequiredService<IMeterFactory>();
        var messagesReceivedCollector = new MetricCollector<long>(
            mf,
            MetricsConstants.MetricNames.MeterName,
            "RequestReceived"
        );
        var metrics = new RequestMetrics(mf);

        metrics.RequestCompleted("TestMessage1", "/test-request-path-1", 200, 200);

        var receivedMeasurements = messagesReceivedCollector.GetMeasurementSnapshot();
        receivedMeasurements.Count.Should().Be(1);
        receivedMeasurements[0].Value.Should().Be(1);
        receivedMeasurements[0].ContainsTags(MetricsConstants.RequestTags.RequestPath).Should().BeTrue();
        receivedMeasurements[0].Tags[MetricsConstants.RequestTags.RequestPath].Should().Be("TestMessage1");
        receivedMeasurements[0].ContainsTags(MetricsConstants.RequestTags.HttpMethod).Should().BeTrue();
        receivedMeasurements[0].Tags[MetricsConstants.RequestTags.HttpMethod].Should().Be("/test-request-path-1");
        receivedMeasurements[0].ContainsTags(MetricsConstants.RequestTags.StatusCode).Should().BeTrue();
        receivedMeasurements[0].Tags[MetricsConstants.RequestTags.StatusCode].Should().Be(200);
        receivedMeasurements[0].ContainsTags(MetricsConstants.RequestTags.Service).Should().BeTrue();
        receivedMeasurements[0].Tags[MetricsConstants.RequestTags.Service].Should().Be("dotnet");
    }

    [Fact]
    public void When_message_faulted_Then_counter_is_incremented()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddMetrics();
        IMeterFactory mf = serviceCollection.BuildServiceProvider().GetRequiredService<IMeterFactory>();
        var messagesReceivedCollector = new MetricCollector<long>(
            mf,
            MetricsConstants.MetricNames.MeterName,
            "RequestFaulted"
        );
        var metrics = new RequestMetrics(mf);

        metrics.RequestFaulted("TestMessage1", "/test-request-path-1", 200, new Exception("Test Message"));

        var receivedMeasurements = messagesReceivedCollector.GetMeasurementSnapshot();
        receivedMeasurements.Count.Should().Be(1);
        receivedMeasurements[0].Value.Should().Be(1);
        receivedMeasurements[0].ContainsTags(MetricsConstants.RequestTags.ExceptionType).Should().BeTrue();
        receivedMeasurements[0].Tags[MetricsConstants.RequestTags.ExceptionType].Should().Be("Exception");
    }
}
