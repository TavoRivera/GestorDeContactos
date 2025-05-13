Imports System.Data.SQLite
Public Class Usuarios
    Private Sub Usuarios_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarUsuarios()
    End Sub

    Private Sub CargarUsuarios()
        Try
            Using con As SQLiteConnection = Conexion.ObtenerConexion()
                Dim cmd As New SQLiteCommand("SELECT u.id, u.usuario, u.contrasena,  r.nombre AS rol_nombre
                    FROM usuarios u
                    JOIN roles r ON u.rol_id = r.id;
                    ", con)
                Dim da As New SQLiteDataAdapter(cmd)
                Dim dt As New DataTable()
                da.Fill(dt)

                dgvUsuarios.DataSource = dt

                ' Opcional: ocultar el ID y mostrar el nombre del rol
                dgvUsuarios.Columns("id").Visible = False


            End Using
        Catch ex As Exception

        End Try
    End Sub

    Private Function ObtenerNombreRol(rolId As Integer) As String
        Using con As SQLiteConnection = Conexion.ObtenerConexion()
            Dim cmd As New SQLiteCommand("SELECT nombre FROM roles WHERE id = @rol_id", con)
            cmd.Parameters.AddWithValue("@rol_id", rolId)
            Dim resultado As Object = cmd.ExecuteScalar()
            If resultado IsNot Nothing Then
                Return resultado.ToString()
            End If
        End Using
        Return "Desconocido"
    End Function

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If dgvUsuarios.SelectedRows.Count > 0 Then
            Dim usuarioId As Integer = Convert.ToInt32(dgvUsuarios.SelectedRows(0).Cells("id").Value)
            Dim confirm As DialogResult = MessageBox.Show("¿Estás seguro de eliminar este usuario?", "Confirmar", MessageBoxButtons.YesNo)

            If confirm = DialogResult.Yes Then
                Try
                    Using con As SQLiteConnection = Conexion.ObtenerConexion()
                        Dim cmd As New SQLiteCommand("DELETE FROM usuarios WHERE id = @id", con)
                        cmd.Parameters.AddWithValue("@id", usuarioId)
                        cmd.ExecuteNonQuery()
                    End Using
                    CargarUsuarios()
                Catch ex As Exception
                    MessageBox.Show("Error al eliminar usuario: " & ex.Message)
                End Try
            End If
        Else
            MessageBox.Show("Selecciona un usuario para eliminar.")
        End If
    End Sub

    Private Sub btnRegistrar_Click(sender As Object, e As EventArgs) Handles btnRegistrar.Click
        Me.Hide()
        Dim frmRegistrar As New Registro()
        frmRegistrar.ShowDialog()
        Me.Close()
    End Sub
End Class