using gestion_hospital.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace gestion_hospital.Controllers
{





    public class PacienteController : ApiController
    {

        //METODO GET//
        public HttpResponseMessage Get()
        {
            string query = "SELECT p.IdPaciente, p.nombre, p.apellido, tp.categoria, p.historiaPaciente " +
                            "FROM dbo.Paciente p " +
                            "INNER JOIN TipoProfesional tp ON p.turnoArea = tp.id";
            DataTable table = new DataTable();
            using(var con= new SqlConnection(ConfigurationManager.
                ConnectionStrings["HospitalAppDB"].ConnectionString))
                using(var cmd= new SqlCommand(query,con))
                using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(table);
            }
            return Request.CreateResponse(HttpStatusCode.OK, table);
        }

        //METODO INSERT //

        public HttpResponseMessage Post(Paciente pac)
        {
            int lastInsertedId = 0;

            try
            {
                string query = @"
            INSERT INTO dbo.Paciente 
            (Nombre, Apellido, TurnoArea, HistoriaPaciente) 
            VALUES
            (@Nombre, @Apellido, @TurnoArea, @HistoriaPaciente);
            SELECT CAST(SCOPE_IDENTITY() AS INT); ";  // Esta consulta devuelve el último ID generado

                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["HospitalAppDB"].ConnectionString))
                {
                    con.Open();

                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@Nombre", pac.Nombre);
                        command.Parameters.AddWithValue("@Apellido", pac.Apellido);
                        command.Parameters.AddWithValue("@TurnoArea", pac.TurnoArea);
                        command.Parameters.AddWithValue("@HistoriaPaciente", pac.HistoriaPaciente);

                        // Ejecuta la primera consulta para insertar el paciente y obtener el último ID generado
                        lastInsertedId = (int)command.ExecuteScalar();

                        // Utiliza el lastInsertedId como sea necesario (lo puedes almacenar, loguear, etc.)
                        Console.WriteLine($"Last Inserted ID: {lastInsertedId}");
                    }
                }

                // Crea una respuesta con el objeto anónimo como JSON
                var response = new
                {
                    Message = "Added successfully",
                    LastInsertedId = lastInsertedId
                };

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging purposes
                Console.WriteLine(ex.ToString());

                // Crea una respuesta de error con el mensaje de error
                var errorResponse = new
                {
                    Message = "Fail: " + ex.Message,
                    LastInsertedId = 0
                };

                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        //METODO UPDATE //

        public string Put(Paciente pac)
        {
            try
            {
                string query = @"
        UPDATE dbo.Paciente 
        SET Nombre = @Nombre,
            Apellido = @Apellido,
            TurnoArea = @TurnoArea,
            HistoriaPaciente = @HistoriaPaciente
        WHERE IdPaciente = @IdPaciente";

                DataTable table = new DataTable();

                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["HospitalAppDB"].ConnectionString))
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@Nombre", pac.Nombre);
                    command.Parameters.AddWithValue("@Apellido", pac.Apellido);
                    command.Parameters.AddWithValue("@TurnoArea", pac.TurnoArea);
                    command.Parameters.AddWithValue("@HistoriaPaciente", pac.HistoriaPaciente);
                    command.Parameters.AddWithValue("@IdPaciente", pac.IdPaciente);

                    con.Open();

                    int rowsAffected = command.ExecuteNonQuery();

                    using (var da = new SqlDataAdapter(command))
                    {
                        command.CommandType = CommandType.Text;
                        da.Fill(table);
                    }
                }

                return "Updated successfully";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "Fail: " + ex.Message;
            }

        }

        // DELETE //

        public string Delete(int id)
        {
            try
            {
                string query = @"
                 delete from dbo.Paciente
                 where IdPaciente=" + id + @"
                ";

                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.
                     ConnectionStrings["HospitalAppDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return "Delete Successfully";
            }
            catch (Exception ex)
            {

                return "Failed to delete!!" + ex.Message;
            }
        }




        //Como es un method custom debemos mapear el routing requerido//

        [Route("api/Recursos/GetAllCategorias")]
        [HttpGet]

        public HttpResponseMessage GetAllCategorias()
        {
            /*
            string query = @"
                  select tp.categoria from dbo.Paciente p " +
                 "INNER JOIN TipoProfesional tp ON p.turnoArea = tp.id";*/

            string query = @"SELECT Id, categoria FROM dbo.TipoProfesional  ";

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


        [Route("api/Paciente/Search")]
        [HttpGet]

        //METODO Search//
        public HttpResponseMessage Search(string nombre, string apellido, int turnoArea)
        {
            try
            {
                string query = @"
                SELECT 
                    p.IdPaciente, 
                    p.nombre, 
                    p.apellido, 
                    tp.categoria, 
                    p.historiaPaciente
                FROM 
                    dbo.Paciente p
                LEFT JOIN 
                    dbo.tipoProfesional tp ON p.TurnoArea = tp.id
                WHERE 
                    (@Nombre IS NULL OR p.Nombre LIKE '%' + @Nombre + '%')
                    AND (@Apellido IS NULL OR p.Apellido LIKE '%' + @Apellido + '%')
                    AND (@TurnoArea = 0 OR p.TurnoArea = @TurnoArea)";

                DataTable table = new DataTable();

                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["HospitalAppDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("@Nombre", string.IsNullOrEmpty(nombre) ? (object)DBNull.Value : (object)nombre);
                    cmd.Parameters.AddWithValue("@Apellido", string.IsNullOrEmpty(apellido) ? (object)DBNull.Value : (object)apellido);
                    cmd.Parameters.AddWithValue("@TurnoArea", turnoArea);

                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return Request.CreateResponse(HttpStatusCode.OK, table);
            }
            catch (Exception ex)
            {
                // Manejar la excepción y devolver un mensaje de error
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error en la búsqueda: " + ex.Message);
            }
        }
    }
}



        //Hasta aca//
    

