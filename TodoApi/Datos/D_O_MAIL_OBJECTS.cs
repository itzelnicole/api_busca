using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using TodoApi.Conexion;
using TodoApi.Modelo;

namespace TodoApi.Datos
{
    public class D_O_MAIL_OBJECTS
    {
        private readonly IConfiguration _configuration;
        private readonly ConexionBD _cn;

        public D_O_MAIL_OBJECTS(IConfiguration configuration)
        {
            _configuration = configuration;
            _cn = new ConexionBD(_configuration);
        }

        public async Task<List<M_O_MAIL_OBJECTS>> MostrarDatos()
        {
            var lista = new List<M_O_MAIL_OBJECTS>();

            try
            {
                // Crear la conexión a la base de datos
                using (var sql = new SqlConnection(_cn.cadenaSQL()))
                {
                    // Resto del código...

                    // Construir la consulta SQL
                    string query = @"
                        SELECT l.MAILITM_PID,l.MAILITM_FID,CONCAT
                        (e.EVENT_TYPE_NM,' (',ma.EVENT_TYPE_CD ,')') AS Event_Type,ma.EVENT_GMT_DT as Event_Date,
                        CONCAT(o.OFFICE_FCD,'-',o.OFFICE_NM ) AS Office,CONCAT(u.USER_DOMAIN,'-', u.USER_FID) 
                        AS Scanned  ,CONCAT(w.WORKSTATION_FID,'-', w.WORKSTATION_DOMAIN) AS Workstation ,
                        CONCAT(c.ITEM_CONDITION_CD,'-', c.ITEM_CONDITION_NM) AS Condition,om.OFFICE_FCD as
                        Next_office FROM L_MAILITMS l join L_MAILITM_EVENTS ma on ma.MAILITM_PID = l.MAILITM_PID 
                        join C_EVENT_TYPES e on ma.EVENT_TYPE_CD  = e.EVENT_TYPE_CD left 
                        join N_OWN_OFFICES o on o.OWN_OFFICE_CD =ma.EVENT_OFFICE_CD  left 
                        join N_OWN_OFFICES om on ma.NEXT_OFFICE_CD=om.OWN_OFFICE_CD left 
                        join L_USERS u on u.USER_PID = ma.USER_PID join L_WORKSTATIONS w on w.WORKSTATION_PID = ma.WORKSTATION_PID left join C_ITEM_CONDITIONS c on c.ITEM_CONDITION_CD=ma.CONDITION_CD ORDER BY  ma.EVENT_GMT_DT desc";



                    // Crear el comando SQL con la consulta y la conexión
                    using (var cmd = new SqlCommand(query, sql))
                    {
                        // Abrir la conexión
                        await sql.OpenAsync();

                        // Ejecutar la consulta y leer los resultados
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var objeto = new M_O_MAIL_OBJECTS
                                {
                                    // Asignar los valores de las nuevas propiedades
                                    MAILITM_PID = reader["MAILITM_PID"].ToString(),
                                    MAILITM_FID = reader["MAILITM_FID"].ToString(),
                                    EventType = reader["Event_Type"].ToString(),
                                    EventDate = reader["Event_Date"].ToString(),
                                    Office = reader["Office"].ToString(),
                                    Scanned = reader["Scanned"].ToString(),
                                    Workstation = reader["Workstation"].ToString(),
                                    Condition = reader["Condition"].ToString(),
                                    NextOffice = reader["Next_office"].ToString(),
                                };

                                lista.Add(objeto);
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                // Aquí deberías manejar la excepción, por ejemplo, registrándola en un archivo de log
                Console.WriteLine(e.ToString());
                throw;
            }

            return lista;
        }
         public async Task<List<M_O_MAIL_OBJECTS>> Buscar(string id)
        {
            var lista = await MostrarDatos();
            return lista.FindAll(item =>
            {
                 string idSinEspacios = item.MAILITM_FID.Replace(" ", string.Empty);
                return idSinEspacios.Equals(id);
            });
        }
    }
    
}