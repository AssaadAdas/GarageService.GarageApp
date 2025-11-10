using GarageService.GarageLib.Models;
using GarageService.GarageLib.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GarageService.GarageApp.ViewModels
{
    public class GarageRegistrationViewModel : BaseViewModel
    {
        private readonly ApiService _ApiService;
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int UserTypeid { get; set; } = 1;
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
        public int SpecializationId {
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

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand BackCommand { get; }

        public GarageRegistrationViewModel(ApiService apiservice)
        {
            _ApiService = apiservice;
            SaveCommand = new Command(async () => await Register());
            BackCommand = new Command(async () => await GoBack());
            LoadCountries();
            LoadSpecializations();

        }
        private async Task GoBack()
        {
            await Shell.Current.GoToAsync($"..");
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
        private async Task Register()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(ConfirmPassword) || string.IsNullOrWhiteSpace(GarageName))
            {
                await Shell.Current.DisplayAlert("Error", "Please fill all required fields", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(ConfirmPassword) != string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert("Error", "Passwords do not match", "OK");
                return;
            }
            var userTypeResponse = await _ApiService.GetUserType(1); // 2 = client user type
            if (!userTypeResponse.IsSuccess || userTypeResponse.Data == null)
            {
                await Shell.Current.DisplayAlert("Error", userTypeResponse.ErrorMessage ?? "Failed to get user type", "OK");
                return;
            }
            var usertype = userTypeResponse.Data;
            // Create user
            var user = new User
            {
                Username = Username,
                Password = Password, // Hash this in production
                UserTypeid = usertype.Id
            };

            var (isSuccess, message, registeredUser) = await _ApiService.RegisterUserAsync(user);
            if (!isSuccess)
            {
                await Shell.Current.DisplayAlert("Error", message, "OK");
                return;
            }

            // Get the user with ID
            var userAdded = await _ApiService.GetUserByUsername(Username);
            if (userAdded != null && userAdded.IsSuccess)
            {
                user = userAdded.Data; // Extract the User object
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", userAdded.ErrorMessage, "OK");
                return;
            }

            // Create garage Profiles
            var garageProfile = new GarageProfile
            {
                GarageName = GarageName,
                CountryId = CountryId,
                PhoneExt = PhoneExt,
                PhoneNumber = PhoneNumber,
                Email = Email,
                Address = Address,
                SpecializationId = SpecializationId,
                IsPremium = false,
                UserId = user.Id,
                GarageLocation = null,
                GaragePaymentMethods = null,
                GaragePaymentOrders = null,
                Country = null,
                GaragePremiumRegistrations = null,
                Specialization =  null,
                User = null,
                VehiclesServices =  null

            };
            var profileAddedResponse = await _ApiService.GarageRegister(garageProfile);

            if (profileAddedResponse.IsSuccess)
            {
                await Shell.Current.DisplayAlert("Success", "Registration successful", "OK");
                await Shell.Current.GoToAsync("//Login");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Internal error", "OK");
            }
        }
    }
}
