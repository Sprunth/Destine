using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace Destine
{
    public class ResourceManager
    {
        private readonly AsyncProducerConsumerQueue<Resource> resources;

        public ResourceManager(int resourceCount = 1)
        {
            resources = new AsyncProducerConsumerQueue<Resource>();
            for (var i = 0; i < resourceCount; i++)
            {
                QueueResource();
            }
        }

        public async Task<Resource> Request()
        {
            return await resources.DequeueAsync();
        }

        public void ReturnResource()
        {
            // ugly to requeue the resource and mis-use the using/dispose pattern
            // so we just queue a new resource
            QueueResource();
        }

        private void QueueResource()
        {
            resources.Enqueue(new Resource(this));
        }
    }
}
