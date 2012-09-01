using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Ana.Utils
{
    public static class DeterministicGuid
    {

        //from: http://geekswithblogs.net/EltonStoneman/archive/2008/06/26/generating-deterministic-guids.aspx
        public static Guid GetDeterministicGuid(string input)
        {
            //use MD5 hash to get a 16-byte hash of the string:
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] inputBytes = Encoding.Default.GetBytes(input);
            byte[] hashBytes = provider.ComputeHash(inputBytes);

            //generate a guid from the hash:
            Guid hashGuid = new Guid(hashBytes);

            return hashGuid;
        }
    }
}
