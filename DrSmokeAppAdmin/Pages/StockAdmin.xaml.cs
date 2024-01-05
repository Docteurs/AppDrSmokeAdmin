using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Controls.Shapes;
using System;
using System.Net;
using static System.Net.Mime.MediaTypeNames;

namespace DrSmokeAppAdmin.Pages;

public partial class StockAdmin : ContentPage
{
	public StockAdmin()
	{
		InitializeComponent();
        BlackCatPage();
        
    }

    public void BlackCatPage()
    {
        string descriptif = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum";
        List<Models.ProduitAdmin> listProduit = new List<Models.ProduitAdmin>();

        listProduit.Add(new Models.ProduitAdmin
        {
            Uuid = Guid.NewGuid().ToString(),
            CategorieProduit = "Fleur de CBD",
            NomProduit = "Amnesia",
            Descriptif = descriptif,
            Quantite = 1000,
            Q1g = true,
            Q3g = true,
            Q5g = false,
            Q10g = true,
            Q20g = true,
            UnGprix = 8,
            TroisGprix = 7.3m,
            CingGprix = 0,
            DixGprix = 70,
            VingtGprix = 120,
            ImgProduit = "https://i0.wp.com/drsmoke.fr/wp-content/uploads/2023/11/Copie-de-Popcorn-Amnesia-Dr-Smoke-Zoom-1-1.png?fit=2000%2C2000&ssl=1"
        });

        // Ajoutez d'autres éléments de la même manière
        listProduit.Add(new Models.ProduitAdmin
        {
            Uuid = Guid.NewGuid().ToString(),
            CategorieProduit = "Fleur CBD",
            NomProduit = "Blueberry Indoor",
            Descriptif = "Description du produit",
            Quantite = 500,
            Q1g = true,
            Q3g = false,
            Q5g = true,
            Q10g = false,
            Q20g = true,
            UnGprix = 5,
            TroisGprix = 10.5m,
            CingGprix = 0,
            DixGprix = 50,
            VingtGprix = 90,
            ImgProduit = "https://i0.wp.com/drsmoke.fr/wp-content/uploads/2023/01/Blueberry-.png?fit=2000%2C2000&ssl=1"
        });

        listProduit.Add(new Models.ProduitAdmin
        {
            Uuid = Guid.NewGuid().ToString(),
            CategorieProduit = "Fleur CBD",
            NomProduit = "Lemon Haze",
            Descriptif = descriptif,
            Quantite = 500,
            Q1g = true,
            Q3g = false,
            Q5g = true,
            Q10g = false,
            Q20g = true,
            UnGprix = 5,
            TroisGprix = 10.5m,
            CingGprix = 0,
            DixGprix = 50,
            VingtGprix = 90,
            ImgProduit = "https://i0.wp.com/drsmoke.fr/wp-content/uploads/2023/12/IMG_20231211_160924_ZOOM-removebg-preview.png?fit=564%2C443&ssl=1"
        });
        listProduit.Add(new Models.ProduitAdmin
        {
            Uuid = Guid.NewGuid().ToString(),
            CategorieProduit = "Fleur CBD",
            NomProduit = "Skittelz Indoor",
            Descriptif = descriptif,
            Quantite = 500,
            Q1g = true,
            Q3g = false,
            Q5g = true,
            Q10g = false,
            Q20g = true,
            UnGprix = 5,
            TroisGprix = 10.5m,
            CingGprix = 0,
            DixGprix = 50,
            VingtGprix = 90,
            ImgProduit = "https://i0.wp.com/drsmoke.fr/wp-content/uploads/2023/01/zkittlez-removebg-preview.png?fit=434%2C491&ssl=1"
        });

        

        Label titleLabel = new Label
        {
            Text = "Ici s'affiche vos produit",
            // More properties set here to define the Label appearance
        };
       
        Button button = new Button
        {
            Text = "Ajouter un produit", 
        };


        StackLayout stackLayout = new StackLayout();

        if (listProduit.Count != 0)
        {
            foreach (var produit in listProduit)
            {
                var stackContent = new StackLayout(); // Créez un StackLayout pour contenir les éléments

                // Ajoutez les labels et le bouton au StackLayout
                stackContent.Children.Add(new Label { Text = produit.CategorieProduit, FontFamily = "Pacifico" });
                stackContent.Children.Add(new Label { Text = produit.NomProduit, FontAttributes = FontAttributes.Bold, FontSize = 15, Margin = new Thickness(0, 5, 0, 5) });
                stackContent.Children.Add(new Label { Text = produit.Descriptif, FontFamily = "Pacifico" });
                stackContent.Children.Add(new Label { Text = produit.Quantite.ToString() + "g/En stock", FontFamily = "Pacifico", Margin = new Thickness(0, 5, 0, 5) });
                stackContent.Children.Add(new Label { Text = produit.UnGprix.ToString() + "€/1g", FontFamily = "Pacifico", Margin = new Thickness(0, 5, 0, 5) });
                stackContent.Children.Add(new Button { Text = "Modifier le produit" });

                var image = new Microsoft.Maui.Controls.Image
                {
                    Source = produit.ImgProduit,
                    WidthRequest = 300,
                    HeightRequest = 250,
                    Aspect = Aspect.AspectFill
                };

                stackContent.Children.Add(image); // Ajoutez l'image à l'intérieur du StackLayout

                Border gradientBorder = new Border
                {
                    StrokeThickness = 4,
                    // Background = Color.FromArgb("#2B0B98"),
                    Padding = new Thickness(16, 8),
                    HorizontalOptions = LayoutOptions.Center,
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = new CornerRadius(40, 0, 0, 40)
                    },
                    Stroke = new LinearGradientBrush
                    {
                        EndPoint = new Point(0, 1),
                        GradientStops = new GradientStopCollection
                {
                    new GradientStop { Color = Colors.Orange, Offset = 0.1f },
                    new GradientStop { Color = Colors.Brown, Offset = 1.0f }
                },
                    },
                    Content = stackContent // Ajoutez le StackLayout comme contenu du Border
                };

                stackLayout.Add(gradientBorder); // Ajoutez le gradientBorder à votre StackLayout principal
            }
        }
        else
        {
            DisplayAlert("Alerte", "Aucun produit trouvé", "OK");
        }

        // More Label objects go here

        ScrollView scrollView = new ScrollView();
        scrollView.Content = stackLayout;
        // ...

        Title = "Page produit";
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
        // grid.Add(button);
        grid.Add(titleLabel);
        grid.Add(scrollView, 0, 1);
        grid.Add(button, 0, 2);
        button.Clicked += async (sender, args) => await DisplayAlert("Alert", "CLiked", "OK");

        Content = grid;
    }
}