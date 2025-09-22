using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace UI.ViewModels
{
    public class RentalViewModel : INotifyPropertyChanged
    {
        private readonly HttpClient _httpClient;
        private ObservableCollection<RentalDto> _rentals;
        private string _customerName;
        private bool _isLoading;

        public RentalViewModel()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7271") };
            Rentals = new ObservableCollection<RentalDto>();
        }

        public ObservableCollection<RentalDto> Rentals
        {
            get => _rentals;
            set
            {
                _rentals = value;
                OnPropertyChanged();
            }
        }

        public string CustomerName
        {
            get => _customerName;
            set
            {
                _customerName = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadRentalsAsync(string customerName)
        {
            try
            {
                IsLoading = true;
                var rentals = await _httpClient.GetFromJsonAsync<RentalDto[]>($"Rental/by-customer/{customerName}");
                Rentals.Clear();
                if (rentals != null)
                {
                    foreach (var rental in rentals)
                    {
                        Rentals.Add(rental);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle error appropriately
                System.Windows.MessageBox.Show($"Error loading rentals: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RentalDto
    {
        public int Id { get; set; }
        public int DaysRented { get; set; }
        public MovieDto Movie { get; set; }
        public CustomerDto Customer { get; set; }
        public string PaymentMethod { get; set; }
    }

    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class CustomerDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}