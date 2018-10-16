using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Metro_
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Product> Products { get; set; }
        static Random rand = new Random();
        public double weight;
        public Product p;

        public MainWindow()
        {
            InitializeComponent();
            Products = new ObservableCollection<Product> { };

            for (int i = 0; i < 50; i++)
            {
                Products.Add(new Product {
                    Name = RandomString(rand.Next(4, 10)),
                    Price = (rand.Next(10,100)+rand.NextDouble()).ToString().Substring(0,5),
                    ImagePath = "/Images/"+rand.Next(1,65)+".png"
                });
            }//Представим, что это загрузка из БД
            productList.ItemsSource = Products;
            weight = 0.000;
        }

        private void ProductList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (productList.SelectedItem != null)
            {
                p = (Product)productList.SelectedItem;
                Price.Text = p.Price;
                Cost.Text = Math.Round(Convert.ToDouble(p.Price) * weight, 2).ToString();
            }
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[rand.Next(s.Length)]).ToArray());
        }

        private void Refresh()
        {
            Weight.Text = Math.Round(weight, 2).ToString();
            Cost.Text = Math.Round(Convert.ToDouble(Price.Text) * Convert.ToDouble(Weight.Text), 2).ToString();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            weight = e.NewValue;
            Refresh();
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            if (weight > 0 && Cost.Text != "0")
            {
                MessageBox.Show(String.Format(
                     "Продукт: {0}\n" +
                     "Цена: {1} \n" +
                     "Вес: {2} \n" +
                     "Сумма: {3}\n" +
                     "[Штрих-код продукта]",
                     p.Name, p.Price, Math.Round(weight, 2), Cost.Text),
                     "Самоклеящая этикетка",
                     MessageBoxButton.OK,
                     MessageBoxImage.Information);

                productList.SelectedItem = null;
                Price.Text = "0";
                Refresh();
            }
        }
    }
}
