using System;
using static System.Console;

namespace HMAC
{
    class Program
    {
        static void Main(string[] args)
        {
            var hashPassword = SecurityManager.GetHashedPassword("password");
            var token = SecurityManager.GenerateToken("elnunez", hashPassword, DateTime.UtcNow.Ticks);

            WriteLine(token);
            ReadKey();
            WriteLine($"Is valid token: {SecurityManager.IsTokenValid(token)}");
            ReadKey();
        }
    }
}
