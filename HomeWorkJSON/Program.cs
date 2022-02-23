using Discord;
using Discord.WebSocket;
using Discord.Commands;
using HomeWorkJSON;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Newtonsoft.Json;

public class Program
{
	public static Task Main(string[] args) => new Program().MainAsync();

	private Task Log(LogMessage logMessage)
	{
        if (logMessage.Exception is CommandException cmdException)
        {
            // We can tell the user that something unexpected has happened
            cmdException.Context.Channel.SendMessageAsync("Something went catastrophically wrong!");

            // We can also log this incident
            Console.WriteLine($"{cmdException.Context.User} failed to execute '{cmdException.Command.Name}' in {cmdException.Context.Channel}.");
            Console.WriteLine(cmdException.ToString());
        }
        Console.WriteLine(logMessage.ToString());
		return Task.CompletedTask;
	}

    private DiscordSocketClient Client;
    private CommandService Commands;
    private CommandHandler CommandHandler;
    private ConfigObject ConfigObject;
    private readonly ServiceProvider _serviceProvider = ServiceProviderUtilities.ConfigureServices();
    public async Task MainAsync()
    {
        
        Client = new DiscordSocketClient(new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Debug
        });

        ConfigObject = JsonConvert.DeserializeObject<ConfigObject>(File.ReadAllText("test.txt"));

        Commands = new CommandService(new CommandServiceConfig
        {
            CaseSensitiveCommands = false,
            DefaultRunMode = RunMode.Async,
            LogLevel = LogSeverity.Debug
        });

        await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);

        CommandHandler = new CommandHandler(Client, Commands);

        Client.Log += Log;
        Client.Ready += Client_Ready;
        Client.MessageReceived += Client_MessageReceived;

        //  You can assign your bot token to a string, and pass that in to connect.
        //  This is, however, insecure, particularly if you plan to have your code hosted in a public repository.
        var token = ConfigObject.botToken;
        // Some alternative options would be to keep your token in an Environment Variable or a standalone file.
        // var token = Environment.GetEnvironmentVariable("NameOfYourEnvironmentVariable");
        // var token = File.ReadAllText("token.txt");
        // var token = JsonConvert.DeserializeObject<AConfigurationClass>(File.ReadAllText("config.json")).Token;

        await Client.LoginAsync(TokenType.Bot, token);
        await Client.StartAsync();

        // Block this task until the program is closed.
        await Task.Delay(-1);
    }
    private async Task Client_Ready()
    {
        await Client.SetGameAsync(ConfigObject.game, "https://www.google.com/");
    }
    private async Task Client_MessageReceived(SocketMessage MessageParam)
    {
        await CommandHandler.HandleCommandAsync(MessageParam);
    }
    public class ServiceProviderUtilities
    {
        public static ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<InfoModule>()
                .BuildServiceProvider();
        }
    }
}
public class ConfigObject
{
    public string botToken { get; set; }
    public string game { get; set; }    
}

/*
var client = new RestClient("https://restcountries.com/v3.1");
var request = new RestRequest("all");
var response = await client.GetAsync(request);
*/