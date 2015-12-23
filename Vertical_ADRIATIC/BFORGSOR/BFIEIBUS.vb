Imports System
Imports System.Data
Imports System.Globalization
Imports System.Text
Imports System.IO
Imports NTSInformatica.CLN__STD




Public Class CLFIEIBUS
  Inherits CLEIEIBUS

    Public Overrides Function Elabora_Child() As Boolean

        Try

            If strTipork.Contains("ART;") Then

                ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export Campi personalizzati articoli..."))
                If Not Elabora_ExportCustomFieldsProdotti(oApp.AscDir & "\io_custom_fields.dat") Then Return False
                arFileGen.Add(oApp.AscDir & "\io_custom_fields.dat")

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

    Public Overridable Function Elabora_ExportCustomFieldsProdotti(ByVal strFileOut As String) As Boolean


        Dim dttTmp As New DataTable
        Dim sbFile As New StringBuilder
        Dim strPrior As String = ""

        Dim Ordinamento As Integer = 0

        Dim sGroup As String = "0"
        Dim sItem As String = "1"

        Dim sVerticale As String = "0"
        Dim sOrizzontale As String = "1"


        Try

            If Not CType(oCldIbus, CLHIEIBUS).cGetCustomFieldsProdotti(strDittaCorrente, dttTmp) Then Return False

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
                sbFile.Append(FormattaRiga("Totale impegnato", "xx_tot_impegnato", Ordinamento, dtrT, "prodotti"))
                Ordinamento += 1
                sbFile.Append(FormattaRiga("Totale evaso", "xx_tot_evaso", Ordinamento, dtrT, "prodotti"))

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

    Function FormattaRiga(ByVal Label As String, ByVal DBCode As String, ByVal Order As Integer, ByVal Riga As DataRow, ByVal Contesto As String, Optional ByVal isDateFormat As Boolean = False) As String

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

        Dim sItem As String = "1"
        Dim sOrizzontale As String = "1"
        Dim sVerticale As String = "0"
        Dim tipo As Integer = 0
        Dim RetValue As String = ""
        Dim FieldValue As String
        Dim sTipoDato As String

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
                          Contesto + "|" & _
                          "1|" & _
                          ConvStr(Riga!xx_ext_code) & "|" & _
                          sOrizzontale & "|" & _
                          ConvStr(Order) & "|" & _
                          sVerticale & "|" & _
                          sTipoDato & "|" & _
                          "01011900000000" & vbCrLf
        Return RetValue
    End Function


End Class