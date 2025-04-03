using Microsoft.Extensions.Hosting;

namespace Defra.TradeImportsData.Api.IntegrationTests;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiWebApplicationFactory : TestWebApplicationFactory<Program>
{
    private static readonly Lock s_lock = new();

    protected override IHost CreateHost(IHostBuilder builder)
    {
        // There is an issue with using CreateBootstrapLogger during host creation
        // that has started happening since the recent CDP Serilog changes have
        // been introduced. In tests, multiple hosts are created in parallel but
        // the CreateBootstrapLogger code is not thread safe and can throw errors.
        //
        // We can mitigate this issue from here by locking host creation so we
        // don't need to change host creation of the app itself.
        builder.UseEnvironment("IntegrationTests");
        lock (s_lock)
            return base.CreateHost(builder);
    }
}
