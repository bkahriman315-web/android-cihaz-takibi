using CommunityToolkit.Mvvm.ComponentModel;

namespace PhoneHub.Desktop.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private string userName = "Bilal Kahriman";

        [ObservableProperty]
        private string appTitle = "PhoneHub Pro - Cihaz İzleme Sistemi";

        public MainViewModel()
        {
        }

        public void Cleanup()
        {
            // Cleanup resources
        }
    }
}
