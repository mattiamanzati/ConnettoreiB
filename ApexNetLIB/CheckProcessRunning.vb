Option Strict Off

Imports System.Data
Imports System
Imports System.Net
Imports RestSharpApex
Imports NewtonsoftApex
Imports System.Threading.Thread
Imports System.IO

Public Class CheckProcessRunning

    Public Shared Function IsProcessRunning(ByVal processPath As String) As Boolean
        Dim RetVal As Boolean

        ' http://msdn.microsoft.com/en-us/library/windows/desktop/aa394372(v=vs.85).aspx
        On Error Resume Next

        Dim IsRunning As Object
        IsRunning = GetObject("winmgmts:").ExecQuery("SELECT * FROM Win32_Process WHERE ExecutablePath = '" & Replace(processPath, "\", "\\") & "'").Count


        If IsRunning = 0 Then
            RetVal = False
        Else
            RetVal = True
        End If

        Return RetVal

    End Function

End Class

