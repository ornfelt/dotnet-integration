﻿using System.Text.RegularExpressions;
using Webpay.Integration.Config;
using Webpay.Integration.Hosted.Helper;
using Webpay.Integration.Order.Create;
using Webpay.Integration.Order.Row;
using Webpay.Integration.Test.Hosted.Payment;
using Webpay.Integration.Util.Constant;
using Webpay.Integration.Util.Testing;

namespace Webpay.Integration.Test.Hosted.Helper;

[TestFixture]
public class HostedXmlBuilderTest
{
    private HostedXmlBuilder _xmlBuilder;
    private CreateOrderBuilder _order;
    private string _xml = "";

    [SetUp]
    public void SetUp()
    {
        _xmlBuilder = new HostedXmlBuilder();
    }

    [Test]
    public void TestBasicXml()
    {
        var order = WebpayConnection.CreateOrder(SveaConfig.GetDefaultConfig())
            .SetClientOrderNumber(TestingTool.DefaultTestClientOrderNumber)
            .SetCountryCode(TestingTool.DefaultTestCountryCode)
            .SetCurrency(TestingTool.DefaultTestCurrency)
            .AddCustomerDetails(Item.IndividualCustomer()
                .SetNationalIdNumber(TestingTool.DefaultTestIndividualNationalIdNumber))
            .AddOrderRow(Item.OrderRow()
                .SetAmountExVat(4)
                .SetVatPercent(25)
                .SetQuantity(1));

        var payment = new FakeHostedPayment(order);
        payment.SetReturnUrl("http://myurl.se")
               .CalculateRequestValues();

        var xml = _xmlBuilder.GetXml(payment);

        const string pattern =
            @"<\?xml version=\""1\.0\"" encoding=\""utf-8\""\?><!--Message generated by Integration package C#-->" +
            @"<payment><customerrefno>[a-zA-Z0-9\-]+</customerrefno>" +
            @"<currency>SEK</currency><amount>500</amount><vat>100</vat><lang>en</lang>" +
            @"<returnurl>http://myurl.se</returnurl><iscompany>false</iscompany>" +
            @"<customer><ssn>194605092222</ssn><country>SE</country></customer><orderrows><row><sku /><name />" +
            @"<description /><amount>500</amount><vat>100</vat><quantity>1</quantity>" +
            @"</row></orderrows><excludepaymentMethods /><addinvoicefee>false</addinvoicefee></payment>";

        Assert.That(xml, Does.Match(new Regex(pattern, RegexOptions.Singleline)));
    }

    [Test]
    public void TestXmlWithIndividualCustomer()
    {
        var order = WebpayConnection.CreateOrder(SveaConfig.GetDefaultConfig())
            .SetCountryCode(TestingTool.DefaultTestCountryCode)
            .SetCurrency(TestingTool.DefaultTestCurrency)
            .SetClientOrderNumber(TestingTool.DefaultTestClientOrderNumber)
            .AddOrderRow(Item.OrderRow()
                .SetAmountExVat(4)
                .SetVatPercent(25)
                .SetQuantity(1))
            .AddCustomerDetails(Item.IndividualCustomer()
                .SetName("Julius", "Caesar")
                .SetInitials("JS")
                .SetNationalIdNumber("666666")
                .SetPhoneNumber("999999")
                .SetEmail("test@svea.com")
                .SetIpAddress("123.123.123.123")
                .SetStreetAddress("Gatan", "23")
                .SetCoAddress("c/o Eriksson")
                .SetZipCode("9999")
                .SetLocality("Stan"));

        var payment = new FakeHostedPayment(order);
        payment.SetReturnUrl("http://myurl.se")
               .CalculateRequestValues();

        var xml = _xmlBuilder.GetXml(payment);

        const string expectedString =
            "<customer><ssn>666666</ssn><firstname>Julius</firstname>" +
            "<lastname>Caesar</lastname><initials>JS</initials>" +
            "<phone>999999</phone><email>test@svea.com</email>" +
            "<address>Gatan</address><housenumber>23</housenumber>" +
            "<address2>c/o Eriksson</address2><zip>9999</zip>" +
            "<city>Stan</city><country>SE</country></customer>";

        Assert.That(xml.Contains(expectedString), Is.True);
        Assert.That(xml.Contains("<ipaddress>123.123.123.123</ipaddress>"), Is.True);
    }

