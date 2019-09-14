using KarmasisPresentation.ViewModels;
using Microsoft.Practices.Unity;
using System.Windows;

namespace KarmasisDesktop.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel viewModel = null;
        public MainWindow(IUnityContainer container)
        {
                InitializeComponent();
                viewModel = container.Resolve<MainWindowViewModel>();
                this.DataContext = viewModel; 
        }
    }
}
