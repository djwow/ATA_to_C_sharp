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
using OpenQA.Selenium.Interactions;


namespace Read_sts_file
{
    class Program
    {
        public static string CMDFile = @"C:\\AtpTestlib\\HX5000\\lib\\dbprog\\source\\VTServer.cmd";
        public static string STSFile = @"C:\\AtpTestlib\\HX5000\\lib\\dbprog\\source\\VTServer.sts";
        public static string SEMFile = @"C:\\AtpTestlib\\HX5000\\lib\\dbprog\\source\\VTServer.sem";
        public static bool initDictionary = false;
        public static int windowsCount = 0;
        public static string SessionManager = "C:/Program Files/Mitel/MiVoiceOffice250/CS5000SessMngr.exe";
        public static Dictionary<string,string> Dic = new Dictionary<string, string>();
        public static Dictionary<string, string> SessionsDictionary= new Dictionary<string, string>();
        public static Dictionary<string, string> WriteCommandDictionary = new Dictionary<string, string>();
        public static RemoteWebDriver driver = null;
        
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
                return null;
            }
        }

        public static string sGETSTRING(string value) //get key from dictionary by value
        {
            foreach (var recordOfDictionary in Dic)
            {
                if (recordOfDictionary.Value.Equals(value))
                    return recordOfDictionary.Key;
            }
            return null;
        }

        public static IList<IWebElement> GETListOfWindows()
        {
            return driver.FindElements(By.XPath("/*[contains(@ControlType,'ControlType.Window') and not (contains(@Name,'perl.exe'))]"));
        }

        public static bool GETListOfPath(IWebElement Window, string Name)
        {
            try
            {

                if (Window.FindElement(By.ClassName("ToolbarWindow32")).FindElement(By.XPath("/*[contains(@Name,'" + Name + "')]")) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public static IList<IWebElement> GETListOfDialogs()
        {
            return driver.FindElements(By.ClassName("#32770"));
        }

        public static void CountWindows (RemoteWebDriver driver, string session)
        {
            if (windowsCount < GETListOfWindows().Count)
            {
                Console.WriteLine(windowsCount);

                CommonProc.WindowsHandle(session);

                windowsCount = GETListOfWindows().Count;
            }
            else
            {
                windowsCount = GETListOfWindows().Count;
            }
        }

        public static void WriteToSTS (string message)
        {
            using (StreamWriter outputFile = new StreamWriter(STSFile, true))
            {
                outputFile.WriteLine(ConvertToTimestamp() + " " + message);
                Thread.Sleep(100);
                //outputFile.Close();
                //File.Delete(CMDFile);
            }
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

          
            //Thread.Sleep(900000);
            fs.Close();
            Console.WriteLine("VO: Unlock file");
            driver.Quit();
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
            //////////////////////////////////////////////////////////////comment
            
            StartWinium();
            /*
            DesiredCapabilities dc = new DesiredCapabilities();
            dc.SetCapability("app", SessionManager);
            dc.SetCapability("args", @" HX01  /USERNAME=admin /PASSWORD=root123");
            dc.SetCapability("launchDelay", "3000");

            Console.WriteLine("Start DBP");

            driver = new RemoteWebDriver(new Uri("http://localhost:9999"), dc);
            Thread.Sleep(5000);*/
            //string CMDFile = @"C:\\temp\\VTServer.cmd";
             
            ///////////////////////////////////////////////////////////////

            
            string command, parameters, line = " ";
            IWebElement Window, SysListView32 = null;
            Match match, cell, MatchSysTreeView32;
            string row = "";
            try
            {
                Console.WriteLine("Read CMDFile file");
                using (StreamReader sr = new StreamReader(CMDFile))
                {
                    while (true)
                    {
                        if (File.Exists(CMDFile))
                        {
                            try
                            {
                                line = sr.ReadLine();
                            }
                            catch
                            {
                                Thread.Sleep(100);
                                break;
                            }
                            Console.WriteLine(line);
                            if (line != null)
                            {
                                match = ParseString(line);
                                command = match.Groups["command"].Value.ToString();
                                parameters = match.Groups["wparam"].Value.ToString();
                                if (command == "")
                                {
                                    command = ParseWrite(line).Groups["command"].Value.ToString();
                                }
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
                                    sr.Close(); //uncomment
                                    Thread.Sleep(100);
                                    File.Delete(CMDFile); //uncomment
                                    break;//return;
                                case "ADDSTRING":
                                    Console.WriteLine("!!!ADDSTRING");
                                    if (initDictionary)
                                    {
                                        if (!Dic.ContainsKey(match.Groups["wparam"].Value.ToString())) 
                                        {
                                        Dic.Add(match.Groups["wparam"].Value.ToString(), match.Groups["lparam"].Value.ToString().Trim('"').Replace("\\x0a", "\r\n").Replace("\\x95", "•").Replace("\\x92", "’"));
                                        }
                                    }
                                    break;
                                case "SYNC":
                                    Console.WriteLine("!!!SYNC");
                                    WriteToSTS ("SERVER: SYNC");
                                    sr.Close();
                                    Thread.Sleep(100);
                                    File.Delete(CMDFile);
                                    return;
                               case "RESET":
                                    Console.WriteLine("!!!RESET");
                                    WriteToSTS("INFO: Resetting session " + parameters);
                                    WriteToSTS("SERVER: EOF");
                                    sr.Close();
                                    File.Delete(CMDFile);
                                    break;
                                case "WRITE":
                                    driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(3));
                                    Console.WriteLine("!!!WRITE");
                                    ParseWriteCommand(line);
                                    
                                    Window = driver.FindElement(By.XPath("/*[contains(@ControlType,'ControlType.Window') and (contains(@Name,'"+parameters+" - "+Program.GETSTRING("IDS_DLG_MITEL_DB_PROGRAMMING")+"'))]")); //hx01 change to parameters
                                    string previuskey = "", previusvalue = "";
                                    foreach (var y in WriteCommandDictionary)
                                            {
                                                if (previusvalue != y.Value.ToString() && previusvalue != "")
                                                {
                                                    previusvalue = y.Value.ToString();
                                                    break;
                                                }
                                                else
                                                {
                                                    previusvalue = y.Value.ToString();
                                                    previuskey = y.Key.ToString();
                                                }
                                            }
                                    string path = "";
                                    foreach (KeyValuePair<string, string> item in WriteCommandDictionary)  //add verify path on top window
                                    {
                                        
                                        Console.WriteLine(item.Key);
                                        switch (item.Value)
                                            {
                                                case "SubView":
                                                    Console.WriteLine("SubView");
                                                    //CommonProc.WindowsHandle(parameters);
                                                    IWebElement SysTreeView32 = Window.FindElement(By.ClassName("SysTreeView32"));
                                                    Console.WriteLine("Current: "+item.Key.ToString());
                                                    path = path + "\\" + GETSTRING(item.Key);
                                                    //IList<IWebElement> ToolbarButtons = GETListOfPath();
                                                    if (!GETListOfPath(Window,GETSTRING(item.Key)))
                                                    {

                                                        if (item.Key.ToString() == previuskey)
                                                        {
                                                            SysTreeView32.FindElement(By.Name(GETSTRING(item.Key))).Click();
                                                            WriteToSTS(path);
                                                        }
                                                        else
                                                        {
                                                            new Actions(driver).DoubleClick(SysTreeView32.FindElement(By.Name(GETSTRING(item.Key)))).Perform();
                                                            if (GETListOfDialogs().Count > 0)
                                                            {
                                                                CommonProc.WindowsHandle(parameters);
                                                                new Actions(driver).DoubleClick(SysTreeView32.FindElement(By.Name(GETSTRING(item.Key)))).Perform();
                                                            }

                                                        }
                                                    }
                                                    break;
                                                case "Row":
                                                    Window.Click();
                                                    SysListView32 = Window.FindElement(By.ClassName("SysListView32"));
                                                    Console.WriteLine("Row");
                                                    IList<IWebElement> rows = SysListView32.FindElements(By.Name(GETSTRING(item.Key)));
                                                    Thread.Sleep(500);
                                                    foreach (IWebElement y in rows)
                                                    {
                                                         if (y.GetAttribute("LocalizedControlType").ToString() == "text")
                                                        {
                                                            row = y.GetAttribute("BoundingRectangle").ToString();
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("No such elements");
                                                        }
                                                    }
                                                    row = ParseCoordinates(row).Groups[2].ToString();
                                                    break;
                                                case "Cell":
                                                    Console.WriteLine("Cell");
                                                    SysListView32 = Window.FindElement(By.ClassName("SysListView32"));
                                                    Console.WriteLine(GETSTRING(item.Key));
                                                    Window.Click();
                                                    IWebElement col = SysListView32.FindElement(By.Name(GETSTRING(item.Key)));
                                                    cell = ParseCoordinates(col.GetAttribute("BoundingRectangle").ToString());
                                                    MatchSysTreeView32 = ParseCoordinates(SysListView32.GetAttribute("BoundingRectangle").ToString());
                                                    new Actions(driver).MoveToElement(SysListView32, ((Convert.ToInt32(cell.Groups[1].ToString()) - Convert.ToInt32(MatchSysTreeView32.Groups[1].ToString())) + Convert.ToInt32(cell.Groups[3].ToString()) / 2), ((Convert.ToInt32(row) - Convert.ToInt32(MatchSysTreeView32.Groups[2].ToString())) + Convert.ToInt32(cell.Groups[4].ToString()) / 2)).Click().Perform();
                                                    Thread.Sleep(1000);
                                                    IWebElement res = driver.SwitchTo().ActiveElement();
                                                    Console.WriteLine(res.GetAttribute("Name"));
                                                    WriteToSTS(res.GetAttribute("Name"));
                                                    Thread.Sleep(100);
                                                    WriteToSTS("SERVER: EOF");
                                                    sr.Close();
                                                    File.Delete(CMDFile);
                                                    break;
                                                default:
                                                    Console.WriteLine("Write -> default");
                                                    break;
                                            }
                                        }
                                    WriteCommandDictionary.Clear();
                                    sr.Close();
                                    File.Delete(CMDFile); //umcomment
                                    break;
                                case "CREATESESSION":
                                    Console.WriteLine("!!!CREATESESSION");
                                    if (!SessionsDictionary.ContainsKey(match.Groups["wparam"].Value.ToString())) ;
                                    {
                                        SessionsDictionary.Add(match.Groups["wparam"].Value.ToString(),
                                        match.Groups["lparam"].Value.ToString().Trim('"'));
                                    }
                                    sr.Close();
                                    WriteToSTS("SERVER: INFO: Configuring session" + match.Groups["wparam"].Value.ToString() + "HASVP key to 1");
                                    Thread.Sleep(100);
                                    File.Delete(CMDFile);
                                    break;
                                case "STARTSESSION":
                                    Console.WriteLine("!!!STARTSESSION");
                                    match = ParseSession(match.Groups["lparam"].Value.ToString());
                                    CommonProc.LaunchDBP(match.Groups["username"].Value.ToString(), match.Groups["password"].Value.ToString(), parameters);
                                    sr.Close();
                                    File.Delete(CMDFile); //umcomment
                                    Thread.Sleep(100);
                                    break;
                               case "STOPSESSION":
                                    Console.WriteLine("!!!STOPSESSION");
                                    driver.FindElementById("Close").Click();
                                    sr.Close();
                                    File.Delete(CMDFile); //umcomment
                                    break;
                               case "SETSESSION":
                                    Console.WriteLine("!!!SETSESSION");
                                    //driver.FindElement(By.Name(parameters + " - " + GETSTRING("IDS_DLG_MITEL_DB_PROGRAMMING"))).Click(); //switch to window???
                                    break;
                               case "ECHO":
                                    Console.WriteLine("!!!ECHO");
                                    switch (parameters)
                                    {
                                        case "Synchronize":
                                            Console.WriteLine("Echo -> Synchronize");
                                            WriteToSTS("Synchronize");
                                            WriteToSTS("SERVER: EOF");
                                            Console.WriteLine("writeSTS");
                                            sr.Close();
                                            File.Delete(CMDFile); //umcomment
                                            Console.WriteLine("File delete");
                                            break;
                                        default:
                                            Console.WriteLine("Write -> default");
                                            break;
                                    }
                                    break;
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

        static Match ParseWrite(String line)
        {
            Regex reg = new Regex(
                "\\u005b\\u0022(?<command>[A-Za-z0-9 _]+)\\u0022");
            Match match = reg.Match(line);
            return match;
        }

        static Match ParseString(String line)
        {
            Regex reg = new Regex(
                "\\u005b\\u0022(?<command>[A-Za-z0-9 _]+)\\u0022(,\\u0022(?<wparam>[^\\u0022]+)\\u0022)?(,(?<lparam>(\\u0022[^\\u0022]+\\u0022)|(\\u007b[^\\u007d]+\\u007d)))?\\u005d");
            //        [     "        STARTSESSION            "   ,   "               HX01",{"ARCHIVE"=>"C:/AXX/HX01/62/H62/USA","HASVP"=>"1","LANGUAGE"=>"USA","PASSWORD"=>"root123","PRODUCT"=>"HX5000_6_2","USERNAME"=>"admin"}] 
            //        [     "        WRITE                   "   ,   [{"TYPE"=>"View"},{"ADDRESS"=>"IDS_ROW_SYSTEM",
            Match match = reg.Match(line);
            return match;
        }

        public static Match ParseSession(String line)
        {
            Regex regx = new Regex(
                "(\\u007b\\u0022[A-Za-z]+\\u0022[^\\u0022]+\\u0022(?<archive>[^\\u0022]+)\\u0022),(\\u0022[A-Za-z]+\\u0022[^\\u0022]+\\u0022(?<hasvp>[0-9]+)\\u0022),(\\u0022[A-Za-z]+\\u0022[^\\u0022]+\\u0022(?<language>[A-Za-z]+)\\u0022),(\\u0022[A-Za-z]+\\u0022[^\\u0022]+\\u0022(?<password>[A-Za-z0-9_]+)\\u0022),(\\u0022[A-Za-z]+\\u0022[^\\u0022]+\\u0022(?<product>[A-Za-z0-9_]+)\\u0022),(\\u0022[A-Za-z]+\\u0022[^\\u0022]+\\u0022(?<username>[A-Za-z0-9_]+)\\u0022)\\u007D");
            //       {      "    ARCHIVE     "       =>        "    C:/AXX/HX01/62/H62/USA  "    ,    "     HASVP      "       =>         "            1        "    ,     "  LANGUAGE   "       =>        "                  USA       "    ,    "    PASSWORD   "        =>       "                root123         "    ,     "   PRODUCT     "       =>       "          HX5000_6_2            "   ,     "  USERNAME   "        =>         "                admin          "        }
            Console.WriteLine(line);
            Match match = regx.Match(line);
            return match;
        }

        
        static void ParseWriteCommand(String line)
        {
            Regex regx = new Regex(
               "(\\u007b\\u0022[A-Za-z0-9 _]+\\u0022[^\\u0022]+\\u0022(?<address>[A-Za-z0-9 _]+)\\u0022,\\u0022[A-Za-z0-9 _]+\\u0022[^\\u0022]+\\u0022(?<type>[A-Za-z0-9 _]+)\\u0022\\u007D)+");
            //        {    "      ADDRESS       "        =>        "              IDS_ROW_SYSTEM   "   ,    "       TYPE       "       =>         "     SubView       "       },{"ADDRESS"=>"IDS_ROW_IP_SETTINGS","TYPE"=>"SubView"},{"ADDRESS"=>"IDS_ROW_DHCP_SERVER_SETTING","TYPE"=>"SubView"},{"ADDRESS"=>"IDS_ROW_DHCP_SERVER_ENABLED","TYPE"=>"Row"},{"ADDRESS"=>"IDS_COL_VALUE","TYPE"=>"Cell"}]]
             //  "(\\u005b\\u0022[A-Za-z0-9 _]+\\u0022,\\u005b\\u007b\\u0022[A-Za-z0-9]+\\u0022[^\\u0022]+\\u0022[A-Za-z0-9 _]+\\u0022\\u007D(,(\\u007b\\u0022[A-Za-z0-9 _]+\\u0022[^\\u0022]+\\u0022[A-Za-z0-9 _]+\\u0022,\\u0022[A-Za-z0-9 _]+\\u0022[^\\u0022]+\\u0022[A-Za-z0-9 _]+\\u0022\\u007D))+\\u005d\\u005d)");
            //["WRITE"                               ,     [     {       "     TYPE          "      =>     "        View          "   }    ,     {    "      ADDRESS       "        =>        "    IDS_ROW_SYSTEM   "   ,    "       TYPE       "       =>         "     SubView       "       },{"ADDRESS"=>"IDS_ROW_IP_SETTINGS","TYPE"=>"SubView"},{"ADDRESS"=>"IDS_ROW_DHCP_SERVER_SETTING","TYPE"=>"SubView"},{"ADDRESS"=>"IDS_ROW_DHCP_SERVER_ENABLED","TYPE"=>"Row"},{"ADDRESS"=>"IDS_COL_VALUE","TYPE"=>"Cell"}]]
            Match match = regx.Match(line);
            Console.WriteLine(line);

            while (match.Success)
            {
                WriteCommandDictionary.Add(match.Groups[2].Value, match.Groups[3].Value);
                Console.WriteLine(match.Groups[2].Value +":"+ match.Groups[3].Value);
                match = match.NextMatch();
            }
        }

        public static Match ParseCoordinates(string line)
        {
            Regex reg = new Regex(
             "([0-9]+),([0-9]+),([0-9]+),([0-9]+)");
            // "\\u005b\\u0022(?<command>[A-Za-z0-9 _]+)\\u0022(,\\u0022(?<wparam>[^\\u0022]+)\\u0022)?(,(?<lparam>(\\u0022[^\\u0022]+\\u0022)|(\\u007b[^\\u007d]+\\u007d)))?\\u005d");
            //       {    "      ADDRESS       "        =>        "              IDS_ROW_SYSTEM   "   ,    "       TYPE       "       =>         "     SubView       "       },{"ADDRESS"=>"IDS_ROW_IP
            Match match = reg.Match(line);
            return match;

        }


        public static void Main()
        {
            //windowsCount = GETListOfWindows().Count;           //after start
            EmulateVTServer();
            //bool rrrr = CommonProc.LaunchDBP("admin", "root123", "HX01");
            //Test();
            /*
            Console.WriteLine("start sess");
            LaunchDBP("admin", "root123", "HX01");
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
             * */

        }
    }
}
