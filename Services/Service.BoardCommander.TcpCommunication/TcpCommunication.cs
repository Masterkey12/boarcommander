using Core.BoardCommander.Exceptions;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Service.BoardCommander.TcpCommunication
{
    public interface ITcpCommunication
    {
        void InitializeConnection(string boardIdentity);
        Task SendDataWithAsync(string boardIdentity, byte[] data);
    }
    public class TcpCommunication : ITcpCommunication
    {
        #region Injections
        private readonly IConfiguration _configuration;
        #endregion
        public TcpCommunication(IConfiguration configuration)
        {
            Client = new TcpClient();
            _configuration = configuration;
        }

        public TcpClient Client { get; private set; }

        public void InitializeConnection(string boardIdentity)
        {
            if(Client == null)
                Client = new TcpClient();

            ServicePointManager.SecurityProtocol = (SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12);
            var section = _configuration.GetSection("Boards").GetSection("Devices").GetSection(boardIdentity);
            if (!_configuration.GetSection("Boards").GetSection("Devices").GetSection(boardIdentity).Exists())
                BoardNotFoundException.ThrowNewException(boardIdentity);
            
            var ip = _configuration.GetSection("Boards").GetSection("Devices").GetSection(boardIdentity)["Ip"];
            var port = _configuration.GetSection("Boards").GetSection("Devices").GetSection(boardIdentity)["Port"];
            var timeout = _configuration.GetSection("Boards").GetSection("Devices").GetSection(boardIdentity)["Timeout"];

            var isConnected = Client.ConnectAsync(ip, Convert.ToInt32(port)).Wait(Convert.ToInt32(timeout));

            if (!isConnected)
                TcpNotConnectedException.ThrowNewException(ip, Convert.ToInt32(port), Convert.ToInt32(timeout));
        }

        public async Task SendDataWithAsync(string boardIdentity, byte[] data)
        {
            if (!Client.Connected)
                InitializeConnection(boardIdentity);

            NetworkStream stream = Client.GetStream();
            await stream.WriteAsync(data, 0, data.Length);

            stream.Close();
            Client.Close();
        }
    }
}