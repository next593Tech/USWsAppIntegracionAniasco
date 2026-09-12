using System;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using USWsSync_UI.Pages;

namespace USWsSync_UI;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);
        AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
        AppWindow.SetIcon("Assets/AppIcon.ico");
        AppWindow.Resize(new Windows.Graphics.SizeInt32(1180, 780));

        NavView.IsPaneOpen = true;
        NavFrame.Navigate(typeof(DownloadPage));
    }

    private void TitleBar_PaneToggleRequested(TitleBar sender, object args)
    {
        NavView.IsPaneOpen = !NavView.IsPaneOpen;
    }

    private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        Type? targetType = null;
        if (args.IsSettingsSelected)
        {
            targetType = typeof(SettingsPage);
        }
        else if (args.SelectedItem is NavigationViewItem item)
        {
            targetType = item.Tag?.ToString() switch
            {
                "download" => typeof(DownloadPage),
                "upload" => typeof(UploadPage),
                "monitor" => typeof(MonitorPage),
                "about" => typeof(AboutPage),
                _ => typeof(DownloadPage)
            };
        }

        if (targetType != null && NavFrame.CurrentSourcePageType != targetType)
        {
            NavFrame.Navigate(targetType);
            NavFrame.BackStack.Clear();
        }
    }
}
