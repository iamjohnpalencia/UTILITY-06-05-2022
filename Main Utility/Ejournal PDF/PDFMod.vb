Imports PdfSharp.Drawing

Module PDFMod
    Public Sub PDFCenterText(gfx As XGraphics, LoopText As String(), textFont As XFont, X As Integer, Y As Integer)
        Dim Format As XStringFormat = New XStringFormat()
        Dim rect As XRect = New XRect(X, Y, 150, 5)
        Dim Brush As XBrush = XBrushes.Black
        Format.Alignment = XStringAlignment.Center
        For Each str As String In LoopText
            gfx.DrawString(str, textFont, Brush, rect, Format)
        Next
    End Sub

    Public Sub PDFRightToLeft(gfx As XGraphics, LoopText As String(), textFont As XFont, X As Integer, Y As Integer)
        Dim Format As XStringFormat = New XStringFormat()
        Dim rect As XRect = New XRect(X, Y, 150, 5)
        Dim Brush As XBrush = XBrushes.Black
        Dim TxtPos As Integer = 1

        For Each str As String In LoopText
            Select Case TxtPos
                Case 1
                    Format.LineAlignment = XLineAlignment.Near
                    Format.Alignment = XStringAlignment.Near
                    gfx.DrawString(str, textFont, Brush, rect, Format)
                Case 2
                    Format.Alignment = XStringAlignment.Far
                    Format.LineAlignment = XLineAlignment.Far
                    gfx.DrawString(str, textFont, Brush, rect, Format)
            End Select
            TxtPos += 1
        Next
        TxtPos = 0

    End Sub

    Public Sub PDFThreeSimpleLine(gfx As XGraphics, loopText As String(), textFont As XFont, X As Integer, Y As Integer)
        Dim Format As XStringFormat = New XStringFormat()
        Dim rect As XRect = New XRect(X, Y, 150, 5)
        Dim Brush As XBrush = XBrushes.Black
        Dim TxtPosition As Integer = 1
        For Each Txt As String In loopText
            Select Case TxtPosition
                Case 1
                    Format.LineAlignment = XLineAlignment.Near
                    Format.Alignment = XStringAlignment.Near
                    gfx.DrawString(Txt, textFont, Brush, rect, Format)
                Case 2
                    Format.Alignment = XStringAlignment.Center
                    Format.LineAlignment = XLineAlignment.Center
                    gfx.DrawString(Txt, textFont, Brush, rect, Format)
                Case 3
                    Format.Alignment = XStringAlignment.Far
                    Format.LineAlignment = XLineAlignment.Far
                    gfx.DrawString(Txt, textFont, Brush, rect, Format)
            End Select
            TxtPosition += 1
        Next
    End Sub

    Public Sub PDFFourSimpleLine(gfx As XGraphics, loopText As String(), textFont As XFont, X As Integer, Y As Integer)
        Dim Format As XStringFormat = New XStringFormat()
        Dim LeftRect As XRect = New XRect(X, Y, 50, 5)
        Dim RightMiddle As XRect = New XRect(50, Y, 50, 5)
        Dim RightMiddleTwo As XRect = New XRect(60, Y, 50, 5)
        Dim RightRect As XRect = New XRect(110, Y, 50, 5)
        Dim Brush As XBrush = XBrushes.Black

        Dim TxtPos As Integer = 1
        For Each str As String In loopText
            Select Case TxtPos
                Case 1
                    Format.LineAlignment = XLineAlignment.Near
                    Format.Alignment = XStringAlignment.Near
                    gfx.DrawString(str, textFont, Brush, LeftRect, Format)
                Case 2
                    Format.Alignment = XStringAlignment.Near
                    Format.LineAlignment = XLineAlignment.Near
                    gfx.DrawString(str, textFont, Brush, RightMiddle, Format)
                Case 3
                    Format.Alignment = XStringAlignment.Far
                    Format.LineAlignment = XLineAlignment.Near
                    gfx.DrawString(str, textFont, Brush, RightMiddleTwo, Format)
                Case 4
                    Format.Alignment = XStringAlignment.Far
                    Format.LineAlignment = XLineAlignment.Near
                    gfx.DrawString(str, textFont, Brush, RightRect, Format)
            End Select
            TxtPos += 1
        Next

    End Sub

    Public Sub PDFSimpleText(gfx As XGraphics, LoopText As String(), textFont As XFont, X As Integer, Y As Integer)
        Dim Format As XStringFormat = New XStringFormat()
        Dim rect As XRect = New XRect(X, Y, 150, 5)
        Dim Brush As XBrush = XBrushes.Black
        Format.LineAlignment = XLineAlignment.Center
        Format.Alignment = XStringAlignment.Near

        For Each str As String In LoopText
            gfx.DrawString(str, textFont, Brush, rect, Format)
        Next

    End Sub
End Module
