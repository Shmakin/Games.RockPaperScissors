using Games.RockPaperScissors.Application.Configurations.DI;
using Games.RockPaperScissors.External.Migrations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Games.RockPaperScissors.Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MigrateDb();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, serviceCollection) =>
                {
                    IConfiguration configuration = context.Configuration;
                    serviceCollection.AddSingleton(configuration);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureWebHost(builder =>
                {
                    builder.UseKestrel();
                });

        private static void MigrateDb()
        {
            CreateScoreboardTableMigrationStep migrationStep = new CreateScoreboardTableMigrationStep("Data Source=Application.db;Cache=Shared");
            migrationStep.Apply();
        }
    }
}