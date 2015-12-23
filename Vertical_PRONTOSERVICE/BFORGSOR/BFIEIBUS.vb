Imports System
Imports System.Data
Imports System.Globalization
Imports System.Text
Imports System.IO
Imports NTSInformatica.CLN__STD



Public Class CLFIEIBUS
  Inherits CLEIEIBUS

  'Public Overrides Function Elabora_Child() As Boolean
  '  Try

  ' Aggiunto 1 = 0 per disabilitare export girovisite da xmobile

  '   If strTipork.Contains("CLI;") Then
  '     ThrowRemoteEvent(New NTSEventArgs("AGGIOLABEL", "Export Dati Girovisite..."))
  '     If Not Elabora_ExportGirovisita(oApp.AscDir & "\IB_CLIFOR_GIROVISITA.DAT") Then Return False
  '     arFileGen.Add(oApp.AscDir & "\IB_CLIFOR_GIROVISITA.DAT")
  '
  '      End If
  '
  '     Return True

  '  Catch ex As Exception
  '--------------------------------------------------------------
  '   If GestErrorCallThrow() Then
  '    Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
  ' Else
  '  ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
  ' End If
  '--------------------------------------------------------------	
  ' End Try
  'End Function


  ' Public Overridable Function Elabora_ExportGirovisita(strFileOut As String) As Boolean

  '  Dim dttTmp As New DataTable
  ' Dim sbFile As New StringBuilder
  '  Try
  '
  '   If Not CType(oCldIbus, CLHIEIBUS).GetGirovisita(strDittaCorrente, dttTmp, "") Then Return False
  '
  '
  '   sbFile.Append("CHIAVE|COD_DITTA|COD_CLIFOR|COD_AGE|COD_DEST|GG_VISITA|SEQUENZA|DAT_ULT_VISITA|GG_ULT_VISITA|DAT_ULT_MOD" & vbCrLf)
  '  For Each dtrT As DataRow In dttTmp.Rows
  '   sbFile.Append(
  '                ConvStr(dtrT!xx_chiave) & "|" & _
  '               strDittaCorrente & "|" & _
  '              ConvStr(dtrT!gi_conto) & "|" & _
  '             ConvStr(dtrT!gi_agente) & "|" & _
  '            ConvStr(dtrT!gi_destin) & "|" & _
  '           ConvStr(dtrT!gi_giorno) & "|" & _
  '          ConvStr(dtrT!gi_sequenza) & "|" & _
  '         ConvData(dtrT!td_datord) & "|" & _
  '        ConvStr(dtrT!xx_weekday_datord) & "|" & _
  '       ConvData(dtrT!xx_ultagg, True) & vbCrLf)
  '      Next

  'Dim w1 As New StreamWriter(strFileOut, False)
  '    w1.Write(sbFile.ToString)
  '   w1.Flush()
  '  w1.Close()
  '
  '   Return True

  '  Catch ex As Exception
  '--------------------------------------------------------------
  '   If GestErrorCallThrow() Then
  '    Throw New NTSException(GestError(ex, Me, "", oApp.InfoError, "", False))
  ' Else
  '     ThrowRemoteEvent(New NTSEventArgs("", GestError(ex, Me, "", oApp.InfoError, "", False)))
  '  End If
  ''--------------------------------------------------------------	
  ' Finally
  '   dttTmp.Clear()
  '  End Try

  'End Function



End Class