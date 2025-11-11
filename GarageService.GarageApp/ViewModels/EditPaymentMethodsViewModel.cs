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
    [QueryProperty(nameof(PaymentMethodId), "paymentMethodId")]
    public class EditPaymentMethodsViewModel: BaseViewModel
    {
        private readonly ApiService _ApiService;
        private readonly ISessionService _sessionService;

        public ICommand BackCommand { get; }
        public ICommand LoadPaymentMethodCommand { get; }
        public ICommand LoadClientCommand { get; }
        public ICommand SaveCommand { get; }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        private bool _isPrimary;
        public bool IsPrimary
        {
            get => _isPrimary;
            set => SetProperty(ref _isPrimary, value);
        }

        private string _cardNumber = string.Empty;
        public string CardNumber
        {
            get => _cardNumber;
            set => SetProperty(ref _cardNumber, value);
        }

        private string _cardHolderName = string.Empty;
        public string CardHolderName
        {
            get => _cardHolderName;
            set => SetProperty(ref _cardHolderName, value);
        }

        private int _expiryMonth;
        public int ExpiryMonth
        {
            get => _expiryMonth;
            set => SetProperty(ref _expiryMonth, value);
        }

        private int _expiryYear;
        public int ExpiryYear
        {
            get => _expiryYear;
            set => SetProperty(ref _expiryYear, value);
        }

        private string _cvv = string.Empty;
        public string Cvv
        {
            get => _cvv;
            set => SetProperty(ref _cvv, value);
        }

        private GarageProfile _GarageProfile;
        public GarageProfile GarageProfile
        {
            get => _GarageProfile;
            set
            {
                if (_GarageProfile != value)
                {
                    _GarageProfile = value;
                    OnPropertyChanged(nameof(GarageProfile));
                }
            }
        }

        private int _PaymentMethodId;
        public int PaymentMethodId
        {
            get => _PaymentMethodId;
            set
            {
                if (SetProperty(ref _PaymentMethodId, value))
                {
                    // fire-and-forget load; avoids blocking setter
                    _ = LoadPaymentMethodAsync();
                }
            }
        }

        private GaragePaymentMethod _GaragePaymentMethod = new();
        public GaragePaymentMethod GaragePaymentMethod
        {
            get => _GaragePaymentMethod;
            set
            {
                if (_GaragePaymentMethod != value)
                {
                    _GaragePaymentMethod = value;
                    OnPropertyChanged(nameof(GaragePaymentMethod));
                }
            }
        }


        public ObservableCollection<int> Months { get; } = new ObservableCollection<int>();

        // Years collection as ints (matches ExpiryYear type)
        public ObservableCollection<int> Years { get; } = new ObservableCollection<int>();

        private void InitializeMonths()
        {
            // Add months as two-digit strings
            for (int i = 1; i <= 12; i++)
            {
                Months.Add(i); // "01", "02", ..., "12"
            }
        }

        private void InitializeYears()
        {
            int currentYear = DateTime.Now.Year;

            // Add current year and next 10 years
            for (int i = 0; i <= 10; i++)
            {
                Years.Add((currentYear + i));
            }
        }
        public EditPaymentMethodsViewModel(ApiService apiService, ISessionService sessionService)
        {
            _ApiService = apiService;
            _sessionService = sessionService;
            SaveCommand = new Command(async () => await SavePaymentMethod());
            BackCommand = new Command(async () => await GoBack());
            LoadClientCommand = new Command(async () => await LoadGarageProfile());
            LoadPaymentMethodCommand = new Command(async () => await LoadPaymentMethodAsync());
            LoadClientCommand.Execute(null);
            InitializeMonths();
            InitializeYears();

        }
        private async Task GoBack()
        {
            await Shell.Current.GoToAsync($"{nameof(GarageDashboardPage)}");
        }
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public async Task LoadPaymentMethodAsync()
        {
            var response = await _ApiService.GetPaymentMethodID(PaymentMethodId);
            GaragePaymentMethod = response.Data;
            if (GaragePaymentMethod != null)
            {
                IsActive = GaragePaymentMethod.IsActive;

                // assign using SetProperty-backed properties so UI updates
                CardNumber = GaragePaymentMethod.CardNumber ?? string.Empty;
                CardHolderName = GaragePaymentMethod.CardHolderName ?? string.Empty;

                // ensure month/year exist in collections then set
                ExpiryMonth = GaragePaymentMethod.ExpiryMonth;
                if (!Months.Contains(ExpiryMonth) && ExpiryMonth >= 1 && ExpiryMonth <= 12)
                    Months.Add(ExpiryMonth);

                ExpiryYear = GaragePaymentMethod.ExpiryYear;
                if (!Years.Contains(ExpiryYear) && ExpiryYear > 0)
                    Years.Add(ExpiryYear);
                IsActive = GaragePaymentMethod.IsActive;
                IsPrimary = GaragePaymentMethod.IsPrimary;
                Cvv = GaragePaymentMethod.Cvv ?? string.Empty;
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

        public async Task SavePaymentMethod()
        {
            if (string.IsNullOrWhiteSpace(CardHolderName))
            {
                await Shell.Current.DisplayAlert("Error", "CardHolderName is required fields", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(Cvv))
            {
                await Shell.Current.DisplayAlert("Error", "Cvv is required fields", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(CardNumber))
            {
                await Shell.Current.DisplayAlert("Error", "CardNumber is required fields", "OK");
                return;
            }

            if (ExpiryMonth == 0)
            {
                await Shell.Current.DisplayAlert("Error", "ExpiryMonth is required fields", "OK");
                return;
            }

            if (ExpiryYear == 0)
            {
                await Shell.Current.DisplayAlert("Error", "ExpiryYear is required fields", "OK");
                return;
            }
            GaragePaymentMethod.Garageid = GarageProfile.Id;
            GaragePaymentMethod.LastModified = DateTime.Now;
            GaragePaymentMethod.CardNumber = CardNumber;
            GaragePaymentMethod.CardHolderName = CardHolderName;
            GaragePaymentMethod.ExpiryMonth = ExpiryMonth;
            GaragePaymentMethod.ExpiryYear = ExpiryYear;
            GaragePaymentMethod.IsPrimary = GaragePaymentMethod.IsPrimary;
            GaragePaymentMethod.IsActive = IsActive;
            GaragePaymentMethod.IsPrimary = IsPrimary;
            GaragePaymentMethod.PaymentTypeId = GaragePaymentMethod.PaymentTypeId;
            GaragePaymentMethod.Cvv = Cvv;
            GaragePaymentMethod.Garage = GarageProfile;

            bool success = await _ApiService.UpdatePaymentMethodAsync(GaragePaymentMethod.Id, GaragePaymentMethod);

            if (success)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Payment method updated.", "OK");
                await Shell.Current.GoToAsync("..");
            }

            else
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to update payment method.", "OK");

        }
    }
}
