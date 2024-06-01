using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.DirectoryServices;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;
using RDPSessionManager;
using MyEmail;
namespace UserAccountManagerLib
{


    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string user = "hson_rdp";
            string pwd = PasswordGenerator.GeneratePassword(20);

            UserAccountManager.SetUserPassword(user, pwd);
            //UserAccountManager.ExpirePasswordOnNextLogon(user);
            RDPLogout.LogoutAllRDPSessions();

            List<string> recipients = new List<string>
            {
                "hson_evan@outlook.com",
            };
            List<string> cc = new List<string>
            {
              
            };
            string subject = "[鴻森智能科技] 密碼更動";
            string body = $"\n\nYour account password has been successfully changed.\n ID : {user} \n password : {pwd}\n\nBest regards,\n鴻森智能科技有限公司 Corp.";
            string attachmentPath = ""; // 替换为附件的实际路径

            try
            {
                // 使用你的 Outlook 帐号信息初始化 EmailSender
                EmailSender emailSender = new EmailSender("smtp-mail.outlook.com", 587, "hson-service@outlook.com", "KuT1Ch@75511");
                emailSender.SendEmail(recipients,cc, subject, body, true);
                Console.WriteLine("Emails sent successfully.");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending emails: {ex.Message}");
           
            }
        }
    }
}
