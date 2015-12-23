Imports System.Data
Imports System.Data.Common
Imports NTSInformatica.CLN__STD

Public Class CLHIEIBUS
  Inherits CLDIEIBUS

  Public Overrides Sub Init(Applic As CLE__APP)
    MyBase.Init(Applic)
  End Sub

    Public Overrides Function GetArt(strDitta As String, ByRef dttOut As DataTable, strQuery As String, strWhere As String) As Boolean
        Dim Riga As Integer
        Dim RetVal As Boolean = False

        Try

            RetVal = MyBase.GetArt(strDitta, dttOut, strQuery, strWhere)

            For Riga = 0 To dttOut.Rows.Count - 1
                dttOut.Rows(Riga)("ar_descr") = dttOut.Rows(Riga)("ar_codalt").ToString & " - " & dttOut.Rows(Riga)("ar_descr").ToString
            Next

            Return RetVal


        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, "GetArt Personalizzata", oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try

    End Function


End Class

