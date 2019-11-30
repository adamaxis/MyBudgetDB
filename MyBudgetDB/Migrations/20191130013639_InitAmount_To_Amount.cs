using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MyBudgetDB.Migrations
{
    public partial class InitAmount_To_Amount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitAmount",
                table: "Budgets");

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "Budgets",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Budgets");

            migrationBuilder.AddColumn<double>(
                name: "InitAmount",
                table: "Budgets",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
