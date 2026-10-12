using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using PrvaWebAplikacija.Service.Common;

namespace PrvaWebAplikacija.Service
{
    public class IdGenerator : IIdGenerator
    {
        public Guid InstanceId { get; } = Guid.NewGuid();

        public Guid NextId()
        {
            return Guid.CreateVersion7();
        }
    }
}
