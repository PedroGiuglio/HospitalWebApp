using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using gestion_hospital.Models;

namespace gestion_hospital.Controllers
{
    public class TurnosController : ApiController
    {
        //METODO GET//
        public HttpResponseMessage Get()
        {
            string query = @"
                            SELECT
                                t.IdTurno,
                                p.IdPaciente,
                                pr.IdTrabajador,
                                pr.nombre AS NombreProfesional,
                                pr.apellido AS ApellidoProfesional,
                                t.Dia,
                                t.Horario
                            FROM
                                dbo.Turnos t
                            INNER JOIN
                                Paciente p ON t.IdPaciente = p.IdPaciente
                            INNER JOIN
                                Doctor pr ON t.IdProfesional = pr.IdTrabajador";
            DataTable table = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.
                ConnectionStrings["HospitalAppDB"].ConnectionString))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(table);
            }
            return Request.CreateResponse(HttpStatusCode.OK, table);
        }

        public string Post(Turnos tur)
        {
            try
            {
                string query = @"
            INSERT INTO dbo.Turnos 
            (IdPaciente, IdProfesional, Dia, Horario) 
            VALUES
            (@IdPaciente, @IdProfesional, @Dia, @Horario)";

                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["HospitalAppDB"].ConnectionString))
                {
                    con.Open(); // Abre la conexión aquí

                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@IdPaciente", tur.IdPaciente);
                        command.Parameters.AddWithValue("@IdProfesional", tur.IdProfesional);
                        command.Parameters.AddWithValue("@Dia", tur.Dia);
                        command.Parameters.AddWithValue("@Horario", tur.Horario);

                        int rowsAffected = command.ExecuteNonQuery();
                    }

                    return "Added successfully";
                }
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging purposes
                Console.WriteLine(ex.ToString());
                return "Fail: " + ex.Message;
            }
        }
    }
}
