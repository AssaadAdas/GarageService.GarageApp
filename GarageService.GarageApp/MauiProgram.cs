using CommunityToolkit.Maui;
using GarageService.GarageApp.Services;
using GarageService.GarageApp.ViewModels;
using GarageService.GarageApp.Views;
using GarageService.GarageLib.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Syncfusion.Maui.Core.Hosting;

namespace GarageService.GarageApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().UseMauiCommunityToolkit();
        builder.UseMauiCommunityToolkit();
        builder
			.UseMauiApp<App>()
            .ConfigureSyncfusionCore()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
        
        // Register MAUI Maps handlers (correct API)
        //builder.ConfigureMauiHandlers(handlers =>
        //{
        //    handlers.AddMauiMaps();
        //});

        // register view models
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<GarageRegistrationViewModel>();   
        builder.Services.AddTransient<EditGarageViewModel>();
        builder.Services.AddTransient<GarageDashboardViewModel>();
        builder.Services.AddTransient<PremiumOffersViewModel>();
        builder.Services.AddTransient<RegisterClientVehicleViewModel>();
        builder.Services.AddTransient<SelectableServiceTypeViewModel>();
        builder.Services.AddTransient<VehiclesServiceViewModel>();
        builder.Services.AddTransient<VehiclesServiceTypeViewModel>();
        builder.Services.AddTransient<GaragePaymentOrdersViewModel>();
        builder.Services.AddTransient<PaymentViewModel>();
        builder.Services.AddTransient<GaragePaymentMethodViewModel>();
        builder.Services.AddTransient<PaymentMethodsViewModel>();
        builder.Services.AddTransient<EditPaymentMethodsViewModel>();
        builder.Services.AddTransient<LastServiceViewModel>();
        builder.Services.AddTransient<LastServiceTypeViewModel>();


        // Register Views
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<GarageDashboardPage>();
        builder.Services.AddTransient<GarageRegistrationPage>();
        builder.Services.AddTransient<EditGaragePage>();
        builder.Services.AddTransient<PremuimPage>();
        builder.Services.AddTransient<RegisterClientVehiclePage>();
        builder.Services.AddTransient<AddServiceTypePage>();
        builder.Services.AddTransient<ServicePage>();
        builder.Services.AddTransient<GaragePaymentOrdersPage>();
        builder.Services.AddTransient<PaymentPage>();
        builder.Services.AddTransient<GaragePaymentMethodPage>();
        builder.Services.AddSingleton<SettingsMenuPopup>();
        builder.Services.AddSingleton<PaymentMethodsPage>();
        builder.Services.AddSingleton<ChangePasswordPage>();
        builder.Services.AddSingleton<EditPaymentMethodsPage>();
        builder.Services.AddSingleton<LastServicePage>();
        builder.Services.AddSingleton<LastServiceTypePage>();
        


        // Services
        builder.Services.AddSingleton<ISessionService, SessionService>();
        builder.Services.AddSingleton<ISecureStorage>(SecureStorage.Default);
        builder.Services.AddSingleton<ApiService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<ServiceFormState>();
        

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
