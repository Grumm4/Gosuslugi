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
        TextBox tb = null;
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
        }

        void ChangePersonalData(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("ChangePersonalData");
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
            //IterateTextBoxes(this, bt.Tag);
            bt.Click -= ChangePersonalData;
            bt.Click += SavePersonalData;
        }

        void SavePersonalData(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("SavePersonalData");
            Button? bt = sender as Button;
            bt.Content = "Изменить";
            
            //сохранение в бд
            foreach (Button button in buttonList)
            {
                if (button.IsEnabled == false)
                {
                    //TextBox tb = IterateTextBoxes(this, button.Tag);
                    tb.IsEnabled = false;
                    button.IsEnabled = true;
                }
            }
            bt.Click -= SavePersonalData;
            bt.Click += ChangePersonalData;
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
    }
}
