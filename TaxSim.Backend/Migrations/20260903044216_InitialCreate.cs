using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaxSim.Backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaxAgents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AgentName = table.Column<string>(type: "TEXT", nullable: false),
                    BehavioralProfile = table.Column<string>(type: "TEXT", nullable: false),
                    Income = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxAgents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxPolicies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PolicyTitle = table.Column<string>(type: "TEXT", nullable: false),
                    NaturalLanguageRule = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxPolicies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SimulationRuns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TaxPolicyId = table.Column<int>(type: "INTEGER", nullable: false),
                    RunTimestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TotalRevenueCollected = table.Column<decimal>(type: "TEXT", nullable: false),
                    ComplianceFailureCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimulationRuns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimulationRuns_TaxPolicies_TaxPolicyId",
                        column: x => x.TaxPolicyId,
                        principalTable: "TaxPolicies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SimulationRunId = table.Column<int>(type: "INTEGER", nullable: false),
                    TaxAgentId = table.Column<int>(type: "INTEGER", nullable: false),
                    ActionTaken = table.Column<string>(type: "TEXT", nullable: false),
                    RiskFlag = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLogs_SimulationRuns_SimulationRunId",
                        column: x => x.SimulationRunId,
                        principalTable: "SimulationRuns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditLogs_TaxAgents_TaxAgentId",
                        column: x => x.TaxAgentId,
                        principalTable: "TaxAgents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_SimulationRunId",
                table: "AuditLogs",
                column: "SimulationRunId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_TaxAgentId",
                table: "AuditLogs",
                column: "TaxAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_SimulationRuns_TaxPolicyId",
                table: "SimulationRuns",
                column: "TaxPolicyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "SimulationRuns");

            migrationBuilder.DropTable(
                name: "TaxAgents");

            migrationBuilder.DropTable(
                name: "TaxPolicies");
        }
    }
}
