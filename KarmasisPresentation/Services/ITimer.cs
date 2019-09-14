using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmasisPresentation.Services
{
    public interface ITimer
    {

        event EventHandler<ElapsedEventArgs> Elapsed;
        bool AutoReset { get; set; }
        bool Enabled { get; set; }
        double Interval { get; set; }
        void Start();
        void Stop();
        void Reset();

    }

    public class ElapsedEventArgs : EventArgs
    {
        public ElapsedEventArgs(DateTime signalTime)
        {
            SignalTime = signalTime;
        }

        public DateTime SignalTime { get; private set; }

    }
}
