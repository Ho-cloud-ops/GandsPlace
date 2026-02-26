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
    public partial class ExplorerPage : Page
    {
        private readonly MainWindow _mainWindow;

        public ExplorerPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            LoadSalles();
        }

        private void LoadSalles()
        {
            var salles = DataService.GetSalles().AsEnumerable();

            // Filtre type
            string type = (CmbFiltreType?.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Tous les types";
            if (type != "Tous les types") salles = salles.Where(s => s.TypeSalle == type);

            // Filtre capacité
            string cap = (CmbFiltreCapacite?.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Toutes capacités";
            if (cap == "Moins de 20 personnes")     salles = salles.Where(s => s.Capacite < 20);
            else if (cap == "20 à 50 personnes")    salles = salles.Where(s => s.Capacite >= 20 && s.Capacite <= 50);
            else if (cap == "Plus de 50 personnes") salles = salles.Where(s => s.Capacite > 50);

            // Tri
            string tri = (CmbTri?.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Nom (A-Z)";
            salles = tri switch
            {
                "Nom (A-Z)" => salles.OrderBy(s => s.Nom),
                "Nom (Z-A)" => salles.OrderByDescending(s => s.Nom),
                "Capacité ↑" => salles.OrderBy(s => s.Capacite),
                "Capacité ↓" => salles.OrderByDescending(s => s.Capacite),
                _ => salles.OrderBy(s => s.Nom)
            };

            var list = salles.ToList();
            if (TxtResultats != null) TxtResultats.Text = $"{list.Count} salle(s)";
            SallesPanel.Children.Clear();
            foreach (var s in list) SallesPanel.Children.Add(BuildCard(s));
        }

        private Border BuildCard(Salle salle)
        {
            var card = new Border { Width=215, Height=270, Margin=new Thickness(8),
                CornerRadius=new CornerRadius(12), ClipToBounds=true, Cursor=Cursors.Hand };
            var grid = new Grid();

            var img = ImageHelper.LoadImageBrush(salle.ImagePath);
            grid.Children.Add(new Border { CornerRadius=new CornerRadius(12),
                Background = img != null ? (Brush)img : new SolidColorBrush(Color.FromRgb(80,80,80)) });

            var grad = new LinearGradientBrush { StartPoint=new System.Windows.Point(0,0.3), EndPoint=new System.Windows.Point(0,1) };
            grad.GradientStops.Add(new GradientStop(Colors.Transparent, 0));
            grad.GradientStops.Add(new GradientStop(Color.FromArgb(235,0,0,0), 1));
            grid.Children.Add(new Border { CornerRadius=new CornerRadius(12), Background=grad });

            var info = new StackPanel { VerticalAlignment=VerticalAlignment.Bottom, Margin=new Thickness(10) };
            info.Children.Add(new TextBlock { Text=salle.Nom,
                FontFamily=new FontFamily("Segoe UI"), FontSize=14, FontWeight=FontWeights.Bold,
                Foreground=Brushes.White, TextWrapping=TextWrapping.Wrap });

            var badge = new Border { Background=new SolidColorBrush(Color.FromArgb(190,30,30,30)),
                CornerRadius=new CornerRadius(8), Padding=new Thickness(8,3,8,3),
                Margin=new Thickness(0,4,0,4), HorizontalAlignment=HorizontalAlignment.Left };
            badge.Child = new TextBlock { Text=salle.TypeSalle,
                FontFamily=new FontFamily("Segoe UI"), FontSize=10, Foreground=Brushes.White };
            info.Children.Add(badge);

            info.Children.Add(new TextBlock { Text=$"👥 {salle.Capacite} personnes max.",
                FontFamily=new FontFamily("Segoe UI"), FontSize=11,
                Foreground=new SolidColorBrush(Color.FromRgb(210,210,210)), Margin=new Thickness(0,0,0,2) });

            info.Children.Add(new TextBlock { Text=string.Join(" · ", salle.Equipements),
                FontFamily=new FontFamily("Segoe UI"), FontSize=10,
                Foreground=new SolidColorBrush(Color.FromRgb(175,175,175)), TextWrapping=TextWrapping.Wrap });

            grid.Children.Add(info);
            card.Child = grid;
            card.MouseLeftButtonDown += (s,e) =>
            { _mainWindow.NavigateTo("DetailSalle", salle); _mainWindow.UpdateNavButtons("Explorer"); };
            return card;
        }

        private void FiltreChanged(object sender, System.Windows.RoutedEventArgs e)
        { if (SallesPanel != null) LoadSalles(); }
    }
}
