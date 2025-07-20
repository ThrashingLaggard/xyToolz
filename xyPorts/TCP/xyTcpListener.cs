using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using xyToolz;
using xyToolz.Helper.Logging;

namespace xyPorts.TCP
{
    /// <summary>
    /// Handling tcp listeners and connections 
    /// </summary>
    public class xyTcpListener
    {
        /// <summary>
        /// Action for EventHandler
        /// </summary>
        public Action<String> ReceivedMessage { get; set; }

        /// <summary>
        /// Active
        /// </summary>
        public Boolean IsListening { get; set; }
        
        /// <summary>
        /// M
        /// </summary>
        public Dictionary<ushort, TcpListener> TcpPortMapping { get; set; }

        public xyTcpListener()
        {
            TcpPortMapping = new Dictionary<ushort, TcpListener>();
        }

        /// <summary>
        /// Read the content of the buffer into a string
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="bytesRead"></param>
        /// <returns></returns>
        private string GetStringFromBuffer(byte[] buffer, UInt32 bytesRead) => Encoding.ASCII.GetString(buffer, 0, (int)bytesRead);

        /// <summary>
        /// Listen on target port
        /// </summary>
        /// <param name="port_"></param>
        public void Listen(ushort port_)
        {
            TcpListener tcp_PortHost = null;
            try
            {
                // Set the TcpListener on Port 13000 by default
                IPAddress localhost = IPAddress.Parse("127.0.0.1");
                ushort port = port_ != 0 ? port_ : (ushort)13000;

                IPEndPoint listeningAddress;
                IPEndPoint.TryParse($"{localhost}:{port}", out listeningAddress);

                // TcpListener tcp_Porthost = new TcpListener(port);
                using (tcp_PortHost = new TcpListener(listeningAddress))
                {
                    TcpPortMapping.Add(port, tcp_PortHost);
                    // Start listening for client requests
                    tcp_PortHost.Start();
                    IsListening = true;
                    xyLog.Log("Waiting for a connection... ");

                    //  Enter the listening loop
                    while (IsListening)
                    {
                        try
                        {
                            // Perform a blocking call to accept requests.
                            TcpClient tcp_Client = tcp_PortHost.AcceptTcpClient();

                            xyLog.Log("Connected!");
                            GetDataStringFromNetworkStream(tcp_Client);
                        }
                        catch (Exception ex)
                        {
                            xyLog.ExLog(ex, LogLevel.Error);
                        }
                    }
                }
            }
            catch (Exception sE)
            {
                xyLog.ExLog(sE, LogLevel.Error);
            }
            finally
            {
                tcp_PortHost?.Stop();
                xyLog.Log("Listener stopped");
            }
            xyLog.Log(Environment.NewLine + "Hit Enter to continue!");
        }


        /// <summary>
        /// Read the incoming data from a network stream into a string
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>String streamData</returns>
        private string GetIncomingData(NetworkStream stream)
        {
            string data = "";
            UInt32 bytesRead;
            Byte[] buffer = new byte[4096];

            while ((bytesRead = (UInt32)stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                data = GetStringFromBuffer(buffer, bytesRead);
                xyLog.Log($"Received {data}");

                ReceivedMessage?.Invoke(data);

                SendConfirmationResponse(stream);
            }
            return data;
        }

        /// <summary>
        /// Confirm the incoming message to the sender
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="response_"></param>
        public void SendConfirmationResponse(NetworkStream stream, String? response_ = null)
        {
            if (response_ == null)
            {
                response_ = $"Received the message at {DateTime.Now}";
            }
            xyLog.Log(response_);

            Byte[] messageBytes = xy.StringToBytes(response_);
            stream.Write(messageBytes, 0, messageBytes.Length);
        }

        /// <summary>
        /// Read the data from a networkstream into a string
        /// </summary>
        /// <param name="tcpClient_"></param>
        /// <returns></returns>
        private String GetDataStringFromNetworkStream(TcpClient tcpClient_)
        {
            try
            {
                using (NetworkStream stream = tcpClient_.GetStream())
                {
                    String data = GetIncomingData(stream);
                    return data;
                }
            }
            catch (Exception ex)
            {
                xyLog.ExLog(ex);
                return "Error!";
            }
        }

        /// <summary>
        /// Returns the corresponding listener for thew target port
        /// </summary>
        /// <param name="port_"></param>
        /// <returns></returns>
        public TcpListener? GetTcpListenerForPort(ushort port_)
        {
            if (TcpPortMapping.TryGetValue(port_, out TcpListener? tcpTarget))
            {
                return tcpTarget;
            }
            xyLog.Log($"Kein Listener für Port {port_} gefunden!");
            return tcpTarget;
        }

        /// <summary>
        /// Stop and dispose the targeted port's listener
        /// </summary>
        /// <param name="port"></param>
        public void StopListeningTcp(ushort port)
        {
            TcpListener? tcpTarget = GetTcpListenerForPort(port);
            if (tcpTarget != null)
            {
                try
                {
                    tcpTarget?.Stop();
                    xyLog.Log("Listener closed.");
                    tcpTarget?.Dispose();
                    TcpPortMapping.Remove(port);
                }
                catch (Exception e)
                {
                    xyLog.ExLog(e);
                }
            }
        }


        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // Alles ab hier ist obsolet!



        //public void ListenTCP(ushort port_)
        //{
        //    BaseLogger _log = new BaseLogger();
        //    TcpListener tcp_PortHost = null;
        //    try
        //    {
        //        // Set the TcpListener on Port 130000 by default
        //        ushort port = port_ != 0 ? port_ : (ushort)13000;
        //        IPAddress localhost = IPAddress.Parse("127.0.0.1");

        //        IPEndPoint listeningAddress;
        //        IPEndPoint.TryParse($"{localhost}:{port}", out listeningAddress);

        //        // TcpListener tcp_Porthost = new TcpListener(port);
        //        using (tcp_PortHost = new TcpListener(listeningAddress))
        //        {
        //            // Start listening for client requests
        //            tcp_PortHost.Start();
        //            _log.yLog("Waiting for a connection... ");

        //            // Buffer for reading Data
        //            Byte[] buffer = new Byte[1024];
        //            String data = "";
        //            Int16 o = 10;

        //            //  Enter the listening loop
        //            while (o > 0)
        //            {


        //                // Perform a blocking call to accept requests.
        //                // Alternative: tcp_Porthost.AcceptSocket()
        //                using (TcpClient tcp_Client = tcp_PortHost.AcceptTcpClient())
        //                {
        //                    _log.yLog("Connected!");
        //                    data = null;

        //                    // Get a stream-object for reading and writing
        //                    NetworkStream stream = tcp_Client.GetStream();

        //                    int i;

        //                    // Loop to receive all the data sent by a client
        //                    while ((i = stream.Read(buffer, 0, buffer.Length)) != 0)
        //                    {
        //                        // Translate the received bytes into ASCII
        //                        data = Encoding.ASCII.GetString(buffer, 0, i);
        //                        _log.yLog($"Received {data}");

        //                        // Processing the data
        //                        data = data.ToUpper();

        //                        // Store the data
        //                        byte[] msg = Encoding.ASCII.GetBytes(data);

        //                        // Respond
        //                        String response = $"Received the message at {DateTime.Now}";
        //                        Byte[] rmsg = Encoding.ASCII.GetBytes(response);
        //                        stream.Write(rmsg, 0, rmsg.Length);
        //                    }
        //                }
        //                o--;
        //            }
        //        }
        //    }
        //    catch (SocketException sE)
        //    {
        //        _log.ExLog(sE, LogLevel.Error);
        //    }
        //    finally
        //    {
        //        tcp_PortHost?.Stop();
        //        _log.Log("Listener stopped", LogLevel.Information);
        //    }
        //    _log.yLog(Environment.NewLine + "Hit Enter to continue!");
        //}

        //public TcpListener? GetTcpListenerForPortSlow(ushort port_)
        //{
        //    foreach (var port in TcpPortMapping)
        //    {
        //        if (port.Key != port_)
        //        {
        //            sLog.Log(port.Key.ToString());
        //        }
        //        else if (port.Key == port_)
        //        {
        //            return port.Value;
        //        }
        //    }
        //    sLog.Log($"Kein Listener für Port {port_} gefunden!");
        //    return null;
        //}

    }
}
