Module Module1
    Public Sub loading() '等待網頁讀取完成
        SearchTrain.Tag = SearchTrain.Text
        ' SearchTrain.Text += " [loading]"
        Do Until SearchTrain.WebBrowser1.ReadyState = WebBrowserReadyState.Complete
            Application.DoEvents() : Threading.Thread.Sleep(200)
        Loop
        If SearchTrain.GroupBox1.Enabled = False Then SearchTrain.GroupBox1.Enabled = True
        If SearchTrain.ComboBox2.Enabled = False Then SearchTrain.ComboBox2.Enabled = True
        'System.Threading.Thread.Sleep(10000)
    End Sub

    Public Function checkdata()

        Dim checkd As HtmlElement = SearchTrain.WebBrowser1.Document().GetElementById("NoResultLabel")

        If checkd IsNot Nothing Then
            Return True
        Else
            Return False
        End If

        'For Each [HtmlElement] As HtmlElement In SearchTrain.WebBrowser1.Document.All
        '    '擷取
        '    If [HtmlElement].GetAttribute("SPAN") Then
        '        MsgBox([HtmlElement].OuterText)
        '        Return False
        '    Else
        '        MsgBox("抓到了")
        '        Return True
        '    End If
        'Next
        Return False
    End Function
End Module
