using System;
using System.Collections.Generic;
using System.Text;
using prva_web_aplikacija.Service.Common;
using prva_web_aplikacija.Common;

namespace prva_web_aplikacija.Service
{
    public class PriceCalculator : IPriceCalculator
    {
        public Guid InstanceId { get; } = Guid.NewGuid();

        public float CalculateFinalPrice(float price)
        {
            return MathF.Round(price * (1 + ShopConstants.Pdv), 2);
        }
    }
}
