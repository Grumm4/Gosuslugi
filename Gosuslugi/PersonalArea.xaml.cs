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
    /// Логика взаимодействия для PersonalArea.xaml
    /// </summary>
    public partial class PersonalArea : Window
    {
        TextBox tb = new TextBox();
        List<Button> buttonList = new List<Button>() { };
        public PersonalArea()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoginTb.Text = Login.currentUser.Login;
            NameTb.Text = Login.currentUser.Name;
            PhoneTb.Text = Login.currentUser.PhoneNumber;
            MailTb.Text = Login.currentUser.Email;

            buttonList.Add(ChangeLoginBt); buttonList.Add(ChangePhoneBt); buttonList.Add(ChangeNameBt); buttonList.Add(ChangeMailBt);

            ShowOrders();
        }

        void ChangePersonalData(object sender, RoutedEventArgs e)
        {
            Button? bt = sender as Button;
            foreach (Button button in buttonList)
            {
                if (button.Name == bt.Name)
                {
                    IterateTextBoxes(this, button.Tag);
                    tb.IsEnabled = true;
                    continue;

                }   
                button.IsEnabled = false;
            }

            bt.Content = "Сохранить";

            bt.Click -= ChangePersonalData;
            bt.Click += SavePersonalData;
        }

        void SavePersonalData(object sender, RoutedEventArgs e)
        {
            Button? bt = sender as Button;
            bt.Content = "Изменить";

            foreach (Button button in buttonList)
            {
                if (button.IsEnabled == false)
                {
                    tb.IsEnabled = false;
                    button.IsEnabled = true;
                }
            }
            bt.Click -= SavePersonalData;
            bt.Click += ChangePersonalData;

            //сохранение в бд
            using (var context = new ApplicationContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Id == Login.currentUser.Id);

                if (user != null)
                {
                    user.Login = LoginTb.Text;
                    user.Name = NameTb.Text;
                    user.PhoneNumber = PhoneTb.Text;
                    user.Email = MailTb.Text;

                    context.SaveChanges();
                }
            }
        }

        public void IterateTextBoxes(DependencyObject parent, object tag)
        {
            
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                
                if (child is TextBox textBox)
                {
                    if (textBox.Tag != null)
                    {
                        if (textBox.Tag.Equals(tag))
                        {
                            tb = textBox;
                            break;
                        }
                    }
                }
                IterateTextBoxes(child, tag);
            }
        }

        void ShowOrders()
        {
            List<OrderModel> orderCards = new List<OrderModel>();

            using (var context = new ApplicationContext())
            {
                var all = context.Orders.Where(o => o.AcceptedUserId == Login.currentUser.Id).ToList();

                //INSERT INTO my_table (id, numbers) VALUES (1, '[1, 2, 3, 4, 5]'); JSON!!!!!!!!!!!!!!!!


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

        private void RejectOrder_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите отказаться от заказа?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                int id = (int)((Button)sender).CommandParameter; // получение id 

                using (var context = new ApplicationContext())
                {
                    OrderModel? order = context.Orders.FirstOrDefault(o => o.Id == id);
                    order.AcceptedUserId = 0;
                    context.SaveChanges();
                    ShowOrders();
                }
            }
        }

        private void ClosePersonalAreaBt_Click(object sender, RoutedEventArgs e) => DialogResult = true;
    }
}
