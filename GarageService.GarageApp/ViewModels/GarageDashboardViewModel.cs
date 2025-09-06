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
        public ICommand CheckVehicleCommand { get; }
        public ICommand PremuimCommand { get; }
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
        //public List<VehicleAppointment> UpcomingVehicleAppointments
        //{
        //    get => _UpcomingvehicleAppointments;
        //    set
        //    {
        //        if (_UpcomingvehicleAppointments != value)
        //        {
        //            _UpcomingvehicleAppointments = value;
        //            OnPropertyChanged(nameof(VehicleAppointment));
        //        }
        //    }
        //}
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
        private bool _isChecked =  false;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                }
            }
        }

        private string _liscencePlate;
        public string LiscencePlate
        {
            get => _liscencePlate;
            set => SetProperty(ref _liscencePlate, value);
        }
        public GarageDashboardViewModel(ApiService apiservice, ISessionService sessionService)
        {
            // Initialize properties and commands
            _ApiService = apiservice;
            _sessionService = sessionService;
            PremuimCommand = new Command(async () => await LoadPremuim());
            //LogOutCommand = new Command(async () => await LogOut());
            AddVehicleCommand = new Command(async () => await AddVehicle());
            AddServicesCommand = new Command<Vehicle>(async (vehicle) => await AddServices(vehicle));
            CheckVehicleCommand = new Command<Vehicle>(async (vehicle) => await CheckVehicle(vehicle));
            SearchVehicleCommand = new Command(async () => await SearchVehicleAsync());
            EditProfileCommand = new Command(async () => await EditProfile());
            //btnServices.enabled = false;
            LoadGarageProfile();
        }
        private async Task AddServices(Vehicle vehicle)
        {
            await Shell.Current.GoToAsync($"{nameof(ServicePage)}?vehileid={vehicle.Id}");
        }
        private async Task AddVehicle()
        {
            await Shell.Current.GoToAsync($"{nameof(RegisterClientVehiclePage)}");
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
                IsChecked = true;
                //btnServices.enabled = true;
                //btnChecks.enabled = false;
                WriteClientNotification(vehicle);
            }
            else
            {
                //btnServices.enabled = false;
                //btnChecks.enabled = true;
                IsChecked = false;
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

        //private async Task AddClientVehicle()
        //{
        //    await Shell.Current.GoToAsync($"{nameof(RegisterClientVehiclePage)}");
        //}

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
        public async Task LoadGarageProfile()
        {
            // Get current user ID from your authentication system
            int Garaged = GetCurrentUserId();

            var response = await _ApiService.GetGarageByID(Garaged);
            if (response.IsSuccess)
            {
                GarageProfile = response.Data;
                LoadUpComintAppointments(GarageProfile.Id);
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
