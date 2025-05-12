Imports System.Data.SQLite

Public Class Registro
    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub txtUsuario_TextChanged(sender As Object, e As EventArgs) Handles txtUsuario.TextChanged

    End Sub

    Private Sub txtContrasena_TextChanged(sender As Object, e As EventArgs) Handles txtContrasena.TextChanged

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub

    Private Sub btnRegistrar_Click(sender As Object, e As EventArgs) Handles btnRegistrar.Click
        Dim nombre As String = txtUsuario.Text.Trim()
        Dim usuario As String = txtUsuario.Text.Trim()
        Dim contrasena As String = txtContrasena.Text.Trim()
        Dim confirmar As String = txtConfirmar.Text.Trim()
        Dim rolId As Integer = Convert.ToInt32(cmbRoles.SelectedValue)

        ' 1. Validar campos vacíos
        If nombre = "" Or contrasena = "" Or confirmar = "" Then
            MessageBox.Show("Todos los campos son obligatorios.")
            Return
        End If

        ' 2. Validar que las contraseñas coincidan
        If contrasena <> confirmar Then
            MessageBox.Show("Las contraseñas no coinciden.")
            Return
        End If

        ' 3. Verificar si el usuario ya existe
        Using con As SQLiteConnection = Conexion.ObtenerConexion()
            Dim verificarUsuarioSql As String = "SELECT COUNT(*) FROM usuarios WHERE usuario = @usuario"
            Using verificarCmd As New SQLiteCommand(verificarUsuarioSql, con)
                verificarCmd.Parameters.AddWithValue("@usuario", usuario)
                Dim existe As Integer = Convert.ToInt32(verificarCmd.ExecuteScalar())

                If existe > 0 Then
                    MessageBox.Show("El nombre de usuario ya está en uso. Elige otro.")
                    Return
                End If
            End Using

            ' 4. Insertar nuevo usuario
            Dim insertarSql As String = "INSERT INTO usuarios( usuario, contrasena, rol_id) VALUES ( @usuario, @contrasena, @rol_id)"
            Using insertarCmd As New SQLiteCommand(insertarSql, con)
                insertarCmd.Parameters.AddWithValue("@usuario", usuario)
                insertarCmd.Parameters.AddWithValue("@contrasena", contrasena)
                insertarCmd.Parameters.AddWithValue("@rol_id", rolId)
                insertarCmd.ExecuteNonQuery()
            End Using
        End Using

        MessageBox.Show("Usuario registrado correctamente.")
        '    Me.Close() ' O redirigir a LoginForm


        Me.Hide()
        Dim frmLogin As New Login()
        frmLogin.ShowDialog()
        Me.Close()
    End Sub


    Private Sub Registro_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarRoles()


    End Sub
    Private Sub CargarRoles()
        Using con As SQLiteConnection = Conexion.ObtenerConexion()
            Dim sql As String = "SELECT id, nombre FROM roles"
            Using cmd As New SQLiteCommand(sql, con)
                Using reader As SQLiteDataReader = cmd.ExecuteReader()
                    Dim dt As New DataTable()
                    dt.Load(reader)

                    cmbRoles.DisplayMember = "nombre"
                    cmbRoles.ValueMember = "id"
                    cmbRoles.DataSource = dt
                End Using
            End Using
        End Using
    End Sub

End Class