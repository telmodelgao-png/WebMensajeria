using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMensajeria.Modelos
{
    [Table("Participanteschats")]
    public class ParticipanteChat
    {
        [Key]
        [Column("id_participante_chat")]
        public int idParticipante { get; set; }

        [ForeignKey("chats")]
        [Column("id_chat")]
        public int idChat { get; set; }

        [ForeignKey("usuarios")]
        [Column("id_usuario")]
        public int idUsuario { get; set; }

        [ForeignKey("roles")]
        [Column("id_rol")]
        public int idRol { get; set; }

        //objetos de navegacion
        public Chat? chats { get; set; }
        public Usuario? usuarios { get; set; }
        public RolParticipante? roles { get; set; }
    }
}
