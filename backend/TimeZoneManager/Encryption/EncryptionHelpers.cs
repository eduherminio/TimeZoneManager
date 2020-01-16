using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace TimeZoneManager.Encryption
{
    public static class EncryptionHelpers
    {
        private static readonly Encoding _encoding = Encoding.UTF8;

        public static string GenerateHash(this string str)
        {
            using (var alg = new SHA512Managed())
            {
                var hash = alg.ComputeHash(_encoding.GetBytes(str));
                return BitConverter.ToString(hash).Replace("-", "");
            }
        }
    }
}
