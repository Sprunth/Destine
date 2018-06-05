using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Destine.Samples
{
    class BasicCar
    {
        public static void Run()
        {
            var world2 = new World { SimEndCondition = w => w.CurrentTime == 15 };
            var basicCar = new BasicCar();
            world2.Process(basicCar.Process(world2));
            world2.Run();
            Console.WriteLine("Done");
            Console.ReadLine();
        }

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
