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
        // Acción GET: api/MyApi
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

        // Acción GET: api/MyApi/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Console.WriteLine("GET user by id");

            try
            {
                Usuario usuario = DataBase.GetUserById(id);

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

        // Acción POST: api/MyApi
        [HttpPost]
        public IActionResult Post([FromBody] JsonDocument value)
        {
            Console.WriteLine("POST user");

            string nombre = value.RootElement.GetProperty("nombre").GetString();
            string dni = value.RootElement.GetProperty("dni").GetString();
            int edad = value.RootElement.GetProperty("edad").GetInt32();
            string email = value.RootElement.GetProperty("email").GetString();

            try { 
                Usuario usuario = new Usuario(0, dni, nombre, edad, email);

                bool result = DataBase.SetUser(usuario);

                if (result)
                {
                    //success
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
            catch(Exception ex)
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

        //Acción PUT: api/MyApi
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

        //Acción DELETE: api/MyApi
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
                        message = "Usuario eliminado"
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
