Imports System.Collections.Generic
Imports System.Text

Public Class ws_rec_leads
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
  Public Property leads() As List(Of TestataLeadsExport)
    Get
      Return m_leads
    End Get
    Set(value As List(Of TestataLeadsExport))
      m_leads = value
    End Set
  End Property
  Private m_leads As List(Of TestataLeadsExport)
End Class


Public Class TestataLeadsExport
  Public Property cap() As String
    Get
      Return m_cap
    End Get
    Set(value As String)
      m_cap = value
    End Set
  End Property
  Private m_cap As String
  Public Property cellulare() As String
    Get
      Return m_cellulare
    End Get
    Set(value As String)
      m_cellulare = value
    End Set
  End Property
  Private m_cellulare As String
  Public Property citta() As String
    Get
      Return m_citta
    End Get
    Set(value As String)
      m_citta = value
    End Set
  End Property
  Private m_citta As String
  Public Property cod_agente() As String
    Get
      Return m_cod_agente
    End Get
    Set(value As String)
      m_cod_agente = value
    End Set
  End Property
  Private m_cod_agente As String
  Public Property cod_campagna() As String
    Get
      Return m_cod_campagna
    End Get
    Set(value As String)
      m_cod_campagna = value
    End Set
  End Property
  Private m_cod_campagna As String
  Public Property cod_ditta() As String
    Get
      Return m_cod_ditta
    End Get
    Set(value As String)
      m_cod_ditta = value
    End Set
  End Property
  Private m_cod_ditta As String
  Public Property cod_lead() As String
    Get
      Return m_cod_lead
    End Get
    Set(value As String)
      m_cod_lead = value
    End Set
  End Property
  Private m_cod_lead As String
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
  Public Property codice_fiscale() As String
    Get
      Return m_codice_fiscale
    End Get
    Set(value As String)
      m_codice_fiscale = value
    End Set
  End Property
  Private m_codice_fiscale As String
  Public Property descrizione1() As String
    Get
      Return m_descrizione1
    End Get
    Set(value As String)
      m_descrizione1 = value
    End Set
  End Property
  Private m_descrizione1 As String
  Public Property descrizione2() As String
    Get
      Return m_descrizione2
    End Get
    Set(value As String)
      m_descrizione2 = value
    End Set
  End Property
  Private m_descrizione2 As String
  Public Property email() As String
    Get
      Return m_email
    End Get
    Set(value As String)
      m_email = value
    End Set
  End Property
  Private m_email As String
  Public Property fax() As String
    Get
      Return m_fax
    End Get
    Set(value As String)
      m_fax = value
    End Set
  End Property
  Private m_fax As String
  Public Property id() As String
    Get
      Return m_id
    End Get
    Set(value As String)
      m_id = value
    End Set
  End Property
  Private m_id As String
  Public Property indirizzo() As String
    Get
      Return m_indirizzo
    End Get
    Set(value As String)
      m_indirizzo = value
    End Set
  End Property
  Private m_indirizzo As String
  Public Property internet() As String
    Get
      Return m_internet
    End Get
    Set(value As String)
      m_internet = value
    End Set
  End Property
  Private m_internet As String
  Public Property nazione() As String
    Get
      Return m_nazione
    End Get
    Set(value As String)
      m_nazione = value
    End Set
  End Property
  Private m_nazione As String
  Public Property note() As String
    Get
      Return m_note
    End Get
    Set(value As String)
      m_note = value
    End Set
  End Property
  Private m_note As String
  Public Property partita_iva() As String
    Get
      Return m_partita_iva
    End Get
    Set(value As String)
      m_partita_iva = value
    End Set
  End Property
  Private m_partita_iva As String
  Public Property provincia() As String
    Get
      Return m_provincia
    End Get
    Set(value As String)
      m_provincia = value
    End Set
  End Property
  Private m_provincia As String
  Public Property telefono() As String
    Get
      Return m_telefono
    End Get
    Set(value As String)
      m_telefono = value
    End Set
  End Property
  Private m_telefono As String
  Public Property utente() As String
    Get
      Return m_utente
    End Get
    Set(value As String)
      m_utente = value
    End Set
  End Property
  Private m_utente As String
  Public Property data_import() As System.Nullable(Of DateTime)
    Get
      Return m_data_import
    End Get
    Set(value As System.Nullable(Of DateTime))
      m_data_import = value
    End Set
  End Property
  Private m_data_import As System.Nullable(Of DateTime)
End Class
