using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;
using RDPSessionManager;
using MyEmail;

namespace sendemail_batch
{
    class Program
    {
        static void Main(string[] args)
        {

            string[] commandLineArgs = Environment.GetCommandLineArgs();
            string name = commandLineArgs[1];
            //string name = "momo";
            name = CapitalizeFirstLetter(name);
            if (name.ToUpper() != "Thomas".ToUpper() && name.ToUpper() != "Evan".ToUpper() && name.ToUpper() != "Momo".ToUpper() && name.ToUpper() != "Chris".ToUpper())
            {
                Console.WriteLine($"使用者名稱錯誤 : {name}");
                Environment.ExitCode = -2;
                //return "使用者名稱錯誤";
                return;
            }
            string user = "hson_rdp";
            string pwd = PasswordGenerator.GeneratePassword(20);

            try
            {
                UserAccountManager.SetUserPasswordEx(user, pwd);
                RDPLogout.LogoutAllRDPSessions();
                Environment.ExitCode = -3;

            }
            catch (Exception ex)
            {

                Console.WriteLine($"RDPSessionManager set error: {ex.Message}");
                Environment.ExitCode = -3;
                return;
            }

           List<string> cc = new List<string>();
           List<string> recipients = new List<string>
            {
                "hson_evan@outlook.com",
                //"hson_dell@outlook.com",
                //"hson_UI@outlook.com",
                //"hongsensales1@outlook.com",
            };
            if (name.ToUpper() == "Thomas".ToUpper())
            {
                cc.Add("hson_UI@outlook.com");
            }
            if (name.ToUpper() == "Momo".ToUpper())
            {
                cc.Add("hson_dell@outlook.com");
            }
            if (name.ToUpper() == "Chris".ToUpper())
            {
                cc.Add("hongsensales1@outlook.com");
            }
            string subject = "[鴻森智能科技] 密碼更動";
            string body = $"Dear {name}:\n\nYour account password has been successfully changed.\nID : {user} \npassword : {pwd}\n\nBest regards,\n鴻森智能科技有限公司 Corp.";
            string attachmentPath = ""; // 替换为附件的实际路径

            try
            {
                // 使用你的 Outlook 帐号信息初始化 EmailSender
                EmailSender emailSender = new EmailSender("smtp-mail.outlook.com", 587, "hson-service@outlook.com", "KuT1Ch@75511");
                emailSender.SendEmail(recipients,cc, subject, body, true);
                Console.WriteLine("Emails sent successfully.");
                Environment.ExitCode = 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending emails: {ex.Message}");
                Environment.ExitCode = -1;

            }
        }
        public static string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return char.ToUpper(input[0]) + input.Substring(1);
        }
    }
}
