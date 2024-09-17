/*
 * Miguel Ángel Martínez Jiménez
 * Versión: 2.0.
 * Fecha: Septiembre  2024.
 * Aplicación: Evaluación y Entrenamiento de la Atención.
 * 
 */


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Net;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.EMMA;
using static demorejilla.Tachar;


namespace demorejilla
{
    [SupportedOSPlatform("windows")]
    public class BaseDatos
    {
        //Nombre de la base de datos para el programa baseDatos.db
        //private readonly string rutaBD = Application.StartupPath + "BD" + Path.DirectorySeparatorChar + "baseDatos4.db";
        private readonly string rutaBD = new CarpetaLocal().carpetaBD() + "rejilla.db";
        private SqliteConnection conexion;

        private Usuarios usuarios;

        public BaseDatos()
        {
            conexion = new SqliteConnection("DataSource=" + rutaBD);
        }

        public BaseDatos(string ruta)
        {
            conexion = new SqliteConnection("DataSource=" + ruta);
        }

        //Crea el fichero con el nombre de la base de datos, si no existe en la solución.
        public void crearBD()
        {
            if (!File.Exists(rutaBD))
            {
                var file = File.Create(rutaBD);
                file.Close();
            }
        }

        //Abre la conexión con la base de datos
        public void conectarBD()
        {
            if (conexion.State != ConnectionState.Open)
            {
                conexion.Open();
            }
        }

        //Cierra la conexión con la base de datos
        public void cerrarConexiion()
        {
            if (conexion.State == ConnectionState.Open)
            {
                conexion.Close();
            }
        }


