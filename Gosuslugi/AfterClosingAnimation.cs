using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Media;

namespace Gosuslugi
{
    internal class AfterClosingAnimation
    {

        internal static void Animate(Window windowToClose, Window windowToOpen)
        {
            if (windowToClose != null)
            {
                // Создаем анимацию исчезновения для первого окна
                DoubleAnimation closeAnimation = new DoubleAnimation();
                closeAnimation.From = 1.0;
                closeAnimation.To = 0.0;
                closeAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(500));
                closeAnimation.Completed += (s, e) => windowToClose.Close();

                // Применяем анимацию к первому окну
                windowToClose.BeginAnimation(Window.OpacityProperty, closeAnimation);
            }

            if (windowToOpen != null)
            {
                // Создаем анимацию появления для второго окна
                DoubleAnimation openAnimation = new DoubleAnimation();
                openAnimation.From = 0.0;
                openAnimation.To = 1.0;
                openAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(500));

                openAnimation.Completed += (s, e) => windowToOpen.Show();
                // Применяем анимацию ко второму окну
                windowToOpen.BeginAnimation(Window.OpacityProperty, openAnimation);
                // Показываем второе окно
                //windowToOpen.Show();
            }

            
        }
    }
}
