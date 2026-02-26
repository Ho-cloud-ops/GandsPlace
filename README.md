# 🏛️ GANDS PLACE
### Application de Réservation de Salles — Lomé, Togo

> Application de bureau Windows (WPF / .NET 8) permettant de rechercher, consulter et réserver des salles de réunion, de cours et des amphithéâtres à Lomé.

---

## 👥 Équipe de développement

| # | Nom |
|---|-----|
| 1 | **Ziad SANHONGOU** |
| 2 | **Mariam DIALLO** |
| 3 | **Honoré N'TSAKPE** |
| 4 | **Brightson GNASSOUNOU-AKPA** |
| 5 | **Emmanuel ADANDE** |

---

## 📋 Présentation

**Gands Place** est une application WPF développée en **C# / .NET 8** pour la gestion de salles événementielles à Lomé, Togo. Elle fonctionne entièrement en local, sans serveur externe — les données sont stockées dans une base de données SQLite locale.

Elle permet aux utilisateurs de :
- Parcourir un catalogue de **20 salles** classées par type
- Filtrer les salles selon le **type**, la **capacité**  souhaités
- Consulter la fiche détaillée d'une salle avec photos et équipements
- Réserver une salle en choisissant une **date**, une **heure de début** et une **heure de fin**
- Suivre l'état de ses réservations dans un **historique personnel**
- Annuler ses réservations actives

---

## 🚀 Prérequis

