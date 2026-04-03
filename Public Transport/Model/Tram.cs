public class Tram : ElectricVehicle
{
    public Tram(int id, string model, double capacity, double batteryCapacity)
        : base(id, model, capacity, batteryCapacity, VehicleType.Tram) { }

    public Tram(string model, double capacity, double batteryCapacity)
        : base(model, capacity, batteryCapacity, VehicleType.Tram) { }

    public override void Charge(double kwh)
    {
        throw new NotImplementedException();
    }
}
