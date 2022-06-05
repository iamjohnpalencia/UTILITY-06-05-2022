Imports System.Xml
Imports System.Xml.Xsl
Imports PdfSharp
Imports PdfSharp.Drawing
Imports PdfSharp.Pdf

Public Class EJOURNAL_PDF

    Property EjournalPDF_Path As String = ""
    Property Transaction_Path As String = ""
    Property xmlNames As List(Of String)
    Public Sub New(ByVal _ListOfXmlName As List(Of String))

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        xmlNames = _ListOfXmlName
    End Sub
    Private Sub CreateXMLFile(ByVal Position As String, LoopText As String(), ByVal TextFont As String, ByVal writer As XmlTextWriter)
        writer.WriteStartElement("Transaction")
        writer.WriteStartElement("Position")
        writer.WriteString(Position)
        writer.WriteEndElement()

        Dim TxtPos As Integer = 1

        For Each str As String In LoopText
            Select Case TxtPos
                Case 1
                    writer.WriteStartElement("Textleft")
                    writer.WriteString(str)
                    writer.WriteEndElement()
                Case 2
                    writer.WriteStartElement("TextMiddleRight")
                    writer.WriteString(str)
                    writer.WriteEndElement()
                Case 3
                    writer.WriteStartElement("TextMiddleLeft")
                    writer.WriteString(str)
                    writer.WriteEndElement()
                Case 4
                    writer.WriteStartElement("TextRight")
                    writer.WriteString(str)
                    writer.WriteEndElement()
            End Select

            TxtPos += 1
        Next

        writer.WriteStartElement("TextFont")
        writer.WriteString(TextFont)
        writer.WriteEndElement()
        writer.WriteEndElement()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Text = "Loading."
        CheckForIllegalCrossThreadCalls = False
        CreateFolder()
    End Sub

    Private Sub CreateFolder()
        Label1.Text = "Checking folder"
        Dim appdataPath As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
        Dim folderPath As String = System.IO.Path.Combine(appdataPath, "POSV1\EJOURNAL-PDF\")
        EjournalPDF_Path = folderPath
        Transaction_Path = System.IO.Path.Combine(appdataPath, "POSV1\")
        If Not System.IO.Directory.Exists(folderPath) Then
            System.IO.Directory.CreateDirectory(folderPath)
            Label1.Text = "Folder Created"
        End If

        GeneratePDF()
    End Sub


    Private Sub GeneratePDF()
        'Try
        Dim document As PdfDocument = New PdfDocument
        document.Info.Title = "Created with PDFsharp"
        Dim page As PdfPage = document.Pages.Add
        Dim gfx As XGraphics = XGraphics.FromPdfPage(page)
        Dim FontDefault As XFont = New XFont("Tahoma", 5)
        Dim FontBold As XFont = New XFont("Tahoma", 6, XFontStyle.Bold)


        Dim GrandTotalRowCount As Integer = 0
        Try
            Dim xmlDoc As New XmlDocument()
            Dim NodeIndex As Integer = 0

            Dim path = EjournalPDF_Path & "CMB-" & Format(Now, "yyyy-MM-dd-HH-mm-ss") & ".xml"

            Dim writer As New XmlTextWriter(path, System.Text.Encoding.UTF8)
            writer.WriteStartDocument(True)
            writer.Formatting = Formatting.Indented
            writer.Indentation = 2
            writer.WriteStartElement("Table")
            Label1.Text = "Reading XML"
            Dim nodes As XmlNodeList
            For Each str As String In xmlNames
                xmlDoc.Load(Transaction_Path & str)
                nodes = xmlDoc.DocumentElement.SelectNodes("/Table/Transaction")
                For i As Integer = 0 To nodes.Count - 1 Step +1
                    Dim Position As String = "", TextLeft As String = "", TextMiddleRight As String = "", TextMiddleLeft As String = "", TextRight As String = "", TextFont As String = ""
                    Dim NewTextDisplay As String() = {}

                    Position = nodes(i).SelectSingleNode("Position").InnerText
                    Dim CheckTextleft As XmlNode = nodes(i).SelectSingleNode("Textleft")
                    TextLeft = If(CheckTextleft IsNot Nothing, nodes(i).SelectSingleNode("Textleft").InnerText, "N/A")

                    Dim CheckTextMiddleRight As XmlNode = nodes(i).SelectSingleNode("TextMiddleRight")
                    TextMiddleRight = If(CheckTextMiddleRight IsNot Nothing, nodes(i).SelectSingleNode("TextMiddleRight").InnerText, "N/A")

                    Dim CheckMiddleLeft As XmlNode = nodes(i).SelectSingleNode("TextMiddleLeft")
                    TextMiddleLeft = If(CheckMiddleLeft IsNot Nothing, nodes(i).SelectSingleNode("TextMiddleLeft").InnerText, "N/A")

                    Dim CheckTextRight As XmlNode = nodes(i).SelectSingleNode("TextRight")
                    TextRight = If(CheckTextRight IsNot Nothing, nodes(i).SelectSingleNode("TextRight").InnerText, "N/A")

                    TextFont = nodes(i).SelectSingleNode("TextFont").InnerText
                    NewTextDisplay = {TextLeft, TextMiddleRight, TextMiddleLeft, TextRight}
                    CreateXMLFile(Position, NewTextDisplay, TextFont, writer)
                Next
            Next

            writer.WriteEndElement()
            writer.WriteEndDocument()
            writer.Close()

            xmlDoc.Load(path)
            nodes = xmlDoc.DocumentElement.SelectNodes("/Table/Transaction")
            Label1.Text = "Created new XML"
            Dim NextPage As Integer = nodes.Count '100


            Dim PageRows As Integer = 80
            Dim TotalRowsPerPage As Integer = NextPage / PageRows '100 / 80 = 1.25
            TotalRowsPerPage += 1
            'TotalRowsPerPage = 2
            Dim RowFirst As Integer = 1
            Dim GetDgvRowCount As Integer = 0

            Dim GFont As XFont

            For a = 1 To TotalRowsPerPage
                If a <> RowFirst Then
                    page = document.AddPage
                    gfx = XGraphics.FromPdfPage(page)
                    Dim RowCount As Integer = 10
                    Dim CountPage As Integer = 0
                    For i As Integer = GetDgvRowCount To nodes.Count - 1 Step +1
                        If CountPage < PageRows Then
                            Dim NewTextDisplay As String() = {}
                            Dim Position As String = "", TextLeft As String = "", TextMiddleRight As String = "", TextMiddleLeft As String = "", TextRight As String = "", TextFont As String = ""
                            Position = nodes(i).SelectSingleNode("Position").InnerText
                            TextLeft = nodes(i).SelectSingleNode("Textleft").InnerText
                            TextMiddleRight = nodes(i).SelectSingleNode("TextMiddleRight").InnerText
                            TextMiddleLeft = nodes(i).SelectSingleNode("TextMiddleLeft").InnerText
                            TextRight = nodes(i).SelectSingleNode("TextRight").InnerText
                            TextFont = nodes(i).SelectSingleNode("TextFont").InnerText

                            Select Case TextFont
                                Case "BOLD"
                                    GFont = FontBold
                                Case Else
                                    GFont = FontDefault
                            End Select

                            Select Case Position
                                Case "C"
                                    NewTextDisplay = {TextLeft}
                                    PDFCenterText(gfx, NewTextDisplay, GFont, 10, RowCount + NodeIndex)
                                Case "LR"
                                    NewTextDisplay = {TextLeft, TextMiddleRight}
                                    PDFRightToLeft(gfx, NewTextDisplay, GFont, 10, RowCount + NodeIndex)
                                Case "S"
                                    NewTextDisplay = {TextLeft}
                                    PDFSimpleText(gfx, NewTextDisplay, GFont, 10, RowCount + NodeIndex)
                                Case "S4"
                                    NewTextDisplay = {TextLeft, TextMiddleRight, TextMiddleLeft, TextRight}
                                    PDFFourSimpleLine(gfx, NewTextDisplay, GFont, 10, RowCount + NodeIndex)
                                Case "S3"
                                    NewTextDisplay = {TextLeft, TextMiddleRight, TextMiddleLeft}
                                    PDFThreeSimpleLine(gfx, NewTextDisplay, GFont, 10, RowCount + NodeIndex)
                            End Select
                            RowCount += 10
                            CountPage += 1
                            GetDgvRowCount += 1
                            NewTextDisplay = {}
                        Else
                            Exit For
                        End If
                    Next
                    RowFirst += 1
                Else
                    Dim RowCount As Integer = 10
                    Dim CountPage As Integer = 0
                    For i As Integer = 0 To nodes.Count - 1 Step +1
                        If i < PageRows Then
                            Dim NewTextDisplay As String() = {}
                            Dim Position As String = "", TextLeft As String = "", TextMiddleRight As String = "", TextMiddleLeft As String = "", TextRight As String = "", TextFont As String = ""
                            Position = nodes(i).SelectSingleNode("Position").InnerText
                            TextLeft = nodes(i).SelectSingleNode("Textleft").InnerText
                            TextMiddleRight = nodes(i).SelectSingleNode("TextMiddleRight").InnerText
                            TextMiddleLeft = nodes(i).SelectSingleNode("TextMiddleLeft").InnerText
                            TextRight = nodes(i).SelectSingleNode("TextRight").InnerText
                            TextFont = nodes(i).SelectSingleNode("TextFont").InnerText
                            Select Case TextFont
                                Case "BOLD"
                                    GFont = FontBold
                                Case Else
                                    GFont = FontDefault
                            End Select
                            Select Case Position
                                Case "C"
                                    NewTextDisplay = {TextLeft}
                                    PDFCenterText(gfx, NewTextDisplay, GFont, 10, RowCount + NodeIndex)
                                Case "LR"
                                    NewTextDisplay = {TextLeft, TextMiddleRight}
                                    PDFRightToLeft(gfx, NewTextDisplay, GFont, 10, RowCount + NodeIndex)
                                Case "S"
                                    NewTextDisplay = {TextLeft}
                                    PDFSimpleText(gfx, NewTextDisplay, GFont, 10, RowCount + NodeIndex)
                                Case "S4"
                                    NewTextDisplay = {TextLeft, TextMiddleRight, TextMiddleLeft, TextRight}
                                    PDFFourSimpleLine(gfx, NewTextDisplay, GFont, 10, RowCount + NodeIndex)
                                Case "S3"
                                    NewTextDisplay = {TextLeft, TextMiddleRight, TextMiddleLeft}
                                    PDFThreeSimpleLine(gfx, NewTextDisplay, GFont, 10, RowCount + NodeIndex)
                            End Select
                            RowCount += 10
                            CountPage += 1
                            GetDgvRowCount += 1
                        Else
                            Exit For
                        End If
                    Next
                End If
            Next
            Label1.Text = "Complete"
            Dim filename = My.Computer.FileSystem.SpecialDirectories.Desktop & "\EJOURNAL" & Format(Now(), "yyyy-MM-dd-HH-mm-ss") & ".pdf"
            document.Save(filename)
            Process.Start(filename)
            Me.Dispose()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted

    End Sub
End Class
