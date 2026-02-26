using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GantsPlace.Services;

namespace GantsPlace.Views
{
    public partial class DemarragePage : Page
    {
        private readonly MainWindow _mainWindow;

        public DemarragePage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            LoadMemberPhotos();
        }

        private void LoadMemberPhotos()
        {
            // Photos des membres : membre1.jpg, membre2.jpg ... dans le dossier Images/
            var circles = new[] { Circle1, Circle2, Circle3, Circle4, Circle5 };
            for (int i = 0; i < circles.Length; i++)
            {
                var brush = ImageHelper.LoadImageBrush($"membre{i + 1}.jpg");
                if (brush != null)
                    circles[i].Background = brush;
                // Essayer aussi .png si .jpg absent
                else
                {
                    brush = ImageHelper.LoadImageBrush($"membre{i + 1}.png");
                    if (brush != null) circles[i].Background = brush;
                }
            }
        }
        
        private void BtnEntrer_Click(object sender, RoutedEventArgs e)
        {
            // Navigation vers la page d'accueil
            _mainWindow.NavigateTo("Accueil");
            _mainWindow.UpdateNavButtons("Accueil");
        }

        private void BtnInscription_Click(object sender, RoutedEventArgs e)
        {
            // Navigation vers la page d'inscription
            _mainWindow.NavigateTo("Inscription");
        }

        private void BtnConnexion_Click(object sender, RoutedEventArgs e)
        {
            // Navigation vers la page de connexion
            _mainWindow.NavigateTo("Login");
        }
    }
}
