﻿using Webpay.Integration.Config;
using Webpay.Integration.Hosted.Admin;
using Webpay.Integration.Util.Constant;
using Webpay.Integration.Util.Testing;
using WebpayWS;

namespace Webpay.Integration.Test;

[TestFixture]
public class WebpayConnectionUnitTest
{
    // WebPay Request and Response Classes
    //
    // createOrder - useInvoicePayment => request class
    [Test] static public void createOrder_useInvoicePayment_returns_InvoicePayment_request()
    {
        new Webpay.Integration.Test.Webservice.Payment.InvoicePaymentTest()
            .TestInvoiceRequestObjectForCustomerIdentityIndividualFromSe();
    }
    // createOrder - useInvoicePayment - useInvoicePayment - doRequest => response class
    
    // createOrder - usePaymentPlanPayment 
    // createOrder - usePaymentPlanPayment - doRequest

    // createOrder - usePaymentMethod

    // createOrder - usePaymentMethod - getPaymentForm
    // createOrder - usePaymentMethod - getPaymentUrl
    // createOrder - usePaymentMethod - doRecur

    // createOrder - usePayPage
    // createOrder - usePayPageCardOnly
    // createOrder - usePayPageDirectBankOnly

    // validation - createOrder - addOrderRow required
    // validation - createOrder - addFee optional
    // validation - createOrder - addDiscount optional

    // validation - createOrder - customerDetails - invoice - individual - SE
    [Test] static public void validation_createOrder_customerDetails_invoice_individual_SE()
    {
        new Webpay.Integration.Test.Webservice.Payment.InvoicePaymentTest()
            .TestInvoiceRequestObjectForCustomerIdentityIndividualFromSe();
    }
    // validation - createOrder - customerDetails - invoice - company - SE
    [Test] static public void validation_createOrder_customerDetails_invoice_company_SE()
    {
        new Webpay.Integration.Test.Webservice.Payment.InvoicePaymentTest()
            .TestInvoiceRequestObjectForCustomerIdentityCompanyFromSe();
    }        
    // validation - createOrder - customerDetails - invoice - individual - NO
    // validation - createOrder - customerDetails - invoice - company - NO
    // validation - createOrder - customerDetails - invoice - individual - DK
    // validation - createOrder - customerDetails - invoice - company - DK
    // validation - createOrder - customerDetails - invoice - individual - FI
    // validation - createOrder - customerDetails - invoice - company - FI
    // validation - createOrder - customerDetails - invoice - individual - DE
    // validation - createOrder - customerDetails - invoice - company - DE
    // validation - createOrder - customerDetails - invoice - individual - NL
    [Test] public static void validation_createOrder_customerDetails_invoice_individual_NL()
    {
        new Webpay.Integration.Test.Webservice.Payment.InvoicePaymentTest()
            .TestInvoiceRequestObjectForCustomerIdentityIndividualFromNl();
    }
    // validation - createOrder - customerDetails - invoice - company - NL
    [Test] public static void validation_createOrder_customerDetails_invoice_company_NL()
    {
        new Webpay.Integration.Test.Webservice.Payment.InvoicePaymentTest()
            .TestInvoiceRequestObjectForCustomerIdentityCompanyFromNl();
    }
    
    // validation - createOrder - customerDetails - partpayment - individual - SE
    // validation - createOrder - customerDetails - partpayment - company - SE
    // validation - createOrder - customerDetails - partpayment - individual - NO
    // validation - createOrder - customerDetails - partpayment - company - NO
    // validation - createOrder - customerDetails - partpayment - individual - DK
    // validation - createOrder - customerDetails - partpayment - company - DK
    // validation - createOrder - customerDetails - partpayment - individual - FI
    // validation - createOrder - customerDetails - partpayment - company - FI
    // validation - createOrder - customerDetails - partpayment - individual - DE
    // validation - createOrder - customerDetails - partpayment - company - DE
    // validation - createOrder - customerDetails - partpayment - individual - NL
    // validation - createOrder - customerDetails - partpayment - company - NL

    // validation - createOrder - hosted - all payment methods are accepted -- see BV documentation, TODO hosted invoice/paymentplan?
    // validation - createOrder - hosted - setReturnUrl
    // validation - createOrder - hosted - setCallbackUrl
    // validation - createOrder - hosted - setCancelUrl
    // validation - createOrder - hosted - setPayPageLanguageCode
    // validation - createOrder - hosted - setSubscriptionType
    // validation - createOrder - hosted - setSubscriptionId
    // validation - createOrder - hosted - customerDetails - getPaymentUrl
    // TODO how to handle the paymentmethods Invoice/Partpayment? -- not tested?? should be added to documentation if disallowed or deprecated.

