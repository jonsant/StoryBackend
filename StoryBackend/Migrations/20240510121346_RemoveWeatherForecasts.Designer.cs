﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StoryBackend.Database;

#nullable disable

namespace StoryBackend.Migrations
{
    [DbContext(typeof(StoryDbContext))]
    [Migration("20240510121346_RemoveWeatherForecasts")]
    partial class RemoveWeatherForecasts
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.4");
#pragma warning restore 612, 618
        }
    }
}
