Imports NTSInformatica.CLN__STD
Imports System.Data.Common
Imports NTSInformatica
Imports System.IO
Imports AMHelper.WS


Public Class CLDIEIBUS
    Inherits CLD__BASE

    Public Const iBNewline As String = "§"

    Public Overridable Function AddWhereCondition(ByRef sqlString As String, ByVal whereCondition As String) As Boolean
        Dim strSQL As String = ""
        Try

            ' Tolgo gli spazi all'inizio
            whereCondition = LTrim(whereCondition)

            ' Se e' stato messo l'and all'inizio, lo tolgo
            If whereCondition.Length > 3 And UCase(Left(whereCondition, 3)) = "AND" Then
                whereCondition = LTrim(Mid(whereCondition, 4))
            End If

            sqlString = Replace(sqlString, "WHERE", "WHERE " & whereCondition & " AND ", Compare:=Microsoft.VisualBasic.CompareMethod.Text)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function ToSql(ByVal strDesc As String) As String
        Dim RetVal As String

        Try

            If strDesc Is Nothing Then
                strDesc = ""
            End If

            RetVal = ConvEmptyToNull(CStrSQL(strDesc))


        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strDesc, oApp.InfoError, "", False)))
        End Try
        Return RetVal
    End Function

    Public Overridable Function ConvEmptyToNull(ByVal strDesc As String) As String
        Dim RetVal As String

        Try

            If strDesc = "''" Then
                RetVal = "null"
            Else
                RetVal = strDesc
            End If

        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strDesc, oApp.InfoError, "", False)))
        End Try
        Return RetVal
    End Function


    Public Overridable Function CheckTabNote(ByVal strDitta As String, ByVal strConto As String, ByVal strProgressivo As String) As Boolean
        Dim strSQL As String = ""
        Dim dttOut As New DataTable
        Try
            strSQL = "select * from tabnote" & _
                     " WHERE codditt=" & CStrSQL(strDitta) & _
                     " AND tb_codnote =" & strProgressivo & _
                     " AND tb_contonot =" & strConto

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            If dttOut.Rows.Count > 0 Then
                Return True
            End If

        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function GetCustomData(ByVal strDitta As String, ByVal key As String, ByVal defvalue As String) As String
        Dim strSQL As String = ""
        Dim RetValue As String = "1"

        Dim dttOut As New DataTable
        Try

            strSQL = "select value from hh_ibdata where codditt = @ditta and [key] = @key"
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@key", CStrSQL(key))

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            If dttOut.Rows.Count = 0 Then
                Execute(String.Format("insert into hh_ibdata ([key], [value], [codditt]) values ({0}, {1}, {2})", CStrSQL(key), CStrSQL(defvalue), CStrSQL(strDitta)), CLE__APP.DBTIPO.DBAZI)
                RetValue = defvalue
            Else
                RetValue = dttOut.Rows(0)!value.ToString
            End If

            Return RetValue

        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function SetCustomData(ByVal strDitta As String, ByVal key As String, ByVal value As String) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = String.Format("update hh_ibdata set value = {0} where codditt = @ditta and [key] = {1}", CStrSQL(value), CStrSQL(key))
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function



    Public Overridable Function GetAgentiCliente(ByVal strDitta As String, ByVal strConto As String, ByRef codAgente1 As Integer, ByRef codAgente2 As Integer, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Dim dttOut As New DataTable
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetAgentiCliente.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@conto", CStrSQL(strConto))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            If dttOut.Rows.Count = 0 Then
                Return False
            Else
                codAgente1 = NTSCInt(dttOut.Rows(0)!an_agente)
                codAgente2 = NTSCInt(dttOut.Rows(0)!an_agente2)
                Return True
            End If

        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function


    Public Overridable Function FindArtUltCost(ByVal strDitta As String, ByVal codArticolo As String, ByRef UltCost As Decimal) As Boolean
        Dim strSQL As String = ""
        Dim dttOut As New DataTable
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "FindArtUltCost.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@codart", CStrSQL(codArticolo))


            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            If dttOut.Rows.Count = 0 Then
                Return False
            Else
                UltCost = NTSCDec(dttOut.Rows(0)!ultcos)
                Return True
            End If

        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function UpdateCliforTabNote(ByVal strDitta As String, ByVal strConto As String, ByVal strProgressivo As String, _
                                              ByVal strTitolo As String, ByVal strNote As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = "UPDATE tabnote" & _
                     " SET tb_desnote=" & CStrSQL(strTitolo.Replace(iBNewline, vbNewLine)) & "," & _
                     " tb_testonot=" & CStrSQL(strNote.Replace(iBNewline, vbNewLine)) & "," & _
                     " WHERE codditt=" & CStrSQL(strDitta) & _
                     " AND tb_codnote =" & strProgressivo & _
                     " AND tb_contonot =" & strConto
            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function
    Public Overridable Function InsertCliforTabNote(ByVal strDitta As String, ByVal strConto As String, ByVal strProgressivo As String, _
                                              ByVal strTitolo As String, ByVal strNote As String) As Boolean
        Dim strSQL As String = ""
        Try
            Dim nNumero As Integer = 0
            nNumero = LegNuma(strDitta, "NT", " ", 0, True)
            nNumero = AggNuma(strDitta, "NT", " ", 0, nNumero, False, False, "")

            strSQL = String.Format( _
                          "insert into tabnote (codditt, tb_codnote, tb_desnote, tb_tiponot, tb_contonot, tb_testonot) values ({0}, {1}, {2}, {3}, {4}, {5})", _
                                      CStrSQL(strDitta), _
                                      nNumero, _
                                      CStrSQL(strTitolo.Replace(iBNewline, vbNewLine)), _
                                      CStrSQL("S"), _
                                      strConto, _
                                      CStrSQL(strNote.Replace(iBNewline, vbNewLine)) _
                                  )

            'strSQL = "insert into tabnote (codditt, tb_codnote, tb_desnote, tb_tiponot, tb_contonot, tb_testonot)" & _
            '  " VALUES  ( " & CStrSQL(strDitta) & ", " & nNumero & ", " & CStrSQL(strTitolo.Replace(iBNewline, vbNewLine)) & ", 'S', " & _
            '  strConto & ", " & _
            '  CStrSQL(strNote.Replace(iBNewline, vbNewLine)) & _
            '  " ) "

            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function UpdateCliforNote(ByVal strDitta As String, ByVal strConto As String, ByVal strNote As String, ByVal strTitolo As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = "UPDATE anagra" & _
                     " SET an_note2=" & CStrSQL(strNote.Replace(iBNewline, vbNewLine)) & "," & _
                     " an_opnome=" & CStrSQL(Left(oApp.User.Nome, 20)) & "," & _
                     " an_ultagg=" & CDataOraSQL(Now) & _
                     " WHERE codditt=" & CStrSQL(strDitta) & _
                     " AND an_conto =" & strConto

            ' " an_note=" & CStrSQL(strTitolo.Replace(iBNewline, vbNewLine)) & "," & _

            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function


    Public Overridable Function UpdateCliforCoordinate(ByVal strDitta As String, ByVal strConto As String, ByVal dLatitudine As Double, ByVal dLongitudine As Double) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = "UPDATE anagra" & _
                     " SET an_hhlat_ib=" & CDblSQL(dLatitudine.ToString()) & "," & _
                     " an_hhlon_ib=" & CDblSQL(dLongitudine.ToString()) & _
                     " WHERE codditt=" & CStrSQL(strDitta) & _
                     " AND an_conto =" & strConto
            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function UpdateLead(ByVal strDitta As String, ByVal dRow As DataRow, ByRef CodLead As Integer) As Boolean
        Dim strSQL As String = ""
        Dim nNumero As Integer = 0
        Dim dttOut As New DataTable
        Dim CodiceLead As New Integer

        Try

            If dRow("cod_lead").ToString <> "" Then
                CodiceLead = CInt(dRow("COD_LEAD"))

                ' Verifico se il codice lead esiste sul db. Se esiste devo fare un update, altrimenti una insert
                strSQL = "select le_codlead from leads where codditt = @ditta and le_codlead = @codlead"
                strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
                strSQL = strSQL.Replace("@codlead", CodiceLead.ToString)

                dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

                ' Se sul db c'è un record nel db posso fare l'update , altrimenti faccio l'insert
                If dttOut.Rows.Count = 1 Then
                    strSQL = _
                    "UPDATE leads set " & _
                    " le_descr1 = " & ConvEmptyToNull(CStrSQL(Left(dRow("DESCRIZIONE1").ToString, 60))) & "," & _
                    " le_descr2 = " & ConvEmptyToNull(CStrSQL(Left(dRow("DESCRIZIONE2").ToString, 50))) & "," & _
                    " le_indir  = " & ConvEmptyToNull(CStrSQL(Left(dRow("INDIRIZZO").ToString, 70))) & "," & _
                    " le_cap    = " & ConvEmptyToNull(CStrSQL(Left(dRow("CAP").ToString, 9))) & "," & _
                    " le_citta  = " & ConvEmptyToNull(CStrSQL(Left(dRow("CITTA").ToString, 50))) & "," & _
                    " le_prov   = " & ConvEmptyToNull(CStrSQL(Left(dRow("PROVINCIA").ToString, 2))) & "," & _
                    " le_stato  = " & ConvEmptyToNull(CStrSQL(Left(dRow("NAZIONE").ToString, 3))) & "," & _
                    " le_note2  = " & ConvEmptyToNull(CStrSQL(dRow("DES_NOTE").ToString.Replace(iBNewline, vbNewLine))) & "," & _
                    " le_pariva = " & ConvEmptyToNull(CStrSQL(Left(dRow("PARTITA_IVA").ToString, 11))) & "," & _
                    " le_codfis = " & ConvEmptyToNull(CStrSQL(Left(dRow("CODICE_FISCALE").ToString, 16))) & "," & _
                    " le_telef  = " & ConvEmptyToNull(CStrSQL(Left(dRow("TELEFONO").ToString, 18))) & "," & _
                    " le_faxtlx = " & ConvEmptyToNull(CStrSQL(Left(dRow("FAX").ToString, 18))) & "," & _
                    " le_cell   = " & ConvEmptyToNull(CStrSQL(Left(dRow("CELLULARE").ToString, 18))) & "," & _
                    " le_email  = " & ConvEmptyToNull(CStrSQL(Left(dRow("EMAIL").ToString, 50))) & "," & _
                    " le_website= " & ConvEmptyToNull(CStrSQL(Left(dRow("INTERNET").ToString, 50))) & "," & _
                    " le_note   = " & ConvEmptyToNull(CStrSQL(Left(dRow("COD_CAMPAGNA").ToString.Replace(iBNewline, vbNewLine), 50))) & _
                    " where codditt = " & CStrSQL(strDitta) & " and le_codlead= " & CStrSQL(CodiceLead)

                    Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

                    CodLead = CodiceLead
                Else
                    Return False
                End If
            End If

            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function InsertCli(ByVal strDitta As String, ByVal ClienteData As Clienti, ByVal Mastro As Integer, ByRef CodCliente As Integer) As Boolean

        Dim nNumero As Integer = 0
        Dim dttOut As New DataTable
        Dim CodiceCliente As New Integer
        Dim strSQL As String = ""

        Try
            nNumero = 0
            nNumero = LegNuma(strDitta, "CC", " ", Mastro, True)
            nNumero = AggNuma(strDitta, "CC", " ", Mastro, nNumero, False, False, "")

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "InsertCliente.sql")
            strSQL = strSQL.Replace("@ditta", ToSql(strDitta))
            strSQL = strSQL.Replace("@an_conto", CStrSQL(nNumero))
            strSQL = strSQL.Replace("@an_codmast", CStrSQL(Mastro))
            strSQL = strSQL.Replace("@cap", ToSql(ClienteData.cap))
            strSQL = strSQL.Replace("@cellulare", ToSql(ClienteData.cellulare))
            strSQL = strSQL.Replace("@cod_canale_vendita", ToSql(ClienteData.cod_canale_vendita))
            strSQL = strSQL.Replace("@cod_categoria", ToSql(ClienteData.cod_categoria))
            strSQL = strSQL.Replace("@cod_citta", ToSql(ClienteData.cod_citta))
            strSQL = strSQL.Replace("@cod_classe_sconto", ToSql(ClienteData.cod_classe_sconto))
            strSQL = strSQL.Replace("@cod_cond_pag", ToSql(ClienteData.cod_cond_pag))
            strSQL = strSQL.Replace("@cod_nazione", ToSql(ClienteData.cod_nazione))
            strSQL = strSQL.Replace("@cod_porto_sped", ToSql(ClienteData.cod_porto_sped))
            strSQL = strSQL.Replace("@codice_fiscale", ToSql(ClienteData.codice_fiscale))
            strSQL = strSQL.Replace("@des_cond_pag", ToSql(ClienteData.des_cond_pag))
            strSQL = strSQL.Replace("@email", ToSql(ClienteData.email))
            strSQL = strSQL.Replace("@fax", ToSql(ClienteData.fax))
            strSQL = strSQL.Replace("@iban", ToSql(ClienteData.iban))
            strSQL = strSQL.Replace("@indirizzo", ToSql(ClienteData.indirizzo))
            strSQL = strSQL.Replace("@titolo_note", ToSql("Cliente inserito ad agente"))
            strSQL = strSQL.Replace("@note", ToSql(ClienteData.note))
            strSQL = strSQL.Replace("@partita_iva", ToSql(ClienteData.partita_iva))
            strSQL = strSQL.Replace("@provincia", ToSql(ClienteData.provincia))
            strSQL = strSQL.Replace("@ragione_sociale", ToSql(ClienteData.ragione_sociale))
            strSQL = strSQL.Replace("@telefono", ToSql(ClienteData.telefono))

            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

#If False Then

            ' Il lead e' gia' associato? (chiave: opcr_codlead, codditt, opcr_opnome)
            strSQL = String.Format("select * from acclead where codditt = {0} and opcr_codlead = {1} and opcr_opnome = {2}", _
                                   CStrSQL(strDitta), _
                                   CodiceLead.ToString, _
                                   CStrSQL(dRow("COD_OPERATORE")) _
                                   )
            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            If dttOut.Rows.Count = 0 Then

                ' Inserisco i dati nella tabella accessi          
                strSQL = String.Format("insert into acclead (codditt, opcr_opnome, opcr_codlead, opcr_crmvis, opcr_crmmod) values ({0}, {1}, {2}, 'S', 'S')", _
                                       CStrSQL(strDitta), _
                                       CStrSQL(dRow("COD_OPERATORE")), _
                                       CStrSQL(nNumero) _
                                       )
                Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

            End If
#End If

            CodCliente = nNumero



            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function InsertLead(ByVal strDitta As String, ByVal dRow As DataRow, ByRef CodLead As Integer) As Boolean
        Dim strSQL As String = ""

        Dim nNumero As Integer = 0
        Dim dttOut As New DataTable
        Dim CodiceLead As New Integer

        Try
            If dRow("COD_LEAD").ToString = "" Then
                If dRow("COD_AGENTE").ToString = "" Then
                    dRow("COD_AGENTE") = "0"
                End If

                nNumero = 0
                nNumero = LegNuma(strDitta, "C8", " ", 0, True)
                nNumero = AggNuma(strDitta, "C8", " ", 0, nNumero, False, False, "")

                strSQL = _
                "INSERT INTO leads (" & _
                "le_codlead ," & _
                "codditt ," & _
                "le_zona  ," & _
                "le_categ  ," & _
                "le_abi  ," & _
                "le_cab  ," & _
                "le_conto  ," & _
                "le_privacy  ," & _
                "le_contattato  ," & _
                "le_nonint  ," & _
                "le_listino  ," & _
                "le_agente2  ," & _
                "le_status  ," & _
                "le_codcana  ," & _
                "le_agente  ," & _
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
                "le_note    )" & _
                "VALUES (   " & _
                CStrSQL(nNumero) & "," & _
                CStrSQL(strDitta) & "," & _
                CStr(0) & "," & _
                CStr(0) & "," & _
                CStr(0) & "," & _
                CStr(0) & "," & _
                CStr(0) & "," & _
                CStrSQL("S") & "," & _
                CStrSQL("S") & "," & _
                CStrSQL("N") & "," & _
                CStr(1) & "," & _
                CStr(0) & "," & _
                CStrSQL("P") & "," & _
                CStr(1) & "," & _
                ConvEmptyToNull(CStrSQL(dRow("COD_AGENTE"))) & "," & _
                ConvEmptyToNull(CStrSQL(dRow("COD_OPERATORE"))) & "," & _
                ConvEmptyToNull(CStrSQL(Left(dRow("DESCRIZIONE1").ToString, 60))) & "," & _
                ConvEmptyToNull(CStrSQL(Left(dRow("DESCRIZIONE2").ToString, 50))) & "," & _
                ConvEmptyToNull(CStrSQL(Left(dRow("INDIRIZZO").ToString, 70))) & "," & _
                ConvEmptyToNull(CStrSQL(Left(dRow("CAP").ToString, 9))) & "," & _
                ConvEmptyToNull(CStrSQL(Left(dRow("CITTA").ToString, 50))) & "," & _
                ConvEmptyToNull(CStrSQL(Left(dRow("PROVINCIA").ToString, 2))) & "," & _
                ConvEmptyToNull(CStrSQL(Left(dRow("NAZIONE").ToString, 3))) & "," & _
                ConvEmptyToNull(CStrSQL(dRow("DES_NOTE").ToString.Replace(iBNewline, vbNewLine))) & "," & _
                ConvEmptyToNull(CStrSQL(Left(dRow("PARTITA_IVA").ToString, 11))) & "," & _
                ConvEmptyToNull(CStrSQL(Left(dRow("CODICE_FISCALE").ToString, 16))) & "," & _
                ConvEmptyToNull(CStrSQL(Left(dRow("TELEFONO").ToString, 18))) & "," & _
                ConvEmptyToNull(CStrSQL(Left(dRow("FAX").ToString, 18))) & "," & _
                ConvEmptyToNull(CStrSQL(Left(dRow("CELLULARE").ToString, 18))) & "," & _
                ConvEmptyToNull(CStrSQL(Left(dRow("EMAIL").ToString, 50))) & "," & _
                ConvEmptyToNull(CStrSQL(Left(dRow("INTERNET").ToString, 50))) & "," & _
                ConvEmptyToNull(CStrSQL(dRow("COD_CAMPAGNA"))) & _
                ")"

                Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

                ' Il lead e' gia' associato? (chiave: opcr_codlead, codditt, opcr_opnome)
                strSQL = String.Format("select * from acclead where codditt = {0} and opcr_codlead = {1} and opcr_opnome = {2}", _
                                       CStrSQL(strDitta), _
                                       CodiceLead.ToString, _
                                       CStrSQL(dRow("COD_OPERATORE")) _
                                       )
                dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

                If dttOut.Rows.Count = 0 Then

                    ' Inserisco i dati nella tabella accessi          
                    strSQL = String.Format("insert into acclead (codditt, opcr_opnome, opcr_codlead, opcr_crmvis, opcr_crmmod) values ({0}, {1}, {2}, 'S', 'S')", _
                                           CStrSQL(strDitta), _
                                           CStrSQL(dRow("COD_OPERATORE")), _
                                           CStrSQL(nNumero) _
                                           )
                    Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

                End If

                CodLead = nNumero
            Else
                Return False
            End If


            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function InsertLeadNote(ByVal strDitta As String, ByVal dRow As DataRow, ByRef CodLead As Integer) As Boolean

        Dim strSQL As String = ""
        Dim TestoNota As String = ""
        Dim cLead As String = ""
        Dim dttOut As New DataTable

        Try
            cLead = dRow("COD_LEAD").ToString


            strSQL = String.Format("select le_note2 from leads where codditt = {0} and le_codlead = {1}", CStrSQL(strDitta), cLead)
            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
            If dttOut.Rows.Count = 1 Then
                TestoNota = String.Format("{0}{1}{1}***Nota {4} del {2}***{1}{3}", _
                                          dttOut.Rows(0)("le_note2"), _
                                          vbCrLf, _
                                          dRow("DAT_INSERT").ToString, _
                                          dRow("NOTA").ToString, _
                                          dRow("COD_OPERATORE").ToString _
                                          )
            End If

            strSQL = String.Format("update leads set le_note2 = {0} where codditt = {1} and le_codlead = {2}", _
                                   CStrSQL(TestoNota.Replace(iBNewline, vbNewLine)), _
                                   CStrSQL(strDitta), _
                                   cLead)

            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

            CodLead = CInt(cLead)

            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function UpdateClifor(ByVal strDitta As String, ByVal strConto As String, ByVal strDati As String()) As Boolean
        Dim strSQL As String = ""

        Const posTelefono1 As Integer = 3
        'Const posTelefono2 As Integer = 4
        Const posFax As Integer = 5
        Const posCell As Integer = 6
        Const posEmail As Integer = 7
        Const posPiva As Integer = 8
        Const posRagioneSociale As Integer = 9
        Const posAddress As Integer = 10
        Const posCap As Integer = 11
        Const posCity As Integer = 12
        Const posProvincia As Integer = 13
        Const posCodicePagamento As Integer = 14
        Const posCodiceFiscale As Integer = 15

        Try

            strSQL = "UPDATE anagra" & _
                    " SET an_telef=" & CStrSQL(Left(strDati(posTelefono1), 18)) & "," & _
                    " an_faxtlx=" & CStrSQL(Left(strDati(posFax), 18)) & "," & _
                    " an_cell=" & CStrSQL(Left(strDati(posCell), 18)) & "," & _
                    " an_email=" & CStrSQL(Left(strDati(posEmail), 50)) & "," & _
                    " an_pariva=" & CStrSQL(Left(strDati(posPiva), 11)) & "," & _
                    " an_descr1=" & CStrSQL(Left(strDati(posRagioneSociale), 50)) & "," & _
                    " an_indir=" & CStrSQL(Left(strDati(posAddress), 70)) & "," & _
                    " an_cap=" & CStrSQL(Left(strDati(posCap), 5)) & "," & _
                    " an_citta=" & CStrSQL(Left(strDati(posCity), 50)) & "," & _
                    " an_prov=" & CStrSQL(Left(strDati(posProvincia), 2)) & "," & _
                    " an_codpag=" & NTSCInt(strDati(posCodicePagamento)) & "," & _
                    " an_codfis=" & CStrSQL(Left(strDati(posCodiceFiscale), 16)) & "," & _
                    " an_opnome=" & CStrSQL(Left(oApp.User.Nome, 20)) & "," & _
                    " an_ultagg=" & CDataOraSQL(Now) & _
                    " WHERE codditt=" & CStrSQL(strDitta) & _
                    " AND an_conto =" & strConto

            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function
    Public Overridable Function AggiungiPersonalizzazioniDb() As Boolean

        Dim dttOut1 As DataTable
        Dim dttOut2 As DataTable

        Dim strSQL As String = ""

        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "CustomSchema.sql")

            Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True
        Catch ex As Exception
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
        End Try
    End Function

    Public Overridable Function GetCodpaga(ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCodpaga.sql")

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetComuni(ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetComuni.sql")

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetNazioni(ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetNazioni.sql")

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function


    Public Overridable Function GetInfo(ByRef dttOut As DataTable, ByVal strRelease As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetInfo.sql")
            strSQL = strSQL.Replace("@release", CStrSQL(strRelease))

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function
    Public Overridable Function GetClassiSconto(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetClassiSconto.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetPorto(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetPorto.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function


    Public Overridable Function GetCliforCate(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCliforCate.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function


    Public Overridable Function GetCanaliVendita(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCanaliVendita.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetClifor(ByVal strTipoCF As String, ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strFiltraConAgenti As String, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetClifor.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@tipocf", CStrSQL(strTipoCF))
            strSQL = strSQL.Replace("@filtra_con_agenti", CStrSQL(strFiltraConAgenti))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetCliforBlo(ByVal strTipoCF As String, ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCliforBlo.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@tipocf", CStrSQL(strTipoCF))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function


    Public Overridable Function GetCliforNote(ByVal strTipoCF As String, ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCliforNote.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@tipocf", CStrSQL(strTipoCF))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetCliforSenzaCoordinate(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCliforSenzaCoordinate.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function


    Public Overridable Function GetCliforDestdiv(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCliforDestdiv.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function
    Public Overridable Function GetCliforAge(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCliforAge.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function
    Public Overridable Function GetCliforDettCon(ByVal strTipoCF As String, ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCliforDettCon.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@tipocf", CStrSQL(strTipoCF))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function
    Public Overridable Function GetCliforFatt(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCliforFatt.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function
    Public Overridable Function GetCliforTestDoc(ByVal strTipoCF As String, ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String, _
                                                 Optional ByVal strGiorniStoricoDocumenti As String = "365") As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCliforTestDoc.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@tipocf", CStrSQL(strTipoCF))
            strSQL = strSQL.Replace("@gg_documenti", CStrSQL(strGiorniStoricoDocumenti))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function
    Public Overridable Function GetCliforRighDoc(ByVal strTipoCF As String, ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String, _
                                                     Optional ByVal strGiorniStoricoDocumenti As String = "365") As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCliforRighDoc.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@tipocf", CStrSQL(strTipoCF))
            strSQL = strSQL.Replace("@gg_documenti", CStrSQL(strGiorniStoricoDocumenti))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function
    Public Overridable Function GetCliforScaDoc(ByVal strTipoCF As String, ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCliforScaDoc.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@tipocf", CStrSQL(strTipoCF))
            strSQL = strSQL.Replace("@anni", CStrSQL(2))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetArt(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strQuery As String, ByVal strWhere As String, Optional strScontoMax As String = "0") As Boolean

        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetArt.sql")
            If strQuery <> "" Then strSQL = strQuery
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@sconto_max_ven", CStrSQL(strScontoMax))


            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetArtGiacenze(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetArtGiacenze.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetArtUltVen(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String, _
                                             Optional ByVal strGiorniUltVen As String = "180") As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetArtUltVen.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@gg_ultacqven", CStrSQL(strGiorniUltVen))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function


    Public Overridable Function GetArtUltAcq(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String, _
                                             Optional ByVal strGiorniUltAcq As String = "180") As Boolean

        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetArtUltAcq.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@gg_ultacqven", CStrSQL(strGiorniUltAcq))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetArtStoart(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String, _
                                             Optional ByVal strFiltroGiorniStoArt As String = "180") As Boolean
        'per ogni articolo/cliente prendo l'ultimo ordine
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetArtStoart.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))
            strSQL = strSQL.Replace("@giornistorico", CStrSQL(strFiltroGiorniStoArt))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function


    Public Overridable Function GetArtListini(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetArtListini.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    ''' <summary>
    ''' Recupera i dati dei Leads
    ''' </summary>
    ''' <param name="strDitta">Ditta per la quale estrarre i dati</param>
    ''' <param name="dttOut">Variabile di ritorno con il risultato della query</param>
    ''' <param name="strWhere">Eventuale condizione di where da appendere alla query</param>
    ''' <param name="strIncludiLeadConClienti">Se impostato a 0 (default), vengono esportati solo i leads senza clienti. 1 per esportare anche i leads associati a clienti</param>
    ''' <returns>True se andato a buon fine, False se ci sono errori</returns>
    ''' <remarks>Commenti vari</remarks>
    Public Overridable Function GetLeads(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String, _
                                         Optional ByVal strIncludiLeadConClienti As String = "0") As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetLeads.sql")
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

    Public Overridable Function GetCampagne(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetCampagne.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function


    Public Overridable Function GetLeadAccessi(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetLeadAccessi.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function


    Public Overridable Function GetLeadAccessiCrm(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetLeadAccessiCrm.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetLeadDetCon(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetLeadDetCon.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetLeadNote(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetLeadNote.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetLeadTestOff(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strIncludiLeadClienti As String, ByVal strWhere As String,
                                               Optional ByVal strFiltroGiorniOfferte As String = "365") As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetLeadTestOff.sql")
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
    Public Overridable Function GetLeadRighOff(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strIncludiLeadClienti As String, ByVal strWhere As String, _
                                               Optional ByVal strFiltroGiorniOfferte As String = "365") As Boolean
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetLeadRighOff.sql")
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

    Public Overridable Function GetArtSconti(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strWhere As String) As Boolean
        Dim strSQL As String = ""
        Try
            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetArtSconti.sql")
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function GetArtCatalogo(ByVal strDitta As String, ByRef dttOut As DataTable, ByVal strQuery As String, ByVal strWhere As String) As Boolean
        'nb: escludo gli articoli descrittivi (eccetto 'd'), gli articoli TCO e gli articoli a varianti non movimentabili (la radice della variante)
        Dim strSQL As String = ""
        Try

            strSQL = ApexNetLIB.EmbeddedResource.GetString(GetType(CLDIEIBUS).Assembly, "GetArtCatalogo.sql")
            ' Se esiste una query personalizzata sovrascrivo quella standard
            If strQuery <> "" Then strSQL = strQuery
            strSQL = strSQL.Replace("@ditta", CStrSQL(strDitta))

            If strWhere <> "" Then AddWhereCondition(strSQL, strWhere)

            dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            Throw (New NTSException(GestError(ex, Me, strSQL, oApp.InfoError, "", False)))
            '--------------------------------------------------------------
        End Try
    End Function


End Class
