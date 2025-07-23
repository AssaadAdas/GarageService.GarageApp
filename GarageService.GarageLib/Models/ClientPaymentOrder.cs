﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.GarageLib.Models
{
    public partial class ClientPaymentOrder
    {
        public int Id { get; set; }

        public string OrderNumber { get; set; } = null!;

        public int ClientId { get; set; }

        public decimal Amount { get; set; }

        public int Currid { get; set; }

        public int PaymentMethodId { get; set; }

        public string? Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ProcessedDate { get; set; }

        public int PremiumOfferid { get; set; }

        public virtual ClientPaymentMethod Client { get; set; } = null!;

        public virtual Currency Curr { get; set; } = null!;

        public virtual PremiumOffer PremiumOffer { get; set; } = null!;
    }
}
