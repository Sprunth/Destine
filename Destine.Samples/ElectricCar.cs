using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Destine.Samples
{
    class ElectricCar
    {
        public static void Run()
        {
            Console.WriteLine("ElectricCar");
            var world = new World {SimEndCondition = world1 => world1.CurrentTime == 15};
            var electricCar = new ElectricCar(world);
            var driver = new Driver(world, electricCar);
            world.Run();
            Console.ReadLine();
        }

        private readonly World _world;
        public Task Action { get; }
        public CancellationTokenSource CancellationToken { get; }

        public ElectricCar(World world)
        {
            _world = world;
            CancellationToken = new CancellationTokenSource();
            Action = _world.Process(Process());
        }

        public async Task Process()
        {
            while (true)
            {
                Console.WriteLine($"Start parking and charging at {_world.CurrentTime}");
                uint chargeDuration = 5;
                try
                {
                    await _world.Process(Charge(chargeDuration));
                }
                catch (OperationCanceledException e)
                {
                    Console.WriteLine("Was interrupted. Hope the battery is full enough...");
                }

                Console.WriteLine($"Start driving at {_world.CurrentTime}");
                uint tripDuration = 2;
                await _world.Timeout(tripDuration);
            }
        }

        private async Task Charge(uint duration)
        {
            await _world.Timeout(duration);
        }
    }

    class Driver
    {
        private readonly World _world;
        private readonly ElectricCar _car;

        public Driver(World world, ElectricCar car)
        {
            _world = world;
            _car = car;
            _world.Process(Process());
        }

        public async Task Process()
        {
            await _world.Timeout(3);
            _car.CancellationToken.Cancel();
        }
    }
}
