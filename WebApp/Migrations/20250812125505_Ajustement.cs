using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class Ajustement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "HeureFin",
                table: "Horaires",
                type: "time(0)",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "HeureDebut",
                table: "Horaires",
                type: "time(0)",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "HeureFin",
                table: "Horaires",
                type: "time",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time(0)");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "HeureDebut",
                table: "Horaires",
                type: "time",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time(0)");
        }
    }
}
