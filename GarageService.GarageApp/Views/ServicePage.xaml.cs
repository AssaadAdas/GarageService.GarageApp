using GarageService.GarageApp.ViewModels;

namespace GarageService.GarageApp.Views;

public partial class ServicePage : ContentPage
{
	public ServicePage(VehiclesServiceViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}