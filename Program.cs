using System;
using System.IO;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace ValantDateString
{
   
    class Program
    {
        enum ExitCode : int
        {
            Success = 0,
            ProgramTerminated = 1,
            InvalidFilename = 2,
            EmptyFile = 3
        }

        public class Globals
        {
            public static int DateStringLength = 8;                     //length of input date text string in MMDDYYYY format
            public static DateTime StartDate = DateTime.Now.Date;       //get date at start of processing. It solves the date crossover at midnight issue      
        }
        static void Main(string[] args)
        {
            Console.Clear();
            string path = null;
            while(true) {
                Console.WriteLine("\nValantDateString - Enter path and file name or press <enter> to exit: ");
                path = Console.ReadLine();
                if (path.Length == 0)
                    Environment.Exit((int)ExitCode.ProgramTerminated);                    //Program terminated by operator
                if (File.Exists(path))
                { 
                    break;                                  //We found the file, let's go read it 
                }
                else
                {
                    Console.WriteLine("\nFile not found");
                }
            }
            
            //read the complete text file into a string array
            string[] inputDates = File.ReadAllLines(path, Encoding.UTF8);

            if (inputDates.Length == 0)
                Environment.Exit((int)ExitCode.EmptyFile);                    //Empty Input File - Exit

            //create a list for all lines of the input date text file
            List<string> goodDatesForOutput = new List<string>();


            foreach (string inputDate in inputDates)
            {

                if (ValidateDateString(inputDate, DateTime.Today))
                {
                    goodDatesForOutput.Add(inputDate);
                    //Console.WriteLine(dateTime);
                }
            }
            if (goodDatesForOutput.Count > 0)                                   //if we have good dates to write to file
            {
                //create the output directory 
                string pathOutputDirectory = Path.GetDirectoryName(path) + "\\Output";
                System.IO.Directory.CreateDirectory(pathOutputDirectory);
                File.WriteAllLines(pathOutputDirectory + "\\MarketingDataFile.txt", goodDatesForOutput, Encoding.UTF8);
                Console.WriteLine("\nDone! " + goodDatesForOutput.Count.ToString() + " Dates Written to output file. Press <enter> to exit");
                Console.ReadKey();
            }
        }

        static Boolean ValidateDateString(string inputDate, DateTime today)
        {
            Boolean returnval = false;
            DateTime dateTime;
            if (inputDate.Length != Globals.DateStringLength)              //only accept full MMDDYYYY dates
            {
                Console.WriteLine("  Invalid date length-" + inputDate);
            }
            else
            {
                if (!DateTime.TryParse(DateFormatter(inputDate), out dateTime))
                {
                    Console.WriteLine("  Invalid date-" + inputDate);
                }
                else
                {
                    if (!TodaysDateCheck(dateTime, Globals.StartDate))
                    {
                        Console.WriteLine("  Date >= today-" + inputDate);
                    }
                    else
                    {
                        returnval = true;                      //we passed all the checks
                    }
                }
            }
            return returnval;
        }
        static string DateFormatter(string inputDate)
        {        
            return inputDate.Substring(0, 2) + "/" + inputDate.Substring(2, 2) + "/" + inputDate.Substring(4, 4);
        }

        static Boolean TodaysDateCheck(DateTime dateTime, DateTime startdate)
        {
            Boolean returnVal = false;
            if (DateTime.Compare(dateTime, startdate) < 0)
                returnVal = true;
            return returnVal;

        }
    }
}
