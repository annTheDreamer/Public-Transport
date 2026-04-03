using System;
using System.Collections.Generic;
using System.Text;

namespace Public_Transport.Interfaces
{
    public interface IElectric
    {
        double BatteryCapacity { get; }
        void Charge(double kwh);
    }
}
