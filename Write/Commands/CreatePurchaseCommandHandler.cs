using CfSample.Write.Models;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CfSample.Write.Commands
{
    public class CreatePurchaseCommandHandler : CommandHandler<PurchaseAggregate,
        PurchaseAggregateIdentity, CreatePurchaseCommand>
    {

        public override Task ExecuteAsync(PurchaseAggregate aggregate,
                CreatePurchaseCommand command,
                CancellationToken cancellationToken) {
            aggregate.SetPurchaseInfo(command.Price, command.ProductName);
            return Task.FromResult(0);
        }
    }
}
