using System;
using System.Collections.Generic;
using System.Text;
using Public_Transport.Interfaces;
using Public_Transport.Services;

namespace Public_Transport.UI
{
    public class ConsoleUI
    {
        private VehicleService _vehicleService;
        private TablePrinter _tablePrinter = new TablePrinter();
        private Dictionary<int, string> _menuOptions = new Dictionary<int, string>
        {
            { 1, "Add Vehicle" },
            { 2, "See All Vehicles" },
            { 3, "Refuel Vehicle" },
            { 4, "Charge Vehicle" },
            { 5, "Exit" },
        };

        public ConsoleUI(VehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public string GetUserInput() => Console.ReadLine() ?? "";

        public void PrintWelcomeMessage() =>
            Console.WriteLine("Welcome to the Fleet Management Project.");

        public void PrintMenuOptions()
        {
            Console.WriteLine(
                "Please select one of the options by typing the number before the option you'd like to select:"
            );
            foreach (var menuOption in _menuOptions)
            {
                Console.WriteLine($"{menuOption.Key}. {menuOption.Value}");
            }
        }

        public Dictionary<int, string> GetMenuOptions() => _menuOptions;

        public List<VehicleType> GetSupportedVehicles() => Enum.GetValues<VehicleType>().ToList();

        public void AddVehicle()
        {
            var vehicleType = SelectVehicleType();
            var model = GetVehicleModel();
            var capacity = GetVehicleProperty("capacity");
            EnergyType energyType;
            double? fuelConsumption;
            double? batteryCapacity;

            if (!VehicleTypeInfo.energyType.TryGetValue(vehicleType, out energyType))
            {
                throw new ArgumentOutOfRangeException($"{nameof(vehicleType)} is not supported");
            }

            if (energyType == EnergyType.Fuel)
            {
                fuelConsumption = GetVehicleProperty("fuel consumption");
                batteryCapacity = null;
            }
            else
            {
                batteryCapacity = GetVehicleProperty("battery capacity");
                fuelConsumption = null;
            }

            _vehicleService.AddVehicle(
                vehicleType,
                model,
                capacity,
                fuelConsumption,
                batteryCapacity
            );
            Console.WriteLine("Vehicle added successfully.");
        }

        public void PrintAllVehicles()
        {
            var allVehicles = _vehicleService.GetAllVehicles();
            Console.WriteLine();
            _tablePrinter.PrintTable(
                allVehicles,
                "Id",
                "Model",
                "Capacity",
                "FuelConsumption",
                "BatteryCapacity"
            );
            Console.WriteLine();
        }

        public IEnumerable<Vehicle> PrintAllRefuelables()
        {
            var refuelables = _vehicleService
                .GetAllVehicles()
                .Where(vehicle =>
                    VehicleTypeInfo.energyType[vehicle.GetVehicleType()] == EnergyType.Fuel
                );
            Console.WriteLine();
            _tablePrinter.PrintTable(refuelables, "Id", "Model", "Capacity", "FuelConsumption");
            return refuelables;
        }

        public IEnumerable<Vehicle> PrintAllRechargeables()
        {
            var rechargeables = _vehicleService
                .GetAllVehicles()
                .Where(vehicle =>
                    VehicleTypeInfo.energyType[vehicle.GetVehicleType()] == EnergyType.Electric
                );
            Console.WriteLine();
            _tablePrinter.PrintTable(rechargeables, "Id", "Model", "Capacity", "BatteryCapacity");
            return rechargeables;
        }

        public VehicleType SelectVehicleType()
        {
            var listOfVehicles = GetSupportedVehicles();
            Console.WriteLine("What kind of vehicle would you like to add?");
            while (true)
            {
                Console.WriteLine("We currently support the following types of vehicles:");
                Console.WriteLine(string.Join(", ", listOfVehicles));
                Console.WriteLine();
                Console.WriteLine("Please type one of the options exactly as shown above:");
                Console.WriteLine();
                var userInput = Console.ReadLine();
                var isTypeValid = Enum.TryParse<VehicleType>(
                    userInput,
                    ignoreCase: true,
                    out var vehicleType
                );
                if (isTypeValid)
                    return vehicleType;
                else
                    Console.WriteLine(
                        $"Unfortunately, we don't support {userInput} as a vehicle type."
                    );
            }
        }

        public string GetVehicleModel()
        {
            Console.WriteLine("Please enter the vehicle model");
            var userInput = GetUserInput();
            return userInput;
        }

        private double? IsDouble(string userInput) =>
            double.TryParse(userInput, out double result) ? result : null;

        public double GetVehicleProperty(string property)
        {
            while (true)
            {
                Console.WriteLine($"Please enter the {property} of the vehicle you want to add:");
                var userInput = GetUserInput();
                var isValueDouble = IsDouble(userInput);

                if (isValueDouble is null)
                    Console.WriteLine(
                        $"Invalid {property} value. Please enter a valid {property} value."
                    );
                else
                    return isValueDouble.Value;
            }
        }

        public void RefuelVehicle() => ResupplyVehicle(EnergyType.Fuel);

        public void RechargeVehicle() => ResupplyVehicle(EnergyType.Electric);

        public void ResupplyVehicle(EnergyType energyType)
        {
            switch (energyType)
            {
                case EnergyType.Fuel:
                    var refuelables = PrintAllRefuelables();
                    var refuelablesId = GetAndValidateIdFrom(refuelables);
                    if (refuelablesId is not null)
                    {
                        GetResupplyAmount(refuelablesId.Value, EnergyType.Fuel);
                    }
                    Console.WriteLine();
                    break;
                case EnergyType.Electric:
                    var rechargeables = PrintAllRechargeables();
                    var rechargeablesId = GetAndValidateIdFrom(rechargeables);
                    if (rechargeablesId is not null)
                    {
                        GetResupplyAmount(rechargeablesId.Value, EnergyType.Electric);
                    }
                    Console.WriteLine();
                    break;
            }
        }

        public void GetResupplyAmount(int id, EnergyType energyType)
        {
            var vehicle = _vehicleService.GetVehicleById(id);
            if (vehicle is null)
                throw new ArgumentNullException("No vehicle found.");
            else
            {
                string unit;
                string action;
                if (energyType == EnergyType.Fuel)
                {
                    unit = "liters";
                    action = "refueled";
                }
                else
                {
                    unit = "kwhs";
                    action = "recharged";
                }

                Console.WriteLine(
                    $"Please enter the number of {unit} you want the vehicle to be {action}:"
                );
                var units = GetUserInput();
                if (double.TryParse(units, out double parsedUnits))
                {
                    if (energyType == EnergyType.Fuel)
                    {
                        var refuelable = (IRefuelable)vehicle;
                        refuelable.Refuel(parsedUnits);
                    }
                    else
                    {
                        var rechargeable = (IElectric)vehicle;
                        rechargeable.Charge(parsedUnits);
                    }
                }
                else
                    Console.WriteLine($"The entered amount of {units} {unit} is not valid.");
            }
        }

        public int? GetAndValidateIdFrom(IEnumerable<Vehicle> vehicles)
        {
            Console.WriteLine("Please enter the ID of the vehicle you want to be recharged:");
            var id = GetUserInput();
            var isIdValid = IsIdValid(id, vehicles);
            return isIdValid ? int.Parse(id) : null;
        }

        public bool IsIdValid(string id, IEnumerable<Vehicle> vehicles)
        {
            if (!int.TryParse(id, out int parsedId))
                throw new ArgumentException("The entered ID is not a valid number.");
            else
            {
                var isVehiclePresent = vehicles.Any(vehicle => vehicle.Id == parsedId);
                if (!isVehiclePresent)
                    Console.WriteLine("The entered ID is not in the provided list.");
                return isVehiclePresent;
            }
        }
    }
}
