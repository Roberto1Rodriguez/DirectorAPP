﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorAPP.Models
{
    public class Asignatura
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public int TipoAsignatura { get; set; }

    }
}
