using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace College_authorization_form
{
    /// <summary>
    /// Логика взаимодействия для AuthForm.xaml
    /// </summary>
    public partial class AuthForm : Window
    {
        public AuthForm()
        {
            InitializeComponent();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            string enteredLogin = loginTextBox.Text;
            string enteredPassword = passwordTextBox.Text;

            using(AppContext db = new AppContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Username == enteredLogin);
                if (user != null)
                {
                    string salt = user.Salt;
                    string saltedPassword = enteredPassword + salt;
                    string hashedPassword = HashPassword(saltedPassword);

                    if(hashedPassword == user.PasswordHash)
                    {
                        MessageBox.Show("Авторизация успешна!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        MainWindow mainWindow = new MainWindow(user.Username);
                        mainWindow.ShowDialog();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка авторизации! Проверьте логин и пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка авторизации! Пользователь не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashBytes); 
            }
        }

        /*private bool VerifyPassword(string  enteredPassword, string savedPasswordHash)
        {
            using(SHA256 sha256 = SHA256.Create())
            {
                byte[] enteredPasswordBytes = Encoding.UTF8.GetBytes(enteredPassword);
                byte[] hashBytes = sha256.ComputeHash(enteredPasswordBytes);
                string enteredPasswordHash = Convert.ToBase64String(hashBytes);

                return enteredPasswordHash == savedPasswordHash;
            }
        }*/

        private void regBtn_Click(object sender, RoutedEventArgs e)
        {
            RegFrom regFrom = new RegFrom();
            regFrom.ShowDialog();
        }
    }
}
