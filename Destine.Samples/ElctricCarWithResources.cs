﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
            var world = new World {};
            var batteryCharingStationManager = new ResourceManager(2);
            for (uint i = 0; i < 4; i++)
            {
                var car = new ElctricCarWithResources(world, $"Car {i}", batteryCharingStationManager, i * 2, 5);
                world.Process(car.Process());
            }

            world.Run();
            Console.ReadLine();
        }

        public ElctricCarWithResources(World world, string name, ResourceManager batteryChargingStation,
            uint drivingTime, uint chargeDuration)
        {
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
            Console.WriteLine($"{_name} starting to charge at {_world.CurrentTime}");
            await _world.Timeout(_chargeDuration);
            Console.WriteLine($"{_name} leaving the bcs at {_world.CurrentTime}");
            batteryStation.Release();
        }

    }
}
