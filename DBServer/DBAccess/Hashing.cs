using System.Security.Cryptography;
using System.Text;

namespace DBServer.DBAccess
{
    public class Hashing
    {
        private static byte[] GetHashArray(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
    
        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHashArray(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}