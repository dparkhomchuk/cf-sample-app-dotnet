namespace CfSample.Write.Models
{
    using EventFlow.Core;

    #region Class: PurchaseAggregateIdentity
    public class PurchaseAggregateIdentity : Identity<PurchaseAggregateIdentity>
    {
        public PurchaseAggregateIdentity(string value) : base(value) { }
    } 

    #endregion

}
