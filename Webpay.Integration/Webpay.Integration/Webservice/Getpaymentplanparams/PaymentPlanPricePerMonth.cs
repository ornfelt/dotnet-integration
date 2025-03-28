﻿using Webpay.Integration.Exception;
using WebpayWS;

namespace Webpay.Integration.Webservice.Getpaymentplanparams;

public class PaymentPlanPricePerMonth
{
    public List<Dictionary<string, long>> Calculate(decimal amount, GetPaymentPlanParamsEuResponse paymentPlanParams)
    {
        var pricesPerMonth = new List<Dictionary<string, long>>();

        if (paymentPlanParams == null)
        {
            return pricesPerMonth;
        }

        foreach (var campaignCode in paymentPlanParams.CampaignCodes)
        {
            if (!(campaignCode.FromAmount <= amount) || !(amount <= campaignCode.ToAmount))
            {
                continue;
            }

            var priceMap = new Dictionary<String, long>();
            var numberOfPayments = Math.Max(1, campaignCode.ContractLengthInMonths - campaignCode.NumberOfPaymentFreeMonths);

            var paymentFactor = CalculatePaymentFactor(numberOfPayments, (double)campaignCode.InterestRatePercent / 100);

            var pricePerMonth = campaignCode.PaymentPlanType switch
            {
                PaymentPlanTypeCode.InterestAndAmortizationFree =>
                    Math.Round((double)campaignCode.InitialFee + (double)amount + (double)campaignCode.NotificationFee),

                PaymentPlanTypeCode.InterestFree =>
                    Math.Round(((double)campaignCode.InitialFee + (double)amount + ((double)campaignCode.NotificationFee * numberOfPayments)) / numberOfPayments),

                PaymentPlanTypeCode.Standard =>
                    Math.Round(((double)campaignCode.InitialFee + ((double)amount * paymentFactor + (double)campaignCode.NotificationFee) * numberOfPayments) / numberOfPayments),

                _ => throw new SveaWebPayException("Invalid PaymentPlanTypeCode")
            };

            priceMap.Add("campaignCode", campaignCode.CampaignCode);
            priceMap.Add("pricePerMonth", (long)pricePerMonth);
            pricesPerMonth.Add(priceMap);
        }

        return pricesPerMonth;
    }

    private double CalculatePaymentFactor(int numberOfPayments, double yearlyInterestRate, int paymentFrequencyPerYear = 12)
    {
        double monthlyInterestRate = yearlyInterestRate / paymentFrequencyPerYear;
        return monthlyInterestRate / (1 - Math.Pow(1 + monthlyInterestRate, -numberOfPayments));
    }
}