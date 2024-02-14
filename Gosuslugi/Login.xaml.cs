using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Security.Cryptography;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gosuslugi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {   
        public static UserModel currentUser;

        public Login()
        {
            InitializeComponent();
        }

        public void LoginLoaded(object sender, RoutedEventArgs e)
        {
            IterateTextBoxes(this);
        }

        private void LoginBt_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new ApplicationContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Login == LoginBox.Text && u.HashedPwd == Registration.HashPwd(PwdBox.Password));
                if (user != null)
                {
                    currentUser = new UserModel
                    {
                        Id = user.Id,
                        Email = user.Email,
                        HashedPwd = user.HashedPwd,
                        Login = user.Login,
                        PhoneNumber = user.PhoneNumber,
                        Role = user.Role
                    };


                    OrderWindow ow = new OrderWindow();
                    ow.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин/пароль");
                }
            }
        }

        private void BtToReg_Click(object sender, RoutedEventArgs e)
        {
            var r = new Registration();
            r.Show();
            this.Close();
        }

        public static void IterateTextBoxes(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is TextBox textBox)
                {
                    if (textBox.Tag != null)
                    {
                        textBox.Text = textBox.Tag.ToString();
                    }

                }

                IterateTextBoxes(child);
            }
        }

        void RemoveText(object sender, RoutedEventArgs e)
        {
            TextBox tb = (sender as TextBox);

            if (tb.Text == tb.Tag.ToString())
            {
                tb.Text = "";
            }
        }

        void AddText(object sender, RoutedEventArgs e)
        {
            TextBox tb = (sender as TextBox);

            if (string.IsNullOrEmpty(tb.Text))
            {
                tb.Text = tb.Tag.ToString();
            }
        }
    }
}