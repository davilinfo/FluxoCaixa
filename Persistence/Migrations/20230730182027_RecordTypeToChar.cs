using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RecordTypeToChar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Record",
                type: "nvarchar(1)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTime",
                table: "Record",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 30, 18, 20, 27, 768, DateTimeKind.Utc).AddTicks(9266),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 30, 14, 43, 24, 197, DateTimeKind.Utc).AddTicks(4366));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Extract",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 30, 18, 20, 27, 768, DateTimeKind.Utc).AddTicks(8756),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 30, 14, 43, 24, 197, DateTimeKind.Utc).AddTicks(3484));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Updated",
                table: "Balance",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 30, 18, 20, 27, 768, DateTimeKind.Utc).AddTicks(8186),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 30, 14, 43, 24, 197, DateTimeKind.Utc).AddTicks(2649));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Balance",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 30, 18, 20, 27, 768, DateTimeKind.Utc).AddTicks(7714),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 30, 14, 43, 24, 197, DateTimeKind.Utc).AddTicks(1811));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Updated",
                table: "Account",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 30, 18, 20, 27, 768, DateTimeKind.Utc).AddTicks(7112),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 30, 14, 43, 24, 197, DateTimeKind.Utc).AddTicks(1006));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Account",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 30, 18, 20, 27, 768, DateTimeKind.Utc).AddTicks(6521),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 30, 14, 43, 24, 196, DateTimeKind.Utc).AddTicks(9948));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Record",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTime",
                table: "Record",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 30, 14, 43, 24, 197, DateTimeKind.Utc).AddTicks(4366),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 30, 18, 20, 27, 768, DateTimeKind.Utc).AddTicks(9266));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Extract",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 30, 14, 43, 24, 197, DateTimeKind.Utc).AddTicks(3484),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 30, 18, 20, 27, 768, DateTimeKind.Utc).AddTicks(8756));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Updated",
                table: "Balance",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 30, 14, 43, 24, 197, DateTimeKind.Utc).AddTicks(2649),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 30, 18, 20, 27, 768, DateTimeKind.Utc).AddTicks(8186));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Balance",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 30, 14, 43, 24, 197, DateTimeKind.Utc).AddTicks(1811),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 30, 18, 20, 27, 768, DateTimeKind.Utc).AddTicks(7714));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Updated",
                table: "Account",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 30, 14, 43, 24, 197, DateTimeKind.Utc).AddTicks(1006),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 30, 18, 20, 27, 768, DateTimeKind.Utc).AddTicks(7112));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Account",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 30, 14, 43, 24, 196, DateTimeKind.Utc).AddTicks(9948),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 30, 18, 20, 27, 768, DateTimeKind.Utc).AddTicks(6521));
        }
    }
}
