﻿// <auto-generated />
using System;
using Infrastructure.Persistence.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230618190158_AddDeviceType")]
    partial class AddDeviceType
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.ActionRule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ComparisonType")
                        .HasColumnType("integer");

                    b.Property<string>("ComparisonValue")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsSchedule")
                        .HasColumnType("boolean");

                    b.Property<string>("MeasureProperty")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("OutletAction")
                        .HasColumnType("integer");

                    b.Property<int>("OutletConfigurationId")
                        .HasColumnType("integer");

                    b.Property<int?>("ScheduledTimeId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("OutletConfigurationId");

                    b.HasIndex("ScheduledTimeId");

                    b.ToTable("ActionRules");
                });

            modelBuilder.Entity("Domain.Entities.Device", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("Domain.Entities.EnvironmentMeasure", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<double>("GroundHumidity")
                        .HasColumnType("double precision");

                    b.Property<double>("Humidity")
                        .HasColumnType("double precision");

                    b.Property<double>("Pressure")
                        .HasColumnType("double precision");

                    b.Property<double>("Temperature")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Measures");
                });

            modelBuilder.Entity("Domain.Entities.Measurement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Result")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("SensorId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("SensorId");

                    b.ToTable("Measurements");
                });

            modelBuilder.Entity("Domain.Entities.OutletConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("OutletId")
                        .HasColumnType("integer");

                    b.Property<int>("PinId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("OutletId")
                        .IsUnique();

                    b.ToTable("OutletConfigurations");
                });

            modelBuilder.Entity("Domain.Entities.ScheduledTime", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Hour")
                        .HasColumnType("integer");

                    b.Property<int>("Minute")
                        .HasColumnType("integer");

                    b.Property<int>("Second")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("ScheduledTime");
                });

            modelBuilder.Entity("Domain.Entities.Sensor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("DeviceId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.ToTable("Sensors");
                });

            modelBuilder.Entity("Domain.Entities.TakenAction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ActionRuleId")
                        .HasColumnType("integer");

                    b.Property<int>("EnvironmentMeasureId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("ActionRuleId");

                    b.HasIndex("EnvironmentMeasureId");

                    b.ToTable("TakenActions");
                });

            modelBuilder.Entity("Domain.Entities.ActionRule", b =>
                {
                    b.HasOne("Domain.Entities.OutletConfiguration", "Outlet")
                        .WithMany()
                        .HasForeignKey("OutletConfigurationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.ScheduledTime", "ScheduledTime")
                        .WithMany()
                        .HasForeignKey("ScheduledTimeId");

                    b.Navigation("Outlet");

                    b.Navigation("ScheduledTime");
                });

            modelBuilder.Entity("Domain.Entities.Measurement", b =>
                {
                    b.HasOne("Domain.Entities.Sensor", "Sensor")
                        .WithMany("Measurements")
                        .HasForeignKey("SensorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sensor");
                });

            modelBuilder.Entity("Domain.Entities.Sensor", b =>
                {
                    b.HasOne("Domain.Entities.Device", "Device")
                        .WithMany("Sensors")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");
                });

            modelBuilder.Entity("Domain.Entities.TakenAction", b =>
                {
                    b.HasOne("Domain.Entities.ActionRule", "ActionRule")
                        .WithMany()
                        .HasForeignKey("ActionRuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.EnvironmentMeasure", "EnvironmentMeasure")
                        .WithMany()
                        .HasForeignKey("EnvironmentMeasureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ActionRule");

                    b.Navigation("EnvironmentMeasure");
                });

            modelBuilder.Entity("Domain.Entities.Device", b =>
                {
                    b.Navigation("Sensors");
                });

            modelBuilder.Entity("Domain.Entities.Sensor", b =>
                {
                    b.Navigation("Measurements");
                });
#pragma warning restore 612, 618
        }
    }
}