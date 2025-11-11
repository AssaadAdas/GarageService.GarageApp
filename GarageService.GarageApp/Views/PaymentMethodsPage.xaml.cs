using GarageService.GarageApp.ViewModels;

namespace GarageService.GarageApp.Views;

public partial class PaymentMethodsPage : ContentPage
{
	public PaymentMethodsPage(PaymentMethodsViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}