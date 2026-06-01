using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KeToanGiaThanhSanPham.Data.Migrations
{
    /// <inheritdoc />
    public partial class initialsetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NguyenVatLieu",
                columns: table => new
                {
                    NguyenVatLieuId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguyenVatLieu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenNguyenVatLieu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DonViTinh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguyenVatLieu", x => x.NguyenVatLieuId);
                });

            migrationBuilder.CreateTable(
                name: "PhanXuong",
                columns: table => new
                {
                    PhanXuongId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaPhanXuong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenPhanXuong = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhanXuong", x => x.PhanXuongId);
                });

            migrationBuilder.CreateTable(
                name: "SanPham",
                columns: table => new
                {
                    SanPhamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSanPham = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenSanPham = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DonViTinh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhanXuongId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPham", x => x.SanPhamId);
                    table.ForeignKey(
                        name: "FK_SanPham_PhanXuong_PhanXuongId",
                        column: x => x.PhanXuongId,
                        principalTable: "PhanXuong",
                        principalColumn: "PhanXuongId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DinhMucKyThuat",
                columns: table => new
                {
                    DinhMucKyThuatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SanPhamId = table.Column<int>(type: "int", nullable: false),
                    NguyenVatLieuId = table.Column<int>(type: "int", nullable: false),
                    SoLuongDinhMuc = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DinhMucKyThuat", x => x.DinhMucKyThuatId);
                    table.ForeignKey(
                        name: "FK_DinhMucKyThuat_NguyenVatLieu_NguyenVatLieuId",
                        column: x => x.NguyenVatLieuId,
                        principalTable: "NguyenVatLieu",
                        principalColumn: "NguyenVatLieuId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DinhMucKyThuat_SanPham_SanPhamId",
                        column: x => x.SanPhamId,
                        principalTable: "SanPham",
                        principalColumn: "SanPhamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DinhMucKyThuat_NguyenVatLieuId",
                table: "DinhMucKyThuat",
                column: "NguyenVatLieuId");

            migrationBuilder.CreateIndex(
                name: "IX_DinhMucKyThuat_SanPhamId",
                table: "DinhMucKyThuat",
                column: "SanPhamId");

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_PhanXuongId",
                table: "SanPham",
                column: "PhanXuongId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DinhMucKyThuat");

            migrationBuilder.DropTable(
                name: "NguyenVatLieu");

            migrationBuilder.DropTable(
                name: "SanPham");

            migrationBuilder.DropTable(
                name: "PhanXuong");
        }
    }
}
