using Microsoft.UI.Xaml.Controls;
using PhoneHub.Desktop.Views;

namespace PhoneHub.Desktop
{
    public sealed partial class MainWindow : Window
    {
        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer is NavigationViewItem item)
            {
                string tag = item.Tag?.ToString() ?? "";
                var pageType = tag switch
                {
                    "Devices" => typeof(DevicesPage),
                    "ScreenCapture" => typeof(ScreenCapturePage),
                    "SilentMode" => typeof(SilentModePage),
                    "FileManager" => typeof(FileManagerPage),
                    "Location" => typeof(LocationPage),
                    "Notifications" => typeof(NotificationsPage),
                    _ => null
                };

                if (pageType != null)
                {
                    ContentFrame.Navigate(pageType);
                }
            }
        }
    }
}
