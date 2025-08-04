using GarageService.GarageApp.ViewModels;

namespace GarageService.GarageApp.Views;

public partial class GarageDashboardPage : ContentPage
{
	public GarageDashboardPage(GarageDashboardViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;

    }
}