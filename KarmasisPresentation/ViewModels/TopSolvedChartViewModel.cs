using KarmasisDomain;
using KarmasisDomain.Repositories;
using KarmasisDomain.Services;
using KarmasisPresentation.Commands;
using KarmasisPresentation.Helpers;
using KarmasisPresentation.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KarmasisPresentation.ViewModels
{
    public class TopSolvedChartViewModel : ViewModelBase
    {
        #region Fields
        TicketRepository ticketrepo;
        #endregion

        #region Properties
        RangeEnabledObservableCollection<ChartEntity> topSolvedByList;
        public RangeEnabledObservableCollection<ChartEntity> TopSolvedByList
        {
            get
            {
                return this.topSolvedByList;
            }
            set
            {
                this.topSolvedByList = value;
                RaisePropertyChanged("TopSolvedByList");
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
        public TopSolvedChartViewModel(IUnityContainer container, IDispatcher dispatcher) : base(dispatcher)
        {
            ticketrepo = container.Resolve<TicketRepository>();
            topSolvedByList = new RangeEnabledObservableCollection<ChartEntity>();
        }
        #endregion

        #region Delegates
        private async Task GetChartDataAsync()
        {
            TicketService tservice = new TicketService(ticketrepo);
            IEnumerable<ChartEntity> tops = await tservice.TopSolvedByAsync();
            await Task.Run(() =>
            {
                try
                {
                    dispatcher.Invoke(() =>
                    {
                        topSolvedByList.Clear();
                        topSolvedByList.AddRange(tops);
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
