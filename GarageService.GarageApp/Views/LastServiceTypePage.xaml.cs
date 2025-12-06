using GarageService.GarageApp.ViewModels;

namespace GarageService.GarageApp.Views;

public partial class LastServiceTypePage : ContentPage
{
	public LastServiceTypePage(LastServiceTypeViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}