public class Metro : ElectricVehicle
{
    public Metro(int id, string model, double capacity, double batteryCapacity)
        : base(id, model, capacity, batteryCapacity, VehicleType.Metro) { }

    public Metro(string model, double capacity, double batteryCapacity)
        : base(model, capacity, batteryCapacity, VehicleType.Metro) { }

    public override void Charge(double kwh)
    {
        throw new NotImplementedException();
    }
}
