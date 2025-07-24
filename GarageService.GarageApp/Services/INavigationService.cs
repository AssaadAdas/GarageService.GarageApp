﻿using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.GarageApp.Services
{
    public interface INavigationService
    {
        Task ShowPopupAsync(Popup popup);
    }
}
