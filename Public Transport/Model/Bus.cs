using Public_Transport.Interfaces;

public class Bus : Vehicle, IRefuelable
{
    public double FuelConsumption { get; init; }

    public Bus(int id, string model, double capacity, double fuelConsumption)
        : base(id, model, capacity, VehicleType.Bus)
    {
        FuelConsumption = fuelConsumption;
    }

    public Bus(string model, double capacity, double fuelConsumption)
        : base(model, capacity, VehicleType.Bus)
    {
        FuelConsumption = fuelConsumption;
    }

    public void Refuel(double liters)
    {
        throw new NotImplementedException();
    }
}
