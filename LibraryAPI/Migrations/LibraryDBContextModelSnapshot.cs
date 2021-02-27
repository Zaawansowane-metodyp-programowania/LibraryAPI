﻿// <auto-generated />
using System;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LibraryAPI.Migrations
{
    [DbContext(typeof(LibraryDBContext))]
    partial class LibraryDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("LibraryAPI.Models.Books", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("ISBN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("actual_stock")
                        .HasColumnType("int");

                    b.Property<string>("author_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("book_description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("book_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("current_stock")
                        .HasColumnType("int");

                    b.Property<string>("language")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("publish_date")
                        .HasColumnType("int");

                    b.Property<string>("publisher_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("LibraryAPI.Models.BooksUsers", b =>
                {
                    b.Property<int>("BooksId")
                        .HasColumnType("int");

                    b.Property<int>("UsersId")
                        .HasColumnType("int");

                    b.Property<DateTime>("due_date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("issue_date")
                        .HasColumnType("datetime2");

                    b.HasKey("BooksId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("BooksUsers");
                });

            modelBuilder.Entity("LibraryAPI.Models.Users", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("authorization")
                        .HasColumnType("int");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("LibraryAPI.Models.BooksUsers", b =>
                {
                    b.HasOne("LibraryAPI.Models.Books", "Books")
                        .WithMany("Users")
                        .HasForeignKey("BooksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LibraryAPI.Models.Users", "Users")
                        .WithMany("Books")
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Books");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("LibraryAPI.Models.Books", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("LibraryAPI.Models.Users", b =>
                {
                    b.Navigation("Books");
                });
#pragma warning restore 612, 618
        }
    }
}
