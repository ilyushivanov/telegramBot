﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TelegramBot.Infrastructure;

#nullable disable

namespace TelegramBot.Infrastructure.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230606145627_Change_Fields_Type")]
    partial class Change_Fields_Type
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TelegramBot.Application.Entities.Isolation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Length")
                        .HasColumnType("integer");

                    b.Property<decimal>("PanelThickness")
                        .HasColumnType("numeric");

                    b.Property<decimal>("PileHeight")
                        .HasColumnType("numeric");

                    b.Property<decimal>("PlinthThickness")
                        .HasColumnType("numeric");

                    b.Property<int>("Width")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Isolation");
                });

            modelBuilder.Entity("TelegramBot.Application.Entities.Session", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<long>("ChatId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Length")
                        .HasColumnType("integer");

                    b.Property<decimal>("PileHeight")
                        .HasColumnType("numeric");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("Width")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Session");
                });
#pragma warning restore 612, 618
        }
    }
}
