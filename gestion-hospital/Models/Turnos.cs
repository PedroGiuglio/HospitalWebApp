using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gestion_hospital.Models
{
    public class Turnos
    {
        public int IdTurno { get; set; }
        public int IdPaciente { get; set; }
        public int IdProfesional { get; set; }
        public string Dia { get; set; }
        public TimeSpan Horario { get; set; }
    }
}