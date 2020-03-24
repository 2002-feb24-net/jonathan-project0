using System;
using System.IO;
using Microsoft.Extensions.Configuration;

using Manager;
using WheyMen.DAL;


namespace Main
{
    class Program
    {
        static OrderMgr ordMgr;
        static LocationMgr locMgr;
        static CustomerMgr custMgr;

        private static IConfiguration _iconfiguration;
        static void GetAppSettingsFile()
        {
            var builder = new ConfigurationBuilder()
                                 .SetBasePath(Directory.GetCurrentDirectory())
                                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _iconfiguration = builder.Build();
        }
 
        static void EnterControlLoop()
        {
            custMgr.Login();
            while (true)
            {
                Console.WriteLine("\n\n");
                Console.WriteLine("1:Order Options\n2:Customer Options\n3:Location Options");
                Console.Write("Enter the number for your choice:");
                int choice = Wrappers.ReadInt();
                Console.WriteLine($"{choice}\n\n");
                switch(choice)
                {
                    case 1: 
                        ordMgr.DisplayOptions();
                        break;
                    case 2: 
                        custMgr.DisplayOptions();
                        break;
                    case 3: 
                        locMgr.DisplayOptions();
                        break;
                 
                    default:
                        Console.WriteLine("Please enter a valid option");
                        continue;
                }
            }
        }
        static void InitManagers()
        {
            locMgr = new LocationMgr(_iconfiguration);
            ordMgr = new OrderMgr(_iconfiguration,locMgr);
            custMgr = new CustomerMgr(_iconfiguration,locMgr,ordMgr);
            ordMgr.SetCm(custMgr);
        }
        static void Main(string[] args)
        {
            GetAppSettingsFile();
            InitManagers();
            EnterControlLoop();
        }
    }
}
