using GarageService.GarageApp.Services;
using GarageService.GarageApp.Views;
using GarageService.GarageLib.Models;
using GarageService.GarageLib.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace GarageService.GarageApp.ViewModels
{
    [QueryProperty(nameof(VehicleId), "vehileid")]
    public class VehiclesServiceViewModel : BaseViewModel, IQueryAttributable
    {
        public VehiclesServiceViewModel(ApiService apiService, ServiceFormState formState, ISessionService sessionService)
        {
            _apiService = apiService;
            _formState = formState;
            _sessionService = sessionService;
            AddServiceTypeCommand = new Command(async () => await AddServiceTypes());
            SaveCommand = new Command(async () => await SaveService());
            BackCommand = new Command(async () => await GoBack());
            LoadGargesCommand = new Command(async () => await LoadGarageProfile());
            LoadGargesCommand.Execute(null);
            if (_formState.VehicleId != 0)
            {
                VehicleId = _formState.VehicleId;
                Odometer = _formState.Odometer;
                GarageId = GarageId;
                Notes = _formState.Notes;
                ServiceDate = _formState.ServiceDate;
                SelectedGarage = SelectedGarage;
                ServiceTypess = _formState.ServiceTypes ?? new ObservableCollection<SelectableServiceTypeViewModel>();
            }

            _sessionService = sessionService;
        }
        private GarageProfile _selectedGarage;
        public GarageProfile SelectedGarage
        {
            get => _selectedGarage;
            set
            {
                if (SetProperty(ref _selectedGarage, value))
                {
                    GarageId = value?.Id ?? 0;
                }
            }
        }
        
        public int GarageId { get; set; }

        private ObservableCollection<SelectableServiceTypeViewModel> _ServiceTypess = new();
        public ObservableCollection<SelectableServiceTypeViewModel> ServiceTypess
        {
            get => _ServiceTypess;
            set
            {
                _ServiceTypess = value;
                OnPropertyChanged(nameof(ServiceTypess));
            }
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {

            // Handle SelectedServiceTypes - only update if we're coming from AddServiceTypePage
            if (query.TryGetValue("SelectedServiceTypes", out var value) && value is IEnumerable<SelectableServiceTypeViewModel> selected)
            {
                if (ServiceTypess == null)
                {
                    ServiceTypess = new ObservableCollection<SelectableServiceTypeViewModel>(selected);
                }
                else
                {
                    // Remove items not in the new selection
                    var toRemove = ServiceTypess.Where(x => !selected.Any(s => s.Id == x.Id)).ToList();
                    foreach (var item in toRemove)
                        ServiceTypess.Remove(item);
                    decimal TotalServiceAmounts = 0;
                    // Add or update items from the new selection
                    foreach (var item in selected)
                    {
                        var existing = ServiceTypess.FirstOrDefault(x => x.Id == item.Id);
                        if (existing == null)
                        {
                            ServiceTypess.Add(item);
                            TotalServiceAmounts += item.Cost;
                        }
                            
                        else
                        {
                            // Only update the properties that might have changed in the service types
                            existing.IsSelected = item.IsSelected;
                            existing.Cost = item.Cost;
                            existing.Notes = item.Notes;
                            existing.CurrId = item.CurrId;
                            existing.CurrDesc = item.CurrDesc;
                            TotalServiceAmounts += item.Cost;
                        }
                    }
                    TotalServiceAmount = TotalServiceAmounts;
                    OnPropertyChanged(nameof(TotalServiceAmount));
                }
                OnPropertyChanged(nameof(ServiceTypess));
            }

            // Handle vehicleid
            if (query.TryGetValue("vehicleid", out var vehicleIdValue))
            {
                if (vehicleIdValue is int id)
                {
                    VehicleId = id;
                }
                else if (int.TryParse(vehicleIdValue?.ToString(), out int parsedId))
                {
                    VehicleId = parsedId;
                }
            }
        }
        
        decimal TotalServiceCost = 0;
        string Currency = "USD";
        private readonly ApiService _apiService;
        private readonly ISessionService _sessionService;
        private readonly ServiceFormState _formState;
        public ICommand LoadServicesTypesCommand { get; }
        public ICommand AddServiceTypeCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand LoadGargesCommand { get; }
        public ICommand BackCommand { get; }
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
        private Vehicle _vehicle;
        public Vehicle Vehicle
        {
            get => _vehicle;
            set
            {
                if (_vehicle != value)
                {
                    _vehicle = value;
                    OnPropertyChanged(nameof(Vehicle));
                }
            }
        }
        private DateTime _serviceDate = DateTime.Now; // Default value
        public DateTime ServiceDate
        {
            get => _serviceDate;
            set => SetProperty(ref _serviceDate, value);
        }

        private int _odometer;
        public int Odometer
        {
            get => _odometer;
            set => SetProperty(ref _odometer, value);
        }

        private string _ServiceLocation;
        public string ServiceLocation
        {
            get => _ServiceLocation;
            set => SetProperty(ref _ServiceLocation, value);
        }
        private string _Notes;
        public string Notes
        {
            get => _Notes;
            set => SetProperty(ref _Notes, value);
        }

        private int _vehileid;
        public int VehicleId
        {
            get => _vehileid;
            set
            {
                _vehileid = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
        public async Task LoadGarageProfile()
        {
            // Get current user ID from your authentication system
            int Garaged = GetCurrentUserId();
            GarageId = Garaged;
            var response = await _apiService.GetGarageByID(Garaged);
            if (response.IsSuccess)
            {
                GarageProfile = response.Data;
                SelectedGarage = GarageProfile;
            }
        }
        private decimal _totalServiceAmount;
        public decimal TotalServiceAmount
        {
            get => _totalServiceAmount;
            set => SetProperty(ref _totalServiceAmount, value);
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
        private async Task GoBack()
        {
            await Shell.Current.GoToAsync($"{nameof(GarageDashboardPage)}");
        }
        private async Task AddServiceTypes()
        {
            _formState.VehicleId = VehicleId;
            _formState.Odometer = Odometer;
            _formState.GarageId = GarageId;
            _formState.Notes = Notes;
            _formState.ServiceDate = ServiceDate;
            _formState.ServiceTypes = ServiceTypess;
            _formState.selectedGarage = SelectedGarage;
            _formState.SelectedServiceTypes = new ObservableCollection<SelectableServiceTypeViewModel>(
              ServiceTypess.Where(st => st.IsSelected));
            await Shell.Current.GoToAsync($"{nameof(AddServiceTypePage)}");
        }

        private async Task SaveService()
        {
            if (ServiceTypess is null)
            {
                await Shell.Current.DisplayAlert("Error", "Service types required fields", "OK");
                return;
            }
            if (Odometer == 0)
            {
                await Shell.Current.DisplayAlert("Error", "Odometer required fields", "OK");
                return;
            }
           
            var vehiclesService = new VehiclesService
            {
                ServiceDate = ServiceDate,
                Odometer = Odometer,
                ServiceLocation = GarageProfile.GarageLocation,
                Notes = Notes,
                Garageid = GarageProfile.Id,
                Vehicleid = VehicleId,
            };
            var ApiResponse = await _apiService.AddVehiclesServicesAsync(vehiclesService);
            var addedvehiclesservice = ApiResponse.Data;
            var vehiclesServiceTypes = new List<VehiclesServiceType>();
            foreach (var serviceType in ServiceTypess)
            {
                if (serviceType.IsSelected)
                {
                    vehiclesServiceTypes.Add(new VehiclesServiceType
                    {
                        VehicleServiceId = addedvehiclesservice.Id,
                        ServiceTypeId = serviceType.Id,
                        Cost = serviceType.Cost,
                        Notes = serviceType.Notes,
                        CurrId = serviceType.CurrId
                        
                    });
                    TotalServiceCost += serviceType.Cost;
                    
                }
                
            }
            foreach (var serviceType in vehiclesServiceTypes)
            {
                var (isSuccess2, message2, addedServiceType) = await _apiService.AddVehiclesServiceTypeAsync(serviceType);
                if (!isSuccess2)
                {
                    await Shell.Current.DisplayAlert("Error", message2, "OK");
                    return;
                }
            }
            CheckVehicle(VehicleId);
        }

        private async Task CheckVehicle(int vehicleid)
        {
            var CheckVehicle = new VehicleCheck
            {
                Vehicleid = vehicleid,
                CheckStatus = "out",
                CheckDate = DateTime.Now,
                GarageId = GarageProfile.Id
            };
            var response = await _apiService.SaveVehicleCheckAsync(CheckVehicle);
            if (response.IsSuccess)
            {
                WriteClientNotification(vehicleid);
                await Shell.Current.DisplayAlert("Success", "Service saved and vehicle checked out.", "OK");
                //await Shell.Current.GoToAsync($"..");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Failed to check out vehicle.", "OK");
            }
        }

        private async Task WriteClientNotification(int vehicleid)
        {
            var vehicleResponse = await _apiService.GetVehicleByID(vehicleid);
            if (vehicleResponse.IsSuccess)
            {
                Vehicle = vehicleResponse.Data;
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Failed to load vehicle details for notification.", "OK");
                return;
            }
            string Notes = $"Your vehicle with Liscence Plate {Vehicle.LiscencePlate} has been checked Out at {DateTime.Now} at Garage {GarageProfile.GarageName} with total cost {TotalServiceCost.ToString()} {Currency} .";

            var Nptification = new ClientNotification
            {

                Clientid = Vehicle.ClientId,
                Notes = Notes,

                IsRead = false
            };
            var response = await _apiService.SaveClientNotificationsAsync(Nptification);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
