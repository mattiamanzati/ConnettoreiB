Partial Public Class FRMIEIBUS
    Inherits FRM__CHIL

    <System.Diagnostics.DebuggerNonUserCode()> _
    Public Sub New()
        MyBase.New()
    End Sub

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Public WithEvents NtsBarManager1 As NTSInformatica.NTSBarManager
    Public WithEvents tlbMain As NTSInformatica.NTSBar
    Public WithEvents tlbStampa As NTSInformatica.NTSBarButtonItem
    Public WithEvents tlbStampaVideo As NTSInformatica.NTSBarButtonItem
    Public WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
    Public WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
    Public WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
    Public WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
    Public WithEvents tlbGuida As NTSInformatica.NTSBarButtonItem
    Public WithEvents tlbEsci As NTSInformatica.NTSBarButtonItem
    Public WithEvents tlbStrumenti As NTSInformatica.NTSBarSubItem
    Public WithEvents tlbImpostaStampante As NTSInformatica.NTSBarButtonItem
    Public WithEvents tlbCambioDitta As NTSInformatica.NTSBarMenuItem
    Public WithEvents tlbCancella As NTSInformatica.NTSBarMenuItem
    Public WithEvents lbImport As NTSInformatica.NTSLabel
    Public WithEvents lbExport As NTSInformatica.NTSLabel
    Public WithEvents ckCodpaga As NTSInformatica.NTSCheckBox
    Public WithEvents ckCatalogo As NTSInformatica.NTSCheckBox
    Public WithEvents ckMagaz As NTSInformatica.NTSCheckBox
    Public WithEvents ckListini As NTSInformatica.NTSCheckBox
    Public WithEvents ckArti As NTSInformatica.NTSCheckBox
    Public WithEvents ckOrdini As NTSInformatica.NTSCheckBox
    Public WithEvents ckCli As NTSInformatica.NTSCheckBox
    Public WithEvents lbStatus As NTSInformatica.NTSLabel
    Public WithEvents tlbElabora As NTSInformatica.NTSBarButtonItem
    Public WithEvents tlbLog As NTSInformatica.NTSBarButtonItem
    Public WithEvents ckSconti As NTSInformatica.NTSCheckBox
    Public WithEvents ckFor As NTSInformatica.NTSCheckBox
    Public WithEvents NtsProgressBar1 As NTSInformatica.NTSProgressBar
    Public WithEvents ckDoc As NTSInformatica.NTSCheckBox
    Public WithEvents lblRelease As NTSInformatica.NTSLabel
    Public WithEvents ckLeads As NTSInformatica.NTSCheckBox
    Public WithEvents ckOff As NTSInformatica.NTSCheckBox
    Public WithEvents ckCoordinate As NTSInformatica.NTSCheckBox
    Public WithEvents ckNotifichePush As NTSInformatica.NTSCheckBox
    Public WithEvents NtsLabel1 As NTSInformatica.NTSLabel
    Public WithEvents lblCustomRelease As NTSInformatica.NTSLabel
    Public WithEvents ckTabBase As NTSInformatica.NTSCheckBox


End Class
