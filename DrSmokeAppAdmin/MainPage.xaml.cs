﻿using System.Text.Json;
using System.Text;

namespace DrSmokeAppAdmin
{
    public partial class MainPage : ContentPage
    {
        bool FormConnexionVisible = false;
        bool FormInscriptionVisible = false;
        public MainPage()
        {
            InitializeComponent();
        }
        public void OnButtonConnexion(object sender, EventArgs args)
        {
            // Inverse la visibilité du formulaire de connexion
            FormConnexionVisible = !FormConnexionVisible;
            formulaireConnexion.IsVisible = FormConnexionVisible;
        }

        public void OnButtonInscription(object sender, EventArgs args)
        {
            FormInscriptionVisible = !FormInscriptionVisible;
            formulaireInscription.IsVisible = FormInscriptionVisible;
            if (FormInscriptionVisible == true)
            {

            }
        }

        async private protected void OnInscriptionnRegistrer(object sender, EventArgs args)
        {

            string email = EmailEntryInscription.Text;
            string password = PasswordEntryInscription.Text;
            string nom = Nom.Text;
            string prenom = Prenom.Text;
            string codePostal = CodePostalEntry.Text;
            string address = AdresseEntry.Text;

            string ville = VilleEntry.Text;

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(nom) && !string.IsNullOrEmpty(prenom) && !string.IsNullOrEmpty(codePostal) && !string.IsNullOrEmpty(address) && !string.IsNullOrEmpty(ville))
            {
                HttpClient _client;
                JsonSerializerOptions _serializerOptions;
                _client = new HttpClient();
                _serializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };

                Uri uri = new Uri(string.Format("http://localhost:3000/admin/inscription"));
                try
                {
                    Models.FromInscriptionAdmin admin = new Models.FromInscriptionAdmin
                    {
                        Email = email,
                        Password = password,
                        Nom = nom,
                        Prenom = prenom,
                        CodePostal = codePostal,
                        Adresse = address,
                        Ville = ville
                    };

                    // Ajout de cet objet à la liste
                    List<Models.FromInscriptionAdmin> admins = new List<Models.FromInscriptionAdmin>();
                    admins.Add(admin);
                    string json = JsonSerializer.Serialize(admins, _serializerOptions);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await _client.PostAsync(uri, content);
                    if (response.IsSuccessStatusCode)
                    {
                        // Traitement à effectuer en cas de réussite de la requête
                        string responseContent = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonSerializer.Deserialize<Models.ReponseAPI>(responseContent);
                        await DisplayAlert("Alert", $"Test ${apiResponse.message} ", "ok");

                    }
                    else
                    {
                        // Traitement à effectuer en cas d'échec de la requête
                        string responseContent = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonSerializer.Deserialize<Models.ReponseAPI>(responseContent);
                        await DisplayAlert("Alert", $"Test ${apiResponse.message} ", "ok");
                    }

                }
                catch (Exception ex)
                {
                    await DisplayAlert("Alert", $"Une erreur est survenue: {ex.ToString()}", "OK");
                }
            }
            else
            {
                await DisplayAlert("Alert", "Tout les champs sont obligatoire", "ok");
            }

        }
        private protected async void OnConnexionRegistrer(object sender, EventArgs args)
        {
            string email = EmailEntryConnexion.Text;
            string password = PasswordEntryConnexion.Text;

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                HttpClient _client;
                JsonSerializerOptions _serializerOptions;
                _client = new HttpClient();
                _serializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };

                Uri uri = new Uri(string.Format("http://localhost:3000/admin/connexion"));

                try
                {
                    Models.FromConnexionAdmin admin = new Models.FromConnexionAdmin()
                    {
                        Email = email,
                        Password = password,
                    };
                    List<Models.FromConnexionAdmin> admins = new List<Models.FromConnexionAdmin> { admin };
                    string json = JsonSerializer.Serialize(admins, _serializerOptions);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await _client.PostAsync(uri, content);
                    if (response.IsSuccessStatusCode)
                    {
                        // Traitement à effectuer en cas de réussite de la requête
                        string responseContent = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonSerializer.Deserialize<Models.ReponseAPI>(responseContent);
                        await DisplayAlert("Alert", $"Test ${apiResponse.message} ", "ok");
                        await SecureStorage.Default.SetAsync("oauth_token", apiResponse.token);
                        await Navigation.PushAsync(new Pages.GestionBoutiqueAdmin());
                    }
                    else
                    {
                        // Traitement à effectuer en cas d'échec de la requête
                        string responseContent = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonSerializer.Deserialize<Models.ReponseAPI>(responseContent);
                        await DisplayAlert("Alert", $"Test ${apiResponse.message} ", "ok");
                    }

                }
                catch (Exception ex)
                {
                    await DisplayAlert("Alert", $"Une erreur est survenue: {ex.ToString()}", "OK");
                }

            }
            else
            {
                await DisplayAlert("Alert", "Tout les champs sont obligatoire", "OK");
            }
        }
    }
}

