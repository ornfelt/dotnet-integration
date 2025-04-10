﻿using System;
using System.Collections.Generic;
using Webpay.Integration.CSharp.Config;
using Webpay.Integration.CSharp.Order.Row;
using Webpay.Integration.CSharp.Util.Constant;

namespace Webpay.Integration.CSharp.Order.Handle
{
    public class AddOrderRowsBuilder : Builder<AddOrderRowsBuilder>
    {
        internal long Id { get; private set; }
        internal PaymentType OrderType { get; set; }
        internal List<OrderRowBuilder> OrderRows { get; private set; }
        internal List<InvoiceFeeBuilder> InvoiceFeeRows { get; private set; }

        public AddOrderRowsBuilder(IConfigurationProvider config) : base(config)
        {
            OrderRows = new List<OrderRowBuilder>();
            InvoiceFeeRows = new List<InvoiceFeeBuilder>();
        }

        public AddOrderRowsBuilder SetOrderId(long orderId)
        {
            Id = orderId;
            return this;
        }

        public override AddOrderRowsBuilder SetCountryCode(CountryCode countryCode)
        {
            _countryCode = countryCode;
            return this;
        }

        public AddOrderRowsBuilder AddOrderRow(OrderRowBuilder orderRow)
        {
            OrderRows.Add(orderRow);
            return this;
        }
        public AddOrderRowsBuilder AddOrderRows( IList<OrderRowBuilder> orderRows)
        {
            OrderRows.AddRange(orderRows);
            return this;
        }

        public AddOrderRowsBuilder AddFee(IRowBuilder fee)
        {
            InvoiceFeeBuilder invoiceFee = fee as InvoiceFeeBuilder;
            if (invoiceFee != null)
            {
                InvoiceFeeRows.Add(invoiceFee);
            }

            return this;
        }

        public AddOrderRowsBuilder AddInvoiceFee(InvoiceFeeBuilder invoiceFee)
        {
            if (invoiceFee == null)
            {
                throw new ArgumentNullException(nameof(invoiceFee));
            }
            InvoiceFeeRows.Add(invoiceFee);
            return this;
        }

        public AdminService.AddOrderRowsRequest AddInvoiceOrderRows()
        {
            OrderType = PaymentType.INVOICE;
            return new AdminService.AddOrderRowsRequest(this);
        }
        public AdminService.AddOrderRowsRequest AddPaymentPlanOrderRows()
        {
            OrderType = PaymentType.PAYMENTPLAN;
            return new AdminService.AddOrderRowsRequest(this);
        }

        public override AddOrderRowsBuilder SetCorrelationId(Guid? correlationId)
        {
            _correlationId = correlationId;
            return this;
        }
    }
}