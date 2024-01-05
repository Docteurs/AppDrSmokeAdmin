using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using System.Net.Http.Headers;
using System.Text.Json;

namespace DrSmokeAppAdmin.Pages;

public partial class GestionBoutiqueAdmin : ContentPage
{
	public GestionBoutiqueAdmin()
	{
		InitializeComponent();
        InfoMagasin();
        genereGridAdmin("stephane@gmail.com", "Theiabut", "Stephane", "11 av general lecelerc", "06190", "logodrsmoke_new.webp", "Roquebrune-cap-martin");
    }
    async private protected void InfoMagasin()
    {

        string oauthToken = await SecureStorage.Default.GetAsync("oauth_token");
        HttpClient _client;
        JsonSerializerOptions _serializerOptions;

        _client = new HttpClient();
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        // string url = $"http://localhost:3000/admin/{oauthToken}";
        Uri uri = new Uri(string.Format($"http://localhost:3000/admin/admin/{oauthToken}"));
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", oauthToken);
        try
        {
            var Items = new List<Models.InfoGestionBoutiqueAdmin>();
            HttpResponseMessage response = await _client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                try
                {

                    var items = JsonSerializer.Deserialize<List<Models.InfoGestionBoutiqueAdmin>>(content, _serializerOptions);

                    Items.AddRange(items);
                    foreach (var item in items)
                    {
                        genereGridAdmin(item.Email, item.Nom, item.Prenom, item.Adresse, item.CodePostal.ToString(), item.ImgUrl, item.Ville);
                        //genereGridAdmin(item.Email, item.Nom,  item.Prenom, item.Adresse, item.CodePostal.ToString(), item.ImgUrl,  item.Ville);
                        // await DisplayAlert("Ok", item.Email, "Ok");
                        // await DisplayAlert("Ok", item.CodePostal.ToString(), "Ok");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Alert", $"Une erreur est survenue: {ex.ToString}", "Ok");
                }
            }
            else
            {
                // Traitement à effectuer en cas de réussite de la requête
                string responseContent = await response.Content.ReadAsStringAsync();

                await DisplayAlert("Alert", responseContent, "ok");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Alert", $"Une erreur est survenue: {ex.ToString()}", "OK");
        }
    }

    private void genereGridAdmin(string email, string nom, string prenom, string adresse, string codePostal, string imgSource, string ville)
    {
        Label titleLabel = new Label
        {
            Text = "Gestion Magasin",
            // More properties set here to define the Label appearance
        };

        StackLayout stackLayout = new StackLayout();
        stackLayout.Add(new Image { Source = imgSource });
        stackLayout.Add(new Label { Text = email });
        stackLayout.Add(new Label { Text = nom });
        stackLayout.Add(new Label { Text= prenom });
        stackLayout.Add(new Label { Text = adresse });
        stackLayout.Add(new Label { Text = codePostal });
        
        stackLayout.Add(new Label { Text = ville });
        // More Label objects go here

        ScrollView scrollView = new ScrollView();
        scrollView.Content = stackLayout;
        // ...

        Title = "ScrollView as a child layout demo";
        Grid grid = new Grid
        {
            Margin = new Thickness(20),
            RowDefinitions =
            {
                new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) }
            }
        };
        grid.Add(titleLabel);
        grid.Add(scrollView, 0, 1);
        // grid.Add(button, 0, 2);

        Content = grid;
    }

}