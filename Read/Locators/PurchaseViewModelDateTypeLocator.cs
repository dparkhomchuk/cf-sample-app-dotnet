using CfSample.ProductTypeMapping;
using CfSample.Write.Events;
using CfSample.Write.Models;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CfSample.Read.Locators
{
    public class PurchaseViewModelDateTypeLocator : IReadModelLocator
    {
        public PurchaseViewModelDateTypeLocator() {}

        public IEnumerable<string> GetReadModelIds(IDomainEvent domainEvent) {
            var purchaseCreated = domainEvent as IDomainEvent<PurchaseAggregate,
                PurchaseAggregateIdentity, PurchaseCreatedEvent>;
            if (purchaseCreated == null) {
                yield break;
            }
            var aggregatedEvent = purchaseCreated.AggregateEvent;
            var productType = ProductTypeProvider.GetProductType(aggregatedEvent.ProductName);
            var purchaseDate = aggregatedEvent.PurchaseDate.Date;
            yield return $"{purchaseDate.ToString()}_{productType}";
        }
    }
}
