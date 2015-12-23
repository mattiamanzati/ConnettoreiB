Imports System.Data
Imports System.Data.Common
Imports NTSInformatica.CLN__STD

Public Class CLHIEIBUS
  Inherits CLDIEIBUS

  Public Overrides Sub Init(Applic As CLE__APP)
    MyBase.Init(Applic)
  End Sub


  Public Overrides Function GetArtUltAcq(strDitta As String, ByRef dttOut As DataTable, strWhere As String, Optional strGiorniUltAcq As String = "180") As Boolean
    Dim Riga As Integer
    Dim RetVal As Boolean = False

    Try

      RetVal = MyBase.GetArtUltAcq(strDitta, dttOut, strWhere, strGiorniUltAcq)

      For Riga = 0 To dttOut.Rows.Count - 1
        If dttOut.Rows(Riga)("mm_unmis").ToString <> dttOut.Rows(Riga)("mm_ump").ToString Then
          dttOut.Rows(Riga)("mm_val") = dttOut.Rows(Riga)("mm_valum2")
        End If
      Next

      Return RetVal


    Catch ex As Exception
      '--------------------------------------------------------------
      Throw (New NTSException(GestError(ex, Me, "GetArtUltAcq Personalizzata", oApp.InfoError, "", False)))
      '--------------------------------------------------------------
    End Try

  End Function


  Public Overrides Function GetArtUltVen(strDitta As String, ByRef dttOut As DataTable, strWhere As String, Optional strGiorniUltVen As String = "180") As Boolean

    Dim Riga As Integer
    Dim RetVal As Boolean = False

    Try

      RetVal = MyBase.GetArtUltVen(strDitta, dttOut, strWhere, strGiorniUltVen)

      For Riga = 0 To dttOut.Rows.Count - 1
        If dttOut.Rows(Riga)("mm_unmis").ToString <> dttOut.Rows(Riga)("mm_ump").ToString Then
          dttOut.Rows(Riga)("mm_val") = dttOut.Rows(Riga)("mm_valum2")
        End If
      Next

      Return RetVal


    Catch ex As Exception
      '--------------------------------------------------------------
      Throw (New NTSException(GestError(ex, Me, "GetArtUltVen Personalizzata", oApp.InfoError, "", False)))
      '--------------------------------------------------------------
    End Try

  End Function

End Class

