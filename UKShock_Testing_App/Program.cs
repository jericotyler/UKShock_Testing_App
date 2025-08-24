//Test Code For Deving a UK Shocker Plugin
using OpenShock;
using PiShock;
using System.Formats.Tar;
using System.Reflection;

//check and set the API key
string FileResults = await StartupTasks.APIKeyCheck();
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
            await OpenShock.API.SendCommand(unit.ID, "Sound", 100, 1000);
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
    
    public static Task<string> APIKeyCheck()
    {
        string PiShockKey = "";
        PiShock.API.Token = PiShockKey;
        string OSAPIKey;
        string OSFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Conf\OpenShockAPI.conf");
        Console.WriteLine(OSFile);
        //String OSFile = "OpenShockAPI.conf";
        if (File.Exists(OSFile) == false) return Task.FromResult("No File");

        else
        {
            StreamReader sr = new StreamReader(OSFile);
            OSAPIKey = sr.ReadLine();
            if (OSAPIKey == null) return Task.FromResult("File Empty");
                //Read the first line of text
                OpenShock.API.Token = OSAPIKey;
                //Console.WriteLine(OSAPIKey);
            
            return Task.FromResult("OK");
        }


    }
    
    //Check for Configs
}

