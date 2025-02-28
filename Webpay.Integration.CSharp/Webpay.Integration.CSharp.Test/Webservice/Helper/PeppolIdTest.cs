﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Webpay.Integration.CSharp.Webservice.Helper;

namespace Webpay.Integration.CSharp.Test.Webservice.Helper
{
    [TestClass]
    public class PeppolIdTest
    {
        [TestMethod]
        public void TestValidPeppolId()
        {
            bool validationResult = PeppolId.IsValidPeppolId("1234:asdf");
            Assert.IsTrue(validationResult);
        }

        [TestMethod]
        public void TestInvalidPeppolId()
        {
            bool validationResult = PeppolId.IsValidPeppolId("1:1");
            Assert.IsTrue(!validationResult);
        }
    }
}
