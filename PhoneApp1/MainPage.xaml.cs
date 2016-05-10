using AeroGear.OAuth2;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Navigation;
using Windows.ApplicationModel.Activation;

namespace PhoneApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
        public static MainPage Current;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            Current = this;
            Browser.Navigate(new Uri("https://drive.google.com"));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var app = App.Current as App;
            if (app.ContinuationActivatedEventArgs != null)
            {
                this.ContinueWebAuthentication(app.ContinuationActivatedEventArgs);
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var config = await GoogleConfig.Create("14059167302-t8brd6ipcg4a7muoiucba3rapvlh63su.apps.googleusercontent.com",
                new List<string>(new string[] { "https://www.googleapis.com/auth/drive" }), "google");

            //var kcconfig = await KeycloakConfig.Create("shoot-third-party", "https://localhost:8443", "shoot-realm");
            //var config = FacebookConfig.Create("1654557457742519", "9cab3cb953d3194908f44f1764b5b921", 
            //    new List<string>(new string[] { "photo_upload, publish_actions" }), "facebook");

            var module = await AccountManager.AddAccount(config);

            if (await module.RequestAccessAndContinue())
            {
                Status.Text = "Access granted";
                NavigateToDrive(module);
            }
        }

        private void NavigateToDrive(OAuth2Module module)
        {
            Browser.Visibility = Visibility.Visible;
            Browser.Navigate(new Uri("https://drive.google.com"), null, "Authorization: " + module.AuthorizationHeaderString);
        }

        public async void ContinueWebAuthentication(WebAuthenticationBrokerContinuationEventArgs args)
        {
            var module = await AccountManager.ParseContinuationEvent(args);
            Status.Text = "Access granted - continuation";
            NavigateToDrive(module);
        }
    }
}