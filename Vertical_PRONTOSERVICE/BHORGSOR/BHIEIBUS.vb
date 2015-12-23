Imports System.Data
Imports System.Data.Common
Imports NTSInformatica.CLN__STD

Public Class CLHIEIBUS
  Inherits CLDIEIBUS

  Public Overrides Sub Init(Applic As CLE__APP)
    MyBase.Init(Applic)
  End Sub

    ' Funzione non usata. Tenuta per il futuro
 

    ' Dim strSQL As String = ""

    ' Funzione non usata. Tenuta per il futuro
    'Public Overridable Function GetGirovisitaXMobile(strDitta As String, ByRef dttOut As DataTable, strWhere As String) As Boolean
    ' Dim strSQL As String = ""
    '
    '    Try
    '
    '     strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLHIEIBUS).Assembly, "GetGirovisita.sql")
    '    strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
    '   dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

    '    Return True

    ' Catch ex As Exception
    '--------------------------------------------------------------
    '    Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
    '--------------------------------------------------------------
    '  End Try
    'End Function


    Public Overrides Function GetArt(strDitta As String, ByRef dttOut As DataTable, strQuery As String, strWhere As String) As Boolean

        Dim Riga As Integer
        Dim RetVal As Boolean = False

        Try

            ' Inoltre nella query c'è questo filtro
            ' "tabcfam.tb_codcfam not in ('90', '99')"
            ' strWhere = " and tabcfam.tb_codcfam not in ('90', '99')"

            RetVal = MyBase.GetArt(strDitta, dttOut, strQuery, strWhere)

            For Riga = 0 To dttOut.Rows.Count - 1
                ' Gli articoli deperibili devono essere classificati con S. Se non lo sono devono avere N
                If dttOut.Rows(Riga)("ar_tipo").ToString = "D" Then
                    dttOut.Rows(Riga)("ar_tipo") = "S"
                Else
                    dttOut.Rows(Riga)("ar_tipo") = "N"
                End If
            Next

            Return RetVal


        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, "GetArt Personalizzata", oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try

    End Function

    ' Esempio
    Public Overrides Function GetCodpaga(ByRef dttOut As DataTable, strWhere As String) As Boolean

        Dim Riga As Integer
        Dim RetVal As Boolean = False

        Try
            ' https://github.com/Apex-net/ConnettoreiB/blob/master/BDIEIBUS/SQL%20Export/GetCodpaga.sql
            RetVal = MyBase.GetCodpaga(dttOut, strWhere)

            For Riga = 0 To dttOut.Rows.Count - 1
                If dttOut.Rows(Riga)("tb_despaga").ToString = "A" Then
                    dttOut.Rows(Riga).Delete()
                End If
            Next

            Return RetVal

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, "GetCodpaga Personalizzata", oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try

    End Function


    Public Overrides Function GetClifor(strTipoCF As String, strDitta As String, ByRef dttOut As DataTable, strFiltraConAgenti As String, strWhere As String) As Boolean

        Dim Riga As Integer
        Dim RetVal As Boolean = False

        Try

            ' Inoltre nella query c'è questo filtro
            ' "tabcfam.tb_codcfam not in ('90', '99')"
            ' strWhere = " and tabcfam.tb_codcfam not in ('90', '99')"

            RetVal = MyBase.GetClifor(strTipoCF, strDitta, dttOut, strFiltraConAgenti, strWhere)

            For Riga = 0 To dttOut.Rows.Count - 1
                If dttOut.Rows(Riga)("an_blocco").ToString <> "N" Then
                    dttOut.Rows(Riga)("an_descr1") = dttOut.Rows(Riga)("an_descr1").ToString.Trim + " [" + dttOut.Rows(Riga)("an_blocco").ToString + "]"
                End If
            Next

            Return RetVal


        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, "GetArt Personalizzata", oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try

    End Function



End Class

