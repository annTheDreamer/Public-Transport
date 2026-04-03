public class ElectricBus : ElectricVehicle
{
    public ElectricBus(int id, string model, double capacity, double batteryCapacity)
        : base(id, model, capacity, batteryCapacity, VehicleType.ElectricBus) { }

    public ElectricBus(string model, double capacity, double batteryCapacity)
        : base(model, capacity, batteryCapacity, VehicleType.ElectricBus) { }

    public override void Charge(double kwh)
    {
        throw new NotImplementedException();
    }
}
