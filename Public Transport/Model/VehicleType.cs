public enum VehicleType
{
    Bus,
    ElectricBus,
    Tram,
    Metro,
}

public enum EnergyType
{
    Fuel,
    Electric,
}

public static class VehicleTypeInfo
{
    public static Dictionary<VehicleType, EnergyType> energyType =
        new()
        {
            { VehicleType.Bus, EnergyType.Fuel },
            { VehicleType.ElectricBus, EnergyType.Electric },
            { VehicleType.Metro, EnergyType.Electric },
            { VehicleType.Tram, EnergyType.Electric },
        };
}
