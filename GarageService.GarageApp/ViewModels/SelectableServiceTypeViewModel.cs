﻿using GarageService.GarageLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.GarageApp.ViewModels
{
    public class SelectableServiceTypeViewModel : BaseViewModel
    {
        public ServiceType ServiceType { get; }
        public int Id => ServiceType.Id;
        public string Description => ServiceType.Description; // or whatever property you use for display

        private decimal _cost;
        public decimal Cost
        {
            get => _cost;
            set
            {
                if (_cost != value)
                {
                    _cost = value;
                    OnPropertyChanged(nameof(Cost));
                }
            }
        }

        private Currency _selectedCurrency;
        public Currency SelectedCurrency
        {
            get => _selectedCurrency;
            set
            {
                SetProperty(ref _selectedCurrency, value);
                CurrId = value?.Id ?? 0;
                CurrDesc = value?.CurrDesc ?? string.Empty;
            }
        }

        private int _currId;
        public int CurrId
        {
            get => _currId;
            set => SetProperty(ref _currId, value);
        }

        private string _currDesc;
        public string CurrDesc
        {
            get => _currDesc;
            set => SetProperty(ref _currDesc, value);
        }
        private string _notes;
        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        public bool IsSelected { get; set; }
        public SelectableServiceTypeViewModel(ServiceType serviceType)
        {
            ServiceType = serviceType;
        }
    }

}
