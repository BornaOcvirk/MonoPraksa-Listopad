using System;
using System.Collections.Generic;
using System.Text;
using PrvaWebAplikacija.Common;
using PrvaWebAplikacija.Service.Common;

namespace PrvaWebAplikacija.Service
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