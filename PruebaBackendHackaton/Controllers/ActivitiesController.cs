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
        //Action Get: api/Activity
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
            catch(Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    resource = "GET",
                    message = "No se encontraron actividades",
                    data = ex.Message
                });

            }
        }

        //Action Get: api/Activity/5
        //Obtener todos los usuarios de una actividad 
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Console.WriteLine("GET activity by id");
            try
            {
                Actividad activity = DataBase.GetActivityById(id);
                return Ok(new
                {
                    success = true,
                    resource = "GET",
                    message = "Actividad encontrada",
                    data = activity
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    resource = "GET",
                    message = "No se encontró la actividad",
                    data = ex.Message
                });
            }
        }

        //Action Post: api/Activity
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

                if(result)
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
            catch(Exception ex)
            {
                return Ok(new
                {
                    succes = false,
                    resource = "POST",
                    message = "No se pudo insertar la actividad",
                    data = ex.Message
                });
            }
            
        }

        //Action Put: api/Activity
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
            catch(Exception ex)
            {
                return Ok(new {
                    success = false,
                    resource = "PUT",
                    message = "No se pudo actualizar la actividad",
                    data = ex.Message
                });
            }
        }

        //Action Delete: api/Activity
        [HttpDelete]
        public IActionResult Delete([FromBody] JsonDocument value)
        {
            Console.WriteLine("DELETE activity");
            
            try
            {
                int id = value.RootElement.GetProperty("id").GetInt32();

                bool result = DataBase.DeleteActivity(id);

                if (result)
                {
                    return Ok(new
                    {
                        success = true,
                        resource = "DELETE",
                        message = "Actividad eliminada",
                        data = id
                    });
                }
                else
                    throw new Exception("No se pudo eliminar la actividad");
            }
            catch(Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    resource = "DELETE",
                    message = "No se pudo eliminar la actividad",
                    data = ex.Message
                });
            }
        }
    }
}
