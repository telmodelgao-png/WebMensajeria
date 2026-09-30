using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebMensajeria.API.Migrations
{
    /// <inheritdoc />
    public partial class V01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rolparticipantes",
                columns: table => new
                {
                    id_rol = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombe_rol = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rolparticipantes", x => x.id_rol);
                });

            migrationBuilder.CreateTable(
                name: "tipoAdjuntos",
                columns: table => new
                {
                    id_tipo_adjunto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre_tipo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipoAdjuntos", x => x.id_tipo_adjunto);
                });

            migrationBuilder.CreateTable(
                name: "tipoChats",
                columns: table => new
                {
                    id_tipo_chat = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre_tipo_chat = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipoChats", x => x.id_tipo_chat);
                });

            migrationBuilder.CreateTable(
                name: "tipoestados",
                columns: table => new
                {
                    id_tipo_estado = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tipoEstado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipoestados", x => x.id_tipo_estado);
                });

            migrationBuilder.CreateTable(
                name: "tiporeacciones",
                columns: table => new
                {
                    id_tipo_reaccion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    simbolo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tiporeacciones", x => x.id_tipo_reaccion);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre_usuario = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    correoelectronico = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.id_usuario);
                });

            migrationBuilder.CreateTable(
                name: "Chat",
                columns: table => new
                {
                    id_chat = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_tipo_chat = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chat", x => x.id_chat);
                    table.ForeignKey(
                        name: "FK_Chat_tipoChats_id_tipo_chat",
                        column: x => x.id_tipo_chat,
                        principalTable: "tipoChats",
                        principalColumn: "id_tipo_chat",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contactos",
                columns: table => new
                {
                    id_contacto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_usuario_principal = table.Column<int>(type: "integer", nullable: false),
                    id_usuario_contacto = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contactos", x => x.id_contacto);
                    table.ForeignKey(
                        name: "FK_contactos_usuarios_id_usuario_contacto",
                        column: x => x.id_usuario_contacto,
                        principalTable: "usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_contactos_usuarios_id_usuario_principal",
                        column: x => x.id_usuario_principal,
                        principalTable: "usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "detallesGrupos",
                columns: table => new
                {
                    id_detalle_grupo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre_grupo = table.Column<string>(type: "text", nullable: false),
                    descripcion = table.Column<string>(type: "text", nullable: false),
                    Id_chat = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detallesGrupos", x => x.id_detalle_grupo);
                    table.ForeignKey(
                        name: "FK_detallesGrupos_Chat_Id_chat",
                        column: x => x.Id_chat,
                        principalTable: "Chat",
                        principalColumn: "id_chat",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mensajes",
                columns: table => new
                {
                    id_mensaje = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mensaje = table.Column<string>(type: "text", nullable: false),
                    Fecha_envio = table.Column<DateTime>(type: "timestamp", nullable: false),
                    id_chat = table.Column<int>(type: "integer", nullable: false),
                    id_Usuario_remitente = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mensajes", x => x.id_mensaje);
                    table.ForeignKey(
                        name: "FK_mensajes_Chat_id_chat",
                        column: x => x.id_chat,
                        principalTable: "Chat",
                        principalColumn: "id_chat",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mensajes_usuarios_id_Usuario_remitente",
                        column: x => x.id_Usuario_remitente,
                        principalTable: "usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Participanteschats",
                columns: table => new
                {
                    id_participante_chat = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_chat = table.Column<int>(type: "integer", nullable: false),
                    id_usuario = table.Column<int>(type: "integer", nullable: false),
                    id_rol = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participanteschats", x => x.id_participante_chat);
                    table.ForeignKey(
                        name: "FK_Participanteschats_Chat_id_chat",
                        column: x => x.id_chat,
                        principalTable: "Chat",
                        principalColumn: "id_chat",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participanteschats_rolparticipantes_id_rol",
                        column: x => x.id_rol,
                        principalTable: "rolparticipantes",
                        principalColumn: "id_rol",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participanteschats_usuarios_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "adjuntoMensajes",
                columns: table => new
                {
                    id_adjunto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    url_archivo = table.Column<string>(type: "text", nullable: false),
                    id_mensaje = table.Column<int>(type: "integer", nullable: false),
                    id_tipo_adjunto = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_adjuntoMensajes", x => x.id_adjunto);
                    table.ForeignKey(
                        name: "FK_adjuntoMensajes_mensajes_id_mensaje",
                        column: x => x.id_mensaje,
                        principalTable: "mensajes",
                        principalColumn: "id_mensaje",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_adjuntoMensajes_tipoAdjuntos_id_tipo_adjunto",
                        column: x => x.id_tipo_adjunto,
                        principalTable: "tipoAdjuntos",
                        principalColumn: "id_tipo_adjunto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "estadoreceptormesajes",
                columns: table => new
                {
                    id_estado_recpetor = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_mensaje = table.Column<int>(type: "integer", nullable: false),
                    id_usuario_receptor = table.Column<int>(type: "integer", nullable: false),
                    id_tipo_estado = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_estadoreceptormesajes", x => x.id_estado_recpetor);
                    table.ForeignKey(
                        name: "FK_estadoreceptormesajes_mensajes_id_mensaje",
                        column: x => x.id_mensaje,
                        principalTable: "mensajes",
                        principalColumn: "id_mensaje",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_estadoreceptormesajes_tipoestados_id_tipo_estado",
                        column: x => x.id_tipo_estado,
                        principalTable: "tipoestados",
                        principalColumn: "id_tipo_estado",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_estadoreceptormesajes_usuarios_id_usuario_receptor",
                        column: x => x.id_usuario_receptor,
                        principalTable: "usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReaccionMensaje",
                columns: table => new
                {
                    id_reaccion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_mensaje = table.Column<int>(type: "integer", nullable: false),
                    id_usuario = table.Column<int>(type: "integer", nullable: false),
                    id_tipo_reaccion = table.Column<int>(type: "integer", nullable: false),
                    TipoReaccionidTipoReaccion = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReaccionMensaje", x => x.id_reaccion);
                    table.ForeignKey(
                        name: "FK_ReaccionMensaje_mensajes_id_mensaje",
                        column: x => x.id_mensaje,
                        principalTable: "mensajes",
                        principalColumn: "id_mensaje",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReaccionMensaje_tiporeacciones_TipoReaccionidTipoReaccion",
                        column: x => x.TipoReaccionidTipoReaccion,
                        principalTable: "tiporeacciones",
                        principalColumn: "id_tipo_reaccion");
                    table.ForeignKey(
                        name: "FK_ReaccionMensaje_usuarios_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_adjuntoMensajes_id_mensaje",
                table: "adjuntoMensajes",
                column: "id_mensaje");

            migrationBuilder.CreateIndex(
                name: "IX_adjuntoMensajes_id_tipo_adjunto",
                table: "adjuntoMensajes",
                column: "id_tipo_adjunto");

            migrationBuilder.CreateIndex(
                name: "IX_Chat_id_tipo_chat",
                table: "Chat",
                column: "id_tipo_chat");

            migrationBuilder.CreateIndex(
                name: "IX_contactos_id_usuario_contacto",
                table: "contactos",
                column: "id_usuario_contacto");

            migrationBuilder.CreateIndex(
                name: "IX_contactos_id_usuario_principal",
                table: "contactos",
                column: "id_usuario_principal");

            migrationBuilder.CreateIndex(
                name: "IX_detallesGrupos_Id_chat",
                table: "detallesGrupos",
                column: "Id_chat");

            migrationBuilder.CreateIndex(
                name: "IX_estadoreceptormesajes_id_mensaje",
                table: "estadoreceptormesajes",
                column: "id_mensaje");

            migrationBuilder.CreateIndex(
                name: "IX_estadoreceptormesajes_id_tipo_estado",
                table: "estadoreceptormesajes",
                column: "id_tipo_estado");

            migrationBuilder.CreateIndex(
                name: "IX_estadoreceptormesajes_id_usuario_receptor",
                table: "estadoreceptormesajes",
                column: "id_usuario_receptor");

            migrationBuilder.CreateIndex(
                name: "IX_mensajes_id_chat",
                table: "mensajes",
                column: "id_chat");

            migrationBuilder.CreateIndex(
                name: "IX_mensajes_id_Usuario_remitente",
                table: "mensajes",
                column: "id_Usuario_remitente");

            migrationBuilder.CreateIndex(
                name: "IX_Participanteschats_id_chat",
                table: "Participanteschats",
                column: "id_chat");

            migrationBuilder.CreateIndex(
                name: "IX_Participanteschats_id_rol",
                table: "Participanteschats",
                column: "id_rol");

            migrationBuilder.CreateIndex(
                name: "IX_Participanteschats_id_usuario",
                table: "Participanteschats",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_ReaccionMensaje_id_mensaje",
                table: "ReaccionMensaje",
                column: "id_mensaje");

            migrationBuilder.CreateIndex(
                name: "IX_ReaccionMensaje_id_usuario",
                table: "ReaccionMensaje",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_ReaccionMensaje_TipoReaccionidTipoReaccion",
                table: "ReaccionMensaje",
                column: "TipoReaccionidTipoReaccion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "adjuntoMensajes");

            migrationBuilder.DropTable(
                name: "contactos");

            migrationBuilder.DropTable(
                name: "detallesGrupos");

            migrationBuilder.DropTable(
                name: "estadoreceptormesajes");

            migrationBuilder.DropTable(
                name: "Participanteschats");

            migrationBuilder.DropTable(
                name: "ReaccionMensaje");

            migrationBuilder.DropTable(
                name: "tipoAdjuntos");

            migrationBuilder.DropTable(
                name: "tipoestados");

            migrationBuilder.DropTable(
                name: "rolparticipantes");

            migrationBuilder.DropTable(
                name: "mensajes");

            migrationBuilder.DropTable(
                name: "tiporeacciones");

            migrationBuilder.DropTable(
                name: "Chat");

            migrationBuilder.DropTable(
                name: "usuarios");

            migrationBuilder.DropTable(
                name: "tipoChats");
        }
    }
}
