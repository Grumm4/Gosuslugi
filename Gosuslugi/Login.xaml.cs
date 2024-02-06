using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Security.Cryptography;

namespace Gosuslugi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        //DbContextOptionsBuilder<ApplicationContext> optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
        
        public Login()
        {
            InitializeComponent();
        }

        public void LoginLoaded(object sender, RoutedEventArgs e)
        {
            
            using (var context = new ApplicationContext())
            {
                var user = new UserModel { Login="testLogin", HashedPwd="NotHashed", PhoneNumber="phone3114", Email="mail@mail.mail" };
                context.Users.Add(user);
                context.SaveChanges();
            }
            
        }

        private void LoginBt_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new ApplicationContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Login == LoginBox.Text && u.HashedPwd == Registration.HashPwd(PwdBox.Password));
                if (user != null)
                {
                    MessageBox.Show("Авторизация прошла успешно!");
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
    }
}