using GarageService.GarageApp.Services;
using GarageService.GarageApp.ViewModels;

namespace GarageService.GarageApp.Views;

public partial class GarageDashboardPage : ContentPage
{
	public GarageDashboardPage(GarageDashboardViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is GarageDashboardViewModel vm)
        {
            await vm.LoadGarageProfile(); // Make sure this method fetches the latest profile
        }
        var navService = Handler.MauiContext.Services.GetService<INavigationService>();
        (navService as NavigationService)?.SetCurrentPage(this);
    }
}