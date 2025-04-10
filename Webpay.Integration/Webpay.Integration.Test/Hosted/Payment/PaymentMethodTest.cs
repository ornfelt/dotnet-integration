﻿using System.Text.RegularExpressions;
using Webpay.Integration.Config;
using Webpay.Integration.Hosted.Helper;
using Webpay.Integration.Order.Row;
using Webpay.Integration.Util.Constant;
using Webpay.Integration.Util.Security;
using Webpay.Integration.Util.Testing;

namespace Webpay.Integration.Test.Hosted.Payment;

[TestFixture]
public class PaymentMethodTest
{
    [Test]
    public void TestPayPagePaymentWithSetPaymentMethod()
    {
        var form = WebpayConnection.CreateOrder(SveaConfig.GetDefaultConfig())
            .AddOrderRow(TestingTool.CreateExVatBasedOrderRow())
            .AddDiscount(TestingTool.CreateRelativeDiscount())
            .AddCustomerDetails(
                Item.IndividualCustomer().SetNationalIdNumber(TestingTool.DefaultTestIndividualNationalIdNumber))
            .SetCountryCode(TestingTool.DefaultTestCountryCode)
            .SetClientOrderNumber(TestingTool.DefaultTestClientOrderNumber)
            .SetOrderDate(TestingTool.DefaultTestDate)
            .SetCurrency(TestingTool.DefaultTestCurrency)
            .SetCustomerReference(TestingTool.DefaultTestCustomerReferenceNumber)
            .UsePaymentMethod(PaymentMethod.KORTCERT)
            .SetReturnUrl("http://myurl.se")
            .GetPaymentForm();

        var xml = form.GetXmlMessage();

        const string regexPattern =
            @"<\?xml version=\""1\.0\"" encoding=\""utf-8\""\?><!--Message generated by Integration package C#-->" +
            @"<payment><paymentmethod>KORTCERT</paymentmethod><customerrefno>[a-zA-Z0-9\-]+</customerrefno>" +
            @"<currency>SEK</currency><amount>12500</amount><vat>2500</vat><lang>en</lang>" +
            @"<returnurl>http://myurl.se</returnurl><iscompany>false</iscompany>" +
            @"<customer><ssn>194605092222</ssn><country>SE</country></customer><orderrows>" +
            @"<row><sku>1</sku><name>Prod</name><description>Specification</description><amount>12500</amount>" +
            @"<vat>2500</vat><quantity>2</quantity><unit>st</unit></row><row><sku>1</sku><name>Relative</name>" +
            @"<description>RelativeDiscount</description><amount>-12500</amount><vat>-2500</vat><quantity>1</quantity><unit>st</unit></row></orderrows>" +
            @"<excludepaymentMethods /><addinvoicefee>false</addinvoicefee></payment>";

        Assert.That(xml, Does.Match(new Regex(regexPattern, RegexOptions.Singleline)));

        var base64Payment = form.GetXmlMessageBase64();
        var html = Base64Util.DecodeBase64String(base64Payment);

        Assert.That(html.Contains("<paymentmethod>KORTCERT</paymentmethod>"), Is.True);
    }

    [Test]
    public void TestPayPagePaymentWithSetPaymentMethodSveaCardPay()
    {
        var form = WebpayConnection.CreateOrder(SveaConfig.GetDefaultConfig())
            .AddOrderRow(TestingTool.CreateExVatBasedOrderRow())
            .AddDiscount(TestingTool.CreateRelativeDiscount())
            .AddCustomerDetails(
                Item.IndividualCustomer().SetNationalIdNumber(TestingTool.DefaultTestIndividualNationalIdNumber))
            .SetCountryCode(TestingTool.DefaultTestCountryCode)
            .SetClientOrderNumber(TestingTool.DefaultTestClientOrderNumber)
            .SetOrderDate(TestingTool.DefaultTestDate)
            .SetCurrency(TestingTool.DefaultTestCurrency)
            .SetCustomerReference(TestingTool.DefaultTestCustomerReferenceNumber)
            .UsePaymentMethod(PaymentMethod.SVEACARDPAY)
            .SetReturnUrl("http://myurl.se")
            .GetPaymentForm();

        var xml = form.GetXmlMessage();

        const string regexPattern =
            @"<\?xml version=\""1\.0\"" encoding=\""utf-8\""\?><!--Message generated by Integration package C#-->" +
            @"<payment><paymentmethod>SVEACARDPAY</paymentmethod><customerrefno>[a-zA-Z0-9\-]+</customerrefno>" +
            @"<currency>SEK</currency><amount>12500</amount><vat>2500</vat><lang>en</lang><returnurl>http://myurl.se</returnurl>" +
            @"<iscompany>false</iscompany><customer><ssn>194605092222</ssn><country>SE</country></customer><orderrows>" +
            @"<row><sku>1</sku><name>Prod</name><description>Specification</description><amount>12500</amount>" +
            @"<vat>2500</vat><quantity>2</quantity><unit>st</unit></row><row><sku>1</sku><name>Relative</name>" +
            @"<description>RelativeDiscount</description><amount>-12500</amount><vat>-2500</vat><quantity>1</quantity><unit>st</unit></row></orderrows>" +
            @"<excludepaymentMethods /><addinvoicefee>false</addinvoicefee></payment>";

        Assert.That(xml, Does.Match(new Regex(regexPattern, RegexOptions.Singleline)));

        var base64Payment = form.GetXmlMessageBase64();
        var html = Base64Util.DecodeBase64String(base64Payment);

        Assert.That(html.Contains("<paymentmethod>SVEACARDPAY</paymentmethod>"), Is.True);
    }