    // validation - createOrder - addOrderRow - precisely two of setAmountIncVat, setAmountExVat, setVatPercent required
    // validation - createOrder - addOrderRow - setAmountIncVat, setVatPercent => priceIncludingVat true
    // validation - createOrder - addOrderRow - setAmountExVat, setVatPercent => priceIncludingVat false
    // validation - createOrder - addOrderRow - setAmountIncVat, setAmountExVat => priceIncludingVat ?? TODO

    // validation - createOrder - setName w/setDescription - webservice => concatenated
    // validation - createOrder - setName w/setDescription - hosted => both sent

    // validation - createOrder - sums - addOrderRow - webservice - priceIncludingVat false
    // validation - createOrder - sums - addOrderRow - webservice - priceIncludingVat true
    // validation - createOrder - sums - addOrderRow - hosted - priceIncludingVat false
    // validation - createOrder - sums - addOrderRow - hosted - priceIncludingVat true
    // validation - createOrder - sums - above w/addFee - webservice - priceIncludingVat false
    [Test] public static async Task validation_createOrder_sums_orderRow_shippingFee_invoiceFee_company_NL()
    {
        new Webpay.Integration.Test.Webservice.Payment.InvoicePaymentTest()
            .TestInvoiceRequestObjectForSEOrderOnOneProductRow();
    }
    // validation - createOrder - sums - above w/addFee - webservice - priceIncludingVat true
    // validation - createOrder - sums - above w/addFee - hosted - priceIncludingVat false
    // validation - createOrder - sums - above w/addFee - hosted - priceIncludingVat true
    // validation - createOrder - sums - above w/fixedDiscount - webservice - priceIncludingVat false
    // validation - createOrder - sums - above w/fixedDiscount - webservice - priceIncludingVat true
    // validation - createOrder - sums - above w/fixedDiscount - hosted - priceIncludingVat false
    // validation - createOrder - sums - above w/fixedDiscount - hosted - priceIncludingVat true
    // validation - createOrder - sums - above w/relativeDiscount - webservice - priceIncludingVat false
    // validation - createOrder - sums - above w/relativeDiscount - webservice - priceIncludingVat true
    // validation - createOrder - sums - above w/relativeDiscount - hosted - priceIncludingVat false
    // validation - createOrder - sums - above w/relativeDiscount - hosted - priceIncludingVat true

    // deliverOrder - deliverInvoiceOrder - with orderRows => request class A
    [Test]
    public void test_deliverOrder_deliverInvoiceOrder_with_order_rows_goes_against_DeliverOrderEU()
    {
        var fakeSveaOrderId = 987654;
        DeliverOrderEuRequest request = WebpayConnection.DeliverOrder(SveaConfig.GetDefaultConfig())
            .AddOrderRow(TestingTool.CreateExVatBasedOrderRow("1"))
            .SetOrderId(fakeSveaOrderId)
            .SetCountryCode(CountryCode.SE)
            .SetInvoiceDistributionType(DistributionType.POST)
            .DeliverInvoiceOrder()
                .PrepareRequest()
        ;
        Assert.IsInstanceOf<DeliverOrderEuRequest>(request);
    }
    // deliverOrder - deliverInvoiceOrder - with orderRows - doRequest => response class A        
    // deliverOrder - deliverInvoiceOrder - with orderRows - setCreditInvoice - doRequest => response class ?      

    // deliverOrder - deliverInvoiceOrder - without orderRows => throws "MISSING VALUE..."
    [Test]
    public void test_deliverOrder_deliverInvoiceOrder_without_order_rows_throws_validation_exception()
    {
        var fakeSveaOrderId = 987654;

        var ex = Assert.Throws<Webpay.Integration.Exception.SveaWebPayValidationException>(() =>             
            WebpayConnection.DeliverOrder(SveaConfig.GetDefaultConfig())
            //.AddOrderRow(TestingTool.CreateExVatBasedOrderRow("1"))
            .SetOrderId(fakeSveaOrderId)
            .SetCountryCode(CountryCode.SE)
            .SetInvoiceDistributionType(DistributionType.POST)
            .DeliverInvoiceOrder()
                .PrepareRequest()
        );
        Assert.That(ex.Message, Is.EqualTo("MISSING VALUE - No order or fee has been included. Use AddOrder(...) or AddFee(...)."));
        //Assert.IsInstanceOf<DeliverOrdersRequest>(request);   // not implemented
    }

    // deliverOrder - deliverPaymentPlanOrder - with orderRows -- TODO test and define/document behaviour for paymentplan orders, existing docs based on invoice?        
    // deliverOrder - deliverPaymentPlanOrder - without orderRows => throws "MISSING VALUE..."

