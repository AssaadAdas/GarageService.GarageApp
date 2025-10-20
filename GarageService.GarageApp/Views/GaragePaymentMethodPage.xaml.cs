using GarageService.GarageApp.ViewModels;

namespace GarageService.GarageApp;

public partial class GaragePaymentMethodPage : ContentPage
{
	public GaragePaymentMethodPage(GaragePaymentMethodViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}