﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.GarageLib.Models
{
    public partial class FuelType
    {
        public int Id { get; set; }

        public string FuelTypeDesc { get; set; } = null!;

        public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}
