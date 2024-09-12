using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace BlazorApp.Services
{
    /// <summary>
    /// Provides custom authentication state management for the Blazor application.
    /// </summary>
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        #region Constants

        private const string AuthTokenKey = "authToken";
        private const string JwtAuthType = "jwt";

        #endregion

        #region Fields

        private readonly ILocalStorageService _localStorage;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the CustomAuthStateProvider class.
        /// </summary>
        /// <param name="localStorage">The local storage service for retrieving the auth token.</param>
        public CustomAuthStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the current authentication state asynchronously.
        /// </summary>
        /// <returns>The current AuthenticationState.</returns>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>(AuthTokenKey);
            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, JwtAuthType);
            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }

        /// <summary>
        /// Notifies the authentication state has changed due to user authentication.
        /// </summary>
        /// <param name="token">The JWT token of the authenticated user.</param>
        public void NotifyUserAuthentication(string token)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), JwtAuthType));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        /// <summary>
        /// Notifies the authentication state has changed due to user logout.
        /// </summary>
        public void NotifyUserLogout()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Parses claims from a JWT token.
        /// </summary>
        /// <param name="jwt">The JWT token to parse.</param>
        /// <returns>An enumerable of Claims extracted from the token.</returns>
        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            if (keyValuePairs != null)
            {
                if (keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles))
                {
                    AddRoleClaims(claims, roles);
                    keyValuePairs.Remove(ClaimTypes.Role);
                }

                claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
            }

            return claims;
        }

        /// <summary>
        /// Adds role claims to the claims collection.
        /// </summary>
        /// <param name="claims">The claims collection to add to.</param>
        /// <param name="roles">The roles object from the JWT payload.</param>
        private void AddRoleClaims(List<Claim> claims, object roles)
        {
            if (roles.ToString().Trim().StartsWith("["))
            {
                var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());
                claims.AddRange(parsedRoles.Select(role => new Claim(ClaimTypes.Role, role)));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
            }
        }

        /// <summary>
        /// Parses a base64 string without padding.
        /// </summary>
        /// <param name="base64">The base64 string to parse.</param>
        /// <returns>The parsed byte array.</returns>
        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        #endregion
    }
}