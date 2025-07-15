using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiapTechChallenge.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SextaMigracao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
               name: "Usuario",
               columns: table => new
               {
                   Id = table.Column<int>(type: "INT", nullable: false)
                       .Annotation("SqlServer:Identity", "1, 1"),
                   Nome = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                   Email = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                   Senha = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                   Role = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                   DataInclusao = table.Column<DateTime>(type: "DATETIME", nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_Usuario", x => x.Id);
               });

            migrationBuilder.CreateTable(
               name: "Categoria",
               columns: table => new
               {
                   Id = table.Column<int>(type: "int", nullable: false)
                       .Annotation("SqlServer:Identity", "1, 1"),
                   Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   DataInclusao = table.Column<DateTime>(type: "datetime2", nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_Categoria", x => x.Id);
               });

            migrationBuilder.CreateTable(
                name: "Produto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Disponivel = table.Column<bool>(type: "bit", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    DataInclusao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produto_Categoria_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
              name: "Pedido",
              columns: table => new
              {
                  Id = table.Column<int>(type: "INT", nullable: false)
                      .Annotation("SqlServer:Identity", "1, 1"),
                  UsuarioId = table.Column<int>(type: "INT", nullable: false),
                  PrecoTotal = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                  Status = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                  TipoEntrega = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                  DescricaoCancelamento = table.Column<string>(type: "VARCHAR(150)", nullable: true),
                  DataInclusao = table.Column<DateTime>(type: "DATETIME", nullable: false)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_Pedido", x => x.Id);
                  table.ForeignKey(
                      name: "FK_Pedido_Usuario_UsuarioId",
                      column: x => x.UsuarioId,
                      principalTable: "Usuario",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
              });

            migrationBuilder.CreateTable(
                name: "PedidoItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PedidoId = table.Column<int>(type: "INT", nullable: false),
                    ProdutoId = table.Column<int>(type: "INT", nullable: false),
                    Quantidade = table.Column<int>(type: "INT", nullable: false),
                    PrecoTotal = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    DataInclusao = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidoItem_Pedido_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedido",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidoItem_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PedidoControleCozinha",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PedidoId = table.Column<int>(type: "INT", nullable: false),
                    NomeCliente = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    Status = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    DataInclusao = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoControleCozinha", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidoControleCozinha_Pedido_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedido",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produto_CategoriaId",
                table: "Produto",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
              name: "IX_Pedido_UsuarioId",
              table: "Pedido",
              column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItem_PedidoId",
                table: "PedidoItem",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItem_ProdutoId",
                table: "PedidoItem",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoControleCozinha_PedidoId",
                table: "PedidoControleCozinha",
                column: "PedidoId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
               name: "Usuario");

            migrationBuilder.DropTable(
                name: "PedidoControleCozinha");

            migrationBuilder.DropTable(
            name: "Produto");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
             name: "PedidoItem");

            migrationBuilder.DropTable(
                name: "Pedido");

        }
    }
}
