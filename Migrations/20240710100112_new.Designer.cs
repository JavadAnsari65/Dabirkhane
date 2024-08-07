﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IliaDabirkhane.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20240710100112_new")]
    partial class @new
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Atteched", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<DateTime?>("CreateDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MessageId")
                        .HasColumnType("int");

                    b.Property<int?>("MessagesId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MessagesId");

                    b.ToTable("Attecheds_tbl");
                });

            modelBuilder.Entity("Messages", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<string>("BodyText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreateDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("SenderUserId")
                        .HasColumnType("int");

                    b.Property<string>("SerialNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SenderUserId");

                    b.ToTable("Messages_tbl");
                });

            modelBuilder.Entity("Recivers", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<DateTime?>("CreateDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("MessageId")
                        .HasColumnType("int");

                    b.Property<int?>("MessagesId")
                        .HasColumnType("int");

                    b.Property<int?>("ReciverId")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MessagesId");

                    b.HasIndex("ReciverId");

                    b.ToTable("Recivers_tbl");
                });

            modelBuilder.Entity("Users", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<string>("Addres_Sender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreateDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName_Sender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName_Sender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NatinalCode_Sender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password_Sender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PerconalCode_Sender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone_Sender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token_Sender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username_Sender")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users_tbl");
                });

            modelBuilder.Entity("Atteched", b =>
                {
                    b.HasOne("Messages", null)
                        .WithMany("Atteched")
                        .HasForeignKey("MessagesId");
                });

            modelBuilder.Entity("Messages", b =>
                {
                    b.HasOne("Users", "SenderUser")
                        .WithMany()
                        .HasForeignKey("SenderUserId");

                    b.Navigation("SenderUser");
                });

            modelBuilder.Entity("Recivers", b =>
                {
                    b.HasOne("Messages", null)
                        .WithMany("Recivers")
                        .HasForeignKey("MessagesId");

                    b.HasOne("Users", "Reciver")
                        .WithMany()
                        .HasForeignKey("ReciverId");

                    b.Navigation("Reciver");
                });

            modelBuilder.Entity("Messages", b =>
                {
                    b.Navigation("Atteched");

                    b.Navigation("Recivers");
                });
#pragma warning restore 612, 618
        }
    }
}
