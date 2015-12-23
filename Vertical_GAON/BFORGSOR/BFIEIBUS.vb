Imports System
Imports System.Data
Imports System.Data.Common
Imports System.Globalization
Imports System.Text
Imports System.IO
Imports NTSInformatica.CLN__STD

Imports AMHelper.WS
Imports AMHelper.CSV
Imports RestSharpApex


Public Class CLFIEIBUS
  Inherits CLEIEIBUS


    Public Overrides Function PreInsert_Ordine(ByRef t As TestataOrdineExport) As Boolean
        Dim RetVal As Boolean = False
        Dim ValKit As String = ""
        Try

            ' Ciclo per ogni riga dell'ordine
            For Each riga As RigaOrdineExport In t.righe

                ' Estraggo il codice kit dell'articolo
                If CType(oCldIbus, CLHIEIBUS).cTipoKit(strDittaCorrente, riga.codice_articolo, ValKit) Then

                    ' Se l'articolo e' di tipo KIT (Analitico) sovrascrivo a zero il valore
                    ' della riga altrimenti l'esplosione di Business va in errore
                    If ValKit = "A" Then
                        riga.prezzo = 0
                    End If
                End If
            Next

            Return True

        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, "Esecuzione PreInsert Ordine Custom", oApp.InfoError, "", False)))
        End Try

        Return RetVal

    End Function

End Class