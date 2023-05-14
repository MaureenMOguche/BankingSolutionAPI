using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class verificationCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "VerificationCode",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerificationCode",
                table: "AspNetUsers");
        }
    }
}
