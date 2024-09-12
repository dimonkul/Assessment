using Microsoft.AspNetCore.Mvc;

namespace API3.Controllers
{
    /// <summary>
    /// Controller for API3 endpoints.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class API3Controller : ControllerBase
    {
        #region Fields

        private readonly IHttpClientFactory _clientFactory;

        #endregion

        #region Constructor



        /// <summary>
        /// Initializes a new instance of the API3Controller.
        /// </summary>
        /// <param name="clientFactory">The HTTP client factory for creating HTTP clients.</param>
        public API3Controller(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Handles GET requests to API3.
        /// </summary>
        /// <returns>A string representing the current time after a 1-second delay.</returns>
        [HttpGet]
        public async Task<string> Get()
        {
            // Simulate a delay of 2 second
            await Task.Delay(2000);

            // Return the current time as a string
            return $"{DateTime.Now.ToLongTimeString()}";
        }
        #endregion
    }
}