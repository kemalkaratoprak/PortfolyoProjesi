using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolyoProjesi.Migrations
{
    /// <inheritdoc />
    public partial class AddDateAndReadStatusToMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ContactMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "ContactMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ContactMessages");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "ContactMessages");
        }
    }
}
