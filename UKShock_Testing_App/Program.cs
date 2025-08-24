//Test Code For Deving a UK Shocker Plugin
using OpenShock;
using PiShock;
using System.Formats.Tar;
using System.Reflection;

//check and set the API key
string FileResults = await StartupTasks.TokenChecks.OSTokenCheck();
if (FileResults != "OK") return;
//Console.ReadLine();

Console.WriteLine("""
    Enter A Choice To Procceed:

    1) Send A Beep to the Shockers
    2) Get A List Of Shockers
    3) Quit
    """);


int input = Convert.ToInt32(Console.ReadLine());
var OSUnits = await OpenShock.API.MakeList();
switch (input){
    case 1:
        Console.Clear();
        
        Console.WriteLine($"{OSUnits.Count} Shock Units Found");
        foreach (var unit in OSUnits)
        {
            Console.WriteLine($"Sending Command to the {unit.Name} Shocker");
            Console.WriteLine(await OpenShock.API.SendCommand(unit.ID, unit.Paused, "Sound", 100, 1000));
            Thread.Sleep(1000);
        }
        break;

    case 2:
        Console.Clear();
        Console.WriteLine(OSUnits.Count);
        foreach (var unit in OSUnits)
        { Console.WriteLine($"{unit.Name}, {unit.ID}"); }
        break;

    case 3:
        break;


}

public class StartupTasks()
{
    //Check for API Keys
    public class TokenChecks
    {
        public static async Task<List<string>> Retrieve()
        {
            var APIKeys = new List<string>();

            APIKeys.Add(await OSTokenCheck());
            APIKeys.Add(await PiTokenCheck());
            return APIKeys;


        }



        public static Task<string> OSTokenCheck()
        {

            string? OSAPIKey;
            string OSFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Conf/OpenShockAPI.conf");
            Console.WriteLine(OSFile);
            //String OSFile = "OpenShockAPI.conf";
            if (File.Exists(OSFile) == false) return Task.FromResult($"No OpenShock API File found at: {OSFile}");

            else
            {
                StreamReader sr = new StreamReader(OSFile);
                OSAPIKey = sr.ReadLine();
                if (OSAPIKey == null) return Task.FromResult($"""
                OpenShock API File Empty, Please add your UserID and API-Key to:
                {OSFile}
                """);
                //Read the first line of text
                OpenShock.API.Token = OSAPIKey;
                //Console.WriteLine(OSAPIKey);

                return Task.FromResult("OK");
            }
        }

    public static Task<string> PiTokenCheck()
    {
        string? PiAPIKey;
        string PiFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Conf/PiShockAPI.conf");
        Console.WriteLine(PiFile);
        //String OSFile = "OpenShockAPI.conf";
        if (File.Exists(PiFile) == false) return Task.FromResult($"""
            No PiShock API File Found at:
            {PiFile}
            """);

        else
        {
            StreamReader sr = new StreamReader(PiFile);
            PiAPIKey = sr.ReadLine();

            if (PiAPIKey == null) return Task.FromResult($"""
                PiShock API File Empty, please add your API Key to
                {PiFile}
                """);

            //Read the first line of text
            PiShock.API.Token = PiAPIKey;
            //Console.WriteLine(OSAPIKey);

            return Task.FromResult("OK");
        }


    }
    }





}
        

 
    
