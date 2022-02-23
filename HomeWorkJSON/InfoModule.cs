using Discord.Commands;
using HomeWorkJSON;
using RestSharp;
using HomeWorkJSON.ApiObjects;
using Discord;

namespace HomeWorkJSON
{
	// Create a module with no prefix
	public class InfoModule : ModuleBase<SocketCommandContext>
	{
		ApiClient client = new ApiClient("https://opentdb.com/");

		// ~say hello world -> hello world
		[Command("say")]
		[Summary("Echoes a message.")]
		public Task SayAsync([Remainder][Summary("The text to echo")] string echo)
			=> ReplyAsync(echo);
		
		[Command("trivia")]
		public async Task GetTriviaFromTriviaApi()
        {
			RestResponse response = await client.GetResponseAsync("");
			TriviaApiObjectList x = client.GetTriviaApiObjectFromJsonResponse(response.Content);
			x.ApiObjectList[0].PopulateAllAnswersList();
			x.ApiObjectList[0].AllAnswers.Shuffle();

			var embed = new EmbedBuilder();
			// Or with methods
			embed
				.WithAuthor($"Category: {x.ApiObjectList[0].Category}")
				.WithTitle($"{x.ApiObjectList[0].Question}")
				.WithCurrentTimestamp();
			switch(x.ApiObjectList[0].Difficulty)
            {
				case "easy":
					embed.WithColor(Color.Green);
					break;
				case "medium":
					embed.WithColor(Color.Orange);
					break;
				case "hard":
					embed.WithColor(Color.Red);
					break;
            }

			int asciiLetter = 97;
			foreach (var y in x.ApiObjectList[0].AllAnswers)
            {
				embed.AddField(Convert.ToChar(asciiLetter++).ToString() + ")", y);
            } // "Field value. I also support [hyperlink markdown](https://example.com)!")
			embed.AddField($"Correct answer:",
				$"||{Convert.ToChar(97 + x.ApiObjectList[0].AllAnswers.FindIndex(a => a.Contains(x.ApiObjectList[0].CorrectAnswer)))}||");

			//Your embed needs to be built before it is able to be sent
			await ReplyAsync(embed: embed.Build());




		//	await ReplyAsync(response.Content);
		}

		// ReplyAsync is a method on ModuleBase 
	}

	// Create a module with the 'sample' prefix
	/*
	[Group("sample")]
	public class SampleModule : ModuleBase<SocketCommandContext>
	{
		// ~sample square 20 -> 400
		[Command("square")]
		[Summary("Squares a number.")]
		public async Task SquareAsync(
			[Summary("The number to square.")]
		int num)
		{
			// We can also access the channel from the Command Context.
			await Context.Channel.SendMessageAsync($"{num}^2 = {Math.Pow(num, 2)}");
		}

		// ~sample userinfo --> foxbot#0282
		// ~sample userinfo @Khionu --> Khionu#8708
		// ~sample userinfo Khionu#8708 --> Khionu#8708
		// ~sample userinfo Khionu --> Khionu#8708
		// ~sample userinfo 96642168176807936 --> Khionu#8708
		// ~sample whois 96642168176807936 --> Khionu#8708
		[Command("userinfo")]
		[Summary
		("Returns info about the current user, or the user parameter, if one passed.")]
		[Alias("user", "whois")]
		public async Task UserInfoAsync(
			[Summary("The (optional) user to get info from")]
		SocketUser user = null)
		{
			var userInfo = user ?? Context.Client.CurrentUser;
			await ReplyAsync($"{userInfo.Username}#{userInfo.Discriminator}");
		}
	}
	*/
}
