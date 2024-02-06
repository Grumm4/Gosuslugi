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
        //public DbContextOptionsBuilder<ApplicationContext> context;

        public Registration()
        {
            InitializeComponent();
        }

        //public Registration(DbContextOptionsBuilder<ApplicationContext> context) => this.context = context;

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
                HashedPwd = HashPwd(TbPass.Password)
            };

            using (var context = new ApplicationContext())
            {
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
    }
}
