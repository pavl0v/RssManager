using System.Diagnostics;
using System.Security.Principal;
using System.Linq;

namespace RssManager.WebAPI
{
    public static class Helper
    {
        public static void Write(string stage, IPrincipal principal)
        {
            Debug.WriteLine("----- " + stage + " -----");
            if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)
            {
                Debug.WriteLine("Anonymous user");
            }
            else
            {
                Debug.WriteLine("User: " + principal.Identity.Name);
            }
            Debug.WriteLine("\n");
        }

        public static long GetCurrentUserID(IPrincipal principal)
        {
            var claimsIdentity = principal.Identity as System.Security.Claims.ClaimsIdentity;
            if (claimsIdentity == null)
                return 0;
            var claim = claimsIdentity.Claims.Where(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault();
            if (claim == null || string.IsNullOrEmpty(claim.Value))
                return 0;
            long id = long.Parse(claim.Value);
            return id;
        }

        public static string GetHashedString(string plainString)
        {
            System.Security.Cryptography.SHA1CryptoServiceProvider sha1 = 
                new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] b = sha1.ComputeHash(System.Text.Encoding.Default.GetBytes(plainString));
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < b.Length; i++)
            {
                sb.Append(b[i].ToString("X1"));
            }
            return sb.ToString();
        }
    }
}