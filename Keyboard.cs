using Telegram.Bot.Types.ReplyMarkups;

namespace TestBot
{
    class Keyboard
    {
        public InlineKeyboardMarkup InlineKeyboard { get; set; }

        public ReplyKeyboardMarkup ReplyKeyboard { get; set; }

        public void SetInlineButtons(params string[] buttonNames)
        {
            var buttons = new InlineKeyboardButton[buttonNames.Length];

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = new InlineKeyboardButton
                {
                    Text = buttonNames[i],
                    CallbackData = buttonNames[i]
                };
            }
            InlineKeyboard = new InlineKeyboardMarkup(buttons);
        }

        public void SetReplyButtons(params string[] buttonNames)
        {
            var buttons = new KeyboardButton[buttonNames.Length];
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = new KeyboardButton { Text = buttonNames[i] };
            }
            ReplyKeyboard = new ReplyKeyboardMarkup(buttons, true);
        }

    }
}
