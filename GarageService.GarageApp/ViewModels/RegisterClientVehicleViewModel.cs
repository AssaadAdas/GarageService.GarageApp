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
    public class RegisterClientVehicleViewModel : BaseViewModel
    {
        private readonly ApiService _ApiService;
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int UserTypeid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

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
        public string Address { get; set; }
        public int UserId { get; set; }
        public bool IsPremium { get; set; } = false;

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
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
        public ICommand RegisterCommand { get; }
        public ICommand BackCommand { get; }

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
        private async Task Register()
        {

        }

        public RegisterClientVehicleViewModel(ApiService apiservice)
        {
            _ApiService = apiservice;
            RegisterCommand = new Command(async () => await Register());
            BackCommand = new Command(async () => await GoBack());
            LoadCountries();
        }
    }
}
