using KarmasisPresentation.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace KarmasisDesktopCore
{
    public class Timer : System.Timers.Timer, ITimer
    {
        public new event EventHandler<KarmasisPresentation.Services.ElapsedEventArgs> Elapsed;

        public Timer()
        {
            base.Elapsed += Base_Elapsed;
        }

        public Timer(double interval)
          : base(interval)
        {
            base.Elapsed += Base_Elapsed;
        }

        public void Reset()
        {
            Stop();
            Start();
        }

        private bool m_disposed = false;
        protected override void Dispose(bool disposing)
        {
            if (m_disposed)
            {
                return;
            }

            base.Elapsed -= Base_Elapsed;
            m_disposed = true;

            base.Dispose(disposing);
        }

        private void Base_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Elapsed != null)
            {
                Elapsed(this, new KarmasisPresentation.Services.ElapsedEventArgs(e.SignalTime));
            }
        }
    }
}
