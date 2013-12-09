<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class iBUpdate
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Text = "iBUpdate"
    End Sub

    Private Sub iBUpdate_Deactivate(sender As Object, e As System.EventArgs) Handles Me.Deactivate

    End Sub

    Private Sub iBUpdate_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

    End Sub

    Private Sub iBUpdate_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        'appena carico la form avvio la procedura di aggiornamento
        Me.AvviaUpdate()
    End Sub
End Class
