Imports System.IO
Imports System.Threading
Imports MySql.Data.MySqlClient

Public Class DBBackup
    Dim threadList As List(Of Thread) = New List(Of Thread)
    Dim thread As Thread
    Dim StoreID As String = ""


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckForIllegalCrossThreadCalls = False
        BackgroundWorker1.WorkerReportsProgress = True
        BackgroundWorker1.WorkerSupportsCancellation = True
        BackgroundWorker1.RunWorkerAsync()
    End Sub
    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            For i = 0 To 100
                BackgroundWorker1.ReportProgress(i)
                Thread.Sleep(10)
                If i = 0 Then
                    thread = New Thread(AddressOf LoadLocalConnection)
                    thread.Start()
                    threadList.Add(thread)
                    For Each t In threadList
                        t.Join()
                    Next
                End If

                If i = 20 Then
                    thread = New Thread(AddressOf GetStoreID)
                    thread.Start()
                    threadList.Add(thread)
                    For Each t In threadList
                        t.Join()
                    Next
                End If

                If i = 30 Then
                    thread = New Thread(AddressOf AutoBackupDB)
                    thread.Start()
                    threadList.Add(thread)
                    For Each t In threadList
                        t.Join()
                    Next
                End If
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Me.Dispose()
    End Sub
    Private Sub AutoBackupDB()
        Try
            Dim DatabaseName = RootDB.ToUpper & "-FBW" & StoreID & Format(Now(), "yyyy-MM-dd-HHmmdd") & ".sql"
            Dim LocalExportPathOne = LocalExportPath & DatabaseName
            Dim CloudExportPathOne = CloudExportPath & DatabaseName

            If My.Computer.FileSystem.DirectoryExists(LocalExportPath) Then
                Using myprocess = Process.Start("cmd.exe", "/k cd C:\xampp\mysql\bin & mysqldump -u " & RootID & " " & RootDB & " > " & LocalExportPathOne)
                    Dim i As Integer
                    For i = 0 To 1
                        If Not myprocess.HasExited Then
                            ' Discard cached information about the process.
                            myprocess.Refresh()
                            ' Print working set to console.
                            Console.WriteLine($"Physical Memory Usage: {myprocess.WorkingSet}")
                            '' Wait 2 seconds.
                            Thread.Sleep(1000)
                        Else
                            Exit For
                        End If
                    Next i
                    ' Close process by sending a close message to its main window.
                    myprocess.CloseMainWindow()
                    ' Free resources associated with process.
                    myprocess.Close()
                End Using

            End If

            If My.Computer.FileSystem.DirectoryExists(CloudExportPath) Then
                Using myprocess = Process.Start("cmd.exe", "/k cd C:\xampp\mysql\bin & mysqldump -u " & RootID & " " & RootDB & " > " & CloudExportPathOne)
                    Dim i As Integer
                    For i = 0 To 1
                        If Not myprocess.HasExited Then
                            ' Discard cached information about the process.
                            myprocess.Refresh()
                            ' Print working set to console.
                            Console.WriteLine($"Physical Memory Usage: {myprocess.WorkingSet}")
                            '' Wait 2 seconds.
                            Thread.Sleep(1000)
                        Else
                            Exit For
                        End If
                    Next i
                    ' Close process by sending a close message to its main window.
                    myprocess.CloseMainWindow()
                    ' Free resources associated with process.
                    myprocess.Close()
                End Using
            End If


        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub GetStoreID()
        Try
            Dim ConnectionLocal As MySqlConnection = LocalhostConn()
            Dim Query As String = "SELECT client_store_id FROM admin_masterlist LIMIT 1"
            Dim Command As MySqlCommand = New MySqlCommand(Query, ConnectionLocal)
            Dim Result As Integer = Command.ExecuteScalar
            StoreID = Result.ToString
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
End Class
