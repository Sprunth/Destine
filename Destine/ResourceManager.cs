using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace Destine
{
    public class ResourceManager
    {
        public readonly AsyncCollection<Resource> _resources;
        private readonly ConcurrentQueue<Resource> _internalBag;

        public ResourceManager(int resourceCount = 1)
        {
            _internalBag= new ConcurrentQueue<Resource>();
            _resources = new AsyncCollection<Resource>(_internalBag);
            for (var i = 0; i < resourceCount; i++)
            {
                QueueResource();
            }
            Console.WriteLine($"ResourceManager constructed with internal bag {BagContents()}");
        }

        public async Task<Resource> Request()
        {
            Console.WriteLine($"Someone requested a resource. {BagContents()}");
            //await _resources.OutputAvailableAsync();
            //Console.WriteLine($"Resource is avaliable to take. {BagContents()}");
            return await _resources.TakeAsync();
        }

        public void ReturnResource()
        {
            // ugly to requeue the resource and mis-use the using/dispose pattern
            // so we just queue a new resource
            QueueResource();
            Console.WriteLine($"New Resource Queued {BagContents()}");
        }

        private void QueueResource()
        {
            _resources.Add(new Resource(this));
        }

        public void PrintStatus()
        {
            Console.WriteLine(BagContents());
        }

        private string BagContents()
        {
            var ret = $"Bag | size: {_internalBag.Count} |";
            foreach (var resource in _internalBag)
            {
                ret += $"{resource.GetHashCode()},";
            }
            
            return ret;
        }
    }
}
