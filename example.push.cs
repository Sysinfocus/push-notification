#:package Newtonsoft.Json@13.0.4
#:package WebPush@1.0.12

// Use DotNetRun dotnet tool to run this file.

using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using WebPush;

const string subject = "mailto:from@domain.com";
const string publicKey = "YOUR-VAPID-PUBLIC-KEY";
const string privateKey = "YOUR-VAPID-PRIVATE-KEY";

Dictionary<Guid, Subscription> subscriptions = [];
// subscriptions.Add(Guid.NewGuid(), new("", "", ""));

var options = new Dictionary<string, object>
{
  ["vapidDetails"] = new VapidDetails(subject, publicKey, privateKey)
};

var payload = JsonSerializer.Serialize(new
{
  notification = new
  {
    title = "Open Google!",
    body = "Start exploring the Google website today!",
    icon = "/logo.png",
    data = new
    {
      url = "https://google.com"
    }
  }
});

using var wpc = new WebPushClient();
try
{
  foreach (var sub in subscriptions)
  {
    Console.WriteLine($"Sending to '{sub.Key}'...");
    var subscription = new PushSubscription(sub.Value.Endpoints, sub.Value.Key, sub.Value.Auth);
    await wpc.SendNotificationAsync(subscription, payload, options);
  }
}
catch (WebPushException exception)
{
  Console.WriteLine("Http Status code: " + exception.StatusCode);
}

public record Subscription(string Endpoints, string Key, string Auth);


// VapidDetails vapidKeys = VapidHelper.GenerateVapidKeys();
// // Prints 2 URL Safe Base64 Encoded Strings
// Console.WriteLine("Public {0}", vapidKeys.PublicKey);
// Console.WriteLine("Private {0}", vapidKeys.PrivateKey);