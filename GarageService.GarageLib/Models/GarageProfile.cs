﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.GarageLib.Models
{
    public partial class GarageProfile
    {
        public int Id { get; set; }

        public string GarageName { get; set; } = null!;

        public string Address { get; set; } = null!;

        public int CountryId { get; set; }

        public string PhoneExt { get; set; } = null!;

        public int PhoneNumber { get; set; }

        public string? Email { get; set; }

        public int SpecializationId { get; set; }

        public int UserId { get; set; }

        public bool IsPremium { get; set; }

        public virtual Country? Country { get; set; } = null!;

        public virtual ICollection<GaragePaymentMethod>? GaragePaymentMethods { get; set; } = new List<GaragePaymentMethod>();

        public virtual ICollection<GaragePaymentOrder>? GaragePaymentOrders { get; set; } = new List<GaragePaymentOrder>();

        public virtual ICollection<GaragePremiumRegistration>? GaragePremiumRegistrations { get; set; } = new List<GaragePremiumRegistration>();

        public virtual Specialization? Specialization { get; set; } = null!;

        public virtual User? User { get; set; } = null!;

        public virtual ICollection<VehiclesService>? VehiclesServices { get; set; } = new List<VehiclesService>();

        public virtual ICollection<VehicleCheck>? VehicleChecks { get; set; } = new List<VehicleCheck>();
        public virtual ICollection<VehicleAppointment>? VehicleAppointments { get; set; } = new List<VehicleAppointment>();
    }
}
