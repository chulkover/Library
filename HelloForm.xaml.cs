using System;
using System.Collections.Generic;
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

namespace Library
{
    /// <summary>
    /// Логика взаимодействия для HelloForm.xaml
    /// </summary>
    public partial class HelloForm : Window
    {
        private int _Seconds;
        public HelloForm()
        {
            InitializeComponent();
            _Seconds = 10;
            LabelTimer.Content = "Закрытия окна через " + _Seconds.ToString() + "...";
            var task = MyTimeFunction();
        }
        async private Task MyTimeFunction()
        {
            while (true)
            {
                await Task.Delay(1000);
                _Seconds--;
                if (_Seconds == 0)
                {
                    this.Close();
                }
                LabelTimer.Content = "Закрытия окна через " + _Seconds.ToString() + "...";
            }

        }
    }
}
