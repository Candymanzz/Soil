using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Crops",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Growth_period = table.Column<int>(type: "integer", nullable: false),
                    Water_requirements = table.Column<string>(type: "text", nullable: false),
                    Optimal_temperature = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Crops", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fertilizers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Composition = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fertilizers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fields",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Area = table.Column<double>(type: "double precision", nullable: false),
                    Soil_type = table.Column<string>(type: "text", nullable: false),
                    Coordinates = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fields", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    End_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    End_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Position = table.Column<string>(type: "text", nullable: false),
                    Contact = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlantingPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Planned_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Expected_yield = table.Column<double>(type: "double precision", nullable: false),
                    Field_id = table.Column<Guid>(type: "uuid", nullable: false),
                    Crop_id = table.Column<Guid>(type: "uuid", nullable: false),
                    Season_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantingPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlantingPlans_Crops_Crop_id",
                        column: x => x.Crop_id,
                        principalTable: "Crops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlantingPlans_Fields_Field_id",
                        column: x => x.Field_id,
                        principalTable: "Fields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlantingPlans_Seasons_Season_id",
                        column: x => x.Season_id,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentTasks",
                columns: table => new
                {
                    EquipmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    TasksId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentTasks", x => new { x.EquipmentId, x.TasksId });
                    table.ForeignKey(
                        name: "FK_EquipmentTasks_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentTasks_Tasks_TasksId",
                        column: x => x.TasksId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TasksWorkers",
                columns: table => new
                {
                    TasksId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TasksWorkers", x => new { x.TasksId, x.WorkersId });
                    table.ForeignKey(
                        name: "FK_TasksWorkers_Tasks_TasksId",
                        column: x => x.TasksId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TasksWorkers_Workers_WorkersId",
                        column: x => x.WorkersId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FertilizationPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Application_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    Plan_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Fertilization_Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FertilizationPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FertilizationPlans_Fertilizers_Fertilization_Id",
                        column: x => x.Fertilization_Id,
                        principalTable: "Fertilizers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FertilizationPlans_PlantingPlans_Plan_Id",
                        column: x => x.Plan_Id,
                        principalTable: "PlantingPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HarvestLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Actual_yield = table.Column<double>(type: "double precision", nullable: false),
                    Harvest_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Quality_rating = table.Column<int>(type: "integer", nullable: false),
                    Plan_Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HarvestLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HarvestLogs_PlantingPlans_Plan_Id",
                        column: x => x.Plan_Id,
                        principalTable: "PlantingPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlantingPlansTasks",
                columns: table => new
                {
                    PlantingPlansId = table.Column<Guid>(type: "uuid", nullable: false),
                    TasksId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantingPlansTasks", x => new { x.PlantingPlansId, x.TasksId });
                    table.ForeignKey(
                        name: "FK_PlantingPlansTasks_PlantingPlans_PlantingPlansId",
                        column: x => x.PlantingPlansId,
                        principalTable: "PlantingPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlantingPlansTasks_Tasks_TasksId",
                        column: x => x.TasksId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentTasks_TasksId",
                table: "EquipmentTasks",
                column: "TasksId");

            migrationBuilder.CreateIndex(
                name: "IX_FertilizationPlans_Fertilization_Id",
                table: "FertilizationPlans",
                column: "Fertilization_Id");

            migrationBuilder.CreateIndex(
                name: "IX_FertilizationPlans_Plan_Id",
                table: "FertilizationPlans",
                column: "Plan_Id");

            migrationBuilder.CreateIndex(
                name: "IX_HarvestLogs_Plan_Id",
                table: "HarvestLogs",
                column: "Plan_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlantingPlans_Crop_id",
                table: "PlantingPlans",
                column: "Crop_id");

            migrationBuilder.CreateIndex(
                name: "IX_PlantingPlans_Field_id",
                table: "PlantingPlans",
                column: "Field_id");

            migrationBuilder.CreateIndex(
                name: "IX_PlantingPlans_Season_id",
                table: "PlantingPlans",
                column: "Season_id");

            migrationBuilder.CreateIndex(
                name: "IX_PlantingPlansTasks_TasksId",
                table: "PlantingPlansTasks",
                column: "TasksId");

            migrationBuilder.CreateIndex(
                name: "IX_TasksWorkers_WorkersId",
                table: "TasksWorkers",
                column: "WorkersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentTasks");

            migrationBuilder.DropTable(
                name: "FertilizationPlans");

            migrationBuilder.DropTable(
                name: "HarvestLogs");

            migrationBuilder.DropTable(
                name: "PlantingPlansTasks");

            migrationBuilder.DropTable(
                name: "TasksWorkers");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "Fertilizers");

            migrationBuilder.DropTable(
                name: "PlantingPlans");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.DropTable(
                name: "Crops");

            migrationBuilder.DropTable(
                name: "Fields");

            migrationBuilder.DropTable(
                name: "Seasons");
        }
    }
}
