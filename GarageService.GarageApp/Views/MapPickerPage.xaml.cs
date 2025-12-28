using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace GarageService.GarageApp.Views;

public partial class MapPickerPage : ContentPage
{
    private Pin _selectedPin;
    public Location SelectedLocation { get; private set; }

    public MapPickerPage()
    {
        InitializeComponent();
        _selectedPin = new Pin
        {
            Label = "Garage Location",
            Type = PinType.Place
        };
        LoadCurrentLocation();
    }

    private async void LoadCurrentLocation()
    {
        try
        {
            var location = await Geolocation.Default.GetLastKnownLocationAsync();
            if (location == null)
            {
                location = await Geolocation.Default.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10)));
            }

            if (location != null)
            {
                locationMap.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(1)));
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error getting location: {ex.Message}");
        }
    }

    private void OnMapClicked(object sender, MapClickedEventArgs e)
    {
        SelectedLocation = e.Location;
        
        locationMap.Pins.Clear();
        _selectedPin.Location = e.Location;
        locationMap.Pins.Add(_selectedPin);
        
        LocationLabel.Text = $"Selected: {e.Location.Latitude:F6}, {e.Location.Longitude:F6}";
    }

    private async void OnConfirmClicked(object sender, EventArgs e)
    {
        if (SelectedLocation == null)
        {
            await DisplayAlert("Error", "Please tap on the map to select a location", "OK");
            return;
        }

        MessagingCenter.Send(this, "LocationSelected", SelectedLocation);
        await Navigation.PopAsync();
    }
}
