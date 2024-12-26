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
    /// Логика взаимодействия для EditPartnerWindow.xaml
    /// </summary>
    public partial class EditPartnerWindow : Window
    {
        private MasterPolEntities MasterPolBD;
        private Partners CurrentPartner;
        public EditPartnerWindow(MasterPolEntities dbContext, Partners partner = null)
        {
            InitializeComponent();
            MasterPolBD = dbContext;
            CurrentPartner = partner;

            LoadTypeComboBox();

            if (CurrentPartner != null)
            {
                NameTextBox.Text = CurrentPartner.NamePartner;
                TypeComboBox.SelectedValue = CurrentPartner.TypePartner;
                RatingTextBox.Text = CurrentPartner.Rating.ToString();
                AddressTextBox.Text = CurrentPartner.Address;
                DirectorTextBox.Text = CurrentPartner.Director;
                PhoneTextBox.Text = CurrentPartner.Phone;
                EmailTextBox.Text = CurrentPartner.Email;
            }
        }

        private void LoadTypeComboBox()
        {
            TypeComboBox.ItemsSource = MasterPolBD.TypePartners.ToList();
            TypeComboBox.DisplayMemberPath = "NameType";
            TypeComboBox.SelectedValuePath = "idType";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                    !int.TryParse(RatingTextBox.Text, out int rating) || rating < 0)
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

                CurrentPartner.NamePartner = NameTextBox.Text;
                CurrentPartner.TypePartner = (int)TypeComboBox.SelectedValue;
                CurrentPartner.Rating = rating;
                CurrentPartner.Address = AddressTextBox.Text;
                CurrentPartner.Director = DirectorTextBox.Text;
                CurrentPartner.Phone = PhoneTextBox.Text;
                CurrentPartner.Email = EmailTextBox.Text;

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
