using System;
using System.Collections.Generic;
using System.Text;

namespace PrvaWebAplikacija.Service.Common
{
    public interface IIdGenerator
    {
        Guid InstanceId { get; }
        Guid NextId();
    }
}
