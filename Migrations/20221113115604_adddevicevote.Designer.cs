﻿// <auto-generated />
using System;
using FeedAppApi.Proxies.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FeedAppApi.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20221113115604_adddevicevote")]
    partial class adddevicevote
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FeedAppApi.Models.Entities.Device", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("PollId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("connectionToken")
                        .HasColumnType("text");

                    b.Property<string>("hashedConnectionKey")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("salt")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("PollId");

                    b.HasIndex("UserId");

                    b.ToTable("device", (string)null);
                });

            modelBuilder.Entity("FeedAppApi.Models.Entities.DeviceVote", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DeviceId")
                        .HasColumnType("uuid");

                    b.Property<int>("Option1")
                        .HasColumnType("integer");

                    b.Property<int>("Option2")
                        .HasColumnType("integer");

                    b.Property<Guid>("PollId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.HasIndex("PollId");

                    b.ToTable("device_vote", (string)null);
                });

            modelBuilder.Entity("FeedAppApi.Models.Entities.Poll", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Access")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("OptionOne")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OptionTwo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<string>("Pincode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("StartTime")
                        .IsRequired()
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("Pincode")
                        .IsUnique();

                    b.ToTable("poll", (string)null);
                });

            modelBuilder.Entity("FeedAppApi.Models.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("text");

                    b.Property<DateTime?>("RefreshTokenExpires")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("user", (string)null);
                });

            modelBuilder.Entity("FeedAppApi.Models.Entities.Vote", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("OptionSelected")
                        .HasColumnType("integer");

                    b.Property<Guid>("PollId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PollId");

                    b.HasIndex("UserId");

                    b.ToTable("vote", (string)null);
                });

            modelBuilder.Entity("FeedAppApi.Models.Entities.Device", b =>
                {
                    b.HasOne("FeedAppApi.Models.Entities.Poll", "ConnectedPoll")
                        .WithMany()
                        .HasForeignKey("PollId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FeedAppApi.Models.Entities.User", "User")
                        .WithMany("Devices")
                        .HasForeignKey("UserId");

                    b.Navigation("ConnectedPoll");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FeedAppApi.Models.Entities.DeviceVote", b =>
                {
                    b.HasOne("FeedAppApi.Models.Entities.Device", "Device")
                        .WithMany()
                        .HasForeignKey("DeviceId");

                    b.HasOne("FeedAppApi.Models.Entities.Poll", "Poll")
                        .WithMany()
                        .HasForeignKey("PollId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");

                    b.Navigation("Poll");
                });

            modelBuilder.Entity("FeedAppApi.Models.Entities.Poll", b =>
                {
                    b.HasOne("FeedAppApi.Models.Entities.User", "Owner")
                        .WithMany("Polls")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("FeedAppApi.Models.Entities.Vote", b =>
                {
                    b.HasOne("FeedAppApi.Models.Entities.Poll", "Poll")
                        .WithMany("Votes")
                        .HasForeignKey("PollId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FeedAppApi.Models.Entities.User", "User")
                        .WithMany("Votes")
                        .HasForeignKey("UserId");

                    b.Navigation("Poll");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FeedAppApi.Models.Entities.Poll", b =>
                {
                    b.Navigation("Votes");
                });

            modelBuilder.Entity("FeedAppApi.Models.Entities.User", b =>
                {
                    b.Navigation("Devices");

                    b.Navigation("Polls");

                    b.Navigation("Votes");
                });
#pragma warning restore 612, 618
        }
    }
}
