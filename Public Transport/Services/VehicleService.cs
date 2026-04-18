using Public_Transport.Factories;
using Public_Transport.Interfaces;
using Public_Transport.Repositories;

namespace Public_Transport.Services
{
    public class VehicleService
    {
        private SqlVehicleRepository _repository;

        public VehicleService(SqlVehicleRepository sqlRepository)
        {
            _repository = sqlRepository;
        }

        public void AddVehicle(
            VehicleType vehicleType,
            string model,
            double capacity,
            double? fuelConsumption,
            double? batteryCapacity
        )
        {
            var vehicle = VehicleFactory.CreateVehicle(
                vehicleType,
                model,
                capacity,
                fuelConsumption,
                batteryCapacity
            );
            _repository.AddVehicle(vehicle);
        }

        public List<Vehicle> GetAllVehicles() => _repository.GetVehicles();

        public Vehicle? GetVehicleById(int id) => _repository.GetVehicleById(id);

        //public void RefuelVehicle(IRefuelable refuelable, double liters)
        //{
        //    refuelable.Refuel(liters);
        //}

        //public void ChargeVehicle(IElectric electric, double kwh)
        //{
        //    electric.Charge(kwh);
        //}
    }
}
