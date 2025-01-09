using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fringaleAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id_cl = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom_cl = table.Column<string>(type: "TEXT", nullable: true),
                    Prenom_cl = table.Column<string>(type: "TEXT", nullable: true),
                    Adresse_cl = table.Column<string>(type: "TEXT", nullable: true),
                    Telephone_cl = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id_cl);
                });

            migrationBuilder.CreateTable(
                name: "Commandes",
                columns: table => new
                {
                    Id_co = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Montant_co = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commandes", x => x.Id_co);
                });

            migrationBuilder.CreateTable(
                name: "Plats",
                columns: table => new
                {
                    Id_pl = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom_pl = table.Column<string>(type: "TEXT", nullable: false),
                    Prix_pl = table.Column<double>(type: "REAL", nullable: false),
                    Categorie_pl = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plats", x => x.Id_pl);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Commandes");

            migrationBuilder.DropTable(
                name: "Plats");
        }
    }
}
