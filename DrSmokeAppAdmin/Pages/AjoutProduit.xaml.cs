
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
        if (CategoriePicker.SelectedIndex != -1)
        {
            string selectedValue = CategoriePicker.SelectedItem.ToString();
            // Faites quelque chose avec la valeur sélectionnée, par exemple l'afficher dans une alerte.
            await DisplayAlert("Sélection", $"Catégorie sélectionnée : {selectedValue}", "OK");
        }

            await DisplayAlert("Alert", "fileResult", "OK");

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
                form.Add(imageContent, "image_produit", result.FileName);
                form.Add(new StringContent("Première valeur"), "string1");
                form.Add(new StringContent("Deuxième valeur"), "string2");

                var data = new
                {
                    string1 = "Première valeur",
                    string2 = "Deuxième valeur"
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
                        await DisplayAlert("Alert", "Données envoyées avec succès", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Alert", $"Erreur lors de l'envoi des données : {response.StatusCode}", "OK");
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
            await DisplayAlert("Alert", "Aucune image sélectionnée.", "OK");
        }
    }





}