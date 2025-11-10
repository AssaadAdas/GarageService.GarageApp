
using GarageService.GarageApp.Views;
using GarageService.GarageLib.Models;
using GarageService.GarageLib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GarageService.GarageApp.ViewModels
{
    [QueryProperty(nameof(PaymentOrderid), "paymentOrderid")]
    public class PaymentViewModel: BaseViewModel
    {
        private readonly ApiService _apiService;
        private readonly ISessionService _sessionService;
        public ICommand LoadPaymentOrderCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand SaveCommand { get; }

        public PaymentViewModel(ApiService apiservice, ISessionService sessionService)
        {
            _apiService = apiservice;
            _sessionService = sessionService;
            LoadPaymentOrderCommand = new Command(async () => await LoadPaymentOrder());
            SaveCommand = new Command(async () => await Save());
            
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

        private GaragePremiumRegistration _GaragePremiumRegistration;
        public GaragePremiumRegistration GaragePremiumRegistrations
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

        private int _PaymentOrderid;
        public int PaymentOrderid
        {
            get => _PaymentOrderid;
            set
            {
                _PaymentOrderid = value;
                LoadPaymentOrderCommand.Execute(null);
            }
        }
        private Decimal _Totalamount;
        public Decimal Totalamount
        {
            get => _Totalamount;
            set
            {
                _Totalamount = value;
                OnPropertyChanged(nameof(Totalamount));
            }
        }
        public async Task Save()
        {
            string OrderStatus = "Processed";
            // Update garage Profiles
            GaragePaymentOrder.Status = OrderStatus;
           
            bool success = await _apiService.UpdateOrderStatusAsync(GaragePaymentOrder.Id, OrderStatus);

            if (success)
            {
                var GaragePremium = new GaragePremiumRegistration
                {
                    Garageid = GaragePaymentOrder.GarageId,
                    Registerdate = DateTime.Now,
                    ExpiryDate = DateTime.Now.AddDays(GaragePaymentOrder.PremiumOffer.PremiumRange), // Assuming 1 year premium
                    IsActive = true
                };

                var response = await _apiService.AddGaragePremium(GaragePremium);
                if (response.IsSuccess)
                {
                    GaragePremiumRegistrations = response.Data;
                    var Result = await _apiService.UpdateGaragePremiumStatusAsync(GaragePaymentOrder.GarageId,true);
                    if (Result)
                    {
                        await Shell.Current.DisplayAlert("Success", "Order Payed successfully", "OK");
                        await Shell.Current.GoToAsync($"{nameof(GarageDashboardPage)}"); // This pops the Edit page and returns to the dashboard
                    }
                    
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to register Premium", "OK");
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Failed to Pay Order", "OK");
            }
        }
        public async Task LoadPaymentOrder()
        {
            // Get current user ID from your authentication system
            var response = await _apiService.GetPaymentOrderByID(PaymentOrderid);
            if (response.IsSuccess)
            {
                GaragePaymentOrder = response.Data;
                Totalamount = GaragePaymentOrder.Amount;

            }
        }
    }
}