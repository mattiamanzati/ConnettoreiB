Imports System.Data
Imports System.Data.Common
Imports NTSInformatica.CLN__STD

Public Class CLHIEIBUS
  Inherits CLDIEIBUS

  Public Overrides Sub Init(Applic As CLE__APP)
    MyBase.Init(Applic)
  End Sub

    Public Overrides Function GetArt(strDitta As String, ByRef dttOut As DataTable, strQuery As String, strWhere As String) As Boolean

        Dim Riga As Integer
        Dim RetVal As Boolean = False

        Try

            ' Inoltre nella query c'è questo filtro
            ' "tabcfam.tb_codcfam not in ('90', '99')"
            ' strWhere = " and tabcfam.tb_codcfam not in ('90', '99')"

            RetVal = MyBase.GetArt(strDitta, dttOut, strQuery, strWhere)

            For Riga = 0 To dttOut.Rows.Count - 1
                ' Nel caso di SONN i raggruppamenti devono essere fatti su famiglia/marca e non su gruppo / sottogruppo
                dttOut.Rows(Riga)("tb_desgmer") = dttOut.Rows(Riga)("tb_descfam").ToString
                dttOut.Rows(Riga)("tb_dessgme") = dttOut.Rows(Riga)("tb_desmarc").ToString
                ' Gli articoli deperibili devono essere classificati con S. Se non lo sono devono avere N
                If dttOut.Rows(Riga)("ar_tipo").ToString = "A" Then
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

  Public Overrides Function GetArtCatalogo(strDitta As String, ByRef dttOut As DataTable, strQuery As String, strWhere As String) As Boolean



    Dim Riga As Integer
    Dim RetVal As Boolean = False

    Try

      ' Inoltre nella query c'è questo filtro
      ' "tabcfam.tb_codcfam not in ('90', '99')"
      ' strWhere = "AND ar_gif1 <> '' AND ar_gif1 IS NOT NULL"

      RetVal = MyBase.GetArtCatalogo(strDitta, dttOut, strQuery, strWhere)

      ' Nel caso di SONN i raggruppamenti devono essere fatti su famiglia/marca e non su gruppo / sottogruppo
      For Riga = 0 To dttOut.Rows.Count - 1
        dttOut.Rows(Riga)("xx_l1") = dttOut.Rows(Riga)("tb_descfam").ToString
        dttOut.Rows(Riga)("xx_l2") = dttOut.Rows(Riga)("tb_desmarc").ToString
      Next

      Return RetVal


    Catch ex As Exception
      '--------------------------------------------------------------
      Throw (New NTSException(GestError(ex, Me, "GetArt Personalizzata", oApp.InfoError, "", False)))
      '--------------------------------------------------------------
    End Try

  End Function

End Class

