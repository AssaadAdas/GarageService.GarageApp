﻿using GarageService.GarageLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.GarageLib.Services
{
    public class SessionService : ISessionService
    {
        private const string UserIdKey = "user_id";
        private const string UserTypeidKey = "user_type_id";
        private const string ProfileIdKey = "profile_id";
        private const string UsernameKey = "username";
        private const string IsPremuimKey = "IsPremuimKey";

        public bool IsLoggedIn => Preferences.ContainsKey(ProfileIdKey);
        public int UserId => Preferences.Get(UserIdKey, -1);
        public int UserTypeid => Preferences.Get(UserTypeidKey, 1);
        public int ProfileId => Preferences.Get(ProfileIdKey, -1);
        public string Username => Preferences.Get(UsernameKey, string.Empty);

        public bool IsPremuim => Preferences.Get(IsPremuimKey, false);

        int ISessionService.UserType => Preferences.Get(UserTypeidKey, 1);

        public void ClearSession()
        {
            Preferences.Remove(UserIdKey);
            Preferences.Remove(UserTypeidKey);
            Preferences.Remove(ProfileIdKey);
            Preferences.Remove(UsernameKey);
        }

        public void CreateSession(User user, GarageProfile garageProfile = null)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            Preferences.Set(UserIdKey, user.Id);
            Preferences.Set(UserTypeidKey, user.UserTypeid);
            Preferences.Set(UsernameKey, user.Username);


            if (user.UserTypeid == 1 && garageProfile != null)
            {
                Preferences.Set(ProfileIdKey, garageProfile.Id);
                Preferences.Set(IsPremuimKey, garageProfile.IsPremium);
            }

        }
    }
}
