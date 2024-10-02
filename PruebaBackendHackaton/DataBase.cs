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

        // RECOGER LOS VALORES DE LA CONNECTIONSTRING DE UN ARCHIVO DE CONFIGURACIÓN
        public static void InitializeConnection()
        {
            //OBTENER DATOS DE UN ARCHIVO DE CONFIGURACIÓN
            connection = new DB_connection("localhost", "hackaton", "admin", "admin");
            connection_user = new DB_connection("localhost", "hackaton", "admin", "admin");
            connection_activity = new DB_connection("localhost", "hackaton", "admin", "admin");
        }

        public static void CloseConnection()
        {
            connection.CloseConnection();
        }

        #region Usuarios

        public static List<Usuario> GetUsers()
        {
            if (connection_user == null)
            {
                throw new System.Exception("No se ha inicializado la conexión a la base de datos");
            }

            var usuarios = connection_user.GetUsers();

            return usuarios;
        }

        public static Usuario GetUserById(int id)
        {
            if (connection_user == null)
            {
                throw new System.Exception("No se ha inicializado la conexión a la base de datos");
            }

            var usuario = connection_user.GetUserById(id);

            return usuario;
        }

        public static bool SetUser(Usuario usuario)
        {
            if (connection == null)
            {
                throw new System.Exception("No se ha inicializado la conexión a la base de datos");
            }

            return connection.SetUser(usuario);
        }

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

        public static bool DeleteUser(int id)
        {
            if (connection == null)
            {
                throw new System.Exception("No se ha inicializado la conexión a la base de datos");
            }

            return connection.DeleteUser(id);
        }
        /*

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

        public static bool SetActivity(Actividad actividad)
        {
            if (connection == null)
            {
                throw new System.Exception("No se ha inicializado la conexión a la base de datos");
            }

            return connection.SetActivity(actividad);
        }

        public static bool UpdateActivity(Actividad actividad)
        {
            if (connection == null)
            {
                throw new System.Exception("No se ha inicializado la conexión a la base de datos");
            }

            return connection.UpdateActivity(actividad);
        }

        public static bool DeleteActivity(int id)
        {
            if (connection == null)
            {
                throw new System.Exception("No se ha inicializado la conexión a la base de datos");
            }

            return connection.DeleteActivity(id);
        }
        /*
         TODO:
            Return json(list<actividad> + list<usuario>)

            GetActivitieUsers(int activitie_id)
            Return json(List<actividad>)
         */
        #endregion
    }
}
