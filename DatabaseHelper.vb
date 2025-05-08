Imports System.Data.SQLite
Imports System.IO

Public Class DatabaseHelper
    Public Shared Sub CrearBaseDeDatos()
        Dim rutaBD As String = "contactos.db"

        ' Solo crear si no existe
        If Not File.Exists(rutaBD) Then
            SQLiteConnection.CreateFile(rutaBD)

            Using con As New SQLiteConnection("Data Source=contactos.db;Version=3;")
                con.Open()
                Dim sql As String = "CREATE TABLE contactos (
                                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        nombre TEXT NOT NULL,
                                        telefono TEXT,
                                        correo TEXT,
                                        direccion TEXT
                                    );"

                Using cmd As New SQLiteCommand(sql, con)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("Base de datos creada correctamente.", "Éxito")
        End If
    End Sub
End Class
