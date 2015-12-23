Imports System.Collections.Generic
Imports System.Text

Public Class Orders

  Public Property meta() As ws_meta
    Get
      Return m_meta
    End Get
    Set(value As ws_meta)
      m_meta = value
    End Set
  End Property
  Private m_meta As ws_meta
  Public Property cod_progetto() As String
    Get
      Return m_cod_progetto
    End Get
    Set(value As String)
      m_cod_progetto = value
    End Set
  End Property
  Private m_cod_progetto As String
  Public Property last_data_import() As System.Nullable(Of DateTime)
    Get
      Return m_last_data_import
    End Get
    Set(value As System.Nullable(Of DateTime))
      m_last_data_import = value
    End Set
  End Property
  Private m_last_data_import As System.Nullable(Of DateTime)
  Public Property last_id() As System.Nullable(Of Decimal)
    Get
      Return m_last_id
    End Get
    Set(value As System.Nullable(Of Decimal))
      m_last_id = value
    End Set
  End Property
  Private m_last_id As System.Nullable(Of Decimal)
  Public Property testate() As List(Of TestataOrdineExport)
    Get
      Return m_testate
    End Get
    Set(value As List(Of TestataOrdineExport))
      m_testate = value
    End Set
  End Property
  Private m_testate As List(Of TestataOrdineExport)
End Class

Public Class ws_meta
  Public Property limit() As Decimal
    Get
      Return m_limit
    End Get
    Set(value As Decimal)
      m_limit = value
    End Set
  End Property
  Private m_limit As Decimal
  Public Property offset() As Decimal
    Get
      Return m_offset
    End Get
    Set(value As Decimal)
      m_offset = value
    End Set
  End Property
  Private m_offset As Decimal
  Public Property total_count() As Decimal
    Get
      Return m_total_count
    End Get
    Set(value As Decimal)
      m_total_count = value
    End Set
  End Property
  Private m_total_count As Decimal
End Class

Public Class TestataOrdineExport
  Public Property cod_agente() As String
    Get
      Return m_cod_agente
    End Get
    Set(value As String)
      m_cod_agente = value
    End Set
  End Property
  Private m_cod_agente As String
  Public Property cod_clifor() As String
    Get
      Return m_cod_clifor
    End Get
    Set(value As String)
      m_cod_clifor = value
    End Set
  End Property
  Private m_cod_clifor As String
  Public Property cod_cond_pag() As String
    Get
      Return m_cod_cond_pag
    End Get
    Set(value As String)
      m_cod_cond_pag = value
    End Set
  End Property
  Private m_cod_cond_pag As String
  Public Property cod_cond_pag_deperibilita() As String
    Get
      Return m_cod_cond_pag_deperibilita
    End Get
    Set(value As String)
      m_cod_cond_pag_deperibilita = value
    End Set
  End Property
  Private m_cod_cond_pag_deperibilita As String
  Public Property cod_destinazione() As String
    Get
      Return m_cod_destinazione
    End Get
    Set(value As String)
      m_cod_destinazione = value
    End Set
  End Property
  Private m_cod_destinazione As String
  Public Property cod_ditta() As String
    Get
      Return m_cod_ditta
    End Get
    Set(value As String)
      m_cod_ditta = value
    End Set
  End Property
  Private m_cod_ditta As String
  Public Property cod_operatore() As String
    Get
      Return m_cod_operatore
    End Get
    Set(value As String)
      m_cod_operatore = value
    End Set
  End Property
  Private m_cod_operatore As String
  Public Property cod_prog() As String
    Get
      Return m_cod_prog
    End Get
    Set(value As String)
      m_cod_prog = value
    End Set
  End Property
  Private m_cod_prog As String
  Public Property guid_test_ord() As String
    Get
      Return m_guid_test_ord
    End Get
    Set(value As String)
      m_guid_test_ord = value
    End Set
  End Property
  Private m_guid_test_ord As String
  Public Property data_consegna() As System.Nullable(Of DateTime)
    Get
      Return m_data_consegna
    End Get
    Set(value As System.Nullable(Of DateTime))
      m_data_consegna = value
    End Set
  End Property
  Private m_data_consegna As System.Nullable(Of DateTime)
  Public Property data_ordine() As System.Nullable(Of DateTime)
    Get
      Return m_data_ordine
    End Get
    Set(value As System.Nullable(Of DateTime))
      m_data_ordine = value
    End Set
  End Property
  Private m_data_ordine As System.Nullable(Of DateTime)
  Public Property ext_cod_tipo_ord() As String
    Get
      Return m_ext_cod_tipo_ord
    End Get
    Set(value As String)
      m_ext_cod_tipo_ord = value
    End Set
  End Property
  Private m_ext_cod_tipo_ord As String
  Public Property id() As Decimal
    Get
      Return m_id
    End Get
    Set(value As Decimal)
      m_id = value
    End Set
  End Property
  Private m_id As Decimal
  Public Property note() As String
    Get
      Return m_note
    End Get
    Set(value As String)
      m_note = value
    End Set
  End Property
  Private m_note As String
  Public Property rag_soc_clifor() As String
    Get
      Return m_rag_soc_clifor
    End Get
    Set(value As String)
      m_rag_soc_clifor = value
    End Set
  End Property
  Private m_rag_soc_clifor As String
  Public Property utente() As String
    Get
      Return m_utente
    End Get
    Set(value As String)
      m_utente = value
    End Set
  End Property
  Private m_utente As String
  Public Property cod_materiale_estensione1() As String
    Get
      Return m_cod_materiale_estensione1
    End Get
    Set(value As String)
      m_cod_materiale_estensione1 = value
    End Set
  End Property
  Private m_cod_materiale_estensione1 As String
  Public Property cod_materiale_estensione2() As String
    Get
      Return m_cod_materiale_estensione2
    End Get
    Set(value As String)
      m_cod_materiale_estensione2 = value
    End Set
  End Property
  Private m_cod_materiale_estensione2 As String
  Public Property cod_colore_estensione1() As String
    Get
      Return m_cod_colore_estensione1
    End Get
    Set(value As String)
      m_cod_colore_estensione1 = value
    End Set
  End Property
  Private m_cod_colore_estensione1 As String
  Public Property cod_colore_estensione2() As String
    Get
      Return m_cod_colore_estensione2
    End Get
    Set(value As String)
      m_cod_colore_estensione2 = value
    End Set
  End Property
  Private m_cod_colore_estensione2 As String
  Public Property data_import() As System.Nullable(Of DateTime)
    Get
      Return m_data_import
    End Get
    Set(value As System.Nullable(Of DateTime))
      m_data_import = value
    End Set
  End Property
  Private m_data_import As System.Nullable(Of DateTime)
  Public Property righe() As List(Of RigaOrdineExport)
    Get
      Return m_righe
    End Get
    Set(value As List(Of RigaOrdineExport))
      m_righe = value
    End Set
  End Property
  Private m_righe As List(Of RigaOrdineExport)
