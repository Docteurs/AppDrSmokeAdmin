

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace DrSmokeAppAdmin.Pages;
[QueryProperty(nameof(Uuid), "Uuid")]
public partial class UpdateProduit : ContentPage
{
    private string uuid = string.Empty;
    private FileResult result;
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
    public UpdateProduit()
	{
		InitializeComponent();
        BindingContext = this;
      
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        HttpClient _client;
        JsonSerializerOptions _serializerOptions;
        if (!string.IsNullOrEmpty(uuid))
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
            await DisplayAlert("Alert", Uuid, "ok");
        }
        else
        {
            await DisplayAlert("Alert", "Err", "ok");
        }
    }

    async public void BackToPage(object sender, EventArgs args)
    {
        await Shell.Current.GoToAsync("..");
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

                    //RemplacerImg.IsVisible = true;
                    //SendProduct(result);
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Alert", $"Une erreur est survenue : {ex.ToString()}", "OK");
        }
    }
    public bool BoolCheckBoxProduitVisible
    {
        get { return CheckBoxProduitVisible.IsChecked; }
        set { CheckBoxProduitVisible.IsChecked = value; }
    }

    public bool BoolCheckBoxProduitNonVisible
    {
        get { return CheckBoxProduitNonVisible.IsChecked; }
        set { CheckBoxProduitNonVisible.IsChecked = value; }
    }

    async private void SendModificationProduit(object sender, EventArgs args)
    {
        string nameProduct = EntryNomProduit.Text;
        string descriptifProduct = EntryDescriptifProduit.Text;
        string categorieProduct = string.Empty;
        int resultQuantiteProductGramme = 0;
        int resultEntryPrix1g = 0;
        int resultEntryPrix3g = 0;
        int resultEntryPrix5g = 0;
        int resultEntryPrix10g = 0;
        int resultEntryPrix20g = 0;
        int IsChecked = 1;

        // Verification du champ quantité en gramme si int ou string    
        if (int.TryParse(EntryQuantiteProduit.Text, out int resultGramme))
        {
            resultQuantiteProductGramme = resultGramme;
        }
        else
        {
            await DisplayAlert("Alert", "Veuillez rentre un nombre entier pour la quantité en gramme", "OK");
        }

        if (CategoriePicker.SelectedIndex != -1)
        {
            //string selectedValue = CategoriePicker.SelectedItem.ToString();
            categorieProduct = CategoriePicker.SelectedItem.ToString();
            // Faites quelque chose avec la valeur sélectionnée, par exemple l'afficher dans une alerte.
            // await DisplayAlert("Sélection", $"Catégorie sélectionnée : {selectedValue}", "OK");
        }
        // Convertion de l'entré en int pour le prix 1g
        if (double.TryParse(EntryPrix1g.Text, out double resultPrix1g))
        {
            resultEntryPrix1g = Convert.ToInt32(resultPrix1g);
        }
        else
        {
            resultEntryPrix1g = Convert.ToInt32(0);
        }

        if (double.TryParse(EntryPrix3g.Text, out double resultPrix3g))
        {
            resultEntryPrix3g = Convert.ToInt32(resultPrix3g);
        }
        else
        {
            resultEntryPrix3g = Convert.ToInt32(0);
        }

        if (double.TryParse(EntryPrix5g.Text, out double resultPrix5g))
        {
            resultEntryPrix5g = Convert.ToInt32(resultPrix5g);
        }
        else
        {
            resultEntryPrix5g = Convert.ToInt32(0);
        }

        if (double.TryParse(EntryPrix10g.Text, out double resultPrix10g))
        {
            resultEntryPrix10g = Convert.ToInt32(resultPrix10g);
        }
        else
        {
            resultEntryPrix10g = Convert.ToInt32(0); // Correction ici
        }

        if (double.TryParse(EntryPrix20g.Text, out double resultPrix20g))
        {
            resultEntryPrix20g = Convert.ToInt32(resultPrix20g);
        }
        else
        {
            resultEntryPrix20g = Convert.ToInt32(0);
        }
        if (CheckBoxProduitVisible.IsChecked)
        {
            IsChecked = 1;
        }
        else
        {
            IsChecked = 0;
        }
        if (CheckBoxProduitVisible.IsChecked && CheckBoxProduitNonVisible.IsChecked)
        {
            IsChecked = 1;
        }
        HttpClient _client;

        _client = new HttpClient();

        string oauthToken = await SecureStorage.Default.GetAsync("oauth_token");
        Uri uri = new Uri(string.Format($"https://get-evolutif.xyz/DrSmokeApi/admin/update-one-produit/{Uuid}"));
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", oauthToken);
        if (result != null)
        {
            try
            {
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

                // Utiliser le bon nom de champ pour l'image (vérifier avec votre API)
                form.Add(imageContent, "image_produit", result.FileName);
                if (string.IsNullOrEmpty(nameProduct))
                {
                    nameProduct = nomProduit.Text;
                }
                if (string.IsNullOrEmpty(descriptifProduct))
                {
                    descriptifProduct = descriptifProduit.Text;
                }
                if (string.IsNullOrEmpty(categorieProduct))
                {
                    categorieProduct = categorieProduit.Text;
                }

                // Utiliser les noms de champ corrects pour les autres données
                form.Add(new StringContent(nameProduct), "nameProduct");
                form.Add(new StringContent(descriptifProduct), "descriptifProduct");
                form.Add(new StringContent(categorieProduct.ToString()), "categorieProduct");
                form.Add(new StringContent(resultQuantiteProductGramme.ToString()), "resultQuantiteProductGramme");
                form.Add(new StringContent(resultEntryPrix1g.ToString()), "prix1gProduit");
                form.Add(new StringContent(resultEntryPrix3g.ToString()), "prix3gProduit");
                form.Add(new StringContent(resultEntryPrix5g.ToString()), "prix5gProduit");
                form.Add(new StringContent(resultEntryPrix10g.ToString()), "prix10gProduit");
                form.Add(new StringContent(resultEntryPrix20g.ToString()), "prix20gProduit");
                form.Add(new StringContent(IsChecked.ToString()), "IsVisible");
                HttpResponseMessage response = await _client.PutAsync(uri, form);
                // Traitement à effectuer en cas de réussite de la requête
                string responseContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<Models.ReponseAPI>(responseContent);
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Alert", $"{apiResponse.Message} ", "ok");
                    await Navigation.PushAsync(new Pages.StockAdmin());
                }
                else
                {
                    await DisplayAlert("Alert", $"{apiResponse.Message} ", "ok");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", $"Une erreur est survenue: {ex.ToString()}", "OK");
                await Navigation.PushAsync(new Pages.ConnexionPage());
            }
        }
        else
        {
            JsonSerializerOptions _serializerOptions;
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            Models.ProduitAdminUpdate produitUpdate = new Models.ProduitAdminUpdate()
            {
                NomProduit = nameProduct,
                Descriptif = descriptifProduct,
                CategorieProduit = categorieProduct.ToString(),
                Quantite = resultQuantiteProductGramme,
                UnGprix = resultEntryPrix1g.ToString(),
                TroisGprix = resultEntryPrix3g.ToString(),
                CingGprix = resultEntryPrix5g.ToString(),
                DixGprix = resultEntryPrix10g.ToString(),
                VingtGprix = resultEntryPrix20g.ToString(),
                IsVisible = IsChecked
            };
            List<Models.ProduitAdminUpdate> produit = new List<Models.ProduitAdminUpdate>();
            produit.Add(produitUpdate);
            string json = JsonSerializer.Serialize(produit, _serializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PutAsync(uri, content);
            // Traitement à effectuer en cas de réussite de la requête
            string responseContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<Models.ReponseAPI>(responseContent);
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Alert", $"{apiResponse.Message} ", "ok");
                await Navigation.PushAsync(new Pages.StockAdmin());
            }
            else
            {
                await DisplayAlert("Alert", $"{apiResponse.Message} ", "ok");
                await Navigation.PushAsync(new Pages.ConnexionPage());
            }

        }
    }

}