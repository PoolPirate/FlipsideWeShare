using AirtableApiClient;
using Common.Extensions;
using FlipsideWeShare.Configuration;
using FlipsideWeShare.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Buffers.Text;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Reflection;


string offset = null;
string errorMessage = null;

using AirtableBase airtableBase = new AirtableBase("shrKo7PACUNKK1U92", "appXUMbcBVt3YgB4j");
var records = await airtableBase.ListRecords("tblHEDBsMC1jourQf");

Console.WriteLine("");

//
// Use 'offset' and 'pageSize' to specify the records that you want
// to retrieve.
// Only use a 'do while' loop if you want to get multiple pages
// of records.
//



//    do
//    {
//        Task<AirtableListRecordsResponse> task = airtableBase.ListRecords(
//               tblHEDBsMC1jourQf,
//               offset,
//        fields,
//               filterByFormula,
//               maxRecords,
//               pageSize,
//               sort,
//               view);

//        AirtableListRecordsResponse response = await task;

//        if (response.Success)
//        {
//            records.AddRange(response.Records.ToList());
//            offset = response.Offset;
//        }
//        else if (response.AirtableApiError is AirtableApiException)
//        {
//            errorMessage = response.AirtableApiError.ErrorMessage;
//            if (response.AirtableApiError is AirtableInvalidRequestException)
//            {
//                errorMessage += "\nDetailed error message: ";
//                errorMessage += response.AirtableApiError.DetailedErrorMessage;
//            }
//            break;
//        }
//        else
//        {
//            errorMessage = "Unknown error";
//            break;
//        }
//    } while (offset != null);
//}


var assembly = Assembly.GetEntryAssembly();
var host = CreateHostBuilder(args).Build();

var dbContext = await host.Services.GetRequiredService<IDbContextFactory<BountyContext>>().CreateDbContextAsync();
await dbContext.Database.MigrateAsync();
await dbContext.DisposeAsync();

await host.Services.InitializeApplicationAsync(assembly);
host.Services.RunApplication(assembly);

await host.RunAsync();

IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration(builder =>
        {
            builder.AddJsonFile("appsettings.json");
        })
        .ConfigureServices((hostContext, services) =>
        {
            services.AddApplication(hostContext.Configuration, options =>
            {
                options.UseServiceLevels = true;
                options.ValidateServiceLevelsOnInitialize = true;
            }, assembly);

            services.AddDbContext<BountyContext>((services, options) =>
            {
                var dbOptions = services.GetRequiredService<DbOptions>();
                options.UseNpgsql(dbOptions.ConnectionString);
            });
            services.AddDbContextFactory<BountyContext>((services, options) =>
            {
                var dbOptions = services.GetRequiredService<DbOptions>();
                options.UseNpgsql(dbOptions.ConnectionString);
            });

            services.AddSingleton<HttpClient>();
        });
}