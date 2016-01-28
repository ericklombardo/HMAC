﻿using System;
using System.Text;

namespace HMAC
{
    public static class SecurityManager
    {
        private const string Alg = "HmacSHA256";
        private static readonly int _expirationMinutes = 10;

        /// <summary>
        /// Generates a token to be used in impersonate calls.
        /// The token is generated by hashing a message with a key, using HMAC SHA256.
        /// The message is: username:timeStamp
        /// The resulting token is then concatenated with username:timeStamp and the result base64 encoded.
        /// </summary>
        public static string GenerateToken(string username, string key, long ticks)
        {
            string hash = string.Join(":", username, ticks.ToString());
            string hashLeft;
            string hashRight;

            using (System.Security.Cryptography.HMAC hmac = System.Security.Cryptography.HMAC.Create(Alg))
            {
                hmac.Key = Encoding.UTF8.GetBytes(key);
                hmac.ComputeHash(Encoding.UTF8.GetBytes(hash));

                hashLeft = Convert.ToBase64String(hmac.Hash);
                hashRight = string.Join(":", username, ticks.ToString());
            }

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Join(":", hashLeft, hashRight)));
        }

        /// <summary>
        /// Checks if a token is valid.
        /// </summary>
        /// <param name="token">string - generated either by GenerateToken() or via client with cryptojs etc.</param>
        /// <param name="formKey"></param>
        /// <returns>bool</returns>
        public static bool IsTokenValid(string token, string formKey)
        {
            // Base64 decode the string, obtaining the token:username:timeStamp.
            string key = Encoding.UTF8.GetString(Convert.FromBase64String(token));

            // Split the parts.
            string[] parts = key.Split(':');
            if (parts.Length != 3) return false;
            
            // Get the hash message, username, and timestamp.
            string username = parts[1];
            long ticks = long.Parse(parts[2]);
            DateTime timeStamp = new DateTime(ticks);

            // Ensure the timestamp is valid.
            bool expired = Math.Abs((DateTime.UtcNow - timeStamp).TotalMinutes) > _expirationMinutes;
            if (expired) return false;
            //
            // Lookup the user's account from the db.
            //
            // Hash the message with the key to generate a token.
            string computedToken = GenerateToken(username, formKey, ticks);

            // Compare the computed token with the one supplied and ensure they match.
            return (token == computedToken);
        }
    }
}