using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Communication
{
    public static class TcpIp
    {
        #region Public Methods
        public static int GetLocalAvailablePort()
        {
            TcpListener listener = null;
            try
            {
                listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 0);
                listener.Start();
                int port = ((IPEndPoint)listener.LocalEndpoint).Port;
                return port;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                listener.Stop();
            }
        }
        #endregion
    }
}
