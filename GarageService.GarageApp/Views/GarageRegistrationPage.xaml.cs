using GarageService.GarageApp.ViewModels;

namespace GarageService.GarageApp.Views;

public partial class GarageRegistrationPage : ContentPage
{
	public GarageRegistrationPage(GarageRegistrationViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}