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
using MasterPol.Models;

namespace MasterPol.Windows
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private MasterPolEntities MasterPolBD = new MasterPolEntities();
        private List<Partners> PartnersList;

        public AdminWindow()
        {
            InitializeComponent();
            LoadPartners();
            LoadFilterOptions();
        }

        private void LoadPartners()
        {
            PartnersList = MasterPolBD.Partners.ToList();
            UpdatePartnersView();
        }

        private void LoadFilterOptions()
        {
            var types = MasterPolBD.TypePartners
                .Select(t => t.NameType)
                .ToList();

            FilterComboBox.Items.Clear();
            FilterComboBox.Items.Add("Все");
            foreach (var type in types)
            {
                FilterComboBox.Items.Add(type);
            }
            FilterComboBox.SelectedIndex = 0;
        }

        private void UpdatePartnersView()
        {
            string query = SearchTextbox.Text.ToLower();
            string selectedType = FilterComboBox.SelectedItem?.ToString();

            var filteredList = PartnersList.Where(p =>
                (selectedType == "Все" || p.TypePartners?.NameType == selectedType) &&
                (
                    (p.TypePartners?.NameType ?? "").ToLower().Contains(query) ||
                    (p.NamePartner ?? "").ToLower().Contains(query) ||
                    (p.Director ?? "").ToLower().Contains(query) ||
                    (p.Phone ?? "").ToLower().Contains(query)
                )
            ).ToList();

            PartnersView.ItemsSource = filteredList;
        }

        private void SearchTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePartnersView();
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePartnersView();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTextbox.Text = "";
            FilterComboBox.SelectedIndex = 0;
            LoadPartners();
        }

        private void AddPartnerButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddPartnerWindow(MasterPolBD);
            MessageBox.Show("Важно, перед добавлением товара убедительная просьба ознакомиться на примере заполнения в редактировании", "Информация!", MessageBoxButton.OK, MessageBoxImage.Information);
            if (addWindow.ShowDialog() == true)
            {
                LoadPartners();
            }
        }
        private void EditPartnerButton_Click(object sender, RoutedEventArgs e)
        {
            if (PartnersView.SelectedItem is Partners selectedPartner)
            {
                var editWindow = new AddPartnerWindow(MasterPolBD, selectedPartner);
                if (editWindow.ShowDialog() == true)
                {
                    LoadPartners();
                }
            }
            else
            {
                MessageBox.Show("Выберите партнёра для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeletePartnerButton_Click(object sender, RoutedEventArgs e)
        {
            if (PartnersView.SelectedItem is Partners selectedPartner)
            {
                var result = MessageBox.Show($"Вы точно хотите удалить данного партнёра \"{selectedPartner.NamePartner}\"?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        MasterPolBD.Partners.Remove(selectedPartner);
                        MasterPolBD.SaveChanges();

                        PartnersList.Remove(selectedPartner);
                        UpdatePartnersView();

                        MessageBox.Show($"Партнёр \"{selectedPartner.NamePartner}\" успешно удалён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            $"Ошибка при удалении партнёра: {ex.Message}",
                            "Ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }

                }
            } else
            {
                MessageBox.Show("Выберите партнёра для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
