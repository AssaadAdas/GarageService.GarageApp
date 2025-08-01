using GarageService.GarageLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<(bool IsSuccess, string Message, GarageProfile RegisteredUser)> GarageRegister(GarageProfile Garage)
        {
            try
            {
                var json = JsonSerializer.Serialize(Garage);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("GarageProfiles", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var registeredGarage = JsonSerializer.Deserialize<GarageProfile>(responseContent);
                    return (true, "Registration successful", registeredGarage);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    return (false, "Client already exists", null);
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
                    throw new Exception("Client profile not found");
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
                Console.WriteLine($"Error updating client profile: {ex.Message}");
                throw;
            }
        }


        /// <summary>
        /// Registers a client profile.
        /// </summary>
        /// <param name="Client"></param>
        /// <returns></returns>
        public async Task<(bool IsSuccess, string Message, ClientProfile RegisteredUser)> ClientRegister(ClientProfile Client)
        {
            try
            {
                var json = JsonSerializer.Serialize(Client);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("ClientProfiles", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var registeredClient = JsonSerializer.Deserialize<ClientProfile>(responseContent);
                    return (true, "Registration successful", registeredClient);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    return (false, "Client already exists", null);
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
