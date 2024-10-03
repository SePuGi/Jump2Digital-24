using ConDB;
using Modelo;
using MySql.Data.MySqlClient;
using System.Runtime.CompilerServices;

namespace PruebaBackendHackaton
{
    public class DataBase
    {
        private static DB_connection connection_user;       //Gets de user
        private static DB_connection connection_activity;   //Gets de activity
        private static DB_connection connection;            //Sets, updates, deletes

        public static void InitializeConnection()
        {
            string[] lines = System.IO.File.ReadAllLines("../props/props_connection.xml");

            string server = lines[1].Split("\"")[1];
            string database = lines[2].Split("\"")[1];
            string user = lines[3].Split("\"")[1];
            string password = lines[4].Split("\"")[1];

            connection = new DB_connection(server, database, user, password);
            connection_user = new DB_connection(server, database, user, password);
            connection_activity = new DB_connection(server, database, user, password);
        }

        #region Usuarios

        /// <summary>
        /// Obtiene una lista con todos los usuarios
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public static List<Usuario> GetUsers()
        {
            if (connection_user == null)
            {
                throw new System.Exception("No se ha inicializado la conexión a la base de datos");
            }

            var usuarios = connection_user.GetUsers();

            return usuarios;
        }

        /// <summary>
        /// Devuelve un usuario a partir de su id con las actividades en las que está inscrito
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public static Usuario GetUserById(int id)
        {
            if (connection_user == null)
            {
                throw new System.Exception("No se ha inicializado la conexión a la base de datos");
            }

            var usuario = connection_user.GetUserById(id);

            return usuario;
        }

        /// <summary>
        /// Crea un usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public static bool SetUser(Usuario usuario)
        {
            if (connection == null)
            {
                throw new System.Exception("No se ha inicializado la conexión a la base de datos");
            }

            return connection.SetUser(usuario);
        }

        /// <summary>
        /// 0 => Usuario eliminado de la actividad
        /// 1 => Usuario no estaba inscrito
        /// 2 => Error
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="activity_id"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public static int RemoveUserFromActivity(int user_id, int activity_id)
        {
            if (connection == null)
            {
                throw new System.Exception("No se ha inicializado la conexión a la base de datos");
            }

            return connection.RemoveUserFromActivity(user_id, activity_id);
        }

        /// <summary>
        /// Actualiza la información de un usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        /// <exception cref="Exception"></exception>
        public static bool UpdateUser(Usuario usuario)
        {
            if (connection == null)
            {
                throw new System.Exception("No se ha inicializado la conexión a la base de datos");
            }

            if (connection_user.GetUsers().Exists(x => x.Id == usuario.Id))
                return connection.UpdateUser(usuario);
            else
                throw new Exception("El usuario no existe");
        }

        /// <summary>
        /// Elimina un usuario siempre y cuando no esté inscrito en ninguna actividad
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public static bool DeleteUser(int id)
        {
            if (connection == null)
            {
                throw new System.Exception("No se ha inicializado la conexión a la base de datos");
            }

            return connection.DeleteUser(id);
        }

        #endregion

        #region Actividades

        /// <summary>
        /// Devuelve una lista con todas las actividades
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public static List<Actividad> GetActivities()
        {
            if (connection_activity == null)
            {
                throw new System.Exception("No se ha inicializado la conexión a la base de datos");
            }
               
            var activities = connection_activity.GetActivities();

            return activities;
        }

        /// <summary>
        /// A partir de la id de una actividad devuelve la propia actividad y los usuarios inscritos en ella
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Actividad GetActivityById(int id)
        {
            if (connection_activity == null)
            {
                throw new System.Exception("No se ha inicializado la conexión a la base de datos");
            }

            var activity = connection_activity.GetActivityById(id);

            return activity;
        }

        /// <summary>
        /// Crea una actividad
        /// </summary>
        /// <param name="actividad"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public static bool SetActivity(Actividad actividad)
        {
            if (connection == null)
            {
                throw new System.Exception("No se ha inicializado la conexión a la base de datos");
            }

            return connection.SetActivity(actividad);
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
        public static int AddUserToActivity(int user_id, int activity_id)
        {
            if (connection == null)
            {
                throw new System.Exception("No se ha inicializado la conexión a la base de datos");
            }

            return connection.AddUserToActivity(user_id, activity_id);
        }


        /// <summary>
        /// Actualizar la información de una actividad
        /// </summary>
        /// <param name="actividad"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public static bool UpdateActivity(Actividad actividad)
        {
            if (connection == null)
            {
                throw new System.Exception("No se ha inicializado la conexión a la base de datos");
            }

            return connection.UpdateActivity(actividad);
        }

        /// <summary>
        /// Eliminar una actividad siempre y cuando no tenga usuarios inscritos
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public static int DeleteActivity(int id)
        {
            if (connection == null)
            {
                throw new System.Exception("No se ha inicializado la conexión a la base de datos");
            }

            return connection.DeleteActivity(id);
            
        }

        #endregion
    }
}
