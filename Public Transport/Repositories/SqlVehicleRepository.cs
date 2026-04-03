using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using Public_Transport.Interfaces;

namespace Public_Transport.Repositories
{
    public class SqlVehicleRepository
    {
        private readonly string _sqlConnection;

        public SqlVehicleRepository(string connectionString)
        {
            _sqlConnection = connectionString;
        }

        // Add
        public void AddVehicle(Vehicle vehicle)
        {
            using var connection = new SqlConnection(_sqlConnection);
            connection.Open();

            string sql =
                @"
                INSERT INTO Vehicles (Model, Capacity, VehicleType, FuelConsumption, BatteryCapacity)
                VALUES (@Model, @Capacity, @VehicleType, @FuelConsumption, @BatteryCapacity)";

            using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Model", vehicle.Model);
            command.Parameters.AddWithValue("@Capacity", vehicle.Capacity);
            command.Parameters.AddWithValue("@VehicleType", vehicle.Type.ToString());

            object fuelConsumption = DBNull.Value;
            object batteryCapacity = DBNull.Value;

            if (vehicle is IRefuelable refuelable)
            {
                fuelConsumption = refuelable.FuelConsumption;
            }
            else if (vehicle is IElectric electric)
            {
                batteryCapacity = electric.BatteryCapacity;
            }

            command.Parameters.AddWithValue("@FuelConsumption", fuelConsumption);
            command.Parameters.AddWithValue("@BatteryCapacity", batteryCapacity);

            command.ExecuteNonQuery();
        }

        // Get all vehicles
        public List<Vehicle> GetVehicles()
        {
            var allVehicles = new List<Vehicle>();

            using var connection = new SqlConnection(_sqlConnection);
            connection.Open();

            string sql =
                @"
                SELECT [Id]
                    ,[Model]
                    ,[Capacity]
                    ,[VehicleType]
                    ,[FuelConsumption]
                    ,[BatteryCapacity]
                FROM [Vehicles]";

            using var command = new SqlCommand(sql, connection);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var id = (int)reader["Id"];

                var model = ((string)reader["Model"]).Trim();
                if (string.IsNullOrWhiteSpace(model))
                    throw new InvalidOperationException(
                        $"Vehicle with id {id} does not have Model value in the database."
                    );

                var capacity = (double)reader["Capacity"];

                // Handling vehicle types as enum - needed for creating the right class derived from Vehicle
                var vehicleTypeString = reader["VehicleType"].ToString()?.Trim();

                if (!Enum.TryParse<VehicleType>(vehicleTypeString, out var vehicleType))
                {
                    throw new InvalidOperationException(
                        $"Invalid vehicle type present in database: '{vehicleTypeString}'"
                    );
                }

                // Handling nullable fields
                double? fuelConsumption = null;
                if (reader["FuelConsumption"] != DBNull.Value)
                {
                    fuelConsumption = (double)reader["FuelConsumption"];
                }

                double? batteryCapacity = null;
                if (reader["BatteryCapacity"] != DBNull.Value)
                {
                    batteryCapacity = (double)reader["BatteryCapacity"];
                }

                // TO DO: move vehicle creation to different method
                switch (vehicleType)
                {
                    case VehicleType.Bus:
                        allVehicles.Add(
                            new Bus(
                                id,
                                model,
                                capacity,
                                RequireValue(fuelConsumption, "FuelConsumption", id)
                            )
                        );
                        break;

                    case VehicleType.ElectricBus:
                        allVehicles.Add(
                            new ElectricBus(
                                id,
                                model,
                                capacity,
                                RequireValue(batteryCapacity, "BatteryCapacity", id)
                            )
                        );
                        break;

                    case VehicleType.Metro:
                        allVehicles.Add(
                            new Metro(
                                id,
                                model,
                                capacity,
                                RequireValue(batteryCapacity, "BatteryCapacity", id)
                            )
                        );
                        break;

                    case VehicleType.Tram:
                        allVehicles.Add(
                            new Tram(
                                id,
                                model,
                                capacity,
                                RequireValue(batteryCapacity, "BatteryCapacity", id)
                            )
                        );
                        break;

                    default:
                        throw new InvalidOperationException(
                            $"Unsupported vehicle type: {vehicleType}"
                        );
                }
            }

            return allVehicles;
        }

        // Refuel
        // Charge

        private static double RequireValue(double? value, string fieldName, int vehicleId)
        {
            if (value is null)
            {
                throw new InvalidOperationException(
                    $"Vehicle with Id {vehicleId} does not have {fieldName} value in the database."
                );
            }

            return value.Value;
        }
    }
}
