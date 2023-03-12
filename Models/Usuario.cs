using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorAPP.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        public string Usuario1 { get; set; } = null!;

        public string Contraseña { get; set; } = null!;

        public int Rol { get; set; }

        public virtual ICollection<Director> Director { get; } = new List<Director>();

    }
}
