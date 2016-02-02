Imports System.Threading.Thread
Imports System.IO

Module Module1

    ' Serveono i seguenti parametri:
    '   1. La cartella in cui scaricare gli aggiornamenti - strAggDir - (esempio C:\Bus\Agg\iBUpdate)
    '   2. La cartella in cui mettere i files - strBusNetDir (esempio: "C:\work\iB\trunk\Connettore\TEST\")

    Dim par_strAggDir As String = "" ' 'strAggDir = "C:\Bus\Agg"
    Dim par_strBusNetDir As String = "" ' 'strBusNetDir = "C:\work\iB\trunk\Connettore\TEST"

    Public Sub Main(ByVal sArgs() As String)
        Dim busNetChiuso As Boolean = False

        If sArgs.Length <> 2 Then
            Console.WriteLine("IBAutoUpdate - APEX-net srl (www.apexnet.it)")
            Console.WriteLine("- Errore: Numero di argomenti errato")
            Environment.Exit(0)
        End If

        Try
            par_strAggDir = sArgs(0).ToString 'es. "C:\Bus\Agg"
            par_strBusNetDir = sArgs(1).ToString 'es. "C:\Bus\Agg\iBUpdate" My.Application.CommandLineArgs(3).ToString

            If Right(par_strAggDir, 1) <> "\" Then
                par_strAggDir = par_strAggDir & "\"
            End If

            If Right(par_strBusNetDir, 1) <> "\" Then
                par_strBusNetDir = par_strBusNetDir & "\"
            End If


            Dim striBUpdateDir = par_strAggDir & "iBUpdate"

            Dim updateEseguito As Boolean = False



            Dim cicli As Integer = 1500 '1500 volte e aspetto 2 secondi, quindi aspetto che l'utente esca da Busnet per 50 minuti

            ' verifico che l'eseguibile di busnet non è utilizzato
            Do
                cicli = cicli - 1


                If IsWriteble(par_strBusNetDir + "BNIEIBUS.dll") AndAlso
                   IsWriteble(par_strBusNetDir + "BEIEIBUS.dll") AndAlso
                   IsWriteble(par_strBusNetDir + "BDIEIBUS.dll") Then
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


            'se busnet è chiuso
            If busNetChiuso Then
                Sleep(1000)
                'copio le dll
                CopyDirectory(striBUpdateDir, par_strBusNetDir, True)

                ScriviLog(par_strAggDir & "iBUpdate.log", "BusNet Aggiornato.")

                updateEseguito = True

                Sleep(2000)
            End If


            Environment.Exit(0)
        Catch ex As Exception
            ScriviLog(par_strAggDir & "iBUpdate.log", "Errore Catch: " & ex.Message)

            Environment.Exit(1)
        End Try


    End Sub

    ' Verifica se il file passato è scrivibile (aggiornabile)
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
        Dim sourceDir As DirectoryInfo = New DirectoryInfo(sourcePath)
        Dim destDir As DirectoryInfo = New DirectoryInfo(destPath)
        If (sourceDir.Exists) Then
            If Not (destDir.Exists) Then
                destDir.Create()
            End If
            Dim pFile As FileInfo
            For Each pFile In sourceDir.GetFiles()
                Try
                    If (overwrite) Then
                        pFile.CopyTo(Path.Combine(destDir.FullName, pFile.Name), True)
                    Else

                        If ((File.Exists(Path.Combine(destDir.FullName, pFile.Name))) = False) Then
                            pFile.CopyTo(Path.Combine(destDir.FullName, pFile.Name), False)
                        End If
                    End If
                Catch ex As Exception
                    ' Non faccio nulla se ho un errore
                End Try
            Next
            Dim dir As DirectoryInfo
            For Each dir In sourceDir.GetDirectories()
                CopyDirectory(dir.FullName, Path.Combine(destDir.FullName, dir.Name), overwrite)
            Next
        Else
            'Potresti gestire un errore in caso il percorso non esiste
        End If
    End Sub

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
        w.WriteLine("CommanLine: {0},{1}", par_strAggDir, par_strBusNetDir)
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


End Module
