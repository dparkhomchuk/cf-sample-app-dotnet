using CfSample.Write.Events;
using EventFlow.Aggregates;
using EventFlow.Core;
using EventFlow.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CfSample.Write.Models
{
    #region Class: PurchaseAggregate
    public class PurchaseAggregate : AggregateRoot<PurchaseAggregate, PurchaseAggregateIdentity>
    {
        public PurchaseAggregate(PurchaseAggregateIdentity id) : base(id) {
            PurchaseDate = DateTime.Now;
        }
        public DateTime PurchaseDate { get; }
        public decimal Price { get; private set; }
        public string ProductName { get; private set; }

        public void SetPurchaseInfo(decimal price, string productName)         {
            Emit(new PurchaseCreatedEvent(PurchaseDate, price, productName));
        }
        public void Apply(PurchaseCreatedEvent even){
        }

    } 
    #endregion
}
