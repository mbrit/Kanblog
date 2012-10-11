using Kanblog.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace Kanblog.App
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            // boot...
            await KanblogRuntime.StartAsync();

            // navigate...
            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(StreamsPage), args.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            // Ensure the current window is active
            Window.Current.Activate();

            // setup...
            this.ConfigureSettings();
            this.ConfigureSharing();
        }

        private void ConfigureSharing()
        {
            var manager = DataTransferManager.GetForCurrentView();
            manager.DataRequested += manager_DataRequested;
        }

        void manager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            if (Window.Current != null)
            {
                var viewModel = ((Page)((Frame)Window.Current.Content).Content).DataContext as ViewModel;
                if (viewModel != null)
                    viewModel.ShareDataRequested(sender, args);
            }
        }

        private void ConfigureSettings()
        {
            var settings = SettingsPane.GetForCurrentView();
            settings.CommandsRequested += settings_CommandsRequested;
        }

        void settings_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            args.Request.ApplicationCommands.Add(new SettingsCommand("GitHubSource", "GitHub Source", async (e) =>
            {
                await KanblogRuntime.OpenUriAsync(new Uri("https://github.com/mbrit/Kanblog"));
            }));
            args.Request.ApplicationCommands.Add(new SettingsCommand("About", "About", async (e) =>
            {
                var version = Package.Current.Id.Version;
                var dialog = new MessageDialog(string.Format("A Kanban-style tool for blog story planning\r\nVersion {0}.{1}.{2}.{3}\r\nUses the MetroLog and sqlite-net libraries, and the SQLite database\r\nLicenced under the MIT license - https://github.com/mbrit/Kanblog",
                    version.Major, version.Minor, version.Build, version.Revision), "Kanblog");
                await dialog.ShowAsync();
            }));
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
