Imports System.Data.SQLite
Imports System.IO

Public Class Main
    Private Sub CargarContactos()
        Dim dt As New DataTable()

        Using con As SQLiteConnection = Conexion.ObtenerConexion()
            Dim query As String = "SELECT * FROM contactos"
            Dim cmd As New SQLiteCommand(query, con)
            Dim da As New SQLiteDataAdapter(cmd)
            da.Fill(dt)
        End Using

        dgvContactos.DataSource = dt
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' DatabaseHelper.CrearBaseDeDatos()
        CargarContactos()



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

    Private Sub dgvContactos_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvContactos.CellContentClick

    End Sub
End Class
