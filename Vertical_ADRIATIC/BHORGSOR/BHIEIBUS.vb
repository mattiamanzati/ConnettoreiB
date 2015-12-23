Imports System.Data
Imports System.Data.Common
Imports NTSInformatica.CLN__STD

Public Class CLHIEIBUS
  Inherits CLDIEIBUS

  Public Overrides Sub Init(Applic As CLE__APP)
    MyBase.Init(Applic)
  End Sub

    Public Function cGetCustomFieldsProdotti(ByVal strDitta As String, ByRef dttOut As DataTable) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLHIEIBUS).Assembly, "cGetCustomFieldsProdotti.sql")
            'strSQL = System.IO.File.ReadAllText("cGetCustomFieldsProdotti.sql")

            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function



End Class

