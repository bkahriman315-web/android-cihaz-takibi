using Microsoft.UI.Xaml.Controls;
using PhoneHub.Desktop.ViewModels;

namespace PhoneHub.Desktop
{
    public sealed partial class MainWindow : Window
    {
        private MainViewModel? _viewModel;

        public MainWindow()
        {
            this.InitializeComponent();
            this.ExtendsContentIntoTitleBar = true;
            this.SetTitleBar(AppTitleBar);
            
            _viewModel = new MainViewModel();
            this.DataContext = _viewModel;
        }

        protected override void OnClosed(object sender, WindowEventArgs args)
        {
            _viewModel?.Cleanup();
            base.OnClosed(sender, args);
        }
    }
}
