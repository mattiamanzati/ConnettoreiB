Imports System.Data
Imports System.Data.Common
Imports NTSInformatica.CLN__STD
Imports AMHelper.WS

Public Class CLHIEIBUS
    Inherits CLDIEIBUS

    Public Overrides Sub Init(ByVal Applic As CLE__APP)
        MyBase.Init(Applic)
    End Sub

    Public Overrides Function GetLeads(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String, _
                                         Optional ByVal strIncludiLeadConClienti As String = "0") As Boolean
        Dim strSQL As String = ""
        Try

            'strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetLeads.sql")

            'TNET_INIMOD
            'TNET_RIGHE_COMMENTO 1
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLHIEIBUS).Assembly, "GetLeads.sql")
            'TNET_FINEMO

            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@includi_lead_clienti", strIncludiLeadConClienti)

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overrides Function GetCliforTestDoc(ByVal strTipoCF As String, ByVal strDitta As String, ByRef dttOut As System.Data.DataTable, ByVal strWhere As String, Optional ByVal strGiorniStoricoDocumenti As String = "365") As Boolean
        Dim dtOfferte As DataTable = Nothing

        Try
            If Not MyBase.GetCliforTestDoc(strTipoCF, strDitta, dttOut, strWhere, strGiorniStoricoDocumenti) Then
                Return False
            End If

            If Not GetCliForTestDoc_Offerte(strDitta, strTipoCF, strGiorniStoricoDocumenti, dtOfferte) Then
                Return False
            End If

            dttOut.Merge(dtOfferte)

            Return True

        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overrides Function GetCliforRighDoc(ByVal strTipoCF As String, ByVal strDitta As String, ByRef dttOut As System.Data.DataTable, ByVal strWhere As String, Optional ByVal strGiorniStoricoDocumenti As String = "365") As Boolean
        Dim dtOfferte As DataTable = Nothing

        Try
            If Not MyBase.GetCliforRighDoc(strTipoCF, strDitta, dttOut, strWhere, strGiorniStoricoDocumenti) Then
                Return False
            End If

            If Not GetCliForRigheDoc_Offerte(strDitta, strTipoCF, strGiorniStoricoDocumenti, dtOfferte) Then
                Return False
            End If

            dttOut.Merge(dtOfferte)

            Return True

        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function GetCliForTestDoc_Offerte(ByVal codditt As String, _
                                                        ByVal tipocf As String, _
                                                        ByVal ggDocumenti As String, _
                                                        ByRef dtOfferte As DataTable) As Boolean
        Dim strsql As String = ""

        Try

            'strsql = "SELECT 'OFF' as xx_tipo, " & _
            '        "testoff.codditt + '§' + CAST(testoff.td_conto AS VARCHAR)+ '§' + CAST(testoff.td_anno as varchar) + '§' + testoff.td_serie + '§' + CAST(testoff.td_numord as varchar) + '§' + testoff.td_tipork as xx_numreg, " & _
            '        "an_tipo, an_conto,testoff.td_tipork as tm_tipork, testoff.td_anno as tm_anno,testoff.td_serie as tm_serie,testoff.td_numord as tm_numdoc, testoff.td_datord as tm_datdoc, testoff.td_totlordo as tm_totdoc, testoff.td_ultagg as tm_ultagg,tb_desvalu,cast(0 as smallint) as tm_magaz,cast(0 as int) as tm_numfat, ' ' as tm_alffat, cast('1900-01-01' as datetime) as tm_datfat,cast(0 as int) as tm_annpar, ' ' as tm_alfpar,cast(0 as int) as tm_numpar, CASE WHEN td_flevas = 'S' THEN '1' ELSE '0' END as xx_flevas, td_tipobf as xx_tipobf, tb_destpbf as tb_destpbf " & _
            '        "FROM testoff WITH (NOLOCK) INNER JOIN anagra WITH (NOLOCK) " & _
            '                "ON testoff.codditt = anagra.codditt AND testoff.td_conto = anagra.an_conto " & _
            '            "INNER JOIN tabtpbf WITH (NOLOCK) " & _
            '                "ON testoff.codditt = tabtpbf.codditt AND testoff.td_tipobf = tabtpbf.tb_codtpbf " & _
            '            "LEFT JOIN tabvalu WITH (NOLOCK) " & _
            '             "ON testoff.td_valuta = tabvalu.tb_codvalu " & _
            '            "INNER JOIN (select codditt,td_tipork,td_anno,td_numord,td_serie,MAX(td_vers) td_vers from testoff " & _
            '                        "group by codditt,td_tipork,td_anno,td_numord,td_serie) MX " & _
            '                "ON MX.codditt = testoff.codditt AND MX.td_tipork = testoff.td_tipork AND MX.td_anno = testoff.td_anno AND MX.td_serie = testoff.td_serie AND MX.td_numord = testoff.td_numord and MX.td_vers=testoff.td_vers " & _
            '        "WHERE 1 = 1 " & _
            '             " AND anagra.codditt =  " & CStrSQL(codditt) & _
            '             " AND an_tipo <> 'S' " & _
            '             " AND ((an_tipo = 'C' and " & CStrSQL(tipocf) & " = 'C') or (an_tipo = 'F' and " & CStrSQL(tipocf) & " = 'F') or (an_tipo <> 'S' and " & CStrSQL(tipocf) & " = 'CF')) " & _
            '             " AND an_status = 'A' " & _
            '             " AND td_annull='N' " & _
            '             " AND td_chiuso='N' "


            '       NON FUNZIONA la chiamata a uno script .SQL aggiunto in BH
            'strsql = ApexNetLIB.EmbeddedResource.GetString(GetType(CLHIEIBUS).Assembly, "GetCliForTestDocOfferte.Sql")
            'strsql = strsql.Replace("@ditta", CStrSQL(codditt))


            '"           CASE td.td_valuta WHEN 0 THEN td.td_totdoc ELSE td.td_totdocv END AS tm_totdoc,  " & vbCrLf 
            strsql = _
                " SELECT     'OFF' AS xx_tipo,  " & vbCrLf & _
                "           td.codditt + '§' + CAST(td.td_conto AS VARCHAR) + '§' +   " & vbCrLf & _
                "           CAST(td.td_anno AS varchar) + '§' + td.td_serie + '§' + CAST(td.td_numord AS varchar) + '§' + td.td_tipork AS xx_numreg,   " & vbCrLf & _
                "           an.an_tipo, an.an_conto,   " & vbCrLf & _
                "           td.td_tipork AS tm_tipork, td.td_anno AS tm_anno, td.td_serie AS tm_serie, td.td_numord AS tm_numdoc,   " & vbCrLf & _
                "           td.td_datord AS tm_datdoc,   " & vbCrLf & _
                "           td.td_totdoc AS tm_totdoc,  " & vbCrLf & _
                "           td.td_ultagg AS tm_ultagg,   " & vbCrLf & _
                "           tv.tb_desvalu, CAST(0 AS smallint) AS tm_magaz, CAST(0 AS int) AS tm_numfat,   " & vbCrLf & _
                "            ' ' AS tm_alffat, CAST('1900-01-01' AS datetime) AS tm_datfat,   " & vbCrLf & _
                "           CAST(0 AS int) AS tm_annpar, ' ' AS tm_alfpar, CAST(0 AS int) AS tm_numpar,   " & vbCrLf & _
                "           td.td_valuta as tm_valuta,  " & vbCrLf & _
                "           CASE WHEN td_flevas = 'S' THEN '1' ELSE '0' END AS xx_flevas, td.td_tipobf AS xx_tipobf, bf.tb_destpbf  " & vbCrLf

            strsql &= _
                "   FROM         testoff AS td WITH (NOLOCK) LEFT OUTER JOIN  " & vbCrLf & _
                "                      leads AS le WITH (NOLOCK) ON td.codditt = le.codditt AND td.td_codlead = le.le_codlead LEFT OUTER JOIN  " & vbCrLf & _
                "                      anagra AS an WITH (NOLOCK) ON le.codditt = an.codditt AND le.le_conto = an.an_conto LEFT OUTER JOIN  " & vbCrLf & _
                "                      tabtpbf AS bf WITH (NOLOCK) ON td.codditt = bf.codditt AND td.td_tipobf = bf.tb_codtpbf LEFT OUTER JOIN  " & vbCrLf & _
                "                      tabvalu as tv WITH (NOLOCK) ON td.td_valuta = tv.tb_codvalu INNER JOIN  " & vbCrLf & _
                "                          (SELECT     codditt, td_tipork, td_anno, td_numord, td_serie, MAX(td_vers) AS td_vers   " & vbCrLf & _
                "                            FROM          testoff AS testoff_1   " & vbCrLf & _
                "                            GROUP BY codditt, td_tipork, td_anno, td_numord, td_serie) AS MX ON MX.codditt = td.codditt AND MX.td_tipork = td.td_tipork AND MX.td_anno = td.td_anno AND   " & vbCrLf & _
                "            MX.td_serie = td.td_serie And MX.td_numord = td.td_numord And MX.td_vers = td.td_vers " & vbCrLf

            'filtra i documenti di tipo OFFERTA relativa ai soli LEADS CLIENTI
            strsql &= _
                "            WHERE  (1 = 1) " & vbCrLf & _
                "	                AND td.td_tipork = '!' " & vbCrLf & _
                " 	                AND an.codditt = " & CStrSQL(codditt) & vbCrLf

            dtOfferte = OpenRecordset(strsql, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function GetCliForRigheDoc_Offerte(ByVal codditt As String, ByVal tipocf As String, ByVal ggDocumenti As String, ByRef dtOfferte As DataTable) As Boolean
        Dim strsql As String = ""

        Try

            'strsql = "SELECT        11  as query, " & vbCrLf & _
            '            "           an.an_tipo, an.an_conto, " & vbCrLf & _
            '            "           td.codditt + '§' + CAST(td.td_conto AS VARCHAR)+ '§' + CAST(td.td_anno as varchar) + '§' + td.td_serie + '§' + CAST(td.td_numord as varchar) + '§' + td.td_tipork as xx_numreg, " & vbCrLf & _
            '            "           td.td_tipork as tm_tipork, " & vbCrLf & _
            '            "           td.td_anno as tm_anno, " & vbCrLf & _
            '            "           td.td_serie as tm_serie,  " & vbCrLf & _
            '            "           td.td_numord as tm_numdoc, " & vbCrLf & _
            '            "           '0' as tm_tipork1,  " & vbCrLf & _
            '            "           '0' as tm_serie1, " & vbCrLf & _
            '            "           0 as tm_numdoc1, " & vbCrLf & _
            '            "           right('00000' + CAST(mo.mo_riga AS NVARCHAR(50)), 4) AS mm_riga2, " & vbCrLf & _
            '            "           mo.mo_codart as mm_codart, " & vbCrLf & _
            '            "           mo.mo_fase as mm_fase, " & vbCrLf & _
            '            "           mo.mo_descr as mm_descr, " & vbCrLf & _
            '            "           mo.mo_desint as mm_desint, " & vbCrLf & _
            '            "           mo.mo_unmis as mm_unmis, " & vbCrLf & _
            '            "           mo.mo_ump as mm_ump, " & vbCrLf & _
            '            "           mo.mo_quant as mm_quant, " & vbCrLf & _
            '            "           mo.mo_colli as mm_colli, " & vbCrLf & _
            '            "            CASE	WHEN	td_valuta=0	THEN	mo_valore  " & vbCrLf & _
            '            "                    ELSE	--	td_valuta <>0 " & vbCrLf & _
            '            "                            (	-- qta  " & vbCrLf & _
            '            "	                            CASE WHEN mo_umprz<>'S' THEN mo_quant ELSE mo_colli END  " & vbCrLf & _
            '            "	                            -- prezzo valuta " & vbCrLf & _
            '            "	                            * mo_prezvalc  " & vbCrLf & _
            '            "	                            * (100-mo_scont1)/100*(100-mo_scont2)/100*(100-mo_scont3)/100*(100-mo_scont4)/100*(100-mo_scont5)/100*(100-mo_scont6)/100  " & vbCrLf & _
            '            "                            ) / mo_perqta " & vbCrLf & _
            '            "            END AS mm_valore,  " & vbCrLf & _
            '            "           mo.mo_scont1," & vbCrLf & _
            '            "           mo.mo_scont2," & vbCrLf & _
            '            "           mo.mo_scont3, " & vbCrLf & _
            '            "            CASE	WHEN td_valuta=0  THEN " & vbCrLf & _
            '            "                            (	mo_prezzo  " & vbCrLf & _
            '            "	                            * (100-mo_scont1)/100*(100-mo_scont2)/100*(100-mo_scont3)/100*(100-mo_scont4)/100*(100-mo_scont5)/100*(100-mo_scont6)/100  " & vbCrLf & _
            '            "                            )  " & vbCrLf & _
            '            "                    ELSE	(	mo_prezvalc " & vbCrLf & _
            '            "	                            * (100-mo_scont1)/100*(100-mo_scont2)/100*(100-mo_scont3)/100*(100-mo_scont4)/100*(100-mo_scont5)/100*(100-mo_scont6)/100  " & vbCrLf & _
            '            "                            )  " & vbCrLf & _
            '            "            END AS xx_prezzo, " & vbCrLf & _
            '            "           td.td_valuta as tm_valuta,  " & vbCrLf & _
            '            "           td.td_ultagg as xx_ultagg  " & vbCrLf

            'strsql &= _
            '        "FROM testoff as td WITH (NOLOCK) INNER JOIN movoff as mo WITH (NOLOCK) " & vbCrLf & _
            '                "ON mo.codditt = td.codditt AND mo.mo.mo_tipork = td.td_tipork AND mo.mo.mo_anno = td.td_anno AND mo.mo.mo_serie = td.td_serie AND mo.mo.mo_numord = td.td_numord and mo.mo.mo_vers=td.td_vers " & vbCrLf & _
            '                "INNER JOIN anagra WITH (NOLOCK) " & vbCrLf & _
            '                "ON td.codditt = an.codditt AND td.td_conto = an.an.an_conto " & vbCrLf & _
            '            "INNER JOIN (select codditt,td_tipork,td_anno,td_numord,td_serie,MAX(td_vers) td_vers from testoff " & vbCrLf & _
            '                        "group by codditt,td_tipork,td_anno,td_numord,td_serie) MX " & vbCrLf & _
            '                "ON MX.codditt = td.codditt AND MX.td_tipork = td.td_tipork AND MX.td_anno = td.td_anno AND MX.td_serie = td.td_serie AND MX.td_numord = td.td_numord and MX.td_vers=td.td_vers " & vbCrLf & _
            '        "WHERE an.codditt = " & CStrSQL(codditt) & vbCrLf & _
            '            " AND an.an_tipo <> 'S' " & vbCrLf & _
            '            " AND an.an_status = 'A' " & vbCrLf & _
            '            " AND ((an.an_tipo = 'C' and " & CStrSQL(tipocf) & " = 'C') or (an.an_tipo = 'F' and " & CStrSQL(tipocf) & " = 'F') or (an.an_tipo <> 'S' and " & CStrSQL(tipocf) & " = 'CF')) " & vbCrLf & _
            '            " AND td.td_annull='N' " & vbCrLf & _
            '            " AND td.td_chiuso='N' " & vbCrLf & _
            '            " AND mo.mo.mo_flevas<>'S'" & vbCrLf & _
            '            " AND mo.mo.mo_abband='N'" & vbCrLf


            '       NON FUNZIONA la chiamata a uno script .SQL aggiunto in BH
            'strsql = ApexNetLIB.EmbeddedResource.GetString(GetType(CLHIEIBUS).Assembly, "GetCliForRigheDocOfferte.Sql")
            'strsql = strsql.Replace("@ditta", CStrSQL(codditt))


            '"            CAST( ( CASE	WHEN	td_valuta=0	THEN	mo_valore   " & vbCrLf & _
            '"                    ELSE	--	td_valuta <>0  " & vbCrLf & _
            '"                            (	-- qta   " & vbCrLf & _
            '"	                            CASE WHEN mo_umprz<>'S' THEN mo_quant ELSE mo_colli END   " & vbCrLf & _
            '"	                            -- prezzo valuta  " & vbCrLf & _
            '"	                            * mo_prezvalc   " & vbCrLf & _
            '"	                            * (100-mo_scont1)/100*(100-mo_scont2)/100*(100-mo_scont3)/100*(100-mo_scont4)/100*(100-mo_scont5)/100*(100-mo_scont6)/100   " & vbCrLf & _
            '"                            ) / mo_perqta  " & vbCrLf & _
            '"                   END) as money) AS mm_valore,   " & vbCrLf & _
            '    "            CASE	WHEN td_valuta=0  THEN  " & vbCrLf & _
            '    "                            (	mo_prezzo   " & vbCrLf & _
            '    "	                            * (100-mo_scont1)/100*(100-mo_scont2)/100*(100-mo_scont3)/100*(100-mo_scont4)/100*(100-mo_scont5)/100*(100-mo_scont6)/100   " & vbCrLf & _
            '    "                            )   " & vbCrLf & _
            '    "                    ELSE	(	mo_prezvalc  " & vbCrLf & _
            '    "	                            * (100-mo_scont1)/100*(100-mo_scont2)/100*(100-mo_scont3)/100*(100-mo_scont4)/100*(100-mo_scont5)/100*(100-mo_scont6)/100   " & vbCrLf & _
            '    "                            )   " & vbCrLf & _
            '    "            END AS xx_prezzo,  " & vbCrLf 
            strsql = _
                "   SELECT        11  as query, " & vbCrLf & _
                "           mo.mo_codart AS xx_codart," & vbCrLf & _
                "           an.an_tipo, an.an_conto,  " & vbCrLf & _
                "           td.codditt + '§' + CAST(td.td_conto AS VARCHAR)+ '§' + CAST(td.td_anno as varchar) + '§' + td.td_serie + '§' + CAST(td.td_numord as varchar) + '§' + td.td_tipork as xx_numreg,  " & vbCrLf & _
                "           td.td_tipork as tm_tipork,  " & vbCrLf & _
                "           td.td_anno as tm_anno,  " & vbCrLf & _
                "           td.td_serie as tm_serie,   " & vbCrLf & _
                "           td.td_numord as tm_numdoc,  " & vbCrLf & _
                "            '0' as tm_tipork1,   " & vbCrLf & _
                "            '0' as tm_serie1,  " & vbCrLf & _
                "           0 as tm_numdoc1,  " & vbCrLf & _
                "           right('00000' + CAST(mo.mo_riga AS NVARCHAR(50)), 4) AS mm_riga2,  " & vbCrLf & _
                "           mo.mo_codart as mm_codart,  " & vbCrLf & _
                "           mo.mo_fase as mm_fase,  " & vbCrLf & _
                "           mo.mo_descr as mm_descr,  " & vbCrLf & _
                "           mo.mo_desint as mm_desint,  " & vbCrLf & _
                "           mo.mo_unmis as mm_unmis,  " & vbCrLf & _
                "           mo.mo_ump as mm_ump,  " & vbCrLf & _
                "           mo.mo_quant as mm_quant,  " & vbCrLf & _
                "           mo.mo_colli as mm_colli,  " & vbCrLf & _
                "           mo_valore  AS mm_valore,   " & vbCrLf & _
                "           mo.mo_scont1, " & vbCrLf & _
                "           mo.mo_scont2, " & vbCrLf & _
                "           mo.mo_scont3,  " & vbCrLf & _
                "           CASE	WHEN mo_quant=0 THEN 0 " & vbCrLf & _
                "               ELSE    Round((mo_valore / mo_quant) * mo_perqta, 4) " & vbCrLf & _
                "           END	AS xx_prezzo, " & vbCrLf & _
                "           td.td_valuta as tm_valuta,   " & vbCrLf & _
                "           mo.mo_stasino, " & vbCrLf & _
                "           td.td_ultagg as xx_ultagg   " & vbCrLf

            strsql &= _
                "   FROM testoff as td WITH (NOLOCK) INNER JOIN movoff as mo WITH (NOLOCK)   " & vbCrLf & _
                "        ON mo.codditt = td.codditt AND mo.mo_tipork = td.td_tipork AND mo.mo_anno = td.td_anno AND mo.mo_serie = td.td_serie AND mo.mo_numord = td.td_numord and mo.mo_vers=td.td_vers   " & vbCrLf & _
                "       INNER JOIN anagra as an WITH (NOLOCK)   " & vbCrLf & _
                "       ON td.codditt = an.codditt AND td.td_conto = an.an_conto   " & vbCrLf & _
                "       INNER JOIN (select codditt,td_tipork,td_anno,td_numord,td_serie,MAX(td_vers) td_vers from testoff   " & vbCrLf & _
                "                   group by codditt,td_tipork,td_anno,td_numord,td_serie) MX   " & vbCrLf & _
                "       ON MX.codditt = td.codditt AND MX.td_tipork = td.td_tipork AND MX.td_anno = td.td_anno AND MX.td_serie = td.td_serie AND MX.td_numord = td.td_numord and MX.td_vers=td.td_vers   " & vbCrLf

            'filtra i documenti di tipo OFFERTA relativa ai soli LEADS CLIENTI
            strsql &= _
                "            WHERE  (1 = 1) " & vbCrLf & _
                "	                AND td.td_tipork = '!' " & vbCrLf & _
                " 	                AND an.codditt = " & CStrSQL(codditt) & vbCrLf


            dtOfferte = OpenRecordset(strsql, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False)))
        End Try
    End Function

    ' Sovrascrive la funzione standard che recupera la lista di lead per operatore, aggiungendo anche gli operatori associati alle zone leads
    ' 09/12/2015 - S. Teodorani
    Public Overrides Function GetLeadAccessi(strDitta As String, ByRef dttOut As DataTable, strWhere As String) As Boolean
        Dim strSQL As String = ""
        Dim dttOutOperatoriZona As New DataTable
        Try
            ' Chiamo la funzione dello standard
            Dim RetValAccessiOri As Boolean = MyBase.GetLeadAccessi(strDitta, dttOut, strWhere)

            ' Esegui la query personalizzata che aggiunge la lista degli operatori per zona
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLHIEIBUS).Assembly, "cGetLeadOperatoriZona.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            dttOutOperatoriZona = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            ' Agigungo ai dati della query standard i leads associati agli operatori passando dalla tabella associativa delle zone.
            dttOut.Merge(dttOutOperatoriZona)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try

    End Function

    Public Overrides Function GetLeadTestOff(ByVal strDitta As String, ByRef dttOut As System.Data.DataTable, ByVal strIncludiLeadClienti As String, ByVal strWhere As String, Optional ByVal strFiltroGiorniOfferte As String = "365") As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLHIEIBUS).Assembly, "GetLeadTestOff.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@includi_lead_clienti", strIncludiLeadClienti)
            strSQL = strSQL.Replace("@gg_offerte", CStrSQL(strFiltroGiorniOfferte))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function cGetLeadSettori(ByVal strDitta As String, ByRef dttOut As System.Data.DataTable) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLHIEIBUS).Assembly, "cGetLeadSettori.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))


            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function cGetLeadFonti(ByVal strDitta As String, ByRef dttOut As System.Data.DataTable) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLHIEIBUS).Assembly, "cGetLeadFonti.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))


            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function cGetOperatori(ByVal strDitta As String, ByRef dttOut As System.Data.DataTable) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLHIEIBUS).Assembly, "cGetOperatori.sql")

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBPRC)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try

    End Function

    Public Overridable Function cGetCampagneOperatore(ByVal strDitta As String, ByRef dttOut As System.Data.DataTable) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLHIEIBUS).Assembly, "cGetCampagneOperatore.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))


            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function cGetCliforOperatoriZona(ByVal strDitta As String, ByRef dttOut As System.Data.DataTable) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLHIEIBUS).Assembly, "cGetCliforOperatoriZona.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function cGetLeadInteressi(ByVal strDitta As String, ByRef dttOut As System.Data.DataTable) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLHIEIBUS).Assembly, "cGetLeadInteressi.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))


            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overrides Function GetLeadRighOff(ByVal strDitta As String, ByRef dttOut As System.Data.DataTable, ByVal strIncludiLeadClienti As String, ByVal strWhere As String, Optional ByVal strFiltroGiorniOfferte As String = "365") As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLHIEIBUS).Assembly, "GetLeadRighOff.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@includi_lead_clienti", strIncludiLeadClienti)
            strSQL = strSQL.Replace("@gg_offerte", CStrSQL(strFiltroGiorniOfferte))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overrides Function InsertLeadNoteData(strDitta As String, t As TestataLeadsNoteExport) As Boolean

        Dim strSQL As String = ""
        Dim TestoNota As String = ""
        Dim cLead As String = ""
        Dim dttOut As New DataTable

        Dim dbConn As DbConnection = Nothing
        Dim dt1 As New DataTable
        Dim vtb_desnota As String = ""

        Try

            'CodLead = NTSCInt(t.cod_lead)

            'Dim nNumero As Integer = 0
            'nNumero = LegNuma(strDitta, "NT", " ", 0, True)
            'nNumero = AggNuma(strDitta, "NT", " ", 0, nNumero, False, False, "")
            'strSQL = "insert into tabnote ( codditt, tb_codnote, tb_desnote, tb_tiponot, tb_contonot, tb_testonot) values  ( {0}, {1}, {2}, 'S', {3}, {4} ) "
            'strSQL = String.Format(strSQL, CStrSQL(strDitta), nNumero, CStrSQL(dRow("TIPO_NOTA").ToString), CodLead, CStrSQL(dRow("NOTA").ToString))

            'Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

            'TNET_MODIMAN
            'per evitare disallineamenti tra progressivi note, INSERITE MODIFICHE con TRANSAZIONE

            dbConn = ApriDB(CLE__APP.DBTIPO.DBAZI)
            ApriTrans(dbConn)

            Dim nNumero As Integer = 0
            nNumero = LegNuma(strDitta, "NT", " ", 0, True)

            strSQL = "SELECT  TOP 1  tb_codnote" & vbCrLf & _
                        " FROM  tabnote " & vbCrLf & _
                        " WHERE  codditt = " & CStrSQL(strDitta) & vbCrLf & _
                        " ORDER BY tb_codnote DESC"

            dt1 = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI, dbConn)
            nNumero = 1
            If dt1.Rows.Count > 0 Then
                nNumero = NTSCInt(dt1.Rows(0)!tb_codnote) + 1
            End If

            vtb_desnota = NTSCStr(t.tipo_nota)
            If vtb_desnota.Trim = "" Then
                vtb_desnota = "Nota creata da IB, operatore: " & NTSCStr(t.cod_operatore).Trim & ", il: " & _
                                    NTSCDate(t.data_import).ToString("dd/MM/yyyy").ToString
                vtb_desnota = Left(vtb_desnota, 80)
            End If

            nNumero = AggNuma(strDitta, "NT", " ", 0, nNumero, False, False, "", dbConn)
            strSQL = "insert into tabnote ( codditt, tb_codnote, tb_desnote, tb_tiponot, tb_contonot, tb_testonot) values  ( {0}, {1}, {2}, 'S', {3}, {4} ) "
            strSQL = String.Format(strSQL, CStrSQL(NTSCStr(strDitta)), nNumero, CStrSQL(vtb_desnota), CStrSQL(NTSCStr(t.cod_lead)), CStrSQL(NTSCStr(t.nota)))

            Execute(strSQL, CLE__APP.DBTIPO.DBAZI, dbConn)

            ChiudiTrans()

            'TNET_FINEMOD


            'CodLead = CodLead
            Return True
        Catch ex As Exception
            AnnullaTrans()
            Throw (New NTSException(GestError(ex, Me, strSQL + "AM ID: " + t.id.ToString(), oApp.InfoError, "", False)))
        End Try


    End Function

    Public Overrides Function InsertLeadData(strDitta As String, Lead As TestataLeadsExport, ByRef CodLead As Integer) As Boolean
        Dim strSQL As String = ""

        Dim nNumero As Integer = 0
        Dim dttOut As New DataTable
        Dim CodiceLead As New Integer

        Dim codAgente As String
        Dim v_operatori_crm As DataTable = Nothing
        Dim lResult As Integer = -1
        Dim strUserCrm As String = ""
        Dim dtUserCrm As DataTable = Nothing

        Dim v_CampCod As Integer = 0, v_ListaCod As Integer = 0
        Dim dt2 As DataTable = Nothing, dt3 As DataTable = Nothing

        Dim vIndir As String = "", vRagSoc As String = "", vTelef As String = ""
        Dim dtl As DataTable = Nothing

        Dim dt1 As New DataTable
        Dim vtb_desnota As String = ""
        Dim vtb_codnote As Integer = 0

        Dim bRet As Boolean = False

        Dim lProgr As Integer = 0
        Dim strDalMessage As String = ""

        Try
            If Lead.cod_lead = "" Or Lead.cod_lead Is Nothing Then
                If Lead.cod_agente = "" Or Lead.cod_agente Is Nothing Then
                    Lead.cod_agente = "0"
                End If

                'TNET_MODIMAN
                ' Controlla se LEADS esistente sulla base di uno dei seguenti campi: IND or RAG_SOC or TEL
                ' Inoltre imposta il campo listino=0 anzichè 1
                ' nel campo note non alimenta la CAMPAGNA


                'nNumero = 0
                'nNumero = LegNuma(strDitta, "C8", " ", 0, True)
                'nNumero = AggNuma(strDitta, "C8", " ", 0, nNumero, False, False, "")

                'strSQL = _
                '"INSERT INTO leads (" & _
                '"le_codlead ," & _
                '"codditt ," & _
                '"le_zona  ," & _
                '"le_categ  ," & _
                '"le_abi  ," & _
                '"le_cab  ," & _
                '"le_conto  ," & _
                '"le_privacy  ," & _
                '"le_contattato  ," & _
                '"le_nonint  ," & _
                '"le_listino  ," & _
                '"le_agente2  ," & _
                '"le_status  ," & _
                '"le_codcana  ," & _
                '"le_agente  ," & _
                '"le_opnome  ," & _
                '"le_descr1  ," & _
                '"le_descr2  ," & _
                '"le_indir   ," & _
                '"le_cap     ," & _
                '"le_citta   ," & _
                '"le_prov    ," & _
                '"le_stato   ," & _
                '"le_note2   ," & _
                '"le_pariva  ," & _
                '"le_codfis  ," & _
                '"le_telef   ," & _
                '"le_faxtlx  ," & _
                '"le_cell    ," & _
                '"le_email   ," & _
                '"le_website ," & _
                '"le_note    )" & _
                '"VALUES (   " & _
                'CStrSQL(nNumero) & "," & _
                'CStrSQL(strDitta) & "," & _
                'CStr(0) & "," & _
                'CStr(0) & "," & _
                'CStr(0) & "," & _
                'CStr(0) & "," & _
                'CStr(0) & "," & _
                'CStrSQL("S") & "," & _
                'CStrSQL("S") & "," & _
                'CStrSQL("N") & "," & _
                'CStr(1) & "," & _
                'CStr(0) & "," & _
                'CStrSQL("P") & "," & _
                'CStr(1) & "," & _
                'ConvEmptyToNull(CStrSQL(dRow("COD_AGENTE"))) & "," & _
                'ConvEmptyToNull(CStrSQL(dRow("COD_OPERATORE"))) & "," & _
                'ConvEmptyToNull(CStrSQL(Left(dRow("DESCRIZIONE1").ToString, 60))) & "," & _
                'ConvEmptyToNull(CStrSQL(Left(dRow("DESCRIZIONE2").ToString, 50))) & "," & _
                'ConvEmptyToNull(CStrSQL(Left(dRow("INDIRIZZO").ToString, 70))) & "," & _
                'ConvEmptyToNull(CStrSQL(Left(dRow("CAP").ToString, 9))) & "," & _
                'ConvEmptyToNull(CStrSQL(Left(dRow("CITTA").ToString, 50))) & "," & _
                'ConvEmptyToNull(CStrSQL(Left(dRow("PROVINCIA").ToString, 2))) & "," & _
                'ConvEmptyToNull(CStrSQL(Left(dRow("NAZIONE").ToString, 3))) & "," & _
                'ConvEmptyToNull(CStrSQL(dRow("DES_NOTE").ToString.Replace(iBNewline, vbNewLine))) & "," & _
                'ConvEmptyToNull(CStrSQL(Left(dRow("PARTITA_IVA").ToString, 11))) & "," & _
                'ConvEmptyToNull(CStrSQL(Left(dRow("CODICE_FISCALE").ToString, 16))) & "," & _
                'ConvEmptyToNull(CStrSQL(Left(dRow("TELEFONO").ToString, 18))) & "," & _
                'ConvEmptyToNull(CStrSQL(Left(dRow("FAX").ToString, 18))) & "," & _
                'ConvEmptyToNull(CStrSQL(Left(dRow("CELLULARE").ToString, 18))) & "," & _
                'ConvEmptyToNull(CStrSQL(Left(dRow("EMAIL").ToString, 50))) & "," & _
                'ConvEmptyToNull(CStrSQL(Left(dRow("INTERNET").ToString, 50))) & "," & _
                'ConvEmptyToNull(CStrSQL(dRow("COD_CAMPAGNA"))) & _
                '")"

                'If dRow("DESCRIZIONE1").ToString.Contains("BROFIL") Then
                '    strSQL = strSQL
                'End If

                vRagSoc = ""
                vIndir = ""
                vTelef = ""

                If Not String.IsNullOrEmpty(Lead.descrizione1) Then
                    vRagSoc = Left(Lead.descrizione1, 70).Trim
                    If vRagSoc <> "" Then
                        vRagSoc = Replace(Replace(Replace(Replace(Replace(Replace(Replace(LTrim(RTrim(vRagSoc)), " ", ""), ".", ""), "-", ""), "\", ""), "/", ""), "+", ""), ",", "")
                    End If
                End If
                If Not String.IsNullOrEmpty(Lead.indirizzo) Then
                    vIndir = Left(Lead.indirizzo, 60).Trim
                    If vIndir <> "" Then
                        vIndir = Replace(Replace(Replace(Replace(Replace(Replace(Replace(LTrim(RTrim(vIndir)), " ", ""), ".", ""), "-", ""), "\", ""), "/", ""), "+", ""), ",", "")
                    End If
                End If
                If Not String.IsNullOrEmpty(Lead.telefono) Then
                    vTelef = Left(Lead.telefono, 18).Trim
                    If vTelef <> "" Then
                        vTelef = Replace(Replace(Replace(Replace(Replace(Replace(Replace(LTrim(RTrim(vTelef)), " ", ""), ".", ""), "-", ""), "\", ""), "/", ""), "+", ""), ",", "")
                    End If
                End If

                'Controlla / Verifica se esiste un lead con almeno uno dei campi sopra
                strSQL = "SELECT  TOP 1  * " & vbCrLf & _
                            " FROM  leads " & vbCrLf & _
                            " WHERE     codditt = " & CStrSQL(strDitta) & vbCrLf & _
                            "       AND ( 1=0  " & vbCrLf
                If vRagSoc.Trim <> "" Then
                    strSQL &= "     OR  Replace(replace(replace(replace(replace(replace(replace(ltrim(rtrim(le_descr1)),' ',''),'.',''),'-',''),'\',''),'/',''),'+',''),',','') = " & _
                                CStrSQL(vRagSoc) & vbCrLf
                End If
                If vIndir.Trim <> "" Then
                    strSQL &= "     OR  Replace(replace(replace(replace(replace(replace(replace(ltrim(rtrim(le_indir)),' ',''),'.',''),'-',''),'\',''),'/',''),'+',''),',','') = " & _
                                CStrSQL(vIndir) & vbCrLf
                End If
                If vTelef.Trim <> "" Then
                    strSQL &= "     OR  Replace(replace(replace(replace(replace(replace(replace(ltrim(rtrim(le_telef)),' ',''),'.',''),'-',''),'\',''),'/',''),'+',''),',','') = " & _
                                CStrSQL(vTelef) & vbCrLf
                End If
                strSQL &= "          ) " & vbCrLf

                dtl = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
                If dtl.Rows.Count > 0 Then

                    'Se LEAD esistente INSERISCE LEAD NELLA LISTA SELEZIONATA Della campagna e alimento le eventuali NOTE
                    CodLead = NTSCInt(dtl.Rows(0)!le_codlead)
                    bRet = Lead_Aggiorna_Dati_Data(strDitta, Lead, CodLead)
                    Return bRet

                End If


                'Ricerca Stato
                Dim vCodStato As String = ""

                strSQL = "SELECT  TOP 1  * " & vbCrLf & _
                            " FROM  TabStat " & vbCrLf & _
                            " WHERE     LTrim(RTrim(tb_desstat)) = " & ToSql(Lead.nazione) & vbCrLf

                dtl = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
                If dtl.Rows.Count > 0 Then
                    vCodStato = dtl.Rows(0)!tb_codstat.ToString
                End If


                nNumero = 0
                nNumero = LegNuma(strDitta, "C8", " ", 0, True)
                nNumero = AggNuma(strDitta, "C8", " ", 0, nNumero, False, False, "")

                ' Dati di default
                If String.IsNullOrEmpty(Lead.cod_agente_lead) Then
                    Lead.cod_agente_lead = "0"
                End If

                If String.IsNullOrEmpty(Lead.cod_canale_vendita) Then
                    Lead.cod_canale_vendita = "0"
                End If

                If String.IsNullOrEmpty(Lead.cod_concorrente) Then
                    Lead.cod_concorrente = "0"
                End If

                If String.IsNullOrEmpty(Lead.cod_interesse) Then
                    Lead.cod_interesse = "0"
                End If

                If String.IsNullOrEmpty(Lead.cod_categoria) Then
                    Lead.cod_categoria = "0"
                End If

                If String.IsNullOrEmpty(Lead.cod_modalita_acquisizione) Then
                    Lead.cod_modalita_acquisizione = "0"
                End If

                strSQL = _
                "INSERT INTO leads (" & _
                "le_codlead ," & _
                "codditt ," & _
                "le_zona  ," & _
                "le_abi  ," & _
                "le_cab  ," & _
                "le_conto  ," & _
                "le_privacy  ," & _
                "le_contattato  ," & _
                "le_nonint  ," & _
                "le_listino  ," & _
                  "le_agente2  ," & _
                "le_status  ," & _
                "le_opnome  ," & _
                "le_descr1  ," & _
                "le_descr2  ," & _
                "le_indir   ," & _
                "le_cap     ," & _
                "le_citta   ," & _
                "le_prov    ," & _
                "le_stato   ," & _
                "le_note2   ," & _
                "le_pariva  ," & _
                "le_codfis  ," & _
                "le_telef   ," & _
                "le_faxtlx  ," & _
                "le_cell    ," & _
                "le_email   ," & _
                "le_website ," & _
                    "le_agente  ," & _
                    "le_codcana  ," & _
                    "le_codcptr  ," & _
                    "le_categ    ," & _
                    "le_hhfonte    ," & _
                    "le_hhlivello_interesse    ," & _
                    "le_hhsettore    ," & _
                    "le_hhfatturato_raccordi    ," & _
                    "le_hhvaluta    ," & _
                    "le_codperv  )"

                'ConvEmptyToNull(CStrSQL(Left(dRow("NAZIONE").ToString, 3))) & "," & _
                strSQL &= _
                "VALUES (   " & _
                CStrSQL(nNumero) & "," & _
                CStrSQL(strDitta) & "," & _
                CStr(0) & "," & _
                CStr(0) & "," & _
                CStr(0) & "," & _
                CStr(0) & "," & _
                CStrSQL("S") & "," & _
                CStrSQL("S") & "," & _
                CStrSQL("N") & "," & _
                CStr(0) & "," & _
                CStr(0) & "," & _
                CStrSQL("P") & "," & _
                ToSql(Lead.cod_operatore) & "," & _
                ToSql(Lead.descrizione1, 60) & "," & _
                ToSql(Lead.descrizione2, 50) & "," & _
                ToSql(Lead.indirizzo, 70) & "," & _
                ToSql(Lead.cap, 9) & "," & _
                ToSql(Lead.citta, 50) & "," & _
                ToSql(Lead.provincia, 2) & "," & _
                ConvEmptyToNull(CStrSQL(vCodStato.ToString)) & "," & _
                ToSql(Lead.note) & "," & _
                ToSql(Lead.partita_iva, 11) & "," & _
                ToSql(Lead.codice_fiscale, 16) & "," & _
                ToSql(Lead.telefono, 18) & "," & _
                ToSql(Lead.fax, 18) & "," & _
                ToSql(Lead.cellulare, 18) & "," & _
                ToSql(Lead.email, 50) & "," & _
                ToSql(Lead.internet, 50) & "," & _
                    ToSql(Lead.cod_agente_lead) & "," & _
                    ToSql(Lead.cod_canale_vendita) & "," & _
                    ToSql(Lead.cod_concorrente) & "," & _
                    ToSql(Lead.cod_categoria) & "," & _
                    ToSql(Lead.cod_fonte) & "," & _
                    ToSql(Lead.cod_interesse) & "," & _
                    ToSql(Lead.cod_settore) & "," & _
                    ToSql(Lead.fatturato) & "," & _
                    ToSql(Lead.cod_valuta) & "," & _
                    ToSql(Lead.cod_modalita_acquisizione) & _
                ")"
                Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

                ' x Marco Malizia
                ' Questi campi vanno inseriti nella insert sopra

                ' Campi nuovi da aggiungere sui leads (Aggiunti sopra)
                'Lead.fatturato (aggiunto sopra)
                'Lead.cod_categoria (aggiunto sopra)
                'Lead.cod_agente_lead (aggiunto sopra)
                'Lead.cod_canale_vendita (aggiunto sopra)
                'Lead.cod_concorrente (aggiunto sopra)
                'Lead.cod_modalita_acquisizione (aggiunto sopra)
                'Lead.cod_fonte (aggiunto sopra)
                'Lead.cod_interesse (aggiunto sopra)
                'Lead.cod_settore (aggiunto sopra)


                'SE compilato il codice mansione (obbligatorio per il contatto), compila la sezione CONTATTI
                If Not String.IsNullOrEmpty(Lead.cod_mansione) Then

                    ' Campi nuovi da inserire nella organig
                    'Lead.nome
                    'Lead.cognome
                    ' Campi nuovi da agigungere nella og_codruaz
                    'Lead.cod_mansione

                    lProgr = MyBase.LegNuma(strDitta, "OG", " ", 0, True)
                    lProgr = MyBase.AggNuma(strDitta, "OG", " ", 0, lProgr, True, True, strDalMessage)

                    strSQL = _
                    "INSERT INTO Organig " & vbCrLf & _
                    "   (   codditt, og_progr, og_codlead, og_tipork, " & vbCrLf & _
                    "       og_descont, og_descont2, " & vbCrLf & _
                    "       og_codruaz, og_email " & vbCrLf & _
                    "   )   " & vbCrLf
                    strSQL = strSQL & _
                    " VALUES ( " & vbCrLf & _
                        ToSql2(strDitta) & ", " & lProgr & ", " & (nNumero) & ", 'L', " & vbCrLf & _
                        ToSql2(Lead.cognome) & ", " & ToSql2(Lead.nome) & ", " & vbCrLf & _
                        ToSql2(Lead.cod_mansione) & ", " & ToSql2(Lead.email) & " " & vbCrLf & _
                    "       )   " & vbCrLf
                    Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

                End If

                'TNET_FINEMOD


                ' Il lead e' gia' associato? (chiave: opcr_codlead, codditt, opcr_opnome)
                strSQL = String.Format("select * from acclead where codditt = {0} and opcr_codlead = {1} and opcr_opnome = {2}", _
                                       CStrSQL(strDitta), _
                                       nNumero.ToString, _
                                       ToSql(Lead.cod_operatore) _
                                       )
                dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

                If dttOut.Rows.Count = 0 Then

                    ' Inserisco i dati nella tabella accessi          
                    strSQL = String.Format("insert into acclead (codditt, opcr_opnome, opcr_codlead, opcr_crmvis, opcr_crmmod) values ({0}, {1}, {2}, 'S', 'S')", _
                                           CStrSQL(strDitta), _
                                           ToSql(Lead.cod_operatore), _
                                           CStrSQL(nNumero) _
                                           )
                    Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

                End If

                CodLead = nNumero


                'TNET_MODIMAN
                'Azioni aggiuntive al NUOVO LEADS
                v_CampCod = NTSCInt(Lead.cod_campagna)

                If Lead.cod_operatore.Trim.ToLower.Contains("gloria") Then
                    codAgente = "999"
                Else
                    codAgente = NTSCInt(Lead.cod_agente).ToString
                    If NTSCInt(codAgente) = 0 Then
                        'se non trova l'agente impostato, mette colombo
                        codAgente = "12"
                    End If

                End If

                'se presente campagna mette status=N, altrimenti status=P
                If v_CampCod <> 0 Then
                    strSQL = " Update leads set " & vbCrLf & _
                                            "   le_hhcampagna_origine = {0}, " & vbCrLf & _
                                            "   le_agente = {3}, " & vbCrLf & _
                                            "   le_status = 'N' " & vbCrLf & _
                                            " WHERE codditt = {1} and le_codlead = {2}" & vbCrLf
                Else
                    strSQL = " Update leads set " & vbCrLf & _
                                            "   le_hhcampagna_origine = {0}, " & vbCrLf & _
                                            "   le_agente = {3}, " & vbCrLf & _
                                            "   le_status = 'P' " & vbCrLf & _
                                            " WHERE codditt = {1} and le_codlead = {2}" & vbCrLf
                End If

                strSQL = String.Format(strSQL, _
                                            CStrSQL(v_CampCod), _
                                            CStrSQL(strDitta), _
                                            CodLead, _
                                            codAgente)
                Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

                'TNET_PERSONALIZZAZIONI_NTS_INIZIO
                'sposto le note inserite da IB da note2 a tabnote
                strSQL = "UPDATE leads SET le_note2=' ' WHERE codditt=" & CStrSQL(strDitta) & " AND le_codlead=" & CodLead
                Execute(strSQL, CLE__APP.DBTIPO.DBAZI)


                '----------------- TabNote
                'legge e imposta il progr note
                strSQL = "SELECT  TOP 1  tb_codnote" & vbCrLf & _
                        " FROM  tabnote " & vbCrLf & _
                        " WHERE  codditt = " & CStrSQL(strDitta) & vbCrLf & _
                        " ORDER BY tb_codnote DESC"
                dt1 = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
                vtb_codnote = 1
                If dt1.Rows.Count > 0 Then
                    vtb_codnote = NTSCInt(dt1.Rows(0)!tb_codnote) + 1
                End If

                vtb_desnota = "Nota creata da IB, operatore: " & Lead.cod_operatore.ToString.Trim & ", il: " & _
                                        Now.ToString("dd/MM/yyyy")
                vtb_desnota = Left(vtb_desnota, 80)

                strSQL = "INSERT INTO tabnote (codditt,tb_codnote,tb_desnote,tb_tiponot,tb_contonot,tb_testonot,tb_hhdata) " & _
                        "SELECT " & CStrSQL(strDitta) & "," & _
                            vtb_codnote & " ," & _
                            CStrSQL(vtb_desnota) & "," & _
                            "'S'," & _
                            CodLead & "," & _
                            CStrSQL(NTSCStr(Lead.note)) & "," & _
                            "GETDATE() "
                Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
                vtb_codnote = AggNuma(strDitta, "NT", " ", 0, vtb_codnote, False, False, "")
                '--------------


                'se posso collego il lead all'agente, diversamente all'utente standard (default 'ADMIN')
                strUserCrm = GetSettingBus("BS--CLIE", "OPZIONI", ".", "UtentePredefinitoCrm", "Admin", " ", "Admin") 'default
                If NTSCInt(codAgente) <> 0 Then
                    strSQL = "SELECT * FROM accdito WHERE codditt = " & CStrSQL(strDitta) & _
                                " AND opdi_codcage = " & NTSCInt(codAgente)
                    dtUserCrm = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
                    If dtUserCrm.Rows.Count > 0 Then
                        strUserCrm = NTSCStr(dtUserCrm.Rows(0)!opdi_opnome).Trim
                    End If
                End If

                'strSQL = "SELECT * " & _
                '            "FROM operat " & _
                '            "WHERE opiscrmus = 'S' " & _
                '                " AND OpNome <>	" & CStrSQL(strUserCrm)
                'v_operatori_crm = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBPRC)
                strSQL = "SELECT * " & _
                            "FROM operat " & _
                            "WHERE opiscrmus = 'S' " & _
                                " AND OpNome <>	" & CStrSQL(Lead.cod_operatore)
                v_operatori_crm = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBPRC)

                For Each v_operatore As DataRow In v_operatori_crm.Rows
                    strSQL = "INSERT INTO acclead (codditt, opcr_opnome, opcr_codlead, opcr_crmvis, opcr_crmmod)" & _
                            " VALUES (" & CStrSQL(strDitta) & ", " & CStrSQL(v_operatore!Opnome.ToString) & ", " & _
                            CodLead & ", " & CStrSQL("S") & ", " & _
                            CStrSQL("S") & " )"
                    lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
                    If lResult = -1 Then
                        Throw New Exception("Si è verificato un errore in inserimento acclead altri operatori da IB")
                        AnnullaTrans()
                        Return False
                    End If
                Next


                'Legge Lista Selezionata associata alla campagna
                v_CampCod = NTSCInt(Lead.cod_campagna)
                If v_CampCod <> 0 Then


                    strSQL = "SELECT * " & vbCrLf & _
                                "FROM tabcamp " & vbCrLf & _
                                " WHERE     codditt=" & CStrSQL(strDitta) & vbCrLf & _
                                "       AND tb_codcamp = " & v_CampCod & vbCrLf
                    dt2 = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
                    If dt2.Rows.Count > 0 Then
                        v_ListaCod = NTSCInt(dt2.Rows(0)!tb_hhcodlista)
                        If v_ListaCod <> 0 Then
                            strSQL = "SELECT * " & vbCrLf & _
                                        "FROM tablsel " & vbCrLf & _
                                        " WHERE     codditt=" & CStrSQL(strDitta) & vbCrLf & _
                                        "       AND tb_codlsel = " & v_ListaCod & vbCrLf
                            dt3 = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
                            If dt3.Rows.Count > 0 Then
                                'aggiunge nuovo Lead al dettaglio lista

                                strSQL = "INSERT INTO  listsel  " & vbCrLf & _
                                        "   (   codditt, lse_codlsel, lse_codlead, lse_tipolc, lse_note ) " & vbCrLf & _
                                        " VALUES " & vbCrLf & _
                                        "   (   " & CStrSQL(strDitta) & "," & vbCrLf & _
                                                v_ListaCod & "," & vbCrLf & _
                                                CodLead & "," & vbCrLf & _
                                                "'L', " & vbCrLf & _
                                                CStrSQL(NTSCStr(Lead.note)) & vbCrLf & _
                                        "   )   " & vbCrLf
                                Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

                            End If
                        End If
                    End If
                End If




                'TNET_FINEMOD
            Else
                Return False
            End If


            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL + "AM Lead ID: " + Lead.id.ToString(), oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function Lead_Aggiorna_Dati_Data(ByVal strDitta As String, Lead As TestataLeadsExport, ByRef CodLead As Integer) As Boolean
        'Dato 1 LEAD, aggiorna i dati del LEAD, NOTE, CAMPAGNA - LISTA-SELEZIONATA
        Dim strSQL As String = ""

        Dim nNumero As Integer = 0
        Dim dttOut As New DataTable
        Dim CodiceLead As New Integer

        Dim codAgente As String
        Dim v_operatori_crm As DataTable = Nothing
        Dim lResult As Integer = -1
        Dim strUserCrm As String = ""
        Dim dtUserCrm As DataTable = Nothing

        Dim v_CampCod As Integer = 0, v_ListaCod As Integer = 0
        Dim dt2 As DataTable = Nothing, dt3 As DataTable = Nothing

        Dim dtl As DataTable = Nothing

        Dim dt1 As New DataTable
        Dim vtb_desnota As String = ""
        Dim vtb_codnote As Integer = 0


        Try


            '' Il lead e' gia' associato? (chiave: opcr_codlead, codditt, opcr_opnome)
            'strSQL = String.Format("select * from acclead where codditt = {0} and opcr_codlead = {1} and opcr_opnome = {2}", _
            '                       CStrSQL(strDitta), _
            '                       CodLead.ToString, _
            '                       CStrSQL(dRow("COD_OPERATORE")) _
            '                       )
            'dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            'If dttOut.Rows.Count = 0 Then

            '    ' Inserisco i dati nella tabella accessi          
            '    strSQL = String.Format("insert into acclead (codditt, opcr_opnome, opcr_codlead, opcr_crmvis, opcr_crmmod) values ({0}, {1}, {2}, 'S', 'S')", _
            '                           CStrSQL(strDitta), _
            '                           CStrSQL(dRow("COD_OPERATORE")), _
            '                           CStrSQL(CodLead) _
            '                           )
            '    Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

            'End If

            'Azioni aggiuntive al NUOVO LEADS
            v_CampCod = NTSCInt(Lead.cod_campagna)

            '----------------- Alimenta le note su TabNote
            'legge e imposta il progr note
            strSQL = "SELECT  TOP 1  tb_codnote" & vbCrLf & _
                    " FROM  tabnote " & vbCrLf & _
                    " WHERE  codditt = " & CStrSQL(strDitta) & vbCrLf & _
                    " ORDER BY tb_codnote DESC"
            dt1 = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
            vtb_codnote = 1
            If dt1.Rows.Count > 0 Then
                vtb_codnote = NTSCInt(dt1.Rows(0)!tb_codnote) + 1
            End If

            vtb_desnota = "Nota creata da IB, operatore: " & Lead.cod_operatore.Trim & ", il: " & _
                                    Now.ToString("dd/MM/yyyy")
            vtb_desnota = Left(vtb_desnota, 80)

            strSQL = "INSERT INTO tabnote (codditt,tb_codnote,tb_desnote,tb_tiponot,tb_contonot,tb_testonot,tb_hhdata) " & _
                    "SELECT " & CStrSQL(strDitta) & "," & _
                        vtb_codnote & " ," & _
                        CStrSQL(vtb_desnota) & "," & _
                        "'S'," & _
                        CodLead & "," & _
                        CStrSQL(NTSCStr(Lead.note)) & "," & _
                        "GETDATE() "
            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
            vtb_codnote = AggNuma(strDitta, "NT", " ", 0, vtb_codnote, False, False, "")
            '--------------


            'Legge Lista Selezionata associata alla campagna
            v_CampCod = NTSCInt(Lead.cod_campagna)
            If v_CampCod <> 0 Then


                strSQL = "SELECT * " & vbCrLf & _
                            "FROM tabcamp " & vbCrLf & _
                            " WHERE     codditt=" & CStrSQL(strDitta) & vbCrLf & _
                            "       AND tb_codcamp = " & v_CampCod & vbCrLf
                dt2 = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
                If dt2.Rows.Count > 0 Then
                    v_ListaCod = NTSCInt(dt2.Rows(0)!tb_hhcodlista)
                    If v_ListaCod <> 0 Then
                        strSQL = "SELECT * " & vbCrLf & _
                                    "FROM tablsel " & vbCrLf & _
                                    " WHERE     codditt=" & CStrSQL(strDitta) & vbCrLf & _
                                    "       AND tb_codlsel = " & v_ListaCod & vbCrLf
                        dt3 = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
                        If dt3.Rows.Count > 0 Then
                            'aggiunge nuovo Lead al dettaglio lista

                            'controlla se Lead già esistente su Lista_Selezionata, 
                            '   altrimenti lo aggiunge
                            strSQL = "SELECT * " & vbCrLf & _
                                    "FROM listsel " & vbCrLf & _
                                    " WHERE     codditt=" & CStrSQL(strDitta) & vbCrLf & _
                                    "       AND lse_codlsel = " & v_ListaCod & vbCrLf & _
                                    "       AND lse_codlead= " & CodLead & vbCrLf
                            dtl = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
                            If dtl.Rows.Count > 0 Then

                                'Aggiorna NOTE su LIST
                                strSQL = "UPDATE listsel  SET  " & vbCrLf & _
                                        "       lse_note = " & CStrSQL(NTSCStr(Lead.note)) & vbCrLf & _
                                        " WHERE " & vbCrLf & _
                                        "           codditt = " & CStrSQL(strDitta) & vbCrLf & _
                                        "       AND lse_codlsel = " & v_ListaCod & vbCrLf & _
                                        "       AND lse_codlead = " & CodLead & vbCrLf
                                Execute(strSQL, CLE__APP.DBTIPO.DBAZI)


                            Else

                                'Aggiunge LEAD su Lista Selezionata
                                strSQL = "INSERT INTO  listsel  " & vbCrLf & _
                                        "   (   codditt, lse_codlsel, lse_codlead, lse_tipolc, lse_note ) " & vbCrLf & _
                                        " VALUES " & vbCrLf & _
                                        "   (   " & CStrSQL(strDitta) & "," & vbCrLf & _
                                                v_ListaCod & "," & vbCrLf & _
                                                CodLead & "," & vbCrLf & _
                                                "'L', " & vbCrLf & _
                                                CStrSQL(NTSCStr(Lead.note)) & vbCrLf & _
                                        "   )   " & vbCrLf
                                Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

                            End If

                        End If
                    End If
                End If
            End If






            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function cGetLeadSconti(ByVal strDitta As String, ByRef dttOut As DataTable) As Boolean
        Dim strSQL As String = ""
        Try
            ' test
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLHIEIBUS).Assembly, "cGetLeadSconti.sql")

            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function cGetCustomFields1(ByVal strDitta As String, ByRef dttOut As DataTable) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLHIEIBUS).Assembly, "cGetCustomFields1.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overrides Function GetLeadNote(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean

        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLHIEIBUS).Assembly, "GetLeadNote.sql")

            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try


    End Function

    'Trasforma NULL in stringa vuota
    Public Overridable Function ToSql2(ByVal strDesc As String, Optional ByVal MaxLength As Integer = 0) As String
        Dim RetVal As String

        Try

            If strDesc Is Nothing Or strDesc = "" Then
                RetVal = ""
            Else
                RetVal = strDesc
            End If

            If MaxLength > 0 And Len(RetVal) >= MaxLength Then
                RetVal = Left(RetVal, MaxLength)
            End If

            'RetVal = ConvEmptyToNull(CStrSQL(RetVal))
            RetVal = (CStrSQL(RetVal))


        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strDesc, oApp.InfoError, "", False)))
        End Try
        Return RetVal
    End Function

End Class

