public class Metro : ElectricVehicle
{
    public Metro(int id, string model, double capacity, double batteryCapacity)
        : base(id, model, capacity, batteryCapacity, VehicleType.Metro) { }

    public Metro(string model, double capacity, double batteryCapacity)
        : base(model, capacity, batteryCapacity, VehicleType.Metro) { }

    public override void Charge(double kwh)
    {
        var capacity = this.Capacity;
        if (kwh > capacity)
            throw new ArgumentOutOfRangeException(
                $"The capacity of {nameof(this.Model)} is {capacity}."
            );

        Console.WriteLine(
            $"{nameof(this.Model)} was recharged - battery capacity increased by {kwh} hwh."
        );
    }
}
