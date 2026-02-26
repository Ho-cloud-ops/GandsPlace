using System.Windows;
using System.Windows.Controls;

namespace GantsPlace.Views
{
    public partial class ContactPage : Page
    {
        private readonly MainWindow _mainWindow;

        public ContactPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void BtnEnvoyer_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNom.Text) ||
                string.IsNullOrWhiteSpace(TxtEmail.Text) ||
                string.IsNullOrWhiteSpace(TxtMessage.Text))
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.",
                                "Champs manquants", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Simulate sending
            TxtNom.Text = "";
            TxtEmail.Text = "";
            TxtMessage.Text = "";
            TxtSucces.Visibility = Visibility.Visible;
        }
    }
}
