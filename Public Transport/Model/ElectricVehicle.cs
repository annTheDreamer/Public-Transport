using Public_Transport.Interfaces;

public abstract class ElectricVehicle : Vehicle, IElectric
{
    public double BatteryCapacity { get; set; }

    protected ElectricVehicle(
        int id,
        string model,
        double capacity,
        double batteryCapacity,
        VehicleType type
    )
        : base(id, model, capacity, type)
    {
        BatteryCapacity = batteryCapacity;
    }

    protected ElectricVehicle(
        string model,
        double capacity,
        double batteryCapacity,
        VehicleType type
    )
        : base(model, capacity, type)
    {
        BatteryCapacity = batteryCapacity;
    }

    public abstract void Charge(double kwh);
}
