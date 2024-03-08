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
    }
}
