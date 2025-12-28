using GarageService.GarageLib.Models;
using GarageService.GarageLib.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Maps;

namespace GarageService.GarageApp.ViewModels
{
    public class EditGarageViewModel: BaseViewModel
    {
        private readonly ISessionService _sessionService;
        private readonly ApiService _ApiService;
        private GarageProfile _garageProfile;
        public GarageProfile GarageProfile
        {
            get => _garageProfile;
            set
            {
                if (_garageProfile != value)
                {
                    _garageProfile = value;
                    OnPropertyChanged(nameof(GarageProfile));
                }
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand LoadCommand { get; }
        public ICommand LoadCountriesCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand GetCurrentLocationCommand { get; }
        public string GarageName { get; set; }
        public string Address { get; set; }
        private int _selectedCountryId;
        public int CountryId
        {
            get => _selectedCountryId;
            set
            {
                SetProperty(ref _selectedCountryId, value);
                // If you need to find the full country object:
                SelectedCountry = Countries?.FirstOrDefault(c => c.Id == value);
            }
        }

        private string _phoneExt;
        public string PhoneExt
        {
            get => _phoneExt;
            set => SetProperty(ref _phoneExt, value);
        }
        public int PhoneNumber { get; set; }
        public string Email { get; set; }
        int _SpecializationId;
        public int SpecializationId
        {
            get => _SpecializationId;
            set
            {
                SetProperty(ref _SpecializationId, value);
                // If you need to find the full country object:
                Selectedspecialization = specializations?.FirstOrDefault(c => c.Id == value);
            }
        }

        private List<Country> _countries;
        private Country _selectedCountry;
        public List<Country> Countries
        {
            get => _countries;
            set => SetProperty(ref _countries, value);
        }

        public Country SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                if (SetProperty(ref _selectedCountry, value))
                {
                    CountryId = value?.Id ?? 0;
                    PhoneExt = value?.PhoneExt ?? string.Empty; // Set PhoneExt from selected country
                }
            }
        }

        //speci

        private List<Specialization> _specializations;
        private Specialization _selectedspecialization;
        public List<Specialization> specializations
        {
            get => _specializations;
            set => SetProperty(ref _specializations, value);
        }

        public Specialization Selectedspecialization
        {
            get => _selectedspecialization;
            set
            {
                if (SetProperty(ref _selectedspecialization, value))
                {
                    SpecializationId = value?.Id ?? 0;
                }
            }
        }

        // Location properties
        private double _selectedLatitude;
        public double SelectedLatitude
        {
            get => _selectedLatitude;
            set => SetProperty(ref _selectedLatitude, value);
        }

        private double _selectedLongitude;
        public double SelectedLongitude
        {
            get => _selectedLongitude;
            set => SetProperty(ref _selectedLongitude, value);
        }

