using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MiraiNavi.Shared.Models.Options;

namespace MiraiNavi.WpfApp.ViewModels;

public partial class InputOptionsViewModel() : ObservableValidator
{
    public InputOptionsViewModel(InputOptions options) : this()
    {
        Type = options.Type;
        Format = options.Format;
        TcpAddress = options.TcpOptions.Address;
        TcpPort = options.TcpOptions.Port;
        SerialPortName = options.SerialOptions.PortName;
        SerialBaudRate = options.SerialOptions.BaudRate;
        SerialDataBits = options.SerialOptions.DataBits;
        SerialParity = options.SerialOptions.Parity;
        SerialStopBits = options.SerialOptions.StopBits;
        SerialRtsEnable = options.SerialOptions.RtsEnable;
        Validate();
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsTcp))]
    [NotifyPropertyChangedFor(nameof(IsSerial))]
    InputType _type = InputType.Tcp;

    const string _defaultTcpAddress = "0.0.0.0";
    const int _defaultTcpPort = 0;
    const string _defaultSerialPortName = "COM1";
    const int _defaultSerialBaudRate = 9600;
    const int _defaultSerialDataBits = 8;

    partial void OnTypeChanged(InputType value)
    {
        var errors = GetErrors().ToArray();
        foreach (var error in errors)
        {
            switch (error.MemberNames.First())
            {
                case nameof(TcpAddress):
                    TcpAddress = _defaultTcpAddress;
                    break;
                case nameof(TcpPort):
                    TcpPort = _defaultTcpPort;
                    break;
                case nameof(SerialPortName):
                    SerialPortName = _defaultSerialPortName;
                    break;
                case nameof(SerialBaudRate):
                    SerialBaudRate = _defaultSerialBaudRate;
                    break;
                case nameof(SerialDataBits):
                    SerialDataBits = _defaultSerialDataBits;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        Validate();
    }

    public bool IsTcp => Type == InputType.Tcp;

    public bool IsSerial => Type == InputType.Serial;

    [ObservableProperty]
    InputFormat _format = InputFormat.RTCM3;

    [ObservableProperty]
    [Required(ErrorMessage = "不能为空")]
    [RegularExpression("^((25[0-5]|(2[0-4]|1\\d|[1-9]|)\\d)\\.?\\b){4}$", ErrorMessage = "非法地址")]
    [NotifyDataErrorInfo]
    string _tcpAddress = _defaultTcpAddress;

    [ObservableProperty]
    [Required(ErrorMessage = "不能为空")]
    [Range(0, IPEndPoint.MaxPort, ErrorMessage = "非法端口")]
    [NotifyDataErrorInfo]
    int _tcpPort = _defaultTcpPort;

    [ObservableProperty]
    [Required(ErrorMessage = "不能为空")]
    [NotifyDataErrorInfo]
    string _serialPortName = _defaultSerialPortName;

    [ObservableProperty]
    [Required(ErrorMessage = "不能为空")]
    [Range(0, int.MaxValue, ErrorMessage = "非法波特率")]
    [NotifyDataErrorInfo]
    int _serialBaudRate = _defaultSerialBaudRate;

    [ObservableProperty]
    Parity _serialParity = Parity.None;

    [ObservableProperty]
    [Range(5, 8, ErrorMessage = "非法数据位")]
    [NotifyDataErrorInfo]
    int _serialDataBits = _defaultSerialDataBits;

    [ObservableProperty]
    StopBits _serialStopBits = StopBits.One;

    [ObservableProperty]
    bool _serialRtsEnable = false;

    public void Validate()
    {
        ClearErrors();
        switch (Type)
        {
            case InputType.Tcp:
                ValidateTcp();
                break;
            case InputType.Serial:
                ValidateSerial();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void ValidateSerial()
    {
        ValidateProperty(SerialPortName, nameof(SerialPortName));
        ValidateProperty(SerialBaudRate, nameof(SerialBaudRate));
        ValidateProperty(SerialDataBits, nameof(SerialDataBits));
    }

    void ValidateTcp()
    {
        ValidateProperty(TcpAddress, nameof(TcpAddress));
        ValidateProperty(TcpPort, nameof(TcpPort));
    }

    public InputOptions GetInputOptions()
    {
        return new()
        {
            Type = Type,
            Format = Format,
            TcpOptions = new IPEndPointOptions()
            {
                Address = TcpAddress,
                Port = TcpPort,
            },
            SerialOptions = new SerialPortOptions()
            {
                BaudRate = SerialBaudRate,
                DataBits = SerialDataBits,
                Parity = SerialParity,
                PortName = SerialPortName,
                RtsEnable = SerialRtsEnable,
                StopBits = SerialStopBits
            }
        };
    }

    public bool TryGetInputOptions([NotNullWhen(true)] out InputOptions? options)
    {
        if (HasErrors)
        {
            options = default;
            return false;
        }
        options = GetInputOptions();
        return true;
    }
}
