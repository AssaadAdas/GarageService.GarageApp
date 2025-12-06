using GarageService.GarageApp.Services;
using GarageService.GarageApp.Views;
using GarageService.GarageLib.Models;
using GarageService.GarageLib.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GarageService.GarageApp.ViewModels
{
    public  class GarageDashboardViewModel: BaseViewModel
    {
        private readonly ApiService _ApiService;
        private readonly ISessionService _sessionService;
        
        public ICommand EditProfileCommand { get; }
        public ICommand AddVehicleCommand { get; }
        public ICommand SearchVehicleCommand { get; }
        public ICommand AddServicesCommand { get; }
        public ICommand GoPendingOrderCommand { get; }
        public ICommand CheckVehicleCommand { get; }
        public ICommand PremuimCommand { get; }
        public ICommand GoSettingCommand { get; }

        public ICommand LastServiceCommand { get; }
        public ICommand LastServiceByTypeCommand { get; }
        private readonly INavigationService _navigationService;
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

        private List<VehicleAppointment> _UpcomingvehicleAppointments;
        public ObservableCollection<VehicleAppointment> UpcomingVehicleAppointments { get; set; } = new();
        
        private GaragePremiumRegistration _GaragePremiumRegistration;
        public GaragePremiumRegistration GaragePremiumRegistration
        {
            get => _GaragePremiumRegistration;
            set
            {
                if (_GaragePremiumRegistration != value)
                {
                    _GaragePremiumRegistration = value;
                    OnPropertyChanged(nameof(GaragePremiumRegistration));
                }
            }
        }

        private List<GaragePaymentOrder> _PendingOrders;
        public List<GaragePaymentOrder> PendingOrders 
         {
            get => _PendingOrders;
            set
            {
                if (_PendingOrders != value)
                {
                    _PendingOrders = value;
                    OnPropertyChanged(nameof(PendingOrders));
                 }
}
        }
        private ObservableCollection<Vehicle> _vehicles = new();
        public ObservableCollection<Vehicle> Vehicles
        {
            get => _vehicles;
            set
            {
                if (_vehicles != value)
                {
                    _vehicles = value;
                    OnPropertyChanged(nameof(Vehicles));
                }
            }
        }

        private string _liscencePlate;
        public string LiscencePlate
        {
            get => _liscencePlate;
            set => SetProperty(ref _liscencePlate, value);
        }
        public GarageDashboardViewModel(ApiService apiservice, ISessionService sessionService, INavigationService navigationService)
        {
            // Initialize properties and commands
            _ApiService = apiservice;
            _sessionService = sessionService;
            _navigationService = navigationService;
            PremuimCommand = new Command(async () => await LoadPremuim());
            //LogOutCommand = new Command(async () => await LogOut());
            AddVehicleCommand = new Command(async () => await AddVehicle());
            AddServicesCommand = new Command<Vehicle>(async (vehicle) => await AddServices(vehicle));
            CheckVehicleCommand = new Command<Vehicle>(async (vehicle) => await CheckVehicle(vehicle));
            GoPendingOrderCommand = new Command<GaragePaymentOrder>(async (GaragePaymentOrder) => await GoPendingOrder(GaragePaymentOrder));
            SearchVehicleCommand = new Command(async () => await SearchVehicleAsync());
            GoSettingCommand = new Command(async () => await GoSettingMethods());
            EditProfileCommand = new Command(async () => await EditProfile());

            LastServiceCommand = new Command<Vehicle>(async (vehicle) => await LastService(vehicle));
            LastServiceByTypeCommand = new Command<Vehicle>(async (vehicle) => await LastServiceByType(vehicle));

            //btnServices.enabled = false;
            LoadGarageProfile();
        }
        private async Task GoSettingMethods()
        {
            int ClientId = GetCurrentUserId();
            var popup = new SettingsMenuPopup();
            await _navigationService.ShowPopupAsync(popup);
        }


        private async Task LastService(Vehicle vehicle)
        {
            await Shell.Current.GoToAsync($"{nameof(LastServicePage)}?vehicleid={vehicle.Id}");
        }

        private async Task LastServiceByType(Vehicle vehicle)
        {
            await Shell.Current.GoToAsync($"{nameof(LastServiceTypePage)}?vehicleid={vehicle.Id}");
            
        }
        private async Task AddServices(Vehicle vehicle)
        {
            await Shell.Current.GoToAsync($"{nameof(ServicePage)}?vehileid={vehicle.Id}");
        }
        private async Task AddVehicle()
        {
            await Shell.Current.GoToAsync($"{nameof(RegisterClientVehiclePage)}");
        }
        private async Task GoPendingOrder(GaragePaymentOrder PendingOrder)
        {
            await Shell.Current.GoToAsync($"{nameof(PaymentPage)}?paymentOrderid={PendingOrder.Id}");
        }
        private async Task CheckVehicle(Vehicle vehicle)
        {
            var CheckVehicle = new VehicleCheck
            {
                Vehicleid = vehicle.Id,
                CheckStatus = "IN",
                CheckDate = DateTime.Now,
                GarageId = GarageProfile.Id
            };
            var response = await _ApiService.SaveVehicleCheckAsync(CheckVehicle);

            if (response.IsSuccess)
            {
                vehicle.IsChecked = true;
                //btnServices.enabled = true;
                //btnChecks.enabled = false;
                WriteClientNotification(vehicle);
            }
            else
            {
                //btnServices.enabled = false;
                //btnChecks.enabled = true;
                vehicle.IsChecked = false;
            }
        }

        private async Task WriteClientNotification(Vehicle vehicle)
        {
            string Notes = $"Your vehicle with Liscence Plate {vehicle.LiscencePlate} has been checked in at {DateTime.Now} at Garage {GarageProfile.GarageName}.";

            var Nptification = new ClientNotification
            {
                
                Clientid = vehicle.ClientId ,
                Notes = Notes,
               
                IsRead = false
            };
            var response = await _ApiService.SaveClientNotificationsAsync(Nptification);
        }

        private async Task LoadPremuim()
        {
            await Shell.Current.GoToAsync($"{nameof(PremuimPage)}");
        }

        private async Task SearchVehicleAsync()
        {
            // Get current user ID from your authentication system
            if (LiscencePlate == string.Empty || LiscencePlate is null)
                return;

            var response = await _ApiService.GetVehicleByLiscenceID(LiscencePlate);
            if (response.IsSuccess)
            {
                Vehicles.Clear();
                Vehicles.Add(response.Data); // If response.Data is a single Vehicle
            }
        }

        private async Task EditProfile()
        {
            await Shell.Current.GoToAsync($"{nameof(EditGaragePage)}");
        }

        public async Task LoadUpComintAppointments(int GarageID)
        {
            var response = await _ApiService.GetUpcomingAppointments(GarageID);
            if (response.IsSuccess)
            {
                //UpcomingVehicleAppointments = response.Data;
                UpcomingVehicleAppointments = new ObservableCollection<VehicleAppointment>(response.Data);
                OnPropertyChanged(nameof(UpcomingVehicleAppointments));
            }
        }

        public async Task LoadGaragePremuim(int GarageID)
        {
            string ErrorMessage = string.Empty;
            GaragePremiumRegistration = await _ApiService.GetActiveRegistrationByGarageId(GarageID);
            if (GaragePremiumRegistration == null)
            {
                ErrorMessage = "No active registration found for this garage.";
            }
        }
        public async Task LoadPendingOrders(int GarageID)
        {
            string ErrorMessage = string.Empty;
            var response = await _ApiService.GetPendingPaymentOrderByID(GarageID);
            if (response != null && response.Data != null)
            {
                PendingOrders = new List<GaragePaymentOrder>(response.Data);
            }
        }

        public async Task LoadGarageProfile()
        {
            // Get current user ID from your authentication system
            int Garaged = GetCurrentUserId();

            var response = await _ApiService.GetGarageByID(Garaged);
            if (response.IsSuccess)
            {
                GarageProfile = response.Data;
                LoadUpComintAppointments(GarageProfile.Id);
                LoadGaragePremuim(GarageProfile.Id);
                LoadPendingOrders(GarageProfile.Id);
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
    }
}
