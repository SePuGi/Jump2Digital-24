# PRUEBA BACKEND HACKATON 2024
##### Proyecto realizado por: Sergi Puiggrós Giralt


## Configuración de la Base de datos

He elegido montar la base de datos en MySQL para hacer las pruebas utilizando 
el XAMPP ya que lo manejo mejor y me ha permitido crear rapidamente 
la base de datos que necesitaba para este proyecto.


En la carpeta **SQL** de este proyecto se encuentra el script **script_bbdd.sql** 
para crear la base de datos y añadir un un par de usuarios de prueba.

### Tablas de la BBDD
  - `usuario` : Información básica de cada usuario
    - `id`      : Id autoincrementado por la base de datos.
    - `dni`     : Dni del usuario (se valida en el modelo).
    - `nombre`  : Nombre del usuario.
    - `edad`    : Edad del usuario.
    - `email`   : Email del usuario (se valida en el modelo).
  
  - `actividad` : Información básica de las actividades
    - `id`                  : Id autoincrementado por la base de datos.
    - `nombre`              : Nombre de la actividad.
    - `descripcion`         : Descripción de la actividad.
    - `capacidad_maxima`    : Capacidad máxima de usuarios que pueden inscribirse en la actividad.
  
  - `usuario-actividad` : Tabla intermedia para controlar las inscripciones a las actividades (combinación de ID's único para evitar que un usuario se pueda inscribir más de una vez a una actividad)
    - `user_id` : Id del usuario.
    - `act_id`  : Id de la actividad .


## Modelo que usa la API

La API usa un modelo parecido a las tablas de la BBDD, 
solo que los usuarios tienen una lista de las actividades en las que estan inscritos 
y las actividades tienen una lista de los usuarios inscritos a ella.

- Usuario:
  - `Id`: El id no se valida ya que la bbdd nos asegura que será único y incremental.
  - `Dni`: Se valida comprobando que tenga los numeros y letra correspondientes.
  - `Nombre`: Se comprueba la cantidad de caracteres del nombre (max: 10).
  - `Edad`: Se comprueba que esté entre 0 y 120.
  - `Email`: Se valdia que el email tenga el formato que deberia tener (email@email.com).
  - `Lista de actividades`: Una lista de las actividades del usuario
- Actividad: 
  - `Id`: El id no se valida ya que la bbdd nos asegura que será único y incremental.
  - `Nombre`: Se comprueba la cantidad de caracteres del nombre (max: 50).
  - `Descripción`: Se comprueba la cantidad de caracteres de la descripción (max: 500).
  - `Cantidad_maxima`: Se comprueba que no sea **0** ni **negativo**.
  - `Lista de usuarios`: Lista de los usuarios que estan inscritos a la actividad.
  
  <br>
# COMO EJECUTAR LA API

Para ejecutar la *API* he preparado un script, que esta en la raiz del proyecto *(gestor_actividades.bat)*, que va a restaurar las dependencias necesarias, compilar el proyecto y ejectuarlo.

> ⚠️ Antes de ejecutar la API hay que modificar el archivo de propiedades para conectarse a la base de datos. Este archivo se encuentra en "./API/props/props_connection.xml", aqui simplemente seria cambiar el valor de cada etiqueta por el que necesites. Se debe indicar:
> - `server` : Dirección en la que se encuentra la bbdd (localhost en mi caso).
> - `database` : Nombre que tiene la base de datos.
> - `user` : El usuario con los privilegios necesarios para hacer cambios en la BBDD.
> - `password` : Contraseña del usuario.
> <br>

## ENDPOINTS

> La función de la API es gestionar usuarios y actividades, asi que aparte de los endpoints básicos de una API se ha añadido alguno más para facilitar la gestión.
> El base path es `https://localhost:7179/api/`.


Esta API soporta los siguientes *endpoints*:

### USUARIOS:
- **GET** `/User`: Devuelve todos los usuarios (solo devuelve los usuarios, las actividades no!)
  - Return: 
    - Una lista de todos los usuarios.
  
- **GET** `/User/{id}`: Devuelve el usuario especificado con las actividades en las que esta inscrito.
  - Return:
    - El usuario indicado con las actividades en las que se a inscrito.

- **POST** `/User`: Registrar un usuario
  > El body de la petición tiene que ser asi: <br> 
   > - {"nombre": "Usuario","dni": "47129665C","edad": 40,"email": "usuario@user.com"}
   - Petición: `/User` + `body {. . .}`
  - Return: 
    - Si la petición no se a completado con éxito devuelve un mensaje de error explicando porque.
    - En caso de que la petición sea exitosa devuelve la información del usuario que se acaba de registrar.
  

- **POST** `/User/{id}`: Elimina el usuario especificado de la actividad especificada
  >  El body de la petición tiene que ser asi: 
   > - {"activity_id":50}
   - Petición: `/User/{id}` + `body {"activity_id":50}`
   - Return:
     - Mensaje diciendo si se ha podido eliminar de la actividad.
     - Si se ha podido, devuelve el id del usuario y de la actividad.
   

- **PUT** `/User/{id}` : Actualiza la información de el usuario especificado 
  >  El body de la petición tiene que ser asi: <br>
  > - {"id":65, "nombre": "Usuario","dni": "47129665C","edad": 40,"email": "usuario@user.com"}
  - Petición: `/User/{id}` + `body {. . .}`
  - Return:  
    - Mensaje diciendo si se ha podido actualizar.
    - Devuelve la información del usuario actualizada.
  
- **DELETE** `/User` : Elimina un usuario y lo desvincula de todas las actividades a las que estaba inscrito
  >  El body de la petición tiene que ser asi: 
  > - {"id":50}
   - Petición: `/User` + `body {"id":50}`
   - Return:
     - Mensaje diciendo si se ha podido eliminar.
     - Si se ha podido, devuelve un mensaje de éxito.
   
<br><hr><br>
### ACTIVIDADES:

- **GET** `/Activities` : Devuelve una lista de todas las actividades.
  - Return: 
    - Todas las actividades.

- **GET** `/Activities/{id}` : Devuelve la actividad especificada con los usuarios inscritos en ella.
  - Return:
    - Informa si ha encontrado (o no) la actividad
    - Toda la información de la actividad
  
- **GET** `/Activities/export` : Devuelve todas las actividades en un archivo en formato JSON.
  - Return:
    - Devuelve un archivo con todas las actividades (solo las actividades)

- **POST** `/Activities` : Crea una actividad.
   >  El body de la petición tiene que ser asi: <br>
  > - {"nombre": "Actividad", "descripcion": "Descripción", "capacidad_maxima": 30}
  - Petición: `/Activities/{id}` + `body {. . .}`
  - Return:
     - Si la petición no se a completado con éxito devuelve un mensaje de error explicando porque.
      - En caso de que la petición sea exitosa devuelve la información de la actividad que se acaba de crear.
  
- **POST** `/Activities/{id_actividad}` : Inscribe un usuario a una actividad.
   >  El body de la petición tiene que ser asi: <br>
  > - {"id":65}
  - Petición: `/Activities/{id}` + `body {"id" : 5}`
  - Return:
    - Indicará si el usuario se a podido registrar correctamente.
    - _La API controla que un usuario no pueda inscribirse dos veces en la misma actividad o que la actividad no exceda la capacidad maxima._
  
- **POST** `/Activities/import` : A partir de un archivo JSON importa actividades.
  >  El archivo de la petición tiene que ser como el ejemplo proporcionado en `./SQL/actividades.json`
    - Return:
      - Devuelve un mensaje de error si no se a importado correctamente.
      - Si se a importado sin problemas, devolverá una lista de todas las actividades importadas.
  
- **PUT** `/Activities` : Actualiza la información de la actividad especificada.
>  El body de la petición tiene que ser asi: <br>
  > - {"id" : 36, "nombre": "Actividad", "descripcion": "Descripción de la actividad", "capacidad_maxima": 20}
  - Petición: `/Activities/{id}` + `body {. . .}`
  - Return:  
    - Mensaje diciendo si se ha podido actualizar.
    - Devuelve la información de la actividad actualizada.
  
- **DELETE** `/Activities` : Elimina la actividad especificada.
  >  El body de la petición tiene que ser asi: 
  > - {"id":50}
   - Petición: `/Activities` + `body {"id":50}`
   - Return:
     - Mensaje diciendo si se ha podido eliminar.
     - Si se ha podido eliminar, devuelve un mensaje de éxito.

<br><hr>