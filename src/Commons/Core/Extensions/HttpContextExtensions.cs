using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class HttpContextExtensions
    {

        public static string GetRemoteHostIpAddress(HttpContext context)
        {
            //lấy địa IpAddress trong Connection của request client gửi lên
            IPAddress? remoteIpAddress = context.Connection.RemoteIpAddress;
            //biến chứa địa chỉ IP V4
            string ipv4 = "";
            //nếu có giá trị
            if (remoteIpAddress != null)
            {
                //nếu là IP v6 thì chuyển về IP V4
                if (remoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    remoteIpAddress = System.Net.Dns.GetHostEntry(remoteIpAddress).AddressList.First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                }
                //gán địa chỉ IP v4 vào biết kết quả
                ipv4 = remoteIpAddress.ToString();
            }
            //trả về kết quả
            return ipv4;
        }
        public static string GetClientIpAddress(this HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var ipAddress = context.Connection.RemoteIpAddress?.ToString();

            // Check if we are behind a proxy
            if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ipAddress = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            }
            else if (context.Request.Headers.ContainsKey("X-Real-IP"))
            {
                ipAddress = context.Request.Headers["X-Real-IP"].FirstOrDefault();
            }

            return ipAddress ?? "Unknown";
        }

        public static string GetLocalIPAddress()
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(i => i.OperationalStatus == OperationalStatus.Up && i.NetworkInterfaceType != NetworkInterfaceType.Loopback);

            foreach (var networkInterface in networkInterfaces)
            {
                var ipProperties = networkInterface.GetIPProperties();
                var unicastAddresses = ipProperties.UnicastAddresses
                    .Where(a => a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !IPAddress.IsLoopback(a.Address));

                foreach (var unicastAddress in unicastAddresses)
                {
                    return unicastAddress.Address.ToString();
                }
            }

            return "127.0.0.1";
        }
    }

}
