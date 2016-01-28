using System;
using static System.Console;

namespace HMAC
{
    class Program
    {
        static void Main(string[] args)
        {
            var token = SecurityManager.GenerateToken("elnunez", "asas@$$%89", DateTime.UtcNow.Ticks);

            WriteLine(token);
            ReadKey();
            WriteLine($"Is valid token: {SecurityManager.IsTokenValid(token, "asas@$$%89")}");
            ReadKey();
        }
    }
}
