Imports System.Data.OleDb '載入類別庫
Public Class Form2
    Dim Provider As String = "Microsoft.ACE.OLEDB.12.0" 'Database provider
    Dim Datasource As String = Application.StartupPath & "\data.accdb" 'Database path
    Dim Security As Boolean = False 'Persist Security
    Dim Mycn As OleDbConnection = New OleDbConnection("Provider=" & Provider & ";Data Source=" &
                                                      Datasource & ";Persist Security Info=" &
                                                      Security & ";") 'SQL連接參數
    Dim DB_Table As String = "tb_test" 'Database table
    Dim Command As OleDbCommand
    Dim icount As Integer = 0
    Dim SQLstr As String = ""
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click


        Try
            Mycn.Open() '連接SQL

            'SQL條件式 INSERT INTO [TableName] VALUES ('A_Data', 'B_Data', 'C_Data', 'D_Data')
            SQLstr = "INSERT INTO " & DB_Table & " VALUES('" & TxtDevice.Text & "','" &
                                                                 TxtIssue.Text & "','" &
                                                                   TxtMail.Text & "','" &
                                                                      TxtTime.Text & "')"

            Command = New OleDbCommand(SQLstr, Mycn) '產生物件(SQL條件式,SQL連線參數)
            icount = Command.ExecuteNonQuery '回傳建立列數
            MsgBox("新建 " & icount & " 筆資料", vbInformation, "Insert success")

        Catch ex As Exception
            MsgBox(ex, vbExclamation, "Error") '錯誤顯示
        End Try
        Mycn.Close() '關閉連接
    End Sub

End Class