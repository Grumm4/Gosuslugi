using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Security.Cryptography;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Gosuslugi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        public static UserModel? currentUser;

        public Login()
        {
            InitializeComponent();
        }
        public void LoginLoaded(object sender, RoutedEventArgs e)
        {
            //
            //AfterClosingAnimation.Animate(null, this);
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
                        Name = user.Name,
                        HashedPwd = user.HashedPwd,
                        Login = user.Login,
                        PhoneNumber = user.PhoneNumber,
                        Role = user.Role
                    };

                    AfterClosingAnimation.Animate(this, new OrderWindow());
                    //LoginBox.Text = string.Empty;
                    //PwdBox.Password = string.Empty;


                    //OrderWindow ow = new OrderWindow();

                    //ow.Show();
                    //this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин/пароль");
                }
            }
        }

        private void BtToReg_Click(object sender, RoutedEventArgs e)
        {
            AfterClosingAnimation.Animate(this, new Registration());
            //var r = new Registration();
            //r.Show();
            //this.Close();
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
            TextBox? tb = (sender as TextBox);

            if (tb?.Text == tb?.Tag.ToString())
            {
                tb.Text = "";
            }
        }

        void AddText(object sender, RoutedEventArgs e)
        {
            TextBox? tb = (sender as TextBox);

            if (string.IsNullOrEmpty(tb?.Text))
            {
                tb.Text = tb.Tag.ToString();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //AfterClosingAnimation.Animate(this, null);
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

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}