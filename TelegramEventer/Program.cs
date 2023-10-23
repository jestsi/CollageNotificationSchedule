using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using File = System.IO.File;

namespace TelegramEventer;

public class BotTelegram
{
    public static TelegramBotClient Bot { get; set; } = new("6488423676:AAFgXcgpUGXettS7SCywNCYaViXHf89lzv0");
    public static async void RunBot()
    {
        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { }, // receive all update types
        };
        Bot.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken
        );
        Console.WriteLine("Running");
        Console.ReadLine();
    }

    public static void SendMessage(long chatId, string message)
    {
        Bot.SendTextMessageAsync(chatId, message);
    }
    public static async Task SendPost(long chatId, string group, DateTime time)
    {
        
        var clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        
        var client = new HttpClient(clientHandler);
        var values = new Dictionary<string, string>
        {
            { "chatId", $"{chatId}" },
            { "group", $"{group}" },
            { "time", $"{time}" },
        };

        var content = new FormUrlEncodedContent(values);

        var response = await client.PostAsync(
            $"https://localhost:7288/api/BotScheduleApi/add?group={group}&time={time:t}&chatId={chatId}",null);
    }

    public static string Group { get; set; }
    public static DateTime Time { get; set; }
    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if(update.Type == UpdateType.Message)
        {
            var message = update.Message;
            
            var messageText = message.Text.ToLower();
            if (messageText == "/start")
            { 
                SendMessage(message.Chat.Id, "Здравствуйте, это GestScheduleBot. Пожалуйста укажите вашу группу коммандой /group <группа>");
            } else if (messageText.Contains("/group"))
            {
                Group = messageText.Split(' ').ElementAtOrDefault(1) ?? ( new Func<string>( () => {
                    SendMessage(message.Chat.Id, "Введите пожайлуста корректно группу");
                    return "КИС-2229";
                }).Invoke());
                SendMessage(message.Chat.Id, "Принято. Пожалуйста, введите время когда вам присылать расписание коммандой /time <время>");
            } else if (messageText.Contains("/time"))
            {
                Time = DateTime.Parse(messageText.Split(' ')[1]);
                SendMessage(message.Chat.Id, "Благодарю, теперь включите их с коммандой /schedule");
            } else if (messageText.Contains("/schedule"))
            {
                await SendPost(message.Chat.Id, Group, Time);
            }
        }
    }
    
    public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var exceptFormated = DateTime.Now.ToString("u") + Newtonsoft.Json.JsonConvert.SerializeObject(exception) +
                             "\n\n";
        Console.WriteLine(exceptFormated);
        await File.AppendAllTextAsync("./Logging/LoggerExceptionsBot.txt",
            exceptFormated, cancellationToken);
    }
    
    public static void Main(string[] args)
    {
        RunBot();
    }
}