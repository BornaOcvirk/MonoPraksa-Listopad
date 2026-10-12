using System;
using System.Collections.Generic;
using System.Text;

namespace PrvaWebAplikacija.Service.Common
{
    public interface IPriceCalculator
    {
        Guid InstanceId { get; }
        decimal CalculateFinalPrice(decimal price);
    }
}
