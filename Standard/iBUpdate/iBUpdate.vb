Imports System.Threading.Thread
Imports System.IO

Public Class iBUpdate

    'parametri da linea di comando
    Dim batch As String = ""
    Dim strBusNetExeDir As String = ""
    'strBusNetExeDir = "C:\work\iB\trunk\Connettore\TEST\BusNet.exe"
    Dim strAggDir As String = ""
    'strAggDir = "C:\Bus\Agg\iBUpdate"
    Dim strBusNetDir As String = ""
    'strBusNetDir = "C:\work\iB\trunk\Connettore\TEST"
    Dim strParametriAvvioBusNet As String = ""
    'strParametriAvvioBusNet = " admin . BUS_SONN Business BNIEIBUS /B c:\programmi\bus\asc\BNIEIBUS.bub SONN"
    Dim strPercorsoFileLog As String = CStr(IIf(Environ$("tmp") <> "", Environ$("tmp"), Environ$("temp"))) & "\" & "iBUpdateLog.txt"

    Public Function AvviaUpdate() As Boolean
        Dim updateEseguito As Boolean = False

        Try
            Dim busNetChiuso As Boolean = False
            Dim strPercorsoFileLogTmp As String = ""

            'Dim temppath As String = CStr(IIf(Environ$("tmp") <> "", Environ$("tmp"), Environ$("temp"))) 'Get temp path
            'ScriviLog(temppath & "\" & "iBUpdateLog.txt", "leggo il command line")

            'leggo da riga di comando
            If My.Application.CommandLineArgs.Count > 0 Then
                batch = My.Application.CommandLineArgs(0).ToString 'se vengo chiamato in modalità batch
                strBusNetExeDir = My.Application.CommandLineArgs(1).ToString 'es. "C:\work\iB\trunk\Connettore\TEST\BusNet.exe"
                'cartella dove si trovano le dll aggiornate
                strAggDir = My.Application.CommandLineArgs(2).ToString 'es. "C:\Bus\Agg\iBUpdate"
                'cartella dove copiare le dll
                strBusNetDir = My.Application.CommandLineArgs(3).ToString ' es. "C:\work\iB\trunk\Connettore\TEST"
                'parametri per avviare busnet in modalità batch
                strParametriAvvioBusNet = My.Application.CommandLineArgs(4).ToString

                strPercorsoFileLogTmp = My.Application.CommandLineArgs(5).ToString 'es. "C:\Bus\Agg"
                If strPercorsoFileLogTmp <> "" Then
                    strPercorsoFileLog = strPercorsoFileLogTmp & "\" & "iBUpdateLog.txt"
                End If
            End If

            Dim cicli As Integer = 1500 '1500 volte e aspetto 2 secondi, quindi aspetto che l'utente esca da Busnet per 50 minuti
            If batch = "False" Then
                ' verifico che l'eseguibile di busnet non è utilizzato
                Do
                    cicli = cicli - 1

                    '  If Not IsProcessRunning(strBusNetExeDir) Then
                    '    busNetChiuso = True
                    '    Exit Do
                    '  Else

                    If IsWriteble(strBusNetDir + "\" + "BNIEIBUS.dll") AndAlso
                       IsWriteble(strBusNetDir + "\" + "BEIEIBUS.dll") AndAlso
                       IsWriteble(strBusNetDir + "\" + "BDIEIBUS.dll") Then
                        busNetChiuso = True
                        Exit Do
                    Else
                        'mi metto in attesa 2 secondi
                        Sleep(2000)
                    End If
                    If (cicli = 0) Then
                        Exit Do
                    End If
                Loop While (cicli > 0)
            Else
                busNetChiuso = True
            End If

            'se busnet è chiuso
            If busNetChiuso Then
                Sleep(1000)
                'copio le dll
                CopyDirectory(strAggDir, strBusNetDir, True)

                ScriviLog(strPercorsoFileLog, "BusNet Aggiornato." & strBusNetExeDir & " Letti i seguenti parametri " & strParametriAvvioBusNet)

                'avvio busnet (*** SI E' DECISO DI NON RIAPRIRLO)
                'Process.Start(strBusNetExeDir, strParametriAvvioBusNet)

                updateEseguito = True

                Sleep(2000)
            End If

            Me.Close()
            Application.Exit()
        Catch ex As Exception
            ScriviLog(strPercorsoFileLog, "Errore Catch: " & ex.Message)
            Me.Close()
            Application.Exit()
        End Try

        Return updateEseguito

    End Function

    Private Function IsProcessRunning(ByVal processPath As String) As Boolean
        ' http://msdn.microsoft.com/en-us/library/windows/desktop/aa394372(v=vs.85).aspx
        On Error Resume Next
        IsProcessRunning = GetObject("winmgmts:").ExecQuery("SELECT * FROM Win32_Process " & _
            "WHERE ExecutablePath = '" & Replace(processPath, "\", "\\") & "'").Count
        Return IsProcessRunning
    End Function

    ' erifica se il file passato è scrivibile (aggiornabile)
    Private Function IsWriteble(pFileName As String) As Boolean
        Dim RetValue As Boolean = True

        Try
            If File.Exists(pFileName) Then
                Dim streamData As FileStream = File.Open(pFileName, FileMode.Open)
                streamData.Close()

            End If
        Catch
            RetValue = False
        End Try

        Return RetValue

    End Function


    Public Sub CopyDirectory(ByVal sourcePath As String, ByVal destPath As String, ByVal overwrite As Boolean)
        Dim sourceDir As System.IO.DirectoryInfo = New System.IO.DirectoryInfo(sourcePath)
        Dim destDir As System.IO.DirectoryInfo = New System.IO.DirectoryInfo(destPath)
        If (sourceDir.Exists) Then
            If Not (destDir.Exists) Then
                destDir.Create()
            End If
            Dim file As System.IO.FileInfo
            For Each file In sourceDir.GetFiles()
                Try
                    If (overwrite) Then
                        file.CopyTo(System.IO.Path.Combine(destDir.FullName, file.Name), True)
                    Else
                        If ((System.IO.File.Exists(System.IO.Path.Combine(destDir.FullName, file.Name))) = False) Then
                            file.CopyTo(System.IO.Path.Combine(destDir.FullName, file.Name), False)
                        End If
                    End If
                Catch ex As Exception
                    ' Non faccio nulla se ho un errore
                End Try
            Next
            Dim dir As System.IO.DirectoryInfo
            For Each dir In sourceDir.GetDirectories()
                CopyDirectory(dir.FullName, System.IO.Path.Combine(destDir.FullName, dir.Name), overwrite)
            Next
        Else
            'Potresti gestire un errore in caso il percorso non esiste
        End If
    End Sub

#Region "ScriviLog"

    Public Sub ScriviLog(ByVal fileLog As String, ByVal logMessage As String)
        Try
            Using w As StreamWriter = File.AppendText(fileLog)
                Log(logMessage, w)

                ' Close the writer and underlying file.
                w.Close()
            End Using
            ' Open and read the file.
            Using r As StreamReader = File.OpenText(fileLog)
                DumpLog(r)
            End Using
        Catch
        End Try
    End Sub

    Public Sub Log(ByVal logMessage As String, ByVal w As TextWriter)
        w.Write(ControlChars.CrLf & "Log Entry : ")
        w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString())
        w.WriteLine("  :")
        w.WriteLine("  :{0}", logMessage)
        w.WriteLine("CommanLine: {0},{1},{2},{3},{4},{5}", batch, strBusNetExeDir, strAggDir, strBusNetDir, strParametriAvvioBusNet, strPercorsoFileLog)
        w.WriteLine("-------------------------------")
        ' Update the underlying file.
        w.Flush()
    End Sub

    Public Sub DumpLog(ByVal r As StreamReader)
        ' While not at the end of the file, read and write lines.
        Dim line As String
        line = r.ReadLine()
        While Not line Is Nothing
            line = r.ReadLine()
        End While
        r.Close()
    End Sub

#End Region

End Class
