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
using ConfigData;
using ConfigCore;
using System.Reflection;
using System.ComponentModel;
using Microsoft.Win32;

namespace projectInd
{
    public partial class MainWindow : Window
    {

        private Build currentBuild = new Build();
        private Builds currentBuilds = new Builds();
        private List<CPU> allCpu;
        private List<GPU> allGpu;
        private List<RAM> allRam;
        private List<Motherboard> allMotherboar;
        private List<PowerSupply> allPowerSupply;
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
            UpdateComboBoxes();

        }

        private void LoadData()
        {
            allCpu = DataLoader.LoadCpu();
            allGpu = DataLoader.LoadGpu();
            allRam = DataLoader.LoadRAM();
            allMotherboar = DataLoader.LoadMotherboardu();
            allPowerSupply = DataLoader.LoadPowerSupply();
        }
        private void UpdateComboBoxes()
        {
            CpuComboBox.ItemsSource = allCpu;
            GpuComboBox.ItemsSource = allGpu;
            RamComboBox.ItemsSource = allRam;
            MbComboBox.ItemsSource = allMotherboar;
            PsuComboBox.ItemsSource = allPowerSupply;
        }

        private void ComponentChanged(object sender, SelectionChangedEventArgs e)
        {
            currentBuild.cpu = CpuComboBox.SelectedItem as CPU;
            currentBuild.gpu = GpuComboBox.SelectedItem as GPU;
            currentBuild.ram = RamComboBox.SelectedItem as RAM;
            currentBuild.motherboard = MbComboBox.SelectedItem as Motherboard;
            currentBuild.powerSupply = PsuComboBox.SelectedItem as PowerSupply;
        }
        private void SaveAllBuilds_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog{ Filter = "Только текстовые|*.txt", InitialDirectory = System.IO.Path.GetFullPath(@"..\..\..\")};
            if (dialog.ShowDialog() == true) {
                currentBuilds.SaveToFile(dialog.FileName);
                MessageBox.Show("Сборки сохранены.");
            }
            else
                MessageBox.Show("Файл не такой.");
        }
        public void LoadAllBuilds_Click(object sender, RoutedEventArgs e) {
            var dialog = new OpenFileDialog { Filter = "Только текстовые|*.txt", InitialDirectory = System.IO.Path.GetFullPath(@"..\..\..\")};
            if (dialog.ShowDialog() == true)
            {
                currentBuilds.LoadFromFile(dialog.FileName, allCpu, allGpu, allRam, allMotherboar, allPowerSupply);
                MessageBox.Show("Сборки сохранены (если они были).");
            }
            else
                MessageBox.Show("Файл не такой.");
        }
        public void ShowBuilds_Click(object sender, RoutedEventArgs e)
        {
            var window = new SelectBuildWindow(currentBuilds.All());
            window.ShowDialog();
            var build = window.SelectedBuild;
            if (build == null)
                return;
            BuildInfoTextBlock.Text = build.BeautyString();
            TipsTextBlock.Text = build.Tips();
            RatingTextBlock.Text = Analizer.AnalyzeCurrentComponente(build.cpu, allCpu) + "\n"+Analizer.AnalyzeCurrentComponente(build.gpu, allGpu)
                + "\n" + Analizer.AnalyzeCurrentComponente(build.motherboard, allMotherboar) + "\n" + Analizer.AnalyzeCurrentComponente(build.ram, allRam)
                + "\n" + Analizer.AnalyzeCurrentComponente(build.powerSupply, allPowerSupply);

        }
        private void CreateGaming_Click(object sender, RoutedEventArgs e)
        {
            BuildReady br = new BuildReady(allCpu, allGpu, allRam, allMotherboar, allPowerSupply);
            Build b = br.CreateBestBuild();
            if (b == null)
                MessageBox.Show("Извините но что-то не так");
            else
            {
                MessageBox.Show("Cборка удалась, добавляем ее в список готовых сборок", b.ToString());
                currentBuilds.AddBuild(b);
            }
        }
        private void CreateBudget_Click(object sender, EventArgs e)
        {
            string input = NumberInput.Text;
            if (int.TryParse(input, out int number))
            {
                BuildReady br = new BuildReady(allCpu, allGpu, allRam, allMotherboar, allPowerSupply);
                Build b = br.CreateByBudget(number);
                if (b == null)
                    MessageBox.Show("Извините но по вашему бюджету собрать идеальную сброку не получилось");
                else
                {
                    MessageBox.Show("Cборка удалась, добавляем ее в список готовых сборок", b.ToString());
                    currentBuilds.AddBuild(b);
                }

            }
            else
            {
                MessageBox.Show("Ошибка: введите корректное целое число.");
            }
        }
        private void CompareBuilds_Click(object sender, RoutedEventArgs e)
        {
            
            var window = new SelectBuildWindow(currentBuilds.All());
            window.ShowDialog();
            var build = window.SelectedBuild;

            var result = Build.Compare(currentBuild, build);
            if (result == "")
            {
                MessageBox.Show("Что-то не так");
            }
            else { MessageBox.Show(result, "Результат сравнения"); }
        }

        private void ShowAllBuilds_Click(Object sender, RoutedEventArgs e)
        {
            if (currentBuilds.Count() == 0)
            {
                MessageBox.Show("Нет сохранённых сборок.");
                return;
            }
            else
            {
                string s = "";
                for(int i = 0; i < currentBuilds.Count(); i++)
                    s += currentBuilds[i].ToString();
                MessageBox.Show(s, "Список всех сборок");
            }

        }
        private void CheckCompatibility_Click(object sender, RoutedEventArgs e)
        {
            if ((currentBuild.Check())=="")
            {
                MessageBox.Show("Cборка удалась, добавляем ее в список готовых сборок", currentBuild.ToString());
                currentBuilds.AddBuild(currentBuild);
                currentBuild = new Build();
                CpuComboBox.SelectedItem = null;
                GpuComboBox.SelectedItem = null;
                RamComboBox.SelectedItem = null;
                MbComboBox.SelectedItem = null;
                PsuComboBox.SelectedItem = null;

            }
            else
                MessageBox.Show(currentBuild.Check());
        }
        private void SortComponentList<T>(ComboBox comboBoxToUpdate, List<T> sourceList, string selected) where T : ConfigCore.IComponent
        {
            switch (selected)
            {
                case "Цена ↑":
                    comboBoxToUpdate.ItemsSource = sourceList.OrderBy(c => c.Price).ToList();
                    break;
                case "Цена ↓":
                    comboBoxToUpdate.ItemsSource = sourceList.OrderByDescending(c => c.Price).ToList();
                    break;
                case "Производительность":
                    comboBoxToUpdate.ItemsSource = sourceList.OrderByDescending(c => c.GetPerfomanceScore()).ToList();
                    break;
                case "Название":
                    comboBoxToUpdate.ItemsSource = sourceList.OrderBy(c => c.Name).ToList();
                    break;
                default:
                    comboBoxToUpdate.ItemsSource = sourceList;
                    break;
            }
        }

        private void CpuSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = ((ComboBoxItem)((ComboBox)sender).SelectedItem)?.Content?.ToString();
            if (selected == null) return;

            SortComponentList(CpuComboBox, allCpu, selected);
        }

        private void GpuSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = ((ComboBoxItem)((ComboBox)sender).SelectedItem)?.Content?.ToString();
            if (selected == null) return;

            SortComponentList(GpuComboBox, allGpu, selected);
        }

        private void RamSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = ((ComboBoxItem)((ComboBox)sender).SelectedItem)?.Content?.ToString();
            if (selected == null) return;

            SortComponentList(RamComboBox, allRam, selected);
        }

        private void MbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = ((ComboBoxItem)((ComboBox)sender).SelectedItem)?.Content?.ToString();
            if (selected == null) return;

            SortComponentList(MbComboBox, allMotherboar, selected);
        }

        private void PsSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = ((ComboBoxItem)((ComboBox)sender).SelectedItem)?.Content?.ToString();
            if (selected == null) return;

            SortComponentList(PsuComboBox, allPowerSupply, selected);
        }
    }
}
