using Microsoft.Extensions.Configuration;
using Public_Transport.App;
using Public_Transport.Repositories;
using Public_Transport.Services;
using Public_Transport.UI;

var appConfiguration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: false)
    .Build();

string connectionString = appConfiguration.GetConnectionString("PublicTransportDb") ?? "";

var sqlRepository = new SqlVehicleRepository(connectionString);
var vehicleService = new VehicleService(sqlRepository);
var consoleUi = new ConsoleUI(vehicleService);

var app = new PublicTransportApp(consoleUi);
app.Start();
Console.WriteLine();
