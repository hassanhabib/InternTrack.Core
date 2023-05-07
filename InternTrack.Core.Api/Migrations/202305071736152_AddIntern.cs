// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Data.Entity.Migrations;

namespace InternTrack.Core.Api.Migrations
{
    public partial class AddIntern : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Interns",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(nullable: false),
                    JoinDate = table.Column<DateTimeOffset>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: false),
                    TimeZone = table.Column<string>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("Interns", t => t.Id);
                });
            
        }
        
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Interns");
        }
    }
}
