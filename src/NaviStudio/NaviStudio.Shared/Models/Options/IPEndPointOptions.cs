using System.ComponentModel.DataAnnotations;
using System.Net;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NaviStudio.Shared.Models.Options;

public partial class IPEndPointOptions : ObservableValidator
{
    #region Public Constructors

    public IPEndPointOptions() { }

    #endregion Public Constructors

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [RegularExpression("^((25[0-5]|(2[0-4]|1\\d|[1-9]|)\\d)\\.?\\b){4}$", ErrorMessage = "非法地址")]
    [Required(ErrorMessage = "不能为空")]
    string _address = IPAddress.None.ToString();

    [ObservableProperty]
    [Range(0, IPEndPoint.MaxPort)]
    [NotifyDataErrorInfo]
    int _port = IPEndPoint.MaxPort;


    #region Public Methods

    public IPEndPoint ToIPEndPoint() => new(IPAddress.Parse(Address), Port);

    #endregion Public Methods
}
