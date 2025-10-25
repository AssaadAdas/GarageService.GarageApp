
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
    [QueryProperty(nameof(PaymentOrderid), "paymentOrderid")]
    public class GaragePaymentOrdersViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private readonly ISessionService _sessionService;
        public ICommand LoadPaymentMethodCommand { get; }
        public ICommand AddPaymentMethodCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand SaveCommand { get; }
        public bool IsSelected = false;

        public GaragePaymentOrdersViewModel(ApiService apiservice, ISessionService sessionService)
        {
            _apiService = apiservice;
            _sessionService = sessionService;
            LoadPaymentMethodCommand = new Command(async () => await LoadPaymentMethods());
            BackCommand = new Command(async () => await GoBack());
            SaveCommand = new Command(async () => await SavePaymentOrder());
            AddPaymentMethodCommand = new Command(async () => await AddPaymentMethodAsync());
            AddPaymentMethodCommand = new Command(async () => await AddPaymentMethod());
            LoadGarageProfile();
        }
        private async Task AddPaymentMethod()
        {
            await Shell.Current.GoToAsync(nameof(GaragePaymentMethodPage));
        }
        private GaragePaymentMethod _selectedPaymentMethod;
        public GaragePaymentMethod SelectedPaymentMethod
        {
            get => _selectedPaymentMethod;
            set => SetProperty(ref _selectedPaymentMethod, value);
        }

        private int _PaymentOrderid;
        public int PaymentOrderid
        {
            get => _PaymentOrderid;
            set
            {
                _PaymentOrderid = value;
                LoadPremuium();
            }
        }

        private async Task GoBack()
        {
            await Shell.Current.GoToAsync($"..");
        }

        private PremiumOffer _premiumOffer;
        public PremiumOffer PremiumOffer
        {
            get => _premiumOffer;
            set
            {
                if (_premiumOffer != value)
                {
                    _premiumOffer = value;
                    OnPropertyChanged(nameof(PremiumOffer));
                }
            }
        }

        private GaragePaymentOrder _GaragePaymentOrder;
        public GaragePaymentOrder GaragePaymentOrder
        {
            get => _GaragePaymentOrder;
            set
            {
                if (_GaragePaymentOrder != value)
                {
                    _GaragePaymentOrder = value;
                    OnPropertyChanged(nameof(GaragePaymentOrder));
                }
            }
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
        public async Task AddPaymentMethodAsync()
        {

        }
        public async Task SavePaymentOrder()
        {
            if (SelectedPaymentMethod == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please select a payment method.", "OK");
                return;
            }
            int GarageID  = GetCurrentUserId();
            string OrderStatus = "Pending";
            string OrderNumber = GarageID.ToString()+"-" +DateTime.Now.Ticks.ToString();

            var garagePaymentOrder = new GaragePaymentOrder
            {
                OrderNumber = OrderNumber,
                GarageId = GarageID,
                Amount = PremiumOffer.PremiumCost,
                Currid = PremiumOffer.CurrId, 
                PaymentMethodId = SelectedPaymentMethod.Id,
                Status = OrderStatus,
                CreatedDate = DateTime.Now,
                ProcessedDate= DateTime.Now,
                PremiumOfferid = PremiumOffer.Id
            };

            var response = await _apiService.AddGaragePaymentOrder(garagePaymentOrder);
            if (response.IsSuccess)
            {
                GaragePaymentOrder = response.Data;
                await Shell.Current.GoToAsync($"{nameof(PaymentPage)}?paymentOrderid={GaragePaymentOrder.Id}");
            }

        }
        public async Task LoadPremuium()
        {
            var response = await _apiService.GetPremiumByID(PaymentOrderid);
            if (response.IsSuccess)
            {
                PremiumOffer = response.Data;
            }
        }

        public async Task LoadPaymentMethods()
        {

            int GarageID = GetCurrentUserId();
            var response = await _apiService.GetPaymentMethodByID(GarageID);
            if (response.IsSuccess)
            {
                GaragePaymentMethod = response.Data;
            }
        }

        public async Task LoadGarageProfile()
        {
            // Get current user ID from your authentication system
            int Garaged = GetCurrentUserId();

            var response = await _apiService.GetGarageByID(Garaged);
            if (response.IsSuccess)
            {
                GarageProfile = response.Data;
                LoadPaymentMethods();
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
