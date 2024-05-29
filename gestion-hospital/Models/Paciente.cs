using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gestion_hospital.Models
{
    public class Paciente
    {
        public int IdPaciente { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public object TurnoArea { get; set; }
        public string HistoriaPaciente { get; set; }
        public string IdTurno { get; set; }
        public DateTime DiaTurno { get; set; }
        public TimeSpan HoraTurno { get; set; }
        public int IdDoctorSelec { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}