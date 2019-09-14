using KarmasisPresentation.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmasisPresentation.ViewModels
{
    public abstract class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {

   
        protected bool isBusy;
        protected IDispatcher dispatcher;

        protected bool isDirty;
        
        public ViewModelBase(IDispatcher dispatcher)
        {

            if (dispatcher == null)
            {
                throw new ArgumentNullException("dispatcher");
            }

            this.dispatcher = dispatcher;
            isBusy = false;
        }

    }
}
