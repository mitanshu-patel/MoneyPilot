using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MoneyPilot.Shared.Common
{
    public static class PasswordEncryptionHelper
    {
        public static string ComputeSHA256Hash(this string input)
        {
            using (SHA256 sha256 = SHA256Managed.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    builder.Append(hash[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
