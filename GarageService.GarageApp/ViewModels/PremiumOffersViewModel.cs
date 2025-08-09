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
   
    public class PremiumOffersViewModel: BaseViewModel
    {
        private readonly ApiService _ApiService;
        private readonly ISessionService _sessionService;
        public ICommand LoadPremiumOffersCommand { get; }
        public ICommand LoadProfileCommand { get; }
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

        //private List<PremiumOffer> _premiumoffers;
        //public List<PremiumOffer> PremiumOffers
        //{
        //    get => _premiumoffers;
        //    set
        //    {
        //        if (_premiumoffers != value)
        //        {
        //            _premiumoffers = value;
        //            OnPropertyChanged(nameof(PremiumOffer));
        //        }
        //    }
        //}
        private ObservableCollection<PremiumOffer> _premiumOffers;
        public ObservableCollection<PremiumOffer> PremiumOffers
        {
            get => _premiumOffers;
            set => SetProperty(ref _premiumOffers, value);
        }
        public PremiumOffersViewModel(ApiService apiservice, ISessionService sessionService)
        {
            _ApiService = apiservice;
            _sessionService = sessionService;
            
            BackCommand = new Command(async () => await GoBack());
            LoadProfileCommand = new Command(async () => await LoadProfile());
            LoadProfileCommand.Execute(null);
            LoadPremiumOffersCommand = new Command(async () => await LoadPremiumOffers());
            
        }
        private async Task GoBack()
        {
            await Shell.Current.GoToAsync($"..");
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        private async Task LoadPremiumOffers()
        {
            // Get current user ID from your authentication system
            var response = await _ApiService.GetOffersByUserType(GarageProfile.User.UserTypeid);
            if (response.IsSuccess)
            {

                PremiumOffers = new ObservableCollection<PremiumOffer>(response.Data);
            }
            else
            {
                // Handle the error, e.g., show a message to the user
                ErrorMessage = response.ErrorMessage;
            }
        }
        private async Task LoadProfile()
        {
            // Get current user ID from your authentication system
            int GarageId = GetCurrentUserId();

            var response = await _ApiService.GetGarageByID(GarageId);
            if (response.IsSuccess)
            {
                GarageProfile = response.Data;
                LoadPremiumOffersCommand.Execute(null);
            }
            else
            {
                // Handle the error, e.g., show a message to the user
                ErrorMessage = response.ErrorMessage;
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
