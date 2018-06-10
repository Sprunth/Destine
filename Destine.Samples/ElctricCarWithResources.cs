using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Destine.Samples
{
    public class ElctricCarWithResources
    {
        private readonly World _world;
        private readonly string _name;
        private readonly ResourceManager _batteryChargingStation;
        private readonly uint _drivingTime;
        private readonly uint _chargeDuration;

        public static void Run()
        {
            Console.WriteLine("ElectricCarWithResources");
            var world = new World {SimEndCondition = world1 => world1.CurrentTime == 15};
            var batteryCharingStationManager = new ResourceManager(2);
            var cars = new List<ElctricCarWithResources>();
            foreach (var i in Enumerable.Range(0, 3))
            {
                var car = new ElctricCarWithResources(world, $"Car {i}", batteryCharingStationManager, (uint)i * 2, 5);
                cars.Add(car);
                world.Process(car.Process());

            }

            world.OnWorldTick = (time) =>
            {
                Console.WriteLine($"-- TICK {time}| ");
                batteryCharingStationManager.PrintStatus();
                //Console.WriteLine($"  - Bag has stuffs: {batteryCharingStationManager._resources.OutputAvailable()}");
            };
            world.Run();
            Console.ReadLine();
        }

        public ElctricCarWithResources(World world, string name, ResourceManager batteryChargingStation,
            uint drivingTime, uint chargeDuration)
        {
            Console.WriteLine($"Created {name}");
            _world = world;
            _name = name;
            _batteryChargingStation = batteryChargingStation;
            _drivingTime = drivingTime;
            _chargeDuration = chargeDuration;
        }

        public async Task Process()
        {
            await _world.Timeout(_drivingTime);
            Console.WriteLine($"{_name} arriving at {_world.CurrentTime}");

            var batteryStation = await _batteryChargingStation.Request();
            Console.WriteLine($"{_name} starting to charge at Time {_world.CurrentTime}, stations {batteryStation.GetHashCode()}");
            await _world.Timeout(_chargeDuration);

            Console.WriteLine($"{_name} leaving the bcs at {_world.CurrentTime}");
            batteryStation.Release();
        }
    }
}
