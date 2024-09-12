using Microsoft.Extensions.Logging;

namespace BlazorApp.Services
{
    /// <summary>
    /// Provides logging services for the Blazor application.
    /// </summary>
    public class LoggingService : ILoggingService
    {
        #region Fields

        private readonly ILogger<LoggingService> _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the LoggingService class.
        /// </summary>
        /// <param name="logger">The logger instance to be used for logging.</param>
        public LoggingService(ILogger<LoggingService> logger)
        {
            _logger = logger;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Logs an information message.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The warning message to be logged.</param>
        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        /// <summary>
        /// Logs an error message with an optional exception.
        /// </summary>
        /// <param name="message">The error message to be logged.</param>
        /// <param name="ex">The exception associated with the error, if any.</param>
        public void LogError(string message, Exception ex = null)
        {
            _logger.LogError(ex, message);
        }

        #endregion
    }
}