using System.Windows;

namespace BinaryToDecimalCalculator
{
    public partial class MainWindow : Window
    {
        public CalculatorViewModel ViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new CalculatorViewModel();
            DataContext = ViewModel;
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Clear();
        }

        private void OpenWindow1_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            window1.Show();
        }

    }
}