| Outil | Version minimale |
|-------|-----------------|
| Système d'exploitation | **Windows 10 ou supérieur** |
| [.NET SDK](https://dotnet.microsoft.com/download) | **8.0** |
| Éditeur recommandé | **VS Code** avec l'extension C# Dev Kit |

> WPF est une technologie **Windows uniquement**. L'application ne peut pas être compilée ou exécutée sur Linux ou macOS.

---

## ⚙️ Installation et lancement

### 1. Ouvrir le projet

```bash
code GandsPlace/
```

### 2. Compiler et lancer

Dans le terminal intégré VS Code (`Ctrl + ù`) :

```bash
cd GandsPlace
dotnet build
dotnet run
```

L'application s'ouvre sur la **page de démarrage** avec les photos de l'équipe.

---

## 🖼️ Gestion des images

Toutes les images doivent être placées dans le dossier `Images/` à côté du fichier `.csproj`. Ce dossier est automatiquement copié dans le dossier de sortie lors de la compilation grâce à la configuration du `.csproj`.

### Structure attendue

```
GandsPlace/
├── GantsPlace.csproj
├── Images/
│   ├── membre1.jpg        ← Ziad SANHONGOU
│   ├── membre2.jpg        ← Mariam DIALLO
│   ├── membre3.jpg        ← Honoré N'TSAKPE
│   ├── membre4.jpg        ← Brightson GNASSOUNOU-AKPA
│   ├── membre5.jpg        ← Emmanuel ADANDE
│   ├── salle1.jpg         ← photo principale salle 1
│   ├── salle1b.jpg        ← photo secondaire (détail)
│   ├── salle1c.jpg        ← photo secondaire (détail)
│   ├── salle1d.jpg        ← photo secondaire (détail)
│   └── ...                ← jusqu'à salle20.jpg
```

> Les formats `.jpg` et `.png` sont tous les deux acceptés. Si une image est absente, un fond gris s'affiche sans provoquer d'erreur.

### Comportement des images
- **Page de démarrage** : les photos `membre1.jpg` à `membre5.jpg` s'affichent dans les cercles. L'application essaie d'abord le `.jpg`, puis le `.png` si absent.
- **Cartes des salles** : photo principale (`salle1.jpg`, etc.)
- **Page détail d'une salle** : photo principale , Capacité et les equipements présents

---

## 🗄️ Base de données

La base de données SQLite est localisée dans le dossier suivant :
```
bin/Debug/net8.0-windows/ReservationSalle.db
```

Ce fichier contient toutes les données de l'application :
- Utilisateurs
- Salles
- Réservations
- Créneaux horaires
- Équipements

> **Note** : Si la base de données n'existe pas au démarrage, elle sera automatiquement créée avec les données initiales (salles, équipements, utilisateur admin).


## 📱 Pages de l'application

### Page de démarrage
- Affiche le logo **GANDS PLACE** et le nom de l'application
- Présente les **5 membres de l'équipe** avec leurs photos dans des cercles
- Boutons **Connexion** et **Inscription** accessibles directement
- Bouton **"Entrer dans l'application"** pour accéder à l'accueil

### Page Accueil
- Affiche les **12 premières salles** par défaut sous forme de cartes
- Barre de recherche multicritères avec :
  - Champ **nombre de personnes** minimum
  - Sélecteur **type de salle**
- La recherche parcourt les **20 salles** et affiche tous les résultats correspondants
- Chaque carte affiche : photo, nom, type, capacité, équipements

### Page Explorer
- Affiche les **20 salles** disponibles
- Filtres en temps réel (mis à jour automatiquement) :
  - **Type de salle** : Tous / Salle de réunion / Salle de cours / Amphithéâtre
  - **Capacité** : Toutes / Moins de 20 / 20 à 50 / Plus de 50 personnes
- Compteur de résultats affiché en temps réel

### Page Détail d'une salle
- Photo principale grande
- Nom, type de salle, capacité maximum
- Description complète
- Équipements affichés sous forme de **badges colorés** (bleu, violet, vert, orange)
- Formulaire de réservation (si connecté) :
  - Sélecteur de date
  - Heure de début et heure de fin (7h à 22h)
  - Message de confirmation en vert après envoi
  - Message d'erreur en rouge si les heures sont invalides
- Si non connecté : message avec bouton "Se connecter"

### Page Historique
- Affiche toutes les réservations de l'utilisateur connecté
- Chaque carte affiche : nom de la salle, type, date, créneau horaire
- **Statuts visuels** :
  - *En attente* : aucun badge (réservation en cours de traitement)
  - ✅ *Confirmée* : badge vert
  - ❌ *Annulée* : badge rouge
- Bouton **Annuler** disponible pour les réservations actives (Confirmée)
- Boîte de dialogue de confirmation avant annulation

### Page Contact
- Informations de contact de l'établissement (email, téléphone, adresse, horaires)
- Formulaire de contact : Nom, Email, Sujet (liste déroulante), Message
- Message de succès affiché après envoi
- Validation : tous les champs obligatoires

### Page Connexion
- Champs Email et Mot de passe avec placeholders animés
- Message d'erreur si les identifiants sont incorrects
- Lien vers la page d'inscription

### Page Inscription
- Champs : Nom complet, Email, Mot de passe, Confirmation du mot de passe
- Validations :
  - Nom obligatoire
  - Email obligatoire
  - Mot de passe minimum 6 caractères
  - Les deux mots de passe doivent correspondre
  - Email unique (refus si déjà existant)
- Message de bienvenue affiché après création du compte


## 🗂️ Architecture du projet

```
GandsPlace/
├── GandsPlace.csproj                  # Configuration .NET 8 / WPF
│                                      # Copie automatique du dossier Images/
├── App.xaml / App.xaml.cs             # Point d'entrée, ressources globales
├── MainWindow.xaml / .cs              # Fenêtre principale + barre de navigation
│
├── Models/
│   └── Models.cs                      # Salle, Reservation, Utilisateur, Session
│
├── Services/
│   ├── DataService.cs                 # Base SQLite, auth, réservations
│   └── ImageHelper.cs                 # Chargement sécurisé des images
│
├── Styles/
│   └── AppStyles.xaml                 # Thème sombre, styles partagés
│
├── ViewModels/                        # (non utilisé - MVVM léger)
│
└── Views/
    ├── DemarragePage.xaml / .cs      # Page de démarrage — équipe + boutons entrée
    ├── AccueilPage.xaml / .cs        # 12 salles + recherche multicritères
    ├── ExplorerPage.xaml / .cs       # 20 salles + filtres type/capacité/équipements
    ├── DetailSallePage.xaml / .cs   # Fiche salle + formulaire réservation
    ├── HistoriquePage.xaml / .cs     # Historique utilisateur + annulation
    ├── LoginPage.xaml / .cs          # Connexion
    ├── InscriptionPage.xaml / .cs    # Création de compte
    └── ContactPage.xaml / .cs        # Formulaire contact + infos établissement
```

---

## 🔄 Flux de réservation

```
1. L'utilisateur choisit une salle (Accueil/Explorer)
2. L'utilisateur remplit le formulaire (date + heures)
3. L'utilisateur clique "Enregistrer la réservation"
        │
        ▼
   Statut : "Confirmée" (badge vert)
        │
        ▼
   L'utilisateur peut annuler sa réservation depuis l'historique
```

---

## 🛠️ Technologies utilisées

| Technologie | Usage |
|-------------|-------|
| **C# 12** | Langage principal |
| **.NET 8** | Framework d'exécution |
| **WPF** | Interface graphique Windows |
| **XAML** | Définition des interfaces utilisateur |
| **SQLite** | Base de données locale |
| **Microsoft.Data.Sqlite** | Fournisseur SQLite pour .NET |

---

## 📞 Contact Gands Place

| | |
|--|--|
| 📧 Email | contact@gantplace.com |
| 📞 Téléphone | +228 96 47 07 52 / +229 52 52 69 95 |
| 📍 Adresse | Lomé — TOGO |
| 🕐 Horaires | Lun-Ven : 8h00-20h00  •  Sam : 9h00-18h00 |

---

*Projet réalisé dans le cadre d'un cours de développement logiciel — Lomé, Togo 🇹🇬*
