using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace Destine
{
    public class ResourceManager
    {
        private readonly AsyncCollection<Resource> resources;

        public ResourceManager(int resourceCount = 1)
        {
            resources = new AsyncCollection<Resource>();
            for (var i = 0; i < resourceCount; i++)
            {
                QueueResource();
            }
        }

        public async Task<Resource> Request()
        {
            Console.WriteLine("Someone requested a resource");
            await resources.OutputAvailableAsync();
            Console.WriteLine("Resource is avaliable to take");
            return await resources.TakeAsync();
        }

        public void ReturnResource()
        {
            // ugly to requeue the resource and mis-use the using/dispose pattern
            // so we just queue a new resource
            QueueResource();
            Console.WriteLine("New Resource Queued");
        }

        private void QueueResource()
        {
            resources.Add(new Resource(this));
        }
    }
}
