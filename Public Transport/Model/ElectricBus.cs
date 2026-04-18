public class ElectricBus : ElectricVehicle
{
    public ElectricBus(int id, string model, double capacity, double batteryCapacity)
        : base(id, model, capacity, batteryCapacity, VehicleType.ElectricBus) { }

    public ElectricBus(string model, double capacity, double batteryCapacity)
        : base(model, capacity, batteryCapacity, VehicleType.ElectricBus) { }

    public override void Charge(double kwh)
    {
        //var capacity = this.Capacity;
        //if (kwh > capacity)
        //    throw new ArgumentOutOfRangeException(
        //        $"The capacity of {nameof(this.Model)} is {capacity}."
        //    );

        Console.WriteLine($"{this.Model} was recharged - battery capacity increased by {kwh} kwh.");
    }
}
