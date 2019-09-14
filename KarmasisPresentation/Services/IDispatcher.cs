using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmasisPresentation.Services
{
    public interface IDispatcher
    {
        void Invoke(Action action);
        void BeginInvoke(Action action, params object[] args);

    }
}
