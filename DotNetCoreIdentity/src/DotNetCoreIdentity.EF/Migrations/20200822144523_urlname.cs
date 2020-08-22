using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetCoreIdentity.EF.Migrations
{
    public partial class urlname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlName",
                table: "Posts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlName",
                table: "Posts");
        }
    }
}
