using AeroGear.OAuth2;
using System.Collections.Generic;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System;
using Windows.Web.Http;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, IWebAuthenticationContinuable
    {
        public static MainPage Current;
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            Current = this;
            Browser.Navigate(new Uri("https://drive.google.com"));
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var config = await GoogleConfig.Create("14059167302-t8brd6ipcg4a7muoiucba3rapvlh63su.apps.googleusercontent.com",
                new List<string>(new string[] { "https://www.googleapis.com/auth/drive" }), "google");

            //  var kcconfig = await KeycloakConfig.Create("shoot-third-party", "https://localhost:8443", "shoot-realm");
            //var config = FacebookConfig.Create("1654557457742519", "9cab3cb953d3194908f44f1764b5b921", 
            //    new List<string>(new string[] { "photo_upload, publish_actions" }), "facebook");

            var module = await AccountManager.AddAccount(config);

            if (await module.RequestAccessAndContinue())
            {
                Status.Text = "Access granted";
                NavigateToDrive(module);
            }
        }

        public async void ContinueWebAuthentication(WebAuthenticationBrokerContinuationEventArgs args)
        {
            var module = await AccountManager.ParseContinuationEvent(args);
            Status.Text = "Access granted - continuation";
            NavigateToDrive(module);
        }

        private void NavigateToDrive(OAuth2Module module)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, new Uri("https://drive.google.com"));
            requestMessage.Headers.Authorization = module.CredentialHeaderValue();
            Browser.NavigateWithHttpRequestMessage(requestMessage);
        }
    }
}
