using Modelo;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConDB
{
    public class DB_connection
    {
        private MySqlConnection connection;

        private MySqlCommand commandUser;
        private MySqlCommand commandActivity;
        private MySqlCommand command;

        public DB_connection(string server, string database, string user, string password)
        {
            
            string connectionString = $"Server={server};Database={database};User Id={user};Password={password};";

            connection = new MySqlConnection(connectionString);
            
            try
            {
                connection.Open();
                Console.WriteLine("--- Conexión exitosa a la base de datos MySQL. ---\n");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error al conectarse a la base de datos: " + ex.Message);
            }
        }

        #region Usuarios

        public List<Usuario> GetUsers()
        {
            List<Usuario> usuarios = new List<Usuario>();

            string query = "SELECT * FROM usuario";
            commandUser = new MySqlCommand(query, connection);

            using (MySqlDataReader reader = commandUser.ExecuteReader())
            {
                
                while (reader.Read())
                {
                    /*
                          - id
                           - nombre
                           - edad
                           - email
                           - dni
                     */
                    int id = reader.GetInt32("id");
                    string dni = reader.GetString("dni");
                    string nombre = reader.GetString("nombre");
                    int edad = reader.GetInt32("edad");
                    string email = reader.GetString("email");

                    try
                    {
                        Usuario usuario = new Usuario(id, dni, nombre, edad, email);
                        usuarios.Add(usuario);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                reader.Close();
            }

            return usuarios;
        }

        public Usuario GetUserById(int id)
        {
            string query = $"SELECT * FROM usuario WHERE id = {id}";
            commandUser = new MySqlCommand(query, connection);

            using (MySqlDataReader reader = commandUser.ExecuteReader())
            {
                if (reader.Read())
                {
                    int id_user = reader.GetInt32("id");
                    string dni = reader.GetString("dni");
                    string nombre = reader.GetString("nombre");
                    int edad = reader.GetInt32("edad");
                    string email = reader.GetString("email");

                    try
                    {
                        Usuario usuario = new Usuario(id_user, dni, nombre, edad, email);
                        return usuario;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                reader.Close();
            }

            return null;

        }

        public bool SetUser(Usuario usuario)
        {
            string query = $"INSERT INTO usuario (dni, nombre, edad, email) VALUES ('{usuario.DNI}', '{usuario.Nombre}', {usuario.Edad}, '{usuario.Email}')";
            command = new MySqlCommand(query, connection);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error al insertar usuario: " + ex.Message);
                return false;
            }

            return true;
        }

        public bool UpdateUser(Usuario usuario)
        {
            string query = $"UPDATE usuario SET dni = '{usuario.DNI}', nombre = '{usuario.Nombre}', edad = {usuario.Edad}, email = '{usuario.Email}' WHERE id = {usuario.Id}";
            command = new MySqlCommand(query, connection);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error al actualizar usuario: " + ex.Message);
                return false;
            }

            return true;
        }

        public bool DeleteUser(int id)
        {
            string query = $"DELETE FROM usuario WHERE id = {id}";
            command = new MySqlCommand(query, connection);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error al eliminar usuario: " + ex.Message);
                return false;
            }

            return true;
        }

        /*
         TODO:

            GetUserActivities(int user_id)   Return json(usuario)
            
            -- controlar que un usuario no pueda inscribirse 2 veces
            -- controlar que un usuario no pueda inscribirse si la actividad esta completa
            SetUserToActivitie(int user_id, int activitie_id)
            If true return 1 else if max_participantes return 0 else (error) return -1
         
            -- Si una actividad no tiene usuarios inscritos, no dejara eliminar usuarios inexistentes
            DeleteUserFromActivitie(int user_id, int activitie_id)
            If true return 1 else return -1
         */

        #endregion

        #region Actividades

        public List<Actividad> GetActivities()
        {
            List<Actividad> actividades = new List<Actividad>();
            string query = "SELECT * FROM actividad";
            commandActivity = new MySqlCommand(query, connection);

            using (MySqlDataReader reader = commandActivity.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    string nombre = reader.GetString("nombre");
                    string descripcion = reader.GetString("descripcion");
                    int capacidad_maxima = reader.GetInt32("capacidad_maxima");

                    try
                    {
                        Actividad actividad = new Actividad(id, nombre, descripcion, capacidad_maxima);
                        actividades.Add(actividad);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                reader.Close();
            }
            return actividades;
        }

        /// <summary>
        /// A partir de la id de una actividad devuelve la propia actividad y los usuarios inscritos en ella
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Actividad GetActivityById(int id)
        {
            //GET ACTIVITY BY ID
            Actividad actividad = new Actividad();

            string query = $"SELECT * FROM actividad WHERE id = {id}";
            commandActivity = new MySqlCommand(query, connection);

            using (MySqlDataReader reader = commandActivity.ExecuteReader())
            {
                if (reader.Read())
                {
                    int id_activity = reader.GetInt32("id");
                    string nombre = reader.GetString("nombre");
                    string descripcion = reader.GetString("descripcion");
                    int capacidad_maxima = reader.GetInt32("capacidad_maxima");

                    try
                    {
                        actividad = new Actividad(id_activity, nombre, descripcion, capacidad_maxima);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                reader.Close();
            }

            //GET USERS FROM ACTIVITY
            query = $"SELECT * FROM usuario WHERE id IN (SELECT user_id FROM usuario_actividad WHERE act_id = {id})";
            commandActivity = new MySqlCommand(query, connection);

            using (MySqlDataReader reader = commandActivity.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id_user = reader.GetInt32("id");
                    string dni = reader.GetString("dni");
                    string nombre = reader.GetString("nombre");
                    int edad = reader.GetInt32("edad");
                    string email = reader.GetString("email");

                    try
                    {
                        Usuario usuario = new Usuario(id_user, dni, nombre, edad, email);
                        actividad.Usuarios.Add(usuario);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                reader.Close();
            }

            Console.WriteLine("Actividad: " + actividad.ToString());
            return actividad;
        }

        public bool SetActivity(Actividad actividad)
        {
            string query = $"INSERT INTO actividad (nombre, descripcion, capacidad_maxima) VALUES ('{actividad.Nombre}', '{actividad.Descripcion}', {actividad.Capacidad_maxima})";
            command = new MySqlCommand(query, connection);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error al insertar actividad: " + ex.Message);
                return false;
            }
            return true;
        }

        public bool UpdateActivity(Actividad actividad)
        {
            string query = $"UPDATE actividad SET nombre = '{actividad.Nombre}', descripcion = '{actividad.Descripcion}', capacidad_maxima = {actividad.Capacidad_maxima} WHERE id = {actividad.Id}";
            command = new MySqlCommand(query, connection);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error al actualizar actividad: " + ex.Message);
                return false;
            }

            return true;
        }

        public bool DeleteActivity(int id)
        {
            string query = $"DELETE FROM actividad WHERE id = {id}";
            command = new MySqlCommand(query, connection);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error al eliminar actividad: " + ex.Message);
                return false;
            }

            return true;
        }
        /*
         TODO:
            Return json(list<actividad> + list<usuario>)

            GetActivitieUsers(int activitie_id)
            Return json(List<actividad>)
         */

        #endregion


        public void CloseConnection()
        {
            connection.Close();
        }
    }
}
