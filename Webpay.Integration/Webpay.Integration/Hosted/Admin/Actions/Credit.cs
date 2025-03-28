using System.Globalization;
using System.Xml;
using Webpay.Integration.Hosted.Admin.Response;
using Webpay.Integration.Order;
using Webpay.Integration.Order.Row.credit;

namespace Webpay.Integration.Hosted.Admin.Actions;

public class Credit : BasicRequest
{
    public readonly long AmountToCredit;
    public readonly long TransactionId;
    public readonly List<Delivery> Deliveries;

    public Credit(long transactionId, long amountToCredit, List<Delivery> deliveries, Guid? correlationId) : base(correlationId)
    {
        TransactionId = transactionId;
        AmountToCredit = amountToCredit;
        Deliveries = deliveries;
    }

    public string GetXmlForDeliveries()
    {
        var xml = "<deliveries>";
        Deliveries.ForEach(delivery =>
        {
            if(delivery != null)
            {
                xml += GetXmlForDelivery(delivery);
            }
            
        });
        xml += "</deliveries>";
        return xml;
    }

    public static CreditResponse Response(XmlDocument responseXml)
    {
        return new CreditResponse(responseXml);
    }

    private string GetXmlForDelivery(Delivery delivery)
    {
        return $"<delivery>" +
                    $"<id>{delivery.Id}</id> " +
                    $"<orderrows>{GetXmlForOrderRows(delivery)}</orderrows>" +
                    $"</delivery>";
    }

    private string GetXmlForOrderRows(Delivery delivery)
    {
        var xml = "";
        if (delivery.OrderRows.Count() > 0 || delivery.NewOrderRows.Count() > 0)
        {

            foreach (var row in delivery.OrderRows)
            {
                xml += GetXmlForOrderRow(row);

            }
            foreach (var row in delivery.NewOrderRows)
            {
                xml += GetXmlForOrderRow(row);

            }
        }
        return xml;
    }

    private string GetXmlForOrderRow(NewCreditOrderRowBuilder orderRow)
    {
        return $"<row>" +
                     $"<name>{orderRow.Name}</name>" +
                     $"<unitprice>{orderRow.UnitPrice}</unitprice>" +
                     $"<quantity>{orderRow.Quantity.ToString(CultureInfo.InvariantCulture)}</quantity>" +
                     $"<vatpercent>{orderRow.VatPercent.ToString(CultureInfo.InvariantCulture)}</vatpercent>" +
                     $"<discountpercent>{orderRow.DiscountPercent.ToString(CultureInfo.InvariantCulture)}</discountpercent>" +
                     $"<discountamount>{orderRow.DiscountAmount}</discountamount>" +
                     $"<unit>{orderRow.Unit}</unit>" +
                     $"<articlenumber>{orderRow.ArticleNumber}</articlenumber>" +
                     $"</row>";
    }

    private string GetXmlForOrderRow(CreditOrderRowBuilder orderRow)
    {
        var quantity = orderRow.Quantity.HasValue ? orderRow.Quantity.Value.ToString(CultureInfo.InvariantCulture) : orderRow.Quantity.ToString();
        return $"<row>" +
               $"<rowid>{orderRow.RowId}</rowid>" +
               $"<quantity>{quantity}</quantity>" +
               $"</row>";
    }

    public bool ValidateCreditRequest(out CreditResponse response)
    {
        response = null;
        var deliveryResponse = ValidateDeliveries();
        if (TransactionId < 0)
        {
            response = GetValidationErrorResponse("Invalid transactionId");
            return false;
        }
        else if (!deliveryResponse.Item1)
        {
            response = deliveryResponse.Item2;
            return false;
        }

        return true;
    }

    private (bool, CreditResponse?) ValidateDeliveries()
    {
        if (!Deliveries.Any() && AmountToCredit <= 0)
        {
            return (false, GetValidationErrorResponse("Invalid Credit Request, CreditAmount or deliveries with order rows are required"));
        }
        if (Deliveries.Any() && AmountToCredit > 0)
        {
            return (false, GetValidationErrorResponse("Invalid Credit Request, Credit by amount and by order rows is not allowed at the same time"));
        }
        if (Deliveries.Any() && AmountToCredit <= 0)
        {
            foreach (var delivery in Deliveries)
            {
                var (isValid, errorResponse) = ValidateDelivery(delivery);
                if (!isValid)
                    return (false, errorResponse);
            }
        }
        return (true, null);
    }

    private (bool, CreditResponse?) ValidateDelivery(Delivery delivery) =>
        delivery switch
        {
            null => (true, null),

            _ when AmountToCredit <= 0 && !delivery.NewOrderRows.Any() && !delivery.OrderRows.Any() =>
                (false, GetValidationErrorResponse("Invalid Credit Request, CreditAmount or order rows are required")),

            _ when AmountToCredit > 0 && delivery.NewOrderRows.Any() && delivery.OrderRows.Any() =>
                (false, GetValidationErrorResponse("Invalid Credit Request, Credit by amount and by order rows is not allowed at the same time")),

            _ when delivery.NewOrderRows.Any(x => string.IsNullOrEmpty(x.Name) || x.Quantity <= 0 || x.VatPercent < 0 || x.DiscountPercent < 0 || x.DiscountAmount < 0) =>
                (false, GetValidationErrorResponse($"Invalid NewOrderRow for delivery Id {delivery.Id}")),

            _ when delivery.OrderRows.Any(x => x.RowId <= 0 || x.Quantity <= 0) =>
                (false, GetValidationErrorResponse($"Invalid OrderRow for delivery Id {delivery.Id}")),

            _ => (true, null)
        };

    private CreditResponse GetValidationErrorResponse(string message)
    {
        var ValidationErrorResponseXml = new XmlDocument();
        ValidationErrorResponseXml.LoadXml($"<?xml version='1.0' encoding='UTF-8'?>" +
            $"<response>" +
            $"<statuscode>403</statuscode>" +
            $"<errorMessage>{message}</errorMessage>" +
            $"</response>");

        return Credit.Response(ValidationErrorResponseXml);
    }
}