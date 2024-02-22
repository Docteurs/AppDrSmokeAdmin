using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;

namespace DrSmokeAppAdmin.Pages;

public partial class GestionBoutiqueAdmin : ContentPage
{
    private FileResult result;
    public bool FormIsVisible = false;
    public List<Models.UpdateBoutiqueAdmin> Boutique { get; private set; }
    public GestionBoutiqueAdmin()
	{
		InitializeComponent();
        InfoMagasin();
    }
    async private protected void InfoMagasin()
    {
        base.OnAppearing();
        string oauthToken = await SecureStorage.Default.GetAsync("oauth_token");
        if (oauthToken == null || oauthToken == "")
        {
            await DisplayAlert("Alert", "Vous devez etre connectez", "OK");
            await Shell.Current.GoToAsync("//MainPage");
        }
        // string oauthToken = await SecureStorage.Default.GetAsync("oauth_token");
        HttpClient _client;
        JsonSerializerOptions _serializerOptions;

        _client = new HttpClient();
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        // string url = $"http://localhost:3000/admin/{oauthToken}";
        Uri uri = new Uri(string.Format($"https://get-evolutif.xyz/DrSmokeApi/admin/admin/{oauthToken}"));
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", oauthToken);
        try
        {
            var Items = new List<Models.InfoGestionBoutiqueAdmin>();
            HttpResponseMessage response = await _client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
              

                    var items = JsonSerializer.Deserialize<List<Models.InfoGestionBoutiqueAdmin>>(content, _serializerOptions);

                    Items.AddRange(items);
                    foreach (var item in items)
                    {
                      
                        VilleMagasin.Text = item.Ville; 
                        //imgUrlMagasin.Uri = item.ImgUrl; 
                        imgUrlMagasin.Uri = new Uri($"{item.ImgUrl}"); 
                        adresseMagasin.Text = item.Adresse;
                        prenomMagasin.Text = item.Prenom;
                        nomMagasin.Text = item.Nom;
                        emailMagasin.Text = item.Email;
                        LundiHoraire.Text = $"Lundi: {item.HoraireLundi}";
                        MardiHoraire.Text = $"Mardi: {item.HoraireMardi}";
                        MercrediHoraire.Text = $"Mercredi: {item.HoraireMercredi}";
                        JeudiHoraire.Text = $"Jeudi: {item.HoraireJeudi}";
                        VendrediHoraire.Text = $"Vendredi: {item.HoraireVendredi}";
                        SamediHoraire.Text = $"Samedi: {item.HoraireSamedi}";
                        DimancheHoraire.Text = $"Dimanche: {item.HoraireDimanche}";
                    }
                   
               
            }
            else
            {
                // Traitement à effectuer en cas de réussite de la requête
                string responseContent = await response.Content.ReadAsStringAsync();
             
                var apiResponse = JsonSerializer.Deserialize<Models.ReponseAPI>(responseContent);
                await DisplayAlert("Alert", $"Test ${apiResponse.Message} ", "ok");
                await Navigation.PushAsync(new Pages.ConnexionPage());
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Alert", $"Une erreur est survenue: {ex.ToString()}", "OK");
            await Navigation.PushAsync(new Pages.ConnexionPage());
        }
    }
  
    public async void OnDeconnexion(object sender, EventArgs args)
    {
        try
        {
            SecureStorage.Default.RemoveAll();
            await DisplayAlert("Alert", "Vous avez été déconnecter", "OK");
            await Navigation.PushAsync(new Pages.ConnexionPage());
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Une erreur est survenue lors de la déconnexion : {ex.Message}");
            // Vous pouvez afficher un message d'erreur ou effectuer d'autres actions de gestion des erreurs ici.
        }
    }

    public async void GoToUpdateMagasin(object sender, EventArgs args)
    {
        await Navigation.PushAsync(new Pages.UpdateBoutique());
    }
}