using GarageService.GarageLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GarageService.GarageLib.Services
{
    public interface IApiService
    {
       
    }
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:44344/api/";
        public ApiService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(BaseUrl)
            };
            _httpClient.Timeout = TimeSpan.FromSeconds(120);

            // Add any default headers if needed
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// login async
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<LoginResponse> LoginAsync(string username, string password)
        {
            try
            {
                var loginRequest = new LoginRequest
                {
                    Username = username,
                    Password = password
                };

                var json = JsonSerializer.Serialize(loginRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("Users/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    return JsonSerializer.Deserialize<LoginResponse>(responseContent, options);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new Exception("Invalid username or password");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Login failed: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                throw new Exception($"Login error: {ex.Message}");
            }
        }

        /// <summary>
        /// get client profile by user ID
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<ApiResponse<GarageProfile>> GetGarageByUserID(int userid)
        {
            try
            {
                var response = await _httpClient.GetAsync($"GarageProfiles/GetGarageProfileByUserID/{userid}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response content as clientprofile
                    var garageprofile = await response.Content.ReadFromJsonAsync<GarageProfile>();

                    if (garageprofile == null) // Handle potential null reference
                    {
                        return new ApiResponse<GarageProfile>
                        {
                            IsSuccess = false,
                            ErrorMessage = "Garageprofile not found"
                        };
                    }

                    return new ApiResponse<GarageProfile> { Data = garageprofile, IsSuccess = true };
                }
                else
                {
                    return new ApiResponse<GarageProfile>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<GarageProfile>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// get client profile by user ID
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Vehicle>> GetVehicleByID(int VehicleID)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Vehicles/{VehicleID}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response content as clientprofile
                    var VehicleR = await response.Content.ReadFromJsonAsync<Vehicle>();

                    if (VehicleR == null) // Handle potential null reference
                    {
                        return new ApiResponse<Vehicle>
                        {
                            IsSuccess = false,
                            ErrorMessage = "Vehicle not found"
                        };
                    }

                    return new ApiResponse<Vehicle> { Data = VehicleR, IsSuccess = true };
                }
                else
                {
                    return new ApiResponse<Vehicle>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<Vehicle>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// GetVehicleTypesAsync
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<VehicleType>>> GetVehicleTypesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("VehicleTypes");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var vehicletypes = await response.Content.ReadFromJsonAsync<List<VehicleType>>();
                    return new ApiResponse<List<VehicleType>>
                    {
                        IsSuccess = true,
                        Data = vehicletypes.OrderBy(c => c.VehicleTypesDesc).ToList(),
                    };
                }
                else
                {
                    return new ApiResponse<List<VehicleType>>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode} - {content}",

                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<VehicleType>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,

                };
            }
        }

        /// <summary>
        /// GetManufacturersAsync
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Manufacturer>>> GetManufacturersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("Manufacturers");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var manufacturers = await response.Content.ReadFromJsonAsync<List<Manufacturer>>();
                    return new ApiResponse<List<Manufacturer>>
                    {
                        IsSuccess = true,
                        Data = manufacturers.OrderBy(c => c.ManufacturerDesc).ToList(),
                    };
                }
                else
                {
                    return new ApiResponse<List<Manufacturer>>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode} - {content}",

                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<Manufacturer>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,

                };
            }
        }

        /// <summary>
        /// GetFuelTypesAsync
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<FuelType>>> GetFuelTypesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("FuelTypes");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var manufacturers = await response.Content.ReadFromJsonAsync<List<FuelType>>();
                    return new ApiResponse<List<FuelType>>
                    {
                        IsSuccess = true,
                        Data = manufacturers.OrderBy(c => c.FuelTypeDesc).ToList(),
                    };
                }
                else
                {
                    return new ApiResponse<List<FuelType>>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode} - {content}",

                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<FuelType>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,

                };
            }
        }

        /// <summary>
        /// GetMeassureUnitsAsync
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<MeassureUnit>>> GetMeassureUnitsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("MeassureUnits");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var MeassureUnits = await response.Content.ReadFromJsonAsync<List<MeassureUnit>>();
                    return new ApiResponse<List<MeassureUnit>>
                    {
                        IsSuccess = true,
                        Data = MeassureUnits.OrderBy(c => c.MeassureUnitDesc).ToList(),
                    };
                }
                else
                {
                    return new ApiResponse<List<MeassureUnit>>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode} - {content}",

                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<MeassureUnit>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,

                };
            }
        }

        /// <summary>
        /// GetServiceTypesAsync
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<ServiceType>>> GetServiceTypesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("ServiceTypes");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var serviceTypes = await response.Content.ReadFromJsonAsync<List<ServiceType>>();
                    return new ApiResponse<List<ServiceType>>
                    {
                        IsSuccess = true,
                        Data = serviceTypes.OrderBy(c => c.Description).ToList(),
                    };
                }
                else
                {
                    return new ApiResponse<List<ServiceType>>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode} - {content}",
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ServiceType>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        /// <summary>
        /// GetCurremciesAsync
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Currency>>> GetCurremciesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("Currencies");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var Currencies = await response.Content.ReadFromJsonAsync<List<Currency>>();
                    return new ApiResponse<List<Currency>>
                    {
                        IsSuccess = true,
                        Data = Currencies.OrderBy(c => c.CurrDesc).ToList(),
                    };
                }
                else
                {
                    return new ApiResponse<List<Currency>>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode} - {content}",
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<Currency>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        /// <summary>
        /// AddVehiclesServicesAsync
        /// </summary>
        /// <param name="vehicleservice"></param>
        /// <returns></returns>
        public async Task<ApiResponse<VehiclesService>> AddVehiclesServicesAsync(VehiclesService vehicleservice)
        {
            try
            {
                var json = JsonSerializer.Serialize(vehicleservice);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("VehiclesServices", content);
                if (response.IsSuccessStatusCode)
                {

                    var VehicleServices = await response.Content.ReadFromJsonAsync<VehiclesService>();
                    return new ApiResponse<VehiclesService>
                    {
                        IsSuccess = true,
                        Data = VehicleServices,
                    };

                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    // Handle different status codes
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        throw new Exception($"Validation error: {errorContent}");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        throw new Exception($"Conflict: {errorContent}");
                    }
                    else
                    {
                        throw new Exception($"API error: {response.StatusCode} - {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Error adding vehicle: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// AddVehiclesServiceTypeAsync
        /// </summary>
        /// <param name="vehicleservicetype"></param>
        /// <returns></returns>
        public async Task<(bool IsSuccess, string Message, VehiclesServiceType vehiclesservicetype)> AddVehiclesServiceTypeAsync(VehiclesServiceType vehicleservicetype)
        {
            try
            {
                var json = JsonSerializer.Serialize(vehicleservicetype);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("VehiclesServiceTypes", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var AddedVehicleServicesTypes = JsonSerializer.Deserialize<VehiclesServiceType>(responseContent);
                    return (true, "Registration successful", AddedVehicleServicesTypes);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    // Handle different status codes
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        throw new Exception($"Validation error: {errorContent}");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        throw new Exception($"Conflict: {errorContent}");
                    }
                    else
                    {
                        throw new Exception($"API error: {response.StatusCode} - {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Error adding vehicle: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// GetVehicleByLiscenceID
        /// </summary>
        /// <param name="LiscencePlate"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Vehicle>> GetVehicleByLiscenceID(String LiscencePlate)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Vehicles/license/{LiscencePlate}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response content as clientprofile
                    var vehicle = await response.Content.ReadFromJsonAsync<Vehicle>();

                    if (vehicle == null) // Handle potential null reference
                    {
                        return new ApiResponse<Vehicle>
                        {
                            IsSuccess = false,
                            ErrorMessage = "Garageprofile not found"
                        };
                    }

                    return new ApiResponse<Vehicle> { Data = vehicle, IsSuccess = true };
                }
                else
                {
                    return new ApiResponse<Vehicle>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<Vehicle>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Register users
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<(bool IsSuccess, string Message, User RegisteredUser)> RegisterUserAsync(User user)
        {
            try
            {
                var json = JsonSerializer.Serialize(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("Users/register", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var registeredUser = JsonSerializer.Deserialize<User>(responseContent);
                    return (true, "Registration successful", registeredUser);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    return (false, "Username already exists", null);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return (false, errorMessage, null);
                }
                else
                {
                    return (false, $"Registration failed: {response.ReasonPhrase}", null);
                }
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred: {ex.Message}", null);
            }
        }

        /// <summary>
        /// GarageRegister
        /// </summary>
        /// <param name="Garage"></param>
        /// <returns></returns>
        public async Task<ApiResponse<GarageProfile>> GarageRegister(GarageProfile Garage)
        {
            try
            {
                var json = JsonSerializer.Serialize(Garage);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("GarageProfiles", content);

                if (response.IsSuccessStatusCode)
                {
                    var garageProfile = await response.Content.ReadFromJsonAsync<GarageProfile>();
                    return new ApiResponse<GarageProfile>
                    {
                        IsSuccess = true,
                        Data = garageProfile,
                    };
                   
                }
                else
                {

                    var errorContent = await response.Content.ReadAsStringAsync();

                    // Handle different status codes
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        throw new Exception($"Validation error: {errorContent}");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        throw new Exception($"Conflict: {errorContent}");
                    }
                    else
                    {
                        throw new Exception($"API error: {response.StatusCode} - {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Error adding Garage: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Update Garage Profile 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="clientProfile"></param>
        /// <returns></returns>
        public async Task<bool> UpdateGarageProfileAsync(int id, GarageProfile garageProfile)
        {
            try
            {
                var json = JsonSerializer.Serialize(garageProfile);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"GarageProfiles/{id}", content);

                if (response.IsSuccessStatusCode)
                {

                    return true;
                }

                // Handle specific status codes if needed
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new Exception("Garage profile not found");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception("Invalid request - ID mismatch");
                }

                return false;
            }
            catch (Exception ex)
            {
                // Log error or handle it appropriately
                Console.WriteLine($"Error updating Garage profile: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// CheckVehicleAsync
        /// </summary>
        /// <param name="vehiclecheck"></param>
        /// <returns></returns>
        public async Task<ApiResponse<VehicleCheck>> SaveVehicleCheckAsync(VehicleCheck vehicleCheck)
        {
            try
            {
                var json = JsonSerializer.Serialize(vehicleCheck);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("vehiclechecks", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var createdVehicleCheck = JsonSerializer.Deserialize<VehicleCheck>(
                        responseContent,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return new ApiResponse<VehicleCheck>
                    {
                        IsSuccess = true,
                        Data = createdVehicleCheck,
                    };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<VehicleCheck>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode} - {errorContent}",
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<VehicleCheck>
                {
                    IsSuccess = false,
                    ErrorMessage = $"Exception: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<ClientNotification>> SaveClientNotificationsAsync(ClientNotification vehicleCheck)
        {
            try
            {
                var json = JsonSerializer.Serialize(vehicleCheck);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("ClientNotifications", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var createdVehicleCheck = JsonSerializer.Deserialize<ClientNotification>(
                        responseContent,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return new ApiResponse<ClientNotification>
                    {
                        IsSuccess = true,
                        Data = createdVehicleCheck,
                    };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<ClientNotification>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode} - {errorContent}",
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<ClientNotification>
                {
                    IsSuccess = false,
                    ErrorMessage = $"Exception: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Registers a client profile.
        /// </summary>
        /// <param name="Client"></param>
        /// <returns></returns>
        public async Task<ApiResponse<ClientProfile>> ClientRegister(ClientProfile Client)
        {
            try
            {
                var json = JsonSerializer.Serialize(Client);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("ClientProfiles", content);

                if (response.IsSuccessStatusCode)
                {
                    var clientProfile = await response.Content.ReadFromJsonAsync<ClientProfile>();
                    return new ApiResponse<ClientProfile>
                    {
                        IsSuccess = true,
                        Data = clientProfile,
                    };

                }
                else
                {

                    var errorContent = await response.Content.ReadAsStringAsync();

                    // Handle different status codes
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        throw new Exception($"Validation error: {errorContent}");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        throw new Exception($"Conflict: {errorContent}");
                    }
                    else
                    {
                        throw new Exception($"API error: {response.StatusCode} - {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding Garage: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Adds a new vehicle asynchronously.
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        public async Task<(bool IsSuccess, string Message, Vehicle vehicle)> AddVehicleAsync(Vehicle vehicle)
        {
            try
            {
                var json = JsonSerializer.Serialize(vehicle);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("Vehicles", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var AddedVehicle = JsonSerializer.Deserialize<Vehicle>(responseContent);
                    return (true, "Registration successful", AddedVehicle);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    // Handle different status codes
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        throw new Exception($"Validation error: {errorContent}");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        throw new Exception($"Conflict: {errorContent}");
                    }
                    else
                    {
                        throw new Exception($"API error: {response.StatusCode} - {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Error adding vehicle: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Get all countries as list
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Country>>> GetCountriesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("Countries");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var countries = await response.Content.ReadFromJsonAsync<List<Country>>();
                    //var countries = JsonSerializer.Deserialize<List<Country>>(content);
                    return new ApiResponse<List<Country>>
                    {
                        IsSuccess = true,
                        Data = countries.OrderBy(c => c.CountryName).ToList(),
                    };
                }
                else
                {
                    return new ApiResponse<List<Country>>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode} - {content}",

                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<Country>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,

                };
            }
        }

        /// <summary>
        /// Get all countries as list
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Specialization>>> GetSpecializationsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("Specializations");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var specializations = await response.Content.ReadFromJsonAsync<List<Specialization>>();
                    //var countries = JsonSerializer.Deserialize<List<Country>>(content);
                    return new ApiResponse<List<Specialization>>
                    {
                        IsSuccess = true,
                        Data = specializations.OrderBy(c => c.SpecializationDesc).ToList(),
                    };
                }
                else
                {
                    return new ApiResponse<List<Specialization>>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode} - {content}",

                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<Specialization>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,

                };
            }
        }

        /// <summary>
        /// Fetches a user type by its ID.
        /// </summary>
        /// <param name="Typeid"></param>
        /// <returns></returns>
        public async Task<ApiResponse<UserType>> GetUserType(int Typeid)
        {
            try
            {
                // Call the API endpoint

                using var response = await _httpClient.GetAsync($"UserTypes/{Typeid}");

                if (response.IsSuccessStatusCode)
                {
                    var userType = await response.Content.ReadFromJsonAsync<UserType>();
                    return new ApiResponse<UserType> { Data = userType, IsSuccess = true };
                }

                // Handle non-success status codes
                var errorMessage = await response.Content.ReadAsStringAsync();
                return response.StatusCode switch
                {
                    HttpStatusCode.NotFound =>
                        new ApiResponse<UserType> { ErrorMessage = "User type not found", IsSuccess = false },
                    _ =>
                        new ApiResponse<UserType> { ErrorMessage = $"Error fetching user type: {response.ReasonPhrase}", IsSuccess = false }
                };
            }
            catch (Exception ex)
            {
                // Handle exceptions (network issues, etc.)
                return new ApiResponse<UserType>
                {
                    ErrorMessage = $"An error occurred while fetching user type: {ex.Message}",
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// get user by user name
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<ApiResponse<User>> GetUserByUsername(string username)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Users/GetUserByUserName/{username}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response content as User
                    var user = await response.Content.ReadFromJsonAsync<User>();

                    if (user == null) // Handle potential null reference
                    {
                        return new ApiResponse<User>
                        {
                            IsSuccess = false,
                            ErrorMessage = "User not found"
                        };
                    }

                    return new ApiResponse<User> { Data = user, IsSuccess = true };
                }
                else
                {
                    return new ApiResponse<User>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<User>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get client by by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<GarageProfile>> GetGarageByID(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"GarageProfiles/{id}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response content as clientprofile
                    var garageprofile = await response.Content.ReadFromJsonAsync<GarageProfile>();

                    if (garageprofile == null) // Handle potential null reference
                    {
                        return new ApiResponse<GarageProfile>
                        {
                            IsSuccess = false,
                            ErrorMessage = "Garage profile not found"
                        };
                    }

                    return new ApiResponse<GarageProfile> { Data = garageprofile, IsSuccess = true };
                }
                else
                {
                    return new ApiResponse<GarageProfile>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<GarageProfile>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get Offers By User Type
        /// </summary>
        /// <param name="userTypeId"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<PremiumOffer>>> GetOffersByUserType(int userTypeId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"PremiumOffers/type/{userTypeId}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response content as clientprofile
                    var premiumoffer = await response.Content.ReadFromJsonAsync<List<PremiumOffer>>();

                    if (premiumoffer == null) // Handle potential null reference
                    {
                        return new ApiResponse<List<PremiumOffer>>
                        {
                            IsSuccess = false,
                            ErrorMessage = "premiumoffer not found"
                        };
                    }

                    //return new ApiResponse<PremiumOffer> { Data = premiumoffer, IsSuccess = true };
                    return new ApiResponse<List<PremiumOffer>> { Data = premiumoffer, IsSuccess = true };
                }
                else
                {
                    return new ApiResponse<List<PremiumOffer>>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<PremiumOffer>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GarageID"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<VehicleAppointment>>> GetUpcomingAppointments(int GarageID)
        {
            try
            {
                // Call the API endpoint

                using var response = await _httpClient.GetAsync($"VehicleAppointments/upcoming/{GarageID}");

                if (response.IsSuccessStatusCode)
                {
                    var appointments = await response.Content.ReadFromJsonAsync<List<VehicleAppointment>>();
                    return new ApiResponse<List<VehicleAppointment>> { Data = appointments, IsSuccess = true };
                }

                // Handle non-success status codes
                var errorMessage = await response.Content.ReadAsStringAsync();
                return response.StatusCode switch
                {
                    HttpStatusCode.NotFound =>
                        new ApiResponse<List<VehicleAppointment>> { ErrorMessage = "User type not found", IsSuccess = false },
                    _ =>
                        new ApiResponse<List<VehicleAppointment>> { ErrorMessage = $"Error fetching user type: {response.ReasonPhrase}", IsSuccess = false }
                };
            }
            catch (Exception ex)
            {
                // Handle exceptions (network issues, etc.)
                return new ApiResponse<List<VehicleAppointment>>
                {
                    ErrorMessage = $"An error occurred while fetching user type: {ex.Message}",
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PremiumID"></param>
        /// <returns></returns>
        public async Task<ApiResponse<PremiumOffer>> GetPremiumByID(int PremiumID)
        {
            try
            {
                var response = await _httpClient.GetAsync($"PremiumOffers/{PremiumID}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response content as clientprofile
                    var Premium = await response.Content.ReadFromJsonAsync<PremiumOffer>();

                    if (Premium == null) // Handle potential null reference
                    {
                        return new ApiResponse<PremiumOffer>
                        {
                            IsSuccess = false,
                            ErrorMessage = "Vehicle not found"
                        };
                    }

                    return new ApiResponse<PremiumOffer> { Data = Premium, IsSuccess = true };
                }
                else
                {
                    return new ApiResponse<PremiumOffer>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<PremiumOffer>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GarageID"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<GaragePaymentMethod>>> GetPaymentMethodByID(int GarageID)
        {
            try
            {
                using var response = await _httpClient.GetAsync($"GaragePaymentMethods/garage/{GarageID}");

                if (response.IsSuccessStatusCode)
                {
                    var paymentMethod = await response.Content.ReadFromJsonAsync<List<GaragePaymentMethod>>();
                    return new ApiResponse<List<GaragePaymentMethod>> { Data = paymentMethod, IsSuccess = true };
                }

                // Handle non-success status codes
                var errorMessage = await response.Content.ReadAsStringAsync();
                return response.StatusCode switch
                {
                    HttpStatusCode.NotFound =>
                        new ApiResponse<List<GaragePaymentMethod>> { ErrorMessage = "User type not found", IsSuccess = false },
                    _ =>
                        new ApiResponse<List<GaragePaymentMethod>> { ErrorMessage = $"Error fetching user type: {response.ReasonPhrase}", IsSuccess = false }
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<GaragePaymentMethod>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Order"></param>
        /// <returns></returns>
        public async Task<ApiResponse<GaragePaymentOrder>> AddGaragePaymentOrder(GaragePaymentOrder Order)
        {
            try
            {
                var json = JsonSerializer.Serialize(Order);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("GaragePaymentOrders", content);

                if (response.IsSuccessStatusCode)
                {
                    var PaymentOrder = await response.Content.ReadFromJsonAsync<GaragePaymentOrder>();
                    return new ApiResponse<GaragePaymentOrder>
                    {
                        IsSuccess = true,
                        Data = PaymentOrder,
                    };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    // Handle different status codes
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        throw new Exception($"Validation error: {errorContent}");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        throw new Exception($"Conflict: {errorContent}");
                    }
                    else
                    {
                        throw new Exception($"API error: {response.StatusCode} - {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding Garage Payment Order: {ex.Message}");
                throw;
            }
        }

        public async Task<ApiResponse<GaragePaymentOrder>> GetPaymentOrderByID(int OrderID)
        {
            try
            {
                using var response = await _httpClient.GetAsync($"GaragePaymentOrders/{OrderID}");

                if (response.IsSuccessStatusCode)
                {
                    var paymentMethod = await response.Content.ReadFromJsonAsync<GaragePaymentOrder>();
                    return new ApiResponse<GaragePaymentOrder> { Data = paymentMethod, IsSuccess = true };
                }

                // Handle non-success status codes
                var errorMessage = await response.Content.ReadAsStringAsync();
                return response.StatusCode switch
                {
                    HttpStatusCode.NotFound =>
                        new ApiResponse<GaragePaymentOrder> { ErrorMessage = "User type not found", IsSuccess = false },
                    _ =>
                        new ApiResponse<GaragePaymentOrder> { ErrorMessage = $"Error fetching user type: {response.ReasonPhrase}", IsSuccess = false }
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<GaragePaymentOrder>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<bool> UpdateGaragePremiumStatusAsync(int garageId, bool isPremium)
        {
            try
            {
                var requestUrl = $"GarageProfiles/{garageId}/premium-status";

                // Body should be a JSON boolean, so we serialize `true` or `false`
                var content = new StringContent(
                    isPremium.ToString().ToLower(),  // true/false in lowercase JSON format
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PatchAsync(requestUrl, content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating premium status: {ex.Message}");
                return false;
            }
        }
       
        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            var requestUrl = $"GaragePaymentOrders/{orderId}/status";

            // Create StringContent for the raw string body (JSON)
            var content = new StringContent($"\"{status}\"", Encoding.UTF8, "application/json");

            var response = await _httpClient.PatchAsync(requestUrl, content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdatePaymentOrderAsync(int id, GaragePaymentOrder Order)
        {
            try
            {
                var json = JsonSerializer.Serialize(Order);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"GaragePaymentOrders/{id}", content);

                if (response.IsSuccessStatusCode)
                {

                    return true;
                }

                // Handle specific status codes if needed
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new Exception("Garage Order not found");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception("Invalid request - ID mismatch");
                }

                return false;
            }
            catch (Exception ex)
            {
                // Log error or handle it appropriately
                Console.WriteLine($"Error updating Garage Order: {ex.Message}");
                throw;
            }
        }

        public async Task<ApiResponse<GaragePremiumRegistration>> AddGaragePremium(GaragePremiumRegistration Order)
        {
            try
            {
                var json = JsonSerializer.Serialize(Order);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("GaragePremiumRegistrations", content);

                if (response.IsSuccessStatusCode)
                {
                    var GaragePremium = await response.Content.ReadFromJsonAsync<GaragePremiumRegistration>();
                    return new ApiResponse<GaragePremiumRegistration>
                    {
                        IsSuccess = true,
                        Data = GaragePremium,
                    };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    // Handle different status codes
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        throw new Exception($"Validation error: {errorContent}");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        throw new Exception($"Conflict: {errorContent}");
                    }
                    else
                    {
                        throw new Exception($"API error: {response.StatusCode} - {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding Garage: {ex.Message}");
                throw;
            }
        }

        public async Task<GaragePremiumRegistration> GetActiveRegistrationByGarageId(int garageId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"GaragePremiumRegistrations/activeGarage/{garageId}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var activeRegistration = JsonSerializer.Deserialize<GaragePremiumRegistration>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return activeRegistration;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // No active registration found
                    return null;
                }
                else
                {
                    // Handle other error status codes
                    throw new HttpRequestException($"API call failed with status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (network errors, etc.)
                throw new Exception($"Error calling API: {ex.Message}", ex);
            }
        }

        public async Task<ApiResponse<List<PaymentType>>> GetPaymentTypesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("PaymentTypes");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var PaymentTypes = await response.Content.ReadFromJsonAsync<List<PaymentType>>();
                    
                    return new ApiResponse<List<PaymentType>>
                    {
                        IsSuccess = true,
                        Data = PaymentTypes.OrderBy(c => c.PaymentTypeDesc).ToList(),
                    };
                }
                else
                {
                    return new ApiResponse<List<PaymentType>>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode} - {content}",

                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<PaymentType>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,

                };
            }
        }

        public async Task<ApiResponse<GaragePaymentMethod>> AddGaragePaymentMethod(GaragePaymentMethod PaymentMethod)
        {
            try
            {
                var json = JsonSerializer.Serialize(PaymentMethod);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("GaragePaymentMethods", content);

                if (response.IsSuccessStatusCode)
                {
                    var garagePaymentMethod = await response.Content.ReadFromJsonAsync<GaragePaymentMethod>();
                    return new ApiResponse<GaragePaymentMethod>
                    {
                        IsSuccess = true,
                        Data = garagePaymentMethod,
                    };
                }
                else
                {

                    var errorContent = await response.Content.ReadAsStringAsync();

                    // Handle different status codes
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        throw new Exception($"Validation error: {errorContent}");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        throw new Exception($"Conflict: {errorContent}");
                    }
                    else
                    {
                        throw new Exception($"API error: {response.StatusCode} - {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Error adding Payment Method: {ex.Message}");
                throw;
            }
        }

    }
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public User User { get; set; }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
