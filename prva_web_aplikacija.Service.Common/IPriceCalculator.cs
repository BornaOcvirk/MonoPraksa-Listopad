using System;
using System.Collections.Generic;
using System.Text;

namespace prva_web_aplikacija.Service.Common
{
    public interface IPriceCalculator
    {
        Guid InstanceId { get; }
        decimal CalculateFinalPrice(decimal price);
    }
}
