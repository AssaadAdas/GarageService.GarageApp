
using GarageService.GarageApp.ViewModels;
using GarageService.GarageLib.Models;

namespace GarageService.GarageApp.Views;

public partial class GaragePaymentOrdersPage : ContentPage
{
	public GaragePaymentOrdersPage(GaragePaymentOrdersViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
    private void OnOfferCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && radioButton.BindingContext is GaragePaymentMethod offer && e.Value)
        {
            if (BindingContext is GaragePaymentOrdersViewModel vm)
                vm.SelectedPaymentMethod = offer;
        }
    }
}