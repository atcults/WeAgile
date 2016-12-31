using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using BuildMaster.Infrastructure;

namespace BuildMaster.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20161231062343_Update Job")]
    partial class UpdateJob
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BuildMaster.Model.Configuration", b =>
                {
                    b.Property<string>("Key")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Value");

                    b.HasKey("Key");

                    b.ToTable("Configurations");
                });

            modelBuilder.Entity("BuildMaster.Model.Job", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("CheckVCS");

                    b.Property<string>("Configuration");

                    b.Property<string>("Name");

                    b.Property<string>("RootLocation");

                    b.Property<int>("TriggerTime");

                    b.HasKey("Id");

                    b.ToTable("Jobs");
                });
        }
    }
}
