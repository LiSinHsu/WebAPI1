Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports Newtonsoft.Json

Public Class WebForm1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadData()
    End Sub

    Private Sub LoadData()

        Try

            Dim request As WebRequest = WebRequest.Create("https://od.moi.gov.tw/api/v1/rest/datastore/301110000A-001859-001")
            request.Credentials = CredentialCache.DefaultCredentials
            Dim response As WebResponse = request.GetResponse()
            Dim dataStream As Stream = response.GetResponseStream()

            Dim reader As New StreamReader(dataStream)
            Dim JsonStr As String = reader.ReadToEnd()

            Dim Obj As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.JsonConvert.DeserializeObject(JsonStr)

            'Dim fields As String = Obj.Item("result")("fields").ToString

            Dim Dt As New DataTable
            Dt = JsonConvert.DeserializeObject(Of DataTable)(Obj.Item("result")("records").ToString)

            div1.InnerHtml += "109年替代役役男贍養金發放統計表"
            div1.InnerHtml += "<table border='1'><tr>"

            For t As Integer = 0 To Dt.Columns.Count - 1
                div1.InnerHtml += "<td>" & Dt.Columns(t).ColumnName & "</td>"
            Next

            For i As Integer = 0 To Dt.Rows.Count - 1
                div1.InnerHtml += "<tr>"
                For j As Integer = 0 To Dt.Columns.Count - 1
                    div1.InnerHtml += "<td>" & Dt.Rows(i)(j).ToString & "</td>"
                Next j

                div1.InnerHtml += "</tr>"
            Next i

            div1.InnerHtml += "</tr></table>"

            Dim cnn As New SqlConnection
            Dim cmd As New SqlCommand
            Dim dr As SqlDataReader = Nothing
            Dim dtResult As New DataTable
            'cnn.ConnectionString = "Data Source=(localdb)\ProjectsV13;Initial Catalog=TEST;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
            cnn.ConnectionString = "Server=(localdb)\ProjectsV13;database=TEST;"
            'uid = spacecraftU1;pwd=Appolo11;database = spacecraft_db;Server = DESKTOP - 99K0FRS\\PRANEETHDB
            cnn.Open()

            cmd.Connection = cnn
            cmd.CommandText = "select * from disbursed_amount"

            dr = cmd.ExecuteReader
            dtResult.Load(dr)

            If dtResult.Rows.Count = 0 Then
                cmd.CommandText = ""
                Dim i As Integer = 0
                For Each drData As DataRow In Dt.Rows
                    i += 1
                    cmd.CommandText += "insert into disbursed_amount (id, month, amount, number, remark) values (" & i & ",N'" & drData("Month").ToString() & "', '" & drData("disbursed_amount").ToString() & "', '" & drData("disbursed_number").ToString() & "', N'" & drData("remarks").ToString() & "'); "
                Next
                cmd.ExecuteNonQuery()
            End If

            cmd.Dispose()
            cnn.Close()

            reader.Close()
            response.Close()

        Catch ex As Exception
            Throw (ex)
        End Try
    End Sub

End Class