using GarageService.GarageApp.ViewModels;

namespace GarageService.GarageApp.Views;

public partial class PremuimPage : ContentPage
{
	public PremuimPage(PremiumOffersViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}