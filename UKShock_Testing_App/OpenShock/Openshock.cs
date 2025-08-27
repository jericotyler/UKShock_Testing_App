using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;

namespace OpenShock
{
	class Config
	{
	public static string? Token;
	public static bool Enabled;
	public static List<string> ShockUnits;

        public static Task<string> TokenCheck()
        {

            string? Key;
            string KeyFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Conf/OpenShockAPI.conf");
            Console.WriteLine(KeyFile);
            if (File.Exists(KeyFile) == false)
            {
                OpenShock.Config.Enabled = false;
                return Task.FromResult($"No OpenShock API File found at: {KeyFile}");
            }
            else
            {
                StreamReader sr = new StreamReader(KeyFile);
                Key = sr.ReadLine();
                if (Key == null)
                {
                    OpenShock.Config.Enabled = false;
                    return Task.FromResult($"""
                OpenShock API File Empty, Please add your UserID and API-Key to:
                {KeyFile}
                """);
                }
                OpenShock.Config.Token = Key;
                OpenShock.Config.Enabled = true;
                return Task.FromResult("OK");
            }
        }

    }
    class API
    {





        public static async Task<string> SendCommand(string ID, bool paused, string command, int intensity = 0, float seconds = 0.3f)
        {
            string Address = "https://api.openshock.app/2/shockers/control";
            string Result;
            string ComID = ID;
            bool ComPaused = paused;
            string ComType = command;
            int ComInt = intensity;
            float miliseconds = seconds * 1000;
            int ComDur = (int)miliseconds;
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

            if (ComPaused == true) { return "Shocker Paused"; }
            Result = await API.CallAPI(Address, CommandJSON);
            return Result;
        }

        public static async Task<string> GetShockers()
        {
            string address = "https://api.openshock.app/1/shockers/own";
            string? ShockerJSON;
            ShockerJSON = await API.CallAPI(address);
            return ShockerJSON;

        }

        public static async Task<List<MakeShocker>> MakeList()
        {
            string json = await OpenShock.API.GetShockers();
            var shockerUnits = new List<MakeShocker>();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var root = JsonSerializer.Deserialize<ShockerVars.Root>(json, options);

            if (root?.Data == null)
            {
                Console.WriteLine("No data returned from API:");
                Console.WriteLine(json); // Debug: see actual JSON
                return null;
            }


            foreach (var dataItem in root.Data)
            {



                foreach (var shocker in dataItem.Shockers)
                {
                    shockerUnits.Add(new MakeShocker(shocker.Name, shocker.Id, shocker.IsPaused));
                }

            }


            return shockerUnits;

        }
        public class ShockerVars { 
            public class Root
            {
                public string? Message { get; set; }
                public List<DataItem>? Data { get; set; }
            }

            public class DataItem
            {
                public List<Shocker>? Shockers { get; set; }
                public string? Id { get; set; }
                public string? Name { get; set; }
                public DateTime CreatedOn { get; set; }
            }

            public class Shocker
            {
                public string? Name { get; set; }
                public bool IsPaused { get; set; }
                public DateTime CreatedOn { get; set; }
                public string? Id { get; set; }
                public int RfId { get; set; }
                public string? Model { get; set; }
            }
    }
        public class MakeShocker
        {
            public string Name;
            public string ID;
            public bool Paused;

            public MakeShocker(string ShockName, string ShockID, bool ShockPaused)
            {
                Name = ShockName;
                ID = ShockID;
                Paused = ShockPaused;

            }
        }
        public static async Task<string> CallAPI(string CallAddress, string CommandJSON = "")
        {

            string UserAgent = "UKShockMod/1.0 (ballshocker@gmail.com)";
            string Result;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            client.DefaultRequestHeaders.Add("Open-Shock-Token", OpenShock.Config.Token);
            string command = CommandJSON;
            var APIRequest = new HttpRequestMessage();
            if (command != "")
            {

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(CallAddress),
                    Content = new StringContent(CommandJSON)


                    {
                        Headers =
                    {
                        ContentType = new MediaTypeHeaderValue("application/json")

                    }
                    }
                };
                APIRequest = request;
            }
            else
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(CallAddress),
                };
                APIRequest = request;

            }

                using (var response = await client.SendAsync(APIRequest))
                {
                    //Console.WriteLine(APIRequest);
                    //Console.WriteLine(response);
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    Result = body;
                }
            return Result;
        }
    }
}
