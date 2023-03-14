using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorAPP.Models
{
    public class DocenteAsignatura
    {
        public int Id { get; set; }

        public int IdDocente { get; set; }

        public int IdAsignatura { get; set; }
    }
}
