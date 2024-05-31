using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDPSessionManager;
namespace ExpirePassword_batch
{
    class Program
    {
        static void Main(string[] args)
        {
            string currentUser = Environment.UserName;
            Console.WriteLine($"currentUser : {currentUser}");
            UserAccountManager.ExpirePasswordOnNextLogon(currentUser);
            System.Threading.Thread.Sleep(2000);
            //Console.ReadKey();
        }
    }
}