End Class

Public Class RigaOrdineExport
  Public Property cod_um_1() As String
    Get
      Return m_cod_um_1
    End Get
    Set(value As String)
      m_cod_um_1 = value
    End Set
  End Property
  Private m_cod_um_1 As String
  Public Property cod_um_2() As String
    Get
      Return m_cod_um_2
    End Get
    Set(value As String)
      m_cod_um_2 = value
    End Set
  End Property
  Private m_cod_um_2 As String
  Public Property codice_articolo() As String
    Get
      Return m_codice_articolo
    End Get
    Set(value As String)
      m_codice_articolo = value
    End Set
  End Property
  Private m_codice_articolo As String
  Public Property data_consegna_riga() As System.Nullable(Of DateTime)
    Get
      Return m_data_consegna_riga
    End Get
    Set(value As System.Nullable(Of DateTime))
      m_data_consegna_riga = value
    End Set
  End Property
  Private m_data_consegna_riga As System.Nullable(Of DateTime)
  Public Property descrizione_riga() As String
    Get
      Return m_descrizione_riga
    End Get
    Set(value As String)
      m_descrizione_riga = value
    End Set
  End Property
  Private m_descrizione_riga As String
  Public Property ext_cod_tipo_riga_omag() As String
    Get
      Return m_ext_cod_tipo_riga_omag
    End Get
    Set(value As String)
      m_ext_cod_tipo_riga_omag = value
    End Set
  End Property
  Private m_ext_cod_tipo_riga_omag As String
  Public Property ext_cod_tipo_riga_ord() As String
    Get
      Return m_ext_cod_tipo_riga_ord
    End Get
    Set(value As String)
      m_ext_cod_tipo_riga_ord = value
    End Set
  End Property
  Private m_ext_cod_tipo_riga_ord As String
  Public Property fattore_conversione() As System.Nullable(Of Decimal)
    Get
      Return m_fattore_conversione
    End Get
    Set(value As System.Nullable(Of Decimal))
      m_fattore_conversione = value
    End Set
  End Property
  Private m_fattore_conversione As System.Nullable(Of Decimal)
  Public Property note() As String
    Get
      Return m_note
    End Get
    Set(value As String)
      m_note = value
    End Set
  End Property
  Private m_note As String
  Public Property prezzo() As System.Nullable(Of Decimal)
    Get
      Return m_prezzo
    End Get
    Set(value As System.Nullable(Of Decimal))
      m_prezzo = value
    End Set
  End Property
  Private m_prezzo As System.Nullable(Of Decimal)
  Public Property prezzo_2() As System.Nullable(Of Decimal)
    Get
      Return m_prezzo_2
    End Get
    Set(value As System.Nullable(Of Decimal))
      m_prezzo_2 = value
    End Set
  End Property
  Private m_prezzo_2 As System.Nullable(Of Decimal)
  Public Property qta() As System.Nullable(Of Decimal)
    Get
      Return m_qta
    End Get
    Set(value As System.Nullable(Of Decimal))
      m_qta = value
    End Set
  End Property
  Private m_qta As System.Nullable(Of Decimal)
  Public Property qta_2() As System.Nullable(Of Decimal)
    Get
      Return m_qta_2
    End Get
    Set(value As System.Nullable(Of Decimal))
      m_qta_2 = value
    End Set
  End Property
  Private m_qta_2 As System.Nullable(Of Decimal)
  Public Property sconto_1() As System.Nullable(Of Decimal)
    Get
      Return m_sconto_1
    End Get
    Set(value As System.Nullable(Of Decimal))
      m_sconto_1 = value
    End Set
  End Property
  Private m_sconto_1 As System.Nullable(Of Decimal)
  Public Property sconto_2() As System.Nullable(Of Decimal)
    Get
      Return m_sconto_2
    End Get
    Set(value As System.Nullable(Of Decimal))
      m_sconto_2 = value
    End Set
  End Property
  Private m_sconto_2 As System.Nullable(Of Decimal)
  Public Property sconto_3() As System.Nullable(Of Decimal)
    Get
      Return m_sconto_3
    End Get
    Set(value As System.Nullable(Of Decimal))
      m_sconto_3 = value
    End Set
  End Property
  Private m_sconto_3 As System.Nullable(Of Decimal)
  Public Property sconto_4() As System.Nullable(Of Decimal)
    Get
      Return m_sconto_4
    End Get
    Set(value As System.Nullable(Of Decimal))
      m_sconto_4 = value
    End Set
  End Property
  Private m_sconto_4 As System.Nullable(Of Decimal)
  Public Property sconto_5() As System.Nullable(Of Decimal)
    Get
      Return m_sconto_5
    End Get
    Set(value As System.Nullable(Of Decimal))
      m_sconto_5 = value
    End Set
  End Property
  Private m_sconto_5 As System.Nullable(Of Decimal)
  Public Property sconto_6() As System.Nullable(Of Decimal)
    Get
      Return m_sconto_6
    End Get
    Set(value As System.Nullable(Of Decimal))
      m_sconto_6 = value
    End Set
  End Property
  Private m_sconto_6 As System.Nullable(Of Decimal)
  Public Property sconto_importo() As System.Nullable(Of Decimal)
    Get
      Return m_sconto_importo
    End Get
    Set(value As System.Nullable(Of Decimal))
      m_sconto_importo = value
    End Set
  End Property
  Private m_sconto_importo As System.Nullable(Of Decimal)
  Public Property mag_perc_1() As System.Nullable(Of Decimal)
    Get
      Return m_mag_perc_1
    End Get
    Set(value As System.Nullable(Of Decimal))
      m_mag_perc_1 = value
    End Set
  End Property
  Private m_mag_perc_1 As System.Nullable(Of Decimal)
  Public Property mag_perc_2() As System.Nullable(Of Decimal)
    Get
      Return m_mag_perc_2
    End Get
    Set(value As System.Nullable(Of Decimal))
      m_mag_perc_2 = value
    End Set
  End Property
  Private m_mag_perc_2 As System.Nullable(Of Decimal)
  Public Property mag_importo() As System.Nullable(Of Decimal)
    Get
      Return m_mag_importo
    End Get
    Set(value As System.Nullable(Of Decimal))
      m_mag_importo = value
    End Set
  End Property
  Private m_mag_importo As System.Nullable(Of Decimal)
  Public Property codice_confezione() As String
    Get
      Return m_codice_confezione
    End Get
    Set(value As String)
      m_codice_confezione = value
    End Set
  End Property
  Private m_codice_confezione As String
  Public Property qta_confezione() As System.Nullable(Of Decimal)
    Get
      Return m_qta_confezione
    End Get
    Set(value As System.Nullable(Of Decimal))
      m_qta_confezione = value
    End Set
  End Property
  Private m_qta_confezione As System.Nullable(Of Decimal)
  Public Property tipo_um() As String
    Get
      Return m_tipo_um
    End Get
    Set(value As String)
      m_tipo_um = value
    End Set
  End Property
  Private m_tipo_um As String
  Public Property cod_combinazione() As String
    Get
      Return m_cod_combinazione
    End Get
    Set(value As String)
      m_cod_combinazione = value
    End Set
  End Property
  Private m_cod_combinazione As String
  Public Property cod_catalogo() As String
    Get
      Return m_cod_catalogo
    End Get
    Set(value As String)
      m_cod_catalogo = value
    End Set
  End Property
  Private m_cod_catalogo As String
  Public Property cod_materiale1() As String
    Get
      Return m_cod_materiale1
    End Get
    Set(value As String)
      m_cod_materiale1 = value
    End Set
  End Property
  Private m_cod_materiale1 As String
  Public Property cod_colore1() As String
    Get
      Return m_cod_colore1
    End Get
    Set(value As String)
      m_cod_colore1 = value
    End Set
  End Property
  Private m_cod_colore1 As String
  Public Property cod_materiale2() As String
    Get
      Return m_cod_materiale2
    End Get
    Set(value As String)
      m_cod_materiale2 = value
    End Set
  End Property
  Private m_cod_materiale2 As String
  Public Property cod_colore2() As String
    Get
      Return m_cod_colore2
    End Get
    Set(value As String)
      m_cod_colore2 = value
    End Set
  End Property
  Private m_cod_colore2 As String
  Public Property cod_materiale3() As String
    Get
      Return m_cod_materiale3
    End Get
    Set(value As String)
      m_cod_materiale3 = value
    End Set
  End Property
  Private m_cod_materiale3 As String
  Public Property cod_colore3() As String
    Get
      Return m_cod_colore3
    End Get
    Set(value As String)
      m_cod_colore3 = value
    End Set
  End Property
  Private m_cod_colore3 As String
  Public Property cod_materiale4() As String
    Get
      Return m_cod_materiale4
    End Get
    Set(value As String)
      m_cod_materiale4 = value
    End Set
  End Property
  Private m_cod_materiale4 As String
  Public Property cod_colore4() As String
    Get
      Return m_cod_colore4
    End Get
    Set(value As String)
      m_cod_colore4 = value
    End Set
  End Property
  Private m_cod_colore4 As String
  Public Property cod_materiale5() As String
    Get
      Return m_cod_materiale5
    End Get
    Set(value As String)
      m_cod_materiale5 = value
    End Set
  End Property
  Private m_cod_materiale5 As String
  Public Property cod_colore5() As String
    Get
      Return m_cod_colore5
    End Get
    Set(value As String)
      m_cod_colore5 = value
    End Set
  End Property
  Private m_cod_colore5 As String
  Public Property cod_materiale6() As String
    Get
      Return m_cod_materiale6
    End Get
    Set(value As String)
      m_cod_materiale6 = value
    End Set
  End Property
  Private m_cod_materiale6 As String
  Public Property cod_colore6() As String
    Get
      Return m_cod_colore6
    End Get
    Set(value As String)
      m_cod_colore6 = value
    End Set
  End Property
  Private m_cod_colore6 As String
  Public Property cod_materiale7() As String
    Get
      Return m_cod_materiale7
    End Get
    Set(value As String)
      m_cod_materiale7 = value
    End Set
  End Property
  Private m_cod_materiale7 As String
  Public Property cod_colore7() As String
    Get
      Return m_cod_colore7
    End Get
    Set(value As String)
      m_cod_colore7 = value
    End Set
  End Property
  Private m_cod_colore7 As String
  Public Property cod_materiale8() As String
    Get
      Return m_cod_materiale8
    End Get
    Set(value As String)
      m_cod_materiale8 = value
    End Set
  End Property
  Private m_cod_materiale8 As String
  Public Property cod_colore8() As String
    Get
      Return m_cod_colore8
    End Get
    Set(value As String)
      m_cod_colore8 = value
    End Set
  End Property
  Private m_cod_colore8 As String
  Public Property cod_materiale9() As String
    Get
      Return m_cod_materiale9
    End Get
    Set(value As String)
      m_cod_materiale9 = value
    End Set
  End Property
  Private m_cod_materiale9 As String
  Public Property cod_colore9() As String
    Get
      Return m_cod_colore9
    End Get
    Set(value As String)
      m_cod_colore9 = value
    End Set
  End Property
  Private m_cod_colore9 As String
  Public Property cod_materiale10() As String
    Get
      Return m_cod_materiale10
    End Get
    Set(value As String)
      m_cod_materiale10 = value
    End Set
  End Property
  Private m_cod_materiale10 As String
  Public Property cod_colore10() As String
    Get
      Return m_cod_colore10
    End Get
    Set(value As String)
      m_cod_colore10 = value
    End Set
  End Property
  Private m_cod_colore10 As String
  Public Property cod_materiale11() As String
    Get
      Return m_cod_materiale11
    End Get
    Set(value As String)
      m_cod_materiale11 = value
    End Set
  End Property
  Private m_cod_materiale11 As String
  Public Property cod_colore11() As String
    Get
      Return m_cod_colore11
    End Get
    Set(value As String)
      m_cod_colore11 = value
    End Set
  End Property
  Private m_cod_colore11 As String
  Public Property cod_materiale12() As String
    Get
      Return m_cod_materiale12
    End Get
    Set(value As String)
      m_cod_materiale12 = value
    End Set
  End Property
  Private m_cod_materiale12 As String
  Public Property cod_colore12() As String
    Get
      Return m_cod_colore12
    End Get
    Set(value As String)
      m_cod_colore12 = value
    End Set
  End Property
  Private m_cod_colore12 As String
  Public Property cod_materiale13() As String
    Get
      Return m_cod_materiale13
    End Get
    Set(value As String)
      m_cod_materiale13 = value
    End Set
  End Property
  Private m_cod_materiale13 As String
  Public Property cod_colore13() As String
    Get
      Return m_cod_colore13
    End Get
    Set(value As String)
      m_cod_colore13 = value
    End Set
  End Property
  Private m_cod_colore13 As String
  Public Property cod_materiale14() As String
    Get
      Return m_cod_materiale14
    End Get
    Set(value As String)
      m_cod_materiale14 = value
    End Set
  End Property
  Private m_cod_materiale14 As String
  Public Property cod_colore14() As String
    Get
      Return m_cod_colore14
    End Get
    Set(value As String)
      m_cod_colore14 = value
    End Set
  End Property
  Private m_cod_colore14 As String
  Public Property cod_materiale15() As String
    Get
      Return m_cod_materiale15
    End Get
    Set(value As String)
      m_cod_materiale15 = value
    End Set
  End Property
  Private m_cod_materiale15 As String
  Public Property cod_colore15() As String
    Get
      Return m_cod_colore15
    End Get
    Set(value As String)
      m_cod_colore15 = value
    End Set
  End Property
  Private m_cod_colore15 As String
  Public Property dettagli() As List(Of DettaglioRigaOrdineExport)
    Get
      Return m_dettagli
    End Get
    Set(value As List(Of DettaglioRigaOrdineExport))
      m_dettagli = value
    End Set
  End Property
  Private m_dettagli As List(Of DettaglioRigaOrdineExport)
