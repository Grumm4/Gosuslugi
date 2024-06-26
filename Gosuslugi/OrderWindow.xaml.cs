﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Reflection;
using System.Runtime.InteropServices;


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

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ShowOrders();
        }

        private void CreateOrderMenu_Click(object sender, RoutedEventArgs e)
        {
            AfterClosingAnimation.Animate(this, new CreateOrderWindow());
            //bool? result = new CreateOrderWindow().ShowDialog();
            //if (result == true)
            //{
            //    ShowOrders();
            //}
        }

        void ShowOrders()
        {
            List<OrderModel> orderModels = new List<OrderModel>();

            using (var context = new ApplicationContext())
            {
                List<OrderModel> orders = new List<OrderModel>();

                if (Login.currentUser.Role != "Admin")
                {
                    orders = context.Orders.Where(o => o.AcceptedUserId == 0).ToList();
                    CreateOrderMenu.Visibility = Visibility.Collapsed;
                }
                else
                    orders = context.Orders.ToList();

                foreach (var order in orders)
                {
                    var orderModel = new OrderModel
                    {
                        Id = order.Id,
                        Title = order.Title,
                        Price = order.Price,
                        Date = order.Date,
                        Place = order.Place,
                        Contacts = order.Contacts,
                        Comments = order.Comments,
                        
                    };

                    //получение имени исполнителя заказа
                    int? id = order.AcceptedUserId.Value;
                    string? exName = context.Users.Where(u => u.Id == id).FirstOrDefault()?.Name;
                    orderModel.ExecutorName = exName;

                    orderModels.Add(orderModel);
                    
                };

                LabelCountOrders.Content = "Колличество активных заказов: " + orders.Count.ToString();
                ICOrders.ItemsSource = orderModels;
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
            //var w = new Login();
            Login.currentUser = new UserModel();
            AfterClosingAnimation.Animate(this, new Login());
            //Login.LoginWindow.Show();
            
        }

        private void AcceptOrderBt_Click(object sender, RoutedEventArgs e)
        {
            //new Login().ShowUserFields();
            //.Show((sender as Button).ActualHeight.ToString() + (sender as Button).ActualWidth);
            Type objectType = Login.currentUser.GetType();
            PropertyInfo[] properties = objectType.GetProperties();

            bool fieldsIsNull = false;
            foreach (PropertyInfo propertyInfo in properties)
            {
                object value = propertyInfo.GetValue(Login.currentUser, null);
                if (value == null) 
                {
                    fieldsIsNull = true;
                }
            }

            if (!fieldsIsNull)
            {
                int id = (int)((Button)sender).CommandParameter; // получение Id заказа при клике по кнопке рядом с ним
                using (var context = new ApplicationContext())
                {
                    var order = context.Orders.FirstOrDefault(o => o.Id == id);
                    order.AcceptedUserId = Login.currentUser?.Id;
                    context.SaveChanges();
                    ShowOrders();
                    MessageBox.Show("Заказ принят и перемещён в личный кабинет");
                }
            }
            else
            {
                MessageBox.Show("Чтобы принять заказ заполните все данные о себе в личном кабинете!", "Ошибка", MessageBoxButton.OK);
            }
        }

        private void ToPersonalArea_Click(object sender, RoutedEventArgs e)
        {
            //new PersonalArea().ShowDialog();

            //this.ShowOrders();

            AfterClosingAnimation.Animate(this, new PersonalArea());
        }

        private void OrderCard_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show((sender as OrderCard).ExecutorName.Text);
        }

        private void DeleteBt_Loaded(object sender, RoutedEventArgs e)
        {
            if (Login.currentUser.Role != "Admin")
            {
                (sender as Button).Visibility = Visibility.Collapsed;
            }
        }

        private void AcceptBt_Loaded(object sender, RoutedEventArgs e)
        {
            if (Login.currentUser.Role == "Admin")
            {
                (sender as Button).Visibility = Visibility.Collapsed;
            }
        }

        private void CollapseBt_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaxiMinimizeBt_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void CloseBt_Click(object sender, RoutedEventArgs e)
        {
            AfterClosingAnimation.Animate(this, null);
        }

        private void MainWindow_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (GridHeader.IsMouseOver)
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    Point mousePosition = Mouse.GetPosition(this);
                    Point screenPosition = PointToScreen(mousePosition);

                    this.WindowState = WindowState.Normal;

                    double newX = screenPosition.X - (ActualWidth / 2);
                    double newY = screenPosition.Y - (e.GetPosition(this).Y);

                    double wind = newX + this.Width;

                    if (wind > 1920)
                        newX = 1920 - this.Width;

                    if (newX < 0)
                        newX = 0;

                    this.Left = newX;
                    this.Top = newY;
                }
                DragMove();
            }
        }
    }
}
