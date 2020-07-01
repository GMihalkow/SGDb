﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SGDb.Statistics.Data;

namespace SGDb.Statistics.Data.Migrations
{
    [DbContext(typeof(StatisticsDbContext))]
    partial class StatisticsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("SGDb.Statistics.Data.Models.GameDetailsView", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned");

                    b.Property<uint>("GameId")
                        .HasColumnType("int unsigned");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("GameDetailsViews");
                });

            modelBuilder.Entity("SGDb.Statistics.Data.Models.Statistics", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned");

                    b.Property<uint>("TotalGamesCount")
                        .HasColumnType("int unsigned");

                    b.Property<uint>("TotalGenresCount")
                        .HasColumnType("int unsigned");

                    b.Property<uint>("TotalPublishersCount")
                        .HasColumnType("int unsigned");

                    b.HasKey("Id");

                    b.ToTable("Statistics");
                });
#pragma warning restore 612, 618
        }
    }
}