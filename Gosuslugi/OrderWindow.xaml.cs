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
            if (Login.currentUser.Role == "Admin") 
            {
                DGOrders.Columns[2].Visibility = Visibility.Collapsed; // Скрытие кнопки "принять заказ"
            }
            else
            {
                CreateOrderMenu.Visibility = Visibility.Collapsed; // Скрытие кнопки "создать заказ"
                DGOrders.Columns[1].Visibility = Visibility.Collapsed; // Скрытие кнопки "удалить заказ"
            }
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
                List<OrderModel> orders = new List<OrderModel>();

                if (Login.currentUser.Role != "Admin")
                    orders = context.Orders.Where(o => o.AcceptedUserId == 0).ToList();
                else
                    orders = context.Orders.ToList();

                //INSERT INTO my_table (id, numbers) VALUES (1, '[1, 2, 3, 4, 5]'); JSON!!!!!!!!!!!!!!!!


                foreach (var order in orders)
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

                LabelCountOrders.Content = "Колличество активных заказов: " + orders.Count.ToString();
                DGOrders.ItemsSource = orderCards;
            }
        }

        private void DeleteOrderBt_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).CommandParameter; // получение Id заказа при клике по кнопке рядом с ним
            using (var context = new ApplicationContext())
            {
                context.Orders.Remove(context.Orders.FirstOrDefault(o => o.Id == id));
                context.SaveChanges();
                ShowOrders();
            }
        }

        private void LogoutMenu_Click(object sender, RoutedEventArgs e)
        {
            Login.currentUser = new UserModel();
            Login l = new Login();
            l.Show();
            Close();
        }

        private void AcceptOrderBt_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).CommandParameter; // получение Id заказа при клике по кнопке рядом с ним
            using (var context = new ApplicationContext())
            {
                var order = context.Orders.FirstOrDefault(o => o.Id==id);
                order.AcceptedUserId = Login.currentUser.Id;
                context.SaveChanges();
                MessageBox.Show("Заказ принят и перемещён в личный кабинет");
                ShowOrders();
            }
        }

        private void ToPersonalArea_Click(object sender, RoutedEventArgs e)
        {
            bool? result = new PersonalArea().ShowDialog();
            if (result == true)
            {
                this.ShowOrders();
            }

        }
    }
}
