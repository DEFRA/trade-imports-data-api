using Elastic.CommonSchema.Serilog;
using Serilog.Events;

namespace Defra.TradeImportsDataApi.Api.Utils.Logging;

public class CustomEcsTextFormatter : EcsTextFormatter
{
    public override void Format(LogEvent logEvent, TextWriter output)
    {
        var ecsEvent = LogEventConverter.ConvertToEcs(logEvent, Configuration);

        if (logEvent.Properties.TryGetValue("Prefix", out var prefix))
            ecsEvent.Message = $"{prefix} {ecsEvent.Message}";

        output.WriteLine(ecsEvent.Serialize());
    }
}
