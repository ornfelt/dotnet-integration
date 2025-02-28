﻿using NUnit.Framework;
using Webpay.Integration.CSharp.Config;
using Webpay.Integration.CSharp.Util.Constant;
using Webpay.Integration.CSharp.Util.Testing;
using Webpay.Integration.CSharp.WebpayWS;

namespace Webpay.Integration.CSharp.IntegrationTest.Webservice.Payment
{
    [TestFixture]
    public class DoPaymentPlanTest
    {
        [Test]
        public void TestPaymentPlanRequestReturnsAcceptedResult()
        {
            GetPaymentPlanParamsEuResponse paymentPlanParam = WebpayConnection.GetPaymentPlanParams(SveaConfig.GetDefaultConfig())
                                                                              .SetCountryCode(TestingTool.DefaultTestCountryCode)
                                                                              .DoRequest();
            long code = paymentPlanParam.CampaignCodes[0].CampaignCode;

            CreateOrderEuResponse response = WebpayConnection.CreateOrder(SveaConfig.GetDefaultConfig())
                                                             .AddOrderRow(TestingTool.CreatePaymentPlanOrderRow())
                                                             .AddCustomerDetails(TestingTool.CreateIndividualCustomer())
                                                             .SetCountryCode(TestingTool.DefaultTestCountryCode)
                                                             .SetCustomerReference(TestingTool.DefaultTestCustomerReferenceNumber)
                                                             .SetClientOrderNumber(TestingTool.DefaultTestClientOrderNumber)
                                                             .SetOrderDate(TestingTool.DefaultTestDate)
                                                             .SetCurrency(TestingTool.DefaultTestCurrency)
                                                             .SetCountryCode(TestingTool.DefaultTestCountryCode)
                                                             .UsePaymentPlanPayment(code)
                                                             .DoRequest();

            Assert.That(response.Accepted, Is.True);
        }

        [Test]
        public void TestDeliverPaymentPlanOrderResult()
        {
            long orderId = createPaymentPlanAndReturnOrderId();

            DeliverOrderEuResponse response = WebpayConnection.DeliverOrder(SveaConfig.GetDefaultConfig())
                                                              .AddOrderRow(TestingTool.CreatePaymentPlanOrderRow())
                                                              .SetOrderId(orderId)
                                                              .SetNumberOfCreditDays(1)
                                                              .SetInvoiceDistributionType(DistributionType.POST)
                                                              .SetCountryCode(TestingTool.DefaultTestCountryCode)
                                                              .DeliverPaymentPlanOrder()
                                                              .DoRequest();

            Assert.That(response.Accepted, Is.True);
        }

        private long createPaymentPlanAndReturnOrderId()
        {
            GetPaymentPlanParamsEuResponse paymentPlanParam = WebpayConnection.GetPaymentPlanParams(SveaConfig.GetDefaultConfig())
                                                                              .SetCountryCode(TestingTool.DefaultTestCountryCode)
                                                                              .DoRequest();
            long code = paymentPlanParam.CampaignCodes[0].CampaignCode;

            CreateOrderEuResponse response = WebpayConnection.CreateOrder(SveaConfig.GetDefaultConfig())
                                                             .AddOrderRow(TestingTool.CreatePaymentPlanOrderRow())
                                                             .AddCustomerDetails(TestingTool.CreateIndividualCustomer())
                                                             .SetCountryCode(TestingTool.DefaultTestCountryCode)
                                                             .SetCustomerReference(TestingTool.DefaultTestCustomerReferenceNumber)
                                                             .SetClientOrderNumber(TestingTool.DefaultTestClientOrderNumber)
                                                             .SetOrderDate(TestingTool.DefaultTestDate)
                                                             .SetCurrency(TestingTool.DefaultTestCurrency)
                                                             .UsePaymentPlanPayment(code)
                                                             .DoRequest();

            return response.CreateOrderResult.SveaOrderId;
        }
    }
}