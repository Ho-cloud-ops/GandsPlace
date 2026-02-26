using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GantsPlace.Services;

namespace GantsPlace.Views
{
    public partial class LoginPage : Page
    {
        private readonly MainWindow _mainWindow;

        public LoginPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;

            TxtEmail.TextChanged += (s, e) => TogglePlaceholder(PlaceholderEmail, TxtEmail.Text);
            TxtPassword.PasswordChanged += (s, e) => TogglePlaceholder(PlaceholderPwd, TxtPassword.Password);
        }

        private void TogglePlaceholder(TextBlock placeholder, string text) =>
            placeholder.Visibility = string.IsNullOrEmpty(text) ? Visibility.Visible : Visibility.Collapsed;

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = TxtEmail.Text.Trim();
            string pwd = TxtPassword.Password;

            // Validation basique
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pwd))
            {
                ShowError("Veuillez remplir tous les champs.");
                return;
            }

            if (!IsValidEmail(email))
            {
                ShowError("L'email n'est pas valide.");
                return;
            }

            // Authentification
            if (DataService.Authentifier(email, pwd))
            {
                _mainWindow.RefreshAuthButtons();
                _mainWindow.NavigateTo("Accueil");
                _mainWindow.UpdateNavButtons("Accueil");
            }
            else
            {
                ShowError("Email ou mot de passe incorrect.");
            }
        }

        private void ShowError(string msg)
        {
            TxtErreur.Text = msg;
            TxtErreur.Visibility = Visibility.Visible;
        }

        private bool IsValidEmail(string email) =>
            Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        private void GoToInscription(object sender, MouseButtonEventArgs e) =>
            _mainWindow.NavigateTo("Inscription");
    }
}