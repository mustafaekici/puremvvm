using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmasisDomain
{
    //Bir Domain objesidir, DomainModelBase classından türeyerek INotifyPropertyChanged yeteneğini propertylerine kazandırır,
    //DataTransfer objesi olarak görev yapar.
    public class ChartEntity: DomainModelBase
    {
        public string Definition { get; set; }
        public long Value { get; set; }
    }
}
