using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Comments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTime",
                table: "Record",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 30, 18, 26, 58, 237, DateTimeKind.Utc).AddTicks(3641),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 30, 18, 20, 27, 768, DateTimeKind.Utc).AddTicks(9266));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Extract",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 30, 18, 26, 58, 237, DateTimeKind.Utc).AddTicks(2698),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 30, 18, 20, 27, 768, DateTimeKind.Utc).AddTicks(8756));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Updated",
                table: "Balance",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 30, 18, 26, 58, 237, DateTimeKind.Utc).AddTicks(1866),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 30, 18, 20, 27, 768, DateTimeKind.Utc).AddTicks(8186));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Balance",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 30, 18, 26, 58, 236, DateTimeKind.Utc).AddTicks(9532),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 30, 18, 20, 27, 768, DateTimeKind.Utc).AddTicks(7714));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Updated",
                table: "Account",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 30, 18, 26, 58, 236, DateTimeKind.Utc).AddTicks(8825),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 30, 18, 20, 27, 768, DateTimeKind.Utc).AddTicks(7112));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Account",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 30, 18, 26, 58, 236, DateTimeKind.Utc).AddTicks(7888),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 30, 18, 20, 27, 768, DateTimeKind.Utc).AddTicks(6521));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTime",
                table: "Record",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 30, 18, 20, 27, 768, DateTimeKind.Utc).AddTicks(9266),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 30, 18, 26, 58, 237, DateTimeKind.Utc).AddTicks(3641));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Extract",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 30, 18, 20, 27, 768, DateTimeKind.Utc).AddTicks(8756),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 30, 18, 26, 58, 237, DateTimeKind.Utc).AddTicks(2698));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Updated",
                table: "Balance",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 30, 18, 20, 27, 768, DateTimeKind.Utc).AddTicks(8186),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 30, 18, 26, 58, 237, DateTimeKind.Utc).AddTicks(1866));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Balance",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 30, 18, 20, 27, 768, DateTimeKind.Utc).AddTicks(7714),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 30, 18, 26, 58, 236, DateTimeKind.Utc).AddTicks(9532));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Updated",
                table: "Account",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 30, 18, 20, 27, 768, DateTimeKind.Utc).AddTicks(7112),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 30, 18, 26, 58, 236, DateTimeKind.Utc).AddTicks(8825));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Account",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 7, 30, 18, 20, 27, 768, DateTimeKind.Utc).AddTicks(6521),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 7, 30, 18, 26, 58, 236, DateTimeKind.Utc).AddTicks(7888));
        }
    }
}
