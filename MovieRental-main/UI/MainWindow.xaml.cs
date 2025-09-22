using System.Windows;
using UI.ViewModels;

namespace UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly RentalViewModel _viewModel;

    public MainWindow()
    {
        InitializeComponent();
        _viewModel = new RentalViewModel();
        DataContext = _viewModel;
    }

    private async void LoadRentals_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(CustomerNameTextBox.Text))
            {
                MessageBox.Show("Please enter a customer name");
                return;
            }

            await _viewModel.LoadRentalsAsync(CustomerNameTextBox.Text);
        }
        catch (Exception exception)
        {
            MessageBox.Show("An error occurred while loading rentals: " + exception.Message);
        }
    }
}