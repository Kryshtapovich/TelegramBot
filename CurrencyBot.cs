using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using TestBot.Configuration;

namespace TestBot
{
    class CurrencyBot
    {
        private readonly ITelegramBotClient botClient;

        private readonly Converter converter;

        private readonly BotConfiguration configuration;

        private readonly Keyboard keyboard;

        private (string curr1, string curr2, decimal sum, bool convert) operation;

        public CurrencyBot()
        {
            configuration = Configurator.GetConfiguration<BotConfiguration>("Config.json");

            botClient = new TelegramBotClient(configuration.BotToken);
            converter = new Converter(configuration.RatesReference, configuration.Currencies);

            keyboard = new Keyboard();
            keyboard.SetInlineButtons(configuration.Currencies);
            keyboard.SetReplyButtons(configuration.Commands[1][0], configuration.Commands[2][0]);

            botClient.OnMessage += MessageReceivedAsync;
            botClient.OnUpdate += OnUpdateAsync;
        }

        public void Start() => botClient?.StartReceiving();

        public void Stop() => botClient?.StopReceiving();

        private async void OnUpdateAsync(object sender, UpdateEventArgs args)
        {
            var update = args.Update;

            switch (update.Type)
            {
                case UpdateType.CallbackQuery:
                    {
                        var message = update.CallbackQuery.Data;
                        var chatId = update.CallbackQuery.Message.Chat.Id;

                        if (operation.curr1 != null && operation.curr2 == null && !operation.convert)
                        {
                            operation.curr2 = message;
                        }

                        if (operation.curr2 == null && !operation.convert)
                        {
                            operation.curr1 = message;
                        }

                        if (operation.curr2 != null && operation.curr1 != null && !operation.convert)
                        {
                            operation.convert = true;
                            await botClient.SendTextMessageAsync(chatId, "Enter the sum");
                        }
                        break;
                    }
            }
        }

        private async void MessageReceivedAsync(object sender, MessageEventArgs args)
        {
            var message = args.Message.Text;
            var chatId = args.Message.Chat.Id;

            if (configuration.Commands[0].Contains(message))
            {
                await botClient.SendTextMessageAsync(chatId, "Choose option",
                    replyMarkup: keyboard.ReplyKeyboard);

            }
            else if (configuration.Commands[1].Contains(message))
            {
                string exRates = converter.ExchangeRates;
                await botClient.SendTextMessageAsync(chatId, exRates);

            }
            else if (configuration.Commands[2].Contains(message))
            {
                await botClient.SendTextMessageAsync(chatId, "Choose 2 currencies",
                    replyMarkup: keyboard.InlineKeyboard);
            }
            else
            {
                if (operation.convert)
                {
                    var res = converter.CalculateRates(operation.curr1,
                        operation.curr2, message);

                    await botClient.SendTextMessageAsync(chatId, res);
                    operation = default;
                }
            }
        }
    }
}
