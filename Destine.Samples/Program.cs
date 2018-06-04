using System;

namespace Destine.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var world = new World {SimEndCondition = w => w.CurrentTime == 5};
            while (world.Tick())
            {
                Console.WriteLine($"World now on tick {world.CurrentTime}");

            }

            Console.WriteLine($"World Ended on tick {world.CurrentTime}");
            Console.ReadLine();
        }
    }
}
