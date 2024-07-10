﻿// <auto-generated />
using System;
using CyanBlog.DbAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CyanBlog.Migrations
{
    [DbContext(typeof(CyanBlogDbContext))]
    [Migration("20240708092043_CommentFatherIDCanBeNull")]
    partial class CommentFatherIDCanBeNull
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CyanBlog.Models.Blog", b =>
                {
                    b.Property<uint>("BlogID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned");

                    b.Property<uint>("ClassId")
                        .HasColumnType("int unsigned");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("CopyRight")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<bool>("Donate")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsComment")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsPublish")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<uint>("ViewCount")
                        .HasColumnType("int unsigned");

                    b.HasKey("BlogID");

                    b.HasIndex("ClassId");

                    b.ToTable("Blog");
                });

            modelBuilder.Entity("CyanBlog.Models.Classify", b =>
                {
                    b.Property<uint>("ClassId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("ClassId");

                    b.ToTable("Classify");
                });

            modelBuilder.Entity("CyanBlog.Models.Comment", b =>
                {
                    b.Property<uint>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned");

                    b.Property<uint>("BlogID")
                        .HasColumnType("int unsigned");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("varchar(2000)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<uint?>("FatherID")
                        .HasColumnType("int unsigned");

                    b.Property<uint>("ManagerId")
                        .HasColumnType("int unsigned");

                    b.Property<uint>("UserId")
                        .HasColumnType("int unsigned");

                    b.HasKey("CommentId");

                    b.HasIndex("BlogID");

                    b.HasIndex("FatherID");

                    b.HasIndex("UserId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("CyanBlog.Models.Friend", b =>
                {
                    b.Property<uint>("FriendId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("PictureUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("FriendId");

                    b.ToTable("Friend");
                });

            modelBuilder.Entity("CyanBlog.Models.Message", b =>
                {
                    b.Property<uint>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("varchar(2000)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<uint>("ManagerId")
                        .HasColumnType("int unsigned");

                    b.Property<uint>("UserId")
                        .HasColumnType("int unsigned");

                    b.HasKey("MessageId");

                    b.HasIndex("UserId");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("CyanBlog.Models.User", b =>
                {
                    b.Property<uint>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("HeadPictureUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("NickName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(35)
                        .HasColumnType("varchar(35)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("UserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("CyanBlog.Models.Blog", b =>
                {
                    b.HasOne("CyanBlog.Models.Classify", "Classify")
                        .WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Classify");
                });

            modelBuilder.Entity("CyanBlog.Models.Comment", b =>
                {
                    b.HasOne("CyanBlog.Models.Blog", "FatherBlog")
                        .WithMany("Comments")
                        .HasForeignKey("BlogID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CyanBlog.Models.Comment", "FatherComment")
                        .WithMany("ChildComments")
                        .HasForeignKey("FatherID");

                    b.HasOne("CyanBlog.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FatherBlog");

                    b.Navigation("FatherComment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CyanBlog.Models.Message", b =>
                {
                    b.HasOne("CyanBlog.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CyanBlog.Models.Blog", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("CyanBlog.Models.Comment", b =>
                {
                    b.Navigation("ChildComments");
                });
#pragma warning restore 612, 618
        }
    }
}
