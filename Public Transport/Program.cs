using Microsoft.Extensions.Configuration;
using Public_Transport.Repositories;

var appConfiguration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .Build();

string connectionString = appConfiguration.GetConnectionString("PublicTransportDb") ?? "";

var sqlRepository = new SqlVehicleRepository(connectionString);

var vehicles = sqlRepository.GetVehicles();
Console.WriteLine();
