using System;
using System.Linq;
using System.Windows;
using System.Data.Entity;
using MasterPol.DataBaseConnect;
using MasterPol.Models;
using MasterPol.Windows;

namespace MasterPol
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AppConnect.MasterPolBD = new MasterPolEntities();
            //user
            //LoginBox.Text = "loginDEsgg2018";
            //PasswordBox.Password = "qhgYnW";
            //manager
            //LoginBox.Text = "loginDEsgg2018";
            //PasswordBox.Password = "qhgYnW";
            //admin
            LoginBox.Text = "loginDEjbz2018";
            PasswordBox.Password = "xIAWNI";
        }

        private void AuthorizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (AppConnect.MasterPolBD == null)
            {
                MessageBox.Show("Ошибка подключения к бд!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string login = LoginBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Все поля должны быть заполненными!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var user = AppConnect.MasterPolBD.Users
                    .Include(u => u.Roles)
                    .FirstOrDefault(u => u.Login == login && u.Password == password);

                if (user == null)
                {
                    MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (user.Roles == null)
                {
                    MessageBox.Show("Роль пользователя не определена. Обратитесь к администратору.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                switch (user.idRole)
                {
                    case 1:
                        MessageBox.Show($"Приветствую, {user.LastName + user.FirstName + " " + user.Patronymic }!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        App.Current.Properties["idUser"] = user.idUser;
                        new MasterPolWindow().Show();
                        this.Close();
                        break;
                    case 2:
                        MessageBox.Show($"Приветствую, {user.LastName + user.FirstName + " " + user.Patronymic}!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        App.Current.Properties["idUser"] = user.idUser;
                        new ManagerWindow().Show();
                        this.Close();
                        break;
                    case 3:
                        MessageBox.Show($"Приветствую, {user.LastName + user.FirstName + " " + user.Patronymic}!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        App.Current.Properties["idUser"] = user.idUser;
                        new AdminWindow().Show();
                        this.Close();
                        break;
                    default:
                        MessageBox.Show("Произоишла ошибка при попытке открыть окно!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка!", $"Вот она: {ex.Message}", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
