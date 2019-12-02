using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MyBudgetDB.Migrations
{
    public partial class AddedExpensesDbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_Budgets_UserBudgetBudgetId",
                table: "Expense");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Expense",
                table: "Expense");

            migrationBuilder.RenameTable(
                name: "Expense",
                newName: "Expenses");

            migrationBuilder.RenameIndex(
                name: "IX_Expense_UserBudgetBudgetId",
                table: "Expenses",
                newName: "IX_Expenses_UserBudgetBudgetId");

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

            migrationBuilder.RenameTable(
                name: "Expenses",
                newName: "Expense");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_UserBudgetBudgetId",
                table: "Expense",
                newName: "IX_Expense_UserBudgetBudgetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Expense",
                table: "Expense",
                column: "IdExpense");

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_Budgets_UserBudgetBudgetId",
                table: "Expense",
                column: "UserBudgetBudgetId",
                principalTable: "Budgets",
                principalColumn: "BudgetId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
