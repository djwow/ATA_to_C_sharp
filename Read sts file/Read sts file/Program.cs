using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System.Text.RegularExpressions;
using OpenQA.Selenium.Winium;


namespace Read_sts_file
{
    class Program
    {
        public static string CMDFile = @"C:\\AtpTestlib\\HX5000\\lib\\dbprog\\source\\VTServer.cmd";
        public static string STSFile = @"C:\\AtpTestlib\\HX5000\\lib\\dbprog\\source\\VTServer.sts";
        public static string SEMFile = @"C:\\AtpTestlib\\HX5000\\lib\\dbprog\\source\\VTServer.sem";
        public static bool initDictionary = false;
        public static string SessionManager = "C:/Program Files/Mitel/MiVoiceOffice250/CS5000SessMngr.exe";
        public static Dictionary<string,string> Dic = new Dictionary<string, string>();
        public static Dictionary<string, string> SessionsDictionary= new Dictionary<string, string>();
        
        public static void StartWinium()
        {
            string PathToWinium = @"C:\\Winium.Desktop.Driver.exe";
            var runningProcessByName = Process.GetProcessesByName("Winium.Desktop.Driver");

            if (runningProcessByName.Length == 0)
            {
                Console.WriteLine("Start Winium driver");
                ProcessStartInfo startInfo = new ProcessStartInfo(PathToWinium);
                startInfo.WindowStyle = ProcessWindowStyle.Minimized;
                Process.Start(startInfo);
            }
        }

