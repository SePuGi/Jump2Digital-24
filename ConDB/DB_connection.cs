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

        /// <summary>
        /// Constructor de la clase DB_connection: Inicializa la conexión a la base de datos
        /// </summary>
        /// <param name="server"></param>
        /// <param name="database"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
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

        /// <summary>
        /// Devuelve una lista con todos los usuarios
        /// </summary>
        /// <returns></returns>
        public List<Usuario> GetUsers()
        {
            List<Usuario> usuarios = new List<Usuario>();

            string query = "SELECT * FROM usuario";
            commandUser = new MySqlCommand(query, connection);

            using (MySqlDataReader reader = commandUser.ExecuteReader())
            {
                
                while (reader.Read())
                {
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

        /// <summary>
        /// Devuelve un usuario a partir de su id con las actividades en las que está inscrito
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Usuario GetUserById(int id)
        {
            //obtenemos usuario
            string query = $"SELECT * FROM usuario WHERE id = {id}";
            commandUser = new MySqlCommand(query, connection);

            Usuario usuario = null;

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
                        usuario = new Usuario(id_user, dni, nombre, edad, email);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                reader.Close();
            }

            if(usuario == null)
                return null;

            //si el usuario existe, obtenemos sus actividades
            query = $"SELECT * FROM actividad WHERE id IN (SELECT act_id FROM usuario_actividad WHERE user_id = {id})";
            commandUser = new MySqlCommand(query, connection);

            using (MySqlDataReader reader = commandUser.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id_activity = reader.GetInt32("id");
                    string nombre = reader.GetString("nombre");
                    string descripcion = reader.GetString("descripcion");
                    int capacidad_maxima = reader.GetInt32("capacidad_maxima");

                    try
                    {
                        Actividad actividad = new Actividad(id_activity, nombre, descripcion, capacidad_maxima);
                        usuario.Actividad.Add(actividad);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                reader.Close();
            }

            return usuario;

        }

        /// <summary>
        /// Crea un nuevo usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Elimina un usuario de una actividad
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="activity_id"></param>
        /// <returns></returns>
        public int RemoveUserFromActivity(int user_id, int activity_id)
        {
            string query = $"DELETE FROM usuario_actividad WHERE user_id = {user_id} AND act_id = {activity_id}";
            command = new MySqlCommand(query, connection);

            try
            {
                int result = command.ExecuteNonQuery();

                if (result == 0)         //El usuario no estaba inscrito
                    return 1;
                else if (result == 1)    //Usuario eliminado de la actividad
                    return 0;
                else
                    return 2;            //Error
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error al eliminar usuario de actividad: " + ex.Message);
                return 2;
            }
        }

        /// <summary>
        /// Actualiza la información de un usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Elimina un usuario de la base de datos y de todas las actividades en las que esté inscrito
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteUser(int id)
        {
            string query = $"DELETE FROM usuario_actividad WHERE user_id = {id}";
            command = new MySqlCommand(query, connection);

            try
            {
                if(command.ExecuteNonQuery() != -1)
                {
                    query = $"DELETE FROM usuario WHERE id = {id}";
                    command = new MySqlCommand(query, connection);
                    command.ExecuteNonQuery();
                }
                else
                {
                    return false;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error al eliminar usuario: " + ex.Message);
                return false;
            }

            return true;
        }

        #endregion

        #region Actividades

        /// <summary>
        /// Devuelve una lista con todas las actividades
        /// </summary>
        /// <returns></returns>
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

            return actividad;
        }

        /// <summary>
        /// Crea una nueva actividad
        /// </summary>
        /// <param name="actividad"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 0 => ya esta inscrito
        /// 1 => inscrito correctamente
        /// 2 => error
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="activity_id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public byte AddUserToActivity(int user_id, int activity_id)
        {
            string query_total_inscritos = $"SELECT COUNT(*) FROM usuario_actividad WHERE act_id = {activity_id}";
            string query_capacidad_maxima = $"SELECT capacidad_maxima FROM actividad WHERE id = {activity_id}";

            command = new MySqlCommand(query_total_inscritos, connection);
            int total_inscritos = Convert.ToInt32(command.ExecuteScalar());

            command = new MySqlCommand(query_capacidad_maxima, connection);
            int capacidad_maxima = Convert.ToInt32(command.ExecuteScalar());

            if (total_inscritos >= capacidad_maxima) //Capacidad maxima de la actividad alcanzada
                return 3;

            string query = $"INSERT INTO usuario_actividad (user_id, act_id) SELECT {user_id}, {activity_id} " +
                $"where NOT EXISTS (select 1 from usuario_actividad where user_id = {user_id} AND act_id = {activity_id})"; 

            command = new MySqlCommand(query, connection);

            try
            {
                int result = command.ExecuteNonQuery();

                if(result == 0)         //El usuario ya está inscrito
                    return 0;
                else if(result == 1)    //Usuario inscrito correctamente
                    return 1;
                else
                    throw new Exception("Error al inscribir usuario en actividad");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error al insertar usuario en actividad: " + ex.Message);
                return 2;
            }
        }

        /// <summary>
        /// Actualiza la información de una actividad
        /// </summary>
        /// <param name="actividad"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Elimina una actividad de la base de datos si no hay usuarios inscritos en ella
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteActivity(int id)
        {
            //eliminar actividad si no hay registros de esta en usuario_actividad
            string query = $"DELETE FROM actividad WHERE id = {id} AND NOT EXISTS (SELECT 1 FROM usuario_actividad WHERE act_id = {id})";
            command = new MySqlCommand(query, connection);

            try
            {
                int result = command.ExecuteNonQuery();

                return result;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error al eliminar actividad: " + ex.Message);
                return 2;
            }
        }

        #endregion
    }
}
