using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using prva_web_aplikacija.Service.Common;

namespace prva_web_aplikacija.Service
{
    public class IdGenerator : IIdGenerator
    {
        private int _current = 5; 
        public Guid InstanceId { get; } = Guid.NewGuid();
        public Guid GenerateId => InstanceId;
        public int NextId()
        {
            return Interlocked.Increment(ref _current);
        }
    }
}
