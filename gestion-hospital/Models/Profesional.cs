using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gestion_hospital.Models
{
    public class Profesional
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Categoria { get; set; }
        public int IdCate { get; set; }
    }
}