using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modelo;
using System.Text.Json;

namespace PruebaBackendHackaton.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// Devuelve una lista con todos los usuarios
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            Console.WriteLine("GET user");
            try
            {
                List<Usuario> usuarios = DataBase.GetUsers();

                return Ok(new
                {
                    success = true,
                    resource = "GET",
                    message = "Usuarios encontrados",
                    data = usuarios
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    resource = "GET",
                    message = "No se encontraron usuarios",
                    data = ex.Message
                });
            }
        }

        /// <summary>
        /// Obtiene información de un usuario especifico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Console.WriteLine("GET user by id");

            try
            {
                Usuario usuario = DataBase.GetUserById(id);

                if(usuario == null)
                    throw new Exception("Usuario no encontrado");

                return Ok(new
                {
                    success = true,
                    resource = "GET",
                    message = "Usuario encontrado",
                    data = usuario
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    resource = "GET",
                    message = "No se encontró el usuario",
                    data = ex.Message
                });
            }
        }

        /// <summary>
        /// Crear un nuevo usuario
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] JsonDocument value)
        {
            Console.WriteLine("POST user");

            string nombre = value.RootElement.GetProperty("nombre").GetString();
            string dni = value.RootElement.GetProperty("dni").GetString();
            int edad = value.RootElement.GetProperty("edad").GetInt32();
            string email = value.RootElement.GetProperty("email").GetString();

            try
            {
                Usuario usuario = new Usuario(0, dni, nombre, edad, email);

                bool result = DataBase.SetUser(usuario);

                if (result)
                {
                    return Ok(new
                    {
                        success = true,
                        resource = "POST",
                        message = "Usuario creado",
                        data = usuario
                    });
                }
                else
                    throw new Exception("Error al crear usuario");
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    resource = "POST",
                    message = "Error al crear usuario",
                    data = ex.Message
                });
            }
        }

        /// <summary>
        /// ELIMINAR UN USUARIO DE LA ACTIVIDAD
        /// 0 => Usuario eliminado de la actividad
        /// 1 => Usuario no estaba inscrito
        /// 2 => Error
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="activity_id"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost("{user_id}")]
        public IActionResult Post(int user_id, [FromBody] JsonDocument value)
        {
            Console.WriteLine("POST user to activity");

            try
            {
                int id_actividad = value.RootElement.GetProperty("activity_id").GetInt32();

                int result = DataBase.RemoveUserFromActivity(user_id, id_actividad);

                if(result == 0)
                {
                    return Ok(new
                    {
                        success = true,
                        resource = "POST",
                        message = "Usuario eliminado de la actividad correctamente",
                        data = new
                        {
                            id_usuario = user_id,
                            id_actividad = id_actividad
                        }
                    });
                }
                else if(result == 1)
                    throw new Exception("Usuario no estaba inscrito");
                else
                    throw new Exception("Error al eliminar usuario de actividad");
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    resource = "POST",
                    message = "Error al eliminar usuario de una actividad",
                    data = ex.Message
                });
            }
        }

        /// <summary>
        /// Actualiza la información de un usuario
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Put([FromBody] JsonDocument value)
        {
            Console.WriteLine("PUT user");

            int id = value.RootElement.GetProperty("id").GetInt32();
            string nombre = value.RootElement.GetProperty("nombre").GetString();
            string dni = value.RootElement.GetProperty("dni").GetString();
            int edad = value.RootElement.GetProperty("edad").GetInt32();
            string email = value.RootElement.GetProperty("email").GetString();

            try
            {
                Usuario usuario = new Usuario(id, dni, nombre, edad, email);

                bool result = DataBase.UpdateUser(usuario);

                if (result)
                {
                    return Ok(new
                    {
                        success = true,
                        resource = "PUT",
                        message = "Usuario actualizado",
                        data = usuario
                    });
                }
                else
                    throw new Exception("Error al actualizar usuario");
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    resource = "PUT",
                    message = "Error al actualizar usuario",
                    data = ex.Message
                });
            }
        }

        /// <summary>
        /// Elimina un usuario y lo desvincula de todas las actividades
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult Delete([FromBody] JsonDocument value)
        {
            Console.WriteLine("DELETE user");
            try
            {
                int id = value.RootElement.GetProperty("id").GetInt32();

                bool result = DataBase.DeleteUser(id);

                if (result)
                {
                    return Ok(new
                    {
                        success = true,
                        resource = "DELETE",
                        message = "Usuario eliminado y desvinculado de todas las actividades inscritas"
                    });
                }
                else
                    throw new Exception("Error al eliminar usuario");
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    resource = "DELETE",
                    message = "Error al eliminar usuario",
                    data = ex.Message
                });
            }

        }
    }
}
