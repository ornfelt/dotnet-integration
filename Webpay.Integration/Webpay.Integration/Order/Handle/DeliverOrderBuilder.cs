﻿using Webpay.Integration.Config;
using Webpay.Integration.Exception;
using Webpay.Integration.Hosted.Admin;
using Webpay.Integration.Hosted.Admin.Actions;
using Webpay.Integration.Order.Row;
using Webpay.Integration.Order.Validator;
using Webpay.Integration.Util.Constant;
using Webpay.Integration.Webservice.Handleorder;

namespace Webpay.Integration.Order.Handle;

public class DeliverOrderBuilder : OrderBuilder<DeliverOrderBuilder>
{
    private HandleOrderValidator _validator;
    private long _orderId;
    private OrderType _orderType = OrderType.NONE;
    private DistributionType _distributionType;
    private long? _invoiceIdToCredit;
    private int _numberOfCreditDays;
    private DateTime? _captureDate;

    public DeliverOrderBuilder(IConfigurationProvider config) : base(config)
    {
        _captureDate = null;
    }

    public HandleOrderValidator GetValidator()
    {
        return _validator;
    }

    public DeliverOrderBuilder SetValidator(HandleOrderValidator validator)
    {
        _validator = validator;
        return this;
    }

    public long GetOrderId()
    {
        return _orderId;
    }

    public DeliverOrderBuilder SetOrderId(long orderId)
    {
        _orderId = orderId;
        return this;
    }

    public OrderType GetOrderType()
    {
        return _orderType;
    }

    public void SetOrderType(OrderType orderType)
    {
        _orderType = orderType;
    }

    public DistributionType GetInvoiceDistributionType()
    {
        return _distributionType;
    }

    public DeliverOrderBuilder SetInvoiceDistributionType(DistributionType type)
    {
        _distributionType = type;
        return this;
    }

    public long? GetCreditInvoice()
    {
        return _invoiceIdToCredit;
    }

    public DeliverOrderBuilder SetCreditInvoice(long invoiceId)
    {
        _invoiceIdToCredit = invoiceId;
        return this;
    }

    public int GetNumberOfCreditDays()
    {
        return _numberOfCreditDays;
    }

    public DeliverOrderBuilder SetNumberOfCreditDays(int numberOfCreditDays)
    {
        _numberOfCreditDays = numberOfCreditDays;
        return this;
    }

    /// <summary>
    /// Prepares the specified payment type for delivery.
    /// </summary>
    /// <param name="paymentType">The type of order to deliver.</param>
    /// <exception cref="SveaWebPayValidationException"></exception>
    /// <returns>HandleOrder</returns>
    public HandleOrder DeliverOrderByPaymentType(PaymentType paymentType)
    {
        _orderType = PaymentTypeExtensions.ToOrderType(paymentType);
        return new HandleOrder(this);
    }

    /// <summary>
    /// Updates the invoice order with additional information and prepares it for delivery.
    /// Will automatically match all order rows that are to be delivered with those which was sent
    /// when creating the invoice order.
    /// </summary>
    /// <exception cref="SveaWebPayValidationException"></exception>
    /// <returns>HandleOrder</returns>
    public HandleOrder DeliverInvoiceOrder()
    {
        _orderType = OrderType.INVOICE;
        return new HandleOrder(this);
    }

    /// <summary>
    /// Prepares the PaymentPlan order for delivery.
    /// </summary>
    /// <exception cref="SveaWebPayValidationException"></exception>
    /// <returns>HandleOrder</returns>
    public HandleOrder DeliverPaymentPlanOrder()
    {
        _orderType = OrderType.PAYMENTPLAN;
        return new HandleOrder(this);
    }

    /// <summary>
    /// Prepares the AccountCredit order for delivery.
    /// </summary>
    /// <exception cref="SveaWebPayValidationException"></exception>
    /// <returns>HandleOrder</returns>
    public HandleOrder DeliverAccountCreditOrder()
    {
        _orderType = OrderType.ACCOUNTCREDIT;
        return new HandleOrder(this);
    }

    public override DeliverOrderBuilder SetCorrelationId(Guid? correlationId)
    {
        _correlationId = correlationId;
        return this;
    }

    public override DeliverOrderBuilder SetFixedDiscountRows(List<FixedDiscountBuilder> fixedDiscountRows)
    {
        FixedDiscountRows = fixedDiscountRows;
        return this;
    }

    public override DeliverOrderBuilder SetRelativeDiscountRows(List<RelativeDiscountBuilder> relativeDiscountRows)
    {
        RelativeDiscountRows = relativeDiscountRows;
        return this;
    }

    public override DeliverOrderBuilder AddOrderRow(OrderRowBuilder itemOrderRow)
    {
        OrderRows.Add(itemOrderRow);
        return this;
    }

    public override DeliverOrderBuilder SetCountryCode(CountryCode countryCode)
    {
        _countryCode = countryCode;
        return this;
    }

    public override DeliverOrderBuilder AddOrderRows(IEnumerable<OrderRowBuilder> itemOrderRow)
    {
        OrderRows.AddRange(itemOrderRow);
        return this;
    }

    public override DeliverOrderBuilder AddDiscount(IRowBuilder itemDiscount)
    {
        if (itemDiscount is FixedDiscountBuilder)
        {
            FixedDiscountRows.Add(itemDiscount as FixedDiscountBuilder);
        }
        else
        {
            RelativeDiscountRows.Add(itemDiscount as RelativeDiscountBuilder);
        }
        return this;
    }

    public override DeliverOrderBuilder AddFee(IRowBuilder itemFee)
    {
        if (itemFee is ShippingFeeBuilder)
        {
            ShippingFeeRows.Add(itemFee as ShippingFeeBuilder);
        }
        else
        {
            InvoiceFeeRows.Add(itemFee as InvoiceFeeBuilder);
        }
        return this;
    }

    public DeliverOrderBuilder SetCaptureDate(DateTime captureDate)
    {
        _captureDate = captureDate;
        return this;
    }

    public HostedActionRequest DeliverCardOrder()
    {
        var action = new Confirm(
            this.GetOrderId(),
            this._captureDate ?? DateTime.Now, // If no captureDate given, use today
            this._correlationId
        );

        var request = new HostedAdmin(this._config, this._countryCode);

        return request.Confirm( action );
    }
}