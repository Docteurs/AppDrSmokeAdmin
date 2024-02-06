namespace DrSmokeAppAdmin
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();

        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute("GestionBoutiqueAdmin", typeof(Pages.GestionBoutiqueAdmin));
            Routing.RegisterRoute("CoonexionPage", typeof(Pages.ConnexionPage));
            Routing.RegisterRoute("AjoutProduit", typeof(Pages.AjoutProduit));
            Routing.RegisterRoute("ProduitDetail", typeof(Pages.ProduitDetail));
            Routing.RegisterRoute("Commande", typeof(Pages.Commande));
           
            Routing.RegisterRoute("UpdateProduit", typeof(Pages.UpdateProduit));
            Routing.RegisterRoute("UpdateBoutique", typeof(Pages.UpdateBoutique));
        }



    }
}
