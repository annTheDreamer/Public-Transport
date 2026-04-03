public abstract class Vehicle
{
    public int Id { get; set; }
    public string Model { get; set; } = string.Empty;
    public double Capacity { get; set; }
    public VehicleType Type { get; init; }

    protected Vehicle(int id, string model, double capacity, VehicleType type)
    {
        Id = id;
        Model = model;
        Capacity = capacity;
        Type = type;
    }

    protected Vehicle(string model, double capacity, VehicleType type)
    {
        Model = model;
        Capacity = capacity;
        Type = type;
    }

    public VehicleType GetVehicleType()
    {
        return Type;
    }
}
