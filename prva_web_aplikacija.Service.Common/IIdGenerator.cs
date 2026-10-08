using System;
using System.Collections.Generic;
using System.Text;

namespace prva_web_aplikacija.Service.Common
{
    public interface IIdGenerator
    {
        Guid InstanceId { get; }
        Guid NextId();
    }
}
