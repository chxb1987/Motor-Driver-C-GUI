using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperButton.StatusGroup
{
    class StatusesModule
    {
        public static readonly List<Example> Statuses = new List<Example>();

        public static void InitSatuses()
        {
            var Status = new Example()
            {
                ExampleDescription = null,
                ExampleDisplayName = "Over Temperature",
                Group = StatusGroups.ControlStatus
            };

            Statuses.Add(Status);

            Status = new Example()
            {
                ExampleDescription = null,
                ExampleDisplayName = "UnderTemperature",
                Group = StatusGroups.DriveStatus
            };

            Statuses.Add(Status);
        }

     



    }
}
