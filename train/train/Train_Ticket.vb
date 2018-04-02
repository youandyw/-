Imports System.Threading
Public Class Train_Ticket
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim ticketlink As String = "http://railway.hinet.net/ctno1.htm?from_station=" & Mid(TextBox2.Text, 1, 3) & "&to_station=" & Mid(TextBox3.Text, 1, 3) &
                            "&getin_date=" & TextBox4.Text & "&train_no=" & TextBox5.Text
        WebBrowser1.Navigate(ticketlink)



    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        '執行查詢
        'For Each ele As HtmlElement In WebBrowser1.Document.All
        '    If ele.GetAttribute("name").ToLower = "returnTicket".ToLower Then ele.InvokeMember("click")
        '    'WebBrowser1.Document.All("submit").InvokeMember("click")
        'Next
        'WebBrowser1.Document.GetElementById("ctno1").InvokeMember("onsubmit")
        'WebBrowser1.Document.GetElementById("ctno1").InvokeMember("click")
        'For Each html As HtmlElement In WebBrowser1.Document.All
        '    If html.InnerHtml = "0" Then
        '        html.InvokeMember("Click")
        '        Exit For
        '    End If
        'Next
        For Each element As HtmlElement In WebBrowser1.Document.GetElementsByTagName("button")
            'element.GetAttribute("name") = "returnTicket
            If element.GetAttribute("type") = "submit" Then
                element.InvokeMember("click")
            Else
                MsgBox("no")
            End If
        Next
        'For Each ele As HtmlElement In WebBrowser1.Document.All '.GetElementsByName("name")
        '    'If ele.GetElementsByTagName("input")  Then ele.InvokeMember("click") Else MsgBox("no")

        '    If ele.GetAttribute("name") = "returnTicket" Then ele.InvokeMember("click") Else MsgBox("no-1")
        '    If ele.GetAttribute("value") = "0" Then ele.InvokeMember("click") Else MsgBox("no-2")
        'Next


        'WebBrowser1.Document.Forms("POST").GetElementsByTagName("input").GetElementsByName("returnTicket").Cast(Of HtmlElement).First().InvokeMember("click")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim form As HtmlElement = WebBrowser1.Document.GetElementById("boxNorm")

        form.All("order_qty_str").SetAttribute("value", ComboBox1.Text)



    End Sub

    Private Sub Train_Ticket_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim ticketlink As String = "http://railway.hinet.net/ctno1.htm?from_station=" & Mid(TextBox2.Text, 1, 3) & "&to_station=" & Mid(TextBox3.Text, 1, 3) &
                           "&getin_date=" & TextBox4.Text & "&train_no=" & TextBox5.Text
        WebBrowser1.Navigate(ticketlink)
        'TextBox2.Text = SearchTrain.fromstation_Ticket.Text & "-" & SearchTrain.fromstation_Cmb.Text '訂票起站
        'TextBox3.Text = SearchTrain.tostation_Ticket.Text & "-" & SearchTrain.tostation_Cmb.Text '訂票訖站
        'TextBox4.Text = SearchTrain.DateTimePicker1.Text '訂票日期
        'TextBox5.Text = SearchTrain.DataGridView1(1, 2).Value.ToString() '訂票車號
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        'Dim form As HtmlElement = WebBrowser1.Document.GetElementById("boxNorm")

        WebBrowser1.Document.All("person_id").SetAttribute("value", TextBox1.Text)
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim form As HtmlElement = WebBrowser1.Document.GetElementById("boxSpec")

        form.All("n_order_qty_str").SetAttribute("value", ComboBox2.Text)
        form.All("z_order_qty_str").SetAttribute("value", ComboBox3.Text)
    End Sub
    'Private Declare Function URLDownloadToFile Lib "urlmon" Alias "URLDownloadToFileA" (ByVal pCaller As Long, ByVal szURL As String, ByVal szFileName As String, ByVal dwReserved As Long, ByVal lpfnCB As Long) As Long
    'Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
    '    If DownloadFile("http://railway.hinet.net/ImageOut.jsp?pageRandom", "C:\Users\youan\Desktop\train(2016-05-22)\train\code.jpg") Then
    '        MsgBox("ok!")
    '    End If

    'End Sub

    'Public Function DownloadFile(URL As String, LocalFilename As String) As Boolean
    '    Dim lngRetVal As Long
    '    lngRetVal = URLDownloadToFile(0, URL, LocalFilename, 0, 0)
    '    If lngRetVal = 0 Then DownloadFile = True
    'End Function

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim rang = WebBrowser1.Document.DomDocument.Body.createControlRange()
        Dim Img = WebBrowser1.Document.Images(1).DomElement

        rang.add(Img)
        rang.execCommand("Copy")
        Dim RegImg As Image = Clipboard.GetImage()
        Clipboard.Clear()
        Me.BackgroundImageLayout = ImageLayout.None
        Me.BackgroundImage = RegImg
        Me.BackgroundImage.Save("C:\Users\youan\Desktop\train(2016-05-22)\train\test_save.bmp")
        Me.BackgroundImage = Nothing
        'PictureBox1.BackgroundImageLayout = ImageLayout.None
        'PictureBox1.Image = RegImg
        'PictureBox1.Image = Image.FromFile("C:\Users\youan\Desktop\train(2016-05-22)\train\test_save.bmp")
        PictureBox1.ImageLocation = ("C:\Users\youan\Desktop\train(2016-05-22)\train\test_save.bmp")



    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        WebBrowser1.Document.All("person_id").SetAttribute("value", TextBox1.Text) '身分證資料

        If GroupBox2.Enabled = True Then '一般訂票
            Dim form As HtmlElement = WebBrowser1.Document.GetElementById("boxNorm")
            form.All("order_qty_str").SetAttribute("value", ComboBox1.Text)
        ElseIf GroupBox3.Enabled = True Then '普悠瑪訂票
            Dim form As HtmlElement = WebBrowser1.Document.GetElementById("boxSpec")

            form.All("n_order_qty_str").SetAttribute("value", ComboBox2.Text)
            form.All("z_order_qty_str").SetAttribute("value", ComboBox3.Text)
        Else
            MsgBox("訂票失敗!")
            Exit Sub
        End If

        WebBrowser1.Document.Forms(0).InvokeMember("submit")
        'For Each element As HtmlElement In WebBrowser1.Document.GetElementsByTagName("button")
        '    'element.GetAttribute("name") = "returnTicket
        '    If element.GetAttribute("type") = "submit" Then
        '        element.InvokeMember("click")
        '    Else
        '        MsgBox("no")
        '    End If
        'Next

    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click

        For Each element As HtmlElement In WebBrowser1.Document.GetElementsByTagName("button")
            If element.GetAttribute("type") = "button" And element.InnerHtml = "重新產生驗證碼" Then
                element.InvokeMember("click")
                Exit For
            End If
        Next
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click

        'For Each ele As HtmlElement In WebBrowser1.Document.All
        '    If ele.GetAttribute("id").ToLower = "sbutton".ToLower Then ele.InvokeMember("click")
        'Next
        WebBrowser1.Document.Forms(0).InvokeMember("submit")
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        GroupBox2.Enabled = True
        GroupBox3.Enabled = True
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        For Each element As HtmlElement In WebBrowser1.Document.GetElementsByTagName("button")
            If element.GetAttribute("type") = "button" And element.InnerHtml = "語音播放" Then
                element.InvokeMember("click")
                Exit For
            End If
        Next
    End Sub

    'Private Sub PictureBox2_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles PictureBox2.Paint
    '    Dim attr As New Imaging.ImageAttributes
    '    '--- 將PictureBox1圖映照至PictureBox2當背景圖
    '    Dim bm1 As New Bitmap(PictureBox1.Image, PictureBox1.Width, PictureBox1.Height)
    '    Dim dstRect As New Rectangle(0, 0, PictureBox2.Width, PictureBox2.Height)
    '    e.Graphics.DrawImage(bm1, dstRect, PictureBox2.Location.X - PictureBox1.Location.X, PictureBox2.Location.Y - PictureBox1.Location.Y, PictureBox2.Size.Width, PictureBox2.Size.Height, GraphicsUnit.Pixel)
    '    '--- 載入PictureBox2圖
    '    Dim bm As New Bitmap(PictureBox2.Image)
    '    '--- 將PictureBox2圖中白色部份將透明
    '    attr.SetColorKey(Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255)) '--白色
    '    e.Graphics.DrawImage(bm, dstRect, 0, 0, bm.Width, bm.Height, GraphicsUnit.Pixel, attr)
    'End Sub
End Class