using System.Net;

namespace MiraiNavi.Shared.Models.Options;

public class IPEndPointOptions
{
    #region Public Constructors

    public IPEndPointOptions() { }

    public IPEndPointOptions(string address, int port)
    {
        Address = address;
        Port = port;
    }

    #endregion Public Constructors

    #region Public Properties

    public string Address { get; set; } = IPAddress.None.ToString();

    public int Port { get; set; } = IPEndPoint.MaxPort;

    #endregion Public Properties

    #region Public Methods

    public IPEndPoint ToIPEndPoint() => new(IPAddress.Parse(Address), Port);

    #endregion Public Methods
}
