Imports System.Data.SQLite

Public Class GestorForm
    Public ContactoId As Integer = 0 ' 0 = nuevo contacto

    Private Sub GestorForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If ContactoId > 0 Then
            CargarContacto()
        End If
    End Sub

    Private Sub CargarContacto()
        Using con As SQLiteConnection = Conexion.ObtenerConexion()
            Dim query As String = "SELECT * FROM contactos WHERE id = @id"
            Dim cmd As New SQLiteCommand(query, con)
            cmd.Parameters.AddWithValue("@id", ContactoId)
            Using reader = cmd.ExecuteReader()
                If reader.Read() Then
                    txtNombre.Text = reader("nombre").ToString()
                    txtTelefono.Text = reader("telefono").ToString()
                    txtCorreo.Text = reader("correo").ToString()
                    txtDireccion.Text = reader("direccion").ToString()
                End If
            End Using
        End Using
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If txtNombre.Text.Trim() = "" Then
            MessageBox.Show("El nombre es obligatorio.")
            Return
        End If
        Try
            Using con As SQLiteConnection = Conexion.ObtenerConexion()
                Using cmd As SQLiteCommand = con.CreateCommand()
                    If ContactoId = 0 Then
                        ' INSERT
                        cmd.CommandText = "INSERT INTO contactos (nombre, telefono, correo, direccion) VALUES (@nombre, @telefono, @correo, @direccion)"
                    Else
                        ' UPDATE
                        cmd.CommandText = "UPDATE contactos SET nombre = @nombre, telefono = @telefono, correo = @correo, direccion = @direccion WHERE id = @id"
                        cmd.Parameters.AddWithValue("@id", ContactoId)
                    End If

                    cmd.Parameters.AddWithValue("@nombre", txtNombre.Text)
                    cmd.Parameters.AddWithValue("@telefono", txtTelefono.Text)
                    cmd.Parameters.AddWithValue("@correo", txtCorreo.Text)
                    cmd.Parameters.AddWithValue("@direccion", txtDireccion.Text)

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try


        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.Close()
    End Sub

    Private Sub txtCorreo_TextChanged(sender As Object, e As EventArgs) Handles txtCorreo.TextChanged

    End Sub
End Class
