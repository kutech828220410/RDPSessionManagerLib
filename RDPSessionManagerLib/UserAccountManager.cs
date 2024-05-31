using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
namespace RDPSessionManager
{
    public class UserAccountManager
    {
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

        /// <summary>
        /// 設置使用者密碼過期時間。
        /// </summary>
        /// <param name="userName">使用者名稱。</param>
        /// <param name="daysUntilExpiration">密碼過期的天數。</param>
        static public void SetPasswordExpiration(string userName, int daysUntilExpiration)
        {
            using (DirectoryEntry userEntry = new DirectoryEntry($"WinNT://{Environment.MachineName}/{userName},user"))
            {
                // 計算過期日期
                DateTime expirationDate = DateTime.Now.AddMinutes(daysUntilExpiration);
                // 設置帳戶過期日期
                userEntry.Properties["PasswordExpirationDate"].Value = expirationDate;
                userEntry.CommitChanges();
                Console.WriteLine($"Password for user {userName} will expire on {expirationDate}.");
            }
        }

  
    }
}
