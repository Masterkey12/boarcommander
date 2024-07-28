namespace Core.BoardCommander.Exceptions
{
    public class TcpNotConnectedException : BaseApiException
    {
        public static void ThrowNewException(string ip, int port, int timeout)
            => throw new TcpNotConnectedException(ip, port, timeout);
        public TcpNotConnectedException(string ip, int port, int timeout) : base(400, $"Cannot connect to Board with IP {ip} and PORT {port} in {timeout} milliseconds")
        {
        }
    }
}