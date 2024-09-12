using Blazored.LocalStorage;
using System.Net.Http.Json;
using BlazorApp.Models;

namespace BlazorApp.Services
{
    /// <summary>
    /// Provides authentication services for the Blazor application.
    /// </summary>
    public class AuthService : IAuthService
    {
        #region Fields

        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private const string AuthTokenKey = "authToken";
        private const string BaseApiUrl = "https://localhost:7013/api/auth";
        private const string LoginEndpoint = BaseApiUrl + "/login";
        private const string RegisterEndpoint = BaseApiUrl + "/register";

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the AuthService class.
        /// </summary>
        /// <param name="httpClient">The HttpClient for making API requests.</param>
        /// <param name="localStorage">The local storage service for storing authentication tokens.</param>
        public AuthService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Attempts to log in a user with the provided credentials.
        /// </summary>
        /// <param name="loginModel">The login credentials.</param>
        /// <returns>True if login is successful, otherwise false.</returns>
        public async Task<bool> Login(LoginModel loginModel)
        {
            var response = await _httpClient.PostAsJsonAsync(LoginEndpoint, loginModel);

            if (response.IsSuccessStatusCode)
            {
                var loginResult = await response.Content.ReadFromJsonAsync<LoginResult>();
                if (loginResult != null && !string.IsNullOrEmpty(loginResult.Token))
                {
                    await _localStorage.SetItemAsync(AuthTokenKey, loginResult.Token);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Logs out the current user by removing the authentication token.
        /// </summary>
        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync(AuthTokenKey);
        }

        /// <summary>
        /// Registers a new user with the provided information.
        /// </summary>
        /// <param name="registerModel">The registration information.</param>
        /// <returns>True if registration is successful, otherwise false.</returns>
        public async Task<bool> Register(RegisterModel registerModel)
        {
            var response = await _httpClient.PostAsJsonAsync(RegisterEndpoint, registerModel);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Retrieves the stored authentication token.
        /// </summary>
        /// <returns>The authentication token if it exists, otherwise null.</returns>
        public async Task<string> GetToken()
        {
            return await _localStorage.GetItemAsync<string>(AuthTokenKey);
        }

        #endregion
    }
}