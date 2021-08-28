using System;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security;

namespace Services.Utilities
{
    public static class SystemInfo
    {
        public static string GetMacAddress()
        {
            try
            {
                return NetworkInterface
                    .GetAllNetworkInterfaces().Where(nic =>
                        nic.OperationalStatus == OperationalStatus.Up &&
                        nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    .Select(nic => nic.GetPhysicalAddress().ToString())
                    .FirstOrDefault();
            }
            catch (SecurityException e)
            {
                throw new Exception("xxx", e);
            }
        }

        public static string GetClientPublicPort()
        {
            var tcpListener = new TcpListener(IPAddress.Loopback, 0);
            tcpListener.Start();
            var port = ((IPEndPoint) tcpListener.LocalEndpoint).Port;
            tcpListener.Stop();
            return port.ToString();
        }

        public static string GetDeviceModel()
        {
            var deviceModel = "";
            var query =
                new SelectQuery(@"Select * from Win32_ComputerSystem");

            using var searcher =
                new ManagementObjectSearcher(query);
            foreach (var o in searcher.Get())
            {
                var process = (ManagementObject) o;
                process.Get();
                deviceModel = process["Model"].ToString();
            }

            return deviceModel;
        }

        public static string GetDeviceManufacturer()
        {
            var deviceManufacturer = "";
            var query =
                new SelectQuery(@"Select * from Win32_ComputerSystem");

            using var searcher =
                new ManagementObjectSearcher(query);
            foreach (var o in searcher.Get())
            {
                var process = (ManagementObject) o;
                process.Get();
                deviceManufacturer = process["Manufacturer"].ToString();
            }

            return deviceManufacturer;
        }

        public static int GetMonitorCount()
        {
            return new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Service = 'monitor'"
            ).Get().Count;
        }

        /// <summary>
        /// Get Operate System Info
        /// </summary>
        /// <returns>Microsoft Windows 10.0.19043</returns>
        public static string GetOsInfo()
        {
            return System.Runtime.InteropServices.RuntimeInformation.OSDescription;
        }

        public static string GetPublicIp()
        {
            var publicIp = string.Empty;
            try
            {
                var requestToIfconfigMe = (HttpWebRequest) WebRequest.Create("https://ifconfig.me/ip");
                requestToIfconfigMe.UserAgent = "curl";
                requestToIfconfigMe.Method = "GET";

                using var responseOfIfconfigMe = requestToIfconfigMe.GetResponse();
                using var readerOfIfconfigMe = new StreamReader(responseOfIfconfigMe.GetResponseStream() ??
                                                                throw new InvalidOperationException());
                // throw new Exception();
                publicIp = readerOfIfconfigMe.ReadToEnd() + " from ifconfig.me";
                // if (string.IsNullOrWhiteSpace(publicIp))
                // {
                //     throw new Exception();
                // }
            }
            catch
            {
                if (string.IsNullOrWhiteSpace(publicIp))
                {
                    var request = (HttpWebRequest) WebRequest.Create("https://ifconfig.io/ip");
                    request.UserAgent = "curl";
                    request.Method = "GET";

                    using var response = request.GetResponse();
                    using var reader =
                        new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException());
                    publicIp = reader.ReadToEnd().Trim() + " from ifconfig.io";
                }
            }

            return publicIp;
        }

        public static string GetLocalIp()
        {
            return Dns.GetHostName();
        }
    }
}