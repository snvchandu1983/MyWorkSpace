using System;
using System.Diagnostics;

namespace LAjitExcelCheck
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Checking the number of instances of EXCEL running.");
            int xlCount = GetIntancesRunning();
            if (xlCount > 0)
            {
                Console.WriteLine(xlCount + " instances of EXCEL running. Sending mail to Dev team.");
                SendAlertMail(xlCount);
            }
        }

        private static void SendAlertMail(int xlCount)
        {
            Mail objMailer = new Mail();
            objMailer.Subject = "Excel Alert.";
            objMailer.Body = string.Format("Excel Instances running on server 192.168.1.52 at {0} is {1}." +
                "Please run taskkill to remove all dormant instances of EXCEL in the server."
                , DateTime.Now.ToLongTimeString(), xlCount);
            objMailer.SendMail();

            if (objMailer.ErrorMessage != null && objMailer.ErrorMessage.Length > 0)
            {
                Console.WriteLine(objMailer.ErrorMessage);
            }
        }

        private static int GetIntancesRunning()
        {
            Process[] arrXlProcs = Process.GetProcessesByName("EXCEL");
            return arrXlProcs.Length;
        }
    }
}
