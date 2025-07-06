using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PojistovnaFullAspPrzeczek.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInsurancePriceDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"ALTER TABLE ""PersonInsurance""
          ALTER COLUMN ""InsurancePrice"" TYPE numeric(18,2)
          USING ""InsurancePrice""::numeric;"
            );
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "InsurancePrice",
                table: "PersonInsurance",
                type: "text",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");
        }
    }
}
