Imports System
Imports System.Data
Imports System.Globalization
Imports System.Text
Imports System.IO
Imports NTSInformatica.CLN__STD

Public Class CLFIEIBUS
  Inherits CLEIEIBUS

    'TNET_OVERLOADS
    Public Overrides Function Elabora_ExportCliforTestDoc(ByVal TipoCliFor As String, ByVal strFileOut As String, ByRef dttTmp As DataTable) As Boolean
        'restituisco le testate dei documenti di magazino/ordini di ogni cliente/fornitore ATTIVO o POTENZIALE
        Dim sbFile As New StringBuilder
        Dim strTipoDoc As String = ""
        Dim strDescEvas As String = ""
        Try
            If Not oCldIbus.GetCliforTestDoc(TipoCliFor, strDittaCorrente, dttTmp, strCustomWhereGetCliforTestDoc, _
                                            strGiorniStoricoDocumenti:=strFiltroGGDocumenti) Then Return False

            sbFile.Append("CHIAVE|COD_DITTA|TIPO_CLIFOR|COD_CLIFOR|COD_TIPODOC|COD_STIPODOC|" & _
                        "FLGDAEVADERE|DATA_DOC|NUMREG|TIPODOC|TIPO|SOTTOTIPO|DATAREG|SEZIONALE|NUM_DOC|DOCORIG|" & _
                        "DEPOSITO|VALUTA|TOTALEDOC|DATACONS|SCADENZE|ESTCONT|TIPOSTATO_DOC|DESSTATO_DOC|DATA_FATT|NUM_FATT|" & _
                        "DES_DOC|DES_NOTE|COD_VALUTA|DAT_ULT_MOD" & vbCrLf)
            For Each dtrT As DataRow In dttTmp.Rows

                Select Case ConvStr(dtrT!xx_flevas)
                    Case "0" : strDescEvas = "Da evadere"
                    Case "1" : strDescEvas = "Evaso"
                End Select

                Select Case NTSCStr(dtrT!tm_tipork)
                    Case "A" : strTipoDoc = "Fatt. Imm. em." '
                    Case "B" : strTipoDoc = "DDT emesso" ' 
                    Case "C" : strTipoDoc = "Corr. emesso"
                    Case "D" : strTipoDoc = "Fatt. Diff. em." '
                    Case "E" : strTipoDoc = "Note di Add. em."
                    Case "F" : strTipoDoc = "Ric.Fisc. em."
                    Case "I" : strTipoDoc = "Riem. Ric.Fisc."
                    Case "J" : strTipoDoc = "Note Accr. ric."
                    Case "(" : strTipoDoc = "Nota accr. diff. ric."
                    Case "K" : strTipoDoc = "Fatt. Diff. ric."
                    Case "L" : strTipoDoc = "Fatt. Imm. ric."
                    Case "M" : strTipoDoc = "DDT ricevuto"
                    Case "N" : strTipoDoc = "Note Accr. em."
                    Case "£" : strTipoDoc = "Nota accr. diff. em."
                    Case "P" : strTipoDoc = "Fatt.Ric.Fisc.Diff."
                    Case "S" : strTipoDoc = "Fatt.Ric.Fisc. em."
                    Case "T" : strTipoDoc = "Carico da prod."
                    Case "U" : strTipoDoc = "Scarico a prod."
                    Case "Z" : strTipoDoc = "Bolle di mov.int."
                    Case "W" : strTipoDoc = "Note di prel."
                    Case "R" : strTipoDoc = "Imp. cliente"
                    Case "O" : strTipoDoc = "Ord. forn."
                    Case "H" : strTipoDoc = "Ord. di prod."
                    Case "X" : strTipoDoc = "Imp. Trasfer."
                    Case "Q" : strTipoDoc = "Prev."
                    Case "#" : strTipoDoc = "Imp. di comm."
                    Case "V" : strTipoDoc = "Imp. cli aperto"
                    Case "$" : strTipoDoc = "Ord. forn aperto"
                    Case "Y" : strTipoDoc = "Imp. di prod."
                        'TNET_INIMOD
                    Case "!" : strTipoDoc = "Offerte CRM"
                        'TNET_FINEMOD
                End Select

                ' ConvStr(dtrT!tm_tipork) & "§" & ConvStr(dtrT!tm_anno) & "§" & ConvStr(dtrT!tm_serie) & "§" & ConvStr(dtrT!tm_numdoc) & "|" & _
                sbFile.Append(strDittaCorrente & "§" & ConvStr(dtrT!tm_tipork) & "§" & ConvStr(dtrT!tm_anno) & "§" & ConvStr(dtrT!tm_serie) & "§" & ConvStr(dtrT!tm_numdoc) & "|" & _
                                strDittaCorrente & "|" & _
                                IIf(ConvStr(dtrT!an_tipo) = "C", 0, 1).ToString & "|" & _
                                ConvStr(dtrT!an_conto) & "|" & _
                                Asc(strTipoDoc).ToString & "|" & _
                                ConvStr(dtrT!xx_tipobf) & "|" & _
                                ConvStr(dtrT!xx_flevas) & "|" & _
                                ConvData(dtrT!tm_datdoc, False) & "|" & _
                                ConvStr(dtrT!xx_numreg) & "|" & _
                                ConvStr(dtrT!tm_tipork) & "|" & _
                                strTipoDoc.PadRight(20).Substring(0, 20).Trim & "|" & _
                                ConvStr(dtrT!tb_destpbf).PadRight(20).Substring(0, 20).Trim & "|" & _
                                "" & "|" & _
                                ConvStr(dtrT!tm_serie) & "|" & _
                                 (ConvStr(dtrT!tm_numdoc) & IIf(NTSCStr(dtrT!tm_serie) <> " ", "/" & ConvStr(dtrT!tm_serie), "").ToString).Trim & "|" & _
                                "" & "|" & _
                                ConvStr(dtrT!tm_magaz) & "|" & _
                                ConvStr(dtrT!tb_desvalu) & "|" & _
                                NTSCDec(dtrT!tm_totdoc).ToString(oApp.FormatSconti) & "|" & _
                                "" & "|" & _
                                "" & "|" & _
                                "" & "|" & _
                                ConvStr(dtrT!xx_flevas) & "|" & _
                                strDescEvas & "|" & _
                                ConvData(dtrT!tm_datfat, False) & "|" & _
                                (ConvStr(dtrT!tm_numfat) & IIf(NTSCStr(dtrT!tm_alffat) <> " ", "/" & ConvStr(dtrT!tm_alffat), "").ToString).Trim & "|" & _
                                ConvStr(dtrT!tm_riferim) & "|" & _
                                ConvStr(dtrT!tm_note) & "|" & _
                                ConvStr(dtrT!tm_valuta) & "|" & _
                                ConvData(dtrT!tm_ultagg, True) & vbCrLf)

                ' ConvStr(dtrT!tm_note).PadRight(1000).Substring(0, 1000).Trim & "|" & _
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        End Try
    End Function


