using Microsoft.AspNetCore.Mvc;

namespace API2.Controllers
{
    /// <summary>
    /// Controller for API2 endpoints.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class API2Controller : ControllerBase
    {
        #region Fields

        private readonly IHttpClientFactory _clientFactory;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the API2Controller.
        /// </summary>
        /// <param name="clientFactory">The HTTP client factory for creating HTTP clients.</param>
        public API2Controller(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Handles GET requests to API2.
        /// </summary>
        /// <returns>A string representing the current time after a 1-second delay.</returns>
        [HttpGet]
        public async Task<string> Get()
        {
            // Simulate a delay of 1 second
            await Task.Delay(1000);

            // Return the current time as a string
            return $"{DateTime.Now.ToLongTimeString()}";
        }

        #endregion
    }
}