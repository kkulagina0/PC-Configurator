using ConfigCore;
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

namespace projectInd
{
    /// <summary>
    /// Interaction logic for SelectBuildWindow.xaml
    /// </summary>
    public partial class SelectBuildWindow : Window
    {
        public Build SelectedBuild { get; private set; }
        public SelectBuildWindow(List<Build> builds)
        {
            InitializeComponent();
            BuildComboBox.ItemsSource = builds;
        }
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            SelectedBuild = BuildComboBox.SelectedItem as Build;
            if (SelectedBuild == null)
            {
                MessageBox.Show("Выберите сборку.");
                return;
            }
            DialogResult = true;
            Close();
        }
    }
}
