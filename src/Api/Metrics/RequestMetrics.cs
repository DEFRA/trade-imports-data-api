using System.Diagnostics;
using System.Diagnostics.Metrics;
using Amazon.CloudWatch.EMF.Model;

namespace Defra.TradeImportsDataApi.Api.Metrics;

public class RequestMetrics
{
    private readonly Counter<long> requestsReceived;
    private readonly Counter<long> requestsFaulted;
    private readonly Histogram<double> requestDuration;

    public RequestMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MetricsConstants.MetricNames.MeterName);

        requestsReceived = meter.CreateCounter<long>(
            "RequestReceived",
            Unit.COUNT.ToString(),
            "Count of messages received"
        );

        requestDuration = meter.CreateHistogram<double>(
            "RequestDuration",
            Unit.MILLISECONDS.ToString(),
            "Duration of request"
        );

        requestsFaulted = meter.CreateCounter<long>("RequestFaulted", Unit.COUNT.ToString(), "Count of request faults");
    }

    public void RequestCompleted(string requestPath, string httpMethod, int statusCode, double milliseconds)
    {
        requestsReceived.Add(1, BuildTags(requestPath, httpMethod, statusCode));
        requestDuration.Record(milliseconds, BuildTags(requestPath, httpMethod, statusCode));
    }

    public void RequestFaulted(string requestPath, string httpMethod, int statusCode, Exception exception)
    {
        var tagList = BuildTags(requestPath, httpMethod, statusCode);
        tagList.Add(MetricsConstants.RequestTags.ExceptionType, exception.GetType().Name);
        requestsFaulted.Add(1, tagList);
    }

    private static TagList BuildTags(string requestPath, string httpMethod, int statusCode)
    {
        return new TagList
        {
            { MetricsConstants.RequestTags.Service, Process.GetCurrentProcess().ProcessName },
            { MetricsConstants.RequestTags.RequestPath, requestPath },
            { MetricsConstants.RequestTags.HttpMethod, httpMethod },
            { MetricsConstants.RequestTags.StatusCode, statusCode },
        };
    }
}
