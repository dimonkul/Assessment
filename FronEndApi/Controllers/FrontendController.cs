using Microsoft.AspNetCore.Mvc;
using System.Text;

/// <summary>
/// Controller responsible for handling frontend-related API requests.
/// This controller acts as a gateway to other API services.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class FrontendController : ControllerBase
{
    #region Constants

    private const string API2_GET_URL = "https://localhost:7280/api/API2";
    private const string API3_GET_URL = "https://localhost:7209/api/API3";

    #endregion

    #region Fields

    private readonly IHttpClientFactory _clientFactory;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the FrontendController.
    /// </summary>
    /// <param name="clientFactory">The HTTP client factory for creating HTTP clients.</param>
    public FrontendController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Handles GET requests by calling two separate APIs concurrently and formatting their results.
    /// </summary>
    /// <returns>A formatted string containing results from both API calls.</returns>
    [HttpGet]
    public async Task<string> Get()
    {
        var client = _clientFactory.CreateClient();

        // Make concurrent API calls using the constant URLs
        var api2Task = client.GetStringAsync(API2_GET_URL);
        var api3Task = client.GetStringAsync(API3_GET_URL);

        // Wait for both API calls to complete
        await Task.WhenAll(api2Task, api3Task);

        var result = new
        {
            Api2Result = await api2Task,
            Api3Result = await api3Task
        };

        // Format and return the results
        return FormatApiResults(result.Api2Result, result.Api3Result);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Formats the results from the two API calls into a readable string.
    /// </summary>
    /// <param name="api2Result">The result from API2.</param>
    /// <param name="api3Result">The result from API3.</param>
    /// <returns>A formatted string containing both API results.</returns>
    private static string FormatApiResults(string api2Result, string api3Result)
    {
        var sb = new StringBuilder();
        sb.AppendLine("API Results:");
        sb.AppendLine("------------");
        sb.AppendLine($"API2: Result returned at {api2Result}");
        sb.AppendLine($"API3: Result returned at {api3Result}");
        return sb.ToString();
    }

    #endregion
}