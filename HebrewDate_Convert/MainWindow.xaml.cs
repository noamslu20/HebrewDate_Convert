using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;


namespace HebrewDate_Convert
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private async void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            string inputDate = GregorianDateTextBox.Text;
            if (DateTime.TryParse(inputDate, out DateTime parsedDate))
            {
                string hebrewDate = await GetHebrewDateAsync(parsedDate);
                HebrewDateLabel.Content = string.IsNullOrEmpty(hebrewDate)
                    ? "Conversion failed. Please try again."
                    : $"Hebrew Date: {hebrewDate}";
            }
            else
            {
                MessageBox.Show("Please enter a valid date in the format YYYY-MM-DD.");
            }
        }

        private async Task<string> GetHebrewDateAsync(DateTime date)
        {
            try
            {
                string url = $"https://www.hebcal.com/converter?cfg=json&gy={date.Year}&gm={date.Month}&gd={date.Day}&g2h=1";
                using HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                    return result.hebrew;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            return null;
        }
    }
}
