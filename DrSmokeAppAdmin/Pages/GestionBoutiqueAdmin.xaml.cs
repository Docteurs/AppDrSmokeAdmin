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
        // genereGridAdmin("stephane@gmail.com", "Theiabut", "Stephane", "11 av general lecelerc", "06190", "logodrsmoke_new.webp", "Roquebrune-cap-martin");
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
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 10)
        };

        Image pageImage = new Image
        {
            Source = "logodrsmoke_new.webp", // Remplacez "chemin_de_votre_image" par le chemin de votre image
            WidthRequest = 400, // Ajustez la taille de l'image selon vos besoins
            HeightRequest = 400,
            Aspect = Aspect.AspectFill,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 40) // Ajoutez de la marge en haut pour aérer visuellement
        };

        Image cardImage = new Image
        {
            Source = imgSource,
            WidthRequest = 400, // Ajustez la taille de l'image selon vos besoins
            HeightRequest = 400,
            Aspect = Aspect.AspectFill
        };

        Label emailLabel = new Label { Text = email };
        Label nomLabel = new Label { Text = nom };
        Label prenomLabel = new Label { Text = prenom };
        Label adresseLabel = new Label { Text = adresse };
        Label codePostalLabel = new Label { Text = codePostal };
        Label villeLabel = new Label { Text = ville };
        Microsoft.Maui.Controls.Button modifierBoutique = new Microsoft.Maui.Controls.Button { Text = "Modifier ma boutique" };
        // Add more Label objects as needed

        StackLayout cardContent = new StackLayout
        {
            Margin = new Thickness(20),
            Spacing = 10,
            Children =
        {
            titleLabel,
            cardImage,
            emailLabel,
            nomLabel,
            prenomLabel,
            adresseLabel,
            codePostalLabel,
            villeLabel,
            modifierBoutique
            // Add more Label objects here
        }
        };

        Frame cardFrame = new Frame
        {
            Content = cardContent,
            CornerRadius = 10,
            Padding = new Thickness(10),
            HasShadow = true,
            HorizontalOptions = LayoutOptions.Center
        };

        ScrollView scrollView = new ScrollView
        {
            Content = new StackLayout
            {
                Children =
            {
                pageImage,
                cardFrame
            }
            }
        };

        Content = scrollView;
        //modifierBoutique += async (sender, e) => { await DisplayAlert("Alert", "Clicked", "OK") };
        modifierBoutique.Clicked += async (sender, args) => await DisplayAlert("Alert", "Clicked", "OK");
    }

}