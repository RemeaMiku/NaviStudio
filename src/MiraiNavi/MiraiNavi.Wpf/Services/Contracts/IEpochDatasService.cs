using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiraiNavi.WpfApp.Models.Navigation;

namespace MiraiNavi.WpfApp.Services.Contracts;

public interface IEpochDatasService
{
    public IEnumerable<EpochData> Datas { get; }

    public void Clear();

    public void Update(EpochData epochData);
}
