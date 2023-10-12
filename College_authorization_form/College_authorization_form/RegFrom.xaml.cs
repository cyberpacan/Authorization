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
    /// Логика взаимодействия для RegFrom.xaml
    /// </summary>
    public partial class RegFrom : Window
    {
        public RegFrom()
        {
            InitializeComponent();
        }

        private void signinBtn_Click(object sender, RoutedEventArgs e)
        {
            string newUsername = loginTextBox.Text;
            string newPassword = passwordTextBox.Text; 

            using(AppContext db = new AppContext())
            {
                var existingUser = db.Users.FirstOrDefault(u => u.Username == newUsername);
                if (existingUser == null)
                {
                    string salt = GenerateSalt();
                    string saltedPassword = newPassword + salt;
                    string hashedPassword = HashPassword(saltedPassword);

                    var newUser = new User
                    {
                        Username = newUsername,
                        PasswordHash = hashedPassword,
                        Salt = salt
                    };

                    db.Users.Add(newUser);
                    db.SaveChanges();
                    MessageBox.Show("Регистрация успешна!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                else
                {
                    MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private string GenerateSalt()
        {
            const int saltLength = 16;
            byte[] saltBytes = new byte[saltLength];
            using(RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
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
    }
}
