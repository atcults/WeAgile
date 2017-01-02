using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BuildMaster.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configurations",
                columns: table => new
                {
                    Key = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configurations", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CheckVCS = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    RootLocation = table.Column<string>(nullable: true),
                    TriggerTime = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobQueues",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FinishTime = table.Column<DateTime>(nullable: true),
                    JobId = table.Column<long>(nullable: false),
                    JobStatus = table.Column<int>(nullable: false),
                    QueuedTime = table.Column<DateTime>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobQueues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobQueues_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobTasks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CommandAruments = table.Column<string>(nullable: true),
                    CommandName = table.Column<string>(nullable: false),
                    JobId = table.Column<long>(nullable: true),
                    RelativePath = table.Column<string>(nullable: true),
                    TaskName = table.Column<string>(nullable: false),
                    TaskOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobTasks_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobQueueTaskResults",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CommandAruments = table.Column<string>(nullable: true),
                    CommandName = table.Column<string>(nullable: false),
                    ExitCode = table.Column<int>(nullable: true),
                    FinishTime = table.Column<DateTime>(nullable: true),
                    JobQueueId = table.Column<long>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: true),
                    TaskName = table.Column<string>(nullable: true),
                    TaskOrder = table.Column<int>(nullable: false),
                    WorkingDirectory = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobQueueTaskResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobQueueTaskResults_JobQueues_JobQueueId",
                        column: x => x.JobQueueId,
                        principalTable: "JobQueues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobQueues_JobId",
                table: "JobQueues",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobQueueTaskResults_JobQueueId",
                table: "JobQueueTaskResults",
                column: "JobQueueId");

            migrationBuilder.CreateIndex(
                name: "IX_JobTasks_JobId",
                table: "JobTasks",
                column: "JobId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configurations");

            migrationBuilder.DropTable(
                name: "JobQueueTaskResults");

            migrationBuilder.DropTable(
                name: "JobTasks");

            migrationBuilder.DropTable(
                name: "JobQueues");

            migrationBuilder.DropTable(
                name: "Jobs");
        }
    }
}
