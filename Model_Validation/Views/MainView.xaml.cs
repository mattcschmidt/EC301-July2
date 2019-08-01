using MahApps.Metro.Controls;
using Model_Validation.ViewModels;

namespace Model_Validation.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : MetroWindow
    {
        private MainViewModel _mainViewModel;

        public MainView(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            this.DataContext = _mainViewModel;
            this.Closing += MainView_Closing;
            InitializeComponent();
        }

        private void MainView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _mainViewModel.OnCloseApplication();
        }
    }
}
