using CfSample.Write.Events;
using CfSample.Write.Models;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CfSample.Read.Models
{
    using ProductTypeMapping;
    public class PurchaseViewModel : IReadModel, IAmReadModelFor<PurchaseAggregate,
            PurchaseAggregateIdentity,
            PurchaseCreatedEvent>
    {
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public string ProductType { get; set; }

        public void Apply(IReadModelContext context, IDomainEvent<PurchaseAggregate,
            PurchaseAggregateIdentity, PurchaseCreatedEvent> domainEvent) {
            var aggregateEvent = domainEvent.AggregateEvent;
            Date = aggregateEvent.PurchaseDate.Date;
            Price += aggregateEvent.Price;
            ProductType = ProductTypeProvider.GetProductType(aggregateEvent.ProductName);
        }
    }
}
