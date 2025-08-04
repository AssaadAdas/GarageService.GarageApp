﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.GarageLib.Models
{
    public partial class GaragePaymentMethod
    {
        public int Id { get; set; }

        public int Garageid { get; set; }

        public string PaymentType { get; set; } = null!;

        public bool IsPrimary { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModified { get; set; }

        public bool IsActive { get; set; }

        public string CardNumber { get; set; } = null!;

        public string CardHolderName { get; set; } = null!;

        public int ExpiryMonth { get; set; }

        public int ExpiryYear { get; set; }

        public string Cvv { get; set; } = null!;

        public virtual GarageProfile? Garage { get; set; } = null!;
    }
}
