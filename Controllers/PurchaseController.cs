using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CfSample.DTO;
using CfSample.Write.Commands;
using EventFlow;
using EventFlow.Queries;
using Microsoft.AspNetCore.Mvc;
using CfSample.Write.Models;
using CfSample.Read.Models;
using EventFlow.ReadStores.InMemory.Queries;
using CfSample.Read.Query;

namespace CfSample.Controllers
{
    public class PurchaseController : Controller
    {

        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _queryProcessor;

        public PurchaseController(ICommandBus commandBus, IQueryProcessor queryProcessor) {
            _commandBus = commandBus;
            _queryProcessor = queryProcessor;
        }

        [HttpGet("/purchase")]
        public async Task<IActionResult> CreatePurchase(CancellationToken cancellationToken) {
            var purchasesList = await _queryProcessor.ProcessAsync(new GetAllPurchasesQuery(), cancellationToken);
            return View(purchasesList);
        }

        [HttpPost("/purchase")]
        public async Task<IActionResult> CreatePurchase(CreatePurchaseDTO createPurchaseModel, CancellationToken cancellationToken)
        {
            var purchaseIdIdentity = PurchaseAggregateIdentity.New;
            var createPurchaseCommand = new CreatePurchaseCommand(DateTime.Now,
                createPurchaseModel.Price,
                createPurchaseModel.ProductName,
                purchaseIdIdentity);
            await _commandBus.PublishAsync(createPurchaseCommand, cancellationToken).ConfigureAwait(false);
            return RedirectToAction("CreatePurchase", "Purchase");
        }
    }
}