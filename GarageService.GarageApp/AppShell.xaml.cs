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
            Routing.RegisterRoute(nameof(PremuimPage), typeof(PremuimPage));
            Routing.RegisterRoute(nameof(RegisterClientVehiclePage), typeof(RegisterClientVehiclePage));
            Routing.RegisterRoute(nameof(ServicePage), typeof(ServicePage));
            Routing.RegisterRoute(nameof(AddServiceTypePage), typeof(AddServiceTypePage));
            Routing.RegisterRoute(nameof(GaragePaymentOrdersPage), typeof(GaragePaymentOrdersPage));
            Routing.RegisterRoute(nameof(PaymentPage), typeof(PaymentPage));
            Routing.RegisterRoute(nameof(GaragePaymentMethodPage), typeof(GaragePaymentMethodPage));



            // Set initial route
            CurrentItem = Items[0];
        }
    }
}
