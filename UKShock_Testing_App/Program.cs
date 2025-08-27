//Test Code For Deving a UK Shocker Plugin
using OpenShock;
using PiShock;
using System;
using System.Formats.Tar;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

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

bool validinput = false;
int input;
do
{
    if (int.TryParse(Console.ReadLine(), out input) && input >= 1 && input <= 3) validinput =true;
    else { Console.WriteLine("Invalid Selection"); }
}
while (validinput == false); ;

switch (input){
    case 1:
        await Commands.SendShockerCommands();
        break;

    case 2:
        await Commands.GetShockerList();
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

class ConfigChecks
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

class Commands
{
    public static async Task SendShockerCommands()
    {
            var OSUnits = await OpenShock.API.MakeList();
            Console.Clear();
            Console.WriteLine($"{OSUnits.Count} Shock Units Found");
            foreach (var unit in OSUnits)
            {
                bool ValidCom = false;
                bool ValidDur = false;
                bool ValidInt = false;
                string comType = "";
                int command;
                float comDur;
                int comInt;
                //Allow User to Select Command Type
                Console.WriteLine("""
                    Select a Command Type to Send:
                    ------------------------------
                    1) Beep
                    2) Vibration
                    3) Shock
                    ------------------------------
                    """);
                do
                {
                    if (int.TryParse(Console.ReadLine(), out command) && command >= 1 && command <= 3) ValidCom = true;
                    else { Console.WriteLine("Invalid Selection"); }
                }
                while (ValidCom == false);
                
                switch (command)
                {
                    case 1:
                        comType = "Sound";
                        break;
                    case 2:
                        comType = "Vibrate";
                        break;
                    case 3:
                        comType = "Shock";
                        break;

                }

                //Get and Check that Intensity is correct
                //Need to make it where this won't run if the command is beep
                Console.WriteLine($"Please enter Command Intensity ( 0 - 100 )");
                do
                {
                    if (int.TryParse(Console.ReadLine(), out comInt) && comInt >= 0 && comInt <= 100) { ValidInt = true;}
                    else { Console.WriteLine("Invalid Number"); }
                }
                while (ValidInt == false);


                //Get and Check that Duration is correct
                Console.WriteLine($"Please enter Command Duration in Seconds (0.3 to 60)");
                do
                {
                    if (float.TryParse(Console.ReadLine(), out comDur) && comDur >= 0.3f && comDur <= 60f) ValidDur = true;
                    else { Console.WriteLine("Invalid Number"); }
                }
                while (ValidDur == false);
                //Send The Command To the Shocker
                Console.WriteLine($"Sending Command to the {unit.Name} Shocker");
                Console.WriteLine(await OpenShock.API.SendCommand(unit.ID, unit.Paused, comType, comInt, comDur));
                await Task.Delay(1000);
            }
            return;      
    }
    public static async Task GetShockerList()
    {
        var OSUnits = await OpenShock.API.MakeList();
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
        return;
    }
}