using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
        if (string.IsNullOrWhiteSpace(CustomerNameTextBox.Text))
        {
            MessageBox.Show("Please enter a customer name");
            return;
        }

        await _viewModel.LoadRentalsAsync(CustomerNameTextBox.Text);
    }
}