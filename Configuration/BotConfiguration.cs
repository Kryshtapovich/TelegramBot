using System.Collections.Generic;

namespace TestBot.Configuration
{
    class BotConfiguration : Configuration
    {
        public string BotToken { get; set; }

        public string RatesReference { get; set; }

        public List<List<string>> Commands { get; set; }
    }
}
