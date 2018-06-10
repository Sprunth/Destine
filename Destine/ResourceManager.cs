using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Nito.AsyncEx;

namespace Destine
{
    public class ResourceManager
    {
        private readonly AsyncCollection<Resource> _buff;
        private readonly ConcurrentBag<Resource> _internalBag;

        private readonly HashSet<Resource> _debugMasterResourceSet;

        public ResourceManager(int resourceCount = 1)
        {
            _debugMasterResourceSet = new HashSet<Resource>();
            
            _internalBag = new ConcurrentBag<Resource>();
            _buff = new AsyncCollection<Resource>(_internalBag);
            for (var i = 0; i < resourceCount; i++)
            {
                QueueResource(new Resource(this));
            }
            Console.WriteLine($"ResourceManager constructed with internal bag {BagContents()}");
            
        }

        public async Task<Resource> Request()
        {
            return await _buff.TakeAsync();
        }

        public void ReturnResource(Resource r)
        {
            // ugly to mis-use the using/dispose pattern
            QueueResource(r);
            Console.WriteLine($"New Resource Queued {BagContents()}");
        }

        private void QueueResource(Resource r)
        {
            _buff.Add(r);
            _debugMasterResourceSet.Add(r);
        }

        public void PrintStatus()
        {
            Console.WriteLine(BagContents());
        }

        private string BagContents()
        {
            var ret = $"Bag | size: {_internalBag.Count} | this: {this.GetHashCode() / 1000}";
            return ret;
        }
    }
}
