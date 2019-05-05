using CfSample.Read.Models;
using EventFlow.Elasticsearch.ReadStores;
using EventFlow.Queries;
using EventFlow.ReadStores;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CfSample.Read.Query
{
    public class GetAllPurchasesQueryHandler : IQueryHandler<GetAllPurchasesQuery, IEnumerable<PurchaseViewModel>>
    {

        private readonly IElasticClient _elasticClient;
        private readonly IReadModelDescriptionProvider _readModelDescriptionProvider;

        public GetAllPurchasesQueryHandler(IElasticClient elasticClient, IReadModelDescriptionProvider readModelDescriptionProvider)
        {
            _elasticClient = elasticClient;
            _readModelDescriptionProvider = readModelDescriptionProvider;
        }

        public async Task<IEnumerable<PurchaseViewModel>> ExecuteQueryAsync(GetAllPurchasesQuery query, CancellationToken cancellationToken) {
            var readModelDescription = _readModelDescriptionProvider.GetReadModelDescription<PurchaseViewModel>();
            var searchResponse = await _elasticClient.SearchAsync<PurchaseViewModel>(s=>s.RequestConfiguration(c => c
                            .AllowedStatusCodes((int)HttpStatusCode.NotFound))
                            .Index(readModelDescription.IndexName.Value)
                            .Query(q=>q.MatchAll())
                            .Size(1000), cancellationToken);
            var result = new List<PurchaseViewModel>();
            foreach(var item in searchResponse.Documents) {
                result.Add(item);
            }
            return result;
        }
    }
}
