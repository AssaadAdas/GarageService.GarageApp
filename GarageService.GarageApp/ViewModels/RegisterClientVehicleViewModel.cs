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
        public ICommand SaveCommand { get; }
        public ICommand BackCommand { get; }

        private async Task GoBack()
        {
            await Shell.Current.GoToAsync($"..");
        }

        private List<VehicleType> _vehicletypes;
        private VehicleType _selectedVehicleType;
        public List<VehicleType> vehicletypes
        {
            get => _vehicletypes;
            set => SetProperty(ref _vehicletypes, value);
        }

        public VehicleType SelectedVehicleType
        {
            get => _selectedVehicleType;
            set
            {
                if (SetProperty(ref _selectedVehicleType, value))
                {
                    VehicleTypeId = value?.Id ?? 0;
                }
            }
        }

        private List<Manufacturer> _manufacturers;
        private Manufacturer _selectedmanufacturers;

        public List<Manufacturer> manufacturers
        {
            get => _manufacturers;
            set => SetProperty(ref _manufacturers, value);
        }

        public Manufacturer SelectedManufacturer
        {
            get => _selectedmanufacturers;
            set
            {
                if (SetProperty(ref _selectedmanufacturers, value))
                {
                    ManufacturerId = value?.Id ?? 0;
                }
            }
        }
        public int VehicleTypeId { get; set; }

        public string VehicleName { get; set; } = null!;

        public int ManufacturerId { get; set; }

        public string Model { get; set; } = null!;

        public string LiscencePlate { get; set; } = null!;

        public int FuelTypeId { get; set; }

        public string ChassisNumber { get; set; } = null!;

        public int MeassureUnitId { get; set; }

        public string? Identification { get; set; }

        public bool Active { get; set; }

        public int Odometer { get; set; }

        private List<FuelType> _fueltypes;
        private FuelType _selectedFuelTypes;
        public List<FuelType> fueltypes
        {
            get => _fueltypes;
            set => SetProperty(ref _fueltypes, value);
        }

        public FuelType Selectedfueltype
        {
            get => _selectedFuelTypes;
            set
            {
                if (SetProperty(ref _selectedFuelTypes, value))
                {
                    FuelTypeId = value?.Id ?? 0;
                }
            }
        }

        private List<MeassureUnit> _meassureunits;
        private MeassureUnit _selectedmeassureunit;

        public List<MeassureUnit> meassureunits
        {
            get => _meassureunits;
            set => SetProperty(ref _meassureunits, value);
        }

        public MeassureUnit selectedmeassureunit
        {
            get => _selectedmeassureunit;
            set
            {
                if (SetProperty(ref _selectedmeassureunit, value))
                {
                    MeassureUnitId = value?.Id ?? 0;
                }
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

        private async void LoadVehiclesTypes()
        {
            try
            {
                ErrorMessage = string.Empty;
                var apiResponse = await _ApiService.GetVehicleTypesAsync();

                if (apiResponse.IsSuccess)
                {
                    vehicletypes = apiResponse.Data;
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

        private async void LoadMeasureUnits()
        {
            try
            {
                ErrorMessage = string.Empty;
                var apiResponse = await _ApiService.GetMeassureUnitsAsync();

                if (apiResponse.IsSuccess)
                {
                    meassureunits = apiResponse.Data;
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

        private async void LoadFuelTypes()
        {
            try
            {
                ErrorMessage = string.Empty;
                var apiResponse = await _ApiService.GetFuelTypesAsync();

                if (apiResponse.IsSuccess)
                {
                    fueltypes = apiResponse.Data;
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

        private async void LoadManufacturers()
        {
            try
            {
                ErrorMessage = string.Empty;
                var apiResponse = await _ApiService.GetManufacturersAsync();

                if (apiResponse.IsSuccess)
                {
                    manufacturers = apiResponse.Data;
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
            try
            {
                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) ||
               string.IsNullOrWhiteSpace(ConfirmPassword) || string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
                {
                    await Shell.Current.DisplayAlert("Error", "Please fill all required fields", "OK");
                    return;
                }
                if (string.IsNullOrWhiteSpace(FirstName) )
                {
                    await Shell.Current.DisplayAlert("Error", "Passwords do not match", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(ConfirmPassword) != string.IsNullOrWhiteSpace(Password))
                {
                    await Shell.Current.DisplayAlert("Error", "Passwords do not match", "OK");
                    return;
                }
                if (string.IsNullOrWhiteSpace(FirstName))
                {
                    await Shell.Current.DisplayAlert("Error", "FirstName is required fields", "OK");
                    return;
                }
                if (string.IsNullOrWhiteSpace(LastName))
                {
                    await Shell.Current.DisplayAlert("Error", "LastName is required fields", "OK");
                    return;
                }
                if (string.IsNullOrWhiteSpace(VehicleName))
                {
                    await Shell.Current.DisplayAlert("Error", "VehicleName is required fields", "OK");
                    return;
                }
                if (string.IsNullOrWhiteSpace(Model))
                {
                    await Shell.Current.DisplayAlert("Error", "Model is required fields", "OK");
                    return;
                }
                var userTypeResponse = await _ApiService.GetUserType(2); // 2 = client user type
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
                    await Shell.Current.DisplayAlert("Error", "Failed to create user", "OK");
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
                    await Shell.Current.DisplayAlert("Error", "Not Found", "OK");
                    return;
                }

                var clientProfile = new ClientProfile
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    CountryId = CountryId,
                    PhoneExt = PhoneExt,
                    PhoneNumber = PhoneNumber,
                    Email = Email,
                    Address = Address,
                    IsPremium = false,
                    UserId = user.Id
                };

                var profileAddedResponse = await _ApiService.ClientRegister(clientProfile);
                while (!profileAddedResponse.IsSuccess)
                {
                    await Shell.Current.DisplayAlert("Error", profileAddedResponse.ErrorMessage ?? "Failed to create profile", "OK");
                    return;
                }
                clientProfile = profileAddedResponse.Data; // Extract the User object
                var newvehicle = new Vehicle
                {
                    VehicleTypeId = VehicleTypeId,
                    VehicleName = VehicleName,
                    ManufacturerId = ManufacturerId,
                    Model = Model,
                    LiscencePlate = LiscencePlate,
                    FuelTypeId = FuelTypeId,
                    ChassisNumber = ChassisNumber,
                    MeassureUnitId = MeassureUnitId,
                    Identification = Identification,
                    Active = Active,
                    ClientId = clientProfile.Id, 
                    Odometer = Odometer
                };

                var VehicleAddedResponse = await _ApiService.AddVehicleAsync(newvehicle);
                if (VehicleAddedResponse.IsSuccess)
                {
                    await Shell.Current.DisplayAlert("Success", "Vehicle added successfully", "OK");
                    // Optionally, navigate back or clear the form
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", VehicleAddedResponse.Message, "OK");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public RegisterClientVehicleViewModel(ApiService apiservice)
        {
            _ApiService = apiservice;
            SaveCommand = new Command(async () => await Register());
            BackCommand = new Command(async () => await GoBack());
            LoadCountries();
            LoadVehiclesTypes();
            LoadMeasureUnits();
            LoadFuelTypes();
            LoadManufacturers();
        }
    }
}
