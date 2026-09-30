using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMensajeria.Modelos
{
    public class Chat
    {
        [Key]
        [Column("id_chat")]
        public int idChat { get; set; } 
        [ForeignKey("tipoChat")]
        [Column("id_tipo_chat")]
        public int idTipoChat { get; set; }
        public TipoChat? tipoChat { get; set; }
        
        public List<ParticipanteChat> participantesChats { get; set; } = new List<ParticipanteChat>();
        public List<Mensaje>mensajes { get; set; }= new List<Mensaje>();
    }
    
}
