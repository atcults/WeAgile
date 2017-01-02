using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using BuildMaster.Infrastructure;

namespace BuildMaster.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("Name");

                    b.Property<string>("RootLocation");

                    b.Property<int>("TriggerTime");

                    b.HasKey("Id");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("BuildMaster.Model.JobTask", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CommandAruments");

                    b.Property<string>("CommandName")
                        .IsRequired();

                    b.Property<long?>("JobdRefId");

                    b.Property<string>("RelativePath");

                    b.Property<string>("TaskName")
                        .IsRequired();

                    b.Property<int>("TaskOrder");

                    b.HasKey("Id");

                    b.HasIndex("JobdRefId");

                    b.ToTable("JobTasks");
                });

            modelBuilder.Entity("BuildMaster.Model.JobTask", b =>
                {
                    b.HasOne("BuildMaster.Model.Job", "Job")
                        .WithMany("JobTasks")
                        .HasForeignKey("JobdRefId");
                });
        }
    }
}
