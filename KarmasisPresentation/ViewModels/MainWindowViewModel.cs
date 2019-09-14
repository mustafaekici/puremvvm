using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarmasisPresentation.Services;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using KarmasisDomain.Repositories;
using Microsoft.Practices.Unity;
using KarmasisDomain.Services;
using KarmasisDomain;
using System.Collections.ObjectModel;
using KarmasisPresentation.Commands;
using KarmasisPresentation.Helpers;

namespace KarmasisPresentation.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        #region Fields
        TicketRepository ticketrepo;
        ITimer timer;
        #endregion

        #region Properties
        bool showMain;
        public bool ShowMain
        {
            get
            {
                return showMain;
            }
            set
            {
                showMain = value;
                RaisePropertyChanged("ShowMain");
            }
        }
     
        RangeEnabledObservableCollection<Ticket> ticketList;
        public RangeEnabledObservableCollection<Ticket> TicketList
        {
            get
            {
                return this.ticketList;
            }
            set
            {
                this.ticketList = value;
                RaisePropertyChanged("TicketList");
            }
        }
        Ticket selectedTicket;
        public Ticket SelectedTicket
        {
            get
            {
                return this.selectedTicket;
            }
            set
            {
                this.selectedTicket = value;
                RaisePropertyChanged("SelectedTicket");
            }
        }
        string _result;
        public string Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
                RaisePropertyChanged("Result");
            }
        }
        public LoginControlViewModel LoginControlViewModel { get; private set; }
        public TopProductChartViewModel TopProductChartViewModel { get; private set; }
        public TopSolvedChartViewModel TopSolvedChartViewModel { get; private set; }
        #endregion

        #region Commands

        private ICommand contentControlLoadedCommand;
        public ICommand ContentControlLoadedCommand
        {
            get
            {
                if (this.contentControlLoadedCommand == null)
                {
                    this.contentControlLoadedCommand = new RelayCommand(ContentControlLoaded);
                }
                return this.contentControlLoadedCommand;
            }
        }
        public ICommand GetTicketListCommand { get; private set; }
        public ICommand SolvedTicketCommand { get; private set; }

        private ICommand ticketCellValueChanged;
        public ICommand TicketCellValueChanged
        {
            get
            {
                if (this.ticketCellValueChanged == null)
                {
                    this.ticketCellValueChanged = new RelayCommand(OnTicketCellValueChanged);
                }
                return this.ticketCellValueChanged;
            }
        }

        private ICommand solvedCommand;
        public ICommand SolvedCommand
        {
            get
            {
                if (this.solvedCommand == null)
                {
                    this.solvedCommand = new AsyncCommand(async () =>
                    {
                        if (SelectedTicket != null)
                        {
                            SelectedTicket.Solved = true;
                            await SolvedTicketAsync();
                        }
                    });
                }
                return this.solvedCommand;
            }
        }

        private ICommand unSolvedCommand;
        public ICommand UnSolvedCommand
        {
            get
            {
                if (this.unSolvedCommand == null)
                {
                    this.unSolvedCommand = new AsyncCommand(async () =>
                    {
                        if (SelectedTicket != null)
                        {
                            SelectedTicket.Solved = false;
                            await SolvedTicketAsync();
                        }
                    });
                }
                return this.unSolvedCommand;
            }
        }
        #endregion

        #region Constructor
        public MainWindowViewModel(IUnityContainer container, IDispatcher dispatcher) : base(dispatcher)
        {
            timer = container.Resolve<ITimer>();
            ticketrepo = container.Resolve<TicketRepository>();
            LoginControlViewModel = new LoginControlViewModel(container, dispatcher);
            TopProductChartViewModel = new TopProductChartViewModel(container, dispatcher);
            TopSolvedChartViewModel = new TopSolvedChartViewModel(container, dispatcher);

            GetTicketListCommand = new AsyncCommand(RefreshTicketListAsync);
            SolvedTicketCommand = new AsyncCommand(SolvedTicketAsync);
            ticketList = new RangeEnabledObservableCollection<Ticket>();
         
            ShowMain = false;
        }

        #endregion

        #region Delegates
        private void ContentControlLoaded()
        {
            LoginControlViewModel.PropertyChanged -= LoginControlViewModel_PropertyChanged;
            LoginControlViewModel.PropertyChanged += LoginControlViewModel_PropertyChanged;
        }

        private void LoginControlViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CheckUser")
            {
                if ((sender as LoginControlViewModel).CheckUser == false)
                {
                    ShowMain = true;
                    GetTicketListCommand.Execute(true);
                    TicketService tservice = new TicketService(ticketrepo);
                    if (tservice.CheckDb()) //if db empty
                    {
                        LoadDummy();        //load dummy data
                    }

                    timer.Interval = 10000;
                    timer.Elapsed += Timer_Elapsed;
                    timer.Start();
                }
            }
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await RefreshTicketListAsync();
        }

        private async Task SolvedTicketAsync()
        {
            TicketService tservice = new TicketService(ticketrepo);
            bool response = await tservice.SolvedTicketAsync(SelectedTicket.ID, LoginControlViewModel.UserName, SelectedTicket.Solved);
            if (response)
            {
                var updaterow = TicketList.FirstOrDefault(x => x.ID == SelectedTicket.ID);
                updaterow.SolvedBy = SelectedTicket.Solved == true ? LoginControlViewModel.UserName : null;
            }
        }

        private void OnTicketCellValueChanged()
        {
            SolvedTicketCommand.Execute(true);
        }
       
        private async Task RefreshTicketListAsync()
        {
            TicketService tservice = new TicketService(ticketrepo);
            var result = await tservice.GetAllTicketsAsync();     
            await Task.Run(() =>
            {         
                try
                {               
                    dispatcher.Invoke(() =>
                    {
                        ticketList.Clear();
                        ticketList.AddRange(result);
                    });
                }
                catch (Exception)
                {
                    //TODO: Publish operation failed
                }
            
            });
            TopProductChartViewModel.GetChartDataCommand.Execute(true);
            TopSolvedChartViewModel.GetChartDataCommand.Execute(true);
            //DeleteDummy
            //for (int i = 1; i < 20; i++)
            //{
            //    await tservice.DeleteTicketAsync(i);
            //}
        }

        public void LoadDummy()
        {
            TicketService tservice = new TicketService(ticketrepo);
            tservice.CreateTicket(
   new Ticket()
   {
       ID = 1,
       CustomerName = "John",
       Description = "Hardware Problem",
       ProductName = "Apple",
       Subject = "Hardware",
       TimeCreated = DateTime.Now,
        
       
   });

            tservice.CreateTicket(
         new Ticket()
         {
             ID = 2,
             CustomerName = "Jim",
             Description = "Software Problem",
             ProductName = "Apple",
             Subject = "Software",
             TimeCreated = DateTime.Now

         });

            tservice.CreateTicket(
         new Ticket()
         {
             ID = 3,
             CustomerName = "Jeremy",
             Description = "Hardware Problem",
             ProductName = "Apple",
             Subject = "Hardware",
             TimeCreated = DateTime.Now

         });

            tservice.CreateTicket(
         new Ticket()
         {
             ID = 4,
             CustomerName = "Corey",
             Description = "Hardware Problem",
             ProductName = "IBM",
             Subject = "Hardware",
             TimeCreated = DateTime.Now

         });

            tservice.CreateTicket(
    new Ticket()
    {
        ID = 5,
        CustomerName = "Tyson",
        Description = "Software Problem",
        ProductName = "IBM",
        Subject = "Hardware",
        TimeCreated = DateTime.Now

    });

            tservice.CreateTicket(
new Ticket()
{
    ID = 6,
    CustomerName = "Tyson",
    Description = "Software Problem",
    ProductName = "IBM",
    Subject = "Software",
    TimeCreated = DateTime.Now

});
            tservice.CreateTicket(
new Ticket()
{
ID = 7,
CustomerName = "Bella",
Description = "Hardware Problem",
ProductName = "IBM",
Subject = "Hardware",
TimeCreated = DateTime.Now

});
            tservice.CreateTicket(
new Ticket()
{
ID = 8,
CustomerName = "Ziwa",
Description = "Hardware Problem",
ProductName = "Apple",
Subject = "Hardware",
TimeCreated = DateTime.Now

});
            tservice.CreateTicket(
new Ticket()
{
ID = 9,
CustomerName = "Ashley",
Description = "Hardware Problem",
ProductName = "IBM",
Subject = "Hardware",
TimeCreated = DateTime.Now

});
            tservice.CreateTicket(
new Ticket()
{
ID = 10,
CustomerName = "Tim",
Description = "Hardware Problem",
ProductName = "Intel",
Subject = "Hardware",
TimeCreated = DateTime.Now

});
            tservice.CreateTicket(
new Ticket()
{
ID = 11,
CustomerName = "Bella",
Description = "Software Problem",
ProductName = "Intel",
Subject = "Software",
TimeCreated = DateTime.Now

});
            tservice.CreateTicket(
new Ticket()
{
ID = 12,
CustomerName = "Travis",
Description = "Software Problem",
ProductName = "Intel",
Subject = "Software",
TimeCreated = DateTime.Now

});
            tservice.CreateTicket(
new Ticket()
{
ID = 13,
CustomerName = "Bella",
Description = "Software Problem",
ProductName = "Intel",
Subject = "Software",
TimeCreated = DateTime.Now

});
            tservice.CreateTicket(
new Ticket()
{
ID = 14,
CustomerName = "Bella",
Description = "Software Problem",
ProductName = "Intel",
Subject = "Software",
TimeCreated = DateTime.Now

});
            tservice.CreateTicket(
new Ticket()
{
ID = 15,
CustomerName = "Steve",
Description = "Software Problem",
ProductName = "Intel",
Subject = "Software",
TimeCreated = DateTime.Now

});
            tservice.CreateTicket(
new Ticket()
{
ID = 16,
CustomerName = "Steve",
Description = "Software Problem",
ProductName = "Intel",
Subject = "Software",
TimeCreated = DateTime.Now

});
            tservice.CreateTicket(
new Ticket()
{
ID = 17,
CustomerName = "Adam",
Description = "Software Problem",
ProductName = "Intel",
Subject = "Software",
TimeCreated = DateTime.Now

});
            tservice.CreateTicket(
new Ticket()
{
ID = 18,
CustomerName = "Adam",
Description = "Software Problem",
ProductName = "Intel",
Subject = "Software",
TimeCreated = DateTime.Now

});
            tservice.CreateTicket(
new Ticket()
{
ID = 19,
CustomerName = "Ryan",
Description = "Software Problem",
ProductName = "Apple",
Subject = "Software",
TimeCreated = DateTime.Now

});
        }
        #endregion

    }
}
