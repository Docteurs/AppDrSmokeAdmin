
using DrSmokeAppAdmin.Models;
using Microsoft.Maui.Storage;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;



namespace DrSmokeAppAdmin.Pages;

public partial class AjoutProduit : ContentPage
{
    private FileResult result;
    public AjoutProduit()
	{
		InitializeComponent();
        // PickAndShow();

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

    public void RemplaceImage(object sender, EventArgs args, FileResult fileResult)
    {
        myImageControl.Source = null;

        //RemplacerImg.IsVisible = false;
    }
    private async void SendProduct(object sender, EventArgs args)
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
            Console.WriteLine("La valeur entrée pour le prix 1g n'est pas un nombre décimal");
        }

        if (double.TryParse(EntryPrix3g.Text, out double resultPrix3g))
        {
            resultEntryPrix3g = Convert.ToInt32(resultPrix3g);
        }
        else
        {
            Console.WriteLine("La valeur entrée pour le prix 3g n'est pas un nombre décimal");
        }

        if (double.TryParse(EntryPrix5g.Text, out double resultPrix5g))
        {
            resultEntryPrix5g = Convert.ToInt32(resultPrix5g);
        }
        else
        {
            Console.WriteLine("La valeur entrée pour le prix 5g n'est pas un nombre décimal");
        }

        if (double.TryParse(EntryPrix10g.Text, out double resultPrix10g))
        {
            resultEntryPrix10g = Convert.ToInt32(resultPrix10g);
        }
        else
        {
            Console.WriteLine("La valeur entrée pour le prix 10g n'est pas un nombre décimal");
        }

        if (double.TryParse(EntryPrix20g.Text, out double resultPrix20g))
        {
            resultEntryPrix20g = Convert.ToInt32(resultPrix20g);
        }
        else
        {
            Console.WriteLine("La valeur entrée pour le prix 20g n'est pas un nombre décimal");
        }

        if (result != null && !string.IsNullOrEmpty(nameProduct) && !string.IsNullOrEmpty(descriptifProduct) && !string.IsNullOrEmpty(categorieProduct) && resultQuantiteProductGramme != 0 && resultEntryPrix1g != 0 && resultEntryPrix3g != 0 && resultEntryPrix5g != 0 && resultEntryPrix10g != 0 && resultEntryPrix20g != 0)
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
                form.Add(imageContent, "image_produit", result.FileName);
                form.Add(new StringContent(nameProduct.ToString()), "nameProduct");
                form.Add(new StringContent(descriptifProduct.ToString()), "descriptifProduct");
                form.Add(new StringContent(categorieProduct.ToString()), "categorieProduct");
                form.Add(new StringContent(resultQuantiteProductGramme.ToString()), "resultQuantiteProductGramme");
                form.Add(new StringContent(resultEntryPrix1g.ToString()), "prix1gProduit");
                form.Add(new StringContent(resultEntryPrix3g.ToString()), "prix3gProduit");
                form.Add(new StringContent(resultEntryPrix5g.ToString()), "prix5gProduit");
                form.Add(new StringContent(resultEntryPrix10g.ToString()), "prix10gProduit");
                form.Add(new StringContent(resultEntryPrix20g.ToString()), "prix20gProduit");


                var data = new
                {
                    nomProduit = nameProduct,
                    descriptifProduit = descriptifProduct,
                    categorieProduit = categorieProduct,
                    quantiteGrammeProduit = resultQuantiteProductGramme,
                    prix1gProduit = resultEntryPrix1g,
                    prix3gProduit = resultEntryPrix3g,
                    prix5gProduit = resultEntryPrix5g,
                    prix10gProduit = resultEntryPrix10g,
                    prix20gProduit = resultEntryPrix20g
                    // Ajoutez d'autres propriétés ici si nécessaire
                };

                //string jsonData = JsonSerializer.Serialize(data);

                // Ajouter le JSON dans la requête
                //form.Add(new StringContent(jsonData, Encoding.UTF8, "application/json"), "json_data");

                // Récupérer le jeton OAuth
                string oauthToken = await SecureStorage.Default.GetAsync("oauth_token");

                // Créer une instance HttpClient
                using (HttpClient httpClient = new HttpClient())
                {
                    // Ajouter le jeton OAuth dans les en-têtes de la requête
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", oauthToken);

                    // URL de destination
                    Uri uri = new Uri("http://localhost:3000/admin/gestion-stock/create");

                    // Envoyer la requête POST
                    HttpResponseMessage response = await httpClient.PostAsync(uri, form);

                    // Vérifier si la requête a réussi
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonSerializer.Deserialize<Models.ReponseAPI>(responseContent);
                        await DisplayAlert("Alert", $"Test ${apiResponse.message} ", "ok");
                        // Get current page
                        //var page = Navigation.NavigationStack.LastOrDefault();

                        // Load new page
                        //await Shell.Current.GoToAsync(nameof(Pages.StockAdmin), false);
                        await Shell.Current.GoToAsync("//StockAdmin");
                        // Remove old page
                        //Navigation.RemovePage(page);
                    }
                    else
                    {
                        // await DisplayAlert("Alert", $"Erreur lors de l'envoi des données : {response.StatusCode}", "OK");
                        string responseContent = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonSerializer.Deserialize<Models.ReponseAPI>(responseContent);
                        await DisplayAlert("Alert", $"Test ${apiResponse.message} ", "ok");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", $"Une exception est survenue : {ex.Message}", "OK");
            }
        }
        else
        {
            await DisplayAlert("Alert", "Tout les champs son obligatoire", "OK");
        }
    }

    async public void BackToPage(object sender, EventArgs args)
    {
        await Shell.Current.GoToAsync("//StockAdmin");
    }



}