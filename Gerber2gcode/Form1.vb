Imports System.Drawing
'Imports System.Math

Public Class Form1
    Dim currentImage As Bitmap
    'Dim bmp, bmp2 As System.Drawing.Bitmap
    Dim bmp, bmp2, bmp3 As Bitmap
    Dim c, c1, c2, c3, c4, c5, c6, c7, c8, c9 As Color
    Dim lenom, lenom_i As String
    Dim blackCount As Integer
    Dim addition As Long
    Dim diviseur, multiplicateur As Double
    Dim absolu, garder As Boolean
    Dim plot_mode As Byte
    Dim linetext As String = ""
    Dim dimx, dimy As Double
    'Dim lebitmap As Bitmap
    Dim zimage As Graphics
    Dim aperture_type As String
    Dim aperture_x, aperture_y As Integer
    Dim curx, cury As Double
    Dim coordx, coordy, coordw, coordh As Double
    Dim curx_int, cury_int As Integer
    Dim valeur_int, valeur_int2 As Integer
    Dim toto As Integer = 1
    Dim titi As Integer
    Dim ofsetx, ofsety As Double
    Dim cncdiam, cncstep, cncvmove, cncvrot, cncvdown, cncprof As Double

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        DataGridView1.Columns.Item(0).HeaderText = "ID"
        DataGridView1.Columns.Item(1).HeaderText = "Type"
        DataGridView1.Columns.Item(2).HeaderText = "Width"
        DataGridView1.Columns.Item(3).HeaderText = "Height"
        cncdiam = Val(TextBox3.Text) / 2
        cncstep = Val(TextBox4.Text)
        cncvmove = Val(TextBox5.Text)
        cncvdown = Val(TextBox6.Text)
        cncvrot = Val(TextBox7.Text)
        cncprof = Val(TextBox8.Text)
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        bmp2 = currentImage
        bmp = New Bitmap(bmp2, bmp2.Width, bmp2.Height)
        bmp = bmp2
        ProgressBar1.Value = 0
        ListBox1.Items.Clear()
        ' Convert to black and white
        For x = 0 To bmp.Width - 1
            For y = 0 To bmp.Height - 1
                c = bmp.GetPixel(x, y)
                addition = c.R
                addition = addition + c.G
                addition = addition + c.B
                If (addition > 255) Then
                    bmp.SetPixel(x, y, Color.White)
                Else
                    bmp.SetPixel(x, y, Color.Black)
                End If
            Next y
        Next x
        PictureBox1.Image = bmp

        ' Create an outline of the image
        bmp3 = New Bitmap(bmp2.Width, bmp2.Height)
        ' If the pixel is black, check the surrounding pixels
        ' If any of the surrounding pixels are white, set the pixel to black
        For x = 0 To bmp.Width - 1
            ProgressBar1.Value = ProgressBar1.Value + 1 : If ProgressBar1.Value = 1000 Then ProgressBar1.Value = 1
            For y = 0 To bmp.Height - 1
                c = bmp.GetPixel(x, y)
                If (c.R = 0 And c.G = 0 And c.B = 0) Then
                    ' Check surrounding pixels
                    If (x > 0 And y > 0 And x < bmp.Width - 1 And y < bmp.Height - 1) Then
                        c2 = bmp.GetPixel(x - 1, y)
                        c3 = bmp.GetPixel(x + 1, y)
                        c4 = bmp.GetPixel(x, y - 1)
                        c5 = bmp.GetPixel(x, y + 1)
                        bmp3.SetPixel(x, y, Color.White)
                        If (c2.R = 255 And c2.G = 255 And c2.B = 255) Then bmp3.SetPixel(x, y, Color.Black)
                        If (c3.R = 255 And c3.G = 255 And c3.B = 255) Then bmp3.SetPixel(x, y, Color.Black)
                        If (c4.R = 255 And c4.G = 255 And c4.B = 255) Then bmp3.SetPixel(x, y, Color.Black)
                        If (c5.R = 255 And c5.G = 255 And c5.B = 255) Then bmp3.SetPixel(x, y, Color.Black)
                    Else
                        bmp3.SetPixel(x, y, Color.White)
                    End If
                Else
                    bmp3.SetPixel(x, y, Color.White)
                End If
            Next y
        Next x
        ProgressBar1.Value = 0
        PictureBox1.Image = bmp3
        PictureBox1.Refresh()
        'PictureBox1.Image.Save(lenom + ".new")
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim savx, savy As Integer
        Dim lebitmap As Bitmap

        'Dessiner le bitmap :
        Dim lesx, lesy As Integer
        lesx = Int(dimx) : lesy = Int(dimy)
        'lebitmap = New Bitmap(lesy, lesy, PictureBox1.Image.PixelFormat)
        lebitmap = New Bitmap(lesy, lesy)
        lebitmap.SetResolution(254, 254)
        lebitmap = PictureBox1.Image

        ListBox1.Items.Clear()
        ListBox1.Items.Add("(Project " & lenom_i & ")")
        ListBox1.Items.Add("(Created by GBR2CNC)")
        ListBox1.Items.Add("(Required tool : Endmill " & cncdiam * 2 & " mm)")
        If absolu Then ListBox1.Items.Add("G90") Else If absolu Then ListBox1.Items.Add("G91")
        ListBox1.Items.Add("G91.1")
        ListBox1.Items.Add("M3 S" & cncvrot)
        ListBox1.Items.Add("G00 Z5.0000")
        ListBox1.Items.Add(vbCrLf)

        For y = 0 To lebitmap.Height - 1
            For x = 0 To lebitmap.Width - 1
                c = lebitmap.GetPixel(x, y)
                If (c.R = 0 And c.G = 0 And c.B = 0) Then
                    c2 = lebitmap.GetPixel(x + 1, y)
                    If (c2.R <> 0 Or c2.G <> 0 Or c2.B <> 0) Then
                        Exit For
                    End If
                    lebitmap.SetPixel(x, y, Color.Green) : coordx = x : coordy = y
                    Do
                        x = x + 1 : If x > lebitmap.Width - 1 Then Exit Do
                        c = lebitmap.GetPixel(x, y)
                        If (c.R = 0 And c.G = 0 And c.B = 0) Then lebitmap.SetPixel(x, y, Color.Green)
                    Loop Until ((c.R <> 0 Or c.G <> 0 Or c.B <> 0) Or (x > lebitmap.Width - 2))
                    x = x - 1
                    c2 = lebitmap.GetPixel(x, y + 1)
                    coordw = x - coordx
                    savx = x + 1 : savy = y
                    ' voyons la ligne de Droite :
                    Do
                        y = y + 1 : If y > lebitmap.Height - 1 Then Exit Do
                        c = lebitmap.GetPixel(x, y)
                        If (c.R = 0 And c.G = 0 And c.B = 0) Then lebitmap.SetPixel(x, y, Color.Green)
                    Loop Until ((c.R <> 0 Or c.G <> 0 Or c.B <> 0) Or (y > lebitmap.Height - 2))
                    y = y - 1
                    coordh = y - coordy
                    ' voyons la ligne du bas :
                    Do
                        x = x - 1 : If x = -1 Then Exit Do
                        c = lebitmap.GetPixel(x, y)
                        If (c.R = 0 And c.G = 0 And c.B = 0) Then lebitmap.SetPixel(x, y, Color.Green)
                    Loop Until ((c.R <> 0 Or c.G <> 0 Or c.B <> 0) Or (x < 1))
                    x = x + 1
                    ' voyons la ligne de Gauche :
                    Do
                        y = y - 1 : If y = -1 Then Exit Do
                        c = lebitmap.GetPixel(x, y)
                        If (c.R = 0 And c.G = 0 And c.B = 0) Then lebitmap.SetPixel(x, y, Color.Green)
                    Loop Until ((c.R <> 0 Or c.G <> 0 Or c.B <> 0) Or (y < 1))
                    y = y + 1
                    If (c.R = 0 And c.G = 0 And c.B = 0) Then lebitmap.SetPixel(x, y, Color.Green)
                    ' revenons là ou on etait
                    x = savx : y = savy
                    percer2("R", coordx, coordy, coordw, coordh)
                    PictureBox1.Image = lebitmap : Me.Update()
                End If
            Next x
        Next y
        ListBox1.Items.Add("M05") : ListBox1.Items.Add("M30")
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Dim valeur, valeur2 As Double
        Dim lebitmap As Bitmap
        'Dessiner le bitmap :
        Dim lesx, lesy As Integer
        lesx = Int(dimx) : lesy = Int(dimy)
        lebitmap = New Bitmap(lesy, lesy)
        lebitmap.SetResolution(254, 254)
        For x = 0 To lebitmap.Width - 1
            For y = 0 To lebitmap.Height - 1
                lebitmap.SetPixel(x, y, Color.White)
            Next
        Next
        ListBox4.Items.Clear()
        PictureBox1.Image = lebitmap
        zimage = Graphics.FromImage(lebitmap)
        ListBox4.Items.Clear()
        ListBox4.Items.Add("(Project " & lenom & ")")
        ListBox4.Items.Add("(Created by GBR2CNC)")
        ListBox4.Items.Add("(Required tool : Endmill " & cncdiam * 2 & " mm)")
        If absolu Then ListBox4.Items.Add("G90") Else If absolu Then ListBox4.Items.Add("G91")
        ListBox4.Items.Add("G91.1")
        ListBox4.Items.Add("M3 S" & cncvrot)
        ListBox4.Items.Add("G00 Z5.0000")
        ListBox4.Items.Add(vbCrLf)

        For i = 0 To ListBox3.Items.Count - 1
            linetext = ListBox3.Items.Item(i)
            'ListBox3.SelectedItem = i : ListBox3.TopIndex = ListBox3.SelectedItem
            If linetext(0) = "D" And linetext(1) <> "D" Then
                valeur = Val(Mid(linetext, 2))
                If valeur > 10 Then
                    For Each row As DataGridViewRow In DataGridView1.Rows
                        If row.Cells(0).Value = valeur Then
                            aperture_type = row.Cells(1).Value
                            valeur = row.Cells(2).Value
                            aperture_x = Math.Round(valeur)
                            valeur = row.Cells(3).Value
                            aperture_y = Math.Round(valeur)
                        End If
                    Next
                End If
            End If


            ' D01 : DRAW :

            'G75*               Must be called before an arc is created.
            'G02*               Set clockwise circular plot mode
            'X0Y600I500J0D01*   Create arc object to (0, 6) with center (5,6)

            If Microsoft.VisualBasic.Strings.Right(linetext, 3) = "D01" Or Microsoft.VisualBasic.Strings.Right(linetext, 2) = "D1" Then
                linetext = Replace(linetext, "D1", "") : linetext = Replace(linetext, "D01", "")
                valeur = curx
                If linetext(0) = "X" Then
                    valeur = ((Val(Mid(linetext, 2))) / diviseur * multiplicateur - ofsetx) : valeur_int = Math.Round(valeur)
                    curx_int = Math.Round(curx) : cury_int = Math.Round(cury)

                    If InStr(linetext, "Y") = 0 Then
                        If plot_mode = 1 Then
                            zimage.DrawLine(Pens.Black, curx_int, cury_int, valeur_int, cury_int) ' (System.Drawing.Pen pen, int x1, int y1, int x2, int y2);             
                            curx = valeur                                       
                        ElseIf plot_mode = 2 Then
                            zimage.DrawArc(Pens.Black, curx_int, cury_int, aperture_x, aperture_x, 0, 90) ' X , Y , Width , Height , angle, Angle
                            curx = valeur
                        ElseIf plot_mode = 3 Then

                            curx = valeur
                        End If
                    End If
                End If

                If (InStr(linetext, "Y") And (linetext(0) <> "Y")) Then
                    valeur2 = ((Val(Mid(linetext, InStr(linetext, "Y") + 1))) / diviseur * multiplicateur - ofsety) : valeur_int2 = Math.Round(valeur2)
                    curx_int = Math.Round(curx) : cury_int = Math.Round(cury)
                    If plot_mode = 1 Then
                        zimage.DrawLine(Pens.Black, curx_int, cury_int, valeur_int, valeur_int2)
                        curx = valeur : cury = valeur2
                    ElseIf plot_mode = 2 Then
                        zimage.DrawArc(Pens.Black, curx_int, cury_int, aperture_x, aperture_x, 0, 90)
                        curx = valeur : cury = valeur2
                    ElseIf plot_mode = 3 Then

                        curx = valeur : cury = valeur2
                    End If
                End If

                If linetext(0) = "Y" Then
                    valeur2 = ((Val(Mid(linetext, 2))) / diviseur * multiplicateur - ofsety) : valeur_int2 = Math.Round(valeur2)
                    curx_int = Math.Round(curx) : cury_int = Math.Round(cury)
                    If plot_mode = 1 Then
                        zimage.DrawLine(Pens.Black, curx_int, cury_int, curx_int, valeur_int2)
                        cury = valeur2
                    ElseIf plot_mode = 2 Then
                        zimage.DrawArc(Pens.Black, curx_int, cury_int, aperture_x, aperture_x, 0, 90)
                        cury = valeur2
                    ElseIf plot_mode = 3 Then

                        cury = valeur2
                    End If
                End If

            End If
            ' D02 : MOVE :
            If Microsoft.VisualBasic.Strings.Right(linetext, 3) = "D02" Or Microsoft.VisualBasic.Strings.Right(linetext, 2) = "D2" Then
                linetext = Replace(linetext, "D2", "") : linetext = Replace(linetext, "D02", "")
                If InStr(linetext, "X") Then curx = Val(Mid(linetext, 2)) / diviseur * multiplicateur - ofsetx
                If InStr(linetext, "Y") Then cury = Val(Mid(linetext, InStr(linetext, "Y") + 1)) / diviseur * multiplicateur - ofsety
                'ListBox4.Items.Add("G0 X" & (curx / 10) & " Y" & (cury / 10)) : Me.Update()
            End If
            ' D03 : put APERTURE :
            If Microsoft.VisualBasic.Strings.Right(linetext, 3) = "D03" Or Microsoft.VisualBasic.Strings.Right(linetext, 2) = "D3" Then
                linetext = Replace(linetext, "D3", "") : linetext = Replace(linetext, "D03", "")
                If InStr(linetext, "X") Then
                    curx = Val(Mid(linetext, 2)) / diviseur * multiplicateur - ofsetx
                End If
                If InStr(linetext, "Y") Then
                    cury = Val(Mid(linetext, InStr(linetext, "Y") + 1)) / diviseur * multiplicateur - ofsety
                End If
                curx_int = Math.Round(curx)
                cury_int = Math.Round(cury)
                If aperture_type = "R" Then
                    curx = curx - (aperture_x / 2) : curx_int = Math.Round(curx)
                    cury = cury - (aperture_y / 2) : cury_int = Math.Round(cury)
                    zimage.DrawRectangle(Pens.Black, curx_int, cury_int, aperture_x, aperture_y) '(System.Drawing.Pen pen, int x, int y, int width, int height)
                    percer("R", curx, cury, aperture_x, aperture_y) '( shape,  x,  y,  width,  height)
                    curx = curx + (aperture_x / 2)
                    cury = cury + (aperture_y / 2)
                End If
                If aperture_type = "C" Then
                    curx = curx - (aperture_x / 2) : curx_int = Math.Round(curx)
                    cury = cury - (aperture_y / 2) : cury_int = Math.Round(cury)
                    zimage.DrawEllipse(Pens.Black, curx_int, cury_int, aperture_x, aperture_x)
                    percer("C", curx, cury, aperture_x, aperture_x) '( shape,  x,  y,  width,  height)
                    'N60 G03 X-100.000 Y0.000 I0.000 J0.000
                    'N70 G03 X100.000 Y0 I0.000 J0.000
                    'N75 G03 X0.000 Y100.000 I0.000 J0.000
                    curx = curx + (aperture_x / 2)
                    cury = cury + (aperture_y / 2)
                End If
                If aperture_type = "O" Then
                    curx = curx - (aperture_x / 2) : curx_int = Math.Round(curx)
                    cury = cury - (aperture_y / 2) : cury_int = Math.Round(cury)
                    zimage.DrawEllipse(Pens.Black, curx_int, cury_int, aperture_x, aperture_y)
                    percer("O", curx, cury, aperture_x, aperture_y) '( shape,  x,  y,  width,  height)
                    curx = curx + (aperture_x / 2)
                    cury = cury + (aperture_y / 2)
                End If

            End If
        Next
        ListBox4.Items.Add("M05") : ListBox4.Items.Add("M30")
        PictureBox1.Image = lebitmap
        PictureBox1.Image.RotateFlip(RotateFlipType.Rotate180FlipX)
        PictureBox1.Refresh()
        bmp = lebitmap
    End Sub

    Private Sub traiter_la_ligne()
        If InStr(linetext, "G04") > 0 Then garder = False : GoTo nextline5
        linetext = Replace(linetext, " ", "", , -1)
        linetext = Replace(linetext, "%", "")
        'linetext = Replace(linetext, "D1", "ZD1")
        'linetext = Replace(linetext, "D2", "ZD2")
        'linetext = Replace(linetext, "D3", "ZD3")
        'linetext = Replace(linetext, "D01", "ZD01")
        'linetext = Replace(linetext, "D02", "ZD02")
        'linetext = Replace(linetext, "D03", "ZD03")

        If InStr(linetext, "X") > 0 Or InStr(linetext, "Y") > 0 Then
            If InStr(linetext, "D") = 0 Then linetext = linetext & "ZD01"
        End If

        If Mid(linetext, 1, 2) = "IN" Then garder = False : GoTo nextline5
        If Mid(linetext, 1, 2) = "LN" Then garder = False : GoTo nextline5
        If Mid(linetext, 1, 3) = "G71" Then garder = False : GoTo nextline5
        If Mid(linetext, 1, 3) = "G75" Then garder = False : GoTo nextline5
        If Mid(linetext, 1, 4) = "MOMM" Then garder = False : GoTo nextline5

        '---determine File System----
        If InStr(linetext, "FS") > 0 Then diviseur = (10 ^ (Val(Mid(linetext, InStr(linetext, "X") + 2)))) : garder = False
        If (InStr(linetext, "MOIN") > 0 Or InStr(linetext, "G70") > 0) Then multiplicateur = 254 : garder = False
        If (InStr(linetext, "MOMM") > 0 Or InStr(linetext, "G71") > 0) Then multiplicateur = 10 : garder = False
        If InStr(linetext, "G90") > 0 Then absolu = True : ListBox2.Items.Add("Coordonnées Absolues") : ListBox4.Items.Add("G90") : garder = False : GoTo nextline5
        If InStr(linetext, "G91") > 0 Then absolu = False : ListBox2.Items.Add("Coordonnées Relatives") : ListBox4.Items.Add("G91") : garder = False : GoTo nextline5
        If InStr(linetext, "G01") > 0 Then plot_mode = 1 : ListBox2.Items.Add("plot_mode : Linear") : garder = False : GoTo nextline5
        If InStr(linetext, "G02") > 0 Then plot_mode = 2 : ListBox2.Items.Add("plot_mode : Clockwise Circular") : garder = False : GoTo nextline5
        If InStr(linetext, "G03") > 0 Then plot_mode = 3 : ListBox2.Items.Add("plot_mode : CounterClockwise Circular") : garder = False : GoTo nextline5

        '---get aperture table-------
        Dim I = 3 : Dim numap As String = ""
        Dim ligne(3) As String

        If Mid(linetext, 1, 3) = "ADD" Then
            While (IsNumeric(linetext(I)))
                numap = numap & linetext(I) : I = I + 1
            End While
            ligne(0) = numap
            ligne(1) = linetext(I)
            If ligne(1) = "C" Then
                ligne(2) = Val(Mid(linetext, InStr(linetext, ",") + 1)) / diviseur
                ligne(3) = ligne(2)
            Else ' R ou O
                ligne(2) = Val(Mid(linetext, InStr(linetext, ",") + 1)) / diviseur
                ligne(3) = Val(Mid(linetext, InStr(linetext, "X") + 1)) / diviseur
            End If
            'Now on converti l'aperture en mm
            ligne(2) = ligne(2) * multiplicateur
            ligne(3) = ligne(3) * multiplicateur
            DataGridView1.Rows.Add(ligne)
            garder = False
        End If
