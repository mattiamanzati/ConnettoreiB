Imports System.Data
Imports System
Imports System.Net
Imports NTSInformatica.CLN__STD


Public Class SetProxy

  Public Shared Function SettingProxy(ByVal ProxyUri As String, ByVal Port As Integer, ByVal ProxyCustomUsername As String, ByVal ProxyCustomUserPassword As String, Optional ByVal ProxyCustomUserDomain As String = "", Optional ByVal UseDefaultCredentials As Boolean = False) As Boolean

    Dim BypassOnLocal As Boolean = False
    Dim BypassList As String = ""


    Dim UseCustomCredentials As Boolean = False
    If ProxyCustomUsername <> "" Then
      UseCustomCredentials = True
    End If


    Try

      ' =================================================================
      ' Address setup
      ' =================================================================
      Dim proxy As New WebProxy(ProxyUri, Port)

      ' =================================================================
      ' Autentication settings
      ' =================================================================
      proxy.UseDefaultCredentials = UseDefaultCredentials
      If Not proxy.UseDefaultCredentials AndAlso UseCustomCredentials Then
        Dim credentials As New NetworkCredential(ProxyCustomUsername, ProxyCustomUserPassword)

        If Not String.IsNullOrEmpty(ProxyCustomUsername) Then
          credentials.Domain = ProxyCustomUserDomain
        End If

        proxy.Credentials = credentials
      End If

      ' =================================================================
      ' Bypass settings
      ' =================================================================
      proxy.BypassProxyOnLocal = BypassOnLocal
      If Not String.IsNullOrEmpty(BypassList) Then
        proxy.BypassList = BypassList.Split(","c)
      End If

      ' Overwrite default proxy with the one we created above
      WebRequest.DefaultWebProxy = proxy


      Return True

    Catch ex As Exception
      '--------------------------------------------------------------
      'Throw (New NTSException(GestError(ex, "", ex.Message, "SetProxy", "", False)))
      Throw (New Exception("SetProxy:" + ex.Message))

      Return False
    End Try


  End Function

  
End Class
