using System;
using System.Collections.Generic;
using System.Text;

namespace Destine
{
    public class Clock
    {
        public uint TickIncrementSize { get; }
        public uint CurrentTick { get; private set; }

        public Clock(uint startingTick = 0, uint tickIncrementSize = 1)
        {
            TickIncrementSize = tickIncrementSize;
            CurrentTick = startingTick;
        }

        public void Tick()
        {
            CurrentTick += TickIncrementSize;
        }
    }
}
