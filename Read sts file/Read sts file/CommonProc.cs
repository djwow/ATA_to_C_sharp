using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System.Threading;
using System.Text.RegularExpressions;
//using Winium.Cruciatus.Core;
//using Winium.Cruciatus.Extensions;

namespace Read_sts_file
{
    class CommonProc
    {
        public static IWebElement text, mainwindow = null;
        public static string name, replacename;
        public static IList<IWebElement> ListOfWindows = Program.GETListOfWindows();
        public static IList<IWebElement> ListOfElements;
        public static bool DBPisReady = false;
        public static bool LaunchDBP(string username, string password, string session) //add session
        {
            Program.StartWinium();
            DesiredCapabilities dc = new DesiredCapabilities();
            dc.SetCapability("app", Program.SessionManager);
            dc.SetCapability("args", @"" + session + " /USERNAME=" + username + " /PASSWORD=" + password);
            dc.SetCapability("launchDelay", "3000");

            Console.WriteLine("Start DBP");

            Program.driver = new RemoteWebDriver(new Uri("http://localhost:9999"), dc);
            Thread.Sleep(3000);
            //IWebElement text, mainwindow = null;
            //string name, replacename;
            //IList<IWebElement> ListOfWindows = Program.GETListOfWindows();
            //IList<IWebElement> ListOfElements;

            while (DBPisReady == false)
            {
                //window = driver.FindElement(By.ClassName("#32770"));
                ListOfWindows = Program.GETListOfWindows();
                foreach (IWebElement list in ListOfWindows)
                {
                    name = list.GetAttribute("Name");
                    replacename = list.GetAttribute("Name").Replace(session + " - ", "");
                    Console.WriteLine(list.GetAttribute("Name"));

                    //switch (Program.sProgram.GETSTRING(window.GetAttribute("Name").Replace(session + " - ", ""))) //change to session
                    switch (Program.sGETSTRING(list.GetAttribute("Name").Replace(session + " - ", "")))
                    {
                        case "IDS_DLG_DATABASE_PROGRAMMING":    //move to another dll
                            {
                                Console.WriteLine("IDS_DLG_GENERAL_POPUP");
                                text = list.FindElement(By.Id("65535"));   ////////////change to all text on window ListOfWindows = driver.FindElements(By.XPath("/*[contains(@ControlType,'ControlType.Text')]"));
                                switch (Program.sGETSTRING(text.GetAttribute("Name")))
                                {
                                    case "IDS_DLG_DATABASE_PROGRAMMING_PRIMARY_ATTENDANT":
                                        {
                                            Console.WriteLine("IDS_DLG_DATABASE_PROGRAMMING_PRIMARY_ATTENDANT");
                                            Program.WriteToSTS("POPUP OPEN: '" + list.GetAttribute("Name") + "','" + text.GetAttribute("Name"));
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_NO"))).Click();
                                            Program.WriteToSTS("POPUP CLOSE: '" + list.GetAttribute("Name") + "','" + text.GetAttribute("Name"));
                                            //Thread.Sleep(1000);
                                            break;
                                        }
                                    case "IDS_TXT_TIME_ZONE_MSG":
                                        {
                                            Console.WriteLine("IDS_TXT_TIME_ZONE_MSG");
                                            Program.WriteToSTS("POPUP OPEN: '" + list.GetAttribute("Name") + "','" + text.GetAttribute("Name"));
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            //Thread.Sleep(1000);
                                            break;
                                        }
                                    case "IDS_TXT_ALL_DATA_IN_THE_VOICE_PROCESSOR_SAVE_DIRECTORY_AND_ANY_SUBDIRECTORIES_WILL_BE_DELETED":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_ALL_DATA_IN_THE_VOICE_PROCESSOR_SAVE_DIRECTORY_AND_ANY_SUBDIRECTORIES_WILL_BE_DELETED");
                                            Program.WriteToSTS("POPUP OPEN: '" + list.GetAttribute("Name") + "','" + text.GetAttribute("Name"));
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                            break;
                                        }
                                    case "IDS_DLG_CALL_PROCESSING_IS_NOT_RESPONDING":
                                        {
                                            Console.WriteLine("WIZARD - IDS_DLG_CALL_PROCESSING_IS_NOT_RESPONDING");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            //systemdbprog_pCurrentSession[0].bForceShutdown = TRUE;
                                            break;
                                        }
                                    ////////////////////////////////////////////////
                                    case "IDS_TXT_YOUR_CARD_OPTIONS_ARE_LIMITED_WARNING":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_YOUR_CARD_OPTIONS_ARE_LIMITED_WARNING");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_YOU_MUST_RESTART_DATABASE_PROGRAMMING_":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_YOU_MUST_RESTART_DATABASE_PROGRAMMING_");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_THE_PROGRAMMING_INACTIVITY_TIMER_HAS_EXPIRED":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_THE_PROGRAMMING_INACTIVITY_TIMER_HAS_EXPIRED");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_DATABASE_PROGRAMMING_IS_SHUTTING_DOWN":
                                        {
                                            Console.WriteLine("systemdbprog_lPopupHandler_Default - IDS_TXT_DATABASE_PROGRAMMING_IS_SHUTTING_DOWN");
                                            //list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            //LOGMESSAGE(LOGMESSAGE_INFO, "WIZARD - PopupHandler_Default - Will wait " + str$(workstation_WAIT_RESTART/1000) + " seconds before restarting DB studio");
                                            //libgui_bSleep(workstation_WAIT_RESTART,TRUE);
                                            //systemdbprog_pCurrentSession[0].bForceShutdown = FALSE
                                            break;
                                        }
                                    case "IDS_TXT_PLEASE_CONTACT_YOUR_LOCAL_NETWORK_ADMINISTRATOR":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_PLEASE_CONTACT_YOUR_LOCAL_NETWORK_ADMINISTRATOR");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }

                                    case "IDS_TXT_CREATING_OR_CHANGING_A_NETWORK_GROUP_CAN_CAUSE_SYSTEM_WIDE_PROGRAMMING_CONFLICTS_WARNING":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_CREATING_OR_CHANGING_A_NETWORK_GROUP_CAN_CAUSE_SYSTEM_WIDE_PROGRAMMING_CONFLICTS_WARNING");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_YOU_CANNOT_DELETE_THE_LOCAL_NODE":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_YOU_CANNOT_DELETE_THE_LOCAL_NODE");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_DELETE_THE_SELECTED_ITEMS":
                                        {
                                            Console.WriteLine("WIZARD - DeleteSelectedItems");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_YOU_CAN_EXCLUDE_THIS_ITEM_ONLY_BY_INCLUDING_IT_INTO_ANOTHER_LIST":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_YOU_CAN_EXCLUDE_THIS_ITEM_ONLY_BY_INCLUDING_IT_INTO_ANOTHER_LIST");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_THIS_CHANGE_WILL_FORCE_VM_CP_RESET":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_THIS_CHANGE_WILL_FORCE_VM_CP_RESET");
                                            //list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"));
                                            //LOGMESSAGE(LOGMESSAGE_INFO, "WIZARD - PopupHandler_Default - Will wait " + str$(workstation_WAIT_RESTART/1000) + " seconds before restarting DB studio");
                                            //libgui_bSleep(workstation_WAIT_RESTART,TRUE);
                                            //systemdbprog_pCurrentSession[0].bForceShutdown = FALSE;
                                            break;
                                        }
                                    case "IDS_TXT_THERE_ARE_PHONES_REFERENCING_AT_LEAST_ONE_OF_THE_SELECTED_KEYMAPS":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_THERE_ARE_PHONES_REFERENCING_AT_LEAST_ONE_OF_THE_SELECTED_KEYMAPS");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_CHANGING_THE_AD_HOC_CONFERENCE_TYPE_CAN_AFFECT_THE_AVAILABLE_IP_RESOURCE_CAPACITY":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_CHANGING_THE_AD_HOC_CONFERENCE_TYPE_CAN_AFFECT_THE_AVAILABLE_IP_RESOURCE_CAPACITY");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_CHANGING_THE_VOIP_ECHO_CANCELLER_SETTING_CAN_AFFECT_THE_AVAILABLE_IP_RESOURCE_CAPACITY":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_CHANGING_THE_VOIP_ECHO_CANCELLER_SETTING_CAN_AFFECT_THE_AVAILABLE_IP_RESOURCE_CAPACITY");
                                            //list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"));
                                            //LOGMESSAGE(LOGMESSAGE_INFO, "WIZARD - PopupHandler_Default - Will wait " + str$(workstation_WAIT_RESTART/1000) + " seconds before restarting DB studio");
                                            //libgui_bSleep(workstation_WAIT_RESTART,TRUE);
                                            //systemdbprog_pCurrentSession[0].bForceShutdown = FALSE;
                                            break;
                                        }
                                    case "IDS_TXT_MITEL_5000_BASIC_VOICE_MAIL_WILL_NOW_BE_ENABLED_AND_THIS_SESSION_WILL_TERMINATE":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_MITEL_5000_BASIC_VOICE_MAIL_WILL_NOW_BE_ENABLED_AND_THIS_SESSION_WILL_TERMINATE");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_MITEL_5000_BASIC_VOICE_MAIL_WILL_NOW_BE_DISABLED_AND_THIS_SESSION_WILL_TERMINATE":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_MITEL_5000_BASIC_VOICE_MAIL_WILL_NOW_BE_DISABLED_AND_THIS_SESSION_WILL_TERMINATE");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_IP_NUMBER_CHANGE_WARNING":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_IP_NUMBER_CHANGE_WARNING");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_IS_THE_SIP_VOICE_MAIL_YOU_ARE_ABOUT_TO_CREATE_A_NUPOINT_UNIFIED_MESSENGER":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_IS_THE_SIP_VOICE_MAIL_YOU_ARE_ABOUT_TO_CREATE_A_NUPOINT_UNIFIED_MESSENGER");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_NETWORK_BANDWIDTH_IMPACT_WARNING":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_NETWORK_BANDWIDTH_IMPACT_WARNING");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_ONE_OR_MORE_TIME_SLOT_GROUPS_CURRENTLY_SPECIFY_A_MAXIMUM_CHANNEL_USAGE_OTHER_THAN":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_ONE_OR_MORE_TIME_SLOT_GROUPS_CURRENTLY_SPECIFY_A_MAXIMUM_CHANNEL_USAGE_OTHER_THAN");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_ONE_OR_MORE_TIME_SLOT_GROUPS_CURRENTLY_SPECIFY_A_MAXIMUM_CHANNEL_USAGE_GREATER_THAN":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_ONE_OR_MORE_TIME_SLOT_GROUPS_CURRENTLY_SPECIFY_A_MAXIMUM_CHANNEL_USAGE_GREATER_THAN");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_WARNING_ENABLING_PREMIUM_FEATURES_NEW_SOFTWARE_LICENSE":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_WARNING_ENABLING_PREMIUM_FEATURES_NEW_SOFTWARE_LICENSE");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_DEVICES_EXCEEDS_VOICE_CHANNELS_WARNING":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_DEVICES_EXCEEDS_VOICE_CHANNELS_WARNING");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_SWITCHING_THE_BS_BVM_SYSTEM_RECORDING_CODEC_REQUIRES_VOICE_MAIL_TO_RESET":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_SWITCHING_THE_BS_BVM_SYSTEM_RECORDING_CODEC_REQUIRES_VOICE_MAIL_TO_RESET");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_WARNING_FAX_OVER_IPRC_NETWORKING_NOT_YET_SUPPORTED":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_WARNING_FAX_OVER_IPRC_NETWORKING_NOT_YET_SUPPORTED");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_WARNING_FAX_OVER_IP_ONLY_SUPPORTED_PROTOCOL":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_WARNING_FAX_OVER_IP_ONLY_SUPPORTED_PROTOCOL");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_WARNING_BY_TOGGLING_NETWORK_WITH_US_FLAG_TO_NO":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_WARNING_BY_TOGGLING_NETWORK_WITH_US_FLAG_TO_NO");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_WARNING_CHANGING_THIS_VALUE_WILL_CAUSE_THE_IP_DEVICE_TO_RESET":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_WARNING_CHANGING_THIS_VALUE_WILL_CAUSE_THE_IP_DEVICE_TO_RESET");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_WARNING_CHANGING_THIS_VALUE_WILL_CAUSE_THE_IP_RESOURCE_APPLICATION_TO_RESET":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_WARNING_CHANGING_THIS_VALUE_WILL_CAUSE_THE_IP_RESOURCE_APPLICATION_TO_RESET");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_WARNING_CHANGING_THIS_VALUE_WILL_CAUSE_THE_SYSTEM_TO_RESET":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_WARNING_CHANGING_THIS_VALUE_WILL_CAUSE_THE_SYSTEM_TO_RESET");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_WARNING_BY_TOGGLING_NETWORK_WITH_US_FLAG_TO_YES":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_WARNING_BY_TOGGLING_NETWORK_WITH_US_FLAG_TO_YES");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_WARNING_GROUP_CALL_PICKUP_WILL_BE_DISABLED":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_WARNING_GROUP_CALL_PICKUP_WILL_BE_DISABLED");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_WARNING_REMOVING_AN_MGCP_GATEWAY":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_WARNING_REMOVING_AN_MGCP_GATEWAY");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_WARNING_YOUR_STATIC_IP_ADDRESS_FOR_THE_PROCESSOR_MODULE_AND_EXPANSTION_CARD_APPEAR_TO_ON_DIFFERENT_SUBNETS":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_WARNING_YOUR_STATIC_IP_ADDRESS_FOR_THE_PROCESSOR_MODULE_AND_EXPANSTION_CARD_APPEAR_TO_ON_DIFFERENT_SUBNETS");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_WARNING_YOUR_STATIC_IP_ADDRESS_FOR_THE_PROCESSOR_MODULE_AND_EXPANSTION_CARD_APPEAR_TO_ON_DIFFERENT_SUBNETS_CHECK_THE_PROGRAMMING":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_WARNING_YOUR_STATIC_IP_ADDRESS_FOR_THE_PROCESSOR_MODULE_AND_EXPANSTION_CARD_APPEAR_TO_ON_DIFFERENT_SUBNETS_CHECK_THE_PROGRAMMING");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_WARNING_YOUR_STATIC_IP_ADDRESS_FOR_THE_PROCESSOR_MODULE_EXPANSTION_CARD_AND_PROCESSING_SERVER_APPEAR_TO_ON_DIFFERENT_SUBNETS":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_WARNING_YOUR_STATIC_IP_ADDRESS_FOR_THE_PROCESSOR_MODULE_EXPANSTION_CARD_AND_PROCESSING_SERVER_APPEAR_TO_ON_DIFFERENT_SUBNETS");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_WARNING_YOUR_SYSTEM_IS_CURRENTLY_RESERVING_IP_RESOURCES_FOR_BASIC_VOICE_MAIL":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_WARNING_YOUR_SYSTEM_IS_CURRENTLY_RESERVING_IP_RESOURCES_FOR_BASIC_VOICE_MAIL");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_NO"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_IT_IS_HIGHLY_RECOMMENDED_THAT_YOU_RESET_CALL_PROCESSING_IN_ORDER_FOR_CHANGES_TO_THE_BASIC_VOICE_MAIL_PORT_RESOURCES":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_IT_IS_HIGHLY_RECOMMENDED_THAT_YOU_RESET_CALL_PROCESSING_IN_ORDER_FOR_CHANGES_TO_THE_BASIC_VOICE_MAIL_PORT_RESOURCES");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_SIP_GATEWAY_1_CANNOT_BE_REMOVED":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_SIP_GATEWAY_1_CANNOT_BE_REMOVED");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_CHANGES_TO_THE_IP_SETTINGS_NOTE":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_CHANGES_TO_THE_IP_SETTINGS_NOTE");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_THE_DND_MESSAGE_NUMBER_CHANGE":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_THE_DND_MESSAGE_NUMBER_CHANGE");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_NO"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_FORWARD_ONLY_MSG":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_FORWARD_ONLY_MSG");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_VOICE_PROCESSOR_DRIVE_NOT_READY":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_VOICE_PROCESSOR_DRIVE_NOT_READY");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_VOICE_PROCESSOR_THE_DISK_CONTAINS_SAVED_DATE":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_VOICE_PROCESSOR_THE_DISK_CONTAINS_SAVED_DATE");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_VPIM_FUNCTIONALITY_REQUIRES_THE_EXTENSION_FOR_THE_MAILBOX_ON_THE_REMOTE_VOICE_PROCESSING_SYSTEM":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_VPIM_FUNCTIONALITY_REQUIRES_THE_EXTENSION_FOR_THE_MAILBOX_ON_THE_REMOTE_VOICE_PROCESSING_SYSTEM");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_WARNING_EXCEED_IP_RESOURCES":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_WARNING_EXCEED_IP_RESOURCES");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_EMAIL_SYSTEM_WARNING":
                                        {
                                            Console.WriteLine("WIZARD - IDS_EMAIL_SYSTEM_WARNING");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_FAX_EMAIL_FORWARDING_EMAIL_GATEWAY":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_FAX_EMAIL_FORWARDING_EMAIL_GATEWAY");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_EMAIL_GATEWAY_MESSAGE":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_EMAIL_GATEWAY_MESSAGE");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_EMAIL_GATEWAY_EMAIL_SYSTEM_MESSAGE":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_EMAIL_GATEWAY_EMAIL_SYSTEM_MESSAGE");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_EMAIL_GATEWAY_EMAIL_SYSTEM_MESSAGE_21":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_EMAIL_GATEWAY_EMAIL_SYSTEM_MESSAGE_21");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_CREATING_THIS_LARGE_AMOUNT_OF_DEVICES_COULD_TAKE_A_SIGNIFICANT_AMOUNT_OF_TIME":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_CREATING_THIS_LARGE_AMOUNT_OF_DEVICES_COULD_TAKE_A_SIGNIFICANT_AMOUNT_OF_TIME");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_CHANGING_DATE_OR_TIME_CAUSE_EMBEDDED_REPORTING_INFORMATAION_SURE_WANT_TO_CONTINIUE":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_CHANGING_DATE_OR_TIME_CAUSE_EMBEDDED_REPORTING_INFORMATAION_SURE_WANT_TO_CONTINIUE");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_FIRST_TIME_RUNNING_THE_HYBRID_BALANCE_TEST":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_FIRST_TIME_RUNNING_THE_HYBRID_BALANCE_TEST");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_TXT_THE_CONFIGURATION_IS_NOT_SUPPORTED":
                                        {
                                            Console.WriteLine("WIZARD - IDS_TXT_THE_CONFIGURATION_IS_NOT_SUPPORTED");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_DLG_5000_CP_IP_SETTINGS_UNICUE":
                                        {
                                            Console.WriteLine("WIZARD - IDS_DLG_5000_CP_IP_SETTINGS_UNICUE");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_DLG_5000_CP_APPROXIMATED_SIZE":
                                        {
                                            Console.WriteLine("WIZARD - IDS_DLG_5000_CP_APPROXIMATED_SIZE");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_DLG_DATABASE_PROGRAMMING":
                                        {
                                            Console.WriteLine("WIZARD - IDS_DLG_DATABASE_PROGRAMMING");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_DLG_THE_APPROXIMATE_SIZE_OF_THE_SELECTED_VOICE_DATA":
                                        {
                                            Console.WriteLine("WIZARD - IDS_DLG_THE_APPROXIMATE_SIZE_OF_THE_SELECTED_VOICE_DATA");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_DLG_THERE_ARE_1_LOCAL_VOICE_PROCESSOR_APLICATION":
                                        {
                                            Console.WriteLine("WIZARD - IDS_DLG_THERE_ARE_1_LOCAL_VOICE_PROCESSOR_APLICATION");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                            break;
                                        }
                                    case "IDS_DLG_DATABASE_DELETE_PRIMARY_ATTENDANT":
                                        {
                                            Console.WriteLine("WIZARD - IDS_DLG_DATABASE_DELETE_PRIMARY_ATTENDANT");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                            break;
                                        }
                                    case "IDS_DLG_DATABASE_SET_PRIMARY_ATTENDANT":
                                        {
                                            Console.WriteLine("WIZARD - IDS_DLG_DATABASE_SET_PRIMARY_ATTENDANT");
                                            list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_NO"))).Click();
                                            break;
                                        }
                                    default:
                                        {
                                            Console.WriteLine("default1");
                                            Thread.Sleep(1000);
                                            break;
                                        }
                                }
                                break;
                            }
                        case "IDS_DLG_GENERAL_POPUP":
                            {
                                Console.WriteLine("IDS_DLG_GENERAL_POPUP");
                                //window.FindElement(By.Name(Program.GETSTRING("IDS_BTN_NO"))).Click();
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
                                break;
                            }
                    }
                    if (list.GetAttribute("Name") == session + " - " + Program.GETSTRING("IDS_DLG_MITEL_DB_PROGRAMMING"))
                    {
                        DBPisReady = true;
                        mainwindow = Program.driver.FindElementByName(session + " - " + Program.GETSTRING("IDS_DLG_MITEL_DB_PROGRAMMING"));
                        Program.driver.SwitchTo().Window(session + " - " + Program.GETSTRING("IDS_DLG_MITEL_DB_PROGRAMMING"));
                    }
                }
                ListOfWindows = Program.GETListOfWindows();
                foreach (IWebElement list in ListOfWindows)
                {
                    if (list.GetAttribute("Name") == session + " - " + Program.GETSTRING("IDS_DLG_MITEL_DB_PROGRAMMING"))
                    {
                        DBPisReady = true;
                        mainwindow = Program.driver.FindElement(By.Name(session + " - " + Program.GETSTRING("IDS_DLG_MITEL_DB_PROGRAMMING")));
                    }
                }
            }

            /////////////////////////////////////////delete
            Program.SessionsDictionary.Add("HX01", "{\"ARCHIVE\"=>\"C:/AXX/HX04/62/H62/USA\",\"HASVP\"=>\"1\",\"LANGUAGE\"=>\"USA\",\"PASSWORD\"=>\"root123\",\"PRODUCT\"=>\"HX5000_6_2\",\"USERNAME\"=>\"admin\"}");
            Program.SessionsDictionary.Add("HX02", "{\"ARCHIVE\"=>\"C:/AXX/HX02/62/H62/USA\",\"HASVP\"=>\"1\",\"LANGUAGE\"=>\"USA\",\"PASSWORD\"=>\"root123\",\"PRODUCT\"=>\"HX5000_6_2\",\"USERNAME\"=>\"admin\"}");
            /////////////////////////////////////////
            Match MatchSession = Program.ParseSession(Program.SessionsDictionary[session].ToString());
            Program.WriteToSTS("WARN: systemdbprog_bSessionActivate: Session '" + session + "' is not launched");
            Program.WriteToSTS("INFO: Configuring session " + session + "PRODUCT key to " + MatchSession.Groups["product"].Value.ToString());
            Program.WriteToSTS("INFO: Configuring session " + session + "LANGUAGE key to " + MatchSession.Groups["language"].Value.ToString());
            Program.WriteToSTS("INFO: Configuring session " + session + "ARCHIVE key to " + MatchSession.Groups["archive"].Value.ToString());
            Program.WriteToSTS("INFO: Configuring session " + session + "HASVP key to " + MatchSession.Groups["hasvp"].Value.ToString());
            Program.WriteToSTS("INFO: Configuring session " + session + "USERNAME key to " + MatchSession.Groups["username"].Value.ToString());
            Program.WriteToSTS("INFO: Configuring session " + session + "PASSWORD key to " + MatchSession.Groups["password"].Value.ToString());
            Program.WriteToSTS("INFO: systemdbprog_bSessionStart: Request to start session '" + session + "'");
            Program.WriteToSTS("INFO: Launching explorer for session '" + session + "'");
            Program.WriteToSTS("INFO: ExplorerLaunch: Launching DB Studio from CLI");
            Program.WriteToSTS("INFO: systemdbprog_bCLISessionLaunch: Launching session '" + session + "'");
            Program.WriteToSTS("INFO: systemdbprog_bCLISessionLaunch: Executing '" + Program.SessionManager + " " + session + " /USERNAME=" + username + " /PASSWORD=" + password + "'");
            Thread.Sleep(100);
            //File.Delete(CMDFile); //uncomment

            Program.driver.FindElement(By.Name("Operations")).Click();
            Program.driver.FindElement(By.Name("Backup Operations")).Click();
            Program.driver.FindElement(By.Name("Save Backup...")).Click();

            Program.driver.Quit();
            Program.driver.Close();
            return true;
        }


        public static void WindowsHandle (RemoteWebDriver driver, string session)
        {
            IWebElement text, mainwindow = null;
            string name, replacename;
            IList<IWebElement> ListOfWindows = Program.GETListOfWindows();
            IList<IWebElement> ListOfElements;
            foreach (IWebElement list in ListOfWindows)
            {
                name = list.GetAttribute("Name");
                replacename = list.GetAttribute("Name").Replace(session + " - ", "");
                Console.WriteLine(list.GetAttribute("Name"));

                //switch (Program.sProgram.GETSTRING(window.GetAttribute("Name").Replace(session + " - ", ""))) //change to session
                switch (Program.sGETSTRING(list.GetAttribute("Name").Replace(session + " - ", "")))
                {
                    case "IDS_DLG_DATABASE_PROGRAMMING":    //move to another dll
                        {
                            Console.WriteLine("IDS_DLG_GENERAL_POPUP");
                            text = list.FindElement(By.Id("65535"));   ////////////change to all text on window ListOfWindows = driver.FindElements(By.XPath("/*[contains(@ControlType,'ControlType.Text')]"));
                            switch (Program.sGETSTRING(text.GetAttribute("Name")))
                            {
                                case "IDS_DLG_DATABASE_PROGRAMMING_PRIMARY_ATTENDANT":
                                    {
                                        Console.WriteLine("IDS_DLG_DATABASE_PROGRAMMING_PRIMARY_ATTENDANT");
                                        Program.WriteToSTS("POPUP OPEN: '" + list.GetAttribute("Name") + "','" + text.GetAttribute("Name"));
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_NO"))).Click();
                                        //Thread.Sleep(1000);
                                        break;
                                    }
                                case "IDS_TXT_TIME_ZONE_MSG":
                                    {
                                        Console.WriteLine("IDS_TXT_TIME_ZONE_MSG");
                                        Program.WriteToSTS("POPUP OPEN: '" + list.GetAttribute("Name") + "','" + text.GetAttribute("Name"));
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        //Thread.Sleep(1000);
                                        break;
                                    }
                                case "IDS_TXT_ALL_DATA_IN_THE_VOICE_PROCESSOR_SAVE_DIRECTORY_AND_ANY_SUBDIRECTORIES_WILL_BE_DELETED":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_ALL_DATA_IN_THE_VOICE_PROCESSOR_SAVE_DIRECTORY_AND_ANY_SUBDIRECTORIES_WILL_BE_DELETED");
                                        Program.WriteToSTS("POPUP OPEN: '" + list.GetAttribute("Name") + "','" + text.GetAttribute("Name"));
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                        break;
                                    }
                                case "IDS_DLG_CALL_PROCESSING_IS_NOT_RESPONDING":
                                    {
                                        Console.WriteLine("WIZARD - IDS_DLG_CALL_PROCESSING_IS_NOT_RESPONDING");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        //systemdbprog_pCurrentSession[0].bForceShutdown = TRUE;
                                        break;
                                    }
                                ////////////////////////////////////////////////
                                case "IDS_TXT_YOUR_CARD_OPTIONS_ARE_LIMITED_WARNING":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_YOUR_CARD_OPTIONS_ARE_LIMITED_WARNING");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_YOU_MUST_RESTART_DATABASE_PROGRAMMING_":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_YOU_MUST_RESTART_DATABASE_PROGRAMMING_");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_THE_PROGRAMMING_INACTIVITY_TIMER_HAS_EXPIRED":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_THE_PROGRAMMING_INACTIVITY_TIMER_HAS_EXPIRED");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_DATABASE_PROGRAMMING_IS_SHUTTING_DOWN":
                                    {
                                        Console.WriteLine("systemdbprog_lPopupHandler_Default - IDS_TXT_DATABASE_PROGRAMMING_IS_SHUTTING_DOWN");
                                        //list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        //LOGMESSAGE(LOGMESSAGE_INFO, "WIZARD - PopupHandler_Default - Will wait " + str$(workstation_WAIT_RESTART/1000) + " seconds before restarting DB studio");
                                        //libgui_bSleep(workstation_WAIT_RESTART,TRUE);
                                        //systemdbprog_pCurrentSession[0].bForceShutdown = FALSE
                                        break;
                                    }
                                case "IDS_TXT_PLEASE_CONTACT_YOUR_LOCAL_NETWORK_ADMINISTRATOR":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_PLEASE_CONTACT_YOUR_LOCAL_NETWORK_ADMINISTRATOR");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }

                                case "IDS_TXT_CREATING_OR_CHANGING_A_NETWORK_GROUP_CAN_CAUSE_SYSTEM_WIDE_PROGRAMMING_CONFLICTS_WARNING":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_CREATING_OR_CHANGING_A_NETWORK_GROUP_CAN_CAUSE_SYSTEM_WIDE_PROGRAMMING_CONFLICTS_WARNING");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_YOU_CANNOT_DELETE_THE_LOCAL_NODE":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_YOU_CANNOT_DELETE_THE_LOCAL_NODE");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_DELETE_THE_SELECTED_ITEMS":
                                    {
                                        Console.WriteLine("WIZARD - DeleteSelectedItems");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_YOU_CAN_EXCLUDE_THIS_ITEM_ONLY_BY_INCLUDING_IT_INTO_ANOTHER_LIST":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_YOU_CAN_EXCLUDE_THIS_ITEM_ONLY_BY_INCLUDING_IT_INTO_ANOTHER_LIST");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_THIS_CHANGE_WILL_FORCE_VM_CP_RESET":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_THIS_CHANGE_WILL_FORCE_VM_CP_RESET");
                                        //list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"));
                                        //LOGMESSAGE(LOGMESSAGE_INFO, "WIZARD - PopupHandler_Default - Will wait " + str$(workstation_WAIT_RESTART/1000) + " seconds before restarting DB studio");
                                        //libgui_bSleep(workstation_WAIT_RESTART,TRUE);
                                        //systemdbprog_pCurrentSession[0].bForceShutdown = FALSE;
                                        break;
                                    }
                                case "IDS_TXT_THERE_ARE_PHONES_REFERENCING_AT_LEAST_ONE_OF_THE_SELECTED_KEYMAPS":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_THERE_ARE_PHONES_REFERENCING_AT_LEAST_ONE_OF_THE_SELECTED_KEYMAPS");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_CHANGING_THE_AD_HOC_CONFERENCE_TYPE_CAN_AFFECT_THE_AVAILABLE_IP_RESOURCE_CAPACITY":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_CHANGING_THE_AD_HOC_CONFERENCE_TYPE_CAN_AFFECT_THE_AVAILABLE_IP_RESOURCE_CAPACITY");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_CHANGING_THE_VOIP_ECHO_CANCELLER_SETTING_CAN_AFFECT_THE_AVAILABLE_IP_RESOURCE_CAPACITY":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_CHANGING_THE_VOIP_ECHO_CANCELLER_SETTING_CAN_AFFECT_THE_AVAILABLE_IP_RESOURCE_CAPACITY");
                                        //list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"));
                                        //LOGMESSAGE(LOGMESSAGE_INFO, "WIZARD - PopupHandler_Default - Will wait " + str$(workstation_WAIT_RESTART/1000) + " seconds before restarting DB studio");
                                        //libgui_bSleep(workstation_WAIT_RESTART,TRUE);
                                        //systemdbprog_pCurrentSession[0].bForceShutdown = FALSE;
                                        break;
                                    }
                                case "IDS_TXT_MITEL_5000_BASIC_VOICE_MAIL_WILL_NOW_BE_ENABLED_AND_THIS_SESSION_WILL_TERMINATE":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_MITEL_5000_BASIC_VOICE_MAIL_WILL_NOW_BE_ENABLED_AND_THIS_SESSION_WILL_TERMINATE");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_MITEL_5000_BASIC_VOICE_MAIL_WILL_NOW_BE_DISABLED_AND_THIS_SESSION_WILL_TERMINATE":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_MITEL_5000_BASIC_VOICE_MAIL_WILL_NOW_BE_DISABLED_AND_THIS_SESSION_WILL_TERMINATE");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_IP_NUMBER_CHANGE_WARNING":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_IP_NUMBER_CHANGE_WARNING");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_IS_THE_SIP_VOICE_MAIL_YOU_ARE_ABOUT_TO_CREATE_A_NUPOINT_UNIFIED_MESSENGER":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_IS_THE_SIP_VOICE_MAIL_YOU_ARE_ABOUT_TO_CREATE_A_NUPOINT_UNIFIED_MESSENGER");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_NETWORK_BANDWIDTH_IMPACT_WARNING":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_NETWORK_BANDWIDTH_IMPACT_WARNING");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_ONE_OR_MORE_TIME_SLOT_GROUPS_CURRENTLY_SPECIFY_A_MAXIMUM_CHANNEL_USAGE_OTHER_THAN":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_ONE_OR_MORE_TIME_SLOT_GROUPS_CURRENTLY_SPECIFY_A_MAXIMUM_CHANNEL_USAGE_OTHER_THAN");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_ONE_OR_MORE_TIME_SLOT_GROUPS_CURRENTLY_SPECIFY_A_MAXIMUM_CHANNEL_USAGE_GREATER_THAN":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_ONE_OR_MORE_TIME_SLOT_GROUPS_CURRENTLY_SPECIFY_A_MAXIMUM_CHANNEL_USAGE_GREATER_THAN");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_WARNING_ENABLING_PREMIUM_FEATURES_NEW_SOFTWARE_LICENSE":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_WARNING_ENABLING_PREMIUM_FEATURES_NEW_SOFTWARE_LICENSE");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_DEVICES_EXCEEDS_VOICE_CHANNELS_WARNING":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_DEVICES_EXCEEDS_VOICE_CHANNELS_WARNING");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_SWITCHING_THE_BS_BVM_SYSTEM_RECORDING_CODEC_REQUIRES_VOICE_MAIL_TO_RESET":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_SWITCHING_THE_BS_BVM_SYSTEM_RECORDING_CODEC_REQUIRES_VOICE_MAIL_TO_RESET");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_WARNING_FAX_OVER_IPRC_NETWORKING_NOT_YET_SUPPORTED":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_WARNING_FAX_OVER_IPRC_NETWORKING_NOT_YET_SUPPORTED");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_WARNING_FAX_OVER_IP_ONLY_SUPPORTED_PROTOCOL":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_WARNING_FAX_OVER_IP_ONLY_SUPPORTED_PROTOCOL");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_WARNING_BY_TOGGLING_NETWORK_WITH_US_FLAG_TO_NO":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_WARNING_BY_TOGGLING_NETWORK_WITH_US_FLAG_TO_NO");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_WARNING_CHANGING_THIS_VALUE_WILL_CAUSE_THE_IP_DEVICE_TO_RESET":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_WARNING_CHANGING_THIS_VALUE_WILL_CAUSE_THE_IP_DEVICE_TO_RESET");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_WARNING_CHANGING_THIS_VALUE_WILL_CAUSE_THE_IP_RESOURCE_APPLICATION_TO_RESET":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_WARNING_CHANGING_THIS_VALUE_WILL_CAUSE_THE_IP_RESOURCE_APPLICATION_TO_RESET");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_WARNING_CHANGING_THIS_VALUE_WILL_CAUSE_THE_SYSTEM_TO_RESET":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_WARNING_CHANGING_THIS_VALUE_WILL_CAUSE_THE_SYSTEM_TO_RESET");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_WARNING_BY_TOGGLING_NETWORK_WITH_US_FLAG_TO_YES":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_WARNING_BY_TOGGLING_NETWORK_WITH_US_FLAG_TO_YES");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_WARNING_GROUP_CALL_PICKUP_WILL_BE_DISABLED":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_WARNING_GROUP_CALL_PICKUP_WILL_BE_DISABLED");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_WARNING_REMOVING_AN_MGCP_GATEWAY":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_WARNING_REMOVING_AN_MGCP_GATEWAY");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_WARNING_YOUR_STATIC_IP_ADDRESS_FOR_THE_PROCESSOR_MODULE_AND_EXPANSTION_CARD_APPEAR_TO_ON_DIFFERENT_SUBNETS":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_WARNING_YOUR_STATIC_IP_ADDRESS_FOR_THE_PROCESSOR_MODULE_AND_EXPANSTION_CARD_APPEAR_TO_ON_DIFFERENT_SUBNETS");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_WARNING_YOUR_STATIC_IP_ADDRESS_FOR_THE_PROCESSOR_MODULE_AND_EXPANSTION_CARD_APPEAR_TO_ON_DIFFERENT_SUBNETS_CHECK_THE_PROGRAMMING":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_WARNING_YOUR_STATIC_IP_ADDRESS_FOR_THE_PROCESSOR_MODULE_AND_EXPANSTION_CARD_APPEAR_TO_ON_DIFFERENT_SUBNETS_CHECK_THE_PROGRAMMING");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_WARNING_YOUR_STATIC_IP_ADDRESS_FOR_THE_PROCESSOR_MODULE_EXPANSTION_CARD_AND_PROCESSING_SERVER_APPEAR_TO_ON_DIFFERENT_SUBNETS":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_WARNING_YOUR_STATIC_IP_ADDRESS_FOR_THE_PROCESSOR_MODULE_EXPANSTION_CARD_AND_PROCESSING_SERVER_APPEAR_TO_ON_DIFFERENT_SUBNETS");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_WARNING_YOUR_SYSTEM_IS_CURRENTLY_RESERVING_IP_RESOURCES_FOR_BASIC_VOICE_MAIL":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_WARNING_YOUR_SYSTEM_IS_CURRENTLY_RESERVING_IP_RESOURCES_FOR_BASIC_VOICE_MAIL");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_NO"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_IT_IS_HIGHLY_RECOMMENDED_THAT_YOU_RESET_CALL_PROCESSING_IN_ORDER_FOR_CHANGES_TO_THE_BASIC_VOICE_MAIL_PORT_RESOURCES":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_IT_IS_HIGHLY_RECOMMENDED_THAT_YOU_RESET_CALL_PROCESSING_IN_ORDER_FOR_CHANGES_TO_THE_BASIC_VOICE_MAIL_PORT_RESOURCES");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_SIP_GATEWAY_1_CANNOT_BE_REMOVED":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_SIP_GATEWAY_1_CANNOT_BE_REMOVED");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_CHANGES_TO_THE_IP_SETTINGS_NOTE":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_CHANGES_TO_THE_IP_SETTINGS_NOTE");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_THE_DND_MESSAGE_NUMBER_CHANGE":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_THE_DND_MESSAGE_NUMBER_CHANGE");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_NO"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_FORWARD_ONLY_MSG":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_FORWARD_ONLY_MSG");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_VOICE_PROCESSOR_DRIVE_NOT_READY":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_VOICE_PROCESSOR_DRIVE_NOT_READY");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_VOICE_PROCESSOR_THE_DISK_CONTAINS_SAVED_DATE":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_VOICE_PROCESSOR_THE_DISK_CONTAINS_SAVED_DATE");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_VPIM_FUNCTIONALITY_REQUIRES_THE_EXTENSION_FOR_THE_MAILBOX_ON_THE_REMOTE_VOICE_PROCESSING_SYSTEM":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_VPIM_FUNCTIONALITY_REQUIRES_THE_EXTENSION_FOR_THE_MAILBOX_ON_THE_REMOTE_VOICE_PROCESSING_SYSTEM");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_WARNING_EXCEED_IP_RESOURCES":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_WARNING_EXCEED_IP_RESOURCES");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_EMAIL_SYSTEM_WARNING":
                                    {
                                        Console.WriteLine("WIZARD - IDS_EMAIL_SYSTEM_WARNING");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_FAX_EMAIL_FORWARDING_EMAIL_GATEWAY":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_FAX_EMAIL_FORWARDING_EMAIL_GATEWAY");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_EMAIL_GATEWAY_MESSAGE":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_EMAIL_GATEWAY_MESSAGE");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_EMAIL_GATEWAY_EMAIL_SYSTEM_MESSAGE":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_EMAIL_GATEWAY_EMAIL_SYSTEM_MESSAGE");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_EMAIL_GATEWAY_EMAIL_SYSTEM_MESSAGE_21":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_EMAIL_GATEWAY_EMAIL_SYSTEM_MESSAGE_21");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_CREATING_THIS_LARGE_AMOUNT_OF_DEVICES_COULD_TAKE_A_SIGNIFICANT_AMOUNT_OF_TIME":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_CREATING_THIS_LARGE_AMOUNT_OF_DEVICES_COULD_TAKE_A_SIGNIFICANT_AMOUNT_OF_TIME");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_CHANGING_DATE_OR_TIME_CAUSE_EMBEDDED_REPORTING_INFORMATAION_SURE_WANT_TO_CONTINIUE":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_CHANGING_DATE_OR_TIME_CAUSE_EMBEDDED_REPORTING_INFORMATAION_SURE_WANT_TO_CONTINIUE");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_FIRST_TIME_RUNNING_THE_HYBRID_BALANCE_TEST":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_FIRST_TIME_RUNNING_THE_HYBRID_BALANCE_TEST");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_TXT_THE_CONFIGURATION_IS_NOT_SUPPORTED":
                                    {
                                        Console.WriteLine("WIZARD - IDS_TXT_THE_CONFIGURATION_IS_NOT_SUPPORTED");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_DLG_5000_CP_IP_SETTINGS_UNICUE":
                                    {
                                        Console.WriteLine("WIZARD - IDS_DLG_5000_CP_IP_SETTINGS_UNICUE");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_DLG_5000_CP_APPROXIMATED_SIZE":
                                    {
                                        Console.WriteLine("WIZARD - IDS_DLG_5000_CP_APPROXIMATED_SIZE");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_DLG_DATABASE_PROGRAMMING":
                                    {
                                        Console.WriteLine("WIZARD - IDS_DLG_DATABASE_PROGRAMMING");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_DLG_THE_APPROXIMATE_SIZE_OF_THE_SELECTED_VOICE_DATA":
                                    {
                                        Console.WriteLine("WIZARD - IDS_DLG_THE_APPROXIMATE_SIZE_OF_THE_SELECTED_VOICE_DATA");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_DLG_THERE_ARE_1_LOCAL_VOICE_PROCESSOR_APLICATION":
                                    {
                                        Console.WriteLine("WIZARD - IDS_DLG_THERE_ARE_1_LOCAL_VOICE_PROCESSOR_APLICATION");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_OK"))).Click();
                                        break;
                                    }
                                case "IDS_DLG_DATABASE_DELETE_PRIMARY_ATTENDANT":
                                    {
                                        Console.WriteLine("WIZARD - IDS_DLG_DATABASE_DELETE_PRIMARY_ATTENDANT");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_YES"))).Click();
                                        break;
                                    }
                                case "IDS_DLG_DATABASE_SET_PRIMARY_ATTENDANT":
                                    {
                                        Console.WriteLine("WIZARD - IDS_DLG_DATABASE_SET_PRIMARY_ATTENDANT");
                                        list.FindElement(By.Name(Program.GETSTRING("IDS_BTN_NO"))).Click();
                                        break;
                                    }
                                default:
                                    {
                                        Console.WriteLine("default1");
                                        Thread.Sleep(1000);
                                        break;
                                    }
                            }
                            break;
                        }
                    case "IDS_DLG_GENERAL_POPUP":
                        {
                            Console.WriteLine("IDS_DLG_GENERAL_POPUP");
                            //window.FindElement(By.Name(Program.GETSTRING("IDS_BTN_NO"))).Click();
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
                            break;
                        }
                }
                if (list.GetAttribute("Name") == session + " - " + Program.GETSTRING("IDS_DLG_MITEL_DB_PROGRAMMING"))
                {
                    DBPisReady = true;
                    mainwindow = driver.FindElementByName(session + " - " + Program.GETSTRING("IDS_DLG_MITEL_DB_PROGRAMMING"));
                    driver.SwitchTo().Window(session + " - " + Program.GETSTRING("IDS_DLG_MITEL_DB_PROGRAMMING"));
                }
            }
            ListOfWindows = Program.GETListOfWindows();
            foreach (IWebElement list in ListOfWindows)
            {
                if (list.GetAttribute("Name") == session + " - " + Program.GETSTRING("IDS_DLG_MITEL_DB_PROGRAMMING"))
                {
                    DBPisReady = true;
                    mainwindow = driver.FindElement(By.Name(session + " - " + Program.GETSTRING("IDS_DLG_MITEL_DB_PROGRAMMING")));
                }
            }

        }

    }
}
