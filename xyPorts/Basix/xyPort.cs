namespace xyPorts.Basix
{
    /// <summary>
    /// Basic class to store information about a port and its status (listening or not/ exclusive use)
    /// also contains a reference to the xyPortChecker class for basic port-checking operations
    /// </summary>
    public class xyPort
    {
        /// <summary>
        /// Port number
        /// </summary>
        public UInt16 PortID { get; set; }

        /// <summary>
        /// Allready has someone using it
        /// </summary>
        public Boolean IsListening { get; set; }

        /// <summary>
        /// Only allows one application to use it at a time
        /// </summary>
        public Boolean IsExclusive { get; set; }

        /// <summary>
        /// Store custom information u need here
        /// </summary>
        public String Information { get; set; }

        /// <summary>
        /// Provides basic port-checking operations
        /// </summary>
        public xyPortChecker PortChecker { get; set; }
    }

}
