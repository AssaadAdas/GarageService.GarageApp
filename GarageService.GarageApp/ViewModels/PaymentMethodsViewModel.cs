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
    public class PaymentMethodsViewModel: BaseViewModel
    {
        private readonly ApiService _apiService;
        private readonly ISessionService _sessionService;
        public ICommand BackCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand LoadPaymentMethodCommand { get; }
        public ICommand AddPaymentMethodCommand { get; }
        public ICommand EditPaymentMethodCommand { get; }

        public PaymentMethodsViewModel(ApiService apiservice, ISessionService sessionService)
        {
            _apiService = apiservice;
            _sessionService = sessionService;
            LoadPaymentMethodCommand = new Command(async () => await LoadPaymentMethods());
            AddPaymentMethodCommand = new Command(async () => await AddPaymentMethod());
            EditPaymentMethodCommand = new Command<GaragePaymentMethod>(async (GaragePaymentMethod) => await EditPaymentMethod(GaragePaymentMethod));
            BackCommand = new Command(async () => await GoBack());
            LoadClientProfile();
        }
        private async Task GoBack()
        {
            await Shell.Current.GoToAsync($"..");
        }

        private async Task AddPaymentMethod()
        {
            await Shell.Current.GoToAsync(nameof(GaragePaymentMethodPage));
        }

        private async Task EditPaymentMethod(GaragePaymentMethod GaragePaymentMethod)
        {
            await Shell.Current.GoToAsync($"{nameof(EditPaymentMethodsPage)}?paymentMethodId={GaragePaymentMethod.Id}");
        }
        private List<GaragePaymentMethod> _garagePaymentMethod;
        public List<GaragePaymentMethod> GaragePaymentMethod
        {
            get => _garagePaymentMethod;
            set
            {
                if (_garagePaymentMethod != value)
                {
                    _garagePaymentMethod = value;
                    OnPropertyChanged(nameof(GaragePaymentMethod));
                }
            }
        }
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
        public async Task LoadPaymentMethods()
        {
            int GarageId = GetCurrentUserId();
            var response = await _apiService.GetPaymentMethodByID(GarageId);
            if (response.IsSuccess)
            {
                GaragePaymentMethod = response.Data;

            }
        }
        public async Task LoadClientProfile()
        {
            // Get current user ID from your authentication system
            int GarageId = GetCurrentUserId();

            var response = await _apiService.GetGarageByID(GarageId);
            if (response.IsSuccess)
            {
                GarageProfile = response.Data;
                LoadPaymentMethodCommand.Execute(null);
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
