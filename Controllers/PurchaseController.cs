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
using Microsoft.Extensions.Logging;

namespace CfSample.Controllers
{
    public class PurchaseController : Controller
    {

        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _queryProcessor;
        private readonly ILogger _logger;

        public PurchaseController(ICommandBus commandBus, IQueryProcessor queryProcessor, ILoggerFactory loggerFactory) {
            _commandBus = commandBus;
            _queryProcessor = queryProcessor;
            _logger = loggerFactory.CreateLogger<PurchaseController>();
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
            _logger.LogInformation("Purchase creation process start commited");
            return RedirectToAction("CreatePurchase", "Purchase");
        }
    }
}