using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dicelanStore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserTableOrOtherChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         

            migrationBuilder.CreateTable(
                name: "material",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__material__3213E83FF7A11FC0", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order_status",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__order_st__3213E83FB6C6D856", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__product___3213E83FBC9DBB5B", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__role__3213E83F0B785139", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "store",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    street = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    province = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    district = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    ward = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    city = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__store__3213E83F3E7FF8A0", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    price = table.Column<double>(type: "float", nullable: true),
                    stock_quantity = table.Column<short>(type: "smallint", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    last_modified_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: true),
                    material_id = table.Column<int>(type: "int", nullable: false),
                    product_type_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__product__3213E83F14EFC8BE", x => x.id);
                    table.ForeignKey(
                        name: "FK_product_material",
                        column: x => x.material_id,
                        principalTable: "material",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_product_product_type",
                        column: x => x.product_type_id,
                        principalTable: "product_type",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    username = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    password = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    phone_number = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    avatar = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    last_login_time = table.Column<DateTime>(type: "datetime", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: true),
                    role_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__user__3213E83F34218B22", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_role",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "image",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    product_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__image__3213E83FC239A74A", x => x.id);
                    table.ForeignKey(
                        name: "FK_image_product",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "order_detail",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<short>(type: "smallint", nullable: true),
                    unit_price = table.Column<double>(type: "float", nullable: true),
                    discount = table.Column<double>(type: "float", nullable: true),
                    total = table.Column<double>(type: "float", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    last_modified_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    product_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__order_de__3213E83F09F65CAA", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_detail_product",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "address",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    street = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    city = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    province = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    ward = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    zip_code = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__address__3213E83FD64780C8", x => x.id);
                    table.ForeignKey(
                        name: "FK_address_user",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "cart",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__cart__3213E83F95B37650", x => x.id);
                    table.ForeignKey(
                        name: "FK_cart_user",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "order",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    phone_number = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    last_modified_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    order_type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    total = table.Column<double>(type: "float", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    order_status_id = table.Column<int>(type: "int", nullable: false),
                    history_id = table.Column<long>(type: "bigint", nullable: false),
                    store_id = table.Column<int>(type: "int", nullable: false),
                    deposit_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__order__3213E83FF42DBBDE", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_order_status",
                        column: x => x.order_status_id,
                        principalTable: "order_status",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_order_store",
                        column: x => x.store_id,
                        principalTable: "store",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_order_user",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "cart_item",
                columns: table => new
                {
                    cart_id = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__cart_ite__6A850DF831EAC01F", x => new { x.cart_id, x.product_id });
                    table.ForeignKey(
                        name: "FK_cart_item_cart",
                        column: x => x.cart_id,
                        principalTable: "cart",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_cart_item_product",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "invoice",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    sub_total = table.Column<double>(type: "float", nullable: true),
                    tax_amount = table.Column<double>(type: "float", nullable: true),
                    payment_method = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    total = table.Column<double>(type: "float", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    order_id = table.Column<long>(type: "bigint", nullable: false),
                    invoice_status_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__invoice__3213E83F55731F2E", x => x.id);
                    table.ForeignKey(
                        name: "FK_invoice_invoice_status",
                        column: x => x.invoice_status_id,
                        principalTable: "invoice_status",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_invoice_order",
                        column: x => x.order_id,
                        principalTable: "order",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_address_user_id",
                table: "address",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_cart_user_id",
                table: "cart",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_cart_item_product_id",
                table: "cart_item",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_image_product_id",
                table: "image",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_invoice_invoice_status_id",
                table: "invoice",
                column: "invoice_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_invoice_order_id",
                table: "invoice",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_order_status_id",
                table: "order",
                column: "order_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_store_id",
                table: "order",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_user_id",
                table: "order",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_detail_product_id",
                table: "order_detail",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_material_id",
                table: "product",
                column: "material_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_product_type_id",
                table: "product",
                column: "product_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_role_id",
                table: "user",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "address");

            migrationBuilder.DropTable(
                name: "cart_item");

            migrationBuilder.DropTable(
                name: "image");

            migrationBuilder.DropTable(
                name: "invoice");

            migrationBuilder.DropTable(
                name: "order_detail");

            migrationBuilder.DropTable(
                name: "cart");

            migrationBuilder.DropTable(
                name: "invoice_status");

            migrationBuilder.DropTable(
                name: "order");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "order_status");

            migrationBuilder.DropTable(
                name: "store");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "material");

            migrationBuilder.DropTable(
                name: "product_type");

            migrationBuilder.DropTable(
                name: "role");
        }
    }
}
