Imports System
Imports System.Diagnostics

Public Class WEDOLogger

    Public Shared Sub WriteToRegistry(ByVal strMsg As String)

        WriteToRegistry(strMsg, EventLogEntryType.Information)

    End Sub


    Public Shared Sub WriteToRegistry(ByVal strMsg As String, ByVal LogType As EventLogEntryType)

        Dim sSource As String = "BNIEIBUS"
 
        Try
            If Not EventLog.SourceExists(sSource) Then
                EventLog.CreateEventSource(sSource, "Application")
            End If
            EventLog.WriteEntry(sSource, strMsg, LogType, 234)
        Catch ex As Exception

        End Try
      
    End Sub

End Class
