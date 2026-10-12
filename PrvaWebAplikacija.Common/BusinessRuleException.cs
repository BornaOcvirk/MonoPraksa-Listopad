using System;
using System.Collections.Generic;
using System.Text;

namespace PrvaWebAplikacija.Common
{
    public class BusinessRuleException : Exception
    {
        public BusinessRuleException(string message) : base(message)
        {
        }
    }
    public class Not_FoundException : Exception
    {
        public Not_FoundException(string message) : base(message)
        {
        }
    }
    public class OutOfStockException : Exception
    {
        public OutOfStockException(string message) : base(message)
        {
        }
    }
    public class InvalidOrderException : Exception
    {
        public InvalidOrderException(string message) : base(message)
        {
        }
    }
    public class OrderLimitException : Exception
    {
        public OrderLimitException(string message) : base(message)
        {
        }
    }
    public class ItemExistsException : Exception
    {
        public ItemExistsException(string message) : base(message)
        {
        }
    }
}
