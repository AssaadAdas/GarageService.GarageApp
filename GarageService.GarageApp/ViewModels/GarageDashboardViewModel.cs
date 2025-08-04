using GarageService.GarageApp.Views;
using GarageService.GarageLib.Models;
using GarageService.GarageLib.Services;
using System;
using System.Collections.Generic;
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
        public GarageDashboardViewModel(ApiService apiservice, ISessionService sessionService)
        {
            // Initialize properties and commands
            _ApiService = apiservice;
            _sessionService = sessionService;
            //_navigationService = navigationService;
            //OpenHistoryCommand = new Command(OpenHistory);
            //AddVehicleCommand = new Command(async () => await AddVehicle());
            //PremuimCommand = new Command(async () => await LoadPremuim());
            //LogOutCommand = new Command(async () => await LogOut());
            //EditVehicleCommand = new Command<Vehicle>(async (vehicle) => await EditVehicle(vehicle));
            //ShowPopUpCommand = new Command<Vehicle>(async (vehicle) => await ShowMenu(vehicle));
            //AddServicesCommand = new Command<Vehicle>(async (vehicle) => await AddServices(vehicle));
            //AddAppointmentCommand = new Command(AddAppointment);
            EditProfileCommand = new Command(async () => await EditProfile());
            //ReadNoteCommand = new Command<ClientNotification>(async (clientnotification) => await ReadNote(clientnotification));
            //// Load data here
            LoadGarageProfile();
        }
        private async Task EditProfile()
        {
            await Shell.Current.GoToAsync($"{nameof(EditGaragePage)}");
        }
        public async Task LoadGarageProfile()
        {
            // Get current user ID from your authentication system
            int Garaged = GetCurrentUserId();

            var response = await _ApiService.GetGarageByID(Garaged);
            if (response.IsSuccess)
            {
                GarageProfile = response.Data;
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
