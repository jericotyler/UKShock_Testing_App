using System.Net.Http.Headers;
using System.Text.Json;

namespace OpenShock
{
    class API
    {
        public static string Token;
        
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
            client.DefaultRequestHeaders.Add("Open-Shock-Token", Token);

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
        
        public static async Task<string> GetShockers()
        {
            string ShockerJSON = "ERROR";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", APIUserAgent);
            client.DefaultRequestHeaders.Add("Open-Shock-Token", Token);
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
        
        public static async Task<List<MakeShocker>> MakeList()
        {
            string json = await OpenShock.API.GetShockers();
            var shockerUnits = new List<MakeShocker>();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var root = JsonSerializer.Deserialize<Root>(json, options);

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
                    shockerUnits.Add(new MakeShocker(shocker.Name, shocker.Id));
                }

            }


            return shockerUnits;

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
        
        public class MakeShocker
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
}