    [Test]
    public void TestXmlWithCompanyCustomer()
    {
        var order = WebpayConnection.CreateOrder(SveaConfig.GetDefaultConfig())
            .AddOrderRow(Item.OrderRow()
                .SetAmountExVat(4)
                .SetVatPercent(25)
                .SetQuantity(1))
            .SetCurrency(TestingTool.DefaultTestCurrency)
            .SetClientOrderNumber(TestingTool.DefaultTestClientOrderNumber)
            .SetCountryCode(TestingTool.DefaultTestCountryCode)
            .AddCustomerDetails(TestingTool.CreateCompanyCustomer());

        var payment = new FakeHostedPayment(order);
        payment.SetReturnUrl("http://myurl.se")
               .CalculateRequestValues();

        var xml = _xmlBuilder.GetXml(payment);

        const string expectedString =
            "<customer><ssn>194608142222</ssn><firstname>Tess, T Persson</firstname>" +
            "<phone>0811111111</phone><email>test@svea.com</email>" +
            "<address>Testgatan</address><housenumber>1</housenumber>" +
            "<address2>c/o Eriksson, Erik</address2><zip>99999</zip>" +
            "<city>Stan</city><country>SE</country></customer>";

        Assert.That(xml.Contains(expectedString), Is.True);
        Assert.That(xml.Contains("<ipaddress>123.123.123.123</ipaddress>"), Is.True);
    }

    [Test]
    public void TestXmlCancelUrl()
    {
        var order = WebpayConnection.CreateOrder(SveaConfig.GetDefaultConfig())
            .SetCountryCode(TestingTool.DefaultTestCountryCode)
            .SetCurrency(TestingTool.DefaultTestCurrency)
            .SetClientOrderNumber(TestingTool.DefaultTestClientOrderNumber)
            .AddOrderRow(TestingTool.CreateMiniOrderRow())
            .AddCustomerDetails(Item.CompanyCustomer());

        var payment = new FakeHostedPayment(order);
        payment.SetCancelUrl("http://www.cancel.com")
               .SetReturnUrl("http://myurl.se")
               .CalculateRequestValues();

        var xml = _xmlBuilder.GetXml(payment);

        Assert.That(xml.Contains("<cancelurl>http://www.cancel.com</cancelurl>"), Is.True);
    }

    [Test]
    public void TestXmlCallbackUrl()
    {
        var order = WebpayConnection.CreateOrder(SveaConfig.GetDefaultConfig())
            .SetCountryCode(TestingTool.DefaultTestCountryCode)
            .SetCurrency(TestingTool.DefaultTestCurrency)
            .SetClientOrderNumber(TestingTool.DefaultTestClientOrderNumber)
            .AddOrderRow(TestingTool.CreateMiniOrderRow())
            .AddCustomerDetails(Item.CompanyCustomer());

        var payment = new FakeHostedPayment(order);
        payment.SetCallbackUrl("http://www.callback.nu")
               .SetCancelUrl("http://www.cancel.com")
               .SetReturnUrl("http://myurl.se")
               .CalculateRequestValues();

        var xml = _xmlBuilder.GetXml(payment);

        Assert.That(xml.Contains("<callbackurl>http://www.callback.nu</callbackurl>"), Is.True);
    }

    [Test]
    public void TestOrderRowXml()
    {
        var order = WebpayConnection.CreateOrder(SveaConfig.GetDefaultConfig())
            .AddOrderRow(Item.OrderRow()
                .SetArticleNumber("0")
                .SetName("Product")
                .SetDescription("Good product")
                .SetAmountExVat(4)
                .SetVatPercent(25)
                .SetQuantity(1)
                .SetUnit("kg"))
            .AddCustomerDetails(Item.CompanyCustomer())
            .SetCountryCode(TestingTool.DefaultTestCountryCode)
            .SetCurrency(TestingTool.DefaultTestCurrency)
            .SetClientOrderNumber(TestingTool.DefaultTestClientOrderNumber);

        var payment = new FakeHostedPayment(order);
        payment.SetPayPageLanguageCode(LanguageCode.sv)
               .SetReturnUrl("http://myurl.se")
               .CalculateRequestValues();

        var xml = _xmlBuilder.GetXml(payment);

        const string expectedString =
            "<orderrows><row><sku>0</sku><name>Product</name>" +
            "<description>Good product</description><amount>500</amount>" +
            "<vat>100</vat><quantity>1</quantity><unit>kg</unit></row></orderrows>";

        Assert.That(xml.Contains(expectedString), Is.True);
    }

