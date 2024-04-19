using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NaviStudio.Shared.Models.Options;

public partial class InputOptions : ObservableValidator
{
    public InputOptions()
    {
        TcpOptions = new();
        SerialOptions = new();
        NtripOptions = new();
    }


    public new bool HasErrors
    {
        get
        {
            var typeOptionsHasErrors = Type switch
            {
                InputType.Tcp => TcpOptions.HasErrors,
                InputType.Serial => SerialOptions.HasErrors,
                InputType.Ntrip => NtripOptions.HasErrors,
                _ => false
            };
            return base.HasErrors || typeOptionsHasErrors;
        }
    }


    [ObservableProperty]
    InputType _type = InputType.Tcp;

    partial void OnTypeChanged(InputType value)
    {
        OnPropertyChanged(nameof(HasErrors));
    }

    [ObservableProperty]
    InputFormat _format = InputFormat.RTCM3;

    [ObservableProperty]
    BaseType _baseType = BaseType.Single;

    [ObservableProperty]
    int _zmqId;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Range(0, int.MaxValue)]
    int _serverCycle = 1000;

    [ObservableProperty]
    string _imuType = "NULL";

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Range(0, int.MaxValue)]
    int _imuRate = 100;

    [ObservableProperty]
    IPEndPointOptions _tcpOptions;

    partial void OnTcpOptionsChanged(IPEndPointOptions value)
    {
        value.ErrorsChanged += (_, _) =>
        {
            if(Type == InputType.Tcp)
                OnPropertyChanged(nameof(HasErrors));
        };
        value.PropertyChanged += (_, _) => OnPropertyChanged(nameof(TcpOptions));
    }


    [ObservableProperty]
    SerialPortOptions _serialOptions;

    partial void OnSerialOptionsChanged(SerialPortOptions value)
    {
        value.ErrorsChanged += (_, _) =>
        {
            if(Type == InputType.Serial)
                OnPropertyChanged(nameof(HasErrors));
        };
        value.PropertyChanged += (_, _) => OnPropertyChanged(nameof(SerialOptions));
    }

    [ObservableProperty]
    NtripOptions _ntripOptions;

    partial void OnNtripOptionsChanged(NtripOptions value)
    {
        value.ErrorsChanged += (_, _) =>
        {
            if(Type == InputType.Ntrip)
                OnPropertyChanged(nameof(HasErrors));
        };
        value.PropertyChanged += (_, _) => OnPropertyChanged(nameof(NtripOptions));
    }
}
