using BlazorApp.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BlazorApp.ViewModels
{
    /// <summary>
    /// ViewModel for interacting with the Web API.
    /// </summary>
    public class WebApiViewModel : INotifyPropertyChanged
    {
        #region Constants

        private const string ApiEndpoint = "https://localhost:7013/api/Frontend";

        #endregion

        #region Fields

        private readonly HttpClient _httpClient;
        private string _result;
        private ILoggingService _loggingService;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the result from the API call.
        /// </summary>
        public string Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the WebApiViewModel class.
        /// </summary>
        /// <param name="httpClient">The HttpClient for making API requests.</param>
        public WebApiViewModel(HttpClient httpClient, ILoggingService loggingService)
        {
            this._httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this._loggingService = loggingService;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sends a request to the Web API and updates the Result property.
        /// </summary>
        public async Task SendRequestToWebApi()
        {
            try
            {
                this.Result = await _httpClient.GetStringAsync(ApiEndpoint);
                this._loggingService.LogInformation(this.Result);
            }
            catch (HttpRequestException ex)
            {
                this.Result = $"Error: {ex.Message}";
                this._loggingService.LogError(this.Result);
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets the property value and raises the PropertyChanged event if the value has changed.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="storage">Reference to the backing field.</param>
        /// <param name="value">The new value.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>True if the value was changed, false otherwise.</returns>
        private bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion
    }
}