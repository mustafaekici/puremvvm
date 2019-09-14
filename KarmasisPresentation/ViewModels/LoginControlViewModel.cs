using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarmasisPresentation.Services;
using Microsoft.Practices.Unity;
using KarmasisDomain.Repositories;
using System.Windows.Input;
using KarmasisDomain.Services;
using GalaSoft.MvvmLight.Command;
using System.Windows.Controls;

namespace KarmasisPresentation.ViewModels
{
    public class LoginControlViewModel : ViewModelBase
    {
      
        #region Fields
        TicketRepository ticketrepo;
        #endregion

        #region Properties

        bool checkUser;
        public bool CheckUser
        {
            get
            {
                return checkUser;
            }
            set
            {
                checkUser = value;
                RaisePropertyChanged("CheckUser");
            }
        }

        string userName;
        public string  UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
                RaisePropertyChanged("UserName");
            }
        }

        #endregion

        #region Commands

        private ICommand _loginButtonCommand;
        public ICommand LoginButtonCommand
        {
            get
            {
                if (this._loginButtonCommand == null)
                {
                    this._loginButtonCommand = new RelayCommand<object>(OnLoginButtonCommand);
                }
                return this._loginButtonCommand;
            }
        }
        #endregion

        #region Constructor
        public LoginControlViewModel(IUnityContainer container, IDispatcher dispatcher) : base(dispatcher)
        {
            ticketrepo = container.Resolve<TicketRepository>();
            CheckUser = true;
        }
        #endregion

        #region Delegates
        private void OnLoginButtonCommand(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            var password = passwordBox.Password;
            TicketService ts = new TicketService(ticketrepo);
            bool result = ts.Login(new KarmasisDomain.User() { UserName =this.UserName, Pass = password });
            CheckUser = !result;
        }
        #endregion
    }
}
