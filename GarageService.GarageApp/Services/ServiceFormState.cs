﻿using GarageService.GarageApp.ViewModels;
using GarageService.GarageLib.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.GarageApp.Services
{
    public class ServiceFormState
    {
        public int VehicleId { get; set; }
        public int Odometer { get; set; }
        public int GarageId { get; set; }
        public string Notes { get; set; }
        public DateTime ServiceDate { get; set; }
        public ObservableCollection<SelectableServiceTypeViewModel> ServiceTypes { get; set; }
        public ObservableCollection<SelectableServiceTypeViewModel> SelectedServiceTypes { get; set; }
        public GarageProfile selectedGarage { get; set; } = null!;
    }
}
