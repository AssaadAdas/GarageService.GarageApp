using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.GarageLib.Models
{

    public partial class PaymentType
    {
        public int Id { get; set; }

        public string? PaymentTypeDesc { get; set; }

        public virtual ICollection<GaragePaymentMethod> GaragePaymentMethods { get; set; } = new List<GaragePaymentMethod>();
    }
}
