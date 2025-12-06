

using GarageService.GarageApp.ViewModels;

namespace GarageService.GarageApp.Views;

public partial class LastServicePage : ContentPage
{
	public LastServicePage(LastServiceViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}