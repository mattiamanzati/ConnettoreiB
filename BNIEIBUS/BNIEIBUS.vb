Imports System.Data
Imports System.IO
Imports ApexNetLIB
Imports Microsoft.Win32.TaskScheduler

Imports NTSInformatica.CLN__STD

Public Class FRMIEIBUS
    Private Moduli_P As Integer = bsModAll
    Private ModuliExt_P As Integer = 0
    Private ModuliSup_P As Integer = 0
    Private ModuliSupExt_P As Integer = 0
    Private ModuliPtn_P As Integer = 0
    Private ModuliPtnExt_P As Integer = 0

    Private IBUpdate As New ApexNetLIB.Download
    Private IBUpgradeAvailable As Boolean = False
    Private IBNewVersionDownloaded As Boolean = False


    Public ReadOnly Property Moduli() As Integer
        Get
            Return Moduli_P
        End Get
    End Property
    Public ReadOnly Property ModuliExt() As Integer
        Get
            Return ModuliExt_P
        End Get
    End Property
    Public ReadOnly Property ModuliSup() As Integer
        Get
            Return ModuliSup_P
        End Get
    End Property
    Public ReadOnly Property ModuliSupExt() As Integer
        Get
            Return ModuliSupExt_P
        End Get
    End Property
    Public ReadOnly Property ModuliPtn() As Integer
        Get
            Return ModuliPtn_P
        End Get
    End Property
    Public ReadOnly Property ModuliPtnExt() As Integer
        Get
            Return ModuliPtnExt_P
        End Get
    End Property

    Public oCleIbus As CLEIEIBUS


    Public oCallParams As CLE__CLDP
    Public oCallBase As CLA__DBAS


    Private components As System.ComponentModel.IContainer

    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMIEIBUS))
        Me.NtsBarManager1 = New NTSInformatica.NTSBarManager()
        Me.tlbMain = New NTSInformatica.NTSBar()
        Me.tlbElabora = New NTSInformatica.NTSBarButtonItem()
        Me.tlbLog = New NTSInformatica.NTSBarButtonItem()
        Me.tlbStrumenti = New NTSInformatica.NTSBarSubItem()
        Me.tlbCambioDitta = New NTSInformatica.NTSBarMenuItem()
        Me.tlbEsci = New NTSInformatica.NTSBarButtonItem()
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
        Me.tlbStampa = New NTSInformatica.NTSBarButtonItem()
        Me.tlbStampaVideo = New NTSInformatica.NTSBarButtonItem()
        Me.tlbGuida = New NTSInformatica.NTSBarButtonItem()
        Me.tlbImpostaStampante = New NTSInformatica.NTSBarButtonItem()
        Me.tlbCancella = New NTSInformatica.NTSBarMenuItem()
        Me.lbExport = New NTSInformatica.NTSLabel()
        Me.lbImport = New NTSInformatica.NTSLabel()
        Me.ckCli = New NTSInformatica.NTSCheckBox()
        Me.ckOrdini = New NTSInformatica.NTSCheckBox()
        Me.ckArti = New NTSInformatica.NTSCheckBox()
        Me.ckListini = New NTSInformatica.NTSCheckBox()
        Me.ckMagaz = New NTSInformatica.NTSCheckBox()
        Me.ckCatalogo = New NTSInformatica.NTSCheckBox()
        Me.ckCodpaga = New NTSInformatica.NTSCheckBox()
        Me.lbStatus = New NTSInformatica.NTSLabel()
        Me.ckSconti = New NTSInformatica.NTSCheckBox()
        Me.ckFor = New NTSInformatica.NTSCheckBox()
        Me.NtsProgressBar1 = New NTSInformatica.NTSProgressBar()
        Me.ckDoc = New NTSInformatica.NTSCheckBox()
        Me.lblRelease = New NTSInformatica.NTSLabel()
        Me.ckLeads = New NTSInformatica.NTSCheckBox()
        Me.ckOff = New NTSInformatica.NTSCheckBox()
        Me.ckCoordinate = New NTSInformatica.NTSCheckBox()
        Me.ckNotifichePush = New NTSInformatica.NTSCheckBox()
        Me.NtsLabel1 = New NTSInformatica.NTSLabel()
        Me.lblCustomRelease = New NTSInformatica.NTSLabel()
        Me.ckTabBase = New NTSInformatica.NTSCheckBox()

        CType(Me.NtsBarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ckCli.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ckOrdini.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ckArti.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ckListini.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ckMagaz.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ckCatalogo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ckCodpaga.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ckSconti.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ckFor.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NtsProgressBar1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ckDoc.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ckLeads.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ckOff.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ckCoordinate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ckNotifichePush.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ckTabBase.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'frmPopup
        '
        Me.frmPopup.Appearance.BackColor = System.Drawing.Color.Red
        Me.frmPopup.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical
        Me.frmPopup.Appearance.Options.UseBackColor = True
        Me.frmPopup.Appearance.Options.UseImage = True
        '
        'frmAuto
        '
        Me.frmAuto.Appearance.BackColor = System.Drawing.Color.Black
        Me.frmAuto.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical
        Me.frmAuto.Appearance.Options.UseBackColor = True
        Me.frmAuto.Appearance.Options.UseImage = True
        '
        'NtsBarManager1
        '
        Me.NtsBarManager1.AllowCustomization = False
        Me.NtsBarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.tlbMain})
        Me.NtsBarManager1.DockControls.Add(Me.barDockControlTop)
        Me.NtsBarManager1.DockControls.Add(Me.barDockControlBottom)
        Me.NtsBarManager1.DockControls.Add(Me.barDockControlLeft)
        Me.NtsBarManager1.DockControls.Add(Me.barDockControlRight)
        Me.NtsBarManager1.Form = Me
        Me.NtsBarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.tlbStampa, Me.tlbStampaVideo, Me.tlbGuida, Me.tlbEsci, Me.tlbStrumenti, Me.tlbImpostaStampante, Me.tlbCambioDitta, Me.tlbCancella, Me.tlbElabora, Me.tlbLog})
        Me.NtsBarManager1.MaxItemId = 22
        '
        'tlbMain
        '
        Me.tlbMain.BarName = "tlbMain"
        Me.tlbMain.DockCol = 0
        Me.tlbMain.DockRow = 0
        Me.tlbMain.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
        Me.tlbMain.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.tlbElabora, True), New DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.None, False, Me.tlbLog, False), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbStrumenti, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbEsci)})
        Me.tlbMain.OptionsBar.AllowQuickCustomization = False
        Me.tlbMain.OptionsBar.DisableClose = True
        Me.tlbMain.OptionsBar.DrawDragBorder = False
        Me.tlbMain.OptionsBar.UseWholeRow = True
        Me.tlbMain.Text = "tlbMain"
        '
        'tlbElabora
        '
        Me.tlbElabora.Caption = "Elabora"
        Me.tlbElabora.Glyph = CType(resources.GetObject("tlbElabora.Glyph"), System.Drawing.Image)
        Me.tlbElabora.Id = 19
        Me.tlbElabora.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F7)
        Me.tlbElabora.Name = "tlbElabora"
        Me.tlbElabora.Visible = True
        '
        'tlbLog
        '
        Me.tlbLog.Caption = "Log"
        Me.tlbLog.Glyph = CType(resources.GetObject("tlbLog.Glyph"), System.Drawing.Image)
        Me.tlbLog.Id = 99
        Me.tlbLog.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F7)
        Me.tlbLog.Name = "tlbLog"
        Me.tlbLog.Visible = False
        '
        'tlbStrumenti
        '
        Me.tlbStrumenti.Caption = "Strumenti"
        Me.tlbStrumenti.Glyph = CType(resources.GetObject("tlbStrumenti.Glyph"), System.Drawing.Image)
        Me.tlbStrumenti.Id = 15
        Me.tlbStrumenti.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.tlbCambioDitta)})
        Me.tlbStrumenti.Name = "tlbStrumenti"
        Me.tlbStrumenti.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionInMenu
        Me.tlbStrumenti.Visible = True
        '
        'tlbCambioDitta
        '
        Me.tlbCambioDitta.Caption = "Cambio ditta"
        Me.tlbCambioDitta.Id = 17
        Me.tlbCambioDitta.ItemShortcut = New DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D))
        Me.tlbCambioDitta.Name = "tlbCambioDitta"
        Me.tlbCambioDitta.NTSIsCheckBox = False
        Me.tlbCambioDitta.Visible = True
        '
        'tlbEsci
        '
        Me.tlbEsci.Caption = "Esci"
        Me.tlbEsci.Glyph = CType(resources.GetObject("tlbEsci.Glyph"), System.Drawing.Image)
        Me.tlbEsci.Id = 12
        Me.tlbEsci.Name = "tlbEsci"
        Me.tlbEsci.Visible = True
        '
        'tlbStampa
        '
        Me.tlbStampa.Caption = "Stampa"
        Me.tlbStampa.Glyph = CType(resources.GetObject("tlbStampa.Glyph"), System.Drawing.Image)
        Me.tlbStampa.Id = 4
        Me.tlbStampa.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F6)
        Me.tlbStampa.Name = "tlbStampa"
        Me.tlbStampa.Visible = True
        '
        'tlbStampaVideo
        '
        Me.tlbStampaVideo.Caption = "Stampa video"
        Me.tlbStampaVideo.Glyph = CType(resources.GetObject("tlbStampaVideo.Glyph"), System.Drawing.Image)
        Me.tlbStampaVideo.Id = 5
        Me.tlbStampaVideo.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F12)
        Me.tlbStampaVideo.Name = "tlbStampaVideo"
        Me.tlbStampaVideo.Visible = True
        '
        'tlbGuida
        '
        Me.tlbGuida.Caption = "Guida"
        Me.tlbGuida.Glyph = CType(resources.GetObject("tlbGuida.Glyph"), System.Drawing.Image)
        Me.tlbGuida.Id = 11
        Me.tlbGuida.Name = "tlbGuida"
        Me.tlbGuida.Visible = True
        '
        'tlbImpostaStampante
        '
        Me.tlbImpostaStampante.Caption = "Imposta Stampante"
        Me.tlbImpostaStampante.Id = 16
        Me.tlbImpostaStampante.Name = "tlbImpostaStampante"
        Me.tlbImpostaStampante.Visible = True
        '
        'tlbCancella
        '
        Me.tlbCancella.Caption = "Cancella"
        Me.tlbCancella.Id = 18
        Me.tlbCancella.Name = "tlbCancella"
        Me.tlbCancella.NTSIsCheckBox = False
        Me.tlbCancella.Visible = True
        '
        'lbExport
        '
        Me.lbExport.AutoSize = True
        Me.lbExport.BackColor = System.Drawing.Color.Transparent
        Me.lbExport.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbExport.Location = New System.Drawing.Point(12, 41)
        Me.lbExport.Margin = New System.Windows.Forms.Padding(3, 2, 3, 3)
        Me.lbExport.Name = "lbExport"
        Me.lbExport.NTSDbField = ""
        Me.lbExport.Size = New System.Drawing.Size(44, 13)
        Me.lbExport.TabIndex = 10
        Me.lbExport.Text = "Export"
        Me.lbExport.Tooltip = ""
        Me.lbExport.UseMnemonic = False
        '
        'lbImport
        '
        Me.lbImport.AutoSize = True
        Me.lbImport.BackColor = System.Drawing.Color.Transparent
        Me.lbImport.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbImport.Location = New System.Drawing.Point(12, 222)
        Me.lbImport.Margin = New System.Windows.Forms.Padding(3, 2, 3, 3)
        Me.lbImport.Name = "lbImport"
        Me.lbImport.NTSDbField = ""
        Me.lbImport.Size = New System.Drawing.Size(47, 13)
        Me.lbImport.TabIndex = 11
        Me.lbImport.Text = "Import"
        Me.lbImport.Tooltip = ""
        Me.lbImport.UseMnemonic = False
        '
        'ckCli
        '
        Me.ckCli.Cursor = System.Windows.Forms.Cursors.Default
        Me.ckCli.Location = New System.Drawing.Point(25, 81)
        Me.ckCli.Margin = New System.Windows.Forms.Padding(3, 1, 3, 3)
        Me.ckCli.Name = "ckCli"
        Me.ckCli.NTSCheckValue = "S"
        Me.ckCli.NTSUnCheckValue = "N"
        Me.ckCli.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.ckCli.Properties.Appearance.Options.UseBackColor = True
        Me.ckCli.Properties.Caption = "Clienti"
        Me.ckCli.Size = New System.Drawing.Size(107, 19)
        Me.ckCli.TabIndex = 14
        '
        'ckOrdini
        '
        Me.ckOrdini.Cursor = System.Windows.Forms.Cursors.Default
        Me.ckOrdini.Location = New System.Drawing.Point(25, 239)
        Me.ckOrdini.Margin = New System.Windows.Forms.Padding(3, 1, 3, 3)
        Me.ckOrdini.Name = "ckOrdini"
        Me.ckOrdini.NTSCheckValue = "S"
        Me.ckOrdini.NTSUnCheckValue = "N"
        Me.ckOrdini.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.ckOrdini.Properties.Appearance.Options.UseBackColor = True
        Me.ckOrdini.Properties.Caption = "Ordini,  preventivi, note, anagr. clienti, leads, ecc"
        Me.ckOrdini.Size = New System.Drawing.Size(337, 19)
        Me.ckOrdini.TabIndex = 15
        '
        'ckArti
        '
        Me.ckArti.Cursor = System.Windows.Forms.Cursors.Default
        Me.ckArti.Location = New System.Drawing.Point(261, 81)
        Me.ckArti.Margin = New System.Windows.Forms.Padding(3, 1, 3, 3)
        Me.ckArti.Name = "ckArti"
        Me.ckArti.NTSCheckValue = "S"
        Me.ckArti.NTSUnCheckValue = "N"
        Me.ckArti.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.ckArti.Properties.Appearance.Options.UseBackColor = True
        Me.ckArti.Properties.Caption = "Articoli"
        Me.ckArti.Size = New System.Drawing.Size(65, 19)
        Me.ckArti.TabIndex = 16
        '
        'ckListini
        '
        Me.ckListini.Cursor = System.Windows.Forms.Cursors.Default
        Me.ckListini.Location = New System.Drawing.Point(261, 127)
        Me.ckListini.Margin = New System.Windows.Forms.Padding(3, 1, 3, 3)
        Me.ckListini.Name = "ckListini"
        Me.ckListini.NTSCheckValue = "S"
        Me.ckListini.NTSUnCheckValue = "N"
        Me.ckListini.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.ckListini.Properties.Appearance.Options.UseBackColor = True
        Me.ckListini.Properties.Caption = "Listini"
        Me.ckListini.Size = New System.Drawing.Size(56, 19)
        Me.ckListini.TabIndex = 17
        '
        'ckMagaz
        '
        Me.ckMagaz.Cursor = System.Windows.Forms.Cursors.Default
        Me.ckMagaz.Location = New System.Drawing.Point(261, 104)
        Me.ckMagaz.Margin = New System.Windows.Forms.Padding(3, 1, 3, 3)
        Me.ckMagaz.Name = "ckMagaz"
        Me.ckMagaz.NTSCheckValue = "S"
        Me.ckMagaz.NTSUnCheckValue = "N"
        Me.ckMagaz.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.ckMagaz.Properties.Appearance.Options.UseBackColor = True
        Me.ckMagaz.Properties.Caption = "Giacenze di magazzino"
        Me.ckMagaz.Size = New System.Drawing.Size(138, 19)
        Me.ckMagaz.TabIndex = 18
        '
        'ckCatalogo
        '
        Me.ckCatalogo.Cursor = System.Windows.Forms.Cursors.Default
        Me.ckCatalogo.Location = New System.Drawing.Point(25, 173)
        Me.ckCatalogo.Margin = New System.Windows.Forms.Padding(3, 1, 3, 3)
        Me.ckCatalogo.Name = "ckCatalogo"
        Me.ckCatalogo.NTSCheckValue = "S"
        Me.ckCatalogo.NTSUnCheckValue = "N"
        Me.ckCatalogo.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.ckCatalogo.Properties.Appearance.Options.UseBackColor = True
        Me.ckCatalogo.Properties.Caption = "Catalogo"
        Me.ckCatalogo.Size = New System.Drawing.Size(71, 19)
        Me.ckCatalogo.TabIndex = 19
        '
        'ckCodpaga
        '
        Me.ckCodpaga.Cursor = System.Windows.Forms.Cursors.Default
        Me.ckCodpaga.Location = New System.Drawing.Point(25, 150)
        Me.ckCodpaga.Margin = New System.Windows.Forms.Padding(3, 1, 3, 3)
        Me.ckCodpaga.Name = "ckCodpaga"
        Me.ckCodpaga.NTSCheckValue = "S"
        Me.ckCodpaga.NTSUnCheckValue = "N"
        Me.ckCodpaga.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.ckCodpaga.Properties.Appearance.Options.UseBackColor = True
        Me.ckCodpaga.Properties.Caption = "Condizioni di pagamento"
        Me.ckCodpaga.Size = New System.Drawing.Size(149, 19)
        Me.ckCodpaga.TabIndex = 20
        '
        'lbStatus
        '
        Me.lbStatus.AutoSize = True
        Me.lbStatus.BackColor = System.Drawing.Color.Transparent
        Me.lbStatus.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbStatus.Location = New System.Drawing.Point(9, 363)
        Me.lbStatus.Margin = New System.Windows.Forms.Padding(3, 2, 3, 3)
        Me.lbStatus.Name = "lbStatus"
        Me.lbStatus.NTSDbField = ""
        Me.lbStatus.Size = New System.Drawing.Size(43, 13)
        Me.lbStatus.TabIndex = 23
        Me.lbStatus.Text = "Pronto."
        Me.lbStatus.Tooltip = ""
        Me.lbStatus.UseMnemonic = False
        '
        'ckSconti
        '
        Me.ckSconti.Cursor = System.Windows.Forms.Cursors.Default
        Me.ckSconti.Location = New System.Drawing.Point(261, 150)
        Me.ckSconti.Margin = New System.Windows.Forms.Padding(3, 1, 3, 3)
        Me.ckSconti.Name = "ckSconti"
        Me.ckSconti.NTSCheckValue = "S"
        Me.ckSconti.NTSUnCheckValue = "N"
        Me.ckSconti.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.ckSconti.Properties.Appearance.Options.UseBackColor = True
        Me.ckSconti.Properties.Caption = "Sconti"
        Me.ckSconti.Size = New System.Drawing.Size(56, 19)
        Me.ckSconti.TabIndex = 24
        '
        'ckFor
        '
        Me.ckFor.Cursor = System.Windows.Forms.Cursors.Default
        Me.ckFor.Location = New System.Drawing.Point(25, 104)
        Me.ckFor.Margin = New System.Windows.Forms.Padding(3, 1, 3, 3)
        Me.ckFor.Name = "ckFor"
        Me.ckFor.NTSCheckValue = "S"
        Me.ckFor.NTSUnCheckValue = "N"
        Me.ckFor.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.ckFor.Properties.Appearance.Options.UseBackColor = True
        Me.ckFor.Properties.Caption = "Fornitori"
        Me.ckFor.Size = New System.Drawing.Size(107, 19)
        Me.ckFor.TabIndex = 25
        '
        'NtsProgressBar1
        '
        Me.NtsProgressBar1.Cursor = System.Windows.Forms.Cursors.Default
        Me.NtsProgressBar1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.NtsProgressBar1.Location = New System.Drawing.Point(0, 391)
        Me.NtsProgressBar1.Name = "NtsProgressBar1"
        Me.NtsProgressBar1.Size = New System.Drawing.Size(498, 16)
        Me.NtsProgressBar1.TabIndex = 27
        Me.NtsProgressBar1.TabStop = False
        Me.NtsProgressBar1.Visible = False
        '
        'ckDoc
        '
        Me.ckDoc.Cursor = System.Windows.Forms.Cursors.Default
        Me.ckDoc.Location = New System.Drawing.Point(25, 127)
        Me.ckDoc.Margin = New System.Windows.Forms.Padding(3, 1, 3, 3)
        Me.ckDoc.Name = "ckDoc"
        Me.ckDoc.NTSCheckValue = "S"
        Me.ckDoc.NTSUnCheckValue = "N"
        Me.ckDoc.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.ckDoc.Properties.Appearance.Options.UseBackColor = True
        Me.ckDoc.Properties.Caption = "Documenti e Scadenze"
        Me.ckDoc.Size = New System.Drawing.Size(148, 19)
        Me.ckDoc.TabIndex = 28
        '
        'lblRelease
        '
        Me.lblRelease.AutoSize = True
        Me.lblRelease.BackColor = System.Drawing.Color.Transparent
        Me.lblRelease.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRelease.Location = New System.Drawing.Point(398, 363)
        Me.lblRelease.Margin = New System.Windows.Forms.Padding(3, 2, 3, 3)
        Me.lblRelease.Name = "lblRelease"
        Me.lblRelease.NTSDbField = ""
        Me.lblRelease.Size = New System.Drawing.Size(49, 13)
        Me.lblRelease.TabIndex = 29
        Me.lblRelease.Text = "99.9.9.9"
        Me.lblRelease.Tooltip = ""
        Me.lblRelease.UseMnemonic = False
        '
        'ckLeads
        '
        Me.ckLeads.Cursor = System.Windows.Forms.Cursors.Default
        Me.ckLeads.Location = New System.Drawing.Point(25, 58)
        Me.ckLeads.Margin = New System.Windows.Forms.Padding(3, 1, 3, 3)
        Me.ckLeads.Name = "ckLeads"
        Me.ckLeads.NTSCheckValue = "S"
        Me.ckLeads.NTSUnCheckValue = "N"
        Me.ckLeads.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.ckLeads.Properties.Appearance.Options.UseBackColor = True
        Me.ckLeads.Properties.Caption = "Leads"
        Me.ckLeads.Size = New System.Drawing.Size(107, 19)
        Me.ckLeads.TabIndex = 30
        '
        'ckOff
        '
        Me.ckOff.Cursor = System.Windows.Forms.Cursors.Default
        Me.ckOff.Location = New System.Drawing.Point(261, 58)
        Me.ckOff.Margin = New System.Windows.Forms.Padding(3, 1, 3, 3)
        Me.ckOff.Name = "ckOff"
        Me.ckOff.NTSCheckValue = "S"
        Me.ckOff.NTSUnCheckValue = "N"
        Me.ckOff.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.ckOff.Properties.Appearance.Options.UseBackColor = True
        Me.ckOff.Properties.Caption = "Offerte"
        Me.ckOff.Size = New System.Drawing.Size(65, 19)
        Me.ckOff.TabIndex = 31
        '
        'ckCoordinate
        '
        Me.ckCoordinate.Cursor = System.Windows.Forms.Cursors.Default
        Me.ckCoordinate.Location = New System.Drawing.Point(25, 304)
        Me.ckCoordinate.Margin = New System.Windows.Forms.Padding(3, 1, 3, 3)
        Me.ckCoordinate.Name = "ckCoordinate"
        Me.ckCoordinate.NTSCheckValue = "S"
        Me.ckCoordinate.NTSUnCheckValue = "N"
        Me.ckCoordinate.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.ckCoordinate.Properties.Appearance.Options.UseBackColor = True
        Me.ckCoordinate.Properties.Caption = "Identificazione delle coordinate geografiche dei clienti"
        Me.ckCoordinate.Size = New System.Drawing.Size(321, 19)
        Me.ckCoordinate.TabIndex = 32
        Me.ckCoordinate.ToolTip = "Identifica le coordinate geografiche del cliente in base all'indirizzo inserito i" & _
    "n anagrafica.  La localizzazione si basa sui servizi di Google."
        '
        'ckNotifichePush
        '
        Me.ckNotifichePush.Cursor = System.Windows.Forms.Cursors.Default
        Me.ckNotifichePush.Location = New System.Drawing.Point(25, 327)
        Me.ckNotifichePush.Margin = New System.Windows.Forms.Padding(3, 1, 3, 3)
        Me.ckNotifichePush.Name = "ckNotifichePush"
        Me.ckNotifichePush.NTSCheckValue = "S"
        Me.ckNotifichePush.NTSUnCheckValue = "N"
        Me.ckNotifichePush.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.ckNotifichePush.Properties.Appearance.Options.UseBackColor = True
        Me.ckNotifichePush.Properties.Caption = "Notifiche push (insoluti)"
        Me.ckNotifichePush.Size = New System.Drawing.Size(321, 19)
        Me.ckNotifichePush.TabIndex = 32
        Me.ckNotifichePush.ToolTip = "Invia notifiche push agli agenti"
        '
        'NtsLabel1
        '
        Me.NtsLabel1.AutoSize = True
        Me.NtsLabel1.BackColor = System.Drawing.Color.Transparent
        Me.NtsLabel1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NtsLabel1.Location = New System.Drawing.Point(12, 287)
        Me.NtsLabel1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 3)
        Me.NtsLabel1.Name = "NtsLabel1"
        Me.NtsLabel1.NTSDbField = ""
        Me.NtsLabel1.Size = New System.Drawing.Size(48, 13)
        Me.NtsLabel1.TabIndex = 33
        Me.NtsLabel1.Text = "Opzioni"
        Me.NtsLabel1.Tooltip = ""
        Me.NtsLabel1.UseMnemonic = False
        '
        'lblCustomRelease
        '
        Me.lblCustomRelease.AutoSize = True
        Me.lblCustomRelease.BackColor = System.Drawing.Color.Transparent
        Me.lblCustomRelease.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCustomRelease.Location = New System.Drawing.Point(339, 35)
        Me.lblCustomRelease.Margin = New System.Windows.Forms.Padding(3, 2, 3, 3)
        Me.lblCustomRelease.Name = "lblCustomRelease"
        Me.lblCustomRelease.NTSDbField = ""
        Me.lblCustomRelease.Size = New System.Drawing.Size(98, 13)
        Me.lblCustomRelease.TabIndex = 34
        Me.lblCustomRelease.Text = "CMATIC / 10.1.1.0"
        Me.lblCustomRelease.Tooltip = ""
        Me.lblCustomRelease.UseMnemonic = False
        '
        'ckTabBase
        '
        Me.ckTabBase.Cursor = System.Windows.Forms.Cursors.Default
        Me.ckTabBase.Location = New System.Drawing.Point(261, 173)
        Me.ckTabBase.Margin = New System.Windows.Forms.Padding(3, 1, 3, 3)
        Me.ckTabBase.Name = "ckTabBase"
        Me.ckTabBase.NTSCheckValue = "S"
        Me.ckTabBase.NTSUnCheckValue = "N"
        Me.ckTabBase.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.ckTabBase.Properties.Appearance.Options.UseBackColor = True
        Me.ckTabBase.Properties.Caption = "Tabelle di base (Città, Comuni, ecc...)"
        Me.ckTabBase.Size = New System.Drawing.Size(214, 19)
        Me.ckTabBase.TabIndex = 35
        '
        'FRMIEIBUS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(498, 407)
        Me.Controls.Add(Me.ckTabBase)
        Me.Controls.Add(Me.lblCustomRelease)
        Me.Controls.Add(Me.NtsLabel1)
        Me.Controls.Add(Me.ckCoordinate)
        Me.Controls.Add(Me.ckNotifichePush)
        Me.Controls.Add(Me.ckOff)
        Me.Controls.Add(Me.ckLeads)
        Me.Controls.Add(Me.lblRelease)
        Me.Controls.Add(Me.ckDoc)
        Me.Controls.Add(Me.NtsProgressBar1)
        Me.Controls.Add(Me.ckFor)
        Me.Controls.Add(Me.ckSconti)
        Me.Controls.Add(Me.lbStatus)
        Me.Controls.Add(Me.ckCodpaga)
        Me.Controls.Add(Me.ckCatalogo)
        Me.Controls.Add(Me.ckMagaz)
        Me.Controls.Add(Me.ckListini)
        Me.Controls.Add(Me.ckArti)
        Me.Controls.Add(Me.ckOrdini)
        Me.Controls.Add(Me.ckCli)
        Me.Controls.Add(Me.lbImport)
        Me.Controls.Add(Me.lbExport)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.HelpButton = True
        Me.MaximizeBox = False
        Me.Name = "FRMIEIBUS"
        Me.Text = "IMPORT / EXPORT VS IBUS"

        CType(Me.NtsBarManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ckCli.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ckOrdini.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ckArti.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ckListini.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ckMagaz.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ckCatalogo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ckCodpaga.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ckSconti.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ckFor.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NtsProgressBar1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ckDoc.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ckLeads.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ckOff.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ckCoordinate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ckNotifichePush.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ckTabBase.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Public Overloads Overrides Sub GestisciEventiEntity(ByVal sender As Object, ByRef e As NTSEventArgs)
        Try
            If Not IsMyThrowRemoteEvent() Then Return 'il messaggio non è per questa form ...
            MyBase.GestisciEventiEntity(sender, e)

            If e.TipoEvento.Trim.Length < 10 Then Return
            Select Case e.TipoEvento.Substring(0, 10)
                Case "AGGIOLABEL"
                    lbStatus.Text = e.Message
                    Me.Refresh()
                Case "AGGIOLOG"
                    lbStatus.Text = e.Message
                    'lstLog.Text = lstLog.Text + vbCrLf + e.Message
                    Me.Refresh()
                Case "PROGRESSBA"
                    NtsProgressBar1.Position = NTSCInt(e.Message)
                    Me.Refresh()
            End Select

        Catch ex As Exception
            '-------------------------------------------------
            Dim strErr As String = GestError(ex, Me, "", oApp.InfoError, oApp.ErrorLogFile, True)
            '-------------------------------------------------
        End Try
    End Sub

    Public Overloads Function Init(ByRef Menu As CLE__MENU, ByRef Param As CLE__CLDP, Optional ByVal Ditta As String = "", Optional ByRef SharedControls As CLE__EVNT = Nothing) As Boolean
        oMenu = Menu
        oApp = oMenu.App
        oCallParams = Param
        If Ditta <> "" Then
            DittaCorrente = Ditta
        Else
            DittaCorrente = oApp.Ditta
        End If
        Me.GctlTipoDoc = ""

        InitializeComponent()
        Me.MinimumSize = Me.Size

        '------------------------------------------------
        'creo e attivo l'entity e inizializzo la funzione che dovrà rilevare gli eventi dall'ENTITY
        Dim strErr As String = ""
        Dim oTmp As Object = Nothing
        If CLN__STD.NTSIstanziaDll(oApp.ServerDir, oApp.NetDir, "BNIEIBUS", "BEIEIBUS", oTmp, strErr, False, "", "") = False Then
            oApp.MsgBoxErr(oApp.Tr(Me, 128265803653608595, "ERRORE in fase di creazione Entity:" & vbCrLf & "|" & strErr & "|"))
            Return False
        End If
        oCleIbus = CType(oTmp, CLEIEIBUS)
        '------------------------------------------------
        bRemoting = Menu.Remoting("BNIEIBUS", strRemoteServer, strRemotePort)
        AddHandler oCleIbus.RemoteEvent, AddressOf GestisciEventiEntity
        If oCleIbus.Init(oApp, oScript, oMenu.oCleComm, "", bRemoting, strRemoteServer, strRemotePort) = False Then Return False

        Return True
    End Function

    Public Overridable Sub InitControls()
        InitControlsBeginEndInit(Me, False)

        Try
            '-------------------------------------------------
            'carico le immagini della toolbar
            Try
                tlbElabora.Glyph = Bitmap.FromFile(oApp.ChildImageDir & "\elabora.gif")
                tlbLog.Glyph = Bitmap.FromFile(oApp.ChildImageDir & "\prnscreen.gif")
                tlbStrumenti.Glyph = Bitmap.FromFile(oApp.ChildImageDir & "\options.gif")
                tlbStampa.Glyph = Bitmap.FromFile(oApp.ChildImageDir & "\print.gif")
                tlbStampaVideo.Glyph = Bitmap.FromFile(oApp.ChildImageDir & "\prnscreen.gif")
                tlbGuida.Glyph = Bitmap.FromFile(oApp.ChildImageDir & "\help.gif")
                tlbEsci.Glyph = Bitmap.FromFile(oApp.ChildImageDir & "\exit.gif")
            Catch ex As Exception
                'non gestisco l'errore: se non c'è una immagine prendo quella standard
            End Try
            tlbMain.NTSSetToolTip()

            ' ADD CK
            ckTabBase.NTSSetParam(oMenu, oApp.Tr(Me, 129877043589480089, "Tabelle di base (Città, Comuni, ecc...)"), "S", "N")
            ckCodpaga.NTSSetParam(oMenu, oApp.Tr(Me, 129877043589636090, "Condizioni di pagamento"), "S", "N")
            ckCatalogo.NTSSetParam(oMenu, oApp.Tr(Me, 129877043589792090, "Catalogo"), "S", "N")
            ckMagaz.NTSSetParam(oMenu, oApp.Tr(Me, 129877043589948090, "Giacenze di magazzino"), "S", "N")
            ckListini.NTSSetParam(oMenu, oApp.Tr(Me, 129877043590104090, "Listini"), "S", "N")
            ckSconti.NTSSetParam(oMenu, oApp.Tr(Me, 129877043590104091, "Sconti"), "S", "N")
            ckArti.NTSSetParam(oMenu, oApp.Tr(Me, 129877043590260091, "Articoli"), "S", "N")
            ckCli.NTSSetParam(oMenu, oApp.Tr(Me, 129877043590572099, "Clienti"), "S", "N")
            ckFor.NTSSetParam(oMenu, oApp.Tr(Me, 129877043590572091, "Fornitori"), "S", "N")
            ckDoc.NTSSetParam(oMenu, oApp.Tr(Me, 129877043590572092, "Documenti e Scadenze"), "S", "N")
            ckOrdini.NTSSetParam(oMenu, oApp.Tr(Me, 129877043590416091, "Ordini / preventivi"), "S", "N")
            ckLeads.NTSSetParam(oMenu, oApp.Tr(Me, 129877043590416098, "Leads"), "S", "N")
            ckOff.NTSSetParam(oMenu, oApp.Tr(Me, 129877043590416092, "Offerte"), "S", "N")
            ckCoordinate.NTSSetParam(oMenu, oApp.Tr(Me, 129877043590416093, "Coordinate di Google"), "S", "N")
            ckNotifichePush.NTSSetParam(oMenu, oApp.Tr(Me, 129877043590416094, "Notifiche push"), "S", "N")

            '-------------------------------------------------
            'chiamo lo script per inizializzare i controlli caricati con source ext
            NTSScriptExec("InitControls", Me, Nothing)
        Catch ex As Exception
            '-------------------------------------------------
            Dim strErr As String = CLN__STD.GestError(ex, Me, "", oApp.InfoError, oApp.ErrorLogFile, True)
            '-------------------------------------------------
        End Try
        InitControlsBeginEndInit(Me, True)
    End Sub

    Public Overridable Function LeggiDatiDitta() As Boolean
        Dim bDbMultiDitta As Boolean = False
        Dim bAnagen As Boolean = False
        Dim nCausale As Integer = 0
        Dim lConto As Integer = 0

        Try
Riprova:
            '-------------------------------------------------
            'se ci sono le caratteristiche visualizzo lo zoom per selezionare le ditte
            DittaCorrente = oMenu.CambioDitta(oCallParams, DittaCorrente, "BNIEIBUS", "", Moduli, ModuliExt, ModuliSup, ModuliSupExt, ModuliPtn, ModuliPtnExt, bAnagen, bDbMultiDitta)
            If DittaCorrente = "" Then Return False

            If bAnagen = False Or bDbMultiDitta = False Then
                tlbCambioDitta.Visible = False
            Else
                GctlSetVisEnab(tlbCambioDitta, True)
            End If

            '-------------------------------------------------
            'leggo le informazioni relative alla ditta corrente
            Me.Text = oMenu.SetCaptionDitt(DittaCorrente, Me.Text)
            oCleIbus.strDittaCorrente = DittaCorrente

            GctlApplicaDefaultValue()

            Return True
        Catch ex As Exception
            '-------------------------------------------------
            Dim strErr As String = GestError(ex, Me, "", oApp.InfoError, oApp.ErrorLogFile, True)
            '-------------------------------------------------
        End Try
    End Function

#Region "Eventi di Form"

    Private Sub FRMIEIBUS_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        If IBUpgradeAvailable And IBNewVersionDownloaded Then
            WEDOLogger.WriteToRegistry("Creo ibupdate.bat e lo eseguo")
            Dim funge As Boolean = IBRunUpdate()

            If Not funge Then
                WEDOLogger.WriteToRegistry("IBRunUpdate() ha fallito la copia degli aggiornamenti da " & IBUpdate.vars_unzipdir, EventLogEntryType.Error)
            End If
        End If


    End Sub



    Public Overridable Sub FRMIEIBUS_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            '-------------------------------------------------
            'predispongo i controlli
            InitControls()
            NtsProgressBar1.Position = 0

            '-------------------------------------------------
            'sempre alla fine di questa funzione: applico le regole della gctl
            GctlSetRoules()

            ' Questa label viene valorizzata solo nell'eventuale componente custom dell'interfaccia
            lblCustomRelease.Text = ""

            'CHIAMO LA PROCEDURA CHE CONTROLLA LA PRESENZA DI AGGIORNAMENTI

            Dim ProxyUrl As String = oMenu.GetSettingBusDitt(DittaCorrente, "Bsieibus", "Opzioni", ".", "ProxyUrl", "", " ", "")
            Dim ProxyUsername As String = oMenu.GetSettingBusDitt(DittaCorrente, "Bsieibus", "Opzioni", ".", "ProxyUsername", "", " ", "")
            Dim ProxyPassword As String = oMenu.GetSettingBusDitt(DittaCorrente, "Bsieibus", "Opzioni", ".", "ProxyPassword", "", " ", "")
            Dim ProxyPort As String = oMenu.GetSettingBusDitt(DittaCorrente, "Bsieibus", "Opzioni", ".", "ProxyPort", "", " ", "")
            Dim CheckboxAttivi As String = oMenu.GetSettingBusDitt(DittaCorrente, "Bsieibus", "Opzioni", ".", "CheckboxAttivi", "", " ", "")
            Dim CheckboxDisabilitati As String = oMenu.GetSettingBusDitt(DittaCorrente, "Bsieibus", "Opzioni", ".", "CheckboxDisabilitati", "", " ", "")

            Dim AutoUpdate As String = oMenu.GetSettingBusDitt(DittaCorrente, "Bsieibus", "Opzioni", ".", "AutoUpdate", "1", " ", "1")
            Dim URLiBUpdate As String = oMenu.GetSettingBusDitt(DittaCorrente, "Bsieibus", "Opzioni", ".", "URLiBUpdate", "http://lm.apexnet.it/iBUpdate", " ", "http://lm.apexnet.it/iBUpdate")

            ' Dati per il test. Chiedere a Matteo di attivare il proxy
            'ProxyUrl = "192.168.10.134"
            'ProxyUsername = "teo"
            'ProxyPassword = "gigi"
            'ProxyPort = "8080"

            'Passo i dati del proxy al BE
            oCleIbus.eProxyUrl = ProxyUrl
            oCleIbus.eProxyUsername = ProxyUsername
            oCleIbus.eProxyPassword = ProxyPassword
            oCleIbus.eProxyPort = ProxyPort


            If ProxyUrl <> "" Then
                ApexNetLIB.SetProxy.SettingProxy(ProxyUrl, CInt(ProxyPort), ProxyUsername, ProxyPassword)
            End If


            ' Esiste una nuova veriosne del connettore da scaricare ?
            If AutoUpdate = "1" Then
                IBUpgradeAvailable = IBCheckForNewVersion(URLiBUpdate)
                If IBUpgradeAvailable Then
                    WEDOLogger.WriteToRegistry(String.Format("E' stato rilevato un nuovo aggiornamento. Esistente:{0}, Disponibile: {1}", IBUpdate.vars_local_version, IBUpdate.vars_newVersion))
                End If
            Else
                WEDOLogger.WriteToRegistry(String.Format("L' aggiornamento automatico è stato disattivato. AutoUpdate: {0}", AutoUpdate.ToString()))
            End If



            ' Se esiste per prima cosa la scarico
            If IBUpgradeAvailable Then
                IBNewVersionDownloaded = IBUpdate.DownloadAndUnzip()
                WEDOLogger.WriteToRegistry(String.Format("Scarico e decomprimo la nuova versione in {0}.", IBUpdate.vars_unzipped_folder))
                ' ...inoltre se non sono in modalità batch avverto l'utente
                If Not oApp.Batch And IBNewVersionDownloaded Then
                    oApp.MsgBoxInfo(oApp.Tr(Me, 128744371685129090, "E' disponibile una nuova versione del connettore di IB" & vbCrLf & "Esci da Business per effettuare l'aggiornamento"))
                End If
            End If


            lblRelease.Text = FileVersionInfo.GetVersionInfo(oApp.NetDir & "\BNIEIBUS.dll").FileVersion


            'CLI;FOR;ART;LIS;SCO;DOC;MAG;CIT;PAG;COO;
            If CheckboxAttivi.Contains("CLI;") Then
                ckCli.Checked = True
            End If
            If CheckboxAttivi.Contains("FOR;") Then
                ckFor.Checked = True
            End If
            If CheckboxAttivi.Contains("ART;") Then
                ckArti.Checked = True
            End If
            If CheckboxAttivi.Contains("LIS;") Then
                ckListini.Checked = True
            End If
            If CheckboxAttivi.Contains("SCO;") Then
                ckSconti.Checked = True
            End If
            If CheckboxAttivi.Contains("DOC;") Then
                ckDoc.Checked = True
            End If
            If CheckboxAttivi.Contains("MAG;") Then
                ckMagaz.Checked = True
            End If
            If CheckboxAttivi.Contains("TBS;") Or CheckboxAttivi.Contains("CIT;") Then
                ckTabBase.Checked = True
            End If
            If CheckboxAttivi.Contains("PAG;") Then
                ckCodpaga.Checked = True
            End If
            If CheckboxAttivi.Contains("COO;") Then
                ckCoordinate.Checked = True
            End If
            If CheckboxAttivi.Contains("PUS;") Then
                ckNotifichePush.Checked = True
            End If
            If CheckboxAttivi.Contains("ORD;") Then
                ckOrdini.Checked = True
            End If
            If CheckboxAttivi.Contains("LEA;") Then
                ckLeads.Checked = True
            End If
            If CheckboxAttivi.Contains("OFF;") Then
                ckOff.Checked = True
            End If
            If CheckboxAttivi.Contains("CAT;") Then
                ckCatalogo.Checked = True
            End If



            'CLI;FOR;ART;LIS;SCO;DOC;MAG;CIT;PAG;COO;
            If CheckboxDisabilitati.Contains("CLI;") Then
                ckCli.Enabled = False
            End If
            If CheckboxDisabilitati.Contains("FOR;") Then
                ckFor.Enabled = False
            End If
            If CheckboxDisabilitati.Contains("ART;") Then
                ckArti.Enabled = False
            End If
            If CheckboxDisabilitati.Contains("LIS;") Then
                ckListini.Enabled = False
            End If
            If CheckboxDisabilitati.Contains("SCO;") Then
                ckSconti.Enabled = False
            End If
            If CheckboxDisabilitati.Contains("DOC;") Then
                ckDoc.Enabled = False
            End If
            If CheckboxDisabilitati.Contains("MAG;") Then
                ckMagaz.Enabled = False
            End If
            If CheckboxDisabilitati.Contains("TBS;") Or CheckboxDisabilitati.Contains("CIT;") Then
                ckTabBase.Enabled = False
            End If
            If CheckboxDisabilitati.Contains("PAG;") Then
                ckCodpaga.Enabled = False
            End If
            If CheckboxDisabilitati.Contains("COO;") Then
                ckCoordinate.Enabled = False
            End If
            If CheckboxDisabilitati.Contains("PUSH;") Then
                ckNotifichePush.Enabled = False
            End If
            If CheckboxDisabilitati.Contains("ORD;") Then
                ckOrdini.Enabled = False
            End If
            If CheckboxDisabilitati.Contains("LEA;") Then
                ckLeads.Enabled = True
            End If
            If CheckboxDisabilitati.Contains("OFF;") Then
                ckOff.Enabled = True
            End If
            If CheckboxDisabilitati.Contains("CAT;") Then
                ckCatalogo.Enabled = True
            End If


        Catch ex As Exception
            '-------------------------------------------------
            Dim strErr As String = CLN__STD.GestError(ex, Me, "", oApp.InfoError, oApp.ErrorLogFile, True)
            '-------------------------------------------------
        End Try
    End Sub
    Public Overridable Sub FRMIEIBUS_ActivatedFirst(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ActivatedFirst
        Dim strT() As String = Nothing
        Dim strT1() As String = Nothing
        Dim i As Integer = 0



        'oCleIbus.strReleaseTracciati = "1.8"
        'Const relTracciati As String = "1.8"


        'ocldibus.strdropboxdir = oCallBase.getsettingbusditt(DittaCorrente, "bsieibus", "opzioni", ".", "dropboxdir", "", " ", "")
        'ocldibus.strcontiesclusi = ocldibus.getsettingbusditt(DittaCorrente, "bsieibus", "opzioni", ".", "contiesclusi", "0", " ", "0").trim
        'ocldibus.strfiltroggstoart = ocldibus.getsettingbusditt(DittaCorrente, "bsieibus", "opzioni", ".", "filtroggstoart", "180", " ", "180").trim
        'ocldibus.strreleasetracciati = ocldibus.getsettingbusditt(DittaCorrente, "bsieibus", "opzioni", ".", "releasetracciati", relTracciati, " ", relTracciati).trim

        '    strReleaseTracciati = oCldIbus.GetSettingBusDitt(strDittaCorrente, "Bsieibus", "Opzioni", ".", "ReleaseTracciati", defRelracciati, " ", defRelracciati).Trim

        Try
            If Not LeggiDatiDitta() Then
                Me.Close()
                Return
            End If

            '-------------------------------------------------
            'se sono stato chiamato in modalità batch:
            'esempio di riga di comando:
            'mirto . PR_2012 business BNIEIBUS /B c:\bus2012\asc\bnieibus.bub
            If oApp.Batch And oApp.AvvioProgrammaParametri.Trim <> "" Then
                'Me.Visible = False
                Me.Top = -10000
                Me.Left = -10000

                'lancio l'elaborazione. i parametri di avvio verranno presi dal file BNVEPNFA.BUB, poi esco
                ' ADD CK
                ckCli.Checked = False
                ckFor.Checked = False
                ckArti.Checked = False
                ckListini.Checked = False
                ckCatalogo.Checked = False
                ckMagaz.Checked = False
                ckTabBase.Checked = False
                ckCodpaga.Checked = False
                ckOrdini.Checked = False
                ckSconti.Checked = False
                ckDoc.Checked = False
                ckLeads.Checked = False
                ckOff.Checked = False
                ckCoordinate.Checked = False
                ckNotifichePush.Checked = False

                Dim strTmp As String = oApp.AvvioProgrammaParametri.Trim
                'strTmp = "c:\bus2012\asc\bnieibus.bub"
                Dim r1 As New System.IO.StreamReader(strTmp)
                Do While Not r1.EndOfStream
                    strT = r1.ReadLine().Split("="c)
                    Select Case strT(0).ToLower
                        Case "tipork"
                            strT1 = strT(1).Split(";"c)
                            For i = 0 To strT1.Length - 1
                                ' ADD CK
                                If strT1(i) = "CLI" Then ckCli.Checked = True
                                If strT1(i) = "FOR" Then ckFor.Checked = True
                                If strT1(i) = "DOC" Then ckDoc.Checked = True
                                If strT1(i) = "ART" Then ckArti.Checked = True
                                If strT1(i) = "LIS" Then ckListini.Checked = True
                                If strT1(i) = "SCO" Then ckSconti.Checked = True
                                If strT1(i) = "CAT" Then ckCatalogo.Checked = True
                                If strT1(i) = "MAG" Then ckMagaz.Checked = True
                                If strT1(i) = "TBS" Or strT1(i) = "CIT" Then ckTabBase.Checked = True
                                If strT1(i) = "PAG" Then ckCodpaga.Checked = True
                                If strT1(i) = "ORD" Then ckOrdini.Checked = True
                                If strT1(i) = "LEA" Then ckLeads.Checked = True
                                If strT1(i) = "OFF" Then ckOff.Checked = True
                                If strT1(i) = "COO" Then ckCoordinate.Checked = True
                                If strT1(i) = "PUS" Then ckNotifichePush.Checked = True
                            Next
                        Case "ditta"
                            DittaCorrente = strT(1)
                            Me.Text = oMenu.SetCaptionDitt(DittaCorrente, Me.Text)
                    End Select
                Loop

                tlbElabora_ItemClick(tlbElabora, Nothing)
                'tlbElabora_ItemClick(tlbLog, Nothing)

                Me.Close()
                Return
            End If
        Catch ex As Exception
            '-------------------------------------------------
            Dim strErr As String = GestError(ex, Me, "", oApp.InfoError, oApp.ErrorLogFile, True)
            '-------------------------------------------------
        End Try
    End Sub
    Public Overridable Sub FRMIEIBUS_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        Dim ctrlTmp As Control = Nothing
        Dim strTipoImport As String = ""
        Dim strTipoExport As String = ""
        Try
            'creazione del file .BUB per la schedulazione
            If e.KeyCode = Keys.F5 And e.Control = True And e.Alt = True And e.Shift = False Then

                Me.ValidaLastControl()

                '-----------------------------------------------------------------------
                'Controlla se è stato selezionato qualcosa
                ' ADD CK
                If ckCli.Checked = False And _
                   ckFor.Checked = False And _
                   ckArti.Checked = False And _
                   ckListini.Checked = False And _
                   ckSconti.Checked = False And _
                   ckCatalogo.Checked = False And _
                   ckTabBase.Checked = False And _
                   ckCodpaga.Checked = False And _
                   ckMagaz.Checked = False And _
                   ckOrdini.Checked = False And _
                   ckLeads.Checked = False And _
                   ckOff.Checked = False And _
                   ckCoordinate.Checked = False And _
                   ckNotifichePush.Checked = False And _
                   ckDoc.Checked = False Then
                    oApp.MsgBoxErr(oApp.Tr(Me, 129877045983932301, "Selezionare almeno un tipo di dato da elaborare"))
                    Return
                End If

                Dim pathSchedulazioneiB As String = "C:\SchedulazioneiB"

                If Directory.Exists(pathSchedulazioneiB) Then
                    If oApp.MsgBoxInfoYesNo_DefNo(oApp.Tr(Me, 129877046001844341, "La cartella " & pathSchedulazioneiB & " esistà già. Vuoi ricreare le schedulazioni ? (Se rispondi SI le schedulazioni esistenti verrano eliminate)")) = Windows.Forms.DialogResult.No Then Return

                    Try
                        Directory.Delete(pathSchedulazioneiB, True)
                    Catch ex As Exception
                        oApp.MsgBoxInfo(oApp.Tr(Me, 129877046001844341, "ERRORE: Non sono riuscito a cancellare la cartella. Forse ci sei dentro! " & pathSchedulazioneiB))
                        Return
                    End Try
                End If

                Directory.CreateDirectory("C:\SchedulazioneiB")

                ' Recupero la Password di Admin
                Dim AdminPassword As String = ""
                oCleIbus.GetPasswordOperatore("Admin", AdminPassword)

                If String.IsNullOrEmpty(AdminPassword) Then
                    AdminPassword = "."
                End If

          
                ' Se ho selezionato gli ordini probabimente sto creando il file di import
                If ckOrdini.Checked = True Then
                    If ckOrdini.Checked Then strTipoImport += "ORD;"

                    If strTipoImport.Length > 0 Then strTipoImport = strTipoImport.Substring(0, strTipoImport.Length - 1)

                    ' Creo il file .bub
                    Dim fileImport As New System.IO.StreamWriter(pathSchedulazioneiB & "\" & "Import.bub", False)
                    fileImport.WriteLine("tipork=" & strTipoImport)
                    fileImport.WriteLine("ditta=" & DittaCorrente)
                    fileImport.Flush()
                    fileImport.Close()

                    ' Creo il bat di lancio del bub
                    Dim BatfileImport As New System.IO.StreamWriter(pathSchedulazioneiB & "\" & "Import.bat", False)
                    Dim BusCommandLineImport As String = String.Format("""{0}\busnet.exe"" {1} {2} {3} {4} BNIEIBUS /B {5}\Import.bub", oApp.NetDir, "Admin", AdminPassword, oApp.Db.Nome, oApp.Profilo, pathSchedulazioneiB)
                    BatfileImport.WriteLine(BusCommandLineImport)
                    BatfileImport.Flush()
                    BatfileImport.Close()



                    ' Creo la schedulazione
                    Using ts As New TaskService()
                        Dim tt As New TimeTrigger()
                        ' **** V1 and V2 properties ******************************************
                        ' Disable the trigger from firing the task
                        tt.Enabled = False

                        ' Default is true
                        ' Set the start time for today at 11:00 p.m.
                        tt.StartBoundary = DateTime.Now
                        ' Default is the time the trigger is instantiated
                        ' Set the last time the trigger will run to a year from now.
                        ' tt.EndBoundary = DateTime.Today + TimeSpan.FromDays(365)
                        ' Default is DateTime.MaxValue (or forever)
                        ' Set the time in between each repetition of the task after it starts to 30 minutes.
                        tt.Repetition.Interval = TimeSpan.FromMinutes(15)
                        ' Default is TimeSpan.Zero (or never)
                        ' Set the time the task will repeat to 1 day.
                        ' tt.Repetition.Duration = TimeSpan.FromDays(1)
                        ' Default is TimeSpan.Zero (or never)
                        ' Set the task to end even if running when the duration is over
                        tt.Repetition.StopAtDurationEnd = True
                        ' Default is false;
                        ' **** V2 only properties ********************************************
                        ' Set an identifier to be used when logging
                        tt.Id = "ImportIB"
                        ' Set the maximum time this task can run once triggered to one hour.
                        tt.ExecutionTimeLimit = TimeSpan.FromHours(1)
                        ' Default is TimeSpan.Zero (or never)
                        ' Trigger must then be added to the task'     

                        ' Create a new task definition and assign properties
                        Dim td As TaskDefinition = ts.NewTask()

                        td.RegistrationInfo.Description = "IB Import dati"

                        td.Triggers.Add(tt)
                        ' Create a trigger that will fire the task at this time every other da
                        'td.Triggers.Add(New DailyTrigger() With { _
                        '       .DaysInterval = 2 _
                        '})

                        ' Create an action that will launch Notepad whenever the trigger fires
                        td.Actions.Add(New ExecAction(pathSchedulazioneiB & "\Import.bat"))
                        td.Principal.RunLevel = TaskRunLevel.Highest
                        'td.Principal.LogonType = TaskLogonType.InteractiveToken


                        ' Register the task in the root folder
                        ts.RootFolder.RegisterTaskDefinition("IB Import dati", td)

                    End Using


                End If


                If ckCli.Checked Then strTipoExport += "CLI;"
                If ckFor.Checked Then strTipoExport += "FOR;"
                If ckArti.Checked Then strTipoExport += "ART;"
                If ckListini.Checked Then strTipoExport += "LIS;"
                If ckSconti.Checked Then strTipoExport += "SCO;"
                If ckDoc.Checked Then strTipoExport += "DOC;"
                If ckCatalogo.Checked Then strTipoExport += "CAT;"
                If ckMagaz.Checked Then strTipoExport += "MAG;"
                If ckTabBase.Checked Then strTipoExport += "TBS;"
                If ckCodpaga.Checked Then strTipoExport += "PAG;"
                If ckLeads.Checked Then strTipoExport += "LEA;"
                If ckOff.Checked Then strTipoExport += "OFF;"
                If ckCoordinate.Checked Then strTipoExport += "COO;"
                If ckNotifichePush.Checked Then strTipoExport += "COO;"

                If strTipoExport.Length > 0 Then strTipoExport = strTipoExport.Substring(0, strTipoExport.Length - 1)

                Dim fileExport As New System.IO.StreamWriter(pathSchedulazioneiB & "\" & "Export.bub", False)

                fileExport.WriteLine("tipork=" & strTipoExport)
                fileExport.WriteLine("ditta=" & DittaCorrente)
                fileExport.Flush()
                fileExport.Close()

                ' Creo il bat di lancio del bub
                Dim BatfileExport As New System.IO.StreamWriter(pathSchedulazioneiB & "\" & "Export.bat", False)
                BatfileExport.WriteLine("taskkill -F -IM ""Dropbox.exe""")
                Dim BusCommandLineExport As String = String.Format("""{0}\busnet.exe"" {1} {2} {3} {4} BNIEIBUS /B {5}\Export.bub", oApp.NetDir, "Admin", AdminPassword, oApp.Db.Nome, oApp.Profilo, pathSchedulazioneiB)
                BatfileExport.WriteLine(BusCommandLineExport)
                BatfileExport.WriteLine("set app=""%programfiles(x86)%\Dropbox\Client\Dropbox.exe""")
                BatfileExport.WriteLine("start ""Restart Dropbox"" %app% -B""")
                BatfileExport.Flush()
                BatfileExport.Close()

                ' Schedulazione export
                ' Creo la schedulazione
                Using ts As New TaskService()

                    Dim tt As New DailyTrigger()
                    tt.Enabled = False
                    tt.StartBoundary = DateTime.Today() + TimeSpan.FromHours(23)

                    tt.DaysInterval = 1

                    ' tt.Repetition.Interval = TimeSpan.FromDays(1)
                    tt.Repetition.StopAtDurationEnd = True
                    tt.Id = "ExportIB"
                    tt.ExecutionTimeLimit = TimeSpan.FromHours(1)

                    Dim td As TaskDefinition = ts.NewTask()
                    td.RegistrationInfo.Description = "IB Export dati"
                    td.Triggers.Add(tt)
                    td.Actions.Add(New ExecAction(pathSchedulazioneiB & "\Export.bat"))

                    td.Principal.RunLevel = TaskRunLevel.Highest

                    ' Register the task in the root folder
                    ts.RootFolder.RegisterTaskDefinition("IB Export dati", td)
                End Using

                oApp.MsgBoxInfo(oApp.Tr(Me, 128744371685129000, "Files creati in " & pathSchedulazioneiB & vbCrLf & "Ho creato anche una bozza di schedulazione (ma le ho disabilitate)" & vbCrLf & "Dagli una occhiata!"))
                e.Handled = True    'altrimenti anche il controllo riceve l'F5 e la routine ZOOM viene eseguita 2 volte!!!
            End If

        Catch ex As Exception
            '-------------------------------------------------
            Dim strErr As String = GestError(ex, Me, "", oApp.InfoError, oApp.ErrorLogFile, True)
            '-------------------------------------------------
        End Try
    End Sub
#End Region


#Region "Eventi Toolbar"
    Public Overridable Sub tlbElabora_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbElabora.ItemClick
        Dim strTipork1 As String = ""
        Try

            If Control.ModifierKeys = Keys.Shift Then
                If oApp.MsgBoxInfoYesNo_DefNo(oApp.Tr(Me, 129877042710458543, "Vuoi eseguire un test di invio ?")) = Windows.Forms.DialogResult.Yes Then

                    oCleIbus.InviaAlert(1, "Test invio alert tipo 1")
                    ' o SaveSettingBus, disponibile nei BN tramite l'oggetto oMenu o nei BE\BD presente nell'oggetto del BD (tipo oCldBoll).

                    ' I parametri della SaveSettingBus sono:
                    '- Cartella Livello 1
                    '- Cartella Livello 2
                    '- Cartella Livello 3
                    '- Nome Opzione Registro
                    '- Valore
                    '- Tipo Documento (se presente dipendenza per tipo documento)
                    '- Dipendenza Nome Operatore (3 caratteri, uno per ogni livello di cartella, valori N = no, S = Si, . = come da cartella)
                    '- Dipendenza Ditta (3 caratteri, uno per ogni livello di cartella, valori N = no, S = Si, . = come da cartella)
                    '- Dipendenza Tipo Documento (3 caratteri, uno per ogni livello di cartella, valori N = no, S = Si, . = come da cartella)
                    '
                    'Esempi:
                    'oCldGsre.SaveSettingBus("BSREGSRE", "RECENT", ".", "AnnoTCO", "2013", " ", "NS.", "...", "...")
                    'oMenu.SaveSettingBus("BNVEBOLL", "Opzioni", ".", "Prova", "123456", " ", "...", "...", "...")


                    'Dim URLiBUpdate As String = oMenu.(DittaCorrente, "Bsieibus", "Opzioni", ".", "URLiBUpdate", "http://lm.apexnet.it/iBUpdate", " ", "http://lm.apexnet.it/iBUpdate")
                    'oApp.MsgBoxInfo("Personalzzazione installata")
                End If

                Return
            End If

            If oApp.Batch = False Then
                If oApp.MsgBoxInfoYesNo_DefNo(oApp.Tr(Me, 129877042710458543, "Confermi l'elaborazione")) = Windows.Forms.DialogResult.No Then Return
            End If



            Me.Cursor = Cursors.WaitCursor

            ' ADD CK
            If ckCli.Checked Then strTipork1 += "CLI;"
            If ckFor.Checked Then strTipork1 += "FOR;"
            If ckDoc.Checked Then strTipork1 += "DOC;"
            If ckArti.Checked Then strTipork1 += "ART;"
            If ckListini.Checked Then strTipork1 += "LIS;"
            If ckSconti.Checked Then strTipork1 += "SCO;"
            If ckCatalogo.Checked Then strTipork1 += "CAT;"
            If ckMagaz.Checked Then strTipork1 += "MAG;"
            If ckTabBase.Checked Then strTipork1 += "TBS;"
            If ckCodpaga.Checked Then strTipork1 += "PAG;"
            If ckOrdini.Checked Then strTipork1 += "ORD;"
            If ckLeads.Checked Then strTipork1 += "LEA;"
            If ckOff.Checked Then strTipork1 += "OFF;"
            If ckCoordinate.Checked Then strTipork1 += "COO;"
            If ckNotifichePush.Checked Then strTipork1 += "PUS;"

            If strTipork1.Length > 0 Then strTipork1 = strTipork1.Substring(0, strTipork1.Length - 1)
            oCleIbus.strTipork = strTipork1
            oCleIbus.strDittaCorrente = DittaCorrente
            oCleIbus.Elabora()

            Me.Cursor = Cursors.Default
            oApp.MsgBoxInfo(oApp.Tr(Me, 129877048402908565, "Elaborazione terminata."))

            If (oCleIbus.LogError = True) And oApp.Batch = False Then
                If oApp.MsgBoxInfoYesNo_DefYes(oApp.Tr(Me, 129877048655397019, "Esistono dei messaggi nel file di log del programma. Visualizzare il file?")) = Windows.Forms.DialogResult.Yes Then
                    System.Diagnostics.Process.Start("notepad", oCleIbus.LogFileName)
                End If
            End If

            NtsProgressBar1.Position = 0
        Catch ex As Exception
            '-------------------------------------------------
            Dim strErr As String = CLN__STD.GestError(ex, Me, "", oApp.InfoError, oApp.ErrorLogFile, True)
            '-------------------------------------------------
        Finally
            lbStatus.Text = oApp.Tr(Me, 129877048548500824, "Pronto.")
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Public Overridable Sub tlbCambioDitta_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbCambioDitta.ItemClick
        Try
            If Not LeggiDatiDitta() Then Me.Close()

        Catch ex As Exception
            '-------------------------------------------------
            Dim strErr As String = CLN__STD.GestError(ex, Me, "", oApp.InfoError, oApp.ErrorLogFile, True)
            '-------------------------------------------------
        End Try
    End Sub

    Public Overridable Sub tlbEsci_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbEsci.ItemClick
        Me.Close()
    End Sub
#End Region

#Region "AutoUpdate"


    ''' <summary>
    ''' Verifica l'esistenza di una versione più aggiornata sul server.
    ''' </summary>
    ''' <returns>Ritorna true se esiste una nuova versione</returns>
    ''' <remarks></remarks>
    Public Function IBCheckForNewVersion(ByVal URLiBUpdate As String) As Boolean
        Try

            ' Leggo la versione del file BNIEIBUS 
            IBUpdate.vars_local_version = FileVersionInfo.GetVersionInfo(oApp.NetDir & "\BNIEIBUS.dll").FileVersion

            ' Imposto le variabili di istanza del downloader
            IBUpdate.vars_url_update = URLiBUpdate & "/iBUpdate.zip"
            IBUpdate.vars_url_version = URLiBUpdate & "/iBUpdate.txt"
            IBUpdate.vars_unzipdir = Path.GetTempPath()

            ' IBUpdate.vars_unzipdir = GetSettingReg("BUSINESS", UCase(oApp.Profilo) & "\BUSAGG", "BusAggDir", "")

            Return IBUpdate.NewVersionAvailable()
        Catch ex As Exception
            '-------------------------------------------------
            Dim strErr As String = CLN__STD.GestError(ex, Me, "", oApp.InfoError, oApp.ErrorLogFile, True)
            '-------------------------------------------------
        End Try

    End Function


    Public Function IBRunUpdate() As Boolean

        Try

            Dim IBUpdateFile As String = IBUpdate.vars_unzipdir & "\ibupdate.bat"

            ' Se esiste cancello il file .bat precedente (Situato as esmepio sotto c:\bus\agg)
            If File.Exists(IBUpdateFile) Then
                File.Delete(IBUpdateFile)
            End If

            ' Leggo il batch da eseguire dalla risorsa embedded dentro l'assembly 
            Dim ib_update_batch As String = ApexNetLIB.EmbeddedResource.GetString(GetType(FRMIEIBUS).Assembly, "ibupdate.bat", oApp.NetDir)

            ' Nel batch imposto la cartella sorgente e quella destinazione
            ib_update_batch = String.Format(ib_update_batch, IBUpdate.vars_unzipped_folder + "\iBUpdate", oApp.NetDir)

            ' Scrivo il file batch nella cartella Agg 
            Dim objWriter As New System.IO.StreamWriter(IBUpdateFile, False)
            objWriter.WriteLine(ib_update_batch)
            objWriter.Close()
            objWriter.Dispose()

            ' Esegui il batch
            Dim startInfo As New ProcessStartInfo(IBUpdateFile)
            startInfo.WindowStyle = ProcessWindowStyle.Hidden
            startInfo.CreateNoWindow = True
            Process.Start(startInfo)

            Return True

            Exit Function

        Catch ex As Exception

            Return False
        End Try

    End Function

    Public Function IncrementaAggNumber(ByVal dirFileAggNumber As String) As Boolean
        Try
            Dim currentVersion As String = ""

            If File.Exists(dirFileAggNumber) = False Then
                Return False
            Else
                'leggo la versione attuale dal file AggNumber.txt
                Using sr As StreamReader = File.OpenText(dirFileAggNumber)
                    Dim input As String
                    input = sr.ReadLine()
                    While Not input Is Nothing
                        currentVersion = currentVersion + input
                        input = sr.ReadLine()
                    End While
                    sr.Close()
                End Using

                Dim iCurrentVersion As Integer
                iCurrentVersion = CInt(currentVersion) + 1
                'scrivo la nuova versione nel file AggNumber.txt
                Using sw As StreamWriter = New StreamWriter(dirFileAggNumber)
                    sw.Write(iCurrentVersion)
                    sw.Close()
                End Using
            End If
        Catch
        End Try
    End Function

#End Region


    Private Sub ckOrdini_CheckedChanged(sender As Object, e As EventArgs) Handles ckOrdini.CheckedChanged

    End Sub

    Private Sub NtsCheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles ckCoordinate.CheckedChanged, ckCoordinate.CheckedChanged, ckCoordinate.CheckedChanged

    End Sub

    Private Sub ckNotifichePush_CheckedChanged(sender As Object, e As EventArgs) Handles ckNotifichePush.CheckedChanged, ckNotifichePush.CheckedChanged, ckNotifichePush.CheckedChanged

    End Sub

    Private Sub lblRelease_Click(sender As Object, e As EventArgs) Handles lblRelease.Click

    End Sub

    Private Sub NtsLabel2_Click(sender As Object, e As EventArgs) Handles lblCustomRelease.Click, lblCustomRelease.Click

    End Sub

    Private Sub tlbLog_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbLog.ItemClick

        System.Diagnostics.Process.Start("notepad", oCleIbus.LogFileName)

        Process.Start("explorer.exe", oCleIbus.strDropBoxDir)
        ' System.Diagnostics.Process.Start("notepad", oCleIbus.LogFileName)
        'If oApp.MsgBoxInfoYesNo_DefYes(oApp.Tr(Me, 129877048655397019, "Esistono dei messaggi nel file di log del programma. Visualizzare il file?")) = Windows.Forms.DialogResult.Yes Then
        ' System.Diagnostics.Process.Start("notepad", oCleIbus.LogFileName)
        'End If

#If False Then

        Dim frmLog As FRMLOGIBUS = Nothing

        frmLog = CType(NTSNewFormModal("FRMLOGIBUS"), FRMLOGIBUS)
        frmLog.Init(oMenu, oCallParams, DittaCorrente)
        frmLog.oCleIbus = oCleIbus

        frmLog.ShowDialog()
#End If

    End Sub

    Private Sub NtsCheckBox1_CheckedChanged_1(sender As Object, e As EventArgs) Handles ckTabBase.CheckedChanged

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub LogWrite(p1 As String, p2 As Boolean)
        Throw New NotImplementedException
    End Sub

End Class
