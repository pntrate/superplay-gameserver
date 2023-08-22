using Serilog;
using Serilog.Events;
using SuperPlay.Game.Client;
using SuperPlay.Game.Domain.Models.Resources;

Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
                                      .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
                                      .CreateLogger();

try
{
    var client = new GameClient("wss://localhost:5277/ws", Log.Logger);
    var deviceId = Guid.NewGuid();

    client.OnLoggedIn += (result) =>
    {
        Log.Logger.Information("Player logged in: " + result.PlayerId);
    };
    client.OnResourcesUpdated += (result) =>
    {
        Log.Logger.Information("Resources updated. New balance: " + result.NewBalance);
    };

    client.OnGiftSent += (result) =>
    {
        Log.Logger.Information("Gift was successfully sent.");
    };

    client.OnGiftEvent += (result) =>
    {
        Log.Logger.Information($"New Gift received! From {result.SenderId}, resource type: {result.Resource!.Type}, resource value: {result.Resource.Balance}");
    };

    client.OnError += (error) =>
    {
        Log.Logger.Error($"Server returned error: {error.Text}");
    };

    await client.Connect();

    while (true)
    {
        Console.Write(@"
    Guids of Seeded players:
    1) id: 9e7cebc7-6961-4725-b5da-c2ae16319be8  deviceid: 89af07a5-75ca-4aa0-be9b-6b0a8686c2e5
    2) id: 35d6e601-1a24-47ab-ac6d-9a92928e4ef3  deviceid: 805987ab-5785-4609-9eca-43fb1305cd4f
    3) id: 62b8fe24-e0bf-4083-8566-b6886ee600a7  deviceid: 8b403820-f525-4bd1-bb92-544e62a63856

    Here are the list of commands available:
    1) login
    2) update-resource (type) (value)
    3) send-gift (playerId) (type) (value) ");

        Console.WriteLine();
        var input = Console.ReadLine();

        var inputParts = input.Split(" ");
        var cmd = inputParts[0];

        switch (cmd)
        {
            case "login":
                {
                    await client.Login(deviceId);
                    break;
                }
            case "update-resource":
                {
                    var resourceTypeStr = inputParts[1];
                    var resourceAmountStr = inputParts[2];

                    var resourceTypeParseResult = Enum.TryParse<ResourceType>(resourceTypeStr, true, out var resourceType);
                    if (!resourceTypeParseResult)
                    {
                        Console.WriteLine("Invalid resource type, value must be either: coins or rolls");
                    }

                    var resourceAmountParseResult = int.TryParse(resourceAmountStr, out var resourceAmount);

                    if (!resourceTypeParseResult)
                    {
                        Console.WriteLine("Invalid resource amount, should be integer number");
                    }

                    await client.UpdateResources(resourceType, resourceAmount);

                    break;
                }
            case "send-gift":
                {
                    var friendIdStr = inputParts[1];
                    var resourceTypeStr = inputParts[2];
                    var resourceAmountStr = inputParts[3];

                    var friendIdParseResult = Guid.TryParse(friendIdStr, out var friendId);

                    if (!friendIdParseResult)
                    {
                        Console.WriteLine("Invalid friend id, should be integer number");
                    }

                    var resourceTypeParseResult = Enum.TryParse<ResourceType>(resourceTypeStr, true, out var resourceType);
                    if (!resourceTypeParseResult)
                    {
                        Console.WriteLine("Invalid resource type, value must be either: coins or rolls");
                    }

                    var resourceAmountParseResult = int.TryParse(resourceAmountStr, out var resourceAmount);

                    if (!resourceTypeParseResult)
                    {
                        Console.WriteLine("Invalid resource amount, should be integer number");
                    }

                    await client.SendGift(friendId, resourceType, resourceAmount);

                    break;
                }
            default:
                {
                    Console.WriteLine("Unknown command");
                    break;
                }
        }

        await Task.Delay(500);
    }
}
catch (Exception ex)
{
    Log.Logger.Error(ex.ToString());
}