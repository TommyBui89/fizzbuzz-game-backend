﻿// <auto-generated />
using FizzBuzzGameBackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace fizzbuzz_game_backend.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.3");

            modelBuilder.Entity("FizzBuzzGameBackend.Models.GameRule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Divisor")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GameSessionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ReplacementText")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GameSessionId");

                    b.ToTable("GameRules");
                });

            modelBuilder.Entity("FizzBuzzGameBackend.Models.GameSession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Duration")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("GameSessions");
                });

            modelBuilder.Entity("FizzBuzzGameBackend.Models.GameRule", b =>
                {
                    b.HasOne("FizzBuzzGameBackend.Models.GameSession", "GameSession")
                        .WithMany("Rules")
                        .HasForeignKey("GameSessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GameSession");
                });

            modelBuilder.Entity("FizzBuzzGameBackend.Models.GameSession", b =>
                {
                    b.Navigation("Rules");
                });
#pragma warning restore 612, 618
        }
    }
}
