//Test Code For Deving a UK Shocker Plugin
using System.Net.Http.Headers;
using System.Text.Json;

//Shocker GreenShocker = new("Green", "0197463d-3da3-76ea-830b-54e465ebbb59");


Console.WriteLine("""
    Enter A Choice To Procceed:

    1) Send A Beep to the Shockers
    2) Get A List Of Shockers
    3) Quit
    """);


int input = Convert.ToInt32(Console.ReadLine());
switch (input)
{
    case 1:
        Console.Clear();
        //Console.WriteLine("Sending a Beep to the Shockers");
        //await OpenShock.SendCommand(GreenShocker.ID, "Sound", 100, 1000);
        Console.WriteLine($"Sending a Beep to the Green Shocker");
        await OpenShock.SendCommand(OpenShock.OrangeShockerID, "Sound", 100, 1000);
        Console.WriteLine("Sending a Beep to the Orange Shocker");
        await OpenShock.SendCommand(OpenShock.BlackShockerID, "Sound", 100, 1000);
        Console.WriteLine("Sending a Beep to the Black Shocker");
        break;

    case 2:
        Console.Clear();
        await OpenShock.MakeList();
        break;

    case 3:
        break;


}
    


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
        string json = await OpenShock.GetShockers();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var root = JsonSerializer.Deserialize<Root>(json, options);

        if (root?.Data == null)
        {
            Console.WriteLine("No data returned from API:");
            Console.WriteLine(json); // Debug: see actual JSON
            return;
        }

        foreach (var dataItem in root.Data)
        {
            var shockerUnits = new List<MakeShocker>();

            foreach (var shocker in dataItem.Shockers)
            {
                shockerUnits.Add(new MakeShocker(shocker.Name, shocker.Id));
            }
            Console.WriteLine(shockerUnits.Count);
            foreach (var unit in shockerUnits)
            {
                Console.WriteLine($"{unit.Name} -> {unit.ID}");
            }
        }

        //Array ShockerList;
        //string jsonfile = await OpenShock.GetShockers();
        //ShockerList = JsonSerializer.Deserialize<Array>(await OpenShock.GetShockers());
        //Console.WriteLine(ShockerList);
        //Console.WriteLine(jsonfile);

        return;

    }

    public class Root
    {
        public string Message { get; set; }
        public List<DataItem> Data { get; set; }
    }

    public class DataItem
    {
        public List<Shocker> Shockers { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class Shocker
    {
        public string Name { get; set; }
        public bool IsPaused { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Id { get; set; }
        public int RfId { get; set; }
        public string Model { get; set; }
    }
    class MakeShocker
    {
        public string Name;
        public string ID;
        public MakeShocker(string ShockName, string ShockID)
        {
            Name = ShockName;
            ID = ShockID;
        }
    }
}

 


class PiShock
{

}

