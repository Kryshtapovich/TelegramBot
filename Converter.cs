using NbrbAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TestBot.Configuration;

namespace TestBot
{
    class Converter
    {
        private readonly ConverterConfiguration configuration;

        public IEnumerable<Rate> CurrenciesRates => configuration.CurrenciesRates;

        public DateTime RatesDate => configuration.RatesDate;

        public string[] Currencies => configuration.Currencies;

        public string ExchangeRates { get; }

        public Converter(string jsonReference, string[] currencies)
        {
            configuration = new ConverterConfiguration(jsonReference, currencies);

            ExchangeRates = GetCurrencyRates();
        }

        private string GetCurrencyRates()
        {
            StringBuilder message = new StringBuilder($"Exchange rates according to NBRB for {RatesDate:d}:\n\n");

            foreach (var rate in CurrenciesRates)
            {
                message.Append($"{rate.Cur_Scale} {rate.Cur_Abbreviation} = {rate.Cur_OfficialRate} BYN\n\n");
            }
            return message.ToString();
        }

        public string CalculateRates(string firstCurrency, string secondCurrency, string sum)
        {
            try
            {
                var amount = decimal.Parse(sum);

                StringBuilder message = new StringBuilder("Calculating according to NBRB rates:\n\n");

                Func<string, bool> isByn = x => x == "BYN";

                var firstScale = isByn(firstCurrency) ? 1 :
                    CurrenciesRates.FirstOrDefault(x => x.Cur_Abbreviation == firstCurrency).Cur_Scale;

                var firstRate = isByn(firstCurrency) ? 1 :
                    CurrenciesRates.FirstOrDefault(x => x.Cur_Abbreviation == firstCurrency).Cur_OfficialRate;

                var secondScale = isByn(secondCurrency) ? 1 :
                    CurrenciesRates.FirstOrDefault(x => x.Cur_Abbreviation == secondCurrency).Cur_Scale;

                var secondRate = isByn(secondCurrency) ? 1 :
                    CurrenciesRates.FirstOrDefault(x => x.Cur_Abbreviation == secondCurrency).Cur_OfficialRate;

                var res = amount * (firstRate / firstScale) / (secondRate / secondScale);

                message.Append($"{amount} {firstCurrency} = {Math.Round((decimal)res, 4)} {secondCurrency}");

                return message.ToString();
            }
            catch (Exception)
            {
                return "Error";
            }
        }

    }
}
