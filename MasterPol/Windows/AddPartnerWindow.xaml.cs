using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для AddPartnerWindow.xaml
    /// </summary>
    public partial class AddPartnerWindow : Window
    {
        private MasterPolEntities MasterPolBD;
        private Partners CurrentPartner;

        public AddPartnerWindow(MasterPolEntities dbContext, Partners partner = null)
        {
            InitializeComponent();
            MasterPolBD = dbContext;
            CurrentPartner = partner;

            LoadTypeComboBox();

            if (CurrentPartner != null)
            {
                ExistingPartnersComboBox.SelectedValue = CurrentPartner.NamePartner;
                TypeComboBox.SelectedValue = CurrentPartner.TypePartner;
                RatingTextBox.Text = CurrentPartner.Rating.ToString();
                AddressTextBox.Text = CurrentPartner.Address;
                PhoneTextBox.Text = CurrentPartner.Phone;
                EmailTextBox.Text = CurrentPartner.Email;
                INNTextBox.Text = CurrentPartner.INN;
                LogoComboBox.SelectedItem = CurrentPartner.Logo;
                DirectorComboBox.SelectedItem = CurrentPartner.Director;
            }
        }


        private void LoadTypeComboBox()
        {
            TypeComboBox.ItemsSource = MasterPolBD.TypePartners.ToList();
            TypeComboBox.DisplayMemberPath = "NameType";
            TypeComboBox.SelectedValuePath = "idType";

            LogoComboBox.ItemsSource = new List<string>
            {
                "basaStroit.png",
                "mont.png",
                "parket.png",
                "picture.png"
            };

            ExistingPartnersComboBox.ItemsSource = MasterPolBD.Partners
                .Select(p => p.NamePartner)
                .Distinct()
                .ToList();

            DirectorComboBox.ItemsSource = MasterPolBD.Partners
                .Select(p => p.Director)
                .Distinct()
                .ToList();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(RatingTextBox.Text, out int rating) || rating < 0)
                {
                    MessageBox.Show("Введите корректные данные!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (TypeComboBox.SelectedValue == null)
                {
                    MessageBox.Show("Выберите тип партнёра!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!Regex.IsMatch(EmailTextBox.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    MessageBox.Show("Введите корректный Email.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (CurrentPartner == null)
                {
                    CurrentPartner = new Partners();
                    MasterPolBD.Partners.Add(CurrentPartner);
                }

                CurrentPartner.TypePartner = (int)TypeComboBox.SelectedValue;
                CurrentPartner.Rating = rating;
                CurrentPartner.Address = AddressTextBox.Text;
                CurrentPartner.Phone = PhoneTextBox.Text;
                CurrentPartner.Email = EmailTextBox.Text;
                CurrentPartner.INN = INNTextBox.Text;
                CurrentPartner.NamePartner = ExistingPartnersComboBox.SelectedItem?.ToString();
                CurrentPartner.Director = DirectorComboBox.SelectedItem?.ToString();
                CurrentPartner.Logo = LogoComboBox.SelectedItem?.ToString();

                MasterPolBD.SaveChanges();
                DialogResult = true;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => $"Свойство: {x.PropertyName}, Ошибка: {x.ErrorMessage}");

                var fullErrorMessage = string.Join("\n", errorMessages);

                MessageBox.Show($"Ошибки валидации данных:\n{fullErrorMessage}", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