        public static int ConvertToTimestamp()
        {
            TimeSpan span = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 1, DateTimeKind.Local));
            return Convert.ToInt32(span.TotalSeconds);
        }

        public static string GETSTRING(string str) //get value from dictionary by key
        {
            try
            {
                return Dic[str].ToString();
            }
            catch
            {
                Console.WriteLine("Value does not found in dictionary");
                return "";
            }
        }

        public static bool LaunchDBP(string username, string password) //add session
        {
            bool DBPisReady = false;
            StartWinium();
            DesiredCapabilities dc = new DesiredCapabilities();
            dc.SetCapability("app", @"C:/Program Files/Mitel/MiVoiceOffice250/CS5000SessMngr.exe");
            dc.SetCapability("args", @"HX01 /USERNAME="+username+" /PASSWORD="+password);
            dc.SetCapability("launchDelay", "3000");

            Console.WriteLine("Start DBP");

            RemoteWebDriver driver = new RemoteWebDriver(new Uri("http://localhost:9999"), dc);
            //var driver = new RemoteWebDriver(new Uri("http://localhost:9999"), dc);
            /////////////////////
            //var window = driver.FindElementByName("HX01 - Session Starting"); //HX01 - Session Starting
            //var window = driver.FindElementsByClassName("ControlType.Window");
            Thread.Sleep(3000);
            var window = driver.FindElement(By.ClassName("#32770"));

            //var MainWindow = driver.FindElement(By.ClassName("ATL:0000000140044350"));

            /*
            Console.WriteLine("////////////");
            Console.WriteLine(window.GetAttribute("Name"));
            Console.WriteLine(sGETSTRING(window.GetAttribute("Name")));
            Console.WriteLine("////////////");
            Console.WriteLine(window.GetAttribute("Name").Replace("HX01 - ", ""));
            Console.WriteLine(sGETSTRING(window.GetAttribute("Name").Replace("HX01 - ", "")));
            //Console.WriteLine(GetKeyByValue(window.GetAttribute("Name")));
            */
            while (DBPisReady == false)
            {
               /* if (MainWindow.Displayed == true)
                {
                    DBPisReady = true;
                }*/
                window = driver.FindElement(By.ClassName("#32770"));
                switch (sGETSTRING(window.GetAttribute("Name").Replace("HX01 - ", ""))) //change to session
                {
                    case "IDS_DLG_DATABASE_PROGRAMMING":
                        {
                            Console.WriteLine("IDS_DLG_GENERAL_POPUP");
                            var text = window.FindElement(By.Id("65535"));
                            switch (sGETSTRING(text.GetAttribute("Name")))
                            {
                                case "IDS_DLG_DATABASE_PROGRAMMING_PRIMARY_ATTENDANT":
                                    {
                                        Console.WriteLine("IDS_DLG_DATABASE_PROGRAMMING_PRIMARY_ATTENDANT");
                                        window.FindElement(By.Name(GETSTRING("IDS_BTN_NO"))).Click();
                                        Thread.Sleep(3000);
                                        break;
                                    }
                                case "IDS_TXT_TIME_ZONE_MSG":
                                    {
                                        Console.WriteLine("IDS_TXT_TIME_ZONE_MSG");
                                        window.FindElement(By.Name(GETSTRING("IDS_BTN_OK"))).Click();
                                        Thread.Sleep(3000);
                                        break;
                                    }

                                default:
                                    {
                                        Console.WriteLine("default1");
                                        Thread.Sleep(3000);
                                        break;
                                    }
                            }
                            break;
                        }
                    case "IDS_DLG_GENERAL_POPUP":
                        {
                            Console.WriteLine("IDS_DLG_GENERAL_POPUP");
                            //window.FindElement(By.Name(GETSTRING("IDS_BTN_NO"))).Click();
                            Thread.Sleep(3000);
                            break;
                        }
                    case "IDS_DLG_SESSION_STARTING":
                        {
                            Console.WriteLine("IDS_DLG_SESSION_STARTING");
                            Thread.Sleep(3000);
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("default");
                            Thread.Sleep(3000);
                            break;
                        }
                }
            }
            Console.WriteLine("00000000000000000000000000000");
            Console.ReadLine();


            /*while (true)
            {
                try
                {
                    window = driver.FindElement(By.ClassName("#32770"));
                }
                catch
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("sleep0");
                }
                if (window != null)
                {
                    if (window.FindElement(By.Name("HX01 - Session Starting")) != null)
                    {
                        Thread.Sleep(2000);
                        Console.WriteLine("sleep1");
                    }


                    else if (window.FindElement(By.Name("Database Programming")) != null)
                    {
                        Console.WriteLine("sleep2");
                        window.FindElement(By.Name("No")).Click();
                    }

                    else
                    {
                        Console.WriteLine("Exit");
                        break;
                    }
                    /*Thread.Sleep(2000);
                    Console.WriteLine("sleep1");
                    try
                    {
                        window = driver.FindElementByName("HX01 - Session Starting");
                    }
                    catch
                    {
                        Console.WriteLine("Exit");
                        break;
                    }

                }

                else
                {
                    Console.WriteLine("wait");
                    Thread.Sleep(2000);
                }
            }
      /*      window = driver.FindElementByName("Database Programming");   //Database programming
            driver.FindElementByName("OK").Click();
            Thread.Sleep(2000);
            window = driver.FindElementByName("HX01 - Session Starting"); //HX1 - Session Starting
            while (window != null)
            {
                Thread.Sleep(2000);
                Console.WriteLine("sleep2");
                try
                {
                    window = driver.FindElementByName("HX01 - Session Starting");
                }
                catch
                {
                    Console.WriteLine("error");
                    break;
                }
            }
            try
            {
                List<Ele> wrkL = driver.findElements(By.className("ControlType.Window"));
                WebElement wrk1 = wrk.findElement(By.name("Database Programming"));
            }
            catch
            {
                Console.WriteLine("error0");
                break;
            }
           
            
            var winFinder = By.Name("Database Programming").AndType(ControlType.Window);
            var win = Winium.Cruciatus.CruciatusFactory.Root.FindElement(winFinder);
            win.FindElementByName("No").Click();
            */

            var window1 = driver.FindElement(By.Name("Database Programming"));
            window1.FindElement(By.Name("No")).Click();
            //driver.FindElementByName("No").Click();
            Thread.Sleep(2000);

            /*
            while (window != null)
            {
                Thread.Sleep(1000);
                Console.WriteLine("sleep2");
                try
                {
                    window = driver.FindElementByName("Database Programming");
                }
                catch
                {
                    Console.WriteLine("Exit");
                    break;
                }
            }
            */

            Console.WriteLine("Database Programming");
            driver.FindElementByName("No").Click();
            Thread.Sleep(2000);

           /* window = driver.FindElementByName("HX01 - Session Starting"); //HX1 - Session Starting
            while (window != null)
            {
                Thread.Sleep(2000);
                Console.WriteLine("sleep3");
                try
                {
                    window = driver.FindElementByName("HX01 - Session Starting");
                }
                catch
                {
                    Console.WriteLine("error");
                    break;
                }
            }
            */
            window = driver.FindElementByName("Database Programming"); //Database programming
            driver.FindElementByName("OK").Click();
            
            Thread.Sleep(2000);
            window = driver.FindElementByClassName("ATL:0000000140044350");  // HX1 - MiVoice Office 250 DB Programmin

            window.FindElement(By.Name("Operations")).Click();
            window.FindElement(By.Name("Backup Operations")).Click();
            window.FindElement(By.Name("Save Backup...")).Click();

            driver.Quit();
            Console.ReadLine();
            return true;
        }

        public static void EmulateVTServer()
        {
            FileStream fs = null;
            double sec = new System.TimeSpan().TotalSeconds;

            string sMessage = "";
            Thread.Sleep(3000);
            fs = new FileStream(SEMFile, FileMode.Open);
            Console.WriteLine("VO: Lock file");
            Thread.Sleep(1000);

            while (File.Exists(SEMFile))
            {
                if (File.Exists(CMDFile))
                {
                    Test();
                }
                else
                {
                    Thread.Sleep(1000);
                }

            }

            
            /*
            using (File.Create(STSFile)) { }
            Console.WriteLine("VO: Load database"); //[DATABASE]
            Thread.Sleep(1000);
            File.Delete(CMDFile);

            Thread.Sleep(2000);*/
            
            //Test(); 

            /*
            using (StreamWriter outputFile = new StreamWriter(STSFile, true))
            {
                Console.WriteLine("VO: Sync " + ConvertToTimestamp()); //[SYNC]
                outputFile.WriteLine(ConvertToTimestamp() + " SERVER: SYNC");
                Thread.Sleep(1000);
                File.Delete(CMDFile);
            }

            Thread.Sleep(1000);
            //******************************************************************************

            using (StreamWriter outputFile = new StreamWriter(STSFile, true))
            {
                Console.WriteLine("VO: Parce data for logining to DBP " + ConvertToTimestamp()); //Create definitions for sessions
                outputFile.WriteLine(ConvertToTimestamp() + " INFO: Configuring session HX01 HASVP key to 1");
                outputFile.WriteLine(ConvertToTimestamp() + " INFO: Configuring session HX01 USERNAME key to admin");
                outputFile.WriteLine(ConvertToTimestamp() + " INFO: Configuring session HX01 PASSWORD key to root123");
                Thread.Sleep(1000);
                File.Delete(CMDFile);
            }

            //code for parce command for DBP
            //******************************************************************************

            */


            //LaunchDBP("111", "111");


            
            //******************************************************************************


            Thread.Sleep(900000);
            fs.Close();
            Console.WriteLine("VO: Unlock file");

            /*while (true)
            {
                if (File.Exists(CMDFile ))
                {
                    try
                    {
                        using (StreamReader sr = new StreamReader(CMDFile))
                        {
                            //String line = sr.ReadToEnd();
                            //Console.WriteLine(line);
                            //sMessage = localDate.ToString() + " " + ;
                            Console.WriteLine("Read CMDFile file");

                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("The file could not be read:");
                        Console.WriteLine(e.Message);
                    }
                    Thread.Sleep(1000);
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }*/
        }

        
        public static void Test()
        {
            string CMDFile = @"C:\\temp\\VTServer.cmd";
            string command, line = " ";
            Match match;
            try
            {
                Console.WriteLine("Read CMDFile file");
                using (StreamReader sr = new StreamReader(CMDFile))
                {
                    while (true)
                    {
                        if (File.Exists(CMDFile))
                        {
                            line = sr.ReadLine();
                            Console.WriteLine(line);
                            if (line != null)
                            {
                                match = ParseString(line);
                                command = match.Groups["command"].Value.ToString();
                            }
                            else
                            {
                                break;
                            }
                            switch (command)
                            {
                                case "BEGINDATABASE":
                                    Console.WriteLine("!!!BEGINDATABASE");
                                    initDictionary = true;
                                    break;
                                case "ENDDATABASE":
                                    initDictionary = false;
                                    using (File.Create(STSFile))
                                    {
                                    }
                                    Console.WriteLine("VO: Load database"); //[DATABASE]
                                    sr.Close();
                                    Thread.Sleep(100);
                                    //File.Delete(CMDFile); //uncomment
                                    return;
                                case "ADDSTRING":
                                    Console.WriteLine("!!!ADDSTRING");
                                    if (initDictionary)
                                    {
                                        if (!Dic.ContainsKey(match.Groups["wparam"].Value.ToString())) ;
                                        Dic.Add(match.Groups["wparam"].Value.ToString(),
                                            match.Groups["lparam"].Value.ToString().Trim('"').Replace("\\x0a", "\r\n").Replace("\\x95", "•").Replace("\\x92", "’"));
                                    }
                                    break;
                                case "SYNC":
                                    Console.WriteLine("!!!SYNC");
                                    using (StreamWriter outputFile = new StreamWriter(STSFile, true))
                                    {
                                        Console.WriteLine("VO: Sync " + ConvertToTimestamp()); //[SYNC]
                                        outputFile.WriteLine(ConvertToTimestamp() + " SERVER: SYNC");
                                        sr.Close();
                                        Thread.Sleep(100);
                                        File.Delete(CMDFile);
                                    }
                                    return;
                                case "WRITE":
                                    Console.WriteLine("!!!WRITE");
                                    break;
                                case "CREATESESSION":
                                    Console.WriteLine("!!!CREATESESSION");
                                    if (!SessionsDictionary.ContainsKey(match.Groups["wparam"].Value.ToString())) ;
                                    {
                                        SessionsDictionary.Add(match.Groups["wparam"].Value.ToString(),
                                        match.Groups["lparam"].Value.ToString().Trim('"'));
                                    }
                                    sr.Close();
                                    Thread.Sleep(100);
                                    File.Delete(CMDFile);
                                    break;
                                case "STARTSESSION":
                                    Console.WriteLine("!!!STARTSESSION");
                                    match = ParseSession(match.Groups["lparam"].Value.ToString());
                                    LaunchDBP(match.Groups["username"].Value.ToString(), (match.Groups["password"].Value.ToString()));

                                    //File.Delete(CMDFile); //umcomment
                                    return;
                            }
                        }
                    }
                    sr.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            Thread.Sleep(1000);
            
        }


        static Match ParseString(String line)
        {
            Regex reg = new Regex(
                "\\u005b\\u0022(?<command>[A-Za-z0-9 _]+)\\u0022(,\\u0022(?<wparam>[^\\u0022]+)\\u0022)?(,(?<lparam>(\\u0022[^\\u0022]+\\u0022)|(\\u007b[^\\u007d]+\\u007d)))?\\u005d");
            Match match = reg.Match(line);
            return match;
        }

        static Match ParseSession(String line)
        {
            Regex regx = new Regex(
                "(\\u007b\\u0022[A-Za-z]+\\u0022[^\\u0022]+\\u0022(?<archive>[^\\u0022]+)\\u0022),(\\u0022[A-Za-z]+\\u0022[^\\u0022]+\\u0022(?<hasvp>[0-9]+)\\u0022),(\\u0022[A-Za-z]+\\u0022[^\\u0022]+\\u0022(?<language>[A-Za-z]+)\\u0022),(\\u0022[A-Za-z]+\\u0022[^\\u0022]+\\u0022(?<password>[A-Za-z0-9_]+)\\u0022),(\\u0022[A-Za-z]+\\u0022[^\\u0022]+\\u0022(?<product>[A-Za-z0-9_]+)\\u0022),(\\u0022[A-Za-z]+\\u0022[^\\u0022]+\\u0022(?<username>[A-Za-z0-9_]+)\\u0022)\\u007D");
            //       {      "    ARCHIVE     "       =>        "    C:/AXX/HX01/62/H62/USA  "    ,    "     HASVP      "       =>         "            1        "    ,     "  LANGUAGE   "       =>        "                  USA       "    ,    "    PASSWORD   "        =>       "                root123         "    ,     "   PRODUCT     "       =>       "          HX5000_6_2            "   ,     "  USERNAME   "        =>         "                admin          "        }
            Console.WriteLine(line);
            Match match = regx.Match(line);
            return match;
        }

        /*
        static Match ParseSession(String line)
        {
            Regex regx = new Regex(
                "(\\u007b\\u0022[A-Za-z0-9]+\\u0022[^\\u0022]+\\u0022(?<hasvp>[0-9]+)\\u0022),(\\u0022[A-Za-z0-9]+\\u0022[^\\u0022]+\\u0022(?<password>[A-Za-z0-9 _]+)\\u0022),(\\u0022[A-Za-z0-9]+\\u0022[^\\u0022]+\\u0022(?<session>[A-Za-z0-9 _]+)\\u0022),(\\u0022[A-Za-z0-9]+\\u0022[^\\u0022]+\\u0022(?<username>[A-Za-z0-9 _]+)\\u0022)\\u007D");
            Console.WriteLine(line);
            Match match = regx.Match(line);
            return match;
        }*/

        public static string sGETSTRING(string value) //get key from dictionary by value
        {
            foreach (var recordOfDictionary in Dic)
            {
                if (recordOfDictionary.Value.Equals(value))
                    return recordOfDictionary.Key;
            }
            return null;
        }

        public static void Main()
        {
            //EmulateVTServer();

            Test();
            Console.WriteLine("start sess");
            LaunchDBP("admin", "root123");
            //Console.ReadLine();
            
            //Console.WriteLine("dictionary");
            //Console.WriteLine(Dic["IDS_DLG_GENERAL_POPUP"].ToString());
            //Console.WriteLine("dictionary1111111");
            //Console.WriteLine(GETSTRING("IDS_DLG_DATABASE_PROGRAMMING_PRIMARY_ATTENDANT"));
            
            //Console.WriteLine(Dic.["IDS_DLG_GENERAL_POPUP"].ToString());
            //Console.ReadLine();
           
            //Console.ReadLine();
            //StartWinium();
            //EmulateVTServer();

        }
    }
}
