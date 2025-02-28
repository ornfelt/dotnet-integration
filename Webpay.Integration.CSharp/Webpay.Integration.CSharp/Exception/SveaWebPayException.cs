﻿using System;

namespace Webpay.Integration.CSharp.Exception
{
    [Serializable]
    public class SveaWebPayException : SystemException
    {
        public SveaWebPayException(String message, System.Exception innerException)
            : base(message, innerException)
        {
        }

        public SveaWebPayException(string message)
            : base(message)
        {
        }
    }
}