using System.Net;
using System.Net.Sockets;
using System.Text;
using xyToolz;

namespace xyPorts.UDP
{
    /// <summary>
    /// Little Class to handle UDP connections
    /// </summary>
    public class xyUdpClient
    {

        /// <summary>
        /// Stores the ports and their corresponding UdpClient 
        /// </summary>
        public static Dictionary<ushort, UdpClient> UdpPortMapping { get; set; }

        public xyUdpClient()
        {
            UdpPortMapping = new Dictionary<ushort, UdpClient>();
        }


        /// <summary>
        /// Helper method to convert a the given IP as string & the target port to an IPEndPoint
        /// </summary>
        /// <param name="ip_"></param>
        /// <param name="port_"></param>
        /// <returns>IPEndpoint target</returns>
        public static IPEndPoint ConvertEndpoint(String ip_, UInt16 port_)
        {
            UInt16 port = (port_ == 0) ? (UInt16)12824 : port_;

            if (IPEndPoint.TryParse($"{ip_}:{port}", out IPEndPoint? endPoint))
            {
                xyLog.Log("Endpoint: " + endPoint.ToString());
            }
            else
            {
                xyLog.Log("Please enter a valid IP address");
            }
            return endPoint;
        }

        public static void ConnectUDP(String ip_, UInt16 port_, String message_)
        {
            // Convert the message to bytes
            Byte[] sendBytes = xy.StringToBytes(message_);

            // Convert the IP and port to an IPEndPoint
            if ( ConvertEndpoint(ip_, port_) is IPEndPoint endPoint)
            {
                // Create a new UdpClient for the duration of this operation
                using (UdpClient udpClient = new UdpClient())
                {
                    // Add the UdpClient to the dictionary
                    UdpPortMapping.Add(port_, udpClient);

                    // Allow other applications to use the same port
                    udpClient.Client.ExclusiveAddressUse = false;
                    udpClient.ExclusiveAddressUse = false;

                    try
                    {
                        // Connect to the target IP and port
                        udpClient.Connect(endPoint);

                        // Send the message
                        udpClient.Send(sendBytes, sendBytes.Length);

                        // Wait for the response
                        ReceiveDataUDP(port_, udpClient);
                    }
                    catch (Exception ex)
                    {
                        xyLog.ExLog(ex);
                    }
                }
            }
        }


        /// <summary>
        /// Opens a UDP port and listens for incoming data and returns the data as a string
        /// </summary>
        /// <param name="port_"></param>
        /// <param name="udp_client_"></param>
        /// <returns>String dataFromTargetPort</returns>
        public static String ReceiveDataUDP(UInt16 port_, UdpClient? udp_client_ = null)
        {
            String responseData = "";

            try
            {
                // Lauscht auf allen verfügbaren Netzwerkschnittstellen
                IPEndPoint remoteIPEndpoint = new IPEndPoint(IPAddress.Any, port_);

                UdpClient udpClient = udp_client_ ?? new UdpClient();
                UdpPortMapping.Add(port_, udpClient);
                udpClient.Client.Bind(remoteIPEndpoint);
                var response = udpClient.Receive(ref remoteIPEndpoint);
                responseData = Encoding.ASCII.GetString(response);

                xyLog.Log($"Received data:     {responseData}");
            }
            catch (Exception ex)
            {
                xyLog.ExLog(ex);
            }
            return responseData;
        }


        /// <summary>
        /// Returns the UdpClient for the target port
        /// </summary>
        /// <param name="port_"></param>
        /// <returns> The System.Net....UdpClient </returns>
        private static UdpClient GetClientForPort(UInt16 port_) => (UdpClient)UdpPortMapping.FirstOrDefault(x => x.Key.Equals(port_)).Value;



        /// <summary>
        /// Kill the UdpClient for the given port
        /// </summary>
        /// <param name="port_"></param>
        public void ClosePortUdp(UInt16 port_)
        {
            try
            {
                UdpClient udpClient = GetClientForPort(port_);
                xyLog.Log($"Client found for port {port_}... Trying to stop it:    ");

                if (udpClient != null)
                {
                    udpClient?.Close();
                    udpClient?.Dispose();
                    UdpPortMapping.Remove(port_);
                }
                xyLog.Log("Client was closed and disposed");
            }
            catch (Exception e)
            {
                xyLog.ExLog(e);
            }
        }
    }
}
