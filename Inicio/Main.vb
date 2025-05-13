Imports System.Data.SQLite
Imports System.IO
Imports GestordeContactos2.My.Resources

Public Class Main
    Public Property RolUsuario As String

    Private Sub CargarContactos()
        Dim dt As New DataTable()
        dgvContactos.DataSource = Nothing ' <- Esto evita el error

        Using con As SQLiteConnection = Conexion.ObtenerConexion()
            Using cmd As New SQLiteCommand("SELECT * FROM contactos", con)
                Using reader As SQLiteDataReader = cmd.ExecuteReader()
                    dt.Load(reader)
                End Using
            End Using
        End Using

        dgvContactos.DataSource = dt
    End Sub



    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'DatabaseHelper.CrearBaseDeDatos()
        CargarContactos()


        btnReporte.Image = Informacion.reporte.ToBitmap()
        btnReporte.ImageAlign = ContentAlignment.MiddleLeft ' (opcional)
        btnReporte.TextAlign = ContentAlignment.MiddleRight ' (opcional)
        btnReporte.Image = New Bitmap(Informacion.reporte.ToBitmap(), New Size(24, 24)) ' ajusta tamaño



        ' Restricciones por rol
        If RolUsuario = "lector" Then
            btnAgregar.Enabled = False
            btnEditar.Enabled = False
            btnEliminar.Enabled = False
            btnUsuarios.Visible = False ' Este botón sería el que abre el formulario de usuarios
        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        Dim form As New GestorForm()

        If form.ShowDialog() = DialogResult.OK Then
            CargarContactos()
        End If
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If dgvContactos.SelectedRows.Count = 0 Then
            MessageBox.Show("Por favor selecciona un contacto para editar.")
            Return
        End If


        Dim contactoId As Integer = Convert.ToInt32(dgvContactos.SelectedRows(0).Cells("id").Value)
        Dim form As New GestorForm()
        form.ContactoId = contactoId

        If form.ShowDialog() = DialogResult.OK Then
            CargarContactos()
        End If


    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If dgvContactos.SelectedRows.Count = 0 Then
            MessageBox.Show("Por favor selecciona un contacto para eliminar.")
            Return
        End If
        Dim resultado = MessageBox.Show("¿Estás seguro de que deseas eliminar este contacto?", "Confirmar eliminación", MessageBoxButtons.YesNo)
        If resultado = DialogResult.Yes Then
            Dim contactoId As Integer = Convert.ToInt32(dgvContactos.SelectedRows(0).Cells("id").Value)

            Using con As SQLiteConnection = Conexion.ObtenerConexion()
                Dim query As String = "DELETE FROM contactos WHERE id = @id"
                Dim cmd As New SQLiteCommand(query, con)
                cmd.Parameters.AddWithValue("@id", contactoId)
                cmd.ExecuteNonQuery()
            End Using

            CargarContactos()
        End If
    End Sub


    Private Sub dgvContactos_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvContactos.CellDoubleClick
        If RolUsuario.ToLower() = "lector" Then
            MessageBox.Show("No tienes permiso para acceder al gestor.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        ' Verifica que la fila clickeada no sea el encabezado o una fila nueva
        If e.RowIndex >= 0 Then
            Dim fila As DataGridViewRow = dgvContactos.Rows(e.RowIndex)
            Dim idObj = fila.Cells("id").Value

            ' Si hay un ID, abrimos para editar
            If idObj IsNot Nothing AndAlso Not IsDBNull(idObj) Then
                Dim contactoId As Integer = Convert.ToInt32(idObj)
                Dim form As New GestorForm()
                form.ContactoId = contactoId

                If form.ShowDialog() = DialogResult.OK Then
                    CargarContactos()
                End If

            Else
                ' Si no hay ID, abrir para agregar nuevo
                Dim form As New GestorForm()
                If form.ShowDialog() = DialogResult.OK Then
                    CargarContactos()
                End If
            End If
        End If
    End Sub

    Private Sub btnReporte_Click(sender As Object, e As EventArgs) Handles btnReporte.Click
        Dim reporte As New FormReporte()
        reporte.ShowDialog()
    End Sub

    Private Sub btnUsuarios_Click(sender As Object, e As EventArgs) Handles btnUsuarios.Click
        If RolUsuario.ToLower() = "administrador" Then
            Dim frmUsuarios As New Usuarios()
            frmUsuarios.ShowDialog()
        Else
            MessageBox.Show("No tienes permiso para acceder a esta función.")
        End If
    End Sub

    Private Sub btnCerrarSesion_Click(sender As Object, e As EventArgs) Handles btnCerrarSesion.Click
        Dim resultado As DialogResult = MessageBox.Show("¿Estás seguro de que deseas cerrar sesión?", "Cerrar sesión", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If resultado = DialogResult.Yes Then
            ' Mostrar el formulario de login nuevamente
            Dim frmLogin As New Login()
            frmLogin.Show()

            ' Cerrar el formulario principal actual
            Me.Close()
        End If
    End Sub
    Private Sub Main_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Application.Exit()
    End Sub

End Class
