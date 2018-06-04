using System;
using System.Collections.Generic;
using System.Text;

namespace Destine
{
    public class Clock
    {
        private readonly uint _tickIncrementSize;
        public uint CurrentTick { get; private set; }

        public Clock(uint startingTick = 0, uint tickIncrementSize = 1)
        {
            _tickIncrementSize = tickIncrementSize;
            CurrentTick = startingTick;
        }

        public void Tick()
        {
            CurrentTick += _tickIncrementSize;
        }
    }
}
