using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Modelo;
using System.Collections.Generic;
using System.Text.Json;

namespace PruebaBackendHackaton.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        /// <summary>
        /// Devuelve una lista con todas las actividades
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            Console.WriteLine("GET activity");
            try
            {
                List<Actividad> activities = DataBase.GetActivities();
                return Ok(new
                {
                    success = true,
                    resource = "GET",
                    message = "Actividades encontradas",
                    data = activities
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    resource = "GET",
                    message = "No se encontraron actividades",
                    data = ex.Message
                });

            }
        }

        /// <summary>
        /// Devuelva una actividad especifica con todos los inscritos
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Console.WriteLine("GET activity by id");
            try
            {
                Actividad activity = DataBase.GetActivityById(id);
                Console.WriteLine(activity.ToString());
                if(activity.Nombre != null && activity.Nombre != "")
                    return Ok(new
                    {
                        success = true,
                        resource = "GET",
                        message = "Actividad encontrada",
                        data = activity
                    });

                else
                    throw new Exception("No se encontró la actividad");
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    resource = "GET",
                    message = "No se encontró la actividad",
                    data = ex.Message
                });
            }
        }

        /// <summary>
        /// Exporta todas las actividades a un archivo JSON
        /// </summary>
        /// <returns></returns>
        [HttpGet("export")]
        public IActionResult ExportActivities()
        {
            Console.WriteLine("GET export activities");
            try
            {
                List<Actividad> activities = DataBase.GetActivities();

                string json = JsonSerializer.Serialize(activities);

                var fileName = "activities.json";
                var bytes = System.Text.Encoding.UTF8.GetBytes(json);

                return File(bytes, "application/json", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    resource = "GET",
                    message = "No se han podido exportar las actividades",
                    data = ex.Message
                });
            }
        }

        /// <summary>
        /// Crear una actividad
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] JsonDocument value)
        {
            Console.WriteLine("POST activity");
            string nombre = value.RootElement.GetProperty("nombre").GetString();
            string descripcion = value.RootElement.GetProperty("descripcion").GetString();
            int capacidad_maxima = value.RootElement.GetProperty("capacidad_maxima").GetInt32();

            try
            {
                Actividad actividad = new Actividad(0, nombre, descripcion, capacidad_maxima);

                bool result = DataBase.SetActivity(actividad);

                if (result)
                {
                    return Ok(new
                    {
                        success = true,
                        resource = "POST",
                        message = "Actividad insertada",
                        data = actividad
                    });
                }
                else
                    throw new Exception("No se pudo insertar la actividad");
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    succes = false,
                    resource = "POST",
                    message = "No se pudo insertar la actividad",
                    data = ex.Message
                });
            }

        }

        /// <summary>
        /// 0 => ya esta inscrito
        /// 1 => inscrito correctamente
        /// 2 => error
        /// 3 => maximo de registros
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="activity_id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("{id_actividad}")]
        public IActionResult PostUserToActivity(int id_actividad, [FromBody] JsonDocument value)
        {
            Console.WriteLine("POST user to activity");
            int id_usuario = value.RootElement.GetProperty("id").GetInt32();

            try
            {
                int result = DataBase.AddUserToActivity(id_usuario, id_actividad);

                if (result == 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        resource = "POST",
                        message = "El usuario ya estaba inscrito",
                        data = new
                        {
                            id_actividad = id_actividad,
                            id_usuario = id_usuario
                        }
                    });
                }
                else if (result == 1)
                {
                    Console.WriteLine("Usuario agregado a la actividad");
                    return Ok(new
                    {
                        success = true,
                        resource = "POST",
                        message = "Usuario agregado a la actividad",
                        data = new
                        {
                            id_actividad = id_actividad,
                            id_usuario = id_usuario
                        }
                    });
                }
                else if (result == 3)
                {
                    return BadRequest(new
                    {
                        success = false,
                        resource = "POST",
                        message = "La actividad tiene el maximo de registros",
                        data = new
                        {
                            id_actividad = id_actividad,
                            id_usuario = id_usuario
                        }
                    });
                }
                else
                    throw new Exception("No se pudo agregar el usuario a la actividad");
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    resource = "POST",
                    message = "No se pudo agregar el usuario a la actividad",
                    data = ex.Message
                });
            }
        }


        /// <summary>
        /// Actualizar una actividad
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Put([FromBody] JsonDocument value)
        {
            Console.WriteLine("PUT activity");
            int id = value.RootElement.GetProperty("id").GetInt32();
            string nombre = value.RootElement.GetProperty("nombre").GetString();
            string descripcion = value.RootElement.GetProperty("descripcion").GetString();
            int capacidad_maxima = value.RootElement.GetProperty("capacidad_maxima").GetInt32();

            try
            {
                Actividad actividad = new Actividad(id, nombre, descripcion, capacidad_maxima);

                bool result = DataBase.UpdateActivity(actividad);

                if (result)
                {
                    return Ok(new
                    {
                        success = true,
                        resource = "PUT",
                        message = "Actividad actualizada",
                        data = actividad
                    });
                }
                else
                    throw new Exception("No se pudo actualizar la actividad");
            }
            catch (Exception ex)
            {
                return BadRequest(new {
                    success = false,
                    resource = "PUT",
                    message = "No se pudo actualizar la actividad",
                    data = ex.Message
                });
            }
        }

        /// <summary>
        /// Elimina una actividad siempre que no tenga usuarios registrados en ella
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult Delete([FromBody] JsonDocument value)
        {
            Console.WriteLine("DELETE activity");

            try
            {
                int id = value.RootElement.GetProperty("id").GetInt32();

                int result = DataBase.DeleteActivity(id);

                if (result == 0)
                    throw new Exception("No se puede eliminar la actividad, aún hay usuarios inscritos");
                else if (result == 1)
                {
                    return Ok(new
                    {
                        success = true,
                        resource = "DELETE",
                        message = "Actividad eliminada",
                        data = id
                    });
                }
                else if (result == 2)
                    throw new Exception("No se pudo eliminar la actividad");
                else
                    throw new Exception("Error desconocido");
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    resource = "DELETE",
                    message = "No se pudo eliminar la actividad",
                    data = ex.Message
                });
            }
        }

        /// <summary>
        /// Importar actividades desde un archivo JSON
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("import")]
        public IActionResult ImportActivities(IFormFile file)
        {
            Console.WriteLine("ARCHIVO RECIBIDO");
            
            if (file == null || file.Length == 0)
                return BadRequest("Por favor, selecciona un archivo.");

            var extension = Path.GetExtension(file.FileName);
            if (extension.ToLower() != ".json")
                return BadRequest("Solo se permiten archivos JSON.");

            Console.WriteLine("POST import activities");
            try
            { 
                List<Actividad> activities = new List<Actividad>();

                string fileContent;
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    fileContent = reader.ReadToEnd();
                }

                using (JsonDocument jsonDocument = JsonDocument.Parse(fileContent))
                {
                    JsonElement root = jsonDocument.RootElement;
                    if (root.ValueKind == JsonValueKind.Array)
                    {
                        foreach (JsonElement element in root.EnumerateArray())
                        {
                            Actividad actividad = new Actividad
                            (
                                0,
                                element.GetProperty("nombre").GetString(),
                                element.GetProperty("descripcion").GetString().Replace("'", ""),
                                element.GetProperty("capacidad_maxima").GetInt32()
                            );
                            activities.Add(actividad);
                        }
                    }
                    else
                        return BadRequest(new
                        {
                            success = false,
                            resource = "POST import",
                            message = "No se pudieron importar las actividades",
                            data = "No se encontró un array de actividades"
                        });
                }
                
                bool result = DataBase.ImportActivities(activities);

                if (result)
                {
                    return Ok(new
                    {
                        success = true,
                        resource = "POST",
                        message = "Actividades importadas",
                        data = activities
                    });
                }
                else
                    throw new Exception("No se pudieron importar las actividades");
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    resource = "POST",
                    message = "No se pudieron importar las actividades",
                    data = ex.Message
                });
            }
        }
    }
}
