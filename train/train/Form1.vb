Public Class Form1



    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub ComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged, ComboBox2.SelectedIndexChanged
        Select Case ComboBox1.Text
            Case "基隆" : TextBox2.Text = "1001"
            Case "三坑" : TextBox2.Text = "1029"
                'Case Else : TextBox2.Text = "ERROR"
        End Select

        Select Case ComboBox2.Text
            Case "基隆" : TextBox3.Text = "1001"
            Case "三坑" : TextBox3.Text = "1029"
                'Case Else : TextBox3.Text = "ERROR"
        End Select
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim Train_url As String = "http://twtraffic.tra.gov.tw/twrail/SearchResult.aspx?"
        'searchtype=0&searchdate=2015/01/12&fromstation=1001&tostation=1029&trainclass=2&fromtime=0000&totime=2359
        WebBrowser1.Navigate(Train_url & _
                             "searchtype=" & TextBox4.Text & _
                             "&searchdate=" & TextBox5.Text & _
                             "&fromstation=" & TextBox2.Text & _
                             "&tostation=" & TextBox3.Text & _
                             "&trainclass=" & TextBox6.Text & _
                             "&fromtime=0000" & TextBox7.Text & _
                             "&totime=" & TextBox8.Text)
    End Sub


    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        'WebBrowser1.Document.GetElementById("FromStationList1").SetAttribute("value", "1001") '起站
        'WebBrowser1.Document.GetElementById("ToStationList1").SetAttribute("value", "1029") '迄站
        '輸入站名
        Dim form As HtmlElement = WebBrowser1.Document.GetElementById("Search")

        If form IsNot Nothing Then
            form.All("FromStationList1").SetAttribute("value", TextBox2.Text)
            form.All("ToStationList1").SetAttribute("value", TextBox3.Text)
            'form.InvokeMember("submit")
        End If

        'For Each ele As HtmlElement In WebBrowser1.Document.All
        '    If ele.GetAttribute("name").ToLower = "TrainClass".ToLower Then
        '        ele.InvokeMember("click")
        '    End If
        'Next
        '選項判斷
        Dim tmp_count As Integer = ComboBox3.Tag

        For Each ele As HtmlElement In WebBrowser1.Document.All
            If ele.GetAttribute("type").ToLower = "radio" Then
                tmp_count += 1
                If tmp_count = 2 Then
                    ele.InvokeMember("click")
                    Exit For
                End If
            End If
        Next

        '輸入日期

        If form IsNot Nothing Then
            form.All("SearchDateSelect").SetAttribute("value", TextBox5.Text)
            'form.InvokeMember("submit")
        End If

        '輸入時間
        If form IsNot Nothing Then
            form.All("FromTimeSelect").SetAttribute("value", TextBox7.Text)
            form.All("ToTimeSelect").SetAttribute("value", TextBox8.Text)
            'form.InvokeMember("submit")
        End If

    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        'WebBrowser1.Document.GetElementById("Search").InvokeMember("submit")

        For Each ele As HtmlElement In WebBrowser1.Document.All
            If ele.GetAttribute("id").ToLower = "Button2".ToLower Then
                ele.InvokeMember("click")
            End If
        Next





        'WebBrowser1.Document.Forms(0).InvokeMember("submit")
        'WebBrowser1.Document.Forms.GetElementsByName("Search").Item(0).InvokeMember("submit")
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        WebBrowser1.Navigate("http://twtraffic.tra.gov.tw/twrail/nonscript.aspx")
        Button3.Enabled = False
        Button4.Enabled = False
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox3.SelectedIndexChanged
        Select Case ComboBox3.Text
            Case "對號列車" : ComboBox3.Tag = "1"
            Case "非對號列車" : ComboBox3.Tag = "0"
            Case "所有車種" : ComboBox3.Tag = "-1"
        End Select
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim form As HtmlElement = WebBrowser1.Document.GetElementById("Search")

        If form IsNot Nothing Then
            form.All("FromStationList1").SetAttribute("value", TextBox2.Text)
            form.All("ToStationList1").SetAttribute("value", TextBox3.Text)
            'form.InvokeMember("submit")
        End If

        Dim tmp_count As Integer = ComboBox3.Tag

        For Each ele As HtmlElement In WebBrowser1.Document.All
            If ele.GetAttribute("type").ToLower = "radio" Then
                tmp_count += 1
                If tmp_count = 2 Then
                    ele.InvokeMember("click")
                    Exit For
                End If
            End If
        Next

        If form IsNot Nothing Then
            form.All("SearchDateSelect").SetAttribute("value", TextBox5.Text)
            'form.InvokeMember("submit")
        End If

        For Each ele As HtmlElement In WebBrowser1.Document.All
            If ele.GetAttribute("id").ToLower = "Button2".ToLower Then
                ele.InvokeMember("click")
            End If
        Next
    End Sub

    'Private Sub WebBrowser1_NewWindow(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles WebBrowser1.NewWindow
    '    e.Cancel = True
    'End Sub

    Private Sub WebBrowser1_Navigated(ByVal sender As Object, ByVal e As System.Windows.Forms.WebBrowserNavigatedEventArgs) Handles WebBrowser1.Navigated
        'If WebBrowser1.Document.Window.Url.ToString <> "http://twtraffic.tra.gov.tw/twrail/nonscript.aspx" Then WebBrowser1.Navigate("http://twtraffic.tra.gov.tw/twrail/nonscript.aspx") : Exit Sub
        Button3.Enabled = True
        Button4.Enabled = True
        TextBox1.Text = WebBrowser1.Document.Window.Url.ToString
    End Sub
End Class