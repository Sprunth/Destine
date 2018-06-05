using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Destine.Samples
{
    class BasicCar
    {
        public async Task Process(World world)
        {
            while (true)
            {
                Console.WriteLine($"Start parking at {world.CurrentTime}");
                uint parkingDuration = 5;
                await world.Timeout(parkingDuration);

                Console.WriteLine($"Start driving at {world.CurrentTime}");
                uint tripDuration = 2;
                await world.Timeout(tripDuration);
            }
        }
    }
}
