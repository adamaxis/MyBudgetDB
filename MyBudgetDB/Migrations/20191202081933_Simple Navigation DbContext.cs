using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MyBudgetDB.Migrations
{
    public partial class SimpleNavigationDbContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_Budgets_UserBudgetBudgetId",
                table: "Expense");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Budgets",
                table: "Budgets");

            migrationBuilder.RenameTable(
                name: "Budgets",
                newName: "UserBudget");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBudget",
                table: "UserBudget",
                column: "BudgetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_UserBudget_UserBudgetBudgetId",
                table: "Expense",
                column: "UserBudgetBudgetId",
                principalTable: "UserBudget",
                principalColumn: "BudgetId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_UserBudget_UserBudgetBudgetId",
                table: "Expense");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBudget",
                table: "UserBudget");

            migrationBuilder.RenameTable(
                name: "UserBudget",
                newName: "Budgets");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Budgets",
                table: "Budgets",
                column: "BudgetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_Budgets_UserBudgetBudgetId",
                table: "Expense",
                column: "UserBudgetBudgetId",
                principalTable: "Budgets",
                principalColumn: "BudgetId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
