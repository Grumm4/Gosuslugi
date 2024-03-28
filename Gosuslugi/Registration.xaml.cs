using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Security.Cryptography;
using System.Reflection;
using System.Windows.Controls;

namespace Gosuslugi
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        bool FieldsIsNull = false;
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
                Email = (TbEmail.Text != TbEmail.Tag) ? TbEmail.Text : null,
                PhoneNumber = (TbPhone.Text != TbPhone.Tag) ? TbPhone.Text : null,
                Login = (TbLogin.Text != TbLogin.Tag) ? TbLogin.Text : null,
                HashedPwd = (TbPass.Password != string.Empty) ? HashPwd(TbPass.Password) : null,
                Name = "",
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


                Type myType = typeof(UserModel);
                FieldInfo[] fields = myType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

                foreach (FieldInfo field in fields)
                {
                    object value = field.GetValue(u);
                    if (value == null) 
                    {
                        FieldsIsNull = true;
                    }
                }
                

                if (!FieldsIsNull)
                {
                    MessageBox.Show("Успешная регистрация! Для входа нажмите кнопку 'Вход'");
                    context.Users.Add(u);
                    context.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Заполните все поля!");
                    FieldsIsNull = false;
                }
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
            AfterClosingAnimation.Animate(this, new Login());
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

        public void RemoveText(object sender, RoutedEventArgs e)
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
            AfterClosingAnimation.Animate(null, this);
            IterateTextBoxes(this);
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
