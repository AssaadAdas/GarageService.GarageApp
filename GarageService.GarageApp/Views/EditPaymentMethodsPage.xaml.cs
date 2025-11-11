using GarageService.GarageApp.ViewModels;

namespace GarageService.GarageApp.Views;

public partial class EditPaymentMethodsPage : ContentPage
{
	public EditPaymentMethodsPage(EditPaymentMethodsViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}