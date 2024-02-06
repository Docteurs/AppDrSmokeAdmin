
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Controls.Shapes;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace DrSmokeAppAdmin.Pages;

public partial class StockAdmin : ContentPage
{
    public StockAdmin()
    {
        InitializeComponent();
        OnAppearing();
        stylePage();
        //generateProduitWithNoProduit();

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Charger les données ici
        BlackCatPage();
    }
    async public void BlackCatPage()
    {
        HttpClient _client;
        JsonSerializerOptions _serializerOptions;
        // string descriptif = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum";
        // List<Models.ProduitAdmin> listProduit = new List<Models.ProduitAdmin>();

        _client = new HttpClient();
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        string oauthToken = await SecureStorage.Default.GetAsync("oauth_token");
        var Items = new List<Models.ProduitAdmin>();
        Uri uri = new Uri(string.Format("https://get-evolutif.xyz/DrSmokeApi/admin/get-all-produit"));
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", oauthToken);

        //Uri uri = new Uri(string.Format($"http://localhost:3000/admin/admin/{oauthToken}"));
        try
        {
            HttpResponseMessage response = await _client.GetAsync(uri);
           
            if (response.IsSuccessStatusCode)
            {

                string content = await response.Content.ReadAsStringAsync();

                var items = JsonSerializer.Deserialize<List<Models.ProduitAdmin>>(content, _serializerOptions);

                if(items != null && items.Count > 0)
                {
                    var grid = new Grid();
                    grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                    if (items.Count > 0)
                    {
                        for (int i = 0; i < items.Count; i++)
                        {
                            var item = items[i];

                            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                            var labelCategorie = new Label { Text = item.CategorieProduit.ToString(), FontFamily = "NunitoRegular", FontAttributes = FontAttributes.Bold, TextColor = Color.FromRgba(26, 111, 54, 255), FontSize = 15, Margin = new Thickness(10) };
                            var labelNom = new Label { Text = item.NomProduit, FontFamily = "NunitoRegular", FontAttributes = FontAttributes.Bold, TextColor = Color.FromRgba(26, 111, 54, 255), FontSize = 15, Margin = new Thickness(10) };
                            var labelDescriptif = new Label { Text = item.Descriptif, FontFamily = "NunitoRegular", FontAttributes = FontAttributes.Bold, TextColor = Color.FromRgba(26, 111, 54, 255), FontSize = 15, Margin = new Thickness(10) };
                            var labelQuantite = new Label { Text = item.Quantite + "g/En stock", FontFamily = "NunitoRegular", FontAttributes = FontAttributes.Bold, TextColor = Color.FromRgba(26, 111, 54, 255), FontSize = 15, Margin = new Thickness(10) };
                            var labelPrix = new Label { Text = item.UnGprix + "€/1g", FontFamily = "NunitoRegular", FontAttributes = FontAttributes.Bold, TextColor = Color.FromRgba(26, 111, 54, 255), FontSize = 15, Margin = new Thickness(10) };
                            var buttonVoirProduit = new Button { Text = "Voir le produit" };
                            buttonVoirProduit.Clicked += async (sender, e) =>
                            {
                                // Code to execute when the button is clicked to add a product
                                // Add the logic here to add a new product
                                // Navigation.PushAsync(new Pages.AjoutProduit());
                                var navigationParameter = new ShellNavigationQueryParameters
                                {
                                    { "Uuid", item.Uuid }
                                };
                                //await Shell.Current.GoToAsync($"//ProduitDetail", navigationParameter);

                                await Shell.Current.GoToAsync($"ProduitDetail", navigationParameter);
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
                                Content = stackContent,
                                CornerRadius = 10,
                                Padding = new Thickness(10),
                                HasShadow = true,
                                HorizontalOptions = LayoutOptions.Center,
                                VerticalOptions = LayoutOptions.Center,
                                Margin = new Thickness(5,5,5,5),
                            };

                            grid.Add(card, 0, i + 1); // Add card to grid, starting from the second row
                        }

                        var ajouterProduitButton = new Button
                        {
                            Text = "Ajouter un produit",
                            FontFamily = "Pacifico",
                            HorizontalOptions = LayoutOptions.Fill,
                            Margin = new Thickness(20, 20, 20, 20),
                            //VerticalOptions = LayoutOptions.EndAndExpand // Align at the bottom,
                        };

                        ajouterProduitButton.Clicked += (sender, e) =>
                        {
                            // Code to execute when the button is clicked to add a product
                            // Add the logic here to add a new product
                            // Navigation.PushAsync(new Pages.AjoutProduit());
                            Shell.Current.GoToAsync("//AjoutProduit");
                        };

                        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                        //grid.Add(ajouterProduitButton, 0, items.Count + 1); // Add button to the last row
                        grid.Add(ajouterProduitButton, 0);

                        //stackContent.Children.Add(ajouterProduitButton);

                        var scrollView = new ScrollView { Content = grid };
                        Content = scrollView;
                    }
                    }
                else
                {
                    GenerateProduitWithNoProduit();
                }

              
            }
            else
            {
                GenerateProduitWithNoProduit();
            }
               
      
          

        }
        catch (Exception ex)
        {
            GenerateProduitWithNoProduit();
            await DisplayAlert("Alert", ex.ToString(), "OK");
            await Navigation.PushAsync(new Pages.ConnexionPage());

        }

    }

    public void GenerateProduitWithNoProduit()
    {
        // Title Label
        var titleLabel = new Label
        {
            Text = "Produit magasin",
            FontAttributes = FontAttributes.Bold,
            FontSize = 24,
            Margin = new Thickness(0, 0, 0, 10)
        };

        // Content Label
        var contentLabel = new Label
        {
            Text = "Aucun produit à afficher pour le moment",
            FontSize = 18,
            TextColor = Colors.Gray,
            HorizontalOptions = LayoutOptions.Center
        };

        // "Ajouter un produit" Button
        var addButton = new Button
        {
            Text = "Ajouter un produit",
            FontSize = 18,
            // BackgroundColor = Colors.Accent,
            TextColor = Colors.White,
            Margin = new Thickness(0, 20, 0, 0)
        };
        addButton.Clicked += async (sender, e) => await Navigation.PushModalAsync(new AjoutProduit());

        // "Déconnexion" Button
        var deconnexionButton = new Button
        {
            Text = "Déconnexion",
            FontSize = 18,
            BackgroundColor = Colors.Red,
            TextColor = Colors.White,
            Margin = new Thickness(0, 10, 0, 0)
        };
        deconnexionButton.Clicked += async (sender, e) => await Navigation.PushModalAsync(new MainPage());

        // StackLayout for Buttons
        var buttonsStackLayout = new StackLayout
        {
            Children = { addButton, deconnexionButton }
        };

        // Main StackLayout
        var mainStackLayout = new StackLayout
        {
            Padding = new Thickness(20),
            Children = { titleLabel, contentLabel, buttonsStackLayout }
        };

        // ScrollView
        var scrollView = new ScrollView
        {
            Content = mainStackLayout
        };

        // Grid
        var grid = new Grid
        {
            Margin = new Thickness(20),
            RowDefinitions =
        {
            new RowDefinition { Height = GridLength.Auto },
            new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
            new RowDefinition { Height = GridLength.Auto }
        }
        };
        grid.Add(scrollView, 0, 1);

        Content = grid;
    }

    public void stylePage() 
    {
        // Style pour tous les boutons
        Style buttonStyle = new Style(typeof(Button))
        {
            Setters = {
        new Setter { Property = Button.BackgroundColorProperty, Value = Color.FromRgba(26, 111, 54, 255) },
        new Setter { Property = Button.FontSizeProperty, Value = 20 },
        new Setter { Property = Button.TextColorProperty, Value = Color.FromRgba(255, 255, 255, 255) }
            }
        };
       

       /* Style labelStyle = new Style(typeof(Label))
        {
            Setters = {
        new Setter { Property = Label.FontFamilyProperty, Value = "NunitoRegular" },
        new Setter { Property = Label.FontAttributesProperty, Value = FontAttributes.Bold },
        new Setter { Property = Label.TextColorProperty, Value = Color.FromRgba(26, 111, 54, 255) },
        new Setter { Property = Label.FontSizeProperty, Value = 15 },
        new Setter { Property = Label.MarginProperty, Value = new Thickness(10) }
            }
        };
        Resources.Add(labelStyle);*/
        Resources.Add(buttonStyle);

    }



}