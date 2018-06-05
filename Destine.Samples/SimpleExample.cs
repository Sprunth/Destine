using System;
using System.Collections.Generic;
using System.Text;

namespace Destine.Samples
{
    class SimpleExample
    {
        public static void Run()
        {
            var world = new World { SimEndCondition = w => w.CurrentTime == 5 };
            world.Run();

            Console.WriteLine($"World Ended on tick {world.CurrentTime}");
            Console.ReadLine();
        }
    }
}
