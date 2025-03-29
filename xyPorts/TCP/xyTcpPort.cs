using System.Net.NetworkInformation;
using System.Net.Sockets;
using xyPorts.Basix;

namespace xyPorts.TCP
{
    /// <summary>
    /// Basic class to store information about a TCP port and its status (listening or not/ exclusive use) and 
    /// also contains a reference to the xyTcpListener and xyTcpClient classes for more TCP operations
    /// </summary>
    public class xyTcpPort : xyPort
    {
        /// <summary>
        /// System.Net.Sockets.TcpListener
        /// </summary>
        public TcpListener TcpListener { get; set; }

        /// <summary>
        /// Providing basic TCP-listener and connection operations
        /// </summary>
        public xyTcpListener xyTcpListener { get; set; }

        /// <summary>
        /// System.net.Sockets.TcpClient
        /// </summary>
        public TcpClient TcpClient { get; set; }


        /// <summary>
        /// Providing basic TCP-client and message sending operations
        /// </summary>
        public xyTcpClient xyTcpClient { get; set; }

        /// <summary>
        ///  System.Net.NetworkInformation.TcpConnectionInformation
        /// </summary>
        public TcpConnectionInformation ConnectionInformation { get; set; }

        /// <summary>
        /// System.Net.NetworkInformation.TcpStatistics
        /// </summary>
        public TcpStatistics Statistics { get; set; }
    }
}
