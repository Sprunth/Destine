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
        /// <summary>
        /// Contains all the Timeout events that processes have configured. Key'ed to cancellation sources (set true on timeout), valued to when (clock) the timeout should get triggered
        /// </summary>
        private readonly Dictionary<TaskCompletionSource<bool>, uint> timeouts = new Dictionary<TaskCompletionSource<bool>, uint>();
        
        private readonly List<Task> processes = new List<Task>();

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
            Console.WriteLine($" -- Now on world tick {CurrentTime} --");
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
            CheckTimeouts();
            while (Tick())
            {
                if (processes.TrueForAll(task => task.Status == TaskStatus.RanToCompletion))
                {
                    Console.WriteLine("No more processes, ending world");
                    _simDone = true;
                    
                }
            }
        }

        public Task Process(Task proc, CancellationTokenSource cts = null)
        {
            // if no cancellation token passed in, just generate a dummy one and toss it
            if (cts == null)
                cts = new CancellationTokenSource();
            var ctts = new CancellationTokenTaskSource<bool>(cts.Token);
            processes.Add(proc);
            return Task.WhenAny(proc, ctts.Task);
        }

        public Task Timeout(uint duration, CancellationTokenSource cts = null)
        {
            // if no cancellation token passed in, just generate a dummy one and toss it
            if (cts == null)
                cts = new CancellationTokenSource();

            var tcs = new TaskCompletionSource<bool>();
            timeouts[tcs] = CurrentTime + duration;
            var ctts = new CancellationTokenTaskSource<bool>(cts.Token);
            return Task.WhenAny(tcs.Task, ctts.Task).ContinueWith(task => timeouts.Remove(tcs), TaskContinuationOptions.ExecuteSynchronously);  // todo: refactor/cleanup with CheckTimeouts
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
