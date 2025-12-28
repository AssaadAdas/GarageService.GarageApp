using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.GarageLib.Models
{
    public partial class Country
    {
        public int Id { get; set; }

        public string CountryName { get; set; }

        public string? PhoneExt { get; set; }

        public byte[]? CountryFlag { get; set; }

        /// <summary>
        /// Gets the display name with flag emoji for the country
        /// </summary>
        public string DisplayName => $"{FlagEmoji} {CountryName}";

        /// <summary>
        /// Gets the flag emoji for the country based on country name
        /// </summary>
        public string FlagEmoji
        {
            get
            {
                // If CountryFlag contains the emoji bytes or country code
                if (CountryFlag != null && CountryFlag.Length > 0)
                {
                    try
                    {
                        var flagStr = Encoding.UTF8.GetString(CountryFlag);
                        if (!string.IsNullOrWhiteSpace(flagStr))
                        {
                            // If it's a 2-character country code, convert to emoji
                            if (flagStr.Length == 2 && flagStr.All(char.IsLetter))
                            {
                                return string.Concat(flagStr.ToUpper().Select(x => char.ConvertFromUtf32(x + 0x1F1A5)));
                            }
                            return flagStr;
                        }
                    }
                    catch { }
                }

                if (string.IsNullOrEmpty(CountryName))
                    return "🌍";

                // Map common country names to flag emojis
                var countryFlags = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "United States", "🇺🇸" },
                    { "USA", "🇺🇸" },
                    { "United Kingdom", "🇬🇧" },
                    { "UK", "🇬🇧" },
                    { "Canada", "🇨🇦" },
                    { "Australia", "🇦🇺" },
                    { "Germany", "🇩🇪" },
                    { "France", "🇫🇷" },
                    { "Italy", "🇮🇹" },
                    { "Spain", "🇪🇸" },
                    { "Netherlands", "🇳🇱" },
                    { "Belgium", "🇧🇪" },
                    { "Switzerland", "🇨🇭" },
                    { "Austria", "🇦🇹" },
                    { "Sweden", "🇸🇪" },
                    { "Norway", "🇳🇴" },
                    { "Denmark", "🇩🇰" },
                    { "Finland", "🇫🇮" },
                    { "Poland", "🇵🇱" },
                    { "Portugal", "🇵🇹" },
                    { "Greece", "🇬🇷" },
                    { "Ireland", "🇮🇪" },
                    { "Japan", "🇯🇵" },
                    { "China", "🇨🇳" },
                    { "India", "🇮🇳" },
                    { "Brazil", "🇧🇷" },
                    { "Mexico", "🇲🇽" },
                    { "Argentina", "🇦🇷" },
                    { "Chile", "🇨🇱" },
                    { "South Africa", "🇿🇦" },
                    { "Egypt", "🇪🇬" },
                    { "Saudi Arabia", "🇸🇦" },
                    { "United Arab Emirates", "🇦🇪" },
                    { "UAE", "🇦🇪" },
                    { "Turkey", "🇹🇷" },
                    { "Russia", "🇷🇺" },
                    { "South Korea", "🇰🇷" },
                    { "Singapore", "🇸🇬" },
                    { "Malaysia", "🇲🇾" },
                    { "Thailand", "🇹🇭" },
                    { "Indonesia", "🇮🇩" },
                    { "Philippines", "🇵🇭" },
                    { "Vietnam", "🇻🇳" },
                    { "New Zealand", "🇳🇿" },
                    { "Israel", "🇮🇱" },
                    { "Lebanon", "🇱🇧" },
                    { "Jordan", "🇯🇴" },
                    { "Kuwait", "🇰🇼" },
                    { "Qatar", "🇶🇦" },
                    { "Bahrain", "🇧🇭" },
                    { "Oman", "🇴🇲" },
                    { "Iraq", "🇮🇶" },
                    { "Syria", "🇸🇾" },
                    { "Yemen", "🇾🇪" },
                    { "Pakistan", "🇵🇰" },
                    { "Bangladesh", "🇧🇩" },
                    { "Sri Lanka", "🇱🇰" },
                    { "Nepal", "🇳🇵" },
                    { "Afghanistan", "🇦🇫" },
                    { "Iran", "🇮🇷" },
                    { "Kazakhstan", "🇰🇿" },
                    { "Ukraine", "🇺🇦" },
                    { "Romania", "🇷🇴" },
                    { "Bulgaria", "🇧🇬" },
                    { "Hungary", "🇭🇺" },
                    { "Czech Republic", "🇨🇿" },
                    { "Slovakia", "🇸🇰" },
                    { "Croatia", "🇭🇷" },
                    { "Serbia", "🇷🇸" },
                    { "Slovenia", "🇸🇮" },
                    { "Bosnia and Herzegovina", "🇧🇦" },
                    { "Macedonia", "🇲🇰" },
                    { "Albania", "🇦🇱" },
                    { "Montenegro", "🇲🇪" },
                    { "Kosovo", "🇽🇰" },
                    { "Moldova", "🇲🇩" },
                    { "Belarus", "🇧🇾" },
                    { "Lithuania", "🇱🇹" },
                    { "Latvia", "🇱🇻" },
                    { "Estonia", "🇪🇪" },
                    { "Iceland", "🇮🇸" },
                    { "Luxembourg", "🇱🇺" },
                    { "Malta", "🇲🇹" },
                    { "Cyprus", "🇨🇾" },
                    { "Monaco", "🇲🇨" },
                    { "Liechtenstein", "🇱🇮" },
                    { "San Marino", "🇸🇲" },
                    { "Vatican City", "🇻🇦" },
                    { "Andorra", "🇦🇩" },
                    { "Morocco", "🇲🇦" },
                    { "Algeria", "🇩🇿" },
                    { "Tunisia", "🇹🇳" },
                    { "Libya", "🇱🇾" },
                    { "Sudan", "🇸🇩" },
                    { "Ethiopia", "🇪🇹" },
                    { "Kenya", "🇰🇪" },
                    { "Ghana", "🇬🇭" },
                    { "Nigeria", "🇳🇬" },
                    { "Senegal", "🇸🇳" },
                    { "Ivory Coast", "🇨🇮" },
                    { "Cameroon", "🇨🇲" },
                    { "Angola", "🇦🇴" },
                    { "Mozambique", "🇲🇿" },
                    { "Madagascar", "🇲🇬" },
                    { "Tanzania", "🇹🇿" },
                    { "Uganda", "🇺🇬" },
                    { "Rwanda", "🇷🇼" },
                    { "Zimbabwe", "🇿🇼" },
                    { "Botswana", "🇧🇼" },
                    { "Namibia", "🇳🇦" },
                    { "Zambia", "🇿🇲" },
                    { "Malawi", "🇲🇼" },
                    { "Mauritius", "🇲🇺" },
                    { "Seychelles", "🇸🇨" },
                    { "Djibouti", "🇩🇯" },
                    { "Eritrea", "🇪🇷" },
                    { "Somalia", "🇸🇴" },
                    { "Chad", "🇹🇩" },
                    { "Niger", "🇳🇪" },
                    { "Mali", "🇲🇱" },
                    { "Burkina Faso", "🇧🇫" },
                    { "Guinea", "🇬🇳" },
                    { "Sierra Leone", "🇸🇱" },
                    { "Liberia", "🇱🇷" },
                    { "Togo", "🇹🇬" },
                    { "Benin", "🇧🇯" },
                    { "Gabon", "🇬🇦" },
                    { "Equatorial Guinea", "🇬🇶" },
                    { "Republic of the Congo", "🇨🇬" },
                    { "Democratic Republic of the Congo", "🇨🇩" },
                    { "Central African Republic", "🇨🇫" },
                    { "Burundi", "🇧🇮" },
                    { "Lesotho", "🇱🇸" },
                    { "Eswatini", "🇸🇿" },
                    { "Comoros", "🇰🇲" },
                    { "Cape Verde", "🇨🇻" },
                    { "São Tomé and Príncipe", "🇸🇹" },
                    { "Guinea-Bissau", "🇬🇼" },
                    { "Gambia", "🇬🇲" },
                    { "Mauritania", "🇲🇷" },
                    { "Western Sahara", "🇪🇭" },
                };

                if (countryFlags.TryGetValue(CountryName, out var flag))
                    return flag;

                // Try to find partial match
                foreach (var kvp in countryFlags)
                {
                    if (CountryName.Contains(kvp.Key, StringComparison.OrdinalIgnoreCase) ||
                        kvp.Key.Contains(CountryName, StringComparison.OrdinalIgnoreCase))
                    {
                        return kvp.Value;
                    }
                }

                return "🌍"; // Default globe emoji
            }
        }

        public virtual ICollection<ClientProfile> ClientProfiles { get; set; } = new List<ClientProfile>();

        public virtual ICollection<GarageProfile> GarageProfiles { get; set; } = new List<GarageProfile>();
    }
}
