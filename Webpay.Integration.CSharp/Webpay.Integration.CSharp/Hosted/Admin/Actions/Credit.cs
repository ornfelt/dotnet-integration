using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Webpay.Integration.CSharp.Hosted.Admin.Response;
using Webpay.Integration.CSharp.Order;
using Webpay.Integration.CSharp.Order.Row;
using Webpay.Integration.CSharp.Order.Row.credit;

namespace Webpay.Integration.CSharp.Hosted.Admin.Actions
{
    public class Credit : BasicRequest
    {
        public readonly long AmountToCredit;
        public readonly long TransactionId;
        public readonly List<NewCreditOrderRowBuilder> NewOrderRows;
        public readonly List<CreditOrderRowBuilder> OrderRows;

        public Credit(long transactionId, long amountToCredit, List<NewCreditOrderRowBuilder> newOrderRows, List<CreditOrderRowBuilder> orderRows, Guid? correlationId) : base(correlationId)
        {
            TransactionId = transactionId;
            AmountToCredit = amountToCredit;
            NewOrderRows = newOrderRows;
            OrderRows = orderRows;
        }
        public string GetXmlForOrderRows()
        {
            if(OrderRows.Count()>0 ||NewOrderRows.Count()>0)
            {
                var xml = "<orderrows>";
                foreach (var row in OrderRows)
                {
                    xml += GetXmlForOrderRow(row);

                }
                foreach (var row in NewOrderRows)
                {
                    xml += GetXmlForOrderRow(row);

                }
                xml += "</orderrows>";
                return xml;
            }
            return "";
        }
        public static CreditResponse Response(XmlDocument responseXml)
        {
            return new CreditResponse(responseXml);
        }

        private string GetXmlForOrderRow(NewCreditOrderRowBuilder orderRow)
        {
            return $"<row>" +
                         $"<name>{orderRow.Name}</name> " +
                         $"<unitprice>{orderRow.UnitPrice}</unitprice> " +
                         $"<quantity>{orderRow.Quantity}</quantity> " +
                         $"<vatpercent>{orderRow.VatPercent}</vatpercent> " +
                         $"<discountpercent>{orderRow.DiscountPercent}</discountpercent> " +
                         $"<discountamount>{orderRow.DiscountAmount}</discountamount> " +
                         $"<unit>{orderRow.Unit}</unit> " +
                         $"<articlenumber>{orderRow.ArticleNumber}</articlenumber>" +
                         $"</row>";
        }
        private string GetXmlForOrderRow(CreditOrderRowBuilder orderRow)
        {
                return $"<row>" +
                       $"<rowId>{orderRow.RowId}</rowId>" +
                       $"<quantity>{orderRow.Quantity}</quantity>" +
                       $"</row>";         
        }
        public bool ValidateCreditRequest(out CreditResponse response)
        {
            response = null;
            if ( TransactionId < 0)
            {
                response =GetValidationErrorResponse("Invalid transactionId");
                return false;
            }
            else if (AmountToCredit < 0)
            {
                response = GetValidationErrorResponse("Invalid AmountToCredit");
                return false;
            }
            else if (NewOrderRows.Count()>0 && NewOrderRows.Any(x=> 
                    string.IsNullOrEmpty(x.Name)
                    || (x.UnitPrice <= 0)
                    || (x.Quantity <= 0)
                    || (x.VatPercent < 0)
                    || (x.DiscountPercent < 0)
                    || (x.DiscountAmount < 0)
                    || string.IsNullOrEmpty(x.Unit)
                    || string.IsNullOrEmpty(x.ArticleNumber)
                ))
            {
                response = GetValidationErrorResponse("Invalid New OrderRow");
                return false;
            }
            else if (OrderRows.Count()>0 && OrderRows.Any(x =>  
                       (x.RowId <= 0)
                    || (x.Quantity <= 0)))
            {
                response = GetValidationErrorResponse("Invalid OrderRow");
                return false;
            }
           return true;
        }

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
}