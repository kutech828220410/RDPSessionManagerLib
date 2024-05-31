using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;

namespace RDPSessionManager
{
    public class PasswordGenerator
    {
        private static readonly char[] UpperCaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private static readonly char[] LowerCaseLetters = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
        private static readonly char[] Digits = "0123456789".ToCharArray();
        private static readonly char[] SpecialCharacters = "!@#$%^&*()_+-=[]{}|;:,.<>?".ToCharArray();


        public static string GeneratePassword(int length)
        {
            if (length < 4)
            {
                throw new ArgumentException("Password length should be at least 4 to include all character types.");
            }

            char[] password = new char[length];
            RandomNumberGenerator rng = RandomNumberGenerator.Create();

            // Ensure the password contains at least one character of each type
            password[0] = UpperCaseLetters[GetRandomIndex(rng, UpperCaseLetters.Length)];
            password[1] = LowerCaseLetters[GetRandomIndex(rng, LowerCaseLetters.Length)];
            password[2] = Digits[GetRandomIndex(rng, Digits.Length)];
            password[3] = SpecialCharacters[GetRandomIndex(rng, SpecialCharacters.Length)];

            char[] allCharacters = UpperCaseLetters.Concat(LowerCaseLetters).Concat(Digits).Concat(SpecialCharacters).ToArray();

            for (int i = 4; i < length; i++)
            {
                password[i] = allCharacters[GetRandomIndex(rng, allCharacters.Length)];
            }

            // Shuffle the password to ensure randomness
            string pwd = new string(password.OrderBy(_ => GetRandomInt(rng)).ToArray());
            Console.WriteLine($"pwd : {pwd}");
            return pwd;
        }

        private static int GetRandomIndex(RandomNumberGenerator rng, int max)
        {
            byte[] randomNumber = new byte[4];
            rng.GetBytes(randomNumber);
            return Math.Abs(BitConverter.ToInt32(randomNumber, 0) % max);
        }

        private static int GetRandomInt(RandomNumberGenerator rng)
        {
            byte[] randomNumber = new byte[4];
            rng.GetBytes(randomNumber);
            return BitConverter.ToInt32(randomNumber, 0);
        }
    }
}
