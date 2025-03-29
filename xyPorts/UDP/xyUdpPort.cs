using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using xyPorts.Basix;

namespace xyPorts.UDP
{
    /// <summary>
    /// Provides basic information about a UDP port and some additional operations via the xyUdpClient class   
    /// </summary>
    public class xyUdpPort : xyPort
    {
        /// <summary>
        /// System.Net.Sockets.UdpClient
        /// </summary>
        public UdpClient UdpClient { get; set; }

        /// <summary>
        /// Custom class to handle UDP connections
        /// </summary>
        public xyUdpClient xyUdpClient { get; set; }

        /// <summary>
        /// System.Net.NetworkInformation.UdpStatistics
        /// </summary>
        public UdpStatistics UdpStatistics { get; set; }

        /// <summary>
        /// System.Net.Sockets.UdpReceiveResult
        /// </summary>
        public UdpReceiveResult UdpReceiveResult { get; set; }
    }
}
