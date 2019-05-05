using CfSample.Write.Models;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using System;

namespace CfSample.Write.Commands
{
    public class CreatePurchaseCommand: Command<PurchaseAggregate, PurchaseAggregateIdentity, IExecutionResult>
    {
        public CreatePurchaseCommand(DateTime purchaseDate,
            decimal price,
            string productName, PurchaseAggregateIdentity identity) : base(identity) {
            PurchaseDate = purchaseDate;
            Price = price;
            ProductName = productName;
        }

        public DateTime PurchaseDate { get; }
        public decimal Price { get; }
        public string ProductName { get; }
    }
}
