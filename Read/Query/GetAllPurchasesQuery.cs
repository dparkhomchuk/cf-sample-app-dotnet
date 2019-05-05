using CfSample.Read.Models;
using EventFlow.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CfSample.Read.Query
{
    public class GetAllPurchasesQuery: IQuery<IEnumerable<PurchaseViewModel>>
    {
    }
}
