using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using GantsPlace.Services;

namespace GantsPlace.Views
{
    public partial class InscriptionPage : Page
    {
        private readonly MainWindow _mainWindow;

        public InscriptionPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;

            // Événements pour gérer les placeholders
            TxtNom.TextChanged += (s, e) => TogglePlaceholder(PhNom, TxtNom.Text);
            TxtEmail.TextChanged += (s, e) => TogglePlaceholder(PhEmail, TxtEmail.Text);
            TxtPassword.PasswordChanged += (s, e) => TogglePlaceholder(PhPwd, TxtPassword.Password);
            TxtConfirm.PasswordChanged += (s, e) => TogglePlaceholder(PhConfirm, TxtConfirm.Password);
        }

        private void TogglePlaceholder(TextBlock placeholder, string text) =>
            placeholder.Visibility = string.IsNullOrEmpty(text) ? Visibility.Visible : Visibility.Collapsed;

        private void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {
            TxtErreur.Visibility = Visibility.Collapsed;

            string nom = TxtNom.Text.Trim();
            string email = TxtEmail.Text.Trim();
            string pwd = TxtPassword.Password;
            string confirm = TxtConfirm.Password;

            // Validation
            if (string.IsNullOrWhiteSpace(nom)) { ShowError("Le nom est obligatoire."); return; }
            if (string.IsNullOrWhiteSpace(email)) { ShowError("L'email est obligatoire."); return; }
            if (!IsValidEmail(email)) { ShowError("L'email n'est pas valide."); return; }
            if (pwd.Length < 6) { ShowError("Le mot de passe doit contenir au moins 6 caractères."); return; }
            if (pwd != confirm) { ShowError("Les mots de passe ne correspondent pas."); return; }

            // Tentative d'inscription
            bool success = DataService.Inscrire(nom, email, pwd);
            if (!success) { ShowError("Un compte existe déjà avec cet email."); return; }

            // Succès
            _mainWindow.RefreshAuthButtons();
            MessageBox.Show(
                $"🎉 Bienvenue chez Gands Place, {nom} !\n\n" +
                $"Votre compte a été créé avec succès.\nEmail : {email}\n\nVous pouvez maintenant réserver vos salles.",
                "Compte créé ✅", MessageBoxButton.OK, MessageBoxImage.Information);

            _mainWindow.NavigateTo("Accueil");
            _mainWindow.UpdateNavButtons("Accueil");
        }

        private void ShowError(string message)
        {
            TxtErreur.Text = message;
            TxtErreur.Visibility = Visibility.Visible;
        }

        private bool IsValidEmail(string email)
        {
            // Vérification basique du format email
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private void GoToLogin(object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            _mainWindow.NavigateTo("Login");
    }
}