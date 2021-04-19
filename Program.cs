using System;
using System.IO;

namespace TestBot
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                CurrencyBot bot = new CurrencyBot();

                bot.Start();

                Console.ReadKey();

                bot.Stop();
            }
            catch (Exception ex)
            {
                using (var file = new StreamWriter("log.txt"))
                {
                    file.Write($"{ex.Message}\n{ex.StackTrace}");
                }
            }
        }
    }
}
