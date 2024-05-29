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
    public class ProfesionalController : ApiController
    {
        //METODO GET//
        public HttpResponseMessage Get()
        {
            string query = @"
                            SELECT
                                d.IdTrabajador,
                                d.nombre,
                                d.apellido,
                                tp.categoria
                            FROM
                                dbo.Doctor d
                            INNER JOIN
                                TipoProfesional tp ON d.tipoProfesional = tp.id";
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


        // DELETE //

        public string Delete(int id)
        {
            try
            {
                string query = @"
                 delete from dbo.Doctor
                 where IdTrabajador=" + id + @"
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

                return "Delete Successfully tabla Doctor";
            }
            catch (Exception ex)
            {

                return "Failed to delete Doc!!" + ex.Message;
            }
        }
    }


}
