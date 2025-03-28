using System.Xml;
using Webpay.Integration.Hosted.Admin.Response;

namespace Webpay.Integration.Hosted.Admin.Actions;

public class LowerAmount:BasicRequest
{
    public readonly long AmountToLower;
    public readonly long TransactionId;

    public LowerAmount(long transactionId, long amountToLower, Guid? correlationId) : base(correlationId)
    {
        TransactionId = transactionId;
        AmountToLower = amountToLower;
    }

    public static LowerAmountResponse Response(XmlDocument responseXml)
    {
        return new LowerAmountResponse(responseXml);
    }
}