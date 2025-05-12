Imports System.Data.SQLite


Public Class Login
    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim usuario As String = txtUsuario.Text.Trim()
        Dim contrasena As String = txtContrasena.Text.Trim()

        Try
            Using con As SQLiteConnection = Conexion.ObtenerConexion()
                ' Primero obtener el rol_id del usuario
                Dim cmd As New SQLiteCommand("SELECT rol_id FROM usuarios WHERE usuario = @usuario AND contrasena = @contrasena", con)
                cmd.Parameters.AddWithValue("@usuario", usuario)
                cmd.Parameters.AddWithValue("@contrasena", contrasena)

                Dim rolId As Object = cmd.ExecuteScalar()

                If rolId IsNot Nothing Then
                    ' Ahora obtener el nombre del rol
                    Dim cmdRol As New SQLiteCommand("SELECT nombre FROM roles WHERE id = @rol_id", con)
                    cmdRol.Parameters.AddWithValue("@rol_id", rolId)

                    Dim nombreRol As Object = cmdRol.ExecuteScalar()

                    If nombreRol IsNot Nothing Then
                        ' Usuario autenticado correctamente
                        Dim frmMain As New Main()
                        frmMain.RolUsuario = nombreRol.ToString().ToLower() ' opcional: convertir a minúsculas
                        frmMain.Show()
                        Me.Hide()
                    Else
                        MessageBox.Show("Rol no encontrado.")
                    End If
                Else
                    MessageBox.Show("Usuario o contraseña incorrectos.")
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error al intentar iniciar sesión: " & ex.Message)
        End Try
    End Sub

    Private Sub txtUsuario_TextChanged(sender As Object, e As EventArgs) Handles txtUsuario.TextChanged

    End Sub

    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub btnRegistrar_Click(sender As Object, e As EventArgs) Handles btnRegistrar.Click
        Me.Hide()
        Dim frmRegistrar As New Registro()
        frmRegistrar.ShowDialog()
        Me.Close()
    End Sub
End Class