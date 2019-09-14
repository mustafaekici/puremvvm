using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarmasisPresentation.Services;
using Microsoft.Practices.Unity;
using KarmasisDomain.Repositories;
using System.Windows.Input;
using KarmasisPresentation.Commands;
using KarmasisDomain.Services;
using KarmasisDomain;
using KarmasisPresentation.Helpers;

namespace KarmasisPresentation.ViewModels
{
    public class TopProductChartViewModel : ViewModelBase
    {

        #region Fields
        TicketRepository ticketrepo;
        #endregion

        #region Properties
        RangeEnabledObservableCollection<ChartEntity> topProductsList;
        public RangeEnabledObservableCollection<ChartEntity> TopProductsList
        {
            get
            {
                return this.topProductsList;
            }
            set
            {
                this.topProductsList = value;
                RaisePropertyChanged("TopProductsList");
            }
        }
        #endregion

        #region Commands
        private ICommand getChartDataCommand;
        public ICommand GetChartDataCommand
        {
            get
            {
                if (this.getChartDataCommand == null)
                {
                    this.getChartDataCommand = new AsyncCommand(async () =>
                    {
                          await GetChartDataAsync();
                    });
                }
                return this.getChartDataCommand;
            }
        }
        #endregion

        #region Constructor
        public TopProductChartViewModel(IUnityContainer container, IDispatcher dispatcher) : base(dispatcher)
        {
            ticketrepo = container.Resolve<TicketRepository>();
            topProductsList = new RangeEnabledObservableCollection<ChartEntity>();
        }
        #endregion

        #region Delegates
        private async Task GetChartDataAsync()
        {
            TicketService tservice = new TicketService(ticketrepo);
            IEnumerable<ChartEntity> tops = await tservice.TopProblemProductsAsync();
            await Task.Run(() =>
            {
                try
                {
                    dispatcher.Invoke(() =>
                    {
                        topProductsList.Clear();
                        topProductsList.AddRange(tops);
                    });
                }
                catch (Exception)
                {
                    //TODO: Publish operation failed
                }
            });
        }
        #endregion
    }
}
