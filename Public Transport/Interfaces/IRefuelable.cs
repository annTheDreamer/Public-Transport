using System;
using System.Collections.Generic;
using System.Text;

namespace Public_Transport.Interfaces
{
    public interface IRefuelable
    {
        double FuelConsumption { get; }
        void Refuel(double liters);
    }
}
