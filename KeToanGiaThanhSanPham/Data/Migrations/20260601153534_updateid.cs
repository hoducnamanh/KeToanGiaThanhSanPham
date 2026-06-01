using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KeToanGiaThanhSanPham.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SanPhamId",
                table: "SanPham",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PhanXuongId",
                table: "PhanXuong",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "NguyenVatLieuId",
                table: "NguyenVatLieu",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "DinhMucKyThuatId",
                table: "DinhMucKyThuat",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SanPham",
                newName: "SanPhamId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PhanXuong",
                newName: "PhanXuongId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "NguyenVatLieu",
                newName: "NguyenVatLieuId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "DinhMucKyThuat",
                newName: "DinhMucKyThuatId");
        }
    }
}
