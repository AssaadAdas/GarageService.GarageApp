using GarageService.GarageApp.ViewModels;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace GarageService.GarageApp.Views;

public partial class EditGaragePage : ContentPage
{
    readonly EditGarageViewModel _vm;
    public EditGaragePage(EditGarageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = _vm = viewModel;
    }
    //fired when user taps map
    private void OnMapClicked(object? sender, MapClickedEventArgs e)
    {
        try
        {
            //LocationMap.Pins.Clear();

            //var pin = new Pin
            //{
            //    Label = "Selected",
            //    Location = e.Location,
            //    Type = PinType.Place
            //};
            //LocationMap.Pins.Add(pin);

            //LocationMap.MoveToRegion(MapSpan.FromCenterAndRadius(e.Location, Distance.FromMeters(500)));

            //_vm.SetSelectedLocation(e.Location.Latitude, e.Location.Longitude);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
        }
    }
}