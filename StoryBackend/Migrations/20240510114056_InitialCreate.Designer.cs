﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StoryBackend.Database;

#nullable disable

namespace StoryBackend.Migrations
{
    [DbContext(typeof(StoryDbContext))]
    [Migration("20240510114056_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.4");

            modelBuilder.Entity("WeatherForecast", b =>
                {
                    b.Property<int>("WeatherForecastId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Summary")
                        .HasColumnType("TEXT");

                    b.Property<int>("TemperatureC")
                        .HasColumnType("INTEGER");

                    b.HasKey("WeatherForecastId");

                    b.ToTable("WeatherForecasts");
                });
#pragma warning restore 612, 618
        }
    }
}