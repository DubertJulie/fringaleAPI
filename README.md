
# Fringale API - Gestion des Commandes pour Restaurateurs

Bienvenue dans l'API Fringale, une solution de gestion de commandes pour les restaurateurs. Cette API permet de gérer les clients, les commandes et les plats via des opérations CRUD (Créer, Lire, Mettre à jour, Supprimer).

## Prérequis

- .NET 6 ou version supérieure
- SQLite pour la base de données
- Swagger pour la documentation de l'API

## Fonctionnalités

### 1. Gestion des Clients
- **GET /clients**: Récupère tous les clients
- **GET /clients/{id}**: Récupère un client par son identifiant
- **POST /clients**: Crée un nouveau client
- **PUT /clients/{id}**: Modifie les informations d'un client
- **DELETE /clients/{id}**: Supprime un client

### 2. Gestion des Commandes
- **GET /commandes**: Récupère toutes les commandes
- **GET /commandes/{id}**: Récupère une commande par son identifiant
- **GET /commandes/date/{date_co}**: Récupère des commandes par leur date
- **POST /commandes**: Crée une nouvelle commande
- **PUT /commandes/{id}**: Modifie une commande
- **DELETE /commandes/{id}**: Supprime une commande

### 3. Gestion des Plats
- **GET /plats**: Récupère tous les plats
- **GET /plats/{id}**: Récupère un plat par son identifiant
- **POST /plats**: Crée un nouveau plat
- **PUT /plats/{id}**: Modifie un plat
- **DELETE /plats/{id}**: Supprime un plat

## Installation

1. Clonez le projet sur votre machine locale.
   
   git clone https://github.com/votre-repository/FringaleAPI.git
   cd FringaleAPI

2. Assurez-vous que vous avez .NET 6 ou version supérieure installé sur votre machine.
Vous pouvez vérifier la version avec cette commande:

   dotnet --version

3. Installez les dépendances avec la commande suivante:

   dotnet restore

4.  Créez et appliquez la base de données avec cette commande
  
    dotnet ef database update 

5. Lancez l'API:
 
   dotnet run
   

# Structure de la base de données

L'API utilise SQLite comme base de données. Elle comporte trois tables principales :
- **Clients**: Contient les informations des clients (nom, prénom, adresse, téléphone).
- **Commandes**: Contient les informations des commandes (montant, date, référence client).
- **Plats**:Contient les informations des plats (nom, prix, catégorie).

# Initialisation de la Base de Données
Lorsque l'application est lancée pour la première fois, la base de données est initialisée avec des données par défaut à l'aide d'un fichier JSON. Vous pouvez ajouter de nouvelles données ou les modifier via l'API.

# Contact Github
DubertJulie,Zélie Lemahieu,kevin2759
