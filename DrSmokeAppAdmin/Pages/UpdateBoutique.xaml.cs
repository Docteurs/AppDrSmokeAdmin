using System.Net.Http.Headers;
using System.Text.Json;

namespace DrSmokeAppAdmin.Pages;

public partial class UpdateBoutique : ContentPage
{
    private FileResult result;
    public UpdateBoutique()
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

    private async void PickAndShow(object sender, EventArgs args)
    {
        try
        {
            var options = new PickOptions
            {
                PickerTitle = "Sélectionnez une image"
                // Vous pouvez également ajouter des filtres de types de fichiers ici si nécessaire
            };

            result = await FilePicker.Default.PickAsync(options);
            if (result != null)
            {
                if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                    result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                {
                    var stream = await result.OpenReadAsync();
                    var image = ImageSource.FromStream(() => stream);

                    // Afficher l'image dans un contrôle d'image (par exemple, "myImageControl" est le nom de votre contrôle d'image dans le XAML)
                    myImageControl.Source = image;
                    myImageControl.WidthRequest = 300;
                    myImageControl.HeightRequest = 300;

                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Alert", $"Une erreur est survenue : {ex.ToString()}", "OK");
        }
    }
    public async void SendModificationProduit(object sender, EventArgs args)
    {
        string Email = EntryEmailAdmin.Text;
        string Addresse = EntryAddresseAdmin.Text;
        string Nom = EntryNomAdmin.Text;
        string Prenom = EntryPrenomAdmin.Text;
/*        bool LundiOuvert = CheckBoxLundi.IsChecked;
        bool MardiOuvert = CheckBoxMardi.IsChecked;
        bool MercrediOuvert = CheckBoxMercredi.IsChecked;
        bool JeudiOuvert = CheckBoxJeudi.IsChecked;
        bool VendrediOuvert = CheckBoxVendredi.IsChecked;
        bool SamediOuvert = CheckBoxSamedi.IsChecked;
        bool DimancheOuvert = CheckBoxDimanche.IsChecked;*/
        string HoraireLundi = EntryHorraireLundi.Text;
        string HoraireMardi = EntryHorraireMardi.Text;
        string HoraireMercredi = EntryHorraireMercredi.Text;
        string HoraireJeudi = EntryHorraireJeudi.Text;
        string HoraireVendredi = EntryHorraireVendredi.Text;
        string HoraireSamedi = EntryHorraireSamedi.Text;
        string HoraireDimanche = EntryHorraireDimanche.Text;


        if (result != null &&
            !string.IsNullOrEmpty(Email) &&
            !string.IsNullOrEmpty(Addresse) &&
            !string.IsNullOrEmpty(Nom) &&
            !string.IsNullOrEmpty(Prenom) &&
            !string.IsNullOrEmpty(HoraireLundi) &&
            !string.IsNullOrEmpty(HoraireMardi) &&
            !string.IsNullOrEmpty(HoraireMercredi) &&
            !string.IsNullOrEmpty(HoraireJeudi) &&
            !string.IsNullOrEmpty(HoraireVendredi) &&
            !string.IsNullOrEmpty(HoraireSamedi) &&
            !string.IsNullOrEmpty(HoraireDimanche))
        {
            HttpClient _client;

            try
            {
                _client = new HttpClient();
                byte[] imageBytes;
                using (MemoryStream ms = new MemoryStream())
                {
                    Stream stream = await result.OpenReadAsync();
                    await stream.CopyToAsync(ms);
                    imageBytes = ms.ToArray();
                }

                ByteArrayContent imageContent = new ByteArrayContent(imageBytes);

                // Utiliser le bon type de contenu pour l'image
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg"); // ou "image/png" selon le format de l'image

                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(imageContent, "image_produit", result.FileName);
                form.Add(new StringContent(Email), "Email");
                form.Add(new StringContent(Addresse), "Addresse");
                form.Add(new StringContent(Nom), "Nom");
                form.Add(new StringContent(Prenom), "Prenom");
/*                form.Add(new StringContent(LundiOuvert.ToString()), "LundiOuvert");
                form.Add(new StringContent(MardiOuvert.ToString()), "MardiOuvert");
                form.Add(new StringContent(MercrediOuvert.ToString()), "MercrediOuvert");
                form.Add(new StringContent(JeudiOuvert.ToString()), "JeudiOuvert");
                form.Add(new StringContent(VendrediOuvert.ToString()), "VendrediOuvert");
                form.Add(new StringContent(SamediOuvert.ToString()), "SamediOuvert");
                form.Add(new StringContent(DimancheOuvert.ToString()), "DimancheOuvert");*/
                form.Add(new StringContent(HoraireLundi), "HoraireLundi");
                form.Add(new StringContent(HoraireMardi), "HoraireMardi");
                form.Add(new StringContent(HoraireMercredi), "HoraireMercredi");
                form.Add(new StringContent(HoraireJeudi), "HoraireJeudi");
                form.Add(new StringContent(HoraireVendredi), "HoraireVendredi");
                form.Add(new StringContent(HoraireSamedi), "HoraireSamedi");
                form.Add(new StringContent(HoraireDimanche), "HoraireDimanche");

                string oauthToken = await SecureStorage.Default.GetAsync("oauth_token");

                // Créer une instance HttpClient

                // Ajouter le jeton OAuth dans les en-têtes de la requête
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", oauthToken);

                // URL de destination
                Uri uri = new Uri($"https://get-evolutif.xyz/DrSmokeApi/admin/admin/update-boutique/{oauthToken}");

                // Envoyer la requête POST
                HttpResponseMessage response = await _client.PutAsync(uri, form);

                // Vérifier si la requête a réussi
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<Models.ReponseAPI>(responseContent);
                    await DisplayAlert("Alert", $"Test ${apiResponse.Message} ", "ok");
                    // Get current page
                    //var page = Navigation.NavigationStack.LastOrDefault();

                    // Load new page
                    //await Shell.Current.GoToAsync(nameof(Pages.StockAdmin), false);
                    await Navigation.PushAsync(new Pages.GestionBoutiqueAdmin());
                    // Remove old page
                    //Navigation.RemovePage(page);
                }
                else
                {
                    // await DisplayAlert("Alert", $"Erreur lors de l'envoi des données : {response.StatusCode}", "OK");
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<Models.ReponseAPI>(responseContent);
                    await DisplayAlert("Alert", $"Test ${apiResponse.Message} ", "ok");
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", $"Une exception est survenue : {ex.Message}", "OK");
                await Navigation.PushAsync(new Pages.ConnexionPage());
            }
        }
        else
        {
            await DisplayAlert("Alert", "l'image, l'email, l'Adresse, le nom, le prenom et les horaire de la semaine sont obligatoire", "OK");
        }
    }
}