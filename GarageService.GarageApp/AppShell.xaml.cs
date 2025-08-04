using GarageService.GarageApp.Views;

namespace GarageService.GarageApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute("Main", typeof(MainPage));
            Routing.RegisterRoute(nameof(GarageRegistrationPage), typeof(GarageRegistrationPage));
            Routing.RegisterRoute(nameof(GarageDashboardPage), typeof(GarageDashboardPage));
            Routing.RegisterRoute(nameof(EditGaragePage), typeof(EditGaragePage));


            // Set initial route
            CurrentItem = Items[0];
        }
    }
}
