Imports System.Data
Imports System.Data.Common
Imports NTSInformatica.CLN__STD

Public Class CLHIEIBUS
  Inherits CLDIEIBUS

  Public Overrides Sub Init(Applic As CLE__APP)
    MyBase.Init(Applic)
  End Sub

    Public Overrides Function GetCliforTestDoc(strTipoCF As String, strDitta As String, ByRef dttOut As DataTable, strWhere As String, Optional strGiorniStoricoDocumenti As String = "365") As Boolean

        Dim Riga As Integer
        Dim RetVal As Boolean = False

        Try

            ' Recupero i dati delle Testate documenti
            RetVal = MyBase.GetCliforTestDoc(strTipoCF, strDitta, dttOut, strWhere, strGiorniStoricoDocumenti)

            ' Ciclo per ogni riga recuperata
            For Riga = 0 To dttOut.Rows.Count - 1
                ' Se non sto trattando i dati della query testord...
                If dttOut.Rows(Riga)("xx_tipo").ToString <> "O" Then

                    If dttOut.Rows(Riga)("tm_serie").ToString = "QUELLO CHE VUOI TU" Then
                        dttOut.Rows(Riga).Delete()
                    End If
                    ' Elimino la riga dal datatable

                End If
            Next

            Return RetVal

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, "GetCliforTestDoc", oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

End Class

