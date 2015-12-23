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


    Public Overrides Function GeneraOrdineAPI(Ordine As TestataOrdineExport, ByRef pNumOrd As Integer, ByRef pAnno As Integer, ByRef pSerie As String, ByRef pTipork As String, ByRef pCodDitta As String) As Boolean
        Dim RetVal As Boolean

        Try
            RetVal = MyBase.GeneraOrdineAPI(Ordine, pNumOrd, pAnno, pSerie, pTipork, pCodDitta)
            CType(oCldIbus, CLHIEIBUS).cUpdateColli(strDittaCorrente, pNumOrd)

            Return True

        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, "Esecuzione custom cUpdateColli", oApp.InfoError, "", False)))
        End Try

        Return RetVal

    End Function

End Class