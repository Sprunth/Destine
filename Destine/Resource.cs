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
        }

        public void Release()
        {
            _resourceManager.ReturnResource();
        }
    }
}
