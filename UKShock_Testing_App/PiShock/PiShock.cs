using System;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;

namespace PiShock
{
    public class Config
    {
        public static string? UserID;
        public static string? Token;
        public static bool Enabled;
        public static List<string> ShockUnits;

        public static Task<string> TokenCheck()
        {

            string? Key;
            string KeyFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Conf/PiShockAPI.conf");
            Console.WriteLine(KeyFile);
            //String OSFile = "OpenShockAPI.conf";
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
                //Read the first line of text
                PiShock.Config.Token = Key;
                PiShock.Config.Enabled = true;
                return Task.FromResult("OK");
            }
        }
        public class API
        {

        }
    }
}