using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using GantsPlace.Models;
using GantsPlace.Services;

namespace GantsPlace.Views
{
    public partial class HistoriquePage : Page
    {
        private readonly MainWindow _mainWindow;

        public HistoriquePage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            LoadReservations();
        }

        private void LoadReservations()
        {
            ListeReservations.Children.Clear();

            if (!Session.EstConnecte)
            {
                AfficherMessageConnexion();
                return;
            }

            var user = Session.UtilisateurConnecte!;
            TxtSoustitre.Text = $"Bienvenue, {user.NomComplet} — {user.Reservations.Count} réservation(s)";

            if (user.Reservations.Count == 0)
            {
                ListeReservations.Children.Add(new TextBlock
                {
                    Text = "Aucune réservation pour le moment.",
                    FontFamily = new FontFamily("Segoe UI"),
                    FontSize = 15,
                    Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170)),
                    Margin = new Thickness(0, 20, 0, 0)
                });
                return;
            }

            foreach (var reservation in user.Reservations)
            {
                ListeReservations.Children.Add(CreateReservationCard(reservation));
            }
        }

        private void AfficherMessageConnexion()
        {
            TxtSoustitre.Text = "Connectez-vous pour voir vos réservations.";

            var btnConnexion = new Button
            {
                Content = "Se connecter",
                Margin = new Thickness(0, 20, 0, 0),
                Padding = new Thickness(24, 12, 24, 12),
                Background = new SolidColorBrush(Color.FromRgb(26, 26, 26)),
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                Cursor = System.Windows.Input.Cursors.Hand,
                FontFamily = new FontFamily("Segoe UI"),
                FontSize = 14
            };
            btnConnexion.Click += (s, e) => _mainWindow.NavigateTo("Login");

            ListeReservations.Children.Add(btnConnexion);
        }

        private Border CreateReservationCard(Reservation r)
        {
            var card = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(55, 55, 55)),
                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(20),
                Margin = new Thickness(0, 0, 0, 12),
                Effect = new DropShadowEffect { ShadowDepth = 2, BlurRadius = 8, Opacity = 0.3, Color = Colors.Black }
            };

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) }); // Infos
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) }); // Date/heure
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // Badge statut
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // Bouton

            // Infos salle
            var leftPanel = new StackPanel();
            leftPanel.Children.Add(new TextBlock
            {
                Text = r.SalleNom,
                FontFamily = new FontFamily("Segoe UI"),
                FontSize = 17,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White,
                Margin = new Thickness(0, 0, 0, 4)
            });
            leftPanel.Children.Add(new TextBlock
            {
                Text = r.TypeSalle,
                FontFamily = new FontFamily("Segoe UI"),
                FontSize = 13,
                Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170))
            });
            Grid.SetColumn(leftPanel, 0);

            // Date et heures
            var centerPanel = new StackPanel { VerticalAlignment = VerticalAlignment.Center };
            centerPanel.Children.Add(new TextBlock
            {
                Text = $"📅 {r.DateFormatee}",
                FontFamily = new FontFamily("Segoe UI"),
                FontSize = 14,
                Foreground = Brushes.White,
                Margin = new Thickness(0, 0, 0, 4)
            });
            centerPanel.Children.Add(new TextBlock
            {
                Text = $"🕐 {r.HeuresFormatees}",
                FontFamily = new FontFamily("Segoe UI"),
                FontSize = 13,
                Foreground = new SolidColorBrush(Color.FromRgb(200, 200, 200))
            });
            Grid.SetColumn(centerPanel, 1);

            // Badge statut
            var badgeColor = r.Statut switch
            {
                "Confirmée" => Color.FromRgb(72, 199, 116),
                "Annulee" => Color.FromRgb(220, 80, 80),
                "Annulée" => Color.FromRgb(220, 80, 80),
                _ => Color.FromRgb(120, 120, 120)
            };
            var badge = new Border
            {
                Background = new SolidColorBrush(badgeColor),
                CornerRadius = new CornerRadius(20),
                Padding = new Thickness(14, 6, 14, 6),
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(12, 0, 0, 0)
            };
            badge.Child = new TextBlock
            {
                Text = r.Statut,
                FontFamily = new FontFamily("Segoe UI"),
                FontSize = 12,
                FontWeight = FontWeights.SemiBold,
                Foreground = Brushes.White
            };
            Grid.SetColumn(badge, 2);

            // Bouton Annuler
            Button? btnAnnuler = null;
            if (r.Statut == "Confirmée")
            {
                btnAnnuler = new Button
                {
                    Content = "Annuler",
                    Margin = new Thickness(10, 0, 0, 0),
                    Padding = new Thickness(14, 6, 14, 6),
                    Background = new SolidColorBrush(Color.FromRgb(60, 60, 60)),
                    Foreground = new SolidColorBrush(Color.FromRgb(220, 80, 80)),
                    BorderThickness = new Thickness(1),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(220, 80, 80)),
                    FontFamily = new FontFamily("Segoe UI"),
                    FontSize = 12,
                    FontWeight = FontWeights.SemiBold,
                    Cursor = System.Windows.Input.Cursors.Hand,
                    VerticalAlignment = VerticalAlignment.Center
                };
                var reservationCopy = r;
                btnAnnuler.Click += (s, e) =>
                {
                    var result = MessageBox.Show($"Annuler la réservation de « {reservationCopy.SalleNom} » du {reservationCopy.DateFormatee} ?",
                        "Confirmer l'annulation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        DataService.AnnulerReservation(reservationCopy);
                        LoadReservations();
                    }
                };
                Grid.SetColumn(btnAnnuler, 3);
            }

            // Ajout au grid
            grid.Children.Add(leftPanel);
            grid.Children.Add(centerPanel);
            grid.Children.Add(badge);
            if (btnAnnuler != null) grid.Children.Add(btnAnnuler);

            card.Child = grid;
            return card;
        }
    }
}