nextline5:
        If (garder) Then ListBox3.Items.Add(linetext)
        garder = True
    End Sub

    Private Sub LoadImageFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        SaveFileDialog1.Filter = "Gcode Files (.gcode, .tap)|*.gcode;*.tap"
        SaveFileDialog1.FilterIndex = 1
        SaveFileDialog1.Title = "Save Gcode"
        SaveFileDialog1.FileName = Mid(lenom, 1, InStr(lenom, ".")) & "gcode"
        Dim dr As DialogResult
        dr = SaveFileDialog1.ShowDialog()
        If (dr = System.Windows.Forms.DialogResult.OK) = False Then Return
        lenom = SaveFileDialog1.FileName
        FileOpen(1, lenom, OpenMode.Output)
        For i = 0 To ListBox1.Items.Count - 1
            linetext = Replace(ListBox1.Items.Item(i), ",", ".")
            Print(1, linetext & vbCrLf)
        Next
        FileClose(1)
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        OpenFileDialog1.Filter = "Gerber Files (.gbr)|*.gbr;"
        OpenFileDialog1.FilterIndex = 1
        OpenFileDialog1.Multiselect = False
        OpenFileDialog1.Title = "Select a Gerber File"
        Dim dr As DialogResult
        dr = OpenFileDialog1.ShowDialog()
        If (dr = System.Windows.Forms.DialogResult.OK) = False Then Return
        Me.Text = "Gbr2CNC " + OpenFileDialog1.FileName
        lenom = OpenFileDialog1.FileName

        Dim valeur, maxix, maxiy, minix, miniy As Double
        absolu = True : garder = True : diviseur = 1 : multiplicateur = 1
        maxix = 0 : maxiy = 0 : minix = 0 : miniy = 0
        ListBox4.Items.Clear() : ListBox3.Items.Clear()
        Using MyReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(lenom)
            MyReader.TextFieldType = FileIO.FieldType.Delimited
            MyReader.SetDelimiters("*")
            Dim currentRow As String()
            While Not MyReader.EndOfData
                Try
                    currentRow = MyReader.ReadFields()
                    For Each Me.linetext In currentRow
                        If (linetext = " " Or linetext = "%" Or linetext = "") Then Exit For
                        traiter_la_ligne()
                    Next
                Catch ex As Microsoft.VisualBasic.
                            FileIO.MalformedLineException
                    MsgBox("Line " & ex.Message &
                    "is not valid and will be skipped.")
                End Try
            End While
        End Using

        ' cherchons les dimensions du circuit imprimé (grace à BoardOUtline.gbr)
        For i = 0 To ListBox3.Items.Count - 1
            linetext = Trim(ListBox3.Items.Item(i))
            If Microsoft.VisualBasic.Strings.Right(linetext, 3) = "D02" Then linetext = Mid(linetext, 1, Len(linetext) - 3)
            If Microsoft.VisualBasic.Strings.Right(linetext, 2) = "D2" Then linetext = Mid(linetext, 1, Len(linetext) - 2)
            If Microsoft.VisualBasic.Strings.Right(linetext, 3) = "D01" Then linetext = Mid(linetext, 1, Len(linetext) - 3)
            If Microsoft.VisualBasic.Strings.Right(linetext, 2) = "D1" Then linetext = Mid(linetext, 1, Len(linetext) - 2)
            If linetext(0) = "X" Then
                valeur = Val(Mid(linetext, 2))
                valeur = valeur / diviseur
                If valeur > maxix Then maxix = valeur
            End If
        Next
        minix = maxix
        For i = 0 To ListBox3.Items.Count - 1
            linetext = ListBox3.Items.Item(i)
            linetext = Replace(linetext, "D1", "")
            If linetext(0) = "X" Then
                valeur = Val(Mid(linetext, 2)) / diviseur
                If valeur < minix Then minix = valeur
            End If
        Next
        For i = 0 To ListBox3.Items.Count - 1
            linetext = ListBox3.Items.Item(i)
            linetext = Replace(linetext, "D1", "")
            If linetext(0) = "Y" Then
                valeur = Val(Mid(linetext, 2)) / diviseur
                If valeur > maxiy Then maxiy = valeur
            End If
        Next
        miniy = maxiy
        For i = 0 To ListBox3.Items.Count - 1
            linetext = ListBox3.Items.Item(i)
            linetext = Replace(linetext, "D1", "")
            If linetext(0) = "Y" Then
                valeur = Val(Mid(linetext, 2)) / diviseur
                If valeur < miniy Then miniy = valeur
            End If
        Next
        dimx = (maxix - minix) * multiplicateur
        dimy = (maxiy - miniy) * multiplicateur
        ListBox2.Items.Add("Dimensions : X=" & (dimx / 10) & " Y=" & (dimy / 10))
        TextBox1.Text = minix * multiplicateur : TextBox2.Text = miniy * multiplicateur
        ofsetx = minix * multiplicateur : ofsety = miniy * multiplicateur
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        OpenFileDialog1.Filter = "Gerber Files (.gbr)|*.gbr;"
        OpenFileDialog1.FilterIndex = 1
        OpenFileDialog1.Multiselect = False
        OpenFileDialog1.Title = "Select a Gerber File"
        Dim dr As DialogResult
        dr = OpenFileDialog1.ShowDialog()
        If (dr = System.Windows.Forms.DialogResult.OK) = False Then Return
        Me.Text = "Gbr2CNC " + OpenFileDialog1.FileName
        lenom = OpenFileDialog1.FileName
        absolu = True : garder = True : diviseur = 1 : multiplicateur = 1
        ListBox3.Items.Clear()

        Using MyReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(lenom)
            MyReader.TextFieldType = FileIO.FieldType.Delimited
            MyReader.SetDelimiters("*")
            Dim currentRow As String()
            While Not MyReader.EndOfData
                Try
                    currentRow = MyReader.ReadFields()
                    For Each Me.linetext In currentRow
                        If (linetext = " " Or linetext = "%" Or linetext = "") Then Exit For
                        traiter_la_ligne()
                    Next
                Catch ex As Microsoft.VisualBasic.
                            FileIO.MalformedLineException
                    MsgBox("Line " & ex.Message &
                    "is not valid and will be skipped.")
                End Try
            End While
        End Using

    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        'toto = PictureBox1.Image.RotateFlip(
        toto = toto + 1 : If toto = 16 Then toto = 0
        PictureBox1.Image.RotateFlip(toto)
        PictureBox1.Refresh()
        Me.Update()
        If toto = 8 Then toto = 1
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        ofsetx = Val(TextBox1.Text)
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        ofsety = Val(TextBox2.Text)
    End Sub

    Private Sub FichierToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        OpenFileDialog1.Filter = "Image Files (.bmp, .jpg, .png)|*.bmp;*.jpg;*.png;"
        OpenFileDialog1.FilterIndex = 1
        OpenFileDialog1.Multiselect = False
        OpenFileDialog1.Title = "Select an Image File"
        Dim dr As DialogResult
        dr = OpenFileDialog1.ShowDialog()
        If (dr = System.Windows.Forms.DialogResult.OK) = False Then Return
        Me.Text = "Gbr2CNC " + OpenFileDialog1.FileName
        lenom_i = OpenFileDialog1.FileName
        PictureBox1.Image = Image.FromFile(OpenFileDialog1.FileName)
        currentImage = New Bitmap(OpenFileDialog1.FileName)
        dimx = 99.06 : dimy = 99.06
        ProgressBar1.Value = 0
        ListBox1.Items.Clear()
    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        SaveFileDialog1.Filter = "Gcode Files (.gcode, .tap)|*.gcode;*.tap"
        SaveFileDialog1.FilterIndex = 1
        SaveFileDialog1.Title = "Save Gcode"
        SaveFileDialog1.FileName = Mid(lenom, 1, InStr(lenom, ".")) & "tap"
        Dim dr As DialogResult
        dr = SaveFileDialog1.ShowDialog()
        If (dr = System.Windows.Forms.DialogResult.OK) = False Then Return
        lenom = SaveFileDialog1.FileName
        FileOpen(1, lenom, OpenMode.Output)
        For i = 0 To ListBox4.Items.Count - 1
            linetext = Replace(ListBox4.Items.Item(i), ",", ".")
            Print(1, linetext & vbCrLf)
        Next
        FileClose(1)
    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        OpenFileDialog1.Filter = "Image Files (.bmp, .jpg, .png)|*.bmp;*.jpg;*.png;"
        SaveFileDialog1.FilterIndex = 1
        SaveFileDialog1.Title = "Save Image"
        SaveFileDialog1.FileName = Mid(lenom, 1, InStr(lenom, ".")) & "bmp"
        Dim dr As DialogResult
        dr = SaveFileDialog1.ShowDialog()
        If (dr = System.Windows.Forms.DialogResult.OK) Then
            lenom = SaveFileDialog1.FileName
            If PictureBox1.Image IsNot Nothing Then PictureBox1.Image.Save(lenom)
        End If
    End Sub

    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
        titi = PictureBox1.SizeMode
        titi = titi + 1 : If titi = 5 Then titi = 0
        PictureBox1.SizeMode = titi
    End Sub

    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox2.Click

    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged
        cncdiam = Val(TextBox3.Text) / 2
    End Sub

    Private Sub TextBox4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox4.TextChanged
        cncstep = Val(TextBox4.Text)
    End Sub

    Private Sub TextBox5_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox5.TextChanged
        cncvmove = Val(TextBox5.Text)
    End Sub

    Private Sub TextBox6_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox6.TextChanged
        cncvdown = Val(TextBox6.Text)
    End Sub

    Private Sub TextBox7_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox7.TextChanged
        cncvrot = Val(TextBox7.Text)
    End Sub

    Private Sub TextBox8_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox8.TextChanged
        cncprof = Val(TextBox8.Text)
    End Sub

    Private Sub percer(ByVal shape As Char, ByVal x As Double, ByVal y As Double, ByVal width As Double, ByVal height As Double)
        Dim passe As Integer
        x = x / 10 : y = y / 10 : width = width / 10 : height = height / 10
        Select Case shape
            Case "R" ' Rectangle
                ListBox4.Items.Add("(Rectangle :)")
                ListBox4.Items.Add("G00 X" & Format((x + cncdiam), "###0.0000") & " Y" & Format((y + cncdiam), "###0.0000"))
                ListBox4.Items.Add("G00 Z0.5")
                passe = 1
                For i = cncstep To cncprof Step cncstep
                    ListBox4.Items.Add("(passe " & passe & ")") : passe = passe + 1
                    ListBox4.Items.Add("G01 Z-" & i & " F" & cncvdown & " S" & cncvrot)
                    ListBox4.Items.Add("G01 X" & Format((x + width - cncdiam), "###0.0000") & " F" & cncvmove)
                    lenom = "G01 X" & Format((x + width - cncdiam), "###0.0000") & " F" & cncvmove
                    ListBox4.Items.Add("G01 Y" & Format((y + height - cncdiam), "###0.0000"))
                    ListBox4.Items.Add("G01 X" & Format((x + cncdiam), "###0.0000"))
                    ListBox4.Items.Add("G01 Y" & Format((y + cncdiam), "###0.0000"))
                Next
                ListBox4.Items.Add("G00 Z5")
                ListBox4.Items.Add(vbCrLf)

            Case "C" ' Circle
                ListBox4.Items.Add("(Circle :)")
                ListBox4.Items.Add("G00 X" & Format((x + cncdiam), "###0.0000") & " Y" & Format(y, "###0.0000"))
                ListBox4.Items.Add("G00 Z0.5")
                passe = 1
                For i = cncstep To cncprof Step cncstep
                    ListBox4.Items.Add("(passe " & passe & ")") : passe = passe + 1
                    ListBox4.Items.Add("G01 Z-" & i & " F" & cncvdown & " S" & cncvrot)
                    ListBox4.Items.Add("G02 X" & Format((x + cncdiam), "###0.0000") & " Y" & Format(y, "###0.0000") & " I" & Format(((width / 2)), "###0.0000") & " F" & cncvdown)
                Next
                ListBox4.Items.Add("G00 Z5")
                ListBox4.Items.Add(vbCrLf)

            Case "O" ' Oblong
                ListBox4.Items.Add("(Ovale :)")
                ListBox4.Items.Add("(Les Ovales ne sont pas implementees dans Gbr2Stencil)")
                ' Bof les Ovales ... ca ne sert à rien :-)
        End Select
    End Sub

    Private Sub percer2(ByVal shape As Char, ByVal x As Double, ByVal y As Double, ByVal width As Double, ByVal height As Double)
        Dim passe As Integer
        x = x / 10 : y = y / 10 : width = width / 10 : height = height / 10
        Select Case shape
            Case "R" ' Rectangle
                ListBox1.Items.Add("(Rectangle :)")
                ListBox1.Items.Add("G00 X" & Format((x + cncdiam), "###0.00") & " Y" & Format((y + cncdiam), "###0.00"))
                ListBox1.Items.Add("G00 Z0.5")
                passe = 1
                For i = cncstep To cncprof Step cncstep
                    ListBox1.Items.Add("(passe " & passe & ")") : passe = passe + 1
                    ListBox1.Items.Add("G01 Z-" & i & " F" & cncvdown & " S" & cncvrot)
                    ListBox1.Items.Add("G01 X" & Format((x + width - cncdiam), "###0.00") & " F" & cncvmove)
                    ListBox1.Items.Add("G01 Y" & Format((y + height - cncdiam), "###0.00"))
                    ListBox1.Items.Add("G01 X" & Format((x + cncdiam), "###0.00"))
                    ListBox1.Items.Add("G01 Y" & Format((y + cncdiam), "###0.00"))
                Next
                ListBox1.Items.Add("G00 Z5")
                ListBox1.Items.Add(vbCrLf)

            Case "C" ' Circle
                ListBox1.Items.Add("(Circle :)")
                ListBox1.Items.Add("G00 X" & Format((x + cncdiam), "###0.00") & " Y" & Format(y, "###0.00"))
                ListBox1.Items.Add("G00 Z0.5")
                passe = 1
                For i = cncstep To cncprof Step cncstep
                    ListBox1.Items.Add("(passe " & passe & ")") : passe = passe + 1
                    ListBox1.Items.Add("G01 Z-" & i & " F" & cncvdown & " S" & cncvrot)
                    ListBox1.Items.Add("G02 X" & Format((x + cncdiam), "###0.0000") & " Y" & Format(y, "###0.0000") & " I" & Format(((width / 2)), "###0.0000") & " F" & cncvdown)
                Next
                ListBox1.Items.Add("G00 Z5")
                ListBox1.Items.Add(vbCrLf)

            Case "O" ' Oblong
                ListBox1.Items.Add("(Ovale :)")
                ListBox1.Items.Add("(Les Ovales ne sont pas implementees dans Gbr2Stencil)")
                ' Bof les Ovales ... ca ne sert à rien :-)
        End Select
    End Sub

    Private Sub PictureBox7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox7.Click
        MsgBox("GBR2CNC V1.00   P.Gourdon Avril 2023", MsgBoxStyle.Information, "GBR2CNC")
    End Sub
End Class
