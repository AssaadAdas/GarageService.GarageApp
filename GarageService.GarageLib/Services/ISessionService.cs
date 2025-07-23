﻿using GarageService.GarageLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.GarageLib.Services
{
    public interface ISessionService
    {
        bool IsLoggedIn { get; }
        int UserId { get; }
        int UserType { get; }
        int ProfileId { get; } // ClientId or GarageId
        string Username { get; }

        bool IsPremuim { get; }
        void CreateSession(User user, GarageProfile garageProfile = null);
        void ClearSession();
    }
}
