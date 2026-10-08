using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using prva_web_aplikacija.Service.Common;

namespace prva_web_aplikacija.Service
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
