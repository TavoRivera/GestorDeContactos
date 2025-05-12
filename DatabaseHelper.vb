Imports System.Data.SQLite
Imports System.IO

Public Class DatabaseHelper
    Public Shared Sub CrearBaseDeDatos()
        Dim rutaBD As String = "contactos.db"

        ' Crear la base si no existe
        If Not File.Exists(rutaBD) Then
            SQLiteConnection.CreateFile(rutaBD)
        End If

        ' Abrir conexión
        Using con As New SQLiteConnection("Data Source=contactos.db;Version=3;")
            con.Open()

            ' Crear tabla contactos si no existe
            Dim sqlContactos As String = "CREATE TABLE IF NOT EXISTS contactos (
                                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            nombre TEXT NOT NULL,
                                            telefono TEXT,
                                            correo TEXT,
                                            direccion TEXT
                                          );"

            ' Crear tabla roles
            Dim sqlRoles As String = "CREATE TABLE IF NOT EXISTS roles (
                                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        nombre TEXT NOT NULL
                                      );"

            Dim insertarRoles As String = "INSERT OR IGNORE INTO roles (id, nombre) VALUES 
                                (1, 'Administrador'),
                                (2, 'Lector');"
            Using cmdInsert As New SQLiteCommand(insertarRoles, con)
                cmdInsert.ExecuteNonQuery()
            End Using

            ' Crear tabla usuarios
            Dim sqlUsuarios As String = "CREATE TABLE IF NOT EXISTS usuarios (
                                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            usuario TEXT NOT NULL,
                                            contrasena TEXT NOT NULL,
                                            rol_id INTEGER,
                                            FOREIGN KEY (rol_id) REFERENCES roles(id)
                                         );"

            ' Ejecutar comandos
            Using cmd As New SQLiteCommand(sqlContactos, con)
                cmd.ExecuteNonQuery()
            End Using

            Using cmd As New SQLiteCommand(sqlRoles, con)
                cmd.ExecuteNonQuery()
            End Using

            Using cmd As New SQLiteCommand(sqlUsuarios, con)
                cmd.ExecuteNonQuery()
            End Using

        End Using

        MessageBox.Show("Base de datos verificada/creada correctamente.", "Éxito")
    End Sub
End Class
