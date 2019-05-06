using System;
using System.Security.Cryptography;
using System.Text;

namespace RClone_Anime.Encrypt
{
    /// <summary>
    /// Stores password entered on the start of the application encrypted by random string
    /// </summary>
    public class PasswordStore
    {
        private readonly string _s;
        private readonly string _p;

        public PasswordStore(string password)
        {
            _s = RandomString();
            _p = StringCipher.Encrypt(password, _s);
        }

        public string Get()
        {
            return StringCipher.Decrypt(_p, _s);
        }

        private static string RandomString()
        {
            uint scale;
            using (var rng = new RNGCryptoServiceProvider())
            {
                var fourBytes = new byte[4];
                rng.GetBytes(fourBytes);
                scale = BitConverter.ToUInt32(fourBytes, 0);
            }

            return RandomString((int) (50 + (100 - 50) * (scale / (double) uint.MaxValue)));
        }

        private static string RandomString(int length)
        {
            const string valid =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_-=+{}:;\\<>?|,./`~[]'";
            var res = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                var uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    var num = BitConverter.ToUInt32(uintBuffer, 0);
                    res.Append(valid[(int) (num % (uint) valid.Length)]);
                }
            }

            return res.ToString();
        }
    }
}