        private string _garageLocation;
        public string GarageLocation
        {
            get => _garageLocation;
            set => SetProperty(ref _garageLocation, value);
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public EditGarageViewModel(ApiService apiservice, ISessionService sessionService)
        {
            _ApiService = apiservice;
            _sessionService = sessionService;
            SaveCommand = new Command(async () => await Register());
            BackCommand = new Command(async () => await GoBack());
            GetCurrentLocationCommand = new Command(async () => await UseCurrentLocationAsync());
            LoadCountries();
            LoadSpecializations();
            LoadCommand = new Command(async () => await LoadProfile());
            LoadCommand.Execute(null);
        }

        private async Task GoBack()
        {
            await Shell.Current.GoToAsync($"..");
        }
        public void SetSelectedLocation(double lat, double lng)
        {
            SelectedLatitude = lat;
            SelectedLongitude = lng;
            _ = ReverseGeocodeAsync(lat, lng);
        }

        private async Task ReverseGeocodeAsync(double lat, double lng)
        {
            try
            {
                var placemarks = await Geocoding.Default.GetPlacemarksAsync(lat, lng);
                var place = placemarks?.FirstOrDefault();
                if (place != null)
                {
                    GarageLocation = $"{lat},{lng}"; // store as "lat,lng"
                    // you can also store human readable address in a separate field if needed
                    Address = string.IsNullOrWhiteSpace(place.Thoroughfare) ? Address : $"{place.Thoroughfare} {place.SubThoroughfare}, {place.Locality}";
                    OnPropertyChanged(nameof(Address));
                }
                else
                {
                    GarageLocation = $"{lat},{lng}";
                }
            }
            catch (Exception ex)
            {
                GarageLocation = $"{lat},{lng}";
                Debug.WriteLine(ex);
            }
        }

        private async Task UseCurrentLocationAsync()
        {
            try
            {
                var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    await Application.Current.MainPage.DisplayAlert("Permission required", "Location permission is required", "OK");
                    return;
                }

                var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                var location = await Geolocation.Default.GetLocationAsync(request);
                if (location == null) return;

                SetSelectedLocation(location.Latitude, location.Longitude);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void LoadCountries()
        {
            try
            {
                ErrorMessage = string.Empty;
                var apiResponse = await _ApiService.GetCountriesAsync();

                if (apiResponse.IsSuccess)
                {
                    Countries = apiResponse.Data;
                }
                else
                {
                    ErrorMessage = apiResponse.ErrorMessage;
                    // Optionally log the error
                    Debug.WriteLine($"API Error: {apiResponse.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "An unexpected error occurred";
                Debug.WriteLine($"Exception: {ex}");
            }
        }

        private async void LoadSpecializations()
        {
            try
            {
                ErrorMessage = string.Empty;
                var apiResponse = await _ApiService.GetSpecializationsAsync();

                if (apiResponse.IsSuccess)
                {
                    specializations = apiResponse.Data;
                }
                else
                {
                    ErrorMessage = apiResponse.ErrorMessage;
                    // Optionally log the error
                    Debug.WriteLine($"API Error: {apiResponse.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "An unexpected error occurred";
                Debug.WriteLine($"Exception: {ex}");
            }
        }

        private int GetCurrentUserId()
        {
            // Implement your actual user ID retrieval logic
            int userId;
            string userType;
            int profileId = 1;
            if (_sessionService.IsLoggedIn)
            {
                userId = _sessionService.UserId;
                userType = _sessionService.UserType.ToString();
                profileId = _sessionService.ProfileId;
            }
            else
            {
                // If not logged in, you might want to redirect to login page or throw an exception
                throw new UnauthorizedAccessException("User is not logged in.");
            }

            return profileId; // Placeholder
        }
        private async Task LoadProfile()
        {
            // Get current user ID from your authentication system
            int GarageId = GetCurrentUserId();

            var response = await _ApiService.GetGarageByID(GarageId);
            if (response.IsSuccess)
            {
                GarageProfile = response.Data;
                if (GarageProfile != null)
                {
                    GarageName = GarageProfile.GarageName;
                    Email = GarageProfile.Email;
                    Address = GarageProfile.Address;
                    PhoneNumber = GarageProfile.PhoneNumber;
                    CountryId = GarageProfile.CountryId;
                    PhoneExt = GarageProfile.PhoneExt;
                    // Set the selected country based on the CountryId
                    SelectedCountry = Countries?.FirstOrDefault(c => c.Id == CountryId);
                    SpecializationId= GarageProfile.SpecializationId;
                    Selectedspecialization = specializations?.FirstOrDefault(c => c.Id == SpecializationId);

                    OnPropertyChanged(nameof(GarageName));
                    OnPropertyChanged(nameof(Email));
                    OnPropertyChanged(nameof(PhoneNumber));
                    OnPropertyChanged(nameof(Address));
                    OnPropertyChanged(nameof(CountryId));
                    OnPropertyChanged(nameof(SpecializationId));
                    OnPropertyChanged(nameof(PhoneExt));
                    OnPropertyChanged(nameof(SelectedCountry));
                    OnPropertyChanged(nameof(Selectedspecialization));
                }
            }
        }

        private async Task Register()
        {
            if (string.IsNullOrWhiteSpace(GarageName))
            {
                await Shell.Current.DisplayAlert("Error", "Please fill all required fields", "OK");
                return;
            }

            // Update garage Profiles
            GarageProfile.GarageName = GarageName;
            GarageProfile.Email = Email;
            GarageProfile.PhoneExt = PhoneExt;
            GarageProfile.PhoneNumber = PhoneNumber;
            GarageProfile.CountryId = SelectedCountry?.Id ?? CountryId;
            GarageProfile.Address = Address;
            GarageProfile.GarageLocation = !string.IsNullOrWhiteSpace(GarageLocation) ? GarageLocation : null; ;
            GarageProfile.SpecializationId = SpecializationId;
            bool success = await _ApiService.UpdateGarageProfileAsync(GarageProfile.Id, GarageProfile);

            if (success)
            {
                await Shell.Current.DisplayAlert("Success", "Profile updated successfully", "OK");
                await Shell.Current.GoToAsync(".."); // This pops the Edit page and returns to the dashboard
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Failed to update profile", "OK");
            }
        }
    }
}
