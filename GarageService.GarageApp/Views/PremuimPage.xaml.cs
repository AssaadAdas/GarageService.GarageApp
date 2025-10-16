using GarageService.GarageApp.ViewModels;
using GarageService.GarageLib.Models;

namespace GarageService.GarageApp.Views;

public partial class PremuimPage : ContentPage
{
	public PremuimPage(PremiumOffersViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
    private void OnOfferCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && radioButton.BindingContext is PremiumOffer offer && e.Value)
        {
            if (BindingContext is PremiumOffersViewModel vm)
                vm.SelectedPremiumOffer = offer;
        }
    }
}