using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BuildMaster.Migrations
{
    public partial class UpdateJob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CheckVCS",
                table: "Jobs",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TriggerTime",
                table: "Jobs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckVCS",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "TriggerTime",
                table: "Jobs");
        }
    }
}
