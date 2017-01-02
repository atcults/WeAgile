using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using BuildMaster.Infrastructure;
using BuildMaster.Model;

namespace BuildMaster.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170102135424_Initial")]
    partial class Initial
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

                    b.Property<string>("Name");

                    b.Property<string>("RootLocation");

                    b.Property<int>("TriggerTime");

                    b.HasKey("Id");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("BuildMaster.Model.JobQueue", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("FinishTime");

                    b.Property<long>("JobId");

                    b.Property<int>("JobStatus");

                    b.Property<DateTime>("QueuedTime");

                    b.Property<DateTime?>("StartTime");

                    b.HasKey("Id");

                    b.HasIndex("JobId");

                    b.ToTable("JobQueues");
                });

            modelBuilder.Entity("BuildMaster.Model.JobQueueTaskResult", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CommandAruments");

                    b.Property<string>("CommandName")
                        .IsRequired();

                    b.Property<int?>("ExitCode");

                    b.Property<DateTime?>("FinishTime");

                    b.Property<long?>("JobQueueId");

                    b.Property<DateTime?>("StartTime");

                    b.Property<string>("TaskName");

                    b.Property<int>("TaskOrder");

                    b.Property<string>("WorkingDirectory");

                    b.HasKey("Id");

                    b.HasIndex("JobQueueId");

                    b.ToTable("JobQueueTaskResults");
                });

            modelBuilder.Entity("BuildMaster.Model.JobTask", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CommandAruments");

                    b.Property<string>("CommandName")
                        .IsRequired();

                    b.Property<long?>("JobId");

                    b.Property<string>("RelativePath");

                    b.Property<string>("TaskName")
                        .IsRequired();

                    b.Property<int>("TaskOrder");

                    b.HasKey("Id");

                    b.HasIndex("JobId");

                    b.ToTable("JobTasks");
                });

            modelBuilder.Entity("BuildMaster.Model.JobQueue", b =>
                {
                    b.HasOne("BuildMaster.Model.Job", "Job")
                        .WithMany("JobQueues")
                        .HasForeignKey("JobId");
                });

            modelBuilder.Entity("BuildMaster.Model.JobQueueTaskResult", b =>
                {
                    b.HasOne("BuildMaster.Model.JobQueue", "JobQueue")
                        .WithMany()
                        .HasForeignKey("JobQueueId");
                });

            modelBuilder.Entity("BuildMaster.Model.JobTask", b =>
                {
                    b.HasOne("BuildMaster.Model.Job", "Job")
                        .WithMany("JobTasks")
                        .HasForeignKey("JobId");
                });
        }
    }
}
