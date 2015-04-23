namespace Webpay.Integration.CSharp.Hosted.Admin.Actions
{
    public class QueryByCustomerRefNo : Query
    {
        public readonly string CustomerRefNo;

        public QueryByCustomerRefNo(string customerRefNo)
        {
            CustomerRefNo = customerRefNo;
        }
    }

    /// <summary>
    ///     Convenience class if you use ClientOrderNumber instead of CustomerRefNo in your code
    /// </summary>
    public class QueryByClientOrderNumber : QueryByCustomerRefNo
    {
        public QueryByClientOrderNumber(string clientOrderNumber) : base(clientOrderNumber)
        {
        }
    }
}