using System.Net.Http.Headers;
using System.Text.Json;


namespace DrSmokeAppAdmin.Pages;

public partial class Commande : ContentPage
{
    public List<Models.Commande> UCommande { get; private set; }
    HttpClient _client;
    JsonSerializerOptions _serializerOptions;
    public Commande()
	{
		InitializeComponent();

    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        // Charger les données ici
        await RecupCommandeUser();
    }
    private async Task<List<Models.Commande>> RecupCommandeUser()
    {
        _client = new HttpClient();
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        try
        {
            var oauthToken = await SecureStorage.Default.GetAsync("oauth_token");
            UCommande = new List<Models.Commande>();
            Uri uri = new Uri("http://localhost:3000/admin/commande-admin/");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", oauthToken);
            HttpResponseMessage response = await _client.GetAsync(uri);
            
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var UCommande = JsonSerializer.Deserialize<List<Models.Commande>>(content, _serializerOptions);
                if (UCommande != null && UCommande.Count > 0)
                {
                        CommandeAdmin(UCommande);
                }
                else
                {
                    NoCommande();
                }

            }
            else
            {
                await DisplayAlert("Alerte", "Une erreur est survenue", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Alerte", $"Une erreur est survenue {ex.ToString()}", "OK");
        }

        return UCommande;
    }

    public void CommandeAdmin(List<Models.Commande> ListCommande)
    {
        var grid = new Grid
        {
            RowDefinitions =
        {
            new RowDefinition(),
            
        },
            ColumnDefinitions =
        {
            new ColumnDefinition(),
         
        }
        };

        int rowIndex = 0;
        foreach (var commande in ListCommande)
        {
            var frame = new Frame
            {
                WidthRequest = 400,
                CornerRadius = 10,
                HasShadow = true,
                Margin = new Thickness(10),
                Padding = new Thickness(10),
                //BackgroundColor = Color.LightGray, // Si vous souhaitez une couleur de fond
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };

            var label1 = new Label
            {
                Text = "Label 1",
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
            };

            var label2 = new Label
            {
                Text = "Label 2",
                HorizontalOptions = LayoutOptions.Center,
            };

            var label3 = new Label
            {
                Text = "Label 3",
                HorizontalOptions = LayoutOptions.Center,
            };

            var button1 = new Button
            {
                Text = "Button 1",
                HorizontalOptions = LayoutOptions.Center,
                //BackgroundColor = Color.Gray,
                //TextColor = Color.White,
            };

            var button2 = new Button
            {
                Text = "Button 2",
                HorizontalOptions = LayoutOptions.Center,
                //BackgroundColor = Color.Gray,
                //TextColor = Color.White,
            };

            frame.Content = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
            {
                label1,
                label2,
                label3,
                new VerticalStackLayout
                {
                    
                    HorizontalOptions = LayoutOptions.Center,
                    Children =
                    {
                        button1,
                        button2,
                    }
                }
            }
            };

            grid.Add(frame, 0, rowIndex);
            rowIndex++;
        }

        var stackLayout = new StackLayout
        {
            Children =
        {
            grid,
        }
        };

        var scrollView = new ScrollView
        {
            Content = stackLayout,
        };

        Content = scrollView;
    }




    public void NoCommande()
    {
        Grid grid = new Grid
        {
            RowDefinitions =
        {
            new RowDefinition { Height = new GridLength(2, GridUnitType.Star) }
        },
            ColumnDefinitions =
        {
            new ColumnDefinition()
        }
        };

        Label label = new Label
        {
            Text = "Aucune Commande pour le moment"
        };

        ScrollView scrollView = new ScrollView
        {
            Margin = new Thickness(20),
            Content = grid
        };

        grid.Children.Add(label);

        Content = scrollView;
    }

}