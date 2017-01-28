using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DL_Core_WebAPP_Release.Models;

namespace DL_Core_WebAPP_Release.Migrations
{
    [DbContext(typeof(DLContext))]
    [Migration("20160628123139_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DL_Using_DotNet_Core.Models.Book", b =>
                {
                    b.Property<int>("BookId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<int>("AvailableCopies");

                    b.Property<string>("Category");

                    b.Property<string>("ISBN");

                    b.Property<string>("Title");

                    b.Property<int>("TotalCopies");

                    b.HasKey("BookId");

                    b.ToTable("Books");
                });
        }
    }
}
