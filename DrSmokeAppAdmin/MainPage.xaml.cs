using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;

namespace DrSmokeAppAdmin
{
    public partial class MainPage : ContentPage
    {
       
        public MainPage()
        {
            InitializeComponent();
            verfiToken();
        }
        private async void verfiToken()
        {
            string oauthToken = await SecureStorage.Default.GetAsync("oauth_token");

            if (oauthToken == null)
            {
                await Navigation.PushAsync(new Pages.ConnexionPage());
            }
            else
            {
              
                await Navigation.PushAsync(new Pages.GestionBoutiqueAdmin());
            }
             
        }
     
    }
}

