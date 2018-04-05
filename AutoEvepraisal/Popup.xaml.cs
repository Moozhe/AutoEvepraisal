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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoEvepraisal
{
    public partial class Popup : Window
    {
        private const int Margin = 10;

        public string SellValue
        {
            get
            {
                return sellValue.Text;
            }
            set
            {
                sellValue.Text = value;
            }
        }

        public string BuyValue
        {
            get
            {
                return buyValue.Text;
            }
            set
            {
                buyValue.Text = value;
            }
        }

        public Popup()
        {
            InitializeComponent();

            this.Loaded += Popup_Loaded;
        }

        private void Popup_Loaded(object sender, RoutedEventArgs e)
        {
            var workArea = SystemParameters.WorkArea;
            this.Left = workArea.Right - this.Width - Margin;
            this.Top = workArea.Bottom - this.Height - Margin;
        }
    }
}
