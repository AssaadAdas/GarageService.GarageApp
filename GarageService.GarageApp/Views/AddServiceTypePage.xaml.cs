using GarageService.GarageApp.ViewModels;

namespace GarageService.GarageApp.Views;

public partial class AddServiceTypePage : ContentPage
{
	public AddServiceTypePage(VehiclesServiceTypeViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}