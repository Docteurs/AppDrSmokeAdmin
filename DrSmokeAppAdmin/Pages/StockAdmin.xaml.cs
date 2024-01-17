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
        BlackCatPage();
        //generateProduitWithNoProduit();

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
        Uri uri = new Uri(string.Format("http://localhost:3000/admin/get-all-produit"));
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", oauthToken);

        //Uri uri = new Uri(string.Format($"http://localhost:3000/admin/admin/{oauthToken}"));
        try
        {
            HttpResponseMessage response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                try
                {
                    var items = JsonSerializer.Deserialize<List<Models.ProduitAdmin>>(content, _serializerOptions);

                    var grid = new Grid();
                    grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                    if (items.Count > 0)
                    {
                        for (int i = 0; i < items.Count; i++)
                        {
                            var item = items[i];

                            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                            var labelCategorie = new Label { Text = item.CategorieProduit.ToString(), FontFamily = "Pacifico" };
                            var labelNom = new Label { Text = item.NomProduit, FontAttributes = FontAttributes.Bold, FontSize = 15, Margin = new Thickness(0, 5, 0, 5) };
                            var labelDescriptif = new Label { Text = item.Descriptif, FontFamily = "Pacifico", WidthRequest = 250, HeightRequest = 250 };
                            var labelQuantite = new Label { Text = item.Quantite + "g/En stock", FontFamily = "Pacifico", Margin = new Thickness(0, 5, 0, 5) };
                            var labelPrix = new Label { Text = item.UnGprix + "€/1g", FontFamily = "Pacifico", Margin = new Thickness(0, 5, 0, 5) };
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
                        };

                        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                        //grid.Add(ajouterProduitButton, 0, items.Count + 1); // Add button to the last row
                        grid.Add(ajouterProduitButton);
                        //stackContent.Children.Add(ajouterProduitButton);

                        var scrollView = new ScrollView { Content = grid };
                        Content = scrollView;
                    }
                    else
                    {
                        generateProduitWithNoProduit();
                    }
                }
                catch (Exception ex)
                {
                    generateProduitWithNoProduit();
                    await DisplayAlert("Alert", $"Une erreur est survenue: {ex.ToString()}", "OK");
                }
            }

        }
        catch (Exception ex)
        {
            generateProduitWithNoProduit();
            await DisplayAlert("Alert", ex.ToString(), "OK");

        }




    }
    public void generateProduitAdminWithStock(string CategorieProduit, string NomProduit, string Descriptif, int Quantite, decimal UnGprix, string ImgProduit)
    {
        Label titleLabel = new Label
        {
            Text = "Ici s'affiche vos produits",
            // Plus de propriétés définies ici pour l'apparence du Label
        };

        Button button = new Button
        {
            Text = "Ajouter un produit",
        };

        StackLayout stackContent = new StackLayout();

        // Créer les labels et les ajouter au StackLayout
        var labelCategorie = new Label { Text = CategorieProduit, FontFamily = "Pacifico" };
        var labelNom = new Label { Text = NomProduit, FontAttributes = FontAttributes.Bold, FontSize = 15, Margin = new Thickness(0, 5, 0, 5) };
        var labelDescriptif = new Label { Text = Descriptif, FontFamily = "Pacifico" };
        var labelQuantite = new Label { Text = Quantite.ToString() + "g/En stock", FontFamily = "Pacifico", Margin = new Thickness(0, 5, 0, 5) };
        var labelPrix = new Label { Text = UnGprix.ToString() + "€/1g", FontFamily = "Pacifico", Margin = new Thickness(0, 5, 0, 5) };
        var image = new Microsoft.Maui.Controls.Image { Source = ImgProduit.ToString(), WidthRequest = 400, HeightRequest = 400 };

        stackContent.Children.Add(image);
        stackContent.Children.Add(labelCategorie);
        stackContent.Children.Add(labelNom);
        stackContent.Children.Add(labelDescriptif);
        stackContent.Children.Add(labelQuantite);
        stackContent.Children.Add(labelPrix);

        // Créer le Frame et lui assigner le StackLayout comme contenu
        Frame frame = new Frame()
        {
            BorderColor = Colors.Black,
            Content = stackContent, // Assigner le StackLayout comme contenu du Frame
            Padding = new Thickness(10),
            CornerRadius = 10,
            HasShadow = true,
            HorizontalOptions = LayoutOptions.Center
        };

        ScrollView scrollView = new ScrollView
        {
            Content = frame // Ajouter le Frame à la ScrollView
        };

        Title = "Page produit";

        var buttonStack = new StackLayout
        {
            HorizontalOptions = LayoutOptions.Center
        };
        buttonStack.Children.Add(button);

        button.Clicked += async (sender, args) => await DisplayAlert("Alert", "Clicked", "OK");

        var mainStack = new StackLayout
        {
            Padding = new Thickness(20),
            VerticalOptions = LayoutOptions.CenterAndExpand,
            HorizontalOptions = LayoutOptions.CenterAndExpand,
            Children =
        {
            titleLabel,
            scrollView,
            buttonStack
        }
        };

        Content = mainStack;
    }


    public void generateProduitWithNoProduit()
    {
        Label titleLabel = new Label
        {
            Text = "Produit magasin",
            // More properties set here to define the Label appearance
        };

        StackLayout stackLayout = new StackLayout();
        stackLayout.Add(new Label { Text = "Aucun produit a afficher pour le moment" });
        //stackLayout.Add(new Button { Text = "Ajouter un produit" });
        // More Label objects go here

        ScrollView scrollView = new ScrollView();
        scrollView.Content = stackLayout;
        // ...

        Title = "Produit boutique";
        Grid grid = new Grid
        {
            Margin = new Thickness(20),
            RowDefinitions =
            {
                new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) }
            }
        };
        Button deconnexion = new Button
        {
            Text = "Deconnexion"
        };

        Button button = new Button { Text = "Ajouter un produit" };
        button.Clicked += async (sender, e) => { await Navigation.PushModalAsync(new AjoutProduit()); };
        deconnexion.Clicked += async (sender, e) => { await Navigation.PushModalAsync(new MainPage()); };
        grid.Add(titleLabel);
        grid.Add(scrollView, 0, 1);
        grid.Add(button, 0, 2);
        grid.Add(deconnexion, 0, 3);

        Content = grid;
    }

}