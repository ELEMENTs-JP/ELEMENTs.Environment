using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELEMENTS
{
    public static class Helper
    {
        public static string ToSecureString(this object strg)
        {
            try
            {
                if (strg == null)
                    return string.Empty;

                return strg.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fail: " + ex.Message);
                return string.Empty;
            }
        }
    }
}