    // deliverOrder - deliverCardOrder
    [Test]
    public void test_deliverOrder_deliverCardOrder_with_order_rows_returns_HostedAdminRequest()
    {
        var fakeSveaOrderId = 987654;
        HostedActionRequest request = WebpayConnection.DeliverOrder(SveaConfig.GetDefaultConfig())
            .AddOrderRow(TestingTool.CreateExVatBasedOrderRow("1"))
            .SetOrderId(fakeSveaOrderId)
            .SetCountryCode(CountryCode.SE)
            .DeliverCardOrder()
        ;
        Assert.IsInstanceOf<HostedActionRequest>(request);
    }
    
    // deliverOrder - TODO deliver other payment method orders? -- document if should use confirmTransaction et al instead?

    // getAddresses - getIndividualAddresses - doRequest => response object, use getIndividualCustomers to get a list of IndividualCustomer
    // getAddresses - getCompanyAddresses - doRequest => response object, use getCompanyCustomers to get a list of CompanyCustomer
    // getAddresses - deprecated legacy methods - doRequest => response object, use deprecated getXXX() to get individual attributes

    // getPaymentPlanParams - SE - doRequest
    // getPaymentPlanParams - NO - doRequest
    // getPaymentPlanParams - FI - doRequest
    // getPaymentPlanParams - DK - doRequest
    // getPaymentPlanParams - DE - doRequest
    // getPaymentPlanParams - NE - doRequest

    // paymentPlanPricePerMonth - verify deprecated
    // Helper.paymentPlanPricePerMonth - verify exists
    // Helper.paymentPlanPricePerMonth - sums - verify sums

    // WebPayAdmin Request and Response Classes
    //
    // WebPayAdmin.cancelOrder() -------------------------------------------------------------------------------------	
    // cancelOrder validators
    // invoice
    //test_validates_all_required_methods_for_cancelOrder_cancelInvoiceOrder
    //test_missing_required_method_for_cancelOrder_cancelInvoiceOrder_setOrderId
    //test_missing_required_method_for_cancelOrder_cancelInvoiceOrder_setCountryCode
    // paymentplan
    //test_validates_all_required_methods_for_cancelOrder_cancelPaymentPlanOrder
    //test_missing_required_method_for_cancelOrder_cancelPaymentPlanOrder_setOrderId
    //test_missing_required_method_for_cancelOrder_cancelPaymentPlanOrder_setCountryCode
    // card
    //test_validates_all_required_methods_for_cancelOrder_cancelCardOrder
    //test_missing_required_method_for_cancelOrder_cancelCardOrder_setOrderId
    //test_missing_required_method_for_cancelOrder_cancelCardOrder_setCountryCode

    // WebPayAdmin.queryOrder() --------------------------------------------------------------------------------------------	
    // .queryInvoiceOrder => AdminService/GetOrdersRequest
    //test_queryOrder_queryInvoiceOrder_returns_GetOrdersRequest
    // .queryPaymentPlanOrder => AdminService/GetOrdersRequest
    // TODO
    // .queryDirectBankOrder => HostedService/QueryTransactionRequest
    // TODO
    // .queryCardOrder => HostedService/QueryTransactionRequest
    //test_queryOrder_queryCardOrder_returns_QueryTransactionRequest

    // builder object validation
    // directbank
    // TODO public void test_validates_all_required_methods_for_queryOrder_queryDirectBankOrder() {
    // card
    //test_validates_all_required_methods_for_queryOrder_queryCardOrder    
    //test_queryOrder_validates_missing_required_method_for_queryCardOrder_setOrderId
    //test_queryOrder_validates_missing_required_method_for_queryCardOrder_setCountryCode

    // WebPayAdmin.deliverOrderRows() --------------------------------------------------------------------------------------------	
    // returned request class
    // TODO other methods
    // .deliverCardOrderRows => HostedService/ConfirmTransactionRequest
    //test_deliverOrderRows_deliverCardOrderRows_returns_ConfirmTransactionResponse
    // builder object validation
    // TODO other methods
    // .deliverCardOrderRows validation
    //test_validates_all_required_methods_for_deliverOrderRows_deliverCardOrderRows
    //test_deliverOrderRows_validates_missing_required_method_for_deliverCardOrderRows

    // WebPayAdmin.cancelOrderRows() --------------------------------------------------------------------------------------------	
    // returned request class
    // TODO other methods
    // .cancelCardOrderRows => HostedService/AnnulTransactionRequest
    //test_cancelOrderRows_cancelCardOrderRows_returns_LowerTransactionResponse
    // builder object validation
    // TODO other methods
    // .cancelCardOrderRow validation
    //test_validates_all_required_methods_for_cancelOrderRows_cancelCardOrderRows
    //test_cancelOrderRows_validates_missing_required_method_for_cancelCardOrderRows

    // WebPayAdmin.creditOrderRows() --------------------------------------------------------------------------------------------	
    // returned request class
    // TODO other methods
    // .creditCardOrderRows => HostedService/CreditTransactionRequest
    //test_creditOrderRows_creditCardOrderRows_returns_LowerTransactionResponse
}
