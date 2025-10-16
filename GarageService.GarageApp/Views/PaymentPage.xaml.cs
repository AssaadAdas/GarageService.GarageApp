using GarageService.GarageApp.ViewModels;

namespace GarageService.GarageApp.Views;

public partial class PaymentPage : ContentPage
{
	public PaymentPage(PaymentViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}