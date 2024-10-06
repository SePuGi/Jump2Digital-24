using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class Actividad
    {
        /*
            - id
           - nombre
           - descripción 
           - capacidad_maxima
        */
        int id;
        string nombre;
        string descripcion;
        int capacidad_maxima;

        List<Usuario> usuarios;

        public Actividad(int id, string nombre, string descripcion, int capacidad_maxima)
        {
            Id = id;
            Nombre = nombre;
            Descripcion = descripcion;
            Capacidad_maxima = capacidad_maxima;

            usuarios = new List<Usuario>();
        }
        public Actividad() { usuarios = new List<Usuario>(); }

        public int Id
        {
            get => id;
            set => id = value;
        }

        public string Nombre
        {
            get => nombre;
            set
            {
                if (ValidarNombre(value))
                    nombre = value;
                else
                    throw new Exception("El nombre de la actividad no es válido");
            }
        }

        public string Descripcion
        {
            get => descripcion;
            set
            {
                if (ValidarDescripcion(value))
                    descripcion = value;
                else
                    throw new Exception("La descripción de la actividad no es válida");
            }
        }

        public int Capacidad_maxima
        {
            get => capacidad_maxima;
            set {
                if(ValidarCapacidad(value))
                    capacidad_maxima = value;
                else
                    throw new Exception("La capacidad máxima no es válida");
            }
        }

        public List<Usuario> Usuarios
        {
            get => usuarios;
            set => usuarios = value;
        }

        //Validaciones
        public bool ValidarNombre(string value)
        {
            if (value.Length > 0 && value.Length < 50)
            {
                return true;
            }
            return false;
        }

        public bool ValidarDescripcion(string value)
        {
            if (value.Length > 0 && value.Length < 500)
            {
                return true;
            }
            return false;
        }

        public bool ValidarCapacidad(int value)
        {
            if (value > 1)
            {
                return true;
            }
            return false;
        }

        public override string? ToString()
        {
            string value = "Id: " + Id + " Nombre: " + Nombre + " Descripción: " + Descripcion + " Capacidad máxima: " + Capacidad_maxima + " Usuarios: " ;

            foreach (Usuario usuario in Usuarios)
            {
                value += usuario.ToString() + " ";
            }

            return value;
        }
    }
}
