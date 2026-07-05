using Microsoft.UI.Xaml.Controls;
using PhoneHub.Desktop.ViewModels;

namespace PhoneHub.Desktop.Views
{
    public sealed partial class SilentModePage : Page
    {
        private SilentModeViewModel? _viewModel;

        public SilentModePage()
        {
            this.InitializeComponent();
            _viewModel = new SilentModeViewModel();
            this.DataContext = _viewModel;
        }
    }
}
