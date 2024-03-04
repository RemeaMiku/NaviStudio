using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviStudio.Shared.Models.Options;

public class NtripOptions
{
    public string CasterHost { get; set; } = string.Empty;

    public string MountPoint { get; set; } = string.Empty;

    public int Port { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
