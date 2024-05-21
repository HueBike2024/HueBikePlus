//------------------------------------------------
// Author:                      Nhan Phan
//
// Copyright 2021 
//------------------------------------------------

using System;
using System.Net;

namespace Core.Helpers
{
    public static class DeviceHelper
    {
        public static string GetIpAddress()
        {
            var ipAddress = string.Empty;
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ipAddress = Convert.ToString(IP);
                }
            }
            return ipAddress;
        }
        public static string GetDeviceName(string ipAddress)
        {
            var deviceName = string.Empty;
            try
            {
                var hostEntry = Dns.GetHostEntry(ipAddress);
                deviceName = hostEntry.HostName;
            }
            catch
            {
                // Machine not found...
            }
            return deviceName;
        }
    }
}
