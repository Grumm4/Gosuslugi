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
using System.Windows.Shapes;

namespace Gosuslugi
{
    /// <summary>
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        public OrderWindow()
        {
            InitializeComponent();
        }

        private void LabelCountOrders_Loaded(object sender, RoutedEventArgs e)
        {
            ShowOrders();
        }

        private void CreateOrderMenu_Click(object sender, RoutedEventArgs e)
        {
            bool? result = new CreateOrderWindow().ShowDialog();
            if (result == true)
            {
                ShowOrders();
            }
        }

        void ShowOrders()
        {
            List<OrderModel> orderCards = new List<OrderModel>();

            using (var context = new ApplicationContext())
            {
                var all = context.Orders.ToList();



                foreach (var order in all)
                {
                    var orderCard = new OrderModel
                    {
                        Id = order.Id,
                        Title = order.Title,
                        Price = order.Price,
                        Date = order.Date,
                        Place = order.Place,
                        Contacts = order.Contacts,
                        Comments = order.Comments
                    };
                    

                    orderCards.Add(orderCard);
                };

                LabelCountOrders.Content = "Колличество активных заказов: " + all.Count.ToString();
                DGOrders.ItemsSource = orderCards;
            }
        }

        private void DeleteOrderBt_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).CommandParameter;
            using (var context = new ApplicationContext())
            {
                context.Orders.Remove(context.Orders.FirstOrDefault(o => o.Id == id));
                context.SaveChanges();
                ShowOrders();
            }
        }
    }
}
