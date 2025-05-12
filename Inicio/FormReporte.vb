Imports System.Data.SQLite
Imports CrystalDecisions.CrystalReports.Engine

Public Class FormReporte
    Private Sub CargarReporte(Optional filtroNombre As String = "")
        Dim ds As New Contactos()

        Using con As SQLiteConnection = Conexion.ObtenerConexion()
            Dim consulta As String = "SELECT * FROM contactos"
            If Not String.IsNullOrEmpty(filtroNombre) Then
                consulta &= " WHERE nombre LIKE @nombre"
            End If

            Dim da As New SQLiteDataAdapter(consulta, con)

            If Not String.IsNullOrEmpty(filtroNombre) Then
                da.SelectCommand.Parameters.AddWithValue("@nombre", "%" & filtroNombre & "%")
            End If

            da.Fill(ds.Tables("contactos"))
        End Using

        Dim rpt As New ReporteContactos()
        rpt.SetDataSource(ds)

        CrystalReportViewer1.ReportSource = rpt
        CrystalReportViewer1.Refresh()
    End Sub

    Private Sub FormReporte_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Crear una instancia del DataSet tipado (Contactos.xsd)
        CargarReporte()

        Dim ds As New Contactos()

        ' Conectarse a la base de datos con el mismo archivo usado en toda la app
        Using con As SQLiteConnection = Conexion.ObtenerConexion()
            ' Preparar adaptador de datos
            Dim da As New SQLiteDataAdapter("SELECT * FROM contactos", con)

            ' Llenar el DataTable del DataSet tipado. El nombre debe coincidir con Contactos.xsd
            da.Fill(ds.Tables("contactos"))
        End Using

        ' Crear instancia del reporte
        Dim rpt As New ReporteContactos()

        ' Establecer la fuente de datos del reporte
        rpt.SetDataSource(ds)

        ' Mostrar el reporte en el visor
        CrystalReportViewer1.ReportSource = rpt
        CrystalReportViewer1.Refresh()
    End Sub

    Private Sub CrystalReportViewer1_Load(sender As Object, e As EventArgs) Handles CrystalReportViewer1.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub btnFiltrar_Click(sender As Object, e As EventArgs) Handles btnFiltrar.Click
        Dim nombreFiltro As String = txtFiltro.Text.Trim()
        CargarReporte(nombreFiltro)
    End Sub
End Class