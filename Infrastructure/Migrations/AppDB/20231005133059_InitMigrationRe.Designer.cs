﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations.AppDB
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20231005133059_InitMigrationRe")]
    partial class InitMigrationRe
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Models.Canteen", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("City")
                        .HasColumnType("int");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("OffersHotMeals")
                        .HasColumnType("bit");

                    b.HasKey("ID");

                    b.ToTable("Canteens");
                });

            modelBuilder.Entity("Domain.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployeeID"));

                    b.Property<int>("CanteenID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EmployeeID");

                    b.HasIndex("CanteenID");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Domain.Models.Packet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CanteenID")
                        .HasColumnType("int");

                    b.Property<int>("City")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("MaxDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("OverEighteen")
                        .HasColumnType("bit");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<int>("ReservedById")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CanteenID");

                    b.HasIndex("ReservedById");

                    b.ToTable("Packets");
                });

            modelBuilder.Entity("Domain.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Alcohol")
                        .HasColumnType("bit");

                    b.Property<byte[]>("ImageData")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PacketId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PacketId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Domain.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("City")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StudentNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Students");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            City = 0,
                            DateOfBirth = new DateTime(2000, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "john.doe@example.com",
                            Name = "John Doe",
                            Phone = "555-555-5555",
                            StudentNumber = 12345
                        },
                        new
                        {
                            Id = 2,
                            City = 2,
                            DateOfBirth = new DateTime(1998, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "jane.smith@example.com",
                            Name = "Jane Smith",
                            Phone = "555-555-1234",
                            StudentNumber = 67890
                        });
                });

            modelBuilder.Entity("Domain.Models.Employee", b =>
                {
                    b.HasOne("Domain.Models.Canteen", "Canteen")
                        .WithMany()
                        .HasForeignKey("CanteenID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Canteen");
                });

            modelBuilder.Entity("Domain.Models.Packet", b =>
                {
                    b.HasOne("Domain.Models.Canteen", "Canteen")
                        .WithMany()
                        .HasForeignKey("CanteenID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Models.Student", "ReservedBy")
                        .WithMany()
                        .HasForeignKey("ReservedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Canteen");

                    b.Navigation("ReservedBy");
                });

            modelBuilder.Entity("Domain.Models.Product", b =>
                {
                    b.HasOne("Domain.Models.Packet", null)
                        .WithMany("Products")
                        .HasForeignKey("PacketId");
                });

            modelBuilder.Entity("Domain.Models.Packet", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
