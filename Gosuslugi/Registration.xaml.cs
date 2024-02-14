using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
using System.Security.Cryptography;

namespace Gosuslugi
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void RegistrBt_Click(object sender, RoutedEventArgs e)
        {
            Registr();
        }
        
        void Registr()
        {
            var u = new UserModel
            {
                Email = TbEmail.Text,
                PhoneNumber = TbPhone.Text,
                Login = TbLogin.Text,
                HashedPwd = HashPwd(TbPass.Password),
                Role = "User"
            };

            using (var context = new ApplicationContext())
            {
                if ((context.Users.FirstOrDefault(u => u.Email == TbEmail.Text)) != null)
                {
                    MessageBox.Show("Пользователь с такой почтой уже существует!");
                    return;
                }
                if ((context.Users.FirstOrDefault(u => u.Login == TbLogin.Text)) != null)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует!");
                    return;
                }
                context.Users.Add(u);
                context.SaveChanges();
            }
        }

        internal static string HashPwd(string password)
        {

            // Создание экземпляра класса SHA256Managed
            using (SHA256Managed sha256 = new SHA256Managed())
            {
                // Преобразование пароля в массив байтов
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Вычисление хеша пароля
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                // Преобразование хеша в строку
                string hashedPassword = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                return hashedPassword;
            }
        }

        private void BtToLogin_Click(object sender, RoutedEventArgs e)
        {
            var l = new Login();
            l.Show();
            this.Close();
        }

        public void IterateTextBoxes(DependencyObject parent)
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IterateTextBoxes(this);
        }
    }
}
