//Test Code For Deving a UK Shocker Plugin
using OpenShock;
using PiShock;

Console.WriteLine("""
    Enter A Choice To Procceed:

    1) Send A Beep to the Shockers
    2) Get A List Of Shockers
    3) Quit
    """);


int input = Convert.ToInt32(Console.ReadLine());
var OSUnits = await OpenShock.API.MakeList();
switch (input)
{
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

static string MakeConfig()
{
    string result = "Bleep";

    return result;
}




