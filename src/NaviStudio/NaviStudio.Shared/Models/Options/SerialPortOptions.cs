using System.ComponentModel.DataAnnotations;
using System.IO.Ports;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NaviStudio.Shared.Models.Options;

public partial class SerialPortOptions : ObservableValidator
{
    [ObservableProperty]
    string _portName = string.Empty;

    [ObservableProperty]
    int _baudRate = 9600;

    [ObservableProperty]
    Parity _parity = Parity.None;

    [ObservableProperty]
    [Range(5, 8, ErrorMessage = "范围为 5-8")]
    [NotifyDataErrorInfo]
    int _dataBits = 8;

    [ObservableProperty]
    StopBits _stopBits = StopBits.One;

    [ObservableProperty]
    bool _rtsEnable = false;
}
