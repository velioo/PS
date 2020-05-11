using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Logger.Logger;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            LogActivity("Test log 1");
            LogCurrentSessionActivities = true;
            LogActivity("Test log 2");

            try
            {
                throw new ArgumentNullException("Test exception");
            }
            catch (Exception e)
            {
                LogError(e.ToString());
            }

            //LogFileDir = "E:\\Games";
            //LogFileName = "test.log";

            LogActivity("Test log 3");

            Console.Write(GetCurrentSessionActivitiesAsString());

            Console.WriteLine("Log File: ");

            PrintLogFileToConsole();
        }
    }
}
