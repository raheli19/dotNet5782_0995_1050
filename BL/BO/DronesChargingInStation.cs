using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class DronesChargingInStation
    {
        public DroneCharging droneCharging = new DroneCharging();
        public override string ToString()
        {
            String result = "";
            //StringBuilder builderDroneInCharges = new StringBuilder();
            //foreach (var elementInCharge in DroneCharging)
            //    builderDroneInCharges.Append(elementInCharge).Append(", ");
            //result += $"Drones charging in this station: {builderDroneInCharges.ToString()}";
            result += droneCharging.ToString();
            return result;
        }
    }
}
