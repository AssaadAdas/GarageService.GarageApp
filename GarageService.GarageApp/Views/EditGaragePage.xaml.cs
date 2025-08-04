using GarageService.GarageApp.ViewModels;

namespace GarageService.GarageApp.Views;

public partial class EditGaragePage : ContentPage
{
	public EditGaragePage(EditGarageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}