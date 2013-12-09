Imports System.IO

Public Class EmbeddedResource
  Private Sub New()
  End Sub

  Public Shared Function GetStream(assembly As System.Reflection.Assembly, name As String) As StreamReader
    For Each resName As String In assembly.GetManifestResourceNames()
      If resName.EndsWith(name) Then
        Return New System.IO.StreamReader(assembly.GetManifestResourceStream(resName))
      End If
    Next
    Return Nothing
  End Function

  Public Shared Function GetString(assembly As System.Reflection.Assembly, name As String) As String
    Dim sr As System.IO.StreamReader = EmbeddedResource.GetStream(assembly, name)
    Dim data As String = sr.ReadToEnd()
    sr.Close()
    Return data
  End Function

  Public Shared Function GetString(name As String) As String
    Return EmbeddedResource.GetString(GetType(EmbeddedResource).Assembly, name)
  End Function
End Class
