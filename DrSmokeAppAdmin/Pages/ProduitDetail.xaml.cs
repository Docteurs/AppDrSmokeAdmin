
using Microsoft.Maui.Graphics.Text;
using System.Collections.Generic;
using System.Net.Http.Headers;

using System.Text;
using System.Text.Json;

namespace DrSmokeAppAdmin.Pages
{
    [QueryProperty(nameof(Uuid), "Uuid")]
    public partial class ProduitDetail : ContentPage
    {
        FileResult result;
        private string uuid;
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

        public ProduitDetail()
        {
            InitializeComponent();
            BindingContext = this;

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            HttpClient _client;
            JsonSerializerOptions _serializerOptions;

            
            if (!string.IsNullOrEmpty(Uuid))
            {
                DisplayAlert("Alert", Uuid, "OK");
                _client = new HttpClient();
                _serializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };
                string oauthToken = await SecureStorage.Default.GetAsync("oauth_token");
                Items = new List<Models.ProduitAdmin>();
                Uri uri = new Uri(string.Format($"http://localhost:3000/admin/get-one-produit/{Uuid}"));
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
                            var grid = new Grid();
                            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                            foreach (var item in items)
                            {
                                //var item = items[i];

                                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                                var labelCategorie = new Label { Text = item.CategorieProduit.ToString(), FontFamily = "Pacifico" };
                                var labelNom = new Label { Text = item.NomProduit, FontAttributes = FontAttributes.Bold, FontSize = 15, Margin = new Thickness(0, 5, 0, 5) };
                                var labelDescriptif = new Label { Text = item.Descriptif, FontFamily = "Pacifico", WidthRequest = 250, HeightRequest = 250 };
                                var labelQuantite = new Label { Text = item.Quantite + "g/En stock", FontFamily = "Pacifico", Margin = new Thickness(0, 5, 0, 5) };
                                var labelPrix = new Label { Text = item.UnGprix + "€/1g", FontFamily = "Pacifico", Margin = new Thickness(0, 5, 0, 5) };
                                var buttonVoirProduit = new Button { Text = "Modifier le produit" };
                                buttonVoirProduit.Clicked += (sender, e) =>
                                {
                                   
                    
                                    var buttonAjouterImage = new Button { Text = "Ajouter une image", BackgroundColor = Color.FromRgba(221, 221, 221, 255), TextColor = Color.FromRgba(51, 51, 51, 255), Margin = new Thickness(10, 10, 10, 10) };
                                    var imageProduit = new Image { Source = "" , Margin = new Thickness(10, 10, 10, 10) };
                                    var entryNomProduit = new Entry { Placeholder = "Nom du produit", Margin = new Thickness(10, 10, 10, 10) };
                                    var entryDescription = new Entry { Placeholder = "Description", Margin = new Thickness(10, 10, 10, 10) };
                                    var entryQuantite = new Entry { Placeholder = "Quantité", Margin = new Thickness(10, 10, 10, 10) };
                                    var entryPrix1g = new Entry { Placeholder = "Prix 1g", Margin = new Thickness(10, 10, 10, 10) };
                                    var entryPrix3g = new Entry { Placeholder = "Prix 3g", Margin = new Thickness(10, 10, 10, 10) };
                                    var entryPrix5g = new Entry { Placeholder = "Prix 5g", Margin = new Thickness(10, 10, 10, 10) };
                                    var entryPrix10g = new Entry { Placeholder = "Prix 10g", Margin = new Thickness(10, 10, 10, 10) };
                                    var entryPrix20g = new Entry { Placeholder = "Prix 20g", Margin = new Thickness(10, 10, 10, 10) };
                                    var buttonEnvoyerModificationProduit = new Button { Text = "Envoyer les modification" };

                                    buttonEnvoyerModificationProduit.Clicked += async (sender, e) =>
                                    {
                                        HttpClient _client;
                                        JsonSerializerOptions _serializerOptions;
                                        _client = new HttpClient();
                                        _serializerOptions = new JsonSerializerOptions
                                        {
                                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                                            WriteIndented = true
                                        };
                                        Uri uri = new Uri(string.Format($"http://localhost:3000/admin/update-one-produit/{Uuid}"));
                                        string oauthToken = await SecureStorage.Default.GetAsync("oauth_token");
                                        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", oauthToken);

                                        try
                                        {
                                            string json = JsonSerializer.Serialize<Models.ProduitAdmin>(item, _serializerOptions);
                                            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                                            HttpResponseMessage response = await _client.PutAsync(uri, content);
                                            string responseContent = await response.Content.ReadAsStringAsync();
                                            var apiResponse = JsonSerializer.Deserialize<Models.ReponseAPI>(responseContent);
                                            if (response.IsSuccessStatusCode)
                                            {
                                                await DisplayAlert("Alert", $"Test {apiResponse.message}", "ok");
                                            }
                                            else
                                            {
                                                await DisplayAlert("Alert", $"Test {apiResponse.message}", "ok");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            await DisplayAlert("Alert", $"Une erreur est survenue: {ex.ToString()}", "OK");
                                        }
                                    };




                                    buttonAjouterImage.Clicked += async (sender, e) =>
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
                                                    imageProduit.Source = image;
                                                    imageProduit.WidthRequest = 300;
                                                    imageProduit.HeightRequest = 300;

                                                    //RemplacerImg.IsVisible = true;
                                                    //SendProduct(result);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            await DisplayAlert("Alert", $"Une erreur est survenue : {ex.ToString()}", "OK");
                                        }
                                    };

                                    CheckBox checkBoxVisible = new CheckBox
                                    {
                                        IsChecked = true,
                                        IsVisible = true
                                    };

                                    // Label associé au CheckBox visible
                                    Label labelVisible = new Label
                                    {
                                        Text = "CheckBox Visible",
                                        VerticalOptions = LayoutOptions.Center // Pour aligner le texte verticalement avec la case à cocher
                                    };

                                    // CheckBox non visible
                                    CheckBox checkBoxNotVisible = new CheckBox
                                    {
                                        IsChecked = false,
                                        IsVisible = true
                                    };
                                    
                                    // Label associé au CheckBox non visible
                                    Label labelNotVisible = new Label
                                    {
                                        Text = "CheckBox Not Visible",
                                        VerticalOptions = LayoutOptions.Center // Pour aligner le texte verticalement avec la case à cocher
                                    };

                                    var stackContent = new StackLayout();
                                    stackContent.Children.Add(imageProduit);
                                    stackContent.Children.Add(buttonAjouterImage);
                                    stackContent.Children.Add(entryNomProduit);
                                    stackContent.Children.Add(entryDescription);
                                    stackContent.Children.Add(entryQuantite);
                                    stackContent.Children.Add(entryPrix1g);
                                    stackContent.Children.Add(entryPrix3g);
                                    stackContent.Children.Add(entryPrix5g);
                                    stackContent.Children.Add(entryPrix10g);
                                    stackContent.Children.Add(entryPrix20g);
                                    // Ajouter les CheckBox et les Labels à votre disposition, par exemple, dans un StackLayout
                                    stackContent.Children.Add(new StackLayout { Children = { checkBoxVisible, labelVisible }, Orientation = StackOrientation.Horizontal });
                                    stackContent.Children.Add(new StackLayout { Children = { checkBoxNotVisible, labelNotVisible }, Orientation = StackOrientation.Horizontal });
                                    stackContent.Children.Add(buttonEnvoyerModificationProduit);

                                    // Ajuster la taille de la Grid
                                    grid.RowDefinitions.Add(new RowDefinition { Height = 200 });
                                    

                                    var card = new Frame
                                    {
                                        BorderColor = Colors.Black,
                                        Content = stackContent,
                                        CornerRadius = 10,
                                        WidthRequest = 300,
                                        HeightRequest = 1100,
                                        // Padding = new Thickness(10),
                                        HasShadow = true,
                                        HorizontalOptions = LayoutOptions.Center,
                                        VerticalOptions = LayoutOptions.Center
                                    };

                                    grid.Add(card, 0, 2);
                                };



                                Microsoft.Maui.Controls.Image cardImage = new Microsoft.Maui.Controls.Image
                                {
                                    Source = $"{item.ImgProduit}",
                                    WidthRequest = 250,
                                    HeightRequest = 250,
                                    Aspect = Aspect.AspectFill
                                };

                                var stackContent = new StackLayout();
                                stackContent.Children.Add(cardImage);
                                stackContent.Children.Add(labelCategorie);
                                stackContent.Children.Add(labelNom);
                                stackContent.Children.Add(labelDescriptif);
                                stackContent.Children.Add(labelQuantite);
                                stackContent.Children.Add(labelPrix);
                                stackContent.Children.Add(buttonVoirProduit);

                                var card = new Frame
                                {
                                    BorderColor = Colors.Black,
                                    Content = stackContent,
                                    CornerRadius = 10,
                                    Padding = new Thickness(10),
                                    HasShadow = true,
                                    HorizontalOptions = LayoutOptions.Center,
                                    VerticalOptions = LayoutOptions.Center
                                };

                                var ButtonRetourArriere = new Button
                                {
                                    Text = "Retour en arriere",
                                    FontFamily = "Pacifico",
                                    HorizontalOptions = LayoutOptions.Fill,
                                    Margin = new Thickness(20, 20, 20, 20),
                                    //VerticalOptions = LayoutOptions.EndAndExpand // Align at the bottom,
                                };

                                ButtonRetourArriere.Clicked += async (sender, e) =>
                                {
                                    // Code to execute when the button is clicked to add a product
                                    // Add the logic here to add a new product
                                    // Navigation.PushAsync(new Pages.AjoutProduit());
                                    await Shell.Current.GoToAsync("..");
                                };

                                grid.Add(card, 0, 1); // Add card to grid, starting from the second row
                                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                                //grid.Add(ajouterProduitButton, 0, items.Count + 1); // Add button to the last row
                                grid.Add(ButtonRetourArriere);
                                //stackContent.Children.Add(ajouterProduitButton);

                                var scrollView = new ScrollView { Content = grid };
                                Content = scrollView;
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
                }
            }
        }
        async public void BackToPage(object sender, EventArgs args)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
