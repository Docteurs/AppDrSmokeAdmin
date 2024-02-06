
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Graphics.Text;
using System.Collections.Generic;
using System.Net.Http.Headers;

using System.Text;
using System.Text.Json;


namespace DrSmokeAppAdmin.Pages
{

    [QueryProperty(nameof(Uuid), "Uuid")]
    public partial class ProduitDetail : ContentPage
    {

        public bool formVisible = false;
       /* private FileResult result;*/
        private string uuid;
        public List<Models.ProduitAdmin> Items { get; private set; }
        public string Uuid
        {
            get => uuid;
            set
            {
                uuid = value;
                OnPropertyChanged();
            }
        }

        public ProduitDetail()
        {
            InitializeComponent();
            BindingContext = this;

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            HttpClient _client;
            JsonSerializerOptions _serializerOptions;


            if (!string.IsNullOrEmpty(Uuid))
            {
                _client = new HttpClient();
                _serializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };
                string oauthToken = await SecureStorage.Default.GetAsync("oauth_token");
                Items = new List<Models.ProduitAdmin>();
                Uri uri = new Uri(string.Format($"https://get-evolutif.xyz/DrSmokeApi/admin/get-one-produit/{Uuid}"));
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", oauthToken);
                try
                {
                    HttpResponseMessage response = await _client.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        //Items = JsonSerializer.Deserialize<List<Models.ProduitAdmin>>(content, _serializerOptions);
                        var items = JsonSerializer.Deserialize<List<Models.ProduitAdmin>>(content, _serializerOptions);
                        //Items.AddRange(items);
                        if (items.Count > 0 && items != null)
                        {

                            foreach (var item in items)
                            {
             
                                categorieProduit.Text = item.CategorieProduit;
                                imageProduit.Source = item.ImgProduit;
                                nomProduit.Text = item.NomProduit;
                                descriptifProduit.Text = item.Descriptif;
                                PrixProduit1g.Text = $"{item.UnGprix} € / 1g";
                                PrixProduit3g.Text = $"{item.TroisGprix} € / 3g";
                                PrixProduit5g.Text = $"{item.CingGprix} € / 5g";
                                PrixProduit10g.Text = $"{item.DixGprix} € / 10g";
                                PrixProduit20g.Text = $"{item.VingtGprix} € / 20g";
                                quantiteProduitDisponible.Text = $"{item.Quantite} G Disponible";
                                if (item.IsVisible == true)
                                {
                                    produitVisible.Text = "Ce produit est visible par les utilisateur";
                                }
                                else
                                {
                                    produitVisible.Text = "Ce produit n'est pas visible par les utilisateur";
                                }
                            }
                        }
                        else
                        {
                            await DisplayAlert("Alert", "Article Inconnue", "OK");
                        }
                    }

                }
                catch (Exception ex)
                {
                    await DisplayAlert("Alert", ex.ToString(), "OK");
                    await Navigation.PushAsync(new Pages.ConnexionPage());
                }
            }
        }
        private async void GoToUpdateProduct(object sender, EventArgs e)
        {
            var navigationParameter = new ShellNavigationQueryParameters
            {
                { "Uuid", Uuid }
            };

            await Shell.Current.GoToAsync($"UpdateProduit", navigationParameter);

        }
     
        async public void BackToPage(object sender, EventArgs args)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
