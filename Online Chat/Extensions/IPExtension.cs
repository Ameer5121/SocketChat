using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Online_Chat.Extensions
{
    static class IPExtension
    {
        public static async Task<string> GetExternalIP(this string IP)
        {
            WebClient web = new WebClient();
            string externalip = await web.DownloadStringTaskAsync("http://icanhazip.com/");
            return externalip.Substring(0, externalip.Length - 2);
        }

        public static string GetInternallIP(this string IP)
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
