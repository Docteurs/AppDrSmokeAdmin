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

            var result = await FilePicker.Default.PickAsync(options);
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
                    SendProduct(result);
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Alert", $"Une erreur est survenue : {ex.ToString()}", "OK");
        }
    }
    public void RemplaceImage(object sender, EventArgs args)
    {
        myImageControl.Source = null;

        //RemplacerImg.IsVisible = false;
    }
    async private Task SendProduct(FileResult fileResult)
    {
        try
        {
            byte[] imageBytes;

            using (MemoryStream ms = new MemoryStream())
            {
                Stream stream = await fileResult.OpenReadAsync();
                await stream.CopyToAsync(ms);
                imageBytes = ms.ToArray();
            }

            // Convertir les données en ByteArrayContent pour l'image
            ByteArrayContent imageContent = new ByteArrayContent(imageBytes);
            imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

            // Ajouter l'image dans le contenu de la requête avec le nom du champ attendu par multer ("image_produit" par exemple)
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(imageContent, "image_produit", fileResult.FileName);

            // Ajouter d'autres valeurs de type string
            form.Add(new StringContent("Première valeur"), "string1");
            form.Add(new StringContent("Deuxième valeur"), "string2");

            // Convertir les données en objet JSON
            var data = new
            {
                string1 = "Première valeur",
                string2 = "Deuxième valeur"
                // Ajoutez d'autres propriétés ici si nécessaire
            };

            string jsonData = JsonSerializer.Serialize(data);

            // Ajouter le JSON dans la requête
            form.Add(new StringContent(jsonData, Encoding.UTF8, "application/json"), "json_data");

            // Récupérer le jeton OAuth
            string oauthToken = await SecureStorage.Default.GetAsync("oauth_token");

            // Créer une instance HttpClient
            HttpClient httpClient = new HttpClient();

            // Ajouter le jeton OAuth dans les en-têtes de la requête
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", oauthToken);

            // URL de destination
            Uri uri = new Uri("http://localhost:3000/admin/gestion-stock/create");

            // Envoyer la requête avec les données multipartie incluant le JSON
            HttpResponseMessage response = await httpClient.PostAsync(uri, form);

            // Vérifier si la requête a réussi
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Alert", "Données envoyées avec succès", "OK");
            }
            else
            {
                await DisplayAlert("Alert", "Erreur lors de l'envoi des données", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Alert", $"Une erreur est survenue : {ex.Message}", "OK");
        }
    }




}