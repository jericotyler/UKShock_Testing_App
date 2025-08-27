
﻿//Test Code For Deving a UK Shocker Plugin
using OpenShock;
using PiShock;
using System.Formats.Tar;
using System.Reflection;

//check and set the API key
string FileResults = await OpenShock.Config.TokenCheck();
if (FileResults != "OK") return;
//Console.ReadLine();
Console.Clear();

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
        Console.WriteLine($"""
Found {OSUnits.Count} units with the following Values:
------------------------------------------------------
Name			|Shocker ID
------------------------------------------------------
""");
        foreach (var unit in OSUnits)
        { Console.WriteLine($"{unit.Name}			|{unit.ID}"); }
	Console.WriteLine($"""
------------------------------------------------------
""");
        break;

    case 3:
        break;


}

class StartupTasks()
{
    //Check for API Keys
    public class TokenChecks
    {
        public static async Task<List<string>> Retrieve()
        {
            var APIKeys = new List<string>();

            APIKeys.Add(await OpenShock.Config.TokenCheck());
            APIKeys.Add(await PiShock.Config.TokenCheck());
            return APIKeys;


        }
         


    }
    }

    public class ConfigChecks
    {
        public static async Task OSConfig()
        {
        //Adding a comments so I can try git status on powerline
        //Adding a comment here to test if it will tell me when the repo has a change

            return;
        }

        public static async Task PiConfig()
        {

            return;
        }

    }