    [Test]
    public void TestDirectPaymentSpecificXml()
    {
        var xml = WebpayConnection.CreateOrder(SveaConfig.GetDefaultConfig())
            .SetCountryCode(TestingTool.DefaultTestCountryCode)
            .SetCurrency(TestingTool.DefaultTestCurrency)
            .SetClientOrderNumber(TestingTool.DefaultTestClientOrderNumber)
            .AddOrderRow(TestingTool.CreateMiniOrderRow())
            .AddCustomerDetails(Item.CompanyCustomer())
            .UsePayPageDirectBankOnly()
            .SetReturnUrl("https://webpaypaymentgatewaystage.svea.com/Webpayconnection/admin/merchantresponSetest.xhtm")
            .GetPaymentForm()
            .GetXmlMessage();

        const string expectedString =
            "<excludepaymentMethods><exclude>BANKAXESS</exclude>" +
            "<exclude>PAYPAL</exclude><exclude>KORTCERT</exclude>" +
            "<exclude>SVEACARDPAY</exclude><exclude>SKRILL</exclude>" +
            "<exclude>SVEAINVOICESE</exclude><exclude>SVEAINVOICEEU_SE</exclude>" +
            "<exclude>SVEASPLITSE</exclude><exclude>SVEASPLITEU_SE</exclude>" +
            "<exclude>SVEAINVOICEEU_DE</exclude><exclude>SVEASPLITEU_DE</exclude>" +
            "<exclude>SVEAINVOICEEU_DK</exclude><exclude>SVEASPLITEU_DK</exclude>" +
            "<exclude>SVEAINVOICEEU_FI</exclude><exclude>SVEASPLITEU_FI</exclude>" +
            "<exclude>SVEAINVOICEEU_NL</exclude><exclude>SVEASPLITEU_NL</exclude>" +
            "<exclude>SVEAINVOICEEU_NO</exclude><exclude>SVEASPLITEU_NO</exclude>" +
            "</excludepaymentMethods>";

        Assert.That(xml.Contains(expectedString), Is.True);
    }

    [Test]
    public void TestCardPaymentSpecificXml()
    {
        var xml = WebpayConnection.CreateOrder(SveaConfig.GetDefaultConfig())
            .SetCountryCode(TestingTool.DefaultTestCountryCode)
            .SetCurrency(TestingTool.DefaultTestCurrency)
            .SetClientOrderNumber(TestingTool.DefaultTestClientOrderNumber)
            .AddOrderRow(TestingTool.CreateMiniOrderRow())
            .AddCustomerDetails(Item.CompanyCustomer())
            .UsePayPageCardOnly()
            .SetReturnUrl("https://webpaypaymentgatewaystage.svea.com/Webpayconnection/admin/merchantresponSetest.xhtm")
            .GetPaymentForm()
            .GetXmlMessage();

        const string expectedString =
            "<excludepaymentMethods><exclude>PAYPAL</exclude>" +
            "<exclude>DBNORDEASE</exclude><exclude>DBSEBSE</exclude>" +
            "<exclude>DBSEBFTGSE</exclude><exclude>DBSHBSE</exclude>" +
            "<exclude>DBSWEDBANKSE</exclude><exclude>BANKAXESS</exclude>" +
            "<exclude>SVEAINVOICESE</exclude>" +
            "<exclude>SVEAINVOICEEU_SE</exclude>" +
            "<exclude>SVEASPLITSE</exclude><exclude>SVEASPLITEU_SE</exclude>" +
            "<exclude>SVEAINVOICEEU_DE</exclude>" +
            "<exclude>SVEASPLITEU_DE</exclude>" +
            "<exclude>SVEAINVOICEEU_DK</exclude>" +
            "<exclude>SVEASPLITEU_DK</exclude>" +
            "<exclude>SVEAINVOICEEU_FI</exclude><exclude>SVEASPLITEU_FI</exclude>" +
            "<exclude>SVEAINVOICEEU_NL</exclude><exclude>SVEASPLITEU_NL</exclude>" +
            "<exclude>SVEAINVOICEEU_NO</exclude><exclude>SVEASPLITEU_NO</exclude>" +
            "</excludepaymentMethods>";

        Assert.That(xml.Contains(expectedString), Is.True);
    }

    [Test]
    public void TestPayPagePaymentSpecificXmlNullPaymentMethod()
    {
        var xml = WebpayConnection.CreateOrder(SveaConfig.GetDefaultConfig())
            .SetCountryCode(TestingTool.DefaultTestCountryCode)
            .SetCurrency(TestingTool.DefaultTestCurrency)
            .SetClientOrderNumber(TestingTool.DefaultTestClientOrderNumber)
            .AddOrderRow(TestingTool.CreateMiniOrderRow())
            .AddCustomerDetails(Item.CompanyCustomer())
            .UsePayPage()
            .SetReturnUrl(
                "https://webpaypaymentgatewaystage.svea.com/Webpayconnection/admin/merchantresponSetest.xhtm")
            .GetPaymentForm()
            .GetXmlMessage();

        Assert.That(xml.Contains("<excludepaymentMethods />"), Is.True);
    }

    [Test]
    public void TestPayPagePaymentSetLanguageCode()
    {
        var xml = WebpayConnection.CreateOrder(SveaConfig.GetDefaultConfig())
            .SetCountryCode(TestingTool.DefaultTestCountryCode)
            .SetCurrency(TestingTool.DefaultTestCurrency)
            .SetClientOrderNumber(TestingTool.DefaultTestClientOrderNumber)
            .AddOrderRow(TestingTool.CreateMiniOrderRow())
            .AddCustomerDetails(Item.CompanyCustomer())
            .UsePayPage()
            .SetPayPageLanguageCode(LanguageCode.sv)
            .SetReturnUrl(
                "https://webpaypaymentgatewaystage.svea.com/Webpayconnection/admin/merchantresponSetest.xhtm")
            .GetPaymentForm()
            .GetXmlMessage();

        Assert.That(xml.Contains("<lang>sv</lang>"), Is.True);
    }

    [Test]
    public void TestPayPagePaymentPayPal()
    {
        var xml = WebpayConnection.CreateOrder(SveaConfig.GetDefaultConfig())
            .SetCountryCode(TestingTool.DefaultTestCountryCode)
            .SetCurrency(TestingTool.DefaultTestCurrency)
            .SetClientOrderNumber(TestingTool.DefaultTestClientOrderNumber)
            .AddOrderRow(TestingTool.CreateMiniOrderRow())
            .AddCustomerDetails(Item.CompanyCustomer())
            .UsePayPage()
            .SetReturnUrl(
                "https://webpaypaymentgatewaystage.svea.com/Webpayconnection/admin/merchantresponSetest.xhtm")
            .SetPaymentMethod(PaymentMethod.PAYPAL)
            .GetPaymentForm()
            .GetXmlMessage();

        Assert.That(xml.Contains("<paymentmethod>PAYPAL</paymentmethod>"), Is.True);
    }

    [Test]
    public void TestPayPagePaymentSpecificXml()
    {
        var xml = WebpayConnection.CreateOrder(SveaConfig.GetDefaultConfig())
            .SetCountryCode(TestingTool.DefaultTestCountryCode)
            .SetCurrency(TestingTool.DefaultTestCurrency)
            .SetClientOrderNumber(TestingTool.DefaultTestClientOrderNumber)
            .AddOrderRow(TestingTool.CreateMiniOrderRow())
            .AddCustomerDetails(Item.CompanyCustomer())
            .UsePayPage()
            .SetReturnUrl(
                "https://webpaypaymentgatewaystage.svea.com/Webpayconnection/admin/merchantresponSetest.xhtm")
            .SetPaymentMethod(PaymentMethod.INVOICE)
            .GetPaymentForm()
            .GetXmlMessage();

        Assert.That(xml.Contains("<paymentmethod>SVEAINVOICEEU_SE</paymentmethod>"), Is.True);
    }
}