        /*
         * Creacion de las tablas para la base de datos
         */
        public void  crearTablaUsuarios()
        {
            string cadena = @"
            CREATE TABLE IF NOT EXISTS USUARIOS(
                Codigo INTEGER PRIMARY KEY, 
                Nombre TEXT NOT NULL, 
                Apellidos TEXT NOT NULL,
                Lateralidad TEXT, 
                Genero TEXT, 
                Deportes TEXT, 
                Edad TEXT, 
                Pais TEXT, 
                Posicion TEXT
            )";
            
            SqliteCommand comando = new SqliteCommand(cadena,conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD
        }


        private void crearTablaRejilla()
        {
            string cadena = @"
		    CREATE TABLE IF NOT EXISTS REJILLA (
			    Codigo INTEGER PRIMARY KEY,
			    Filas TEXT,
			    Columnas TEXT,
			    Errores INTEGER,
			    Aciertos INTEGER,
			    Fecha TEXT,
			    BotonesTotal TEXT,
			    TamañoBoton TEXT,
			    Tachar TEXT,
			    Intercambiar TEXT,
			    CantidadBotones INTEGER,
			    Observaciones TEXT,
			    TipoRejilla TEXT,
			    FKTipoRejilla INTEGER,
			    FKUsuarios INTEGER,
			    FKEmparejar INTEGER,
			    FKLInea INTEGER,
			    FKSonidos INTEGER,
                TiempoTranscurrido TEXT,
                ValorMedia TEXT,
                ValorVarianza TEXT,
                TiempoCorregido TEXT,
                TiempoCorreccionEfectuada TEXT,
                TiempoTareaPreliminar TEXT,
                TiempoValorMediaCorreccion TEXT,
                TiempoValorVarianzaCorreccion TEXT,
                TiempoMaximo TEXT,
                TiempoMinimo TEXT,
                TLimite	TEXT,
	            TAleatorio	TEXT,
                Tiempos	TEXT,
                TiemposLatencia	TEXT,
                FKColorFondo INTERGER,
                FKOrdenTachado INTEGER,
                FechaFormateada TEXT,
                TiksGrafica TEXT,
                TiksTiempo TEXT
		    )";

            SqliteCommand comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD
        }

        private void crearTablaNumerica()
        {
            string cadena = @"
            CREATE TABLE IF NOT EXISTS NUMERICA (
                Codigo INTEGER PRIMARY KEY,
                ValorInicial NUMERIC,
                Incremento NUMERIC,
                Orden TEXT
            )";
            SqliteCommand comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD
        }

        private void crearTablaAbecedario()
        {
            string cadena = @"
            CREATE TABLE IF NOT EXISTS ABECEDARIO (
                Codigo INTEGER PRIMARY KEY,
                ValorInicial TEXT,
                Tipo TEXT,
                Orden TEXT
            )";
            SqliteCommand comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD
        }

        private void crearTablaColores()
        {
            string cadena = @"
            CREATE TABLE IF NOT EXISTS COLORES (
                Codigo INTEGER PRIMARY KEY,
                CantidadBotones TEXT,
                ColoresFondo TEXT,
                ColoresTachar TEXT
            )";
            SqliteCommand comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD
        }

        private void crearTablaColoresFondo()
        {
            string cadena = @"
            CREATE TABLE IF NOT EXISTS COLORESFONDO (
                Codigo INTEGER PRIMARY KEY,
                ColoresPlantilla TEXT,
                ColoresElegidos TEXT
            )";
            SqliteCommand comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD
        }

        private void crearTablaEmparejar()
        {
            string cadena = @"
            CREATE TABLE IF NOT EXISTS EMPAREJAR (
                Codigo INTEGER PRIMARY KEY,
                Tiempo TEXT,
                Orden TEXT,
                Posicion TEXT
            )";
            SqliteCommand comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD
        }

        private void crearTablaImagenes()
        {
            string cadena = @"
            CREATE TABLE IF NOT EXISTS IMAGENES (
                Codigo INTEGER PRIMARY KEY,
                CantidadBotones TEXT,
                ImagenesRejilla BLOB,
                ImagenesTachar BLOB
            )";
            SqliteCommand comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD
        }

        private void crearTablaLetras()
        {
            string cadena = @"
            CREATE TABLE IF NOT EXISTS LETRAS (
                Codigo INTEGER PRIMARY KEY,
                CantidadBotones TEXT,
                Fuente TEXT,
                LetrasRejilla TEXT,
                LetrasTachar TEXT
            )";
            SqliteCommand comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD
        }

        private void crearTablaWindings()
        {
            string cadena = @"
            CREATE TABLE IF NOT EXISTS WINDINGS (
                Codigo INTEGER PRIMARY KEY,
                LetrasRejilla TEXT,
                LetrasTachar TEXT,
                Porcentaje TEXT
            )";
            SqliteCommand comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD
        }

        private void crearTablaSonidos()
        {
            string cadena = @"
            CREATE TABLE IF NOT EXISTS SONIDOS (
                Codigo INTEGER PRIMARY KEY,
                Metronomo TEXT,
                Canciones TEXT
            )";
            SqliteCommand comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD
        }

        private void crearTablaLineaDistractora()
        {
            string cadena = @"
            CREATE TABLE IF NOT EXISTS LINEADISTRACTORA (
                Codigo INTEGER PRIMARY KEY,
                Color TEXT,
                Grosor TEXT,
                Velocidad TEXT,
                TipoMovimiento TEXT
            )";
            SqliteCommand comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD
        }

        private void crearTablaIndices()
        {
            string cadena = @"
            CREATE TABLE IF NOT EXISTS INDICES (
                Codigo INTEGER PRIMARY KEY autoincrement,
                Abecedario INTEGER,
                Colores INTEGER,
                ColoresFondo INTEGER,
                Emparejar INTEGER,
                Imagenes INTEGER,
                Letras INTEGER,
                LineaDistractora INTEGER,
                Numerica INTEGER,
                Rejilla INTEGER,
                Sonidos INTEGER,
                Windings INTEGER,
                OrdenTachado,
                Usuarios INTEGER,
                CodigoUltimoUsuario INTEGER
            )";
            SqliteCommand comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD
        }

        private void crearTablaControlTachado()
        {
            string cadena = @"
            CREATE TABLE IF NOT EXISTS CONTROLTACHADO (
                Codigo INTEGER PRIMARY KEY,
                Fuente	TEXT,
                Key	TEXT,
                Titulos	TEXT,
                Indices	TEXT,
                Conteo	TEXT,
                Localizado	TEXT,
                Tiempos	TEXT,
                OrdenPulsado TEXT
            )";
            SqliteCommand comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD
        }

        /*
         * Creacion de las tablas para la base de datos
         * Se agrupan en un mismo método la creación de todas las tablas
         */

        public void crearTablas()
        {
            crearTablaIndices();
            crearTablaAbecedario();
            crearTablaColores();
            crearTablaColoresFondo();
            crearTablaEmparejar();
            crearTablaImagenes();
            crearTablaLetras();
            crearTablaLineaDistractora();
            crearTablaNumerica();
            crearTablaRejilla();
            crearTablaSonidos();
            crearTablaUsuarios();
            crearTablaWindings();
            crearTablaControlTachado();
            usuarioDefecto(1, "Pruebas", "Pruebas", "DERECHA", "MASCULINO");
        }

        //Borrar todas las tablas de la BD
        public void borrarTablas()
        {
            string cadena = @"DROP TABLE IF EXISTS ABECEDARIO";
            SqliteCommand comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DROP TABLE IF EXISTS INDICES";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DROP TABLE IF EXISTS COLORES";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DROP TABLE IF EXISTS COLORESFONDO";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DROP TABLE IF EXISTS EMPAREJAR";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DROP TABLE IF EXISTS IMAGENES";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DROP TABLE IF EXISTS LETRAS";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DROP TABLE IF EXISTS LINEADISTRACTORA";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DROP TABLE IF EXISTS REJILLA";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DROP TABLE IF EXISTS SONIDOS";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DROP TABLE IF EXISTS USUARIOS";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DROP TABLE IF EXISTS WINDINGS";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DROP TABLE IF EXISTS CONTROLTACHADO";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD
        }

        public void borrarDatosTablas()
        {
            string cadena = @"DELETE FROM ABECEDARIO";
            SqliteCommand comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DELETE FROM INDICES";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DELETE FROM COLORES";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DELETE FROM COLORESFONDO";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DELETE FROM EMPAREJAR";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DELETE FROM IMAGENES";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DELETE FROM LETRAS";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DELETE FROM LINEADISTRACTORA";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DELETE FROM REJILLA";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DELETE FROM SONIDOS";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DELETE FROM USUARIOS";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DELETE FROM WINDINGS";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DELETE FROM CONTROLTACHADO";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD
        }


        public void borrarDatosTablasMenosUsuarios()
        {
            string cadena = @"DELETE FROM ABECEDARIO";
            SqliteCommand comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DELETE FROM INDICES";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DELETE FROM COLORES";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DELETE FROM COLORESFONDO";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DELETE FROM EMPAREJAR";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DELETE FROM IMAGENES";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DELETE FROM LETRAS";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DELETE FROM LINEADISTRACTORA";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DELETE FROM REJILLA";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DELETE FROM SONIDOS";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DELETE FROM WINDINGS";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD

            cadena = @"DELETE FROM CONTROLTACHADO";
            comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();//Ejecuto la consulta sobre la BD
        }

        /*
         * Operaciones sobre la tabla INDICES
         */
        //Inicializa la tablas indices y el usuario por defecto de la B.D. que es Pruebas.
        public void inicialiarTablasIndicesUsuarios()
        {
            string cadena = @"
                    INSERT INTO INDICES (Abecedario, Colores, ColoresFondo, Emparejar, Imagenes, Letras, LineaDistractora, 
                    Numerica, Rejilla, Sonidos, Windings, OrdenTachado, Usuarios, CodigoUltimoUsuario) VALUES (0,0,0,0,0,0,0,0,0,0,0,0,1,1)";//Meto el usuario pruebas que va con el indice a 1.

            SqliteCommand comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery();
        }

        // Obtiene el índice que será clave en la tabla correspondiente. Se le pasa el nombre de la columna de la que queremos obtener el indice. 
        public string obtenerIndice (string nombreColumna)
        {
            string cadena = $" SELECT {nombreColumna} FROM INDICES";
            SqliteCommand comando = new SqliteCommand(cadena, conexion);
            SqliteDataReader lector = comando.ExecuteReader();
            object valor = "";
            while (lector.Read())
            {
                valor = lector[0];
            }
            return valor.ToString();
        }

        public void actualizarIndice (string nombreColumna, int valor)
        {
            string cadena = $" UPDATE INDICES SET {nombreColumna} = {valor}";
            SqliteCommand comando = new SqliteCommand(cadena, conexion);
            comando.ExecuteNonQuery ();
        }


        /*
         * Operaciones sobre la tabla USUARIOS
         */

        private void usuarioDefecto(int code, string name, string surname, string lat, string sex)
        {
            string consulta = @"INSERT INTO Usuarios (Codigo, Nombre, Apellidos, Lateralidad, Genero) 
                         VALUES (@code, @name, @surname, @lat, @sex)";


            using (SqliteCommand commando = new SqliteCommand(consulta, conexion))
            {
                // Parámetros obligatorios
                commando.Parameters.AddWithValue("@code", code);
                commando.Parameters.AddWithValue("@name", name);
                commando.Parameters.AddWithValue("@surname", surname);

                // Parámetros opcionales que podrían ser nulos
                commando.Parameters.AddWithValue("@lat", string.IsNullOrEmpty(lat) ? (object)DBNull.Value : lat);
                commando.Parameters.AddWithValue("@sex", string.IsNullOrEmpty(sex) ? (object)DBNull.Value : sex);

                // Abrir la conexión y ejecutar el comando
                commando.ExecuteNonQuery();
            }
        }

        public void insertarUsuario(int code, string name, string surname, string lat, string sex, string sports, string age, string country, string position)
        {
            string consulta = @"INSERT INTO Usuarios (Codigo, Nombre, Apellidos, Lateralidad, Genero, Deportes, Edad, Pais, Posicion) 
                         VALUES (@code, @name, @surname, @lat, @sex, @sports, @age, @country, @position)";


            using (SqliteCommand commando = new SqliteCommand(consulta, conexion))
            {
                // Parámetros obligatorios
                commando.Parameters.AddWithValue("@code", code);
                commando.Parameters.AddWithValue("@name", name);
                commando.Parameters.AddWithValue("@surname", surname);

                // Parámetros opcionales que podrían ser nulos
                commando.Parameters.AddWithValue("@lat", string.IsNullOrEmpty(lat) ? (object)DBNull.Value : lat);
                commando.Parameters.AddWithValue("@sex", string.IsNullOrEmpty(sex) ? (object)DBNull.Value : sex);
                commando.Parameters.AddWithValue("@sports", string.IsNullOrEmpty(sports) ? (object)DBNull.Value : sports);
                commando.Parameters.AddWithValue("@age", string.IsNullOrEmpty(age) ? (object)DBNull.Value : age);
                commando.Parameters.AddWithValue("@country", string.IsNullOrEmpty(country) ? (object)DBNull.Value : country);
                commando.Parameters.AddWithValue("@position", string.IsNullOrEmpty(position) ? (object)DBNull.Value : position);

                // Abrir la conexión y ejecutar el comando
                commando.ExecuteNonQuery();
            }
        }

        public void borrarUsuario (int code)
        {
            string consulta = "DELETE FROM USUARIOS WHERE Codigo = @code";
            SqliteCommand comando = new SqliteCommand(consulta, conexion);
            comando.Parameters.AddWithValue ("@code", code);
            comando.ExecuteNonQuery();

            //Borramos todas las rejillas realizadas por el usuario de la tabla REJILLA
            consulta = "DELETE FROM REJILLA WHERE FKUsuarios = @code";
            comando = new SqliteCommand (consulta, conexion);
            comando.Parameters.AddWithValue("@code", code);
            comando.ExecuteNonQuery();
        }

        /*
        public void actualizarUsuario(int code, string name, string surname, string lat, string sex, string sports, string age, string country, string position)
        {
            // Verificación de los parámetros obligatorios
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname))
            {
                throw new RejillaException("Los campos 'Nombre' y 'Apellidos' son obligatorios.");
            }

            // Construir dinámicamente la consulta SQL
            StringBuilder queryBuilder = new StringBuilder("UPDATE Usuarios SET ");

            // Agregar los campos que no son nulos
            if (!string.IsNullOrWhiteSpace(lat)) queryBuilder.Append("Lateralidad = @lat, ");
            if (!string.IsNullOrWhiteSpace(sex)) queryBuilder.Append("Genero = @sex, ");
            if (!string.IsNullOrWhiteSpace(sports)) queryBuilder.Append("Deportes = @sports, ");
            if (!string.IsNullOrWhiteSpace(age)) queryBuilder.Append("Edad = @age, ");
            if (!string.IsNullOrWhiteSpace(country)) queryBuilder.Append("Pais = @country, ");
            if (!string.IsNullOrWhiteSpace(position)) queryBuilder.Append("Posicion = @position, ");

            // Eliminar la coma final de la consulta
            queryBuilder.Length -= 2;

            // Añadir la condición WHERE
            queryBuilder.Append(" WHERE Code = @code");


            SqliteCommand command = new SqliteCommand(queryBuilder.ToString(), conexion);

            // Asignar el parámetro para el código
            command.Parameters.AddWithValue("@Codigo", code);

            // Asignar los valores opcionales
            //if (!string.IsNullOrEmpty(lat)) command.Parameters.AddWithValue("@lat", lat);
            //if (!string.IsNullOrEmpty(sex)) command.Parameters.AddWithValue("@sex", sex);
            if (!string.IsNullOrEmpty(sports)) command.Parameters.AddWithValue("@sports", sports);
            if (!string.IsNullOrEmpty(age)) command.Parameters.AddWithValue("@age", age);
            if (!string.IsNullOrEmpty(country)) command.Parameters.AddWithValue("@country", country);
            if (!string.IsNullOrEmpty(position)) command.Parameters.AddWithValue("@position", position);

            // Ejecutar el comando
            command.ExecuteNonQuery();
            //Comprobar si se actualiza algo

            int rowsAffected = command.ExecuteNonQuery();

            /*
            // Verificar si se actualizó algún registro
            if (rowsAffected > 0)
            {
                MessageBox.Show("Usuario actualizado correctamente.", "ACTUALIZANDO");
            }
            else
            {
                MessageBox.Show("No se encontró ningún usuario con el código especificado.", "ACTUALIZANDO"); 
            }


        }
            */

        public int consultarUsuario(string name, string surname, string lat, string sex, string sports, string age, string country, string position)
        {
            int resultado = -1;
            string consulta = @"SELECT Codigo FROM Usuarios WHERE Nombre = @name AND Apellidos=@surname AND Lateralidad=@lat AND Genero=@sex AND Deportes=@sports OR Deportes IS NULL
                AND Edad=@age OR Edad IS NULL AND Pais=@country OR Pais IS NULL AND Posicion=@position OR Posicion IS NULL";//Busqueda exacta.
            
            SqliteCommand commando = new SqliteCommand(consulta, conexion);
            // Parámetros obligatorios
            commando.Parameters.AddWithValue("@name", name);
            commando.Parameters.AddWithValue("@surname", surname);
            commando.Parameters.AddWithValue("@lat", lat);
            commando.Parameters.AddWithValue("@sex", sex);

            // Parámetros opcionales que podrían ser nulos
            //commando.Parameters.AddWithValue("@lat", string.IsNullOrEmpty(lat) ? (object)DBNull.Value : lat);
            //commando.Parameters.AddWithValue("@sex", string.IsNullOrEmpty(sex) ? (object)DBNull.Value : sex);
            commando.Parameters.AddWithValue("@sports", string.IsNullOrEmpty(sports) ? (object)DBNull.Value : sports);
            commando.Parameters.AddWithValue("@age", string.IsNullOrEmpty(age) ? (object)DBNull.Value : age);
            commando.Parameters.AddWithValue("@country", string.IsNullOrEmpty(country) ? (object)DBNull.Value : country);
            commando.Parameters.AddWithValue("@position", string.IsNullOrEmpty(position) ? (object)DBNull.Value : position);

            object resulta = commando.ExecuteScalar();

            if (resulta != null)//Si encuentra una fila que coindice, devolvemos el indice, en otro caso -1.
            {
                resultado = Convert.ToInt32(resulta);
            }
            return resultado;
        }


        public void actualizarUsuario(int code, string name, string surname, string lat, string sex, string sports, string age, string country, string position)
        {
            string consulta = @"UPDATE Usuarios SET Nombre = @name, Apellidos=@surname, Lateralidad=@lat, Genero=@sex, Deportes=@sports, Edad=@age, Pais=@country, Posicion=@position WHERE Codigo=@code";


            using (SqliteCommand commando = new SqliteCommand(consulta, conexion))
            {
                // Parámetros obligatorios
                commando.Parameters.AddWithValue("@code", code);
                commando.Parameters.AddWithValue("@name", name);
                commando.Parameters.AddWithValue("@surname", surname);
                commando.Parameters.AddWithValue("@lat", lat);
                commando.Parameters.AddWithValue("@sex", sex);

                // Parámetros opcionales que podrían ser nulos
                //commando.Parameters.AddWithValue("@lat", string.IsNullOrEmpty(lat) ? (object)DBNull.Value : lat);
                //commando.Parameters.AddWithValue("@sex", string.IsNullOrEmpty(sex) ? (object)DBNull.Value : sex);
                commando.Parameters.AddWithValue("@sports", string.IsNullOrEmpty(sports) ? (object)DBNull.Value : sports);
                commando.Parameters.AddWithValue("@age", string.IsNullOrEmpty(age) ? (object)DBNull.Value : age);
                commando.Parameters.AddWithValue("@country", string.IsNullOrEmpty(country) ? (object)DBNull.Value : country);
                commando.Parameters.AddWithValue("@position", string.IsNullOrEmpty(position) ? (object)DBNull.Value : position);

                // Abrir la conexión y ejecutar el comando
                commando.ExecuteNonQuery();
            }
        }


        public int consultarUsuarioFKTablaRejilla (int codigo)
        {
            int resultado = -1;
            string consulta = @"SELECT Codigo FROM REjilla WHERE FKUser = @codigo";

            SqliteCommand commando = new SqliteCommand(consulta, conexion);
            // Parámetros obligatorios
            commando.Parameters.AddWithValue("@codigo", codigo);
            
            object resulta = commando.ExecuteScalar();

            if (resulta != null)//Si encuentra una fila que coindice, devolvemos el indice, en otro caso -1.
            {
                resultado = Convert.ToInt32(resulta);
            }
            return resultado;
        }

        public SqliteDataReader obtenerDatosUsuarioPorPK(int codigo)
        {
            SqliteDataReader reader = null;
            string consulta = "SELECT * FROM Usuarios WHERE Codigo=@codigo";

            SqliteCommand comando = new SqliteCommand(consulta,conexion);
            comando.Parameters.AddWithValue("@codigo", codigo);

            reader = comando.ExecuteReader();
            return reader;
        }


        /*
         * Tabla Numérica
         */

        public int consultarNumerica (int inicial, int incremento, string orden)
        {
            int resultado = -1;
            string consulta = "SELECT Codigo FROM Numerica WHERE valorInicial=@inicial AND incremento=@incremento AND Orden=@orden";//Busqueda exacta.

            //consulta = "SELECT Codigo FROM Numerica WHERE valorInicial=@inicial AND incremento=@incremento AND Orden like @orden";//Busqueda exacta.

            SqliteCommand commando = new SqliteCommand (consulta, conexion);
            commando.Parameters.AddWithValue("@inicial", inicial);
            commando.Parameters.AddWithValue ("@incremento", incremento);
            commando.Parameters.AddWithValue("@orden", orden);

            object resulta = commando.ExecuteScalar();

            if (resulta != null)//Si encuentra una fila que coindice, devolvemos el indice, en otro caso -1.
            {
                resultado = Convert.ToInt32(resulta);
            }
            return resultado;
        }

        public void insertarNumerica (int codigo, int inicial, int incremento, string orden)
        {
            string consulta = "INSERT INTO Numerica values (@codigo, @inicial, @incremento, @orden)";
            SqliteCommand command = new SqliteCommand ( consulta, conexion);
            command.Parameters.AddWithValue("@codigo", codigo);
            command.Parameters.AddWithValue("@inicial", inicial);
            command.Parameters.AddWithValue("@incremento", incremento);
            command.Parameters.AddWithValue("@orden", orden );

            command.ExecuteNonQuery ();//Inserto la fila.
        }

        public void borrarNumerica (int codigo)
        {
            string consulta = "DELETE FROM Numerica WHERE codigo=@codigo";
            SqliteCommand command = new SqliteCommand(consulta, conexion);
            command.Parameters.AddWithValue("@codigo", codigo);
            command.ExecuteNonQuery();
        }

        public SqliteDataReader obtenerNumericaFKTablaRejilla(int codigo)
        {
            SqliteDataReader reader = null;
            string consulta = "SELECT * FROM Numerica WHERE Codigo=@codigo";

            SqliteCommand comando = new SqliteCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@codigo", codigo);

            reader = comando.ExecuteReader();
            return reader;
        }


        /*
         * Tabla Abecedario
         */

        public int consultarAbecedario (string valorInicial, string tipo, string orden)
        {
            int resultado = -1;
            string consulta = "SELECT Codigo FROM Abecedario WHERE valorInicial=@inicial AND tipo=@tipo AND Orden=@orden";//Busqueda exacta.

            //consulta = "SELECT Codigo FROM Abecedario WHERE valorInicial=@inicial AND tipo=@tipo AND Orden like @orden";//Busqueda exacta.

            SqliteCommand commando = new SqliteCommand(consulta, conexion);
            commando.Parameters.AddWithValue("@inicial", valorInicial);
            commando.Parameters.AddWithValue("@tipo", tipo);
            commando.Parameters.AddWithValue("@orden", orden);

            object resulta = commando.ExecuteScalar();

            if (resulta != null)//Si encuentra una fila que coindice, devolvemos el indice, en otro caso -1.
            {
                resultado = Convert.ToInt32(resulta);
            }
            return resultado;
        }

        public void insertarAbecedario(int codigo, string inicial, string tipo, string orden)
        {
            string consulta = "INSERT INTO Abecedario values (@codigo, @inicial, @tipo, @orden)";
            SqliteCommand command = new SqliteCommand(consulta, conexion);
            command.Parameters.AddWithValue("@codigo", codigo);
            command.Parameters.AddWithValue("@inicial", inicial);
            command.Parameters.AddWithValue("@tipo", tipo);
            command.Parameters.AddWithValue("@orden", orden);

            command.ExecuteNonQuery();//Inserto la fila.
        }

        public void borrarAbecedario(int codigo)
        {
            string consulta = "DELETE FROM Abecedario WHERE codigo=@codigo";
            SqliteCommand command = new SqliteCommand(consulta, conexion);
            command.Parameters.AddWithValue("@codigo", codigo);
            command.ExecuteNonQuery();
        }

        public SqliteDataReader obtenerAbecedarioFKTablaRejilla(int codigo)
        {
            SqliteDataReader reader = null;
            string consulta = "SELECT * FROM Abecedario WHERE Codigo=@codigo";

            SqliteCommand comando = new SqliteCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@codigo", codigo);

            reader = comando.ExecuteReader();
            return reader;
        }

        /*
         * Tabla Colores
         */

        public int consultarColores(string cantidadBotones, string colorFondo, string colorTachar)
        {
            int resultado = -1;
            string consulta = "SELECT Codigo FROM Colores WHERE CantidadBotones=@cantidadBotones AND coloresFondo=@colorFondo AND coloresTachar=@colorTachar";//Busqueda exacta.

            SqliteCommand commando = new SqliteCommand(consulta, conexion);
            commando.Parameters.AddWithValue("@cantidadBotones", cantidadBotones);
            commando.Parameters.AddWithValue("@colorFondo", colorFondo );
            commando.Parameters.AddWithValue("@colorTachar", colorTachar);

            object resulta = commando.ExecuteScalar();

            if (resulta != null)//Si encuentra una fila que coindice, devolvemos el indice, en otro caso -1.
            {
                resultado = Convert.ToInt32(resulta);
            }
            return resultado;
        }

        public void insertarColores(int codigo, string cantBotones, string colorsFondo, string colorsTachar)
        {
            string consulta = "INSERT INTO Colores values (@codigo, @cantBotones, @colorsFondo, @colorsTachar)";
            SqliteCommand command = new SqliteCommand(consulta, conexion);
            command.Parameters.AddWithValue("@codigo", codigo);
            command.Parameters.AddWithValue("@cantBotones", cantBotones);
            command.Parameters.AddWithValue("@colorsFondo", colorsFondo);
            command.Parameters.AddWithValue("@colorsTachar", colorsTachar);

            command.ExecuteNonQuery();//Inserto la fila.
        }

        public void borrarColores(int codigo)
        {
            string consulta = "DELETE FROM Colores WHERE codigo=@codigo";
            SqliteCommand command = new SqliteCommand(consulta, conexion);
            command.Parameters.AddWithValue("@codigo", codigo);
            command.ExecuteNonQuery();
        }

        public SqliteDataReader obtenerColoresFKTablaRejilla(int codigo)
        {
            SqliteDataReader reader = null;
            string consulta = "SELECT * FROM Colores WHERE Codigo=@codigo";

            SqliteCommand comando = new SqliteCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@codigo", codigo);

            reader = comando.ExecuteReader();
            return reader;
        }

        /*
         * Tabla Emparejar
         */

        public int consultarEmparejar(string time, string order, string position)
        {
            int resultado = -1;
            string consulta = "SELECT Codigo FROM Emparejar WHERE Tiempo=@time AND Orden=@order AND Posicion=@position";//Busqueda exacta.

            SqliteCommand commando = new SqliteCommand(consulta, conexion);
            commando.Parameters.AddWithValue("@time", time);
            commando.Parameters.AddWithValue("@order", order);
            commando.Parameters.AddWithValue("@position", position);

            object resulta = commando.ExecuteScalar();

            if (resulta != null)//Si encuentra una fila que coindice, devolvemos el indice, en otro caso -1.
            {
                resultado = Convert.ToInt32(resulta);
            }
            return resultado;
        }

        public void insertarEmparejar(int codigo, string time, string order, string position)
        {
            string consulta = "INSERT INTO Colores values (@codigo, @time, @order, @position)";
            SqliteCommand command = new SqliteCommand(consulta, conexion);
            command.Parameters.AddWithValue("@codigo", codigo);
            command.Parameters.AddWithValue("@time", time);
            command.Parameters.AddWithValue("@order", order);
            command.Parameters.AddWithValue("@position", position);

            command.ExecuteNonQuery();//Inserto la fila.
        }

        public void borrarEmparejar(int codigo)
        {
            string consulta = "DELETE FROM Emparejar WHERE codigo=@codigo";
            SqliteCommand command = new SqliteCommand(consulta, conexion);
            command.Parameters.AddWithValue("@codigo", codigo);
            command.ExecuteNonQuery();
        }

        public SqliteDataReader obtenerEmparejarFKTablaRejilla(int codigo)
        {
            SqliteDataReader reader = null;
            string consulta = "SELECT * FROM Emparejar WHERE Codigo=@codigo";

            SqliteCommand comando = new SqliteCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@codigo", codigo);

            reader = comando.ExecuteReader();
            return reader;
        }

        /*
         * Tabla Imagenes
         */

        public int consultarImagenes(string cantBotones, string imagenRejilla, string imagenTachar)
        {
            int resultado = -1;
            string consulta = "SELECT Codigo FROM Imagenes WHERE CantidadBotones=@cantBotones AND ImagenesRejilla=@imagenRejilla AND ImagenesTachar=@imagenTachar";//Busqueda exacta.

            SqliteCommand commando = new SqliteCommand(consulta, conexion);
            commando.Parameters.AddWithValue("@cantBotones", cantBotones);
            commando.Parameters.AddWithValue("@imagenRejilla", imagenRejilla);
            commando.Parameters.AddWithValue("@imagenTachar", imagenTachar);

            object resulta = commando.ExecuteScalar();

            if (resulta != null)//Si encuentra una fila que coindice, devolvemos el indice, en otro caso -1.
            {
                resultado = Convert.ToInt32(resulta);
            }
            return resultado;
        }

        public int consultarImagenes(string cantBotones, byte[] imagenRejilla, byte[] imagenTachar)
        {
            int resultado = -1;
            string consulta = "SELECT Codigo FROM Imagenes WHERE CantidadBotones=@cantBotones AND ImagenesRejilla=@imagenRejilla AND ImagenesTachar=@imagenTachar";//Busqueda exacta.

            SqliteCommand commando = new SqliteCommand(consulta, conexion);
            commando.Parameters.AddWithValue("@cantBotones", cantBotones);
            commando.Parameters.AddWithValue("@imagenRejilla", imagenRejilla);
            commando.Parameters.AddWithValue("@imagenTachar", imagenTachar);

            object resulta = commando.ExecuteScalar();

            if (resulta != null)//Si encuentra una fila que coindice, devolvemos el indice, en otro caso -1.
            {
                resultado = Convert.ToInt32(resulta);
            }
            return resultado;
        }

        public void insertarImagenes(int codigo, string cantBotones, string imagenesRejilla, string imagenesTachar)
        {
            string consulta = "INSERT INTO Imagenes values (@codigo, @cantBotones, @imagenesRejilla, @imagenesTachar)";
            SqliteCommand command = new SqliteCommand(consulta, conexion);
            command.Parameters.AddWithValue("@codigo", codigo);
            command.Parameters.AddWithValue("@cantBotones", cantBotones);
            command.Parameters.AddWithValue("@imagenesRejilla", imagenesRejilla);
            command.Parameters.AddWithValue("@imagenesTachar", imagenesTachar);

            command.ExecuteNonQuery();//Inserto la fila.
        }

        public void insertarImagenes(int codigo, string cantBotones, byte[] imagenesRejilla, byte[] imagenesTachar)
        {
            string consulta = "INSERT INTO Imagenes values (@codigo, @cantBotones, @imagenesRejilla, @imagenesTachar)";
            SqliteCommand command = new SqliteCommand(consulta, conexion);
            command.Parameters.AddWithValue("@codigo", codigo);
            command.Parameters.AddWithValue("@cantBotones", cantBotones);
            command.Parameters.AddWithValue("@imagenesRejilla", imagenesRejilla);
            command.Parameters.AddWithValue("@imagenesTachar", imagenesTachar);

            command.ExecuteNonQuery();//Inserto la fila.
        }

        public void borrarImagenes(int codigo)
        {
            string consulta = "DELETE FROM Imagenes WHERE codigo=@codigo";
            SqliteCommand command = new SqliteCommand(consulta, conexion);
            command.Parameters.AddWithValue("@codigo", codigo);
            command.ExecuteNonQuery();
        }



        public SqliteDataReader obtenerImagenesFKTablaRejilla(int codigo)
        {
            SqliteDataReader reader = null;
            string consulta = "SELECT * FROM Imagenes WHERE Codigo=@codigo";

            SqliteCommand comando = new SqliteCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@codigo", codigo);

            reader = comando.ExecuteReader();
            return reader;
        }

        /*
         * Tabla Letras
         */

        public int consultarLetras(string cantBotones, string fuente, string letrasRejilla, string letrasTachar)
        {
            int resultado = -1;
            string consulta = "SELECT Codigo FROM Letras WHERE CantidadBotones=@cantBotones AND Fuente=@fuente AND letrasRejilla=@letrasRejilla AND letrasTachar=@letrasTachar";//Busqueda exacta.

            SqliteCommand commando = new SqliteCommand(consulta, conexion);
            commando.Parameters.AddWithValue("@cantBotones", cantBotones);
            commando.Parameters.AddWithValue("@fuente", fuente);
            commando.Parameters.AddWithValue("@letrasRejilla", letrasRejilla);
            commando.Parameters.AddWithValue("@letrasTachar", letrasTachar);

            object resulta = commando.ExecuteScalar();

            if (resulta != null)//Si encuentra una fila que coindice, devolvemos el indice, en otro caso -1.
            {
                resultado = Convert.ToInt32(resulta);
            }
            return resultado;
        }

        public void insertarLetras(int codigo, string cantBotones, string font, string letrasRejilla, string letrasTachar)
        {
            string consulta = "INSERT INTO Letras values (@codigo, @cantBotones, @font, @letrasRejilla, @letrasTachar)";
            SqliteCommand command = new SqliteCommand(consulta, conexion);
            command.Parameters.AddWithValue("@codigo", codigo);
            command.Parameters.AddWithValue("@cantBotones", cantBotones);
            command.Parameters.AddWithValue("@font", font);
            command.Parameters.AddWithValue("@letrasRejilla", letrasRejilla);
            command.Parameters.AddWithValue("@letrasTachar", letrasTachar);

            command.ExecuteNonQuery();//Inserto la fila.
        }

        public void borrarLetras(int codigo)
        {
            string consulta = "DELETE FROM Letras WHERE codigo=@codigo";
            SqliteCommand command = new SqliteCommand(consulta, conexion);
            command.Parameters.AddWithValue("@codigo", codigo);
            command.ExecuteNonQuery();
        }

        public SqliteDataReader obtenerLetrasFKTablaRejilla(int codigo)
        {
            SqliteDataReader reader = null;
            string consulta = "SELECT * FROM Letras WHERE Codigo=@codigo";

            SqliteCommand comando = new SqliteCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@codigo", codigo);

            reader = comando.ExecuteReader();
            return reader;
        }
        
        /*
         * Tabla LineaDistractora
         */

        public int consultarLinea(string color, string grosor, string velocidad, string tipoMovimiento)
        {
            int resultado = -1;
            string consulta = "SELECT Codigo FROM Lineadistractora WHERE color=@color AND grosor=@grosor AND velocidad=@velocidad AND tipoMovimiento=@tipoMovimiento";//Busqueda exacta.

            SqliteCommand commando = new SqliteCommand(consulta, conexion);
            commando.Parameters.AddWithValue("@color", color);
            commando.Parameters.AddWithValue("@grosor", grosor);
            commando.Parameters.AddWithValue("@velocidad", velocidad);
            commando.Parameters.AddWithValue("@tipoMovimiento", tipoMovimiento);

            object resulta = commando.ExecuteScalar();

            if (resulta != null)//Si encuentra una fila que coindice, devolvemos el indice, en otro caso -1.
            {
                resultado = Convert.ToInt32(resulta);
            }
            return resultado;
        }

        public void insertarLinea(int codigo, string color, string grosor, string velocidad, string tipoMovimiento)
        {
            string consulta = "INSERT INTO Lineadistractora values (@codigo, @color, @grosor, @velocidad, @tipoMovimiento)";
            SqliteCommand command = new SqliteCommand(consulta, conexion);
            command.Parameters.AddWithValue("@codigo", codigo);
            command.Parameters.AddWithValue("@color", color);
            command.Parameters.AddWithValue("@grosor", grosor);
            command.Parameters.AddWithValue("@velocidad", velocidad);
            command.Parameters.AddWithValue("@tipoMovimiento", tipoMovimiento);

            command.ExecuteNonQuery();//Inserto la fila.
        }

        public void borrarLinea(int codigo)
        {
            string consulta = "DELETE FROM LineaDistractora WHERE codigo=@codigo";
            SqliteCommand command = new SqliteCommand(consulta, conexion);
            command.Parameters.AddWithValue("@codigo", codigo);
            command.ExecuteNonQuery();
        }

        public SqliteDataReader obtenerLineaDistractoraFKTablaRejilla(int codigo)
        {
            SqliteDataReader reader = null;
            string consulta = "SELECT * FROM LineaDistractora WHERE Codigo=@codigo";

            SqliteCommand comando = new SqliteCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@codigo", codigo);

            reader = comando.ExecuteReader();
            return reader;
        }


        /*
         * Tabla Sonidos
         */

        public int consultarSonidos(string metronomo, string canciones)
        {
            int resultado = -1;
            string consulta = "SELECT Codigo FROM Sonidos WHERE metronomo=@metronomo AND canciones=@canciones ";//Busqueda exacta.

            SqliteCommand commando = new SqliteCommand(consulta, conexion);
            commando.Parameters.AddWithValue("@metronomo", string.IsNullOrEmpty(metronomo) ? (object)DBNull.Value : metronomo);
            commando.Parameters.AddWithValue("@canciones", string.IsNullOrEmpty(canciones) ? (object)DBNull.Value : canciones);          
         

            object resulta = commando.ExecuteScalar();

            if (resulta != null)//Si encuentra una fila que coindice, devolvemos el indice, en otro caso -1.
            {
                resultado = Convert.ToInt32(resulta);
            }
            return resultado;
        }

        public void insertarSonidos(int codigo, string metronomo, string canciones)
        {
            string consulta = @"INSERT INTO Sonidos (Codigo, metronomo, canciones) values (@codigo, @metronomo, @canciones)";


            using (SqliteCommand commando = new SqliteCommand(consulta, conexion))
            {
                // Parámetros obligatorios
                commando.Parameters.AddWithValue("@codigo", codigo);

                // Parámetros opcionales que podrían ser nulos
                commando.Parameters.AddWithValue("@metronomo", string.IsNullOrEmpty(metronomo) ? (object)DBNull.Value : metronomo);
                commando.Parameters.AddWithValue("@canciones", string.IsNullOrEmpty(canciones) ? (object)DBNull.Value : canciones);

                // Abrir la conexión y ejecutar el comando

                commando.ExecuteNonQuery();
            }
        }

        public void borrarSonidos(int codigo)
        {
            string consulta = "DELETE FROM Sonidos WHERE codigo=@codigo";
            SqliteCommand command = new SqliteCommand(consulta, conexion);
            command.Parameters.AddWithValue("@codigo", codigo);
            command.ExecuteNonQuery();
        }

        public SqliteDataReader obtenerSonidosFKTablaRejilla(int codigo)
        {
            SqliteDataReader reader = null;
            string consulta = "SELECT * FROM Sonidos WHERE Codigo=@codigo";

            SqliteCommand comando = new SqliteCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@codigo", codigo);

            reader = comando.ExecuteReader();
            return reader;
        }

        /*
         * Tabla Windings
         */

        public int consultarWindings(string letrasRejilla, string letrasTachar, string porcentaje)
        {
            int resultado = -1;
            string consulta = "SELECT Codigo FROM Windings WHERE letrasRejilla=@letrasRejilla AND letrasTachar=@letrasTachar AND Porcentaje=@porcentaje ";//Busqueda exacta.

            SqliteCommand commando = new SqliteCommand(consulta, conexion);
            commando.Parameters.AddWithValue("@letrasRejilla", letrasRejilla);
            commando.Parameters.AddWithValue("@letrasTachar", letrasTachar);
            commando.Parameters.AddWithValue("@porcentaje",porcentaje);

            object resulta = commando.ExecuteScalar();

            if (resulta != null)//Si encuentra una fila que coindice, devolvemos el indice, en otro caso -1.
            {
                resultado = Convert.ToInt32(resulta);
            }
            return resultado;
        }

        public void insertarWindings(int codigo, string letrasRejilla, string letrasTachar, string porcentaje)
        {
            string consulta = @"INSERT INTO Windings values (@codigo, @letrasRejilla, @letrasTachar, @porcentaje)";


            using (SqliteCommand commando = new SqliteCommand(consulta, conexion))
            {
                // Parámetros obligatorios
                commando.Parameters.AddWithValue("@codigo", codigo);
                commando.Parameters.AddWithValue("@letrasRejilla", letrasRejilla);
                commando.Parameters.AddWithValue("@letrasTachar", letrasTachar);
                commando.Parameters.AddWithValue("@porcentaje", porcentaje);

                // Abrir la conexión y ejecutar el comando

                commando.ExecuteNonQuery();
            }
        }

        public void borrarWindings(int codigo)
        {
            string consulta = "DELETE FROM Windings WHERE codigo=@codigo";
            SqliteCommand command = new SqliteCommand(consulta, conexion);
            command.Parameters.AddWithValue("@codigo", codigo);
            command.ExecuteNonQuery();
        }

        public SqliteDataReader obtenerWindingsFKTablaRejilla(int codigo)
        {
            SqliteDataReader reader = null;
            string consulta = "SELECT * FROM Windings WHERE Codigo=@codigo";

            SqliteCommand comando = new SqliteCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@codigo", codigo);

            reader = comando.ExecuteReader();
            return reader;
        }

        /*
         * Tabla Colores de Fondo
         */

        public int consultarColoresFondo(string colorPlantilla, string colorTachar)
        {
            int resultado = -1;
            string consulta = "SELECT Codigo FROM ColoresFondo WHERE coloresPlantilla=@colorPlantilla AND coloreselegidos=@colorTachar";//Busqueda exacta.

            SqliteCommand commando = new SqliteCommand(consulta, conexion);
            commando.Parameters.AddWithValue("@colorPlantilla", colorPlantilla);
            commando.Parameters.AddWithValue("@colorTachar", colorTachar);


            object resulta = commando.ExecuteScalar();

            if (resulta != null)//Si encuentra una fila que coindice, devolvemos el indice, en otro caso -1.
            {
                resultado = Convert.ToInt32(resulta);
            }
            return resultado;
        }

        public void insertarColoresFondo(int codigo,  string colorsFondo, string plantilla)
        {
            string consulta = "INSERT INTO ColoresFondo values (@codigo, @colorsFondo, @plantilla)";
            SqliteCommand command = new SqliteCommand(consulta, conexion);
            command.Parameters.AddWithValue("@colorsFondo", colorsFondo);
            command.Parameters.AddWithValue("@plantilla", plantilla);
            command.Parameters.AddWithValue("@codigo", codigo);

            command.ExecuteNonQuery();//Inserto la fila.
        }


        public void borrarColoresFondo(int codigo)
        {
            string consulta = "DELETE FROM ColoresFondo WHERE codigo=@codigo";
            SqliteCommand command = new SqliteCommand(consulta, conexion);
            command.Parameters.AddWithValue("@codigo", codigo);
            command.ExecuteNonQuery();
        }

        public SqliteDataReader obtenerColoresFondoFKTablaRejilla(int codigo)
        {
            SqliteDataReader reader = null;
            string consulta = "SELECT * FROM ColoresFondo WHERE Codigo=@codigo";

            SqliteCommand comando = new SqliteCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@codigo", codigo);

            reader = comando.ExecuteReader();
            return reader;
        }


        /*
         * Tabla Control Tachado
         */

        public void insertarControlTachado(int codigo, string fuente, string listaKey, string listatitulos, string listaIndices, string listaConteoPulsaciones,
            string listaBotonLocalizado, string listaTiempos, string ordenPulsado )
        {
            string consulta = "INSERT INTO ControlTachado values (@codigo, @fuente, @listaKey, @listatitulos, @listaIndices, @listaConteoPulsaciones, " +
                "@listaBotonLocalizado, @listaTiempos, @ordenPulsado)";
            SqliteCommand command = new SqliteCommand(consulta, conexion);
            command.Parameters.AddWithValue("@codigo", codigo);
            command.Parameters.AddWithValue("@fuente", fuente);
            command.Parameters.AddWithValue("@listaKey", listaKey);
            command.Parameters.AddWithValue("@listatitulos", listatitulos);
            command.Parameters.AddWithValue("@listaIndices", listaIndices);
            command.Parameters.AddWithValue("@listaConteoPulsaciones", listaConteoPulsaciones);
            command.Parameters.AddWithValue("@listaBotonLocalizado", listaBotonLocalizado);
            command.Parameters.AddWithValue("@listaTiempos", listaTiempos);
            command.Parameters.AddWithValue("@ordenPulsado", ordenPulsado);

            command.ExecuteNonQuery();//Inserto la fila.string consulta = "";
        }

        /*
        public int consultarControlTachado(string fuente, string datos, string pulsacion, string orden)
        {
            int resultado = -1;

            string consulta = "SELECT Codigo FROM ControlTachado WHERE Fuente=@fuente AND DatosControlTachado=@datos AND OrdenPulsacion=@pulsacion AND Orden=@orden";//Busqueda exacta.

            SqliteCommand commando = new SqliteCommand(consulta, conexion);
            commando.Parameters.AddWithValue("@fuente", fuente);
            commando.Parameters.AddWithValue("@datos", datos);
            commando.Parameters.AddWithValue("@pulsacion", pulsacion);
            commando.Parameters.AddWithValue("@orden", orden);

            object resulta = commando.ExecuteScalar();

            if (resulta != null)//Si encuentra una fila que coindice, devolvemos el indice, en otro caso -1.
            {
                resultado = Convert.ToInt32(resulta);
            }
            return resultado;
        }
        */
        public SqliteDataReader obtenerControlTachadoFKRejilla(int codigo)
        {
            SqliteDataReader reader = null;
            string consulta = "SELECT * FROM ControlTachado WHERE Codigo=@codigo";

            SqliteCommand comando = new SqliteCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@codigo", codigo);

            reader = comando.ExecuteReader();
            return reader;

        }


        /*
         * Métodos, para buscar los usuarios en el datagridView.
         */
        public DataTable devolverTodoslosUsuarios()
        {
            DataTable dt = new DataTable();
            string consulta = "SELECT * FROM Usuarios";


            SqliteCommand cmd = new SqliteCommand(consulta, conexion);
            SqliteDataReader reader = cmd.ExecuteReader();

            dt.Load(reader);
            return dt;
        }

        public DataTable devolverTodoslosUsuariosNombre(string name)
        {
            DataTable dt = new DataTable();
            string consulta = "SELECT * FROM Usuarios WHERE Nombre LIKE @name";


            SqliteCommand cmd = new SqliteCommand(consulta, conexion);
            cmd.Parameters.AddWithValue("@name", name + "%");

            SqliteDataReader reader = cmd.ExecuteReader();

            dt.Load(reader);
            return dt;
        }

        public DataTable devolverTodoslosUsuariosApellidos(string name)
        {
            DataTable dt = new DataTable();
            string consulta = "SELECT * FROM Usuarios WHERE Apellidos LIKE @name";


            SqliteCommand cmd = new SqliteCommand(consulta, conexion);
            cmd.Parameters.AddWithValue("@name", name + "%");

            SqliteDataReader reader = cmd.ExecuteReader();

            dt.Load(reader);
            return dt;
        }

        public DataTable devolverTodoslosUsuariosLAteralidad(string name)
        {
            DataTable dt = new DataTable();
            string consulta = "SELECT * FROM Usuarios WHERE Lateralidad LIKE @name";


            SqliteCommand cmd = new SqliteCommand(consulta, conexion);
            cmd.Parameters.AddWithValue("@name", name + "%");

            SqliteDataReader reader = cmd.ExecuteReader();

            dt.Load(reader);
            return dt;
        }

        public DataTable devolverTodoslosUsuariosGenero(string name)
        {
            DataTable dt = new DataTable();
            string consulta = "SELECT * FROM Usuarios WHERE Genero LIKE @name";


            SqliteCommand cmd = new SqliteCommand(consulta, conexion);
            cmd.Parameters.AddWithValue("@name", name + "%");

            SqliteDataReader reader = cmd.ExecuteReader();

            dt.Load(reader);
            return dt;
        }

        public DataTable devolverTodoslosUsuariosDeportes(string name)
        {
            DataTable dt = new DataTable();
            string consulta = "SELECT * FROM Usuarios WHERE Deportes LIKE @name";


            SqliteCommand cmd = new SqliteCommand(consulta, conexion);
            cmd.Parameters.AddWithValue("@name", name + "%");

            SqliteDataReader reader = cmd.ExecuteReader();

            dt.Load(reader);
            return dt;
        }

        public DataTable devolverTodoslosUsuariosEdad(string name)
        {
            DataTable dt = new DataTable();
            string consulta = "SELECT * FROM Usuarios WHERE Edad LIKE @name";


            SqliteCommand cmd = new SqliteCommand(consulta, conexion);
            cmd.Parameters.AddWithValue("@name", name + "%");

            SqliteDataReader reader = cmd.ExecuteReader();

            dt.Load(reader);
            return dt;
        }

        public DataTable devolverTodoslosUsuariosPais(string name)
        {
            DataTable dt = new DataTable();
            string consulta = "SELECT * FROM Usuarios WHERE Pais LIKE @name";


            SqliteCommand cmd = new SqliteCommand(consulta, conexion);
            cmd.Parameters.AddWithValue("@name", name + "%");

            SqliteDataReader reader = cmd.ExecuteReader();

            dt.Load(reader);
            return dt;
        }

        public DataTable devolverTodoslosUsuariosPosicion(string name)
        {
            DataTable dt = new DataTable();
            string consulta = "SELECT * FROM Usuarios WHERE Posicion LIKE @name";


            SqliteCommand cmd = new SqliteCommand(consulta, conexion);
            cmd.Parameters.AddWithValue("@name", name + "%");

            SqliteDataReader reader = cmd.ExecuteReader();

            dt.Load(reader);
            return dt;
        }


        /*
         * Operaciones Sobre la Tabla Rejilla
         */
        public DataTable consultarRejillaPorUsuario(int codigo)
        {
            DataTable dt = new DataTable();
            string consulta = "SELECT * FROM Rejilla WHERE FKUsuarios=@codigo";


            SqliteCommand commando = new SqliteCommand(consulta, conexion);
            commando.Parameters.AddWithValue("@codigo", codigo);
            SqliteDataReader reader = commando.ExecuteReader();

            dt.Load(reader);
            
            return dt;
        }


        public void actualizarObservacionesRejilla(int codigo, string observaciones)
        {
            string consulta = @"UPDATE REJILLA SET Observaciones=@observaciones WHERE codigo=@codigo";
            SqliteCommand comando = new SqliteCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@codigo", codigo);
            comando.Parameters.AddWithValue("@observaciones", observaciones);

            comando.ExecuteNonQuery();
        }

        //Insertar una Fila en la tabla Rejilla
            public void insertarRejilla(int codigo, string filas, string columnas, int errores, int aciertos, string fecha, int botonesTotal, string tamañoBoton, string tachar,
            string intercambiar, string cantidadBotones, string observaciones, string tipoRejilla, int FKTipoRejilla, int FKUser, int FKEmparejar, int FKLinea, int FKSonidos,
            string timeTranscurrido, string timeValorMedia, string timeValorVarianza, string timeCorregido, string timeCorreccionEfectuada, string timeTareaPreliminar,
            string timeValorMediaCorrecion, string timeValorVarianzacorrecion, string timeMaximo, string timeMinimo, string timeLimite, string timeAleatorio, 
            string times, string timesLatencia, int colorFondo, int controlTachado, string fechaFormateada, string JsonTikGrafica, string JsonTikTiempos)
        {
            string consulta = @"INSERT INTO Rejilla (Codigo, Filas, Columnas, Errores, Aciertos, Fecha, BotonesTotal, TamañoBoton, Tachar,
                            Intercambiar, CantidadBotones, Observaciones, TipoRejilla, FKTipoRejilla, FKUsuarios, FKEmparejar, FKLinea, FKSonidos,
                            TiempoTranscurrido, ValorMedia, ValorVarianza, TiempoCorregido, TiempoCorreccionEfectuada, TiempoTareaPreliminar,
                            TiempoValorMediaCorreccion, TiempoValorVarianzaCorreccion, TiempoMaximo, TiempoMinimo, TLimite, TAleatorio, Tiempos,
                            TiemposLatencia, FKColorFondo, FKOrdenTachado, FechaFormateada, TiksGrafica, TiksTiempo) 
                         VALUES (@codigo, @filas, @columnas, @errores, @aciertos, @fecha, @botonesTotal, @tamañoBoton, @tachar, @intercambiar, 
                        @cantidadBotones, @observaciones, @tipoRejilla, @FKTipoRejilla, @FKUser, @FKEmparejar, @FKLinea, @FKSonidos,
                        @timeTranscurrido, @timeValorMedia, @timeValorVarianza, @timeCorregido, @timeCorreccionEfectuada, @timeTareaPreliminar,
                        @timeValorMediaCorrecion, @timeValorVarianzacorrecion, @timeMaximo, @timeMinimo, @timeLimite, @timeAleatorio, @times, @timesLatencia,
                        @colorFondo, @ordenTachado, @fechaFormateada, @JsonTikGrafica, @JsonTikTiempos)";


            using (SqliteCommand commando = new SqliteCommand(consulta, conexion))
            {
                // Parámetros obligatorios
                commando.Parameters.AddWithValue("@codigo", codigo);
                commando.Parameters.AddWithValue("@filas", filas);
                commando.Parameters.AddWithValue("@columnas", columnas);
                commando.Parameters.AddWithValue("@errores", errores);
                commando.Parameters.AddWithValue("@aciertos", aciertos);
                commando.Parameters.AddWithValue("@fecha", fecha);
                commando.Parameters.AddWithValue("@fechaFormateada", fechaFormateada);
                commando.Parameters.AddWithValue("@botonesTotal", botonesTotal);
                commando.Parameters.AddWithValue("@tamañoBoton", tamañoBoton);
                commando.Parameters.AddWithValue("@tipoRejilla", tipoRejilla);
                commando.Parameters.AddWithValue("@FKTipoRejilla", FKTipoRejilla);
                commando.Parameters.AddWithValue("@timeTranscurrido", timeTranscurrido);
                commando.Parameters.AddWithValue("@timeMaximo", timeMaximo);
                commando.Parameters.AddWithValue("@timeMinimo", timeMinimo);
                commando.Parameters.AddWithValue("@timeValorMedia", timeValorMedia);
                commando.Parameters.AddWithValue("@timeValorVarianza", timeValorVarianza);
                commando.Parameters.AddWithValue("@FKUser", FKUser);

                // Parámetros opcionales que podrían ser nulos
                commando.Parameters.AddWithValue("@tachar", string.IsNullOrEmpty(tachar) ? (object)DBNull.Value : tachar);
                commando.Parameters.AddWithValue("@intercambiar", string.IsNullOrEmpty(intercambiar) ? (object)DBNull.Value : intercambiar);
                commando.Parameters.AddWithValue("@cantidadBotones", string.IsNullOrEmpty(cantidadBotones) ? (object)DBNull.Value : cantidadBotones);
                commando.Parameters.AddWithValue("@observaciones", string.IsNullOrEmpty(observaciones) ? (object)DBNull.Value : observaciones);
                commando.Parameters.AddWithValue("@FKEmparejar", string.IsNullOrEmpty(FKEmparejar.ToString()) ? (object)DBNull.Value : FKEmparejar);
                commando.Parameters.AddWithValue("@FKLinea", string.IsNullOrEmpty(FKLinea.ToString()) ? (object)DBNull.Value : FKLinea);
                commando.Parameters.AddWithValue("@FKSonidos", string.IsNullOrEmpty(FKSonidos.ToString()) ? (object)DBNull.Value : FKSonidos);

                commando.Parameters.AddWithValue("@timeCorregido", string.IsNullOrEmpty(timeCorregido.ToString()) ? (object)DBNull.Value : timeCorregido);
                commando.Parameters.AddWithValue("@timeCorreccionEfectuada", string.IsNullOrEmpty(timeCorreccionEfectuada.ToString()) ? (object)DBNull.Value : timeCorreccionEfectuada);
                commando.Parameters.AddWithValue("@timeTareaPreliminar", string.IsNullOrEmpty(timeTareaPreliminar.ToString()) ? (object)DBNull.Value : timeTareaPreliminar);
                commando.Parameters.AddWithValue("@timeValorMediaCorrecion", string.IsNullOrEmpty(timeValorMediaCorrecion.ToString()) ? (object)DBNull.Value : timeValorMediaCorrecion);
                commando.Parameters.AddWithValue("@timeValorVarianzacorrecion", string.IsNullOrEmpty(timeValorVarianzacorrecion.ToString()) ? (object)DBNull.Value : timeValorVarianzacorrecion);
                commando.Parameters.AddWithValue("@timeLimite", string.IsNullOrEmpty(timeLimite.ToString()) ? (object)DBNull.Value : timeLimite);
                commando.Parameters.AddWithValue("@timeAleatorio", string.IsNullOrEmpty(timeAleatorio.ToString()) ? (object)DBNull.Value : timeAleatorio);
                commando.Parameters.AddWithValue("@times", string.IsNullOrEmpty(times) ? (object)DBNull.Value : times);
                commando.Parameters.AddWithValue("@timesLatencia", string.IsNullOrEmpty(timesLatencia.ToString()) ? (object)DBNull.Value : timesLatencia);
                commando.Parameters.AddWithValue("@colorFondo", string.IsNullOrEmpty(colorFondo.ToString()) ? (object)DBNull.Value : colorFondo);
                commando.Parameters.AddWithValue("@ordenTachado", string.IsNullOrEmpty(controlTachado.ToString()) ? (object)DBNull.Value : controlTachado);
                commando.Parameters.AddWithValue("@JsonTikGrafica", string.IsNullOrEmpty(JsonTikGrafica.ToString()) ? (object)DBNull.Value : JsonTikGrafica);
                commando.Parameters.AddWithValue("@JsonTikTiempos", string.IsNullOrEmpty(JsonTikTiempos) ? (object)DBNull.Value : JsonTikTiempos);

                // Abrir la conexión y ejecutar el comando
                commando.ExecuteNonQuery();
            }
        }


        public DataTable devolverTodasLasRejillas()
        {
            DataTable dt = new DataTable();
            string consulta = "SELECT * FROM Rejilla";


            SqliteCommand cmd = new SqliteCommand(consulta, conexion);
            SqliteDataReader reader = cmd.ExecuteReader();

            dt.Load(reader);
            return dt;
        }


        public DataTable devolverRejillaPorDeporte(string deporte)
        {
            DataTable dt = new DataTable();
            string consulta = "SELECT Codigo FROM Usuarios WHERE Deportes LIKE @deporte";
            List<int> listaUsuariosCunmplen = new List<int>();

            SqliteCommand cmd = new SqliteCommand(consulta, conexion);
            cmd.Parameters.AddWithValue("@deporte", deporte + "%");
            SqliteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                listaUsuariosCunmplen.Add(Convert.ToInt32(reader[0].ToString()));
            }
            reader.Close();
            string listaFkUsers = String.Join(", ", listaUsuariosCunmplen);
            consulta = $"SELECT * FROM Rejilla WHERE FKUsuarios IN ({listaFkUsers})";
            cmd = new SqliteCommand(consulta,conexion);
            reader = cmd.ExecuteReader();

            dt.Load(reader);
            return dt;
        }

        public DataTable devolverRejillaPorPosicion(string posicion)
        {
            DataTable dt = new DataTable();
            string consulta = "SELECT Codigo FROM Usuarios WHERE Posicion LIKE @posicion";
            List<int> listaUsuariosCunmplen = new List<int>();

            SqliteCommand cmd = new SqliteCommand(consulta, conexion);
            cmd.Parameters.AddWithValue("@posicion", posicion + "%");
            SqliteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                listaUsuariosCunmplen.Add(Convert.ToInt32(reader[0].ToString()));
            }
            reader.Close();
            string listaFkUsers = String.Join(",", listaUsuariosCunmplen);
            consulta = $"SELECT * FROM Rejilla WHERE FKUsuarios IN ({listaFkUsers})";
            cmd = new SqliteCommand(@consulta,conexion);
            reader = cmd.ExecuteReader();

            dt.Load(reader);
            return dt;
        }

        public DataTable devolverRejillaPorPais(string pais)
        {
            DataTable dt = new DataTable();
            string consulta = "SELECT Codigo FROM Usuarios WHERE Pais LIKE @pais";
            List<int> listaUsuariosCunmplen = new List<int>();

            SqliteCommand cmd = new SqliteCommand(consulta, conexion);
            cmd.Parameters.AddWithValue("@pais", pais + "%");
            SqliteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                listaUsuariosCunmplen.Add(Convert.ToInt32(reader[0].ToString()));
            }
            reader.Close();
            string listaFkUsers = String.Join(",", listaUsuariosCunmplen);
            consulta = $"SELECT * FROM Rejilla WHERE FKUsuarios IN ({listaFkUsers})";
            cmd = new SqliteCommand(@consulta, conexion);
            reader = cmd.ExecuteReader();

            dt.Load(reader);
            return dt;
        }

        public DataTable devolverRejillaPorTipoRejilla(string tipoRejilla)
        {
            DataTable dt = new DataTable();
            string consulta = "SELECT * FROM Rejilla WHERE TipoRejilla LIKE @tipoRejilla";


            SqliteCommand cmd = new SqliteCommand(consulta, conexion);
            cmd.Parameters.AddWithValue("@tipoRejilla", tipoRejilla + "%");
            SqliteDataReader reader = cmd.ExecuteReader();

            dt.Load(reader);
            return dt;
        }

        public DataTable devolverRejillaPorDeporteyPosicion(string deporte, string posicion)
        {
            DataTable dt = new DataTable();
            string consulta = "SELECT Codigo FROM Usuarios WHERE Deporte LIKE @deporte and Posicion LIKE @posicion";
            List<int> listaUsuariosCunmplen = new List<int>();

            SqliteCommand cmd = new SqliteCommand(consulta, conexion);
            cmd.Parameters.AddWithValue("@deporte", deporte + "%");
            cmd.Parameters.AddWithValue("@posicion", posicion + "%");
            SqliteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                listaUsuariosCunmplen.Add(Convert.ToInt32(reader[0].ToString()));
            }
            string listaFkUsers = String.Join(",", listaUsuariosCunmplen);
            consulta = $"SELECT * FROM Rejilla WHERE FKUsuarios IN ({listaFkUsers})";
            cmd = new SqliteCommand(@consulta, conexion);
            reader = cmd.ExecuteReader();

            dt.Load(reader);
            return dt;
        }

        public DataTable devolverRejillaPorEdad(string inicio, string fin)
        {
            DataTable dt = new DataTable();
            int primero = 0, ultimo = 0;
            primero = Convert.ToInt32(inicio);
            ultimo = Convert.ToInt32(fin);
            string consulta = "SELECT Codigo FROM Usuarios WHERE CAST(Edad AS INTEGER) BETWEEN @primero AND @ultimo";//los extremos están incluidos
            List<int> listaUsuariosCumplen = new List<int>();

            SqliteCommand cmd = new SqliteCommand(consulta, conexion);
            cmd.Parameters.AddWithValue("@primero", primero);
            cmd.Parameters.AddWithValue("@ultimo", ultimo);
            SqliteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                listaUsuariosCumplen.Add(Convert.ToInt32(reader[0].ToString()));
            }
            string listaFkUsers = String.Join(",", listaUsuariosCumplen);

            string inClause = $"({listaFkUsers})";
            reader.Close();
            
            consulta = $"SELECT * FROM Rejilla WHERE FKUsuarios IN {inClause}";
            cmd = new SqliteCommand(consulta,conexion);
            reader = cmd.ExecuteReader();
            dt.Load(reader);
            return dt;
        }

        public DataTable devolverRejillaPorFecha(string inicio, string fin)
        {
            DataTable dt = new DataTable();
            string consulta = "SELECT * FROM Rejilla WHERE FechaFormateada >= @inicio AND FechaFormateada <= @fin";

            SqliteCommand cmd = new SqliteCommand(consulta, conexion);
            cmd.Parameters.AddWithValue("@inicio", inicio);
            cmd.Parameters.AddWithValue("@fin", fin);

            SqliteDataReader reader = cmd.ExecuteReader();

            dt.Load(reader);
            reader.Close();
       
            return dt;
        }

        public string rutaFicheroBD()
        {
            return this.rutaBD;
        }

    }

}

