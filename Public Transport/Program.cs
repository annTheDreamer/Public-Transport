using Microsoft.Extensions.Configuration;
using Public_Transport.Repositories;
using Public_Transport.Services;

var appConfiguration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: false)
    .Build();

string connectionString = appConfiguration.GetConnectionString("PublicTransportDb") ?? "";

var sqlRepository = new SqlVehicleRepository(connectionString);
var vehicleService = new VehicleService(sqlRepository);

var vehicles = sqlRepository.GetVehicles();
Console.WriteLine();
