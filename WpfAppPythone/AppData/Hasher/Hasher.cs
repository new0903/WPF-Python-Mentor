﻿using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppPythone.AppData.Hasher
{
    public static class Hasher
    {
        private readonly static int _iterCount = 1000;
        private static RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
        public static string HashPasswordV3(string password)
        {
            int saltSize = 128 / 8;
            // Produce a version 3 (see comment above) text hash.
            byte[] salt = new byte[saltSize];
            randomNumberGenerator.GetBytes(salt);
            byte[] subkey = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA512, _iterCount, 256 / 8);

            var outputBytes = new byte[13 + salt.Length + subkey.Length];
            outputBytes[0] = 0x01; // format marker
            WriteNetworkByteOrder(outputBytes, 1, (uint)KeyDerivationPrf.HMACSHA512);
            WriteNetworkByteOrder(outputBytes, 5, (uint)_iterCount);
            WriteNetworkByteOrder(outputBytes, 9, (uint)saltSize);
            Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
            Buffer.BlockCopy(subkey, 0, outputBytes, 13 + saltSize, subkey.Length);
            return Convert.ToBase64String(outputBytes);
        }

        public static bool VerifyHashedPasswordV3(string HashedPassword, string password, out int iterCount, out KeyDerivationPrf prf)
        {
            iterCount = default(int);
            prf = default(KeyDerivationPrf);
            try
            {
                byte[] decodedHashedPassword = Convert.FromBase64String(HashedPassword);
                // Read header information
                prf = (KeyDerivationPrf)ReadNetworkByteOrder(decodedHashedPassword, 1);
                iterCount = (int)ReadNetworkByteOrder(decodedHashedPassword, 5);
                int saltLength = (int)ReadNetworkByteOrder(decodedHashedPassword, 9);

                // Read the salt: must be >= 128 bits
                if (saltLength < 128 / 8)
                {
                    return false;
                }
                byte[] salt = new byte[saltLength];
                Buffer.BlockCopy(decodedHashedPassword, 13, salt, 0, salt.Length);

                // Read the subkey (the rest of the payload): must be >= 128 bits
                int subkeyLength = decodedHashedPassword.Length - 13 - salt.Length;
                if (subkeyLength < 128 / 8)
                {
                    return false;
                }
                byte[] expectedSubkey = new byte[subkeyLength];
                Buffer.BlockCopy(decodedHashedPassword, 13 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

                // Hash the incoming password and verify it
                byte[] actualSubkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, subkeyLength);
#if NETSTANDARD2_0 || NETFRAMEWORK
            return ByteArraysEqual(actualSubkey, expectedSubkey);
#elif NETCOREAPP
                return CryptographicOperations.FixedTimeEquals(actualSubkey, expectedSubkey);
#else
#error Update target frameworks
#endif
            }
            catch
            {
                // This should never occur except in the case of a malformed payload, where
                // we might go off the end of the array. Regardless, a malformed payload
                // implies verification failed.
                return false;
            }
        }


        private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
        {
            return ((uint)(buffer[offset + 0]) << 24)
                | ((uint)(buffer[offset + 1]) << 16)
                | ((uint)(buffer[offset + 2]) << 8)
                | ((uint)(buffer[offset + 3]));
        }
        private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
        {
            buffer[offset + 0] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)(value >> 0);
        }
    }
}
