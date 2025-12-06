using GarageService.GarageApp.Views;
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
    [QueryProperty(nameof(VehicleId), "vehicleid")]
    public class LastServiceTypeViewModel : BaseViewModel
    {
        private readonly ApiService _ApiService;
        private readonly ISessionService _sessionService;
        public ICommand LoadLastServiceCommand { get; }
        public ICommand LoadServiceTypesCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand SaveCommand { get; }
        private int _vehicleId;
        public int VehicleId
        {
            get => _vehicleId;
            set
            {
                _vehicleId = value;
                OnPropertyChanged(nameof(VehicleId));
            }
        }

        private int _serviceTypeId;
        public int ServiceTypeId
        {
            get => _serviceTypeId;
            set
            {
                SetProperty(ref _serviceTypeId, value);
                // If you need to find the full country object:
                SelectedServiceType = ServiceTypes?.FirstOrDefault(c => c.Id == value);
            }
        }
        private List<ServiceType> _ServiceTypes;
        private ServiceType _selectedServiceType;
        public List<ServiceType> ServiceTypes
        {
            get => _ServiceTypes;
            set => SetProperty(ref _ServiceTypes, value);
        }

        public ServiceType SelectedServiceType
        {
            get => _selectedServiceType;
            set
            {
                if (SetProperty(ref _selectedServiceType, value))
                {
                    ServiceTypeId = value?.Id ?? 0;
                    LoadLastServiceCommand.Execute(null);
                }
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
        public LastServiceTypeViewModel(ApiService apiservice, ISessionService sessionService)
        {
            _ApiService = apiservice;
            _sessionService = sessionService;
            LoadLastServiceCommand = new Command(async () => await LoadLastService());
            LoadServiceTypesCommand = new Command(async () => await LoadServiceTypes());
            SaveCommand = new Command(async () => await GoBack());
            BackCommand = new Command(async () => await GoBack());
            LoadServiceTypesCommand.Execute(null);
        }
        private VehiclesService _VehiclesService;
        public VehiclesService VehiclesService
        {
            get => _VehiclesService;
            set => SetProperty(ref _VehiclesService, value);
        }

        private async Task GoBack()
        {
            await Shell.Current.GoToAsync($"{nameof(GarageDashboardPage)}");
        }

        private async Task LoadLastService()
        {
            try
            {
                IsBusy = true;
                VehiclesService = null;

                var response = await _ApiService.GetVehicleLastServiceByType(VehicleId, ServiceTypeId);
                if (response.IsSuccess)
                {
                    VehiclesService = response.Data;
                }
                IsBusy = false;
            }
            catch (Exception ex)
            {
                // Handle error (show alert, etc.)
                await Shell.Current.DisplayAlert("Error", $"Failed to load Last service: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LoadServiceTypes()
        {
            try
            {
                ErrorMessage = string.Empty;
                var apiResponse = await _ApiService.GetServiceTypesAsync();

                if (apiResponse.IsSuccess)
                {
                    ServiceTypes = apiResponse.Data;
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
    }
}
