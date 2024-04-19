using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NaviStudio.Shared.Models.Options;

public partial class NtripOptions : ObservableValidator
{
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "不能为空")]
    string _casterHost = string.Empty;

    [ObservableProperty]
    string _mountPoint = string.Empty;

    [ObservableProperty]
    [Range(0, IPEndPoint.MaxPort)]
    [NotifyDataErrorInfo]
    int _port;

    [ObservableProperty]
    string _userName = string.Empty;

    [ObservableProperty]
    string _password = string.Empty;

}
