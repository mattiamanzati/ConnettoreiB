Imports System.Data
Imports System.Data.Common
Imports NTSInformatica.CLN__STD

Public Class CLHIEIBUS
  Inherits CLDIEIBUS

  Public Overrides Sub Init(Applic As CLE__APP)
    MyBase.Init(Applic)
  End Sub

    Public Overridable Function cUpdateColli(ByVal strDitta As String, ByVal NumOrdine As Integer) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = "UPDATE movord SET  mo_colli = 0 WHERE mo_numord = " + NTSCStr(NumOrdine) + " and codditt=" + CStrSQL(strDitta) + " and (cast(GETDATE() As Date) = cast(mo_ultagg As Date))"

            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function



End Class

