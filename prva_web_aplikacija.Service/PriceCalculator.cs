using System;
using System.Collections.Generic;
using System.Text;
using prva_web_aplikacija.Common;
using prva_web_aplikacija.Service.Common;

namespace prva_web_aplikacija.Service
{
    public class PriceCalculator : IPriceCalculator
    {
        public Guid InstanceId { get; } = Guid.NewGuid();

        public decimal CalculateFinalPrice(decimal price)
        {
            return Math.Round(price * (1 + ShopConstants.Pdv), 2, MidpointRounding.AwayFromZero);
        }
    }
}