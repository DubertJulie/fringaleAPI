﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace fringaleAPI.Migrations
{
    [DbContext(typeof(FringaleAPIDb))]
    partial class FringaleAPIDbModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("fringaleAPI.Client", b =>
                {
                    b.Property<int>("Id_cl")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Adresse_cl")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nom_cl")
                        .HasColumnType("TEXT");

                    b.Property<string>("Prenom_cl")
                        .HasColumnType("TEXT");

                    b.Property<string>("Telephone_cl")
                        .HasColumnType("TEXT");

                    b.HasKey("Id_cl");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("fringaleAPI.Commande", b =>
                {
                    b.Property<int>("Id_co")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date_co")
                        .HasColumnType("TEXT");

                    b.Property<int>("Id_cl")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Montant_co")
                        .HasColumnType("REAL");

                    b.HasKey("Id_co");

                    b.ToTable("Commandes");
                });

            modelBuilder.Entity("fringaleAPI.Plat", b =>
                {
                    b.Property<int>("Id_pl")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Categorie_pl")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nom_pl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Prix_pl")
                        .HasColumnType("REAL");

                    b.HasKey("Id_pl");

                    b.ToTable("Plats");
                });

            modelBuilder.Entity("fringaleAPI.PlatParCommande", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CommandeId_co")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Id_co")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Id_pl")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PlatId_pl")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CommandeId_co");

                    b.HasIndex("PlatId_pl");

                    b.ToTable("PlatParCommande");
                });

            modelBuilder.Entity("fringaleAPI.PlatParCommande", b =>
                {
                    b.HasOne("fringaleAPI.Commande", null)
                        .WithMany("PlatParCommandes")
                        .HasForeignKey("CommandeId_co");

                    b.HasOne("fringaleAPI.Plat", null)
                        .WithMany("PlatParCommandes")
                        .HasForeignKey("PlatId_pl");
                });

            modelBuilder.Entity("fringaleAPI.Commande", b =>
                {
                    b.Navigation("PlatParCommandes");
                });

            modelBuilder.Entity("fringaleAPI.Plat", b =>
                {
                    b.Navigation("PlatParCommandes");
                });
#pragma warning restore 612, 618
        }
    }
}
