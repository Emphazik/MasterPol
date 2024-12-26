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
using MasterPol.DataBaseConnect;
using MasterPol.Models;

namespace MasterPol.Windows
{
    /// <summary>
    /// Логика взаимодействия для MasterPolWindow.xaml
    /// </summary>
    public partial class MasterPolWindow : Window
    {
        private MasterPolEntities MasterPolBD = new MasterPolEntities();
        private List<Partners> PartnersList;

        public MasterPolWindow()
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

        }
    }


}
