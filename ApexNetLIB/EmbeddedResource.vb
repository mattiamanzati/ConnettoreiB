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

    Public Shared Function GetString(assembly As System.Reflection.Assembly, name As String, CustomSQLPath As String) As String

        Dim data As String = ""
        Dim SQLFile As String = ""

        ' Se non ho specificato il percorso in cui cercare la query
        If CustomSQLPath = "" Then
            ' Il percorso e' quello di esecuzione 
            SQLFile = name
        Else
            ' se no e' quello passato come argomento
            SQLFile = CustomSQLPath & "\" & name
        End If

        '
        If SQLFile <> "" Then
            If File.Exists(SQLFile) Then
                data = File.ReadAllText(SQLFile, System.Text.Encoding.UTF8)
                data = "-- Query custom " + SQLFile + vbCrLf + data
            Else
                Dim sr As System.IO.StreamReader
                sr = EmbeddedResource.GetStream(assembly, name)
                data = sr.ReadToEnd()
                sr.Close()
            End If

        End If

        Return data
    End Function

    Public Shared Function GetString(assembly As System.Reflection.Assembly, name As String) As String

        'EmbeddedResource.GetString(assembly As System.Reflection.Assembly, name As String, CustomSQLPath As String) As String

        Return EmbeddedResource.GetString(assembly, name, "")

    End Function


    'Public Shared Function GetString(name As String) As String
    '   Return EmbeddedResource.GetString(GetType(EmbeddedResource).Assembly, name)
    'End Function
End Class
