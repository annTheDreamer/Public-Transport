using System;
using System.Collections.Generic;
using System.Text;
using Public_Transport.Repositories;

namespace Public_Transport.Factories
{
    public static class VehicleFactory
    {
        public static Vehicle CreateVehicle(
            VehicleType vehicleType,
            string model,
            double capacity,
            double? fuelConsumption,
            double? batteryCapacity
        )
        {
            return vehicleType switch
            {
                VehicleType.Bus => new Bus(
                    model,
                    capacity,
                    RequireValue(fuelConsumption, "FuelConsumption")
                ),

                VehicleType.ElectricBus => new ElectricBus(
                    model,
                    capacity,
                    RequireValue(batteryCapacity, "BatteryCapacity")
                ),

                VehicleType.Metro => new Metro(
                    model,
                    capacity,
                    RequireValue(batteryCapacity, "BatteryCapacity")
                ),

                VehicleType.Tram => new Tram(
                    model,
                    capacity,
                    RequireValue(batteryCapacity, "BatteryCapacity")
                ),

                _ => throw new InvalidOperationException(
                    $"Unsupported vehicle type: {vehicleType}"
                ),
            };
        }

        public static Vehicle CreateVehicle(
            VehicleType vehicleType,
            int id,
            string model,
            double capacity,
            double? fuelConsumption,
            double? batteryCapacity
        )
        {
            return vehicleType switch
            {
                VehicleType.Bus => new Bus(
                    id,
                    model,
                    capacity,
                    RequireValue(fuelConsumption, "FuelConsumption")
                ),

                VehicleType.ElectricBus => new ElectricBus(
                    id,
                    model,
                    capacity,
                    RequireValue(batteryCapacity, "BatteryCapacity")
                ),

                VehicleType.Metro => new Metro(
                    id,
                    model,
                    capacity,
                    RequireValue(batteryCapacity, "BatteryCapacity")
                ),

                VehicleType.Tram => new Tram(
                    id,
                    model,
                    capacity,
                    RequireValue(batteryCapacity, "BatteryCapacity")
                ),

                _ => throw new InvalidOperationException(
                    $"Unsupported vehicle type: {vehicleType}"
                ),
            };
        }

        private static double RequireValue(double? value, string fieldName)
        {
            if (value is null)
            {
                throw new InvalidOperationException(
                    $"Vehicle does not have {fieldName} value in the database."
                );
            }

            return value.Value;
        }
    }
}
