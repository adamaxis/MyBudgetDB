using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MyBudgetDB.Migrations
{
    public partial class SimpleNavigationDbContext2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_UserBudget_UserBudgetBudgetId",
                table: "Expense");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBudget",
                table: "UserBudget");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Expense",
                table: "Expense");

            migrationBuilder.RenameTable(
                name: "UserBudget",
                newName: "Budgets");

            migrationBuilder.RenameTable(
                name: "Expense",
                newName: "Expenses");

            migrationBuilder.RenameIndex(
                name: "IX_Expense_UserBudgetBudgetId",
                table: "Expenses",
                newName: "IX_Expenses_UserBudgetBudgetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Budgets",
                table: "Budgets",
                column: "BudgetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses",
                column: "IdExpense");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Budgets_UserBudgetBudgetId",
                table: "Expenses",
                column: "UserBudgetBudgetId",
                principalTable: "Budgets",
                principalColumn: "BudgetId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Budgets_UserBudgetBudgetId",
                table: "Expenses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Budgets",
                table: "Budgets");

            migrationBuilder.RenameTable(
                name: "Expenses",
                newName: "Expense");

            migrationBuilder.RenameTable(
                name: "Budgets",
                newName: "UserBudget");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_UserBudgetBudgetId",
                table: "Expense",
                newName: "IX_Expense_UserBudgetBudgetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Expense",
                table: "Expense",
                column: "IdExpense");

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
    }
}
