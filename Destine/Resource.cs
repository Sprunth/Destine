using System;
using System.Collections.Generic;
using System.Text;

namespace Destine
{
    public class Resource
    {
        private readonly ResourceManager _resourceManager;

        public Resource(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
            Console.WriteLine($"Resource created: {this.GetHashCode()}");
        }

        public void Release()
        {
            _resourceManager.ReturnResource();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() / 10000;
        }
    }
}
