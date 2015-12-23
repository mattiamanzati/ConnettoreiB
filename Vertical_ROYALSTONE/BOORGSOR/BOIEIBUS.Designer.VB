Partial Public Class FROIEIBUS

  <System.Diagnostics.DebuggerNonUserCode()> _
  Public Sub New()
    MyBase.New()
  End Sub

  'Form overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
    MyBase.Dispose(disposing)
  End Sub

  Private Sub FROIEIBUS_Load(sender As Object, e As EventArgs) Handles Me.Load


        lblCustomRelease.Text = "ROYALSTONE" + " - " + FileVersionInfo.GetVersionInfo(oApp.NetDir & "\BOIEIBUS.dll").FileVersion

  End Sub
End Class
