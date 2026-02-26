using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using GantsPlace.Models;
using GantsPlace.Services;
using GantsPlace.Views;

namespace GantsPlace
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ShowDemarrage();
        }

        public void ShowDemarrage()
        {
            NavigateToPage(new DemarragePage(this));
            TxtUserName.Visibility = Visibility.Collapsed;
            BtnDeconnexion.Visibility = Visibility.Collapsed;
        }

        public void ShowMainApp()
        {
            NavigateTo("Accueil");
            UpdateNavButtons("Accueil");
        }

        public void NavigateTo(string pageName, object? param = null)
        {
            Page? page = pageName switch
            {
                "Accueil" => new AccueilPage(this),
                "Explorer" => new ExplorerPage(this),
                "Historique" => new HistoriquePage(this),
                "Contact" => new ContactPage(this),
                "Login" => new LoginPage(this),
                "Inscription" => new InscriptionPage(this),
                "DetailSalle" => param is Salle s ? new DetailSallePage(this, s) : null,
                _ => null
            };

            if (page != null)
                NavigateToPage(page);
        }

        private void NavigateToPage(Page page)
        {
            // Simple fade transition
            MainFrame.Navigate(page);
        }

        public void UpdateNavButtons(string activePage)
        {
            BtnAccueil.FontWeight = activePage == "Accueil" ? FontWeights.Bold : FontWeights.Normal;
            BtnExplorer.FontWeight = activePage == "Explorer" ? FontWeights.Bold : FontWeights.Normal;
            BtnHistorique.FontWeight = activePage == "Historique" ? FontWeights.Bold : FontWeights.Normal;
            BtnContact.FontWeight = activePage == "Contact" ? FontWeights.Bold : FontWeights.Normal;
        }

        public void RefreshAuthButtons()
        {
            if (Session.EstConnecte)
            {
                TxtUserName.Text = Session.UtilisateurConnecte?.NomComplet ?? "";
                TxtUserName.Visibility = Visibility.Visible;
                BtnDeconnexion.Visibility = Visibility.Visible;
            }
            else
            {
                TxtUserName.Visibility = Visibility.Collapsed;
                BtnDeconnexion.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnAccueil_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo("Accueil");
            UpdateNavButtons("Accueil");
        }

        private void BtnExplorer_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo("Explorer");
            UpdateNavButtons("Explorer");
        }

        private void BtnHistorique_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo("Historique");
            UpdateNavButtons("Historique");
        }

        private void BtnContact_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo("Contact");
            UpdateNavButtons("Contact");
        }

        private void BtnConnexion_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo("Login");
            UpdateNavButtons("");
        }

        private void BtnInscription_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo("Inscription");
            UpdateNavButtons("");
        }

        private void BtnDeconnexion_Click(object sender, RoutedEventArgs e)
        {
            Session.UtilisateurConnecte = null;
            ShowDemarrage();
        }
    }
}
