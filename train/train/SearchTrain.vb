Imports System.IO
Imports System.Data.OleDb
Imports System.Text.RegularExpressions

Public Class SearchTrain

    Private Sub SearchTrain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Show()

        '日期出初始化設定
        With DateTimePicker1
            .MinDate = Now.Date                 '預設從今日讀取時刻表
            .MaxDate = DateTime.Now.AddDays(44) '往後推44天這範圍查詢火車時刻
            .Value = Now.Date
        End With

        loading() '等待網頁讀取完成

        '讀取車站代號
        Try

            Dim Path As New StreamReader(Application.StartupPath & "\train_new.dt", System.Text.Encoding.Default)
            Dim sLine As String = ""
            Dim i As Integer

            Do

                sLine = Path.ReadLine()

                For i = 1 To Len(sLine)
                    If Mid(sLine, i, 1) = "," Then Exit For
                    Application.DoEvents()
                Next


                If Not sLine Is Nothing Then
                    fromstation_Cmb.Items.Add(Mid(sLine, 1, i - 1)) '起站站名
                    tostation_Cmb.Items.Add(Mid(sLine, 1, i - 1)) '迄站站名

                    from_NO.Items.Add(Mid(sLine, i + 1, Len(sLine) - i - 6)) '起站車站代號
                    to_NO.Items.Add(Mid(sLine, i + 1, Len(sLine) - i - 6)) '訖站車站代號

                    fromstation_Ticket.Items.Add(Mid(sLine, i + 6, Len(sLine) - i - 7)) '訂票起站
                    tostation_Ticket.Items.Add(Mid(sLine, i + 6, Len(sLine) - i - 7)) '訂票訖站

                    from_check.Items.Add(Mid(sLine, i + 10, Len(sLine) - i)) '起站區域
                    to_check.Items.Add(Mid(sLine, i + 10, Len(sLine) - i)) '訖站區域
                    'tostation_Cmb.Items.Add(Mid(sLine, i + 1, Len(sLine) - i))
                End If
            Loop Until sLine Is Nothing
            Path.Close()

            i = Nothing
            Path = Nothing
            sLine = Nothing

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        ''讀取訂車票代號
        'Try
        '    Dim Path_Ticket As New StreamReader(Application.StartupPath & "\ticket.dt", System.Text.Encoding.Default)
        '    Dim sLine As String = ""
        '    Dim i As Integer

        '    Do

        '        sLine = Path_Ticket.ReadLine()

        '        For i = 1 To Len(sLine)
        '            If Mid(sLine, i, 1) = "," Then Exit For
        '            Application.DoEvents()
        '        Next


        '        If Not sLine Is Nothing Then
        '            fromstation_Ticket.Items.Add(Mid(sLine, 1, 3))
        '            tostation_Ticket.Items.Add(Mid(sLine, 1, 3))
        '        End If
        '    Loop Until sLine Is Nothing
        '    Path_Ticket.Close()

        '    i = Nothing
        '    Path_Ticket = Nothing
        '    sLine = Nothing
        'Catch ex As Exception
        '    MsgBox(ex.Message)
        'End Try

    End Sub


    '車站名稱互換
    Private Sub changestation_btn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles changestation_btn.Click
        Dim change_word As String

        change_word = fromstation_Cmb.Text
        fromstation_Cmb.Text = tostation_Cmb.Text
        tostation_Cmb.Text = change_word
        change_word = Nothing
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        '初始化查詢
        WebBrowser1.Navigate("http://twtraffic.tra.gov.tw/twrail/nonscript.aspx")
        DataGridView1.Rows.Clear()

        loading()
        Search_info.Visible = True

        Dim form As HtmlElement = WebBrowser1.Document.GetElementById("Search") '輸入站名
        Try
            If form IsNot Nothing Then
                With form
                    Select Case from_check.Text
                        Case 0
                            .All("FromStationList1").SetAttribute("value", from_NO.Text)                                        '站名(起站)
                        Case 1
                            .All("FromStationList2").SetAttribute("value", from_NO.Text)                                        '站名(起站)
                        Case 2
                            .All("FromStationList3").SetAttribute("value", from_NO.Text)                                        '站名(起站)
                        Case 3
                            .All("FromStationList4").SetAttribute("value", from_NO.Text)                                        '站名(起站)
                    End Select

                    Select Case to_check.Text
                        Case 0
                            .All("ToStationList1").SetAttribute("value", to_NO.Text)                                            '站名(迄站)
                        Case 1
                            .All("ToStationList2").SetAttribute("value", to_NO.Text)                                            '站名(迄站)
                        Case 2
                            .All("ToStationList3").SetAttribute("value", to_NO.Text)                                            '站名(迄站)
                        Case 3
                            .All("ToStationList4").SetAttribute("value", to_NO.Text)                                            '站名(迄站)
                    End Select

                    .All("SearchDateSelect").SetAttribute("value", DateTimePicker1.Text)                                '日期
                    .All("FromTimeSelect").SetAttribute("value", Mid(ComboBox3.Text, 1, 2) & Mid(ComboBox3.Text, 4, 2)) '時間(起站)
                    .All("ToTimeSelect").SetAttribute("value", Mid(ComboBox4.Text, 1, 2) & Mid(ComboBox4.Text, 4, 2))   '時間(迄站)
                End With
            End If
        Catch ex As Exception
            Search_info.Visible = False
            MsgBox("自動填入出現問題 : " & vbCrLf + vbCrLf & ex.Message)
        End Try


        Dim tmp_count As Integer = ComboBox1.Tag    '讀取選項值
        Try
            '選項判斷
            For Each ele As HtmlElement In WebBrowser1.Document.All
                With ele
                    If .GetAttribute("type").ToLower = "radio" Then tmp_count += 1
                    If tmp_count = 2 Then .InvokeMember("click") : Exit For
                End With
            Next

            '執行查詢
            For Each ele As HtmlElement In WebBrowser1.Document.All
                If ele.GetAttribute("id").ToLower = "Button2".ToLower Then ele.InvokeMember("click")
            Next

        Catch ex As Exception
            MsgBox("選項判斷或執行查詢出現問題 : " & vbCrLf + vbCrLf & ex.Message)
        End Try

        '釋放記憶體
        form = Nothing
        tmp_count = Nothing


        'loading()

        'Timer1.Enabled = True : Timer1.Start()


        ''讀入表格
        'Dim Data(10) As String
        'Dim ResultView As HtmlElement = WebBrowser1.Document().GetElementById("ResultGridView") '表格名
        'Try
        '    If ResultView IsNot Nothing Then
        '        With ResultView
        '            For i As Integer = 0 To .GetElementsByTagName("tr").Count - 2 '讀取表格列表數目
        '                For j = 0 To 9 '抓取表格資料
        '                    Data(j) = .GetElementsByTagName("tr")(1 + i).GetElementsByTagName("td")(j).InnerText '表格內容
        '                Next
        '                DataGridView1.Rows.Add(New Object() {Data(0), Data(1), Data(2), Data(3), Data(4), Data(5), Data(6), Data(7), Data(8), Data(9)}) '讀出內容
        '            Next
        '        End With
        '    End If
        'Catch ex As Exception
        '    MsgBox("讀入表格出現問題 : " & ex.Message)
        'End Try

        ''釋放記憶體
        'Data = Nothing
        'ResultView = Nothing


    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Select Case ComboBox1.Text '車種選擇
            Case "對號列車" : ComboBox1.Tag = "1"
            Case "非對號列車" : ComboBox1.Tag = "0"
            Case "全部車種" : ComboBox1.Tag = "-1"
        End Select
    End Sub

    '待修改
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        WebBrowser1.Navigate("http://twtraffic.tra.gov.tw/twrail/mobile/home.aspx")

    End Sub

    Private Sub station_Cmb_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles fromstation_Cmb.Validating, tostation_Cmb.Validating

        With fromstation_Cmb
            If String.IsNullOrEmpty(.Text) And String.IsNullOrEmpty(tostation_Cmb.Text) Then
                '這裡判斷當使用者沒輸入任何資料時, 要做什麼處理
            Else
                If .SelectedIndex = -1 Then .Text = "" '防止輸入非item資料，遇錯直接清除資料
                If tostation_Cmb.SelectedIndex = -1 Then tostation_Cmb.Text = ""
                If Mid(.Text, 1, 1) = "-" Then .Text = "" : Exit Sub
                If Mid(tostation_Cmb.Text, 1, 1) = "-" Then tostation_Cmb.Text = "" : Exit Sub
                '防止重複站名出現
                If .Text = tostation_Cmb.Text Then MsgBox("無法選擇重複的站名", MsgBoxStyle.Exclamation + vbOKOnly, "Error") : e.Cancel = True
            End If
        End With

    End Sub

    Private Sub fromstation_Cmb_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles fromstation_Cmb.SelectedIndexChanged
        from_NO.SelectedIndex = fromstation_Cmb.SelectedIndex
        from_check.SelectedIndex = from_NO.SelectedIndex
        fromstation_Ticket.SelectedIndex = fromstation_Cmb.SelectedIndex
    End Sub
    Private Sub tostation_Cmb_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles tostation_Cmb.SelectedIndexChanged
        to_NO.SelectedIndex = tostation_Cmb.SelectedIndex
        to_check.SelectedIndex = to_NO.SelectedIndex
        tostation_Ticket.SelectedIndex = tostation_Cmb.SelectedIndex
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        fromstation_Cmb.SelectedIndex = 9
        tostation_Cmb.SelectedIndex = 21
        ComboBox1.SelectedIndex = 2
        ComboBox3.SelectedIndex = 0
        ComboBox4.SelectedIndex = 24
    End Sub

    Dim train_no As Integer = 0
    Private Sub WebBrowser1_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted


        If checkdata() = True Then Search_info.Visible = False : MsgBox("本次查詢無資料，請重新輸入查詢條件!") : Exit Sub


        '讀取到存入表格
        Dim Data(10) As String
        Dim ResultView As HtmlElement = WebBrowser1.Document().GetElementById("ResultGridView") '表格名
        'Dim link As HtmlElement = WebBrowser1.Document().GetElementById("TicketingLink")

        Try
            If ResultView IsNot Nothing Then
                With ResultView
                    For i As Integer = 0 To .GetElementsByTagName("tr").Count - 2 '讀取表格列表數目
                        For j = 0 To 9 '抓取表格資料
                            'Data(j) = .GetElementsByTagName("tr")(1 + i).GetElementsByTagName("td")(j).InnerText
                            Data(j) = Replace(.GetElementsByTagName("tr")(1 + i).GetElementsByTagName("td")(j).InnerText, " ", "") '表格內容
                        Next

                        'For Each link As HtmlElement In WebBrowser1.Document.All
                        '    If Not link.GetAttribute("Href") = Nothing And Strings.Right(link.GetAttribute("Href"), 3) <> "gif" And
                        '        Strings.Right(link.GetAttribute("Href"), 3) <> "jpg" And Strings.Right(link.GetAttribute("Href"), 3) <> "css" Then
                        '        'Dim DataLink = New System.Uri([HtmlElement].GetAttribute("Href"))
                        '        Dim DataLink = link.GetAttribute("Href")
                        '    End If
                        'Next


                        '顏色更改
                        Dim colorDB As Boolean = False

                        '剩餘時間
                        Dim RTime = Replace(.GetElementsByTagName("tr")(1 + i).GetElementsByTagName("td")(4).InnerText, " ", "")
                        Dim RTime_M = DateDiff(DateInterval.Minute, CDate(Now.ToShortTimeString), CDate(RTime))
                        Dim RTime_H = DateDiff(DateInterval.Hour, CDate(Now.ToShortTimeString), CDate(RTime))
                        If DateTimePicker1.Text = Now.Date Then '非當日不計算剩餘時間
                            If RTime_M > 0 Then '歸零代表火車已經開走
                                If RTime_M > 59 Then '60分上下判斷是否轉換小時
                                    If RTime_M Mod 60 <> 0 Then
                                        Data(4) += " [" & RTime_H & "時" & RTime_M Mod 60 & "分]"
                                    Else
                                        Data(4) += " [" & RTime_H & "時]"
                                    End If
                                Else
                                    Data(4) += " [" & RTime_M & "分]"
                                End If
                            Else
                                colorDB = True
                            End If
                        Else
                            Data(4) += " [ " & DateTimePicker1.Text & " ]"
                        End If

                        Try
                            Select Case Data(0)
                                Case "自強", "太魯閣", "莒光", "普悠瑪"
                                    Dim img As Image = Image.FromFile(Application.StartupPath & "\orderticket.jpg")
                                    DataGridView1.Rows.Add(New Object() {Data(0), Data(1), Data(2), Data(3), Data(4), Data(5), Data(6), Data(7), Data(8), img}) '讀出內容(可訂票)
                                Case Else
                                    DataGridView1.Rows.Add(New Object() {Data(0), Data(1), Data(2), Data(3), Data(4), Data(5), Data(6), Data(7), Data(8)}) '讀出內容
                            End Select
                        Catch img_ex As Exception
                            Search_info.Visible = False
                            MsgBox("找不到訂票圖片 : " & img_ex.Message)
                            Application.Exit()
                        End Try


                        If colorDB = True Then
                            DataGridView1.Rows(DataGridView1.RowCount - 1).DefaultCellStyle.BackColor = Color.Pink
                            colorDB = False
                        Else
                            DataGridView1.Rows(DataGridView1.RowCount - 1).DefaultCellStyle.BackColor = Color.PaleGreen
                        End If
                    Next

                    GroupBox2.Text = "火車時刻表 [ " & fromstation_Cmb.Text & " → " & tostation_Cmb.Text & " ]" '顯示起訖站

                End With
            End If

            'For Each [HtmlElement] As HtmlElement In WebBrowser1.Document.All
            '    If [HtmlElement].TagName = "id" And Not [HtmlElement].GetAttribute("Href") = Nothing Then
            '        Dim DataLink = New System.Uri([HtmlElement].get)
            '        'Dim DataLink = New System.Uri([HtmlElement].GetAttribute("Href"))
            '        MsgBox(DataLink)
            '    End If

            'Next

            'Dim link As HtmlElement = WebBrowser1.Document().GetElementById("TicketingLink")
            'If link IsNot Nothing Then
            '    MsgBox(link)
            'End If

            '只顯示綠色部分
            If CheckBox1.Checked = True Then
                For i As Integer = DataGridView1.Rows.Count - 1 To 0 Step -1
                    If DataGridView1.Rows(i).DefaultCellStyle.BackColor = Color.Pink Then
                        DataGridView1.Rows.RemoveAt(i)
                    End If
                Next
            End If

            '只顯示車次
            If train_no <> 0 Then
                For j As Integer = DataGridView1.Rows.Count - 1 To 0 Step -1
                    'MsgBox(DataGridView1(1, j).Value.ToString())
                    If train_no <> DataGridView1(1, j).Value.ToString() Then
                        DataGridView1.Rows.RemoveAt(j)
                    End If
                Next
            End If


            Search_info.Visible = False
        Catch ex As Exception
            Search_info.Visible = False
            MsgBox("讀入表格出現問題 :  " & ex.Message)
        End Try

        ' 釋放記憶體
        Data = Nothing
        ResultView = Nothing

        Me.Text = Me.Tag

    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Try
            '訂票連結
            If (e.ColumnIndex = 9) Then
                'Dim link As String = DataGridView1(e.ColumnIndex, e.RowIndex).Value.ToString()
                'MsgBox(DataGridView1(0, e.RowIndex).Value.ToString())
                Dim TrainDB As String = DataGridView1(0, e.RowIndex).Value.ToString()
                'MsgBox(TrainDB)
                'http://railway.hinet.net/ctno1.htm?from_station=092&to_station=102&getin_date=2016/04/11&train_no=105
                Dim ticketlink As String = "http://railway.hinet.net/ctno1.htm?from_station=" & fromstation_Ticket.Text & "&to_station=" & tostation_Ticket.Text &
                            "&getin_date=" & DateTimePicker1.Text & "&train_no=" & DataGridView1(1, e.RowIndex).Value.ToString()

                Select Case TrainDB
                    Case "自強", "太魯閣", "莒光", "普悠瑪"
                        'DataGridView1.Rows(DataGridView1.RowCount - 1).DefaultCellStyle.BackColor = Color.Pink
                        'DataGridView1.CurrentCell.RowIndex
                        If DataGridView1.Rows(DataGridView1.CurrentCell.RowIndex).DefaultCellStyle.BackColor = Color.Pink Then
                            MsgBox("超過時間，無法訂票！")
                        Else
                            Train_Ticket.Text = "訂票系統 - " & TrainDB
                            Train_Ticket.TextBox2.Text = fromstation_Ticket.Text & "-" & fromstation_Cmb.Text '訂票起站
                            Train_Ticket.TextBox3.Text = tostation_Ticket.Text & "-" & tostation_Cmb.Text '訂票訖站
                            Train_Ticket.TextBox4.Text = DateTimePicker1.Text '訂票日期
                            Train_Ticket.TextBox5.Text = DataGridView1(1, e.RowIndex).Value.ToString() '訂票車號

                            If TrainDB = "普悠瑪" Then
                                Train_Ticket.GroupBox2.Enabled = False
                                Train_Ticket.GroupBox3.Enabled = True
                            Else
                                Train_Ticket.GroupBox2.Enabled = True
                                Train_Ticket.GroupBox3.Enabled = False
                            End If
                            System.Diagnostics.Process.Start(ticketlink)
                            Train_Ticket.Show()




                        End If

                    Case Else
                End Select


            End If

            '車次連結
            If (e.ColumnIndex = 1) Then
                'Dim link As String = DataGridView1(e.ColumnIndex, e.RowIndex).Value.ToString()
                'MsgBox(e.ColumnIndex & " / " & e.RowIndex)
                Dim link As String = "http://twtraffic.tra.gov.tw/twrail/SearchResultContent.aspx?searchdate=" & DateTimePicker1.Text &
                    "&traincode=" & DataGridView1(e.ColumnIndex, e.RowIndex).Value.ToString() & "&trainclass=" &
                    DataGridView1(e.ColumnIndex - 1, e.RowIndex).Value.ToString() & "&mainviaroad=1&fromstation=" & from_NO.Text & "&tostation=" & to_NO.Text & "&language="
                System.Diagnostics.Process.Start(link)
            End If
        Catch ex As Exception

        End Try

    End Sub
    Private Function UrlIsExist(url As [String]) As Boolean '檢查網頁是否存在
        Dim u As System.Uri = Nothing
        Try
            u = New Uri(url)
        Catch
            Return False
        End Try
        Dim isExist As Boolean = False
        Dim r As System.Net.HttpWebRequest = TryCast(System.Net.HttpWebRequest.Create(u), System.Net.HttpWebRequest)
        r.Method = "HEAD"
        Try
            Dim s As System.Net.HttpWebResponse = TryCast(r.GetResponse(), System.Net.HttpWebResponse)
            If s.StatusCode = System.Net.HttpStatusCode.OK Then
                isExist = True
            End If
        Catch x As System.Net.WebException
            Try
                isExist = False '(TryCast(x.Response, System.Net.HttpWebResponse).StatusCode <> System.Net.HttpStatusCode.NotFound)
            Catch
                isExist = False '(x.Status = System.Net.WebExceptionStatus.Success)
            End Try
        End Try
        Return isExist
    End Function
    Dim checkUrl As Boolean = False

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        'If checkUrl = False Then
        '    If UrlIsExist("http://twtraffic.tra.gov.tw/twrail/nonscript.aspx") = False Then
        '        MsgBox("目前無法查詢! 請稍後在試~")
        '        Exit Sub
        '    Else
        '        checkUrl = True
        '    End If
        'End If


        'MsgBox("OK")

        Try

            ComboBox2.Text = Replace(ComboBox2.Text, "　", " ")
            Dim MyString = ComboBox2.Text '注入(防止全形空白，一律轉成半形空白)

            '錯字預防
            MyString = Replace(MyString, "台", "臺")
            ComboBox2.Text = Replace(MyString, "台", "臺")

            '移除左右空白字串
            ComboBox2.Text = Trim(ComboBox2.Text)

            '空格分割
            Dim charSeparators() As Char = {" "}
            Dim strResult() = MyString.Trim().Split(charSeparators, StringSplitOptions.RemoveEmptyEntries)

            '顯示分割狀況(除錯用)
            Debug.Print(" [編號] = 站名")
            For i As Integer = 0 To strResult.Length - 1
                Debug.Print(" [ " & i & " ] = " & strResult(i))
            Next

            '預設值
            DateTimePicker1.Text = Now.Date
            ComboBox3.Text = "00:00" : ComboBox4.Text = "23:59"
            ComboBox1.Text = "全部車種"




            '字串判斷
            For i As Integer = 0 To strResult.Length - 1

                Debug.Print("i = " & i & " / strResult = " & strResult(i)) '(除錯用)

                Select Case i
                    Case 0
                        '起站
                        fromstation_Cmb.SelectedIndex = -1 '初始化
                        fromstation_Cmb.Text = strResult(0)
                        If fromstation_Cmb.SelectedIndex = -1 Then
                            fromstation_Cmb.Text = ""
                            'from_NO.Text = ""
                            'from_check.Text = ""
                            MsgBox("無此起站名")
                            Exit Sub
                        End If
                    Case 1
                        '迄站
                        tostation_Cmb.SelectedIndex = -1 '初始化
                        tostation_Cmb.Text = strResult(1)
                        If tostation_Cmb.SelectedIndex = -1 Then
                            tostation_Cmb.Text = ""

                            MsgBox("無此迄站名")
                            Exit Sub
                        End If
                End Select

                train_no = 0 '車次初始化
                'Train_No_Info.Text = "" '車次初始化
                Select Case strResult(i)
                    'Case "現", "現在"
                    '    ComboBox3.Text = Now.Date.Hour & ":00" : DateTimePicker1.Text = Now.Date
                    Case "今早"
                        ComboBox3.Text = "00:00" : ComboBox4.Text = "12:00" : DateTimePicker1.Text = Now.Date
                    Case "今晚"
                        ComboBox3.Text = "12:00" : ComboBox4.Text = "23:59" : DateTimePicker1.Text = Now.Date
                    Case "明早"
                        ComboBox3.Text = "00:00" : ComboBox4.Text = "12:00" : DateTimePicker1.Text = Now.Date.AddDays(1)
                    Case "明晚"
                        ComboBox3.Text = "12:00" : ComboBox4.Text = "23:59" : DateTimePicker1.Text = Now.Date.AddDays(1)
                    Case "早", "早上"
                        ComboBox3.Text = "00:00" : ComboBox4.Text = "12:00"
                    Case "晚", "晚上"
                        ComboBox3.Text = "12:00" : ComboBox4.Text = "23:59"
                    Case "上午", "早晨"
                        ComboBox3.Text = "05:00" : ComboBox4.Text = "11:00"
                    Case "下午", "傍晚"
                        ComboBox3.Text = "13:00" : ComboBox4.Text = "17:00"
                    Case "對", "對號", "普悠", "普悠瑪", "普悠瑪號", "自強", "自強號"
                        ComboBox1.Text = "對號列車"
                    Case "非對", "非對號", "區間", "區間車", "電聯", "電聯車"
                        ComboBox1.Text = "非對號列車"
                    Case "今", "今天", "今日"
                        DateTimePicker1.Text = Now.Date
                    Case "明", "明日", "明天"
                        DateTimePicker1.Text = Now.Date.AddDays(1)
                    Case "後", "後日", "後天"
                        DateTimePicker1.Text = Now.Date.AddDays(2)
                    Case Else
                        Dim ms As MatchCollection = Regex.Matches(strResult(i), """([^""]*)""") '車次
                        For Each m As Match In ms
                            'MsgBox(m.Groups(1).Value) '車次編號(4位數)
                            train_no = m.Groups(1).Value
                            'Train_No_Info.Text = m.Groups(1).Value
                        Next
                End Select



            Next

            '呼叫執行
            Me.Button1_Click(Nothing, Nothing)

        Catch ex As Exception
            If ComboBox2.Text = Nothing Then
                MsgBox("請勿空白!")
            End If
        End Try


    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim MyExcel As New Microsoft.Office.Interop.Excel.Application()
        MyExcel.Application.Workbooks.Add()
        MyExcel.Visible = True

        '獲取標題
        Dim Cols As Integer
        For Cols = 1 To DataGridView1.Columns.Count
            MyExcel.Cells(1, Cols) = DataGridView1.Columns(Cols - 1).HeaderText
        Next

        '往excel表裡添加資料()
        Dim i As Integer
        For i = 0 To DataGridView1.RowCount - 1
            Dim j As Integer
            For j = 0 To DataGridView1.ColumnCount - 1
                If Me.DataGridView1(j, i).Value Is System.DBNull.Value Then

                    MyExcel.Cells(i + 2, j + 1) = ""
                Else
                    MyExcel.Cells(i + 2, j + 1) = DataGridView1(j, i).Value.ToString

                End If
            Next j
        Next i
    End Sub
    Dim Datasource As String = Application.StartupPath & "\test.accdb"
    Dim connString As String = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" & Datasource & ""
    Dim MyConn As OleDbConnection
    Dim da As OleDbDataAdapter
    Dim ds As DataSet
    Dim tables As DataTableCollection
    Dim source1 As New BindingSource
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        MyConn = New OleDbConnection
        MyConn.ConnectionString = connString
        ds = New DataSet
        tables = ds.Tables
        da = New OleDbDataAdapter("Select * from [items]", MyConn) 'Change items to your database name
        da.Fill(ds, "items") 'Change items to your database name
        Dim view As New DataView(tables(0))
        source1.DataSource = view
        DataGridView1.DataSource = view

    End Sub

    Private Sub ComboBox2_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles ComboBox2.KeyDown
        If e.KeyCode = Keys.Enter Then Call Button3_Click(Nothing, Nothing)
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        'MsgBox(train_no)
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        '選取指定車次
        DataGridView1.ClearSelection()
            For j As Integer = DataGridView1.Rows.Count - 1 To 0 Step -1
            If Train_No_Info.Text = DataGridView1(1, j).Value.ToString() Then
                DataGridView1.Rows(j).Selected = True
                DataGridView1.FirstDisplayedScrollingRowIndex = j
            Else

            End If
        Next

    End Sub
End Class