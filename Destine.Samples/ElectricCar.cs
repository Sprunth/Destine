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
        public CancellationTokenSource CancellationToken { get; private set; }

        public ElectricCar(World world)
        {
            _world = world;
            Action = _world.Process(Process());
        }

        public async Task Process()
        {
            while (true)
            {
                Console.WriteLine($"Start parking and charging at {_world.CurrentTime}");
                uint chargeDuration = 5;

                await _world.Process(Charge(chargeDuration));
                
                Console.WriteLine($"Start driving at {_world.CurrentTime}");
                uint tripDuration = 2;
                await _world.Timeout(tripDuration);
            }
        }

        private async Task Charge(uint duration)
        {
            CancellationToken = new CancellationTokenSource();
            await _world.Timeout(duration, CancellationToken);
            if (CancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Was interrupted. Hope the battery is full enough...");
            }
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
            Console.WriteLine("Attempting to cancel");
            _car.CancellationToken.Cancel();
        }
    }
}
