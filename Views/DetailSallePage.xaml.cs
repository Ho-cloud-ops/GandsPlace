using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GantsPlace.Models;
using GantsPlace.Services;

namespace GantsPlace.Views
{
    public partial class DetailSallePage : Page
    {
        private readonly MainWindow _mainWindow;
        private readonly Salle _salle;
        private bool _saving = false;

        public DetailSallePage(MainWindow mainWindow, Salle salle)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _salle = salle;
            LoadData();
            SetupHeures();
            UpdateConnexion();
        }

        private void LoadData()
        {
            TxtNomSalle.Text    = _salle.Nom;
            TxtType.Text        = _salle.TypeSalle;
            TxtCapacite.Text    = _salle.Capacite.ToString();
            TxtDescription.Text = _salle.Description;
            DpDate.SelectedDate = DateTime.Today.AddDays(1);

            // Photos
            var main = ImageHelper.LoadImageBrush(_salle.ImagePath);
            if (main != null) MainPhoto.Background = main;
            string base_ = System.IO.Path.GetFileNameWithoutExtension(_salle.ImagePath ?? "");
            string ext   = System.IO.Path.GetExtension(_salle.ImagePath ?? ".jpg");
            var b2 = ImageHelper.LoadImageBrush(base_ + "b" + ext);
            var b3 = ImageHelper.LoadImageBrush(base_ + "c" + ext);
            var b4 = ImageHelper.LoadImageBrush(base_ + "d" + ext);
           
            // Équipements : badges colorés
            string[] colors = { "#4A90D9", "#7B68EE", "#48C774", "#E8A020" };
            string[] icons  = { "📽️", "🖥️", "📶", "🎤" };
            for (int i = 0; i < _salle.Equipements.Count; i++)
            {
                string color = colors[i % colors.Length];
                string icon  = _salle.Equipements[i] switch {
                    "Projecteur" => "📽️", "Écran" => "🖥️", "Wi-Fi" => "📶", "Micro" => "🎤", _ => "✅"
                };
                var badge = new Border
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color)),
                    CornerRadius = new CornerRadius(20),
                    Padding = new Thickness(14, 7, 14, 7),
                    Margin = new Thickness(0, 0, 8, 8)
                };
                badge.Child = new TextBlock
                {
                    Text = $"{icon} {_salle.Equipements[i]}",
                    FontFamily = new FontFamily("Segoe UI"),
                    FontSize = 13, Foreground = Brushes.White
                };
                EquipementsPanel.Children.Add(badge);
            }
        }

        private void SetupHeures()
        {
            for (int h = 7; h <= 22; h++)
            { CmbHeureDebut.Items.Add($"{h:00}:00"); CmbHeureFin.Items.Add($"{h:00}:00"); }
            CmbHeureDebut.SelectedIndex = 2;
            CmbHeureFin.SelectedIndex   = 4;
        }

        private void UpdateConnexion()
        {
            PanelNonConnecte.Visibility = Session.EstConnecte ? Visibility.Collapsed : Visibility.Visible;
            PanelFormulaire.Visibility  = Session.EstConnecte ? Visibility.Visible   : Visibility.Collapsed;
        }

        private void Champ_Changed(object sender, EventArgs e) { /* live update si besoin */ }

        private void BtnReserver_Click(object sender, RoutedEventArgs e)
        {
            if (!Session.EstConnecte)
            {
                _mainWindow.NavigateTo("Login");
                return;
            }

            if (DpDate.SelectedDate == null)
            {
                ShowMsg("⚠️ Sélectionnez une date.", "#FF6B6B");
                SuccessIndicator.Visibility = Visibility.Collapsed;
                return;
            }

            int debut = CmbHeureDebut.SelectedIndex + 7;
            int fin = CmbHeureFin.SelectedIndex + 7;

            if (fin <= debut)
            {
                ShowMsg("⚠️ L'heure de fin doit être après le début.", "#FF6B6B");
                SuccessIndicator.Visibility = Visibility.Collapsed;
                return;
            }

            string heureDebut = $"{debut:00}:00";
            string heureFin = $"{fin:00}:00";

            // 1. Créer ou récupérer le creneau
            int idCreneau = DataService.GetOrCreateCreneau(heureDebut, heureFin);

            // 2. Ajouter réservation
            DataService.AjouterReservation(
                _salle.Id,
                idCreneau,
                DpDate.SelectedDate.Value.ToString("yyyy-MM-dd")
            );

            // Show success indicator
            SuccessIndicator.Visibility = Visibility.Visible;
            TxtMessage.Visibility = Visibility.Collapsed;
            
            // Disable button temporarily
            BtnReserver.IsEnabled = false;
            BtnReserver.Content = "✓ Réservé !";
            
            // Recharger les réservations de l'utilisateur
            if (Session.UtilisateurConnecte != null)
            {
                DataService.LoadUserReservations(Session.UtilisateurConnecte);
            }

            // Auto-navigate to historique after a short delay
            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1.5)
            };
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                _mainWindow.NavigateTo("Historique");
                _mainWindow.UpdateNavButtons("Historique");
            };
            timer.Start();
        }
       

        private void ShowMsg(string msg, string hex)
        {
            TxtMessage.Text = msg;
            TxtMessage.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
            TxtMessage.Visibility = Visibility.Visible;
        }

        private void BtnGoLogin_Click(object sender, RoutedEventArgs e) => _mainWindow.NavigateTo("Login");

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        { _mainWindow.NavigateTo("Explorer"); _mainWindow.UpdateNavButtons("Explorer"); }
    }
}
