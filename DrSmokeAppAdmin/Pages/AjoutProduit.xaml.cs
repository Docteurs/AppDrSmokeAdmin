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
                PickerTitle = "S�lectionnez une image"
                // Vous pouvez �galement ajouter des filtres de types de fichiers ici si n�cessaire
            };

            var result = await FilePicker.Default.PickAsync(options);
            if (result != null)
            {
                if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                    result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                {
                    var stream = await result.OpenReadAsync();
                    var image = ImageSource.FromStream(() => stream);

                    // Afficher l'image dans un contr�le d'image (par exemple, "myImageControl" est le nom de votre contr�le d'image dans le XAML)
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

            // Convertir les donn�es en ByteArrayContent pour l'image
            ByteArrayContent imageContent = new ByteArrayContent(imageBytes);
            imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

            // Ajouter l'image dans le contenu de la requ�te avec le nom du champ attendu par multer ("image_produit" par exemple)
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(imageContent, "image_produit", fileResult.FileName);

            // Ajouter d'autres valeurs de type string
            form.Add(new StringContent("Premi�re valeur"), "string1");
            form.Add(new StringContent("Deuxi�me valeur"), "string2");

            // Convertir les donn�es en objet JSON
            var data = new
            {
                string1 = "Premi�re valeur",
                string2 = "Deuxi�me valeur"
                // Ajoutez d'autres propri�t�s ici si n�cessaire
            };

            string jsonData = JsonSerializer.Serialize(data);

            // Ajouter le JSON dans la requ�te
            form.Add(new StringContent(jsonData, Encoding.UTF8, "application/json"), "json_data");

            // R�cup�rer le jeton OAuth
            string oauthToken = await SecureStorage.Default.GetAsync("oauth_token");

            // Cr�er une instance HttpClient
            HttpClient httpClient = new HttpClient();

            // Ajouter le jeton OAuth dans les en-t�tes de la requ�te
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", oauthToken);

            // URL de destination
            Uri uri = new Uri("http://localhost:3000/admin/gestion-stock/create");

            // Envoyer la requ�te avec les donn�es multipartie incluant le JSON
            HttpResponseMessage response = await httpClient.PostAsync(uri, form);

            // V�rifier si la requ�te a r�ussi
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Alert", "Donn�es envoy�es avec succ�s", "OK");
            }
            else
            {
                await DisplayAlert("Alert", "Erreur lors de l'envoi des donn�es", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Alert", $"Une erreur est survenue : {ex.Message}", "OK");
        }
    }




}