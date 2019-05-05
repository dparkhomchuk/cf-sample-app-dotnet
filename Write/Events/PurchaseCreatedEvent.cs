using CfSample.Write.Models;
using EventFlow.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CfSample.Write.Events
{
    public class PurchaseCreatedEvent : AggregateEvent<PurchaseAggregate,
        PurchaseAggregateIdentity>
    {
        public DateTime PurchaseDate { get; }
        public decimal Price { get; }
        public string ProductName { get; }

        public PurchaseCreatedEvent(DateTime purchaseDate, decimal price, string productName)
        {
            PurchaseDate = purchaseDate;
            Price = price;
            ProductName = productName;
        }

    }
}
