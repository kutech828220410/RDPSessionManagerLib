using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
namespace RDPSessionManager
{
    public class UserAccountManager
    {
        static private string domainName = "";

        /// <summary>
        /// 設置使用者密碼。
        /// </summary>
        /// <param name="userName">使用者名稱。</param>
        /// <param name="newPassword">新密碼。</param>
        static public void SetUserPassword(string userName, string newPassword)
        {
            using (DirectoryEntry userEntry = new DirectoryEntry($"WinNT://{Environment.MachineName}/{userName},user"))
            {
                userEntry.Invoke("SetPassword", new object[] { newPassword });
                userEntry.CommitChanges();
                Console.WriteLine($"Password for user {userName} has been set successfully.");
            }
        }
        public static void SetUserPasswordEx(string username, string newPassword)
        {
            using (PrincipalContext context = new PrincipalContext(ContextType.Machine))
            {
                UserPrincipal user = UserPrincipal.FindByIdentity(context, username);
                if (user != null)
                {
                    user.SetPassword(newPassword);
                    user.Save();
                    Console.WriteLine($"Password for user {username} has been set.");
                }
                else
                {
                    Console.WriteLine($"User {username} not found.");
                }
            }
        }
        /// <summary>
        /// 設置使用者密碼過期時間。
        /// </summary>
        /// <param name="userName">使用者名稱。</param>
        /// <param name="minsUntilExpiration">密碼過期時間。</param>
        static public void SetPasswordExpiration(string userName, int minsUntilExpiration)
        {
            using (DirectoryEntry userEntry = new DirectoryEntry($"WinNT://{Environment.MachineName}/{userName},user"))
            {
                // 計算過期日期
                DateTime expirationDate = DateTime.Now.AddMinutes(minsUntilExpiration);
                // 設置帳戶過期日期
                userEntry.Properties["PasswordExpirationDate"].Value = expirationDate;
                userEntry.CommitChanges();
                Console.WriteLine($"Password for user {userName} will expire on {expirationDate}.");
            }
        }

        static public void ExpirePasswordOnNextLogon(string username)
        {
            using (PrincipalContext context = new PrincipalContext(ContextType.Machine))
            {
                UserPrincipal user = UserPrincipal.FindByIdentity(context, username);
                if (user != null)
                {
                    user.ExpirePasswordNow();
                    user.Save();
                    Console.WriteLine($"Password for user {username} will expire on next logon.");
                }
                else
                {
                    Console.WriteLine($"User {username} not found.");
                }
            }
        }

        static public void DisableAccountAfterFirstLogon(string username)
        {
            using (PrincipalContext context = new PrincipalContext(ContextType.Machine))
            {
                UserPrincipal user = UserPrincipal.FindByIdentity(context, username);
                if (user != null)
                {
                    user.Enabled = false;
                    user.Save();
                    Console.WriteLine($"User account {username} has been disabled after first logon.");
                }
                else
                {
                    Console.WriteLine($"User {username} not found.");
                }
            }
        }

    }
}
