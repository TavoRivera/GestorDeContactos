Imports System.Data.SQLite

Public Class Conexion
    Private Shared conexionString As String = "Data Source=contactos.db;Version=3;"

    Public Shared Function ObtenerConexion() As SQLiteConnection
        Dim con As New SQLiteConnection(conexionString)
        con.Open()
        Return con
    End Function
End Class
