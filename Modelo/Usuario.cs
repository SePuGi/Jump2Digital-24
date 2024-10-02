using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Modelo
{
    public class Usuario
    {
        int id;
        string dni;
        string nombre;
        int edad;
        string email;

        List<Actividad> actividad;

        public Usuario(int id, string dni, string nombre, int edad, string email)
        {
            Id = id;
            DNI = dni;
            Nombre = nombre;
            Edad = edad;
            Email = email;

            actividad = new List<Actividad>();
        }
        public Usuario() { }

        public int Id
        {
            get => id;
            set => id = value;
        }

        public string DNI
        {
            get => dni;
            set
            {
                if (ValidarDNI(value))
                    dni = value;
                else
                    throw new Exception("El DNI no es válido");
            }
        }

        public string Nombre
        {
            get => nombre;
            set
            {
                if (ValidarNombre(value))
                    nombre = value;
                else
                    throw new Exception("El nombre de usuario no es válido");
            }
        }
        
        public int Edad
        {
            get => edad;
            set
            {
                if (ValidarEdad(value))
                    edad = value;
                else
                    throw new Exception("La edad no es válida");
            }
        }
        public string Email
        {
            get => email;
            set
            {
                if (ValidarEmail(value))
                    email = value;
                else
                    throw new Exception("El email no es válido");
            }
        }

        public List<Actividad> Actividad
        {
            get => actividad;
            set => actividad = value;
        }

        //Validaciones
        public bool ValidarDNI(string value)
        {
            //TODO: Validar DNI con calculo de letra
            Regex dni = new Regex("^[0-9]{8}[A-Z]$");
            if (dni.IsMatch(value))
            {
                return true;
            }
            return false;
        }

        public bool ValidarNombre(string value)
        {
            if (value.Length > 0 && value.Length < 10)
            {
                return true;
            }
            return false;
        }

        public bool ValidarApellidos(string value)
        {
            if (value.Length > 0 && value.Length < 30)
            {
                return true;
            }
            return false;
        }

        public bool ValidarEdad(int value)
        {
            if (value > 0 && value < 120)
            {
                return true;
            }
            return false;
        }

        public bool ValidarEmail(string value)
        {
            Regex email = new Regex("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$");
            if (email.IsMatch(value))
            {
                return true;
            }
            return false;
        }

        public bool ValidarTelefono(string value)
        {
            Regex telefono = new Regex("^[0-9]{9}$");
            if (telefono.IsMatch(value))
            {
                return true;
            }
            return false;
        }


        public override string ToString()
        {
            return $"ID: {Id}, DNI: {DNI}, Nombre: {Nombre}, Edad: {Edad}, Email: {Email}";
        }


    }
}
