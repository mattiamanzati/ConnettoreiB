Imports System.Data
Imports System.Data.Common
Imports NTSInformatica.CLN__STD

Public Class CLHIEIBUS
  Inherits CLDIEIBUS

  Public Overrides Sub Init(Applic As CLE__APP)
    MyBase.Init(Applic)
  End Sub

    Public Overridable Function cTipoKit(ByVal strDitta As String, ByVal CodArt As String, ByRef ValoreKit As String) As Boolean
        Dim strSQL As String = ""
        Dim dttOut As DataTable

        Try
           ' strSQL = "UPDATE movord SET  mo_colli = 0 WHERE mo_numord = " + NTSCStr(NumOrdine) + " and codditt=" + CStrSQL(strDitta) + " and (cast(GETDATE() As Date) = cast(mo_ultagg As Date))"

            strSQL = String.Format("SELECT ar_tipokit FROM artico WHERE codditt = '{0}' AND ar_codart = '{1}'", strDitta, CodArt)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

            If dttOut.Rows.Count = 0 Then
                Return False
            Else
                ValoreKit = NTSCStr(dttOut.Rows(0)!ar_tipokit)
                Return True
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function



End Class

