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
    /// Логика взаимодействия для CreateOrderWindow.xaml
    /// </summary>
    public partial class CreateOrderWindow : Window
    {
        public CreateOrderWindow()
        {
            InitializeComponent();
        }

        private void CreateOrderBt_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new ApplicationContext())
            {
                context.Orders.Add(new OrderModel
                {
                    Title = TitleTb.Text,
                    Price = Convert.ToDecimal(PriceTb.Text),
                    Date = DateDp.Text,
                    Place = PlaceTb.Text,
                    Contacts = ContactsTb.Text,
                    Comments = CommentsTb.Text,
                    AcceptedUserId = 0
                });
                context.SaveChanges();
            }
            AfterClosingAnimation.Animate(this, null);
            DialogResult = true;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AfterClosingAnimation.Animate(null, this);
            IterateTextBoxes(this);
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AfterClosingAnimation.Animate(this, null);
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
            AfterClosingAnimation.Animate(this, new OrderWindow());
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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