    [Test]
    public void TestPayPagePaymentWithSetPaymentMethodSveaCardPayPF()
    {
        var form = WebpayConnection.CreateOrder(SveaConfig.GetDefaultConfig())
            .AddOrderRow(TestingTool.CreateExVatBasedOrderRow())
            .AddDiscount(TestingTool.CreateRelativeDiscount())
            .AddCustomerDetails(
                Item.IndividualCustomer().SetNationalIdNumber(TestingTool.DefaultTestIndividualNationalIdNumber))
            .SetCountryCode(TestingTool.DefaultTestCountryCode)
            .SetClientOrderNumber(TestingTool.DefaultTestClientOrderNumber)
            .SetOrderDate(TestingTool.DefaultTestDate)
            .SetCurrency(TestingTool.DefaultTestCurrency)
            .SetCustomerReference(TestingTool.DefaultTestCustomerReferenceNumber)
            .UsePaymentMethod(PaymentMethod.SVEACARDPAY_PF)
            .SetReturnUrl("http://myurl.se")
            .GetPaymentForm();

        var xml = form.GetXmlMessage();

        const string regexPattern =
            @"<\?xml version=\""1\.0\"" encoding=\""utf-8\""\?><!--Message generated by Integration package C#-->" +
            @"<payment><paymentmethod>SVEACARDPAY_PF</paymentmethod><customerrefno>[a-zA-Z0-9\-]+</customerrefno>" +
            @"<currency>SEK</currency><amount>12500</amount><vat>2500</vat><lang>en</lang>" +
            @"<returnurl>http://myurl.se</returnurl><iscompany>false</iscompany>" +
            @"<customer><ssn>194605092222</ssn><country>SE</country></customer><orderrows>" +
            @"<row><sku>1</sku><name>Prod</name><description>Specification</description><amount>12500</amount>" +
            @"<vat>2500</vat><quantity>2</quantity><unit>st</unit></row><row><sku>1</sku><name>Relative</name>" +
            @"<description>RelativeDiscount</description><amount>-12500</amount><vat>-2500</vat><quantity>1</quantity><unit>st</unit></row></orderrows>" +
            @"<excludepaymentMethods /><addinvoicefee>false</addinvoicefee></payment>";

        Assert.That(xml, Does.Match(new Regex(regexPattern, RegexOptions.Singleline)));

        var base64Payment = form.GetXmlMessageBase64();
        var html = Base64Util.DecodeBase64String(base64Payment);

        Assert.That(html.Contains("<paymentmethod>SVEACARDPAY_PF</paymentmethod>"), Is.True);
    }

    [Test]
    public void TestPayPagePaymentWithSetPaymentMethodNl()
    {
        var form = WebpayConnection.CreateOrder(SveaConfig.GetDefaultConfig())
            .AddOrderRow(TestingTool.CreateExVatBasedOrderRow())
            .AddDiscount(TestingTool.CreateRelativeDiscount())
            .AddCustomerDetails(
                Item.IndividualCustomer()
                    .SetInitials("SB")
                    .SetBirthDate("19460509")
                    .SetName("Sneider", "Boasman")
                    .SetStreetAddress("Gate", "42")
                    .SetLocality("BARENDRECHT")
                    .SetZipCode("1102 HG"))
            .SetCountryCode(CountryCode.NL)
            .SetClientOrderNumber(TestingTool.DefaultTestClientOrderNumber)
            .SetCustomerReference(TestingTool.DefaultTestCustomerReferenceNumber)
            .SetOrderDate(TestingTool.DefaultTestDate)
            .SetCurrency(TestingTool.DefaultTestCurrency)
            .UsePaymentMethod(PaymentMethod.INVOICE)
            .SetReturnUrl("http://myurl.se")
            .GetPaymentForm();

        var xml = form.GetXmlMessage();

        const string regexPattern =
            @"<\?xml version=\""1\.0\"" encoding=\""utf-8\""\?><!--Message generated by Integration package C#-->" +
            @"<payment><paymentmethod>INVOICE</paymentmethod><customerrefno>[a-zA-Z0-9\-]+</customerrefno>" +
            @"<currency>SEK</currency><amount>12500</amount><vat>2500</vat><lang>en</lang><returnurl>http://myurl.se</returnurl>" +
            @"<iscompany>false</iscompany><customer><ssn>19460509</ssn><firstname>Sneider</firstname><lastname>Boasman</lastname>" +
            @"<initials>SB</initials><address>Gate</address><housenumber>42</housenumber><zip>1102 HG</zip><city>BARENDRECHT</city>" +
            @"<country>NL</country></customer><orderrows><row><sku>1</sku><name>Prod</name><description>Specification</description><amount>12500</amount>" +
            @"<vat>2500</vat><quantity>2</quantity><unit>st</unit></row><row><sku>1</sku><name>Relative</name><description>RelativeDiscount</description>" +
            @"<amount>-12500</amount><vat>-2500</vat><quantity>1</quantity><unit>st</unit></row></orderrows><excludepaymentMethods /><addinvoicefee>false</addinvoicefee></payment>";

        Assert.That(xml, Does.Match(new Regex(regexPattern, RegexOptions.Singleline)));

        var base64Payment = form.GetXmlMessageBase64();
        var html = Base64Util.DecodeBase64String(base64Payment);

        Assert.That(html.Contains("<paymentmethod>INVOICE</paymentmethod>"), Is.True);
    }
}