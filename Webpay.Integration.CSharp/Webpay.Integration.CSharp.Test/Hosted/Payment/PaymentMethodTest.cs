﻿using NUnit.Framework;
using Webpay.Integration.CSharp.Hosted.Helper;
using Webpay.Integration.CSharp.Order.Row;
using Webpay.Integration.CSharp.Util.Constant;
using Webpay.Integration.CSharp.Util.Security;

namespace Webpay.Integration.CSharp.Test.Hosted.Payment
{
    [TestFixture]
    public class PaymentMethodTest
    {
        [Test]
        public void TestPayPagePaymentWithSetPaymentMethod()
        {
            PaymentForm form = WebpayConnection.CreateOrder()
                                               .AddOrderRow(Item.OrderRow()
                                                                .SetArticleNumber("1")
                                                                .SetQuantity(2)
                                                                .SetAmountExVat(new decimal(100.00))
                                                                .SetDescription("Specification")
                                                                .SetName("Prod")
                                                                .SetUnit("st")
                                                                .SetVatPercent(25)
                                                                .SetDiscountPercent(0))
                                               .AddDiscount(Item.RelativeDiscount()
                                                                .SetDiscountId("1")
                                                                .SetDiscountPercent(50)
                                                                .SetUnit("st")
                                                                .SetName("Relative")
                                                                .SetDescription("RelativeDiscount"))
                                               .AddCustomerDetails(Item.IndividualCustomer()
                                                                       .SetNationalIdNumber("194605092222"))
                                               .SetCountryCode(CountryCode.SE)
                                               .SetClientOrderNumber("33")
                                               .SetOrderDate("2012-12-12")
                                               .SetCurrency(Currency.SEK)
                                               .UsePaymentMethod(PaymentMethod.KORTCERT)
                                               .SetReturnUrl("http://myurl.se")
                                               .GetPaymentForm();


            string xml = form.GetXmlMessage();
            const string expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><!--Message generated by Integration package C#--><payment><paymentmethod>KORTCERT</paymentmethod><customerrefno>33</customerrefno><returnurl>http://myurl.se</returnurl><currency>SEK</currency><amount>12500</amount><lang>en</lang><customer><ssn>194605092222</ssn><country>SE</country></customer><vat>2500</vat><orderrows><row><sku>1</sku><name>Prod</name><description>Specification</description><amount>12500</amount><vat>2500</vat><quantity>2</quantity><unit>st</unit></row><row><sku>1</sku><name>Relative</name><description>RelativeDiscount</description><amount>-12500</amount><vat>-2500</vat><quantity>1</quantity><unit>st</unit></row></orderrows><excludepaymentMethods /><iscompany>false</iscompany><addinvoicefee>false</addinvoicefee></payment>";
            Assert.AreEqual(expectedXml, xml);

            string base64Payment = form.GetXmlMessageBase64();
            string html = Base64Util.DecodeBase64String(base64Payment);
            const string expectedSub = "KORTCERT";
            string actual = html.Substring(html.IndexOf("<paymentmethod>", 0) + "<paymentmethod>".Length, expectedSub.Length);

            Assert.AreEqual(expectedSub, actual);
        }

        [Test]
        public void TestPayPagePaymentWithSetPaymentMethodNl()
        {
            PaymentForm form = WebpayConnection.CreateOrder()
                                               .AddOrderRow(Item.OrderRow()
                                                                .SetArticleNumber("1")
                                                                .SetQuantity(2)
                                                                .SetAmountExVat(new decimal(100.00))
                                                                .SetDescription("Specification")
                                                                .SetName("Prod")
                                                                .SetUnit("st")
                                                                .SetVatPercent(25)
                                                                .SetDiscountPercent(0))
                                               .AddDiscount(Item.RelativeDiscount()
                                                                .SetDiscountId("1")
                                                                .SetDiscountPercent(50)
                                                                .SetUnit("st")
                                                                .SetName("Relative")
                                                                .SetDescription("RelativeDiscount"))
                                               .AddCustomerDetails(Item.IndividualCustomer()
                                                                       .SetInitials("SB")
                                                                       .SetBirthDate("19460509")
                                                                       .SetName("Sneider", "Boasman")
                                                                       .SetStreetAddress("Gate", "42")
                                                                       .SetLocality("BARENDRECHT")
                                                                       .SetZipCode("1102 HG"))
                                               .SetCountryCode(CountryCode.NL)
                                               .SetClientOrderNumber("33")
                                               .SetOrderDate("2012-12-12")
                                               .SetCurrency(Currency.SEK)
                                               .UsePaymentMethod(PaymentMethod.INVOICE)
                                               .SetReturnUrl("http://myurl.se")
                                               .GetPaymentForm();


            string xml = form.GetXmlMessage();
            const string expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><!--Message generated by Integration package C#--><payment><paymentmethod>INVOICE</paymentmethod><customerrefno>33</customerrefno><returnurl>http://myurl.se</returnurl><currency>SEK</currency><amount>12500</amount><lang>en</lang><customer><ssn>19460509</ssn><firstname>Sneider</firstname><lastname>Boasman</lastname><initials>SB</initials><address>Gate</address><zip>1102 HG</zip><housenumber>42</housenumber><city>BARENDRECHT</city><country>NL</country></customer><vat>2500</vat><orderrows><row><sku>1</sku><name>Prod</name><description>Specification</description><amount>12500</amount><vat>2500</vat><quantity>2</quantity><unit>st</unit></row><row><sku>1</sku><name>Relative</name><description>RelativeDiscount</description><amount>-12500</amount><vat>-2500</vat><quantity>1</quantity><unit>st</unit></row></orderrows><excludepaymentMethods /><iscompany>false</iscompany><addinvoicefee>false</addinvoicefee></payment>";
            Assert.AreEqual(expectedXml, xml);

            string base64Payment = form.GetXmlMessageBase64();
            string html = Base64Util.DecodeBase64String(base64Payment);
            const string expectedSub = "INVOICE";
            string actual = html.Substring(html.IndexOf("<paymentmethod>", 0) + "<paymentmethod>".Length, expectedSub.Length);

            Assert.AreEqual(expectedSub, actual);
        }
    }
}