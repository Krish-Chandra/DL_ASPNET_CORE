using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DL_Core_WebAPP_Release.Models;

namespace DL_Core_WebAPP_Release.Migrations
{
    [DbContext(typeof(DLContext))]
    [Migration("20160720082015_AssIssue")]
    partial class AssIssue
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DL_Core_WebAPP_Release.Models.Author", b =>
                {
                    b.Property<int>("AuthorID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("City");

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<string>("Phone");

                    b.Property<string>("PinCode");

                    b.Property<string>("State");

                    b.HasKey("AuthorID");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("DL_Core_WebAPP_Release.Models.Book", b =>
                {
                    b.Property<int>("BookId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AuthorId");

                    b.Property<int>("AvailableCopies");

                    b.Property<int>("CategoryId");

                    b.Property<string>("ISBN");

                    b.Property<int>("PublisherId");

                    b.Property<string>("Title");

                    b.Property<int>("TotalCopies");

                    b.HasKey("BookId");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("PublisherId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("DL_Core_WebAPP_Release.Models.Category", b =>
                {
                    b.Property<int>("CategoryID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("CategoryID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("DL_Core_WebAPP_Release.Models.Issue", b =>
                {
                    b.Property<int>("IssueID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BookId");

                    b.Property<string>("DueOn");

                    b.Property<string>("IssuedOn");

                    b.Property<string>("UserId");

                    b.HasKey("IssueID");

                    b.HasIndex("BookId");

                    b.ToTable("Issues");
                });

            modelBuilder.Entity("DL_Core_WebAPP_Release.Models.Publisher", b =>
                {
                    b.Property<int>("PublisherID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("City");

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<string>("Phone");

                    b.Property<string>("PinCode");

                    b.Property<string>("State");

                    b.HasKey("PublisherID");

                    b.ToTable("Publishers");
                });

            modelBuilder.Entity("DL_Core_WebAPP_Release.Models.Request", b =>
                {
                    b.Property<int>("RequestID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BookId");

                    b.Property<string>("RequesedOn");

                    b.Property<string>("UserId")
                        .HasAnnotation("MaxLength", 450);

                    b.HasKey("RequestID");

                    b.HasIndex("BookId");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("DL_Core_WebAPP_Release.Models.Book", b =>
                {
                    b.HasOne("DL_Core_WebAPP_Release.Models.Author", "Author")
                        .WithMany("Books")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DL_Core_WebAPP_Release.Models.Category", "Category")
                        .WithMany("Books")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DL_Core_WebAPP_Release.Models.Publisher", "Publisher")
                        .WithMany("Books")
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DL_Core_WebAPP_Release.Models.Issue", b =>
                {
                    b.HasOne("DL_Core_WebAPP_Release.Models.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DL_Core_WebAPP_Release.Models.Request", b =>
                {
                    b.HasOne("DL_Core_WebAPP_Release.Models.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
