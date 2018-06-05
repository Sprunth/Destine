using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace Destine
{
    public class World
    {
        public Func<World, bool> SimEndCondition = world => false;
        private readonly Clock clock;
        public uint CurrentTime => clock.CurrentTick;

        private bool _simDone = false;
        private Dictionary<TaskCompletionSource<bool>, uint> timeouts = new Dictionary<TaskCompletionSource<bool>, uint>();
        
        private List<Task> processes = new List<Task>();

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
            CheckTimeouts();

            if (SimEndCondition != null && SimEndCondition.Invoke(this))
            {
                _simDone = true;
                return false;
            }
            return true;
        }

        public void Run()
        {
            while (Tick())
            {
            }
        }

        public Task Process(Task proc, CancellationTokenSource cts = null)
        {
            // if no cancellation token passed in, just generate a dummy one and toss it
            if (cts == null)
                cts = new CancellationTokenSource();
            
            processes.Add(proc);
            return Task.WhenAny(proc, cts.Token.AsTask());
        }

        public Task Timeout(uint duration, CancellationTokenSource cts = null)
        {
            // if no cancellation token passed in, just generate a dummy one and toss it
            if (cts == null)
                cts = new CancellationTokenSource();

            var tcs = new TaskCompletionSource<bool>();
            timeouts[tcs] = CurrentTime + duration;
            //return tcs.Task;
            return Task.WhenAny(tcs.Task, cts.Token.AsTask()).ContinueWith(task => timeouts.Remove(tcs), TaskContinuationOptions.ExecuteSynchronously);  // todo: refactor/cleanup wiht CheckTimeouts
        }

        private void CheckTimeouts()
        {
            var toRemove = new List<TaskCompletionSource<bool>>();
            // todo: linq?
            foreach (var tcs in timeouts.Keys)
            {
                if (timeouts[tcs] <= CurrentTime)
                {
                    toRemove.Add(tcs);
                }
            }

            toRemove.ForEach(tcs => tcs.SetResult(true));
            toRemove.ForEach(tcs => timeouts.Remove(tcs));
        }
    }
}
