Imports System.Net
Imports System.IO
Imports System.Data
'Imports NTSInformatica.CLN__STD

Public Class Download
    Public Event AmountDownloadedChanged(ByVal iNewProgress As Long)
    Public Event FileDownloadSizeObtained(ByVal iFileSize As Long)
    Public Event FileDownloadComplete()
    Public Event FileDownloadFailed(ByVal ex As Exception)

    Private filesize As Long

    'Vars in the dialog
    Public vars_unzipdir As String

    Public vars_msgText As String = "E' presente un aggiornamento del programma. Si desidera effettuare l'aggiornamento? (scelta consigliata!)"
    Public vars_msgTitle As String = "AutoUpdate"
    Public vars_newVersion As String
    Public vars_batch As Boolean = False 'specifica se il programma è sttao eseguito in modalità batch
    Private mCurrentFile As String = String.Empty

    Public Function CheckForUpdates(ByVal UrlFileUpdate As String) As Boolean
        Try
            Dim temppath As String = CStr(IIf(Environ$("tmp") <> "", Environ$("tmp"), Environ$("temp"))) 'Get temp path
            Dim rand As Integer = CInt(Int((10000 - 1 + 1) * Rnd()) + 1) 'Generate random number to have an unique filename
            Dim dldname As String = Path.GetFileName(UrlFileUpdate) 'Get filename of file to be downloaded
            DownloadFileWithProgress(UrlFileUpdate, temppath.ToString + "\" + rand.ToString + dldname) 'Download the file
            Dim a As New Ionic.Utils.Zip.ZipFile(temppath.ToString + "\" + rand.ToString + dldname) 'Create new var with zip file
            For Each zipfile As Ionic.Utils.Zip.ZipEntry In a 'for each loop for every file in zip
                If My.Computer.FileSystem.FileExists(vars_unzipdir + "\" + zipfile.FileName) Then 'If this file already exist, delete it
                    My.Computer.FileSystem.DeleteFile(vars_unzipdir + "\" + zipfile.FileName)
                    'If My.Computer.FileSystem.FileExists(vars_unzipdir + "\" + zipfile.FileName + ".OLD") Then
                    '    My.Computer.FileSystem.DeleteFile(vars_unzipdir + "\" + zipfile.FileName + ".OLD")
                    'End If
                    'My.Computer.FileSystem.RenameFile(vars_unzipdir + "\" + zipfile.FileName, zipfile.FileName + ".OLD")
                End If
                zipfile.Extract(vars_unzipdir) 'Extract this file
            Next
            Return True
        Catch ex As Exception
            Return False
        End Try
        Return Nothing
    End Function

    Private ReadOnly Property CurrentFile() As String
        Get
            Return mCurrentFile
        End Get
    End Property

    Private Function DownloadFile(ByVal URL As String, ByVal Location As String) As Boolean
        Try
            mCurrentFile = GetFileName(URL)
            Dim WC As New WebClient
            WC.DownloadFile(URL, Location)
            RaiseEvent FileDownloadComplete()
            Return True
        Catch ex As Exception
            RaiseEvent FileDownloadFailed(ex)
            Return False
        End Try
    End Function

    Private Function GetFileName(ByVal URL As String) As String
        Try
            Return URL.Substring(URL.LastIndexOf("/") + 1)
        Catch ex As Exception
            Return URL
        End Try
    End Function

    Private Function DownloadFileWithProgress(ByVal URL As String, ByVal Location As String) As Boolean
        Dim FS As FileStream = Nothing

        Try
            mCurrentFile = GetFileName(URL)
            Dim wRemote As WebRequest
            Dim bBuffer As Byte()
            ReDim bBuffer(256)
            Dim iBytesRead As Integer
            Dim iTotalBytesRead As Integer

            FS = New FileStream(Location, FileMode.Create, FileAccess.Write)
            wRemote = WebRequest.Create(URL)
            Dim myWebResponse As WebResponse = wRemote.GetResponse
            RaiseEvent FileDownloadSizeObtained(myWebResponse.ContentLength)
            Dim sChunks As Stream = myWebResponse.GetResponseStream
            Do
                iBytesRead = sChunks.Read(bBuffer, 0, 256)
                FS.Write(bBuffer, 0, iBytesRead)
                iTotalBytesRead += iBytesRead
                If myWebResponse.ContentLength < iTotalBytesRead Then
                    RaiseEvent AmountDownloadedChanged(myWebResponse.ContentLength)
                Else
                    RaiseEvent AmountDownloadedChanged(iTotalBytesRead)
                End If
            Loop While Not iBytesRead = 0
            sChunks.Close()
            FS.Close()
            RaiseEvent FileDownloadComplete()
            Return True
        Catch ex As Exception
            If Not (FS Is Nothing) Then
                FS.Close()
                FS = Nothing
            End If
            RaiseEvent FileDownloadFailed(ex)
            Return False
        End Try
    End Function

    Private Sub _Downloader_FileDownloadComplete() Handles Me.FileDownloadComplete
        '*** QUI POSSO SCRIVERE QUELLO DA FARE ALLA FINE DEL DOWNLOAD (ES. AGGIORNAMENTO VERSIONE, ETC.) ***
    End Sub

    Private Sub _Downloader_FileDownloadFailed(ByVal ex As System.Exception) Handles Me.FileDownloadFailed
        If Not vars_batch Then
            'NON SI VUOLE FARE VEDERE NESSUN MESSAGGIO DI ERRORE ALL'UTENTE
            'MsgBox("Attenzione. Errore nello scaricamento dell'aggiornamento al connettore iB." + vbNewLine + vbNewLine + ex.Message)
        End If
    End Sub

  Public Function CheckNewVersion(ByVal UrlVersionUpdate As String, CurrentVersion As String) As Boolean
    Try
      Dim tokenNewVersion() As String = Nothing
      Dim newVersionString As String = ""

      Dim tokenMyVersion() As String = Nothing
      Dim myVersionString As String = ""

      Dim client As WebClient = New System.Net.WebClient() 'Create new var to get newest version available

      tokenNewVersion = client.DownloadString(UrlVersionUpdate).Split("."c)
      newVersionString = tokenNewVersion(0).PadLeft(1, "0"c) + tokenNewVersion(1).PadLeft(2, "0"c) + tokenNewVersion(2).PadLeft(2, "0"c) + tokenNewVersion(3).PadLeft(3, "0"c)

      tokenMyVersion = CurrentVersion.Split("."c)
      myVersionString = tokenMyVersion(0).PadLeft(1, "0"c) + tokenMyVersion(1).PadLeft(2, "0"c) + tokenMyVersion(2).PadLeft(2, "0"c) + tokenMyVersion(3).PadLeft(3, "0"c)


      Dim newVersion As Integer = CInt(newVersionString)
      Dim myVersion As Integer = CInt(myVersionString)

      If CLng(newVersion) > CLng(myVersion) Then 'Check if newest version is > than installed version
        vars_newVersion = newVersionString
        Return True 'If so, return true
      Else
        Return False 'Otherwise, return false
      End If
    Catch
      Return False
    End Try
  End Function

End Class