#Region "Personalizzazioni Apex"
    Public Overrides Function Elabora_Child() As Boolean
        Try

            If strTipork.Contains("LEA;") Then

                'ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export Leads Sconti..."))
                'If Not Elabora_ExportLeadSconti(oApp.AscDir & "\IB_LEAD_SCONTI.DAT") Then Return False
                'arFileGen.Add(oApp.AscDir & "\IB_LEAD_SCONTI.DAT")

                'ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export Campi personalizzati..."))
                'If Not Elabora_ExportCustomFields1(oApp.AscDir & "\IB_CUSTOM_FIELDS.DAT") Then Return False
                'arFileGen.Add(oApp.AscDir & "\IB_CUSTOM_FIELDS.DAT")

                ' Inizio

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export Leads Sconti..."))
                If Not Elabora_ExportLeadSconti(oApp.AscDir & "\io_lead_sconti.DAT") Then Return False
                arFileGen.Add(oApp.AscDir & "\io_lead_sconti.dat")

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export Leads Settori..."))
                If Not Elabora_ExportLeadSettori(oApp.AscDir & "\io_lead_settori.dat") Then Return False
                arFileGen.Add(oApp.AscDir & "\io_lead_settori.dat")

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export Leads Interessi..."))
                If Not Elabora_ExportLeadInteressi(oApp.AscDir & "\io_lead_interessi.dat") Then Return False
                arFileGen.Add(oApp.AscDir & "\io_lead_interessi.dat")

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export Leads Fonti..."))
                If Not Elabora_ExportLeadFonti(oApp.AscDir & "\io_lead_fonti.dat") Then Return False
                arFileGen.Add(oApp.AscDir & "\io_lead_fonti.dat")

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export Campi personalizzati..."))
                If Not Elabora_ExportCustomFields1(oApp.AscDir & "\io_custom_fields.DAT") Then Return False
                arFileGen.Add(oApp.AscDir & "\io_custom_fields.dat")

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export Campagne Operatore..."))
                If Not Elabora_ExportCampagneOperatore(oApp.AscDir & "\io_campagne_operatore.dat") Then Return False
                arFileGen.Add(oApp.AscDir & "\io_campagne_operatore.dat")

            End If

            If strTipork.Contains("CLI;") Then

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export Operatori cliente..."))
                If Not Elabora_ExportCliforOperatore(oApp.AscDir & "\io_clifor_operatore.dat") Then Return False
                arFileGen.Add(oApp.AscDir & "\io_clifor_operatore.dat")


            End If

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        End Try
    End Function

    Public Overridable Function Elabora_ExportLeadSettori(ByVal strFileOut As String) As Boolean
        'esporta tutti i comuni
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder

        Try

            If Not CType(oCldIbus, CLHIEIBUS).cGetLeadSettori(strDittaCorrente, dttTmp) Then Return False

            sbFile.Append("CHIAVE|COD_DITTA|COD_SETTORE|DES_SETTORE" & vbCrLf)
            For Each dtrT As DataRow In dttTmp.Rows
                sbFile.Append(ConvStr(dtrT!xx_chiave) & "|" & _
                                strDittaCorrente & "|" & _
                                ConvStr(dtrT!tb_codhhse) & "|" & _
                                ConvStr(dtrT!tb_deshhse) & vbCrLf)
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function

    Public Overridable Function Elabora_ExportLeadInteressi(ByVal strFileOut As String) As Boolean
        'esporta tutti i comuni
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder

        Try

            If Not CType(oCldIbus, CLHIEIBUS).cGetLeadInteressi(strDittaCorrente, dttTmp) Then Return False

            sbFile.Append("CHIAVE|COD_DITTA|COD_INTERESSE|DES_INTERESSE" & vbCrLf)
            For Each dtrT As DataRow In dttTmp.Rows
                sbFile.Append(ConvStr(dtrT!xx_chiave) & "|" & _
                                strDittaCorrente & "|" & _
                                ConvStr(dtrT!tb_codhhin) & "|" & _
                                ConvStr(dtrT!tb_deshhin) & vbCrLf)
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function

    Public Overridable Function Elabora_ExportLeadFonti(ByVal strFileOut As String) As Boolean
        'esporta tutti i comuni
        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder

        Try

            If Not CType(oCldIbus, CLHIEIBUS).cGetLeadFonti(strDittaCorrente, dttTmp) Then Return False

            sbFile.Append("CHIAVE|COD_DITTA|COD_FONTE|DES_FONTE" & vbCrLf)
            For Each dtrT As DataRow In dttTmp.Rows
                sbFile.Append(ConvStr(dtrT!xx_chiave) & "|" & _
                                strDittaCorrente & "|" & _
                                ConvStr(dtrT!tb_codhhfn) & "|" & _
                                ConvStr(dtrT!tb_deshhfn) & vbCrLf)
            Next

            If dttTmp.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function

    Public Overridable Function Elabora_ExportCampagneOperatore(ByVal strFileOut As String) As Boolean
        'esporta tutti i comuni
        Dim dttTmpCampagneOperat As New DataTable
        Dim dttTmpOperat As New DataTable
        Dim campagneTrovate As Integer = 0
        Dim sbFile As New StringBuilder

        Try

            If Not CType(oCldIbus, CLHIEIBUS).cGetOperatori(strDittaCorrente, dttTmpOperat) Then Return False

            If Not CType(oCldIbus, CLHIEIBUS).cGetCampagneOperatore(strDittaCorrente, dttTmpCampagneOperat) Then Return False


            sbFile.Append("CHIAVE|COD_DITTA|COD_OPERATORE|COD_CAMPAGNA" & vbCrLf)

            ' Tutti gli operatori non impostati nella tabella campagne operatori, devono vedere tutto,
            ' quindi li estratto con un null nella campagna
            For Each dtrT As DataRow In dttTmpOperat.Rows

                campagneTrovate = dttTmpCampagneOperat.Select(String.Format("xx_cod_operatore='{0}'", ConvStr(dtrT!OpNome))).Length()

                If campagneTrovate = 0 Then
                    sbFile.Append( _
                        strDittaCorrente & "§" & ConvStr(dtrT!OpNome) & "§" & "ALL" & "|" & _
                        strDittaCorrente & "|" & _
                        ConvStr(dtrT!OpNome) & _
                        "|" & vbCrLf)

                End If

            Next

            ' Poi estraggo le associazioni operatori campagne
            For Each dtrT As DataRow In dttTmpCampagneOperat.Rows

                sbFile.Append( _
                    ConvStr(dtrT!xx_chiave) & "|" & _
                    strDittaCorrente & "|" & _
                    ConvStr(dtrT!xx_cod_operatore) & "|" & _
                    ConvStr(dtrT!xx_cod_campagna) & vbCrLf)

            Next

            If dttTmpCampagneOperat.Rows.Count > 0 Or dttTmpOperat.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmpCampagneOperat.Clear()
        End Try
    End Function

    Public Overridable Function Elabora_ExportCliforOperatore(ByVal strFileOut As String) As Boolean
        'esporta tutti i comuni
        Dim dttTmpOperat As New DataTable
        Dim dttTmpCliforOperatoriZona As New DataTable
        Dim clientiTrovati As Integer = 0
        Dim sbFile As New StringBuilder

        Try

            If Not CType(oCldIbus, CLHIEIBUS).cGetOperatori(strDittaCorrente, dttTmpOperat) Then Return False

            If Not CType(oCldIbus, CLHIEIBUS).cGetCliforOperatoriZona(strDittaCorrente, dttTmpCliforOperatoriZona) Then Return False


            sbFile.Append("CHIAVE|COD_DITTA|TIPO_CLIFOR|COD_OPERATORE|COD_CLIFOR" & vbCrLf)

            ' Tutti gli operatori non impostati nella tabella campagne operatori, devono vedere tutto,
            ' quindi li estratto con un null nella campagna
            For Each dtrT As DataRow In dttTmpOperat.Rows

                clientiTrovati = dttTmpCliforOperatoriZona.Select(String.Format("xx_cod_operatore='{0}'", ConvStr(dtrT!OpNome))).Length()

                If clientiTrovati = 0 Then
                    sbFile.Append( _
                        strDittaCorrente & "§" & ConvStr(dtrT!OpNome) & "§" & "ALL" & "|" & _
                        strDittaCorrente & "|" & _
                        "0|" & _
                        ConvStr(dtrT!OpNome) & _
                        "|" & vbCrLf)

                End If

            Next

            ' Poi estraggo le associazioni operatori campagne
            For Each dtrT As DataRow In dttTmpCliforOperatoriZona.Rows

                sbFile.Append( _
                    ConvStr(dtrT!xx_chiave) & "|" & _
                    strDittaCorrente & "|" & _
                    "0|" & _
                    ConvStr(dtrT!xx_cod_operatore) & "|" & _
                    ConvStr(dtrT!xx_conto) & vbCrLf)


            Next

            If dttTmpCliforOperatoriZona.Rows.Count > 0 Or dttTmpOperat.Rows.Count > 0 Then
                Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
                w1.Write(sbFile.ToString)
                w1.Flush()
                w1.Close()
            End If


            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmpCliforOperatoriZona.Clear()
        End Try
    End Function


    Public Overridable Function Elabora_ExportLeadSconti(ByVal strFileOut As String) As Boolean



        'esporta gli sconti in vigore alla data odierna

        Dim strTrattaSc1 As String = oCldIbus.GetSettingBus("OPZIONI", ".", ".", "TrattaSc1", "S", " ", "S")
        Dim strTrattaSc2 As String = oCldIbus.GetSettingBus("OPZIONI", ".", ".", "TrattaSc2", "S", " ", "S")
        Dim strTrattaSc3 As String = oCldIbus.GetSettingBus("OPZIONI", ".", ".", "TrattaSc3", "S", " ", "S")
        Dim strTrattaSc4 As String = oCldIbus.GetSettingBus("OPZIONI", ".", ".", "TrattaSc4", "S", " ", "S")
        Dim strTrattaSc5 As String = oCldIbus.GetSettingBus("OPZIONI", ".", ".", "TrattaSc5", "S", " ", "S")
        Dim strTrattaSc6 As String = oCldIbus.GetSettingBus("OPZIONI", ".", ".", "TrattaSc6", "S", " ", "S")

        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Dim strPrior As String = ""

        Try

            If Not CType(oCldIbus, CLHIEIBUS).cGetLeadSconti(strDittaCorrente, dttTmp) Then Return False

            sbFile.Append("CHIAVE|COD_DITTA|COD_ART|COD_LEAD|CLASSE_ARTICOLO|DES_CLASSE_ARTICOLO|COD_PROMO|" & _
                    "QUANTITA_INIZIO|QUANTITA_FINE|DATA_INIZIO|DATA_FINE|PRIORITA|SCONTO1|SCONTO2|SCONTO3|SCONTO4|" & _
                    "SCONTO5|SCONTO6|TIPO_SCONTO1|TIPO_SCONTO2|TIPO_SCONTO3|TIPO_SCONTO4|TIPO_SCONTO5|TIPO_SCONTO6|DAT_ULT_MOD" & vbCrLf)
            For Each dtrT As DataRow In dttTmp.Rows
                If oApp.oGvar.strPriorSconti <> "ACBDFE" Then
                    strPrior = dtrT!xx_prior1.ToString
                Else
                    strPrior = dtrT!xx_prior2.ToString
                End If

                sbFile.Append(strDittaCorrente & "§" & ConvStr(dtrT!ar_codart) & "§" & ConvStr(dtrT!so_conto) & "§" & ConvStr(dtrT!so_clscan) & "§" & ConvStr(dtrT!so_clscar) & "§" & ConvStr(dtrT!so_codtpro) & "§" & ConvStr(dtrT!so_daquant) & "§" & ConvData(dtrT!so_datagg, False) & "|" & _
                              strDittaCorrente & "|" & _
                              IIf(ConvStr(dtrT!ar_codart) = "0", "", ConvStr(dtrT!ar_codart)).ToString & "|" & _
                              IIf(ConvStr(dtrT!so_conto) = "0", "", ConvStr(dtrT!so_conto)).ToString & "|" & _
                              IIf(ConvStr(dtrT!so_clscar) = "0", "", ConvStr(dtrT!so_clscar)).ToString & "|" & _
                              ConvStr(dtrT!tb_descsar) & "|" & _
                              IIf(ConvStr(dtrT!so_codtpro) = "0", "", ConvStr(dtrT!so_codtpro)).ToString & "|" & _
                              NTSCDec(dtrT!so_daquant).ToString("0.00") & "|" & _
                              NTSCDec(dtrT!so_aquant).ToString("0.00") & "|" & _
                              ConvData(dtrT!so_datagg, False) & "|" & _
                              ConvData(dtrT!so_datscad, False) & "|" & _
                              strPrior & "|" & _
                              NTSCDec(dtrT!so_scont1).ToString("0.00") & "|" & _
                              NTSCDec(dtrT!so_scont2).ToString("0.00") & "|" & _
                              NTSCDec(dtrT!so_scont3).ToString("0.00") & "|" & _
                              NTSCDec(dtrT!so_scont4).ToString("0.00") & "|" & _
                              NTSCDec(dtrT!so_scont5).ToString("0.00") & "|" & _
                              NTSCDec(dtrT!so_scont6).ToString("0.00") & "|" & _
                              strTrattaSc1 & "|" & _
                              strTrattaSc2 & "|" & _
                              strTrattaSc3 & "|" & _
                              strTrattaSc4 & "|" & _
                              strTrattaSc5 & "|" & _
                              strTrattaSc5 & "|" & _
                              ConvData(dtrT!so_ultagg, True) & vbCrLf)
            Next

            Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
            w1.Write(sbFile.ToString)
            w1.Flush()
            w1.Close()

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function

    Public Overridable Function Elabora_ExportCustomFields1(ByVal strFileOut As String) As Boolean


        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Dim strPrior As String = ""

        Dim Ordinamento As Integer = 0

        Dim sGroup As String = "0"
        Dim sItem As String = "1"

        Dim sVerticale As String = "0"
        Dim sOrizzontale As String = "1"


        Try

            If Not CType(oCldIbus, CLHIEIBUS).cGetCustomFields1(strDittaCorrente, dttTmp) Then Return False

            sbFile.Append( _
      "CHIAVE             |" & _
      "COD_DITTA          |" & _
      "NOME               |" & _
      "VALORE             |" & _
      "TIPO               |" & _
      "CHIAVE_PADRE       |" & _
      "CONTESTO           |" & _
      "VISUAL_MASK        |" & _
      "COD_EXT            |" & _
      "POSIZIONE_VALORE   |" & _
      "ORDINAMENTO        |" & _
      "POSIZIONE_NOME     |" & _
      "TIPO_DATO          |" & _
      "DAT_ULT_MOD         " & _
       vbCrLf).Replace(" ", "")

            For Each dtrT As DataRow In dttTmp.Rows
                Ordinamento += 1
                sbFile.Append(FormattaRigaLead("Trasf. in potenziale", "le_hhin_potenziale", Ordinamento, dtrT, isDateFormat:=True))
                Ordinamento += 1
                sbFile.Append(FormattaRigaLead("Trasf. in anagrafica", "le_hhin_anagrafica", Ordinamento, dtrT, isDateFormat:=True))
                Ordinamento += 1
                sbFile.Append(FormattaRigaLead("Fonte", "le_hhfonte", Ordinamento, dtrT))
                Ordinamento += 1
                sbFile.Append(FormattaRigaLead("Interesse", "le_hhlivello_interesse", Ordinamento, dtrT))
                Ordinamento += 1
                sbFile.Append(FormattaRigaLead("Settore", "le_hhsettore", Ordinamento, dtrT))
                Ordinamento += 1
                sbFile.Append(FormattaRigaLead("Fatturato CMATIC", "le_hhfatturato_cmatic", Ordinamento, dtrT))
                Ordinamento += 1
                sbFile.Append(FormattaRigaLead("Fatturato Raccordi", "le_hhfatturato_raccordi", Ordinamento, dtrT))
                Ordinamento += 1
                sbFile.Append(FormattaRigaLead("Fascia di mercato", "le_hhfascia_mercato", Ordinamento, dtrT))
                Ordinamento += 1
                sbFile.Append(FormattaRigaLead("Condizioni concorrente", "le_hhcondizioni_concorrente", Ordinamento, dtrT))
                Ordinamento += 1
                sbFile.Append(FormattaRigaLead("Campagna origine", "xx_hhcampagna_origine", Ordinamento, dtrT))
                Ordinamento += 1
                sbFile.Append(FormattaRigaLead("Pagamento", "xx_hhcodpaga", Ordinamento, dtrT))
                Ordinamento += 1
                sbFile.Append(FormattaRigaLead("Porto", "tb_desport", Ordinamento, dtrT))
                Ordinamento += 1
                sbFile.Append(FormattaRigaLead("Data acquisizione", "le_hhdtaper", Ordinamento, dtrT, isDateFormat:=True))
                Ordinamento += 1
                sbFile.Append(FormattaRigaLead("Cod. esente", "xx_desciva", Ordinamento, dtrT))
                Ordinamento += 1
                sbFile.Append(FormattaRigaLead("Tipo bolla / fattura", "tb_destpbf", Ordinamento, dtrT))


                ' Label le_hhin_potenziale
                'sbFile.Append(
                '              ConvStr(dtrT!xx_chiave) & "§" & "le_hhin_potenziale" & _
                '              strDittaCorrente & "|" & _
                '              "Trasf. in potenziale" & "|" & _
                '              ConvData(dtrT!le_hhin_potenziale, False) & "|" & _
                '              sItem & "|" & _
                '              "|" & _
                '              "LEADS|" & _
                '              ConvStr(dtrT!le_codlead) & "|" & _
                '              sOrizzontale & "|" & _
                '              ConvStr(Ordinamento) & "|" & _
                '              sVerticale & "|" & _
                '              ConvData(dtrT!xx_ultagg, True) & vbCrLf)

                '' Valore le_hhin_anagrafica
                'sbFile.Append(
                '              ConvStr(dtrT!xx_chiave) & "§" & "le_hhin_anagrafica" & _
                '              strDittaCorrente & "|" & _
                '              "Trasf. in anagrafica" & "|" & _
                '              ConvData(dtrT!le_hhin_anagrafica, False) & "|" & _
                '              sItem & "|" & _
                '              "|" & _
                '              "LEADS|" & _
                '              ConvStr(dtrT!le_codlead) & "|" & _
                '              sOrizzontale & "|" & _
                '              ConvStr(Ordinamento) & "|" & _
                '              sVerticale & "|" & _
                '              ConvData(dtrT!xx_ultagg, True) & vbCrLf)

            Next

            Dim w1 As New StreamWriter(strFileOut, False, System.Text.Encoding.UTF8)
            w1.Write(sbFile.ToString)
            w1.Flush()
            w1.Close()

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            If GestErrorCallThrow() Then
                Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
            Else
                ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
            End If
            '--------------------------------------------------------------	
        Finally
            dttTmp.Clear()
        End Try
    End Function

    Function FormattaRigaLead(ByVal Label As String, ByVal DBCode As String, ByVal Order As Integer, ByVal Riga As DataRow, Optional ByVal isDateFormat As Boolean = False) As String
        Dim sItem As String = "1"
        Dim sOrizzontale As String = "1"
        Dim sVerticale As String = "0"
        Dim tipo As Integer = 0
        Dim RetValue As String = ""
        Dim FieldValue As String
        Dim sTipoDato As String

        'Contesto (in minuscolo):
        '	- clienti
        '	- fornitori
        '	- prodotti
        '	- leads

        '- Visual_mask (partendo da destra): 
        '- pos. 0 : modulo schede
        '- pos. 1 : modulo ordini
        '- pos. 2 : modulo CRM

        'Esempi:
        '- vis_mask: 100 visibile nel tab CRM
        '- vis_mask: 1 visibile nel tab schede
        '- vis_mask: 11 visibile nel tab schede e ordini 

        'Il campo visual_mask può avere lunghezza variabile.


        Select Case Riga.Item(DBCode).GetType.Name
            Case "String"
                sTipoDato = "0"
                FieldValue = ConvStr(Riga.Item(DBCode))
            Case "Date"
                sTipoDato = "1"
                FieldValue = ConvData(Riga.Item(DBCode))
            Case "DateTime"
                sTipoDato = "2"
                FieldValue = ConvData(Riga.Item(DBCode), True)
            Case "Int32"
                sTipoDato = "3"
                FieldValue = ConvStr(Riga.Item(DBCode))
            Case "Decimal"
                sTipoDato = "4"
                FieldValue = ConvStr(Riga.Item(DBCode))
            Case Else
                ' di che tipo e ? Non lo. Lo tratto come string
                sTipoDato = "0"
                FieldValue = ConvStr(Riga.Item(DBCode))
        End Select

        ' Purtroppo anche se il db dovrebbe tornare un date, mi arriva un datetime
        ' Quindi lo forzo io
        If isDateFormat And sTipoDato = "2" Then
            sTipoDato = "1"
            FieldValue = ConvData(Riga.Item(DBCode))
        End If


        RetValue = _
                          ConvStr(Riga!xx_chiave) & "§" & DBCode & "|" & _
                          strDittaCorrente & "|" & _
                          Label & "|" & _
                          FieldValue & "|" & _
                          sItem & "|" & _
                          "|" & _
                          "leads|" & _
                          "100|" & _
                          ConvStr(Riga!le_codlead) & "|" & _
                          sOrizzontale & "|" & _
                          ConvStr(Order) & "|" & _
                          sVerticale & "|" & _
                          sTipoDato & "|" & _
                          ConvData(Riga!xx_ultagg, True) & vbCrLf
        Return RetValue
    End Function
#End Region
End Class