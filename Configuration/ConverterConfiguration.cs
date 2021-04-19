using NbrbAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace TestBot.Configuration
{
    class ConverterConfiguration : Configuration
    {
        public IEnumerable<Rate> CurrenciesRates { get; private set; }

        public DateTime RatesDate { get; private set; }

        public ConverterConfiguration(string jsonReference, string[] currencies)
        {
            SetConfiguration(jsonReference, currencies);
        }

        public void SetConfiguration(string jsonReference, string[] currencies)
        {
            Currencies = new string[currencies.Length];
            currencies.CopyTo(Currencies, 0);

            var jsonString = new WebClient().DownloadString(new Uri(jsonReference));

            CurrenciesRates = JsonConvert.DeserializeObject<IEnumerable<Rate>>(jsonString)
                                            .ToList().FindAll(x => currencies.Contains(x.Cur_Abbreviation));

            RatesDate = CurrenciesRates.FirstOrDefault().Date;
        }
    }
}