End Class

Public Class DettaglioRigaOrdineExport
  Public Property cod_assortimento() As String
    Get
      Return m_cod_assortimento
    End Get
    Set(value As String)
      m_cod_assortimento = value
    End Set
  End Property
  Private m_cod_assortimento As String
  Public Property cod_sviluppo() As String
    Get
      Return m_cod_sviluppo
    End Get
    Set(value As String)
      m_cod_sviluppo = value
    End Set
  End Property
  Private m_cod_sviluppo As String
  Public Property cod_taglia() As String
    Get
      Return m_cod_taglia
    End Get
    Set(value As String)
      m_cod_taglia = value
    End Set
  End Property
  Private m_cod_taglia As String
  Public Property cod_taglia_guida() As String
    Get
      Return m_cod_taglia_guida
    End Get
    Set(value As String)
      m_cod_taglia_guida = value
    End Set
  End Property
  Private m_cod_taglia_guida As String
  Public Property ext_cod_tipo_det_riga_ord() As String
    Get
      Return m_ext_cod_tipo_det_riga_ord
    End Get
    Set(value As String)
      m_ext_cod_tipo_det_riga_ord = value
    End Set
  End Property
  Private m_ext_cod_tipo_det_riga_ord As String
  Public Property qta() As System.Nullable(Of Decimal)
    Get
      Return m_qta
    End Get
    Set(value As System.Nullable(Of Decimal))
      m_qta = value
    End Set
  End Property
  Private m_qta As System.Nullable(Of Decimal)
End Class
