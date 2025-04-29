using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using xyPorts.UDP;
using xyToolz;

namespace xyPorts.TCP
{
    /// <summary>
    /// Helper class to send messages via TCP-
    /// </summary>
    public class xyTcpClient
    {
        //    public static void Main(string[] args)
        //    {

        //        //xyTcpListener listener = new xyTcpListener();

        //        //Task.Run(() => { listener.Listen(13000); });

        //        //Task.Delay(2000).Wait();

        //        ConnectTCP("127.0.0.1", 17, "Ahuhu!");
        //    }


        /// <summary>
        /// Get the IPEndPoint for the given IP and port
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static IPEndPoint GetEndPoint(string ip, ushort port) => xyUdpClient.ConvertEndpoint(ip, port);



        /// <summary>
        /// Connect to a target port at a target ip adress and send a message
        /// </summary>
        /// <param name="ip_address"></param>
        /// <param name="port_"></param>
        /// <param name="message"></param>
        public static void ConnectTCP(string ip_address, ushort port_, string message)
        {
            // Check and store the passed IP-address and port, set default value if needed
            ushort _port = (port_ != 0) ? port_ : (ushort)13000;

            try
            {
                // Ensure the instance is disposed later
                using (TcpClient client = new())
                {
                    client.Connect(GetEndPoint(ip_address, port_));
                    xyLog.Log("Created a client and built the connection!");

                    // Get a client stream for reading and writing
                    NetworkStream stream = client.GetStream();

                    // Send the message 
                    SendMsgTcp(client, stream, message);

                    // Receive Server Response      
                    String response = ReceiveResponseTCP(stream);

                }
            }
            catch (Exception e)
            {
                xyLog.ExLog(e);
            }
            xyLog.Log(Environment.NewLine + Environment.NewLine + "Press Enter to continue!" + Environment.NewLine);
        }

        /// <summary>
        /// Send a message 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="stream"></param>
        /// <param name="message"></param>
        public static void SendMsgTcp(TcpClient client, NetworkStream stream, string message)
        {
            // Translate the passed data into ASCII and store it 
            Byte[] buffer = xy.StringToBytes(message);

            // Send the message to the connected TcpServer
            stream.Write(buffer, 0, buffer.Length);
            xyLog.Log($"Sent message: {message}");
        }

        /// <summary>
        /// Get the confirmation message
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ReceiveResponseTCP(NetworkStream stream)
        {
            // Store the response data 
            Byte[] buffer = new Byte[4096];

            // Read the first batch of the TcpServer response bytes
            int byteCount = stream.Read(buffer, 0, buffer.Length);
            String responseData = Encoding.ASCII.GetString(buffer, 0, byteCount);
            xyLog.Log("#Response from Listener: " + responseData);
            return responseData;
        }

    }

}
