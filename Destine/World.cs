using System;

namespace Destine
{
    public class World
    {
        public Func<World, bool> SimEndCondition = world => false;
        private readonly Clock clock;
        public uint CurrentTime => clock.CurrentTick;

        private bool _simDone = false;

        public World()
        {
            clock = new Clock();

        }

        /// <summary>
        /// Takes a step in the world time, if possible.
        /// </summary>
        /// <returns>True if world is still alive (end condition not satisfied)</returns>
        public bool Tick()
        {
            if (_simDone)
                return false;

            clock.Tick();

            if (SimEndCondition != null && SimEndCondition.Invoke(this))
            {
                _simDone = true;
                return false;
            }
            return true;
        }
    }
}
