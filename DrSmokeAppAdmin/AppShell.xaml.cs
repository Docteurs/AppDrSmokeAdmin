namespace DrSmokeAppAdmin
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("MainPage", typeof(MainPage));
            Routing.RegisterRoute("GestionBoutiqueAdmin", typeof(Pages.GestionBoutiqueAdmin));
            Routing.RegisterRoute("StockAdmin", typeof(Pages.StockAdmin));
            Routing.RegisterRoute("AjoutProduit", typeof(Pages.AjoutProduit));
            
        }
     
    }
}
