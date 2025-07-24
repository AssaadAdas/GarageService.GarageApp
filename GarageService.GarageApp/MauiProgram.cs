using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using GarageService.GarageLib.Services;
using GarageService.GarageApp.Services;

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
