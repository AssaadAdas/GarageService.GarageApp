using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.GarageLib.Models
{
    public partial class VehicleCheck
    {
        public int Id { get; set; }

        public int? Vehicleid { get; set; }

        public string? CheckStatus { get; set; }
        public DateTime? CheckDate { get; set; }
        public int? GarageId { get; set; }
        public virtual Vehicle? Vehicle { get; set; }
        public virtual GarageProfile? GarageProfile { get; set; }
    }
}
