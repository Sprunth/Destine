using System;

namespace Destine.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var world = new World {SimEndCondition = w => w.CurrentTime == 5};
            world.Run();

            Console.WriteLine($"World Ended on tick {world.CurrentTime}");
            Console.ReadLine();

            Console.WriteLine("BasicCar");

            var world2 = new World {SimEndCondition = w => w.CurrentTime == 15};
            var basicCar = new BasicCar();
            world2.Process(basicCar.Process(world2));
            world2.Run();

            Console.WriteLine("Done");
            Console.ReadLine();

        }
    }
}
