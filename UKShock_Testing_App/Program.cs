// See https://aka.ms/new-console-template for more information
using System.Net.Http.Headers;

Console.WriteLine("Sending a Beep to the Shockers");

Shocker GreenShocker = new Shocker("Green", "0197463d-3da3-76ea-830b-54e465ebbb59");


await OpenShock.SendCommand(GreenShocker.ID,"Sound",100,1000);
Console.WriteLine($"Sending a Beep to the Green Shocker");
await OpenShock.SendCommand(OpenShock.OrangeShockerID, "Sound",100,1000);
Console.WriteLine("Sending a Beep to the Orange Shocker");
await OpenShock.SendCommand(OpenShock.BlackShockerID, "Sound",100,1000);
Console.WriteLine("Sending a Beep to the Black Shocker");
//await OpenShock.GetShockers();
class OpenShock
{   
    static public string GreenShockerID = $"0197463d-3da3-76ea-830b-54e465ebbb59";
    static public string OrangeShockerID = $"01974644-8d29-7006-a33f-e4fee8e5b906";
    static public string BlackShockerID = $"01974645-862c-7e98-9036-3d7aa3447e7b";

    static string APIToken = "ynSwrRwrG2lzQQp7IoVRZd7tESK3B1XaLV82dqBD312fOO6kVFOX5oHWnDeQjOLs";
    static string APIUserAgent = "UKShockMod/1.0 (migratorycreatuesllc@gmail.com)";
    public static async Task SendCommand(string a = "Null", string b = "Stop", int c = 0, int z = 300)
    {
        string Result;
        string ComID = a;
        string ComType = b;
        int ComInt = c;
        int ComDur = z;
        string CommandJSON = $$"""
    {
  "shocks": [
    {
      "id": "{{ComID}}",
      "type": "{{ComType}}",
      "intensity": {{ComInt}},
      "duration": {{ComDur}},
      "exclusive": true
    }
  ],
  "customName": null
}
""";


        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", APIUserAgent);
        client.DefaultRequestHeaders.Add("Open-Shock-Token", APIToken);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("https://api.openshock.app/2/shockers/control"),
            Content = new StringContent(CommandJSON)


            {
                Headers =
        {
            ContentType = new MediaTypeHeaderValue("application/json")

        }
            }
        };

        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            //Console.WriteLine(body);
            Result = body;
        }
        return;
    }
    public static async Task <string> GetShockers()
    { 
        string ShockerJSON = "ERROR";
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", APIUserAgent);
        client.DefaultRequestHeaders.Add("Open-Shock-Token", APIToken);
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://api.openshock.app/1/shockers/own"),
        };
        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            ShockerJSON = body;
            
        }

        return ShockerJSON;
        
        //Console.Write("Don't Read This");
    }

    public static async Task MakeList()
    {
        string jsonfile = await OpenShock.GetShockers();
        Shocker GreenShockerID = new Shocker("Green", "0197463d-3da3-76ea-830b-54e465ebbb59");
    }
}
 class Shocker
{
    public string Name;
    public string ID;
    public Shocker(string ShockName,string ShockID)
    {
        Name = ShockName;
        ID = ShockID;
    }
}
 
class PiShock
{

}
