using System.Net.NetworkInformation;

namespace xyPorts.Basix
{

    /// <summary>
    /// Helper class for checking the status of ports
    /// </summary>
    public class xyPortChecker
    {

        /// <summary>
        /// Helper Method to get the IPGlobalProperties needed to get the active connections
        /// </summary>
        /// <returns></returns>
        public IPGlobalProperties GetIpgProps() => IPGlobalProperties.GetIPGlobalProperties();

        /// <summary>
        /// Get all active TCP connections
        /// </summary>
        /// <returns>IEnumerable[portNumbers]</returns>
        public IEnumerable<int> GetActiveConnections()
        {
            List<int> lst_ActiveTcpConnections = new List<int>();
            var connections = GetIpgProps().GetActiveTcpConnections();

            foreach (var port in connections)
            {
                lst_ActiveTcpConnections.Add(port.LocalEndPoint.Port);

            }
            lst_ActiveTcpConnections.Sort();
            return lst_ActiveTcpConnections;
        }


        /// <summary>
        /// Get all active TCP listeners
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetActiveTcpListeners()
        {
            List<int> lst_ActiveTCPs = new List<int>();
            var connections = GetIpgProps().GetActiveTcpListeners();

            foreach (var port in connections)
            {
                lst_ActiveTCPs.Add(port.Port);
            }
            lst_ActiveTCPs.Sort();
            return lst_ActiveTCPs;
        }

        /// <summary>
        /// Get all active UDP listeners
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetActiveUdpListeners()
        {
            List<int> lst_ActiveUDPs = new List<int>();
            var connections = GetIpgProps().GetActiveUdpListeners();

            foreach (var port in connections)
            {
                lst_ActiveUDPs.Add(port.Port);
            }
            lst_ActiveUDPs.Sort();
            return lst_ActiveUDPs;
        }

        /// <summary>
        /// Get all open ports
        /// </summary>
        /// <returns>IEnumerable[portNumbers]</returns>
        public IEnumerable<int> GetOpenPorts()
        {
            List<int> lst_OpenedPorts = new List<int>();

            lst_OpenedPorts.AddRange(GetActiveConnections());
            lst_OpenedPorts.AddRange(GetActiveTcpListeners());
            lst_OpenedPorts.AddRange(GetActiveUdpListeners());
            lst_OpenedPorts.Sort();

            IEnumerable<int> eOpenPorts = lst_OpenedPorts.Distinct();
            return eOpenPorts;
        }

        /// <summary>
        /// Checks if the given port is already in use
        /// </summary>
        /// <param name="port"></param>
        /// <returns>
        /// True if not used
        /// False if already in used
        /// </returns>
        public bool IsPortAvaillable(int port)
        {
            bool isAvaillable = false;
            try
            {
                var openPorts = GetOpenPorts();
                foreach (var p in openPorts)
                {
                    if (port == p)
                    {
                        Console.WriteLine($" {isAvaillable}:    Port {port} is already in use.");
                        return isAvaillable;
                    }
                }
                isAvaillable = true;
                Console.WriteLine($"{isAvaillable}:    Port {port} is free, use it now!");

                return isAvaillable;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + e.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// Checks the given ports if they are already in use
        /// </summary>
        /// <param name="ports"></param>
        /// <returns>IEnumerable[isAvaillable, portNumber]</returns>
        public IEnumerable<(bool, int)> ArePortsAvaillable(List<int> ports)
        {
            int counter = 0;
            var yetOpen = GetOpenPorts().ToList();
            List<(bool, int)> isAvaillable = new List<(bool, int)>();

            foreach (var port in ports)
            {
                Console.WriteLine(port);
                foreach (var yoP in yetOpen)
                {
                    if (port == yoP)
                    {
                        isAvaillable.Add((false, port));
                    }
                }
            }
            if (isAvaillable.Count == 0)
            {
                isAvaillable.Add((true, 0));
            }
            else
            {
                Console.WriteLine($"These ports are already in use:");
            }
            return isAvaillable;
        }
    }
}
