using Microsoft.AspNetCore.Builder;

namespace TradeImportsDataApi.Test.Config;

public class EnvironmentTest
{

   [Fact]
   public void IsNotDevModeByDefault()
   { 
       var builder = WebApplication.CreateEmptyBuilder(new WebApplicationOptions());
       var isDev = TradeImportsDataApi.Config.Environment.IsDevMode(builder);
       Assert.False(isDev);
   }
}
