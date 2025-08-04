using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using GarageService.GarageLib.Services;
using GarageService.GarageApp.Services;
using GarageService.GarageApp.Views;
using GarageService.GarageApp.ViewModels;

namespace GarageService.GarageApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().UseMauiCommunityToolkit();
        builder
			.UseMauiApp<App>()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        // register view models
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<GarageRegistrationViewModel>();
        builder.Services.AddTransient<EditGarageViewModel>();
        builder.Services.AddTransient<GarageDashboardViewModel>();

        // Register Views
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<GarageDashboardPage>();
        builder.Services.AddTransient<GarageRegistrationPage>();
        builder.Services.AddTransient<EditGaragePage>();
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
