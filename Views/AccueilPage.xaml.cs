using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GantsPlace.Models;
using GantsPlace.Services;

namespace GantsPlace.Views
{
    public partial class AccueilPage : Page
    {
        private readonly MainWindow _mainWindow;

        public AccueilPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            // Afficher seulement les 12 premières salles au démarrage
            LoadSalles(limit: 12);
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && tb.Text == (string)tb.Tag)
                tb.Text = "";
        }
        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && string.IsNullOrWhiteSpace(tb.Text))
                tb.Text = (string)tb.Tag;
        }

        private void LoadSalles(int? capMin = null, string? type = null,
                                 List<string>? equipVoulus = null, int limit = 0)
        {
            var salles = DataService.Salles.AsEnumerable();

            // Filtre capacité
            if (capMin.HasValue)
                salles = salles.Where(s => s.Capacite >= capMin.Value);

            // Filtre type
            if (!string.IsNullOrEmpty(type) && type != "Tous les types")
                salles = salles.Where(s => s.TypeSalle == type);

            // Limiter à 12 pour la page d'accueil si aucun filtre
            var list = limit > 0 ? salles.Take(limit).ToList() : salles.ToList();
            TxtResultats.Text = limit > 0 && capMin == null && type == null && (equipVoulus == null || equipVoulus.Count == 0)
                ? $"— {DataService.Salles.Count} salles au total"
                : $"— {list.Count} résultat(s)";

            SallesPanel.Children.Clear();
            foreach (var s in list) SallesPanel.Children.Add(BuildCard(s));
        }

        private Border BuildCard(Salle salle)
        {
            var card = new Border { Width=255, Height=195, Margin=new Thickness(8),
                CornerRadius=new CornerRadius(12), ClipToBounds=true, Cursor=Cursors.Hand };
            var grid = new Grid();

            var img = ImageHelper.LoadImageBrush(salle.ImagePath);
            grid.Children.Add(new Border { CornerRadius=new CornerRadius(12),
                Background = img != null ? (Brush)img : new SolidColorBrush(Color.FromRgb(80,80,80)) });

            var grad = new LinearGradientBrush { StartPoint=new System.Windows.Point(0,0.3), EndPoint=new System.Windows.Point(0,1) };
            grad.GradientStops.Add(new GradientStop(Colors.Transparent, 0));
            grad.GradientStops.Add(new GradientStop(Color.FromArgb(220,0,0,0), 1));
            grid.Children.Add(new Border { CornerRadius=new CornerRadius(12), Background=grad });

            var info = new StackPanel { VerticalAlignment=VerticalAlignment.Bottom, Margin=new Thickness(12) };
            info.Children.Add(new TextBlock { Text=salle.Nom,
                FontFamily=new FontFamily("Segoe UI"), FontSize=15, FontWeight=FontWeights.SemiBold,
                Foreground=Brushes.White });

            var row = new StackPanel { Orientation=Orientation.Horizontal, Margin=new Thickness(0,4,0,2) };
            var badge = new Border { Background=new SolidColorBrush(Color.FromArgb(170,30,30,30)),
                CornerRadius=new CornerRadius(8), Padding=new Thickness(7,2,7,2), Margin=new Thickness(0,0,8,0) };
            badge.Child = new TextBlock { Text=salle.TypeSalle,
                FontFamily=new FontFamily("Segoe UI"), FontSize=10, Foreground=Brushes.White };
            row.Children.Add(badge);
            row.Children.Add(new TextBlock { Text=$"👥 {salle.Capacite} pers.",
                FontFamily=new FontFamily("Segoe UI"), FontSize=11,
                Foreground=new SolidColorBrush(Color.FromRgb(210,210,210)),
                VerticalAlignment=VerticalAlignment.Center });
            info.Children.Add(row);

            // Équipements mini
            info.Children.Add(new TextBlock { Text=string.Join(" · ", salle.Equipements),
                FontFamily=new FontFamily("Segoe UI"), FontSize=10,
                Foreground=new SolidColorBrush(Color.FromRgb(180,180,180)), TextWrapping=TextWrapping.Wrap });

            grid.Children.Add(info);
            card.Child = grid;
            card.MouseLeftButtonDown += (s,e) =>
            { _mainWindow.NavigateTo("DetailSalle", salle); _mainWindow.UpdateNavButtons("Explorer"); };
            return card;
        }

        private void BtnRechercher_Click(object sender, RoutedEventArgs e)
        {
            string p = TxtPersonnes.Text == "ex: 20" ? "" : TxtPersonnes.Text;
            int? cap = int.TryParse(p, out int c) ? c : null;
            string? type = (CmbType.SelectedItem as ComboBoxItem)?.Content?.ToString();

            
            // Recherche = 20 salles ; affichage initial = 12 salles
            bool hasFilter = cap.HasValue || (type != "Tous les types") ;
            LoadSalles(cap, type);
        }
    }
}
