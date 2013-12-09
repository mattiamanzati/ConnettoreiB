Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Tcp
Imports System.IO
Imports System.Configuration
Imports System.Reflection
Imports System.Threading.Thread
Imports System.Globalization
Imports Microsoft.VisualBasic
Imports Microsoft.Win32
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Security.Cryptography

Public Class FRMBUSNET

  '************** RICORDARSI DI COMPILARE SEMPRE CON L'OPZIONE CPU x86, ALTRIMENTI NON GIRA LA CREAZIONE DEGLI ACTIVEX DI BUSINESS SU 64 BIT!!! ***********  compila->opzioni di compilazione avanzate->cpu di destinazione=x86

  'quando si cambia l'icona di business deve essere fatto anche in bn__Chil e bn__std come icona collegata alla form!!!!

  'per mutex
  'Costanti nome dei MUTEX
  Public Const bsMTXBus = "NtsBusinessMutex" 'BUSINESS, BUSBATCH
  Public Const bsMTXUtl = "NtsBusUtilMutex" 'BUSCONV/BUSCONVS/BUSUPSIZ/BUSCPERS/BUSCPER/BUSSETD
  Public Const bsMTXSet = "NtsBusSetupMutex" 'BUSSETUP
  Public Const bsMTXAgg = "NtsBusAggMutex" 'BUSAGGN
  Public Const bsMTXSrv = "NtsBusServMutex" 'BUSSERV

  'Variabili pubbliche con gli handle dei mutex creati
  Public glNtsMutexGlobal As Integer
  Public glNtsMutexLocal As Integer
  Public bBatch As Boolean = False
  Public DirP As String = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)

  'Per controllo dell'applicazione in esecuzione attraverso Mutex globali (semaforo del s.o.)
  Private Declare Function CreateMutex Lib "kernel32" Alias "CreateMutexA" (ByVal lpMutexAttributes As Integer, ByVal bInitialOwner As Integer, ByVal lpName As String) As Integer
  Private Declare Function OpenMutex Lib "kernel32" Alias "OpenMutexA" (ByVal dwDesiredAccess As Integer, ByVal bInheritHandle As Integer, ByVal lpName As String) As Integer
  Private Declare Function ReleaseMutex Lib "kernel32" (ByVal hMutex As Integer) As Integer
  Private Declare Function CloseHandle Lib "kernel32" (ByVal hObject As Integer) As Integer
  Private Declare Function SetSecurityDescriptorDacl Lib "advapi32.dll" (ByVal pSecurityDescriptor As IntPtr, ByVal bDaclPresent As Integer, ByVal pDacl As Byte, ByVal bDaclDefaulted As Integer) As Integer
  Private Declare Function InitializeSecurityDescriptor Lib "advapi32.dll" (ByVal pSecurityDescriptor As IntPtr, ByVal dwRevision As Integer) As Integer

  Structure SECURITY_ATTRIBUTES
    Dim nLength As Integer
    Dim lpSecurityDescriptor As Integer
    Dim bInheritHandle As Integer
  End Structure
  Private Structure ACL
    Dim AclRevision As Byte
    Dim Sbz1 As Byte
    Dim AclSize As Integer
    Dim AceCount As Integer
    Dim Sbz2 As Integer
  End Structure
  Private Structure SECURITY_DESCRIPTOR
    Dim Revision As Byte
    Dim Sbz1 As Byte
    Dim Control As Integer
    Dim Owner As Integer
    Dim Group As Integer
    Dim sACL As ACL
    Dim Dacl As ACL
  End Structure
  Private Const SECURITY_DESCRIPTOR_REVISION = 1
  Private Const ERROR_ALREADY_EXISTS = 183&
  Private Const STANDARD_RIGHTS_REQUIRED = &HF0000
  Private Const SYNCHRONIZE = &H100000
  Private Const MUTANT_QUERY_STATE = &H1
  Private Const MUTEX_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED Or SYNCHRONIZE Or MUTANT_QUERY_STATE

  Const LOGON_WITH_PROFILE As Integer = &H1
  Const CREATE_DEFAULT_ERROR_MODE As Integer = &H4000000

  Public Structure PROCESS_INFO
    Public hProcess As IntPtr
    Public hThread As IntPtr
    Public dwProcessId As Integer
    Public dwThreadId As Integer
  End Structure
  Public Structure STARTUP_INFO
    Public cb As Integer
    Public lpReserved As Integer
    <MarshalAs(UnmanagedType.LPTStr)> Public lpDesktop As String
    <MarshalAs(UnmanagedType.LPTStr)> Public lpTitle As String
    Public dwX As Long
    Public dwY As Integer
    Public dwXSize As Integer
    Public dwYSize As Integer
    Public dwXCountChars As Integer
    Public dwYCountChars As Integer
    Public dwFillAttribute As Integer
    Public dwFlags As Integer
    Public wShowWindow As Short
    Public cbReserved2 As Short
    Public lpReserved2 As Integer
    Public hStdInput As Integer
    Public hStdOutput As Integer
    Public hStdError As Integer
  End Structure
  <DllImport("advapi32.dll")> _
  Public Shared Function CreateProcessWithLogonW(<MarshalAs(UnmanagedType.LPWStr)> ByVal lpUsername As String, _
                                          <MarshalAs(UnmanagedType.LPWStr)> ByVal lpDomain As String, _
                                          <MarshalAs(UnmanagedType.LPWStr)> ByVal lpPassword As String, _
                                          ByVal dwLogonFlags As Integer, _
                                          <MarshalAs(UnmanagedType.LPWStr)> ByVal lpApplicationName As String, _
                                          <MarshalAs(UnmanagedType.LPWStr)> ByVal lpCommandLine As String, _
                                          ByVal lpCreationFlags As Integer, _
                                          ByVal lpVoid As Integer, _
                                          <MarshalAs(UnmanagedType.LPWStr)> ByVal lpCurrentDirectory As String, _
                                          ByRef lpStartupInfo As STARTUP_INFO, _
                                          ByRef lpProcessInfo As PROCESS_INFO) As Integer
  End Function
  Private Declare Function GetLastError Lib "kernel32" () As Long

  Private Sub fmBusnet_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    'Dim strTmp As String = ""
    Dim strParam As String = ""
    Dim strDir As String
    Dim i As Integer
    Dim bDisableMutex As Boolean = False
    Dim bExists As Boolean = False
    Dim strProfilo As String = ""

    Me.Height = 0
    Me.Width = 0

    'strDir = Directory.GetCurrentDirectory
    strDir = Application.StartupPath

    If My.Application.CommandLineArgs.Count > 0 Then
      For i = 0 To My.Application.CommandLineArgs.Count - 1
        strParam = strParam & My.Application.CommandLineArgs(i).ToString.Replace(" ", "*§§§*§§§*") & " "
      Next
    End If
    If strParam = "" Then
      'a volte, in debug, non legge la riga di comando ...
      strParam = ". . . Business"
    Else
      strParam = Trim(strParam)
    End If

    'imposto la dir in bn__std
    strProfilo = CheckBusProfilo(Command, False)
    DirP = GetSettingReg("Business", strProfilo, "Dir", "")
    If Not System.IO.Directory.Exists(DirP) Then
      DirP = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
    End If

    If strParam.ToUpper.IndexOf(" /B ") > -1 Then
      bBatch = True
      WriteMsgBoxToLog("Busness NET Avviato con la seguente riga di comando: " & strParam, False)
    End If

    '*************************************************************
    'PER PRIMA COSA IMPOSTO LA DATA NEL FORMAT OCON ANNO A 4 CIFRE, DIVERSAMENTE SE INSERISCO UNA DATA NEL 
    'FORMATO 31/12/2099 VIENE TRADOTTA IN 31/12/1999 QUANDO SI FA .toshortdatestring!!!!!
    Try
      Dim strDateFormat As String = CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern
      If strDateFormat.IndexOf("yyyy") = -1 Then
        strDateFormat = strDateFormat.Replace("yy", "yyyy")
        Dim oCulture As CultureInfo = New CultureInfo(CurrentThread.CurrentCulture.Name)
        oCulture.DateTimeFormat.ShortDatePattern = strDateFormat
        CurrentThread.CurrentCulture = oCulture
        'strTmp = CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern
      End If
    Catch ex As Exception
      If bBatch = False Then
        MsgBox("Error starting Bus.net during set DATETIME format: " & ex.ToString)
      Else
        WriteMsgBoxToLog("Error starting Bus.net during set DATETIME format: " & ex.ToString, True)
      End If
      Me.Close()
      Return
    End Try

    '*************************************************************
    'preparo il canale per la comunicazione dei REMOTING:
    'dentro ai vari entity ci sarà una riga del tipo quella commentata per avvisare che il dal non è in locale ma è in remoto
    ChannelServices.RegisterChannel(New TcpClientChannel(), False)
    'RemotingConfiguration.RegisterActivatedClientType(GetType(CLDORGSOR), "tcp://Mirto:1973")

    'Controlla se sono disabilitati i Mutex
    glNtsMutexGlobal = 0
    glNtsMutexLocal = 0
    bDisableMutex = CBool(Val(GetSettingReg("Business", "Debug", "DisableMutex", "0")))
    If Not bDisableMutex Then
      'Controlla che nonsiano in esecuzione programmi di utilità (BUSCONV/BUSCONVS/BUSUPSIZ/BUSCPERS/BUSCPER/BUSSETD)
      If NtsMutexTrova(bsMTXUtl) Then
        If bBatch = False Then
          MsgBox("Uno o più componenti di Business, sotto indicati, sono attualmente in esecuzione:" & vbCrLf & vbCrLf & "BUSCONV/S.EXE" & vbCrLf & "BUSUPSIZ.EXE" & vbCrLf & "BUSCPER/S.EXE" & vbCrLf & "BUSSETD.EXE" & vbCrLf & vbCrLf & "Chiudere tutte le istanze dei programmi prima di eseguire l'applicazione.", vbExclamation, "Messaggio di Business Net")
        Else
          WriteMsgBoxToLog("Uno o più componenti di Business, sotto indicati, sono attualmente in esecuzione:" & vbCrLf & vbCrLf & "BUSCONV/S.EXE" & vbCrLf & "BUSUPSIZ.EXE" & vbCrLf & "BUSCPER/S.EXE" & vbCrLf & "BUSSETD.EXE" & vbCrLf & vbCrLf & "Chiudere tutte le istanze dei programmi prima di eseguire l'applicazione.", True)
        End If
        Me.Close()
        Return
      End If
      'Controlla che non ci siano in esecuzione BUSAGGN
      If NtsMutexTrova(bsMTXAgg) Then
        If bBatch = False Then
          MsgBox("Uno o più componenti di Business, sotto indicati, sono attualmente in esecuzione:" & vbCrLf & vbCrLf & "BUSAGGN.EXE (Installazione aggiornamenti)" & vbCrLf & vbCrLf & "Chiudere tutte le istanze dei programmi prima di eseguire l'applicazione.", vbExclamation, "Messaggio di Business Net ")
        Else
          WriteMsgBoxToLog("Uno o più componenti di Business, sotto indicati, sono attualmente in esecuzione:" & vbCrLf & vbCrLf & "BUSAGGN.EXE (Installazione aggiornamenti)" & vbCrLf & vbCrLf & "Chiudere tutte le istanze dei programmi prima di eseguire l'applicazione.", True)
        End If
        Me.Close()
        Return
      End If
      'Controlla che non ci siano in esecuzione BUSSETUP
      If NtsMutexTrova(bsMTXSet) Then
        If bBatch = False Then
          MsgBox("Uno o più componenti di Business, sotto indicati, sono attualmente in esecuzione:" & vbCrLf & vbCrLf & "BUSSETUP.EXE (Installazione di Business) " & vbCrLf & vbCrLf & "Chiudere tutte le istanze dei programmi prima di eseguire l'applicazione.", vbExclamation, "Messaggio di Business Net")
        Else
          WriteMsgBoxToLog("Uno o più componenti di Business, sotto indicati, sono attualmente in esecuzione:" & vbCrLf & vbCrLf & "BUSSETUP.EXE (Installazione di Business) " & vbCrLf & vbCrLf & "Chiudere tutte le istanze dei programmi prima di eseguire l'applicazione.", True)
        End If
        Me.Close()
        Return
      End If
    End If
    'esco se l'aggiornamento non deve partire in automatico all'avvio di Business
    If bBatch = False Then
      If GetSettingReg("BUSINESS", UCase(strProfilo) & "\BUSAGG", "BusaggAutoUpdate", "N") <> "N" Then
        'lancio la routine che verifica se ci sono aggiornamenti da caricare prima di avviare Busiess
        If Not bDisableMutex Then
          'L'installazione aggiornamenti la fa partire solo se non è già in esecuzione Business
          If Not NtsMutexTrova(bsMTXBus) Then
            If Not CaricaAgg() Then
              Me.Close()
              Return
            End If
          End If
        Else
          If Not CaricaAgg() Then
            Me.Close()
            Return
          End If
        End If
      End If    'If strTmp <> "N" Then
    End If    'If bBatch = False Then

    If Not bDisableMutex Then
      'Crea il mutex per il controllo se l'applicazione è in esecuzione
      'DOPO AVER INSTALLATO GLI AGGIORNAMENTI
      NtsMutexCrea(bsMTXBus, bExists)
    End If

    '*************************************************************
    'carico l'oggetto MENU e lo avvio passandogli la riga di comando
    Try
      Dim assem As System.Reflection.Assembly
      Dim tpChild As Type
      Dim fmMenu As Object

      assem = System.Reflection.Assembly.LoadFrom(strDir & "\BN__MENU.DLL")
      tpChild = assem.GetType("NTSInformatica.FRMMENU", True, True)
      fmMenu = Activator.CreateInstance(tpChild)

      If Not fmMenu.InitMenu(strParam, strDir, Me, Nothing, DirP) Then
        Me.Close()
        Return
      End If

      'visualizzo il menu (non modale)
      fmMenu.show()
    Catch ex As Exception
      If bBatch = False Then
        MsgBox("Error starting Bus.net: " & ex.ToString, MsgBoxStyle.Critical, "Messaggio di Business NET")
      Else
        WriteMsgBoxToLog("Error starting Bus.net: " & ex.ToString, True)
      End If
      Me.Close()
      Return
    End Try
  End Sub

  Private Sub fmBusnet_Unload(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
    'Elimina il mutex che eventualmente ha creato all'avvio
    Try
      NtsMutexElimina()
    Catch ex As Exception

    End Try
  End Sub

  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMBUSNET))
    Me.SuspendLayout()
    '
    'FRMBUSNET
    '
    Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
    Me.ClientSize = New System.Drawing.Size(116, 32)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Location = New System.Drawing.Point(-1000, 0)
    Me.Name = "FRMBUSNET"
    Me.ShowInTaskbar = False
    Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
    Me.Text = "Business NET"
    Me.WindowState = System.Windows.Forms.FormWindowState.Minimized
    Me.ResumeLayout(False)

  End Sub


  Public Overridable Function CaricaAgg() As Boolean
    'carico gli aggiornamenti all'avvio di busnet
    'Dim objAgg As Object
    Dim strTmp As String
    Dim strBusDir As String
    Dim strBusAggdir As String
    Dim strBusTmpdir As String
    Dim strProfilo As String
    Dim strCurrentVer As String
    Dim bUserXXX As Boolean = False
    Dim strAggUser As String = ""
    Dim strAggPwd As String = ""
    Dim strDomain As String = ""
    Dim strUser As String = ""
    Dim nAggNumberClient As Integer = 0
    Dim nAggNumberServer As Integer = 0
    Dim i As Integer = 0
    'Dim s As New Security.SecureString

    Try
      strProfilo = CheckBusProfilo(Command, bUserXXX)

      If UCase(strProfilo) = "BUSOFFLINE" Then Return True

      'leggo le dir dove dovrebbero esserci gli aggiornamenti
      strBusTmpdir = GetSettingReg("BUSINESS", UCase(strProfilo) & "\BUSAGG", "BusAggTmp", "C:\Bustmp")
      If strBusTmpdir.Length > 0 Then
        If strBusTmpdir.Substring(strBusTmpdir.Length - 1) <> "\" Then strBusTmpdir += "\"
      End If

      If UCase(strProfilo) = "BUSOFFLINE" Then
        strBusAggdir = GetSettingReg("BUSINESS", UCase(strProfilo) & "\BUSAGG", "BusAggDirOffline", "")
      Else
        strBusAggdir = GetSettingReg("BUSINESS", UCase(strProfilo) & "\BUSAGG", "BusAggDir", "")
      End If
      If strBusAggdir.Length > 0 Then
        If strBusAggdir.Substring(strBusAggdir.Length - 1) <> "\" Then strBusAggdir += "\"
      End If

      'versione di Business locale
      If UCase(strProfilo) = "BUSOFFLINE" Then
        nAggNumberServer = Val(GetSettingReg("BUSINESS", UCase(strProfilo) & "\BUSAGG", "AggNumberOffline", "0"))
      Else
        nAggNumberServer = Val(GetSettingReg("BUSINESS", UCase(strProfilo) & "\BUSAGG", "AggNumber", "0"))
      End If

      'versione di Business sul server
      'Controlla se il Server è raggiungibile
      'e se esiste il file .txt dove è salvato il numero di aggiornamento nel Server
      nAggNumberClient = nAggNumberServer
      If System.IO.File.Exists(strBusAggdir & "Aggnumber.txt") = False Then
        MsgBox("Non esiste il file '" & strBusAggdir & "Aggnumber.txt'," & vbCrLf & "o il Server non è raggiungibile.", vbExclamation)
        Return True
      Else
        Dim r1 As New System.IO.StreamReader(strBusAggdir & "Aggnumber.txt")
        If Not r1.EndOfStream Then
          nAggNumberClient = Val(r1.ReadLine)
        End If
        r1.Close()
      End If

      'esco se le versioni sono uguali
      If nAggNumberClient = nAggNumberServer Then Return True

      '-----------------------------------------
      'devo eseguire l'aggiornamento: se posso carico la versione aggiornata di busaggn.exe
      strAggUser = GetSettingReg("BUSINESS", UCase(strProfilo) & "\BUSAGG", "BusAggUser", "")
      strAggPwd = GetSettingReg("BUSINESS", UCase(strProfilo) & "\BUSAGG", "BusAggPwd", "")

      strBusDir = GetSettingReg("BUSINESS", strProfilo, "CmbDir", "")
      If strBusDir.Length > 0 Then
        If strBusDir.Substring(strBusDir.Length - 1) <> "\" Then strBusDir += "\"
      End If
      strTmp = strBusDir & "Busaggn.exe"

      If System.IO.File.Exists(strTmp) Then
        strCurrentVer = System.Diagnostics.FileVersionInfo.GetVersionInfo(strTmp).FileVersion
        Formatta(strCurrentVer)
      Else
        strCurrentVer = "0000.00.0000"
      End If

      'verifico se sul server c'è una versione più aggiornata di Busaggn.exe: se c'è la scarico e la registro
      GetNewAggc(strBusDir, strBusTmpdir, strCurrentVer)

      GetNewAggc(strBusDir, strBusAggdir, strCurrentVer)

      'se la versione corrente è '0000.00.0000' do messaggio ed esco
      If strCurrentVer = "0000.00.0000" Then
        MsgBox("Attenzione: non è possibile avviare la procedura di aggiornamento in quanto sul pc non è stato trovato il file BUSAGGN.EXE", vbExclamation)
        Return True
      End If

      '----------------------------------
      'lancio Busaggn.exe e chiudo Busnet.exe
      MsgBox("Attenzione: sul server è presente un aggiornamento di Business. Business NET verrà chiuso e verrà avviata la procedura di aggiornamento", MsgBoxStyle.Information)

      'vecchio sistema: quello precedente dava errrore quando da xp utente di tipo user devo sfogliare la rete per accedere ad aggnumber.txt, quando in realtà tutto l'ambiente è impostato correttamente
      'Dim a As New ProcessStartInfo()
      'Dim p As New Process()
      'Try
      '  a.FileName = strBusDir & "Busaggn.exe"
      '  a.Arguments = strProfilo & " /B"
      '  a.WorkingDirectory = strBusDir
      '  a.LoadUserProfile = True
      '  a.UseShellExecute = False
      '  a.RedirectStandardOutput = True
      '  If strAggUser.Trim <> "" Then
      '    i = strAggUser.IndexOf("\")
      '    If i < 0 Then
      '      strDomain = ""
      '      strUser = strAggUser
      '    Else
      '      strDomain = strAggUser.Substring(0, i)
      '      strUser = strAggUser.Substring(i + 1)
      '    End If
      '    a.Domain = strDomain
      '    a.UserName = strUser

      '    PwdDeCript(strAggPwd, strAggPwd)

      '    For i = 0 To strAggPwd.Length - 1
      '      s.AppendChar(strAggPwd(i))
      '    Next
      '    a.Password = s.Copy

      '    If System.Environment.OSVersion.Version.Major >= 6 Then
      '      p.StartInfo = a      'in XP così da errore !!!! handle non valido
      '      p.Start()
      '    Else
      '      'specifico per windows XP
      '      Process.Start(strBusDir & "Busaggn.exe", strProfilo & " /B", strUser, s.Copy, strDomain)
      '    End If
      '  Else
      '    If System.Environment.OSVersion.Version.Major >= 6 Then
      '      p.StartInfo = a      'in XP così da errore !!!! handle non valido
      '      p.Start()
      '    Else
      '      'specifico per windows XP: non posso fare il test per vista, visto che se ho lo uacattivo non sono mai un admin
      '      'se l'utente passatomi non è una admin do un messaggio e non faccio partire l'aggiornamento
      '      Dim user As New System.Security.Principal.WindowsPrincipal(System.Security.Principal.WindowsIdentity.GetCurrent())
      '      If user.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator) = False Then
      '        MsgBox("Attenzione: dovrebbero essere caricati degli aggiornamenti di Business NET" & vbCrLf & _
      '               "ma l'utente di windows corrente non è un amministratore" & vbCrLf & _
      '               "e nelle proprietà degli aggiornamenti non è stato indicato" & vbCrLf & _
      '               "un utente di tipo administrator. Gli aggiornamenti non verranno caricati", MsgBoxStyle.Critical)
      '        Return True
      '      Else
      '        Process.Start(strBusDir & "Busaggn.exe", strProfilo & " /B")
      '      End If
      '    End If
      '  End If    'If strAggUser.Trim <> "" Then

      '  Me.Close()      'chiudo: l'utente dovrà riavviare bus a mano dopo la fine degli aggiornamenti
      '  Return False    'in questo modo busnet viene chiuso...
      'Catch ex As Exception
      '  MsgBox("Errore in fase di avvio '" & strBusDir & "Busaggn.exe" & "' per installazione aggiornamenti." & vbCrLf & "Gli aggiornamenti non verranno caricati." & vbCrLf & vbCrLf & vbCrLf & "Errore: " & vbCrLf & ex.ToString)
      '  Return True
      'End Try

      'nuovo sistema
      Try
        Dim pSecurityAttributes As SECURITY_ATTRIBUTES
        pSecurityAttributes.bInheritHandle = True
        pSecurityAttributes.nLength = Marshal.SizeOf(pSecurityAttributes)
        pSecurityAttributes.lpSecurityDescriptor = IntPtr.Zero
        Dim pStartInfo As STARTUP_INFO = Nothing
        pStartInfo.cb = Len(pStartInfo)
        Dim pProcessInfo As PROCESS_INFO

        If strAggUser.Trim <> "" Then
          i = strAggUser.IndexOf("\")
          If i < 0 Then
            strDomain = ""
            strUser = strAggUser
          Else
            strDomain = strAggUser.Substring(0, i)
            strUser = strAggUser.Substring(i + 1)
          End If
          
          PwdDeCript(strAggPwd, strAggPwd)


          'avvio gli aggiornamenti con credenziali specificate
          Dim iReturn As Integer = CreateProcessWithLogonW(strUser, strDomain, strAggPwd, _
                                          LOGON_WITH_PROFILE, Nothing, _
                                          strBusDir & "Busaggn.exe " & strProfilo & " /B", _
                                          CREATE_DEFAULT_ERROR_MODE, Nothing, _
                                          System.Environment.CurrentDirectory, pStartInfo, pProcessInfo)
          If iReturn = 0 Then
            Select Case GetLastError()
              Case 1326
                MsgBox("Errore in fase di avvio di avvio aggiornamenti:" & vbCrLf & "dominio, utente o password errati o non impostati (o utente non di tipo administrator). Errore: " & vbCrLf & GetLastError().ToString, vbExclamation, "Messaggio di Busaggn")

              Case 2
                MsgBox("Errore in fase di avvio di avvio aggiornamenti:" & vbCrLf & " programma " & strBusDir & "Busaggn.exe' non trovato. Errore: " & vbCrLf & GetLastError().ToString, vbExclamation, "Messaggio di Busaggn")

              Case Else
                MsgBox("Errore in fase di avvio di avvio aggiornamenti" & vbCrLf & ": Errore: " & vbCrLf & GetLastError().ToString, vbExclamation, "Messaggio di Busaggn")
            End Select
            Return True 'proseguo senza avviare gli aggiornamenti
          End If
        Else
          'non ho indicato credenziali: se l'utente non è un administrator verrà generato un errore non gestito
          Process.Start(strBusDir & "Busaggn.exe", strProfilo & " /B")
        End If    'If strAggUser.Trim <> "" Then

        Me.Close()      'chiudo: l'utente dovrà riavviare bus a mano dopo la fine degli aggiornamenti
        Return False    'in questo modo busnet viene chiuso...

      Catch ex As Exception
        MsgBox("Errore in fase di avvio '" & strBusDir & "Busaggn.exe" & "' per installazione aggiornamenti." & vbCrLf & "Gli aggiornamenti non verranno caricati." & vbCrLf & vbCrLf & vbCrLf & "Errore: " & vbCrLf & ex.ToString)
        Return True
      End Try

    Catch ex As Exception
      MsgBox("Error starting Business NET - CaricaAgg: " & ex.ToString)
      Me.Close()
    End Try
  End Function

  Public Function CheckBusProfilo(ByVal strComando As String, ByRef bUserXXX As Boolean) As String
    'scompone la riga di comando in : opnome passwd codazi profilo
    Dim nPos As Integer
    Dim nPos1 As Integer
    Dim nPos2 As Integer
    Dim nPos3 As Integer
    Dim nPos4 As Integer
    Dim strC As String

    CheckBusProfilo = "Business"

    If Len(Trim(strComando)) = 0 Then Exit Function

    'Legge la linea di comando
    strC = strComando
    nPos = InStr(1, strC, " ")
    If nPos <> 0 Then
      nPos1 = InStr(nPos + 1, strC, " ")
      If Trim(Microsoft.VisualBasic.Left(strC, nPos - 1)).ToUpper = "XXX" Then bUserXXX = True
      'gstrOpnome = Trim(Left$(strC, nPos - 1))
    End If
    If nPos1 <> 0 Then
      nPos2 = InStr(nPos1 + 1, strC, " ")
      'gstrPasswd = Mid$(strC, nPos + 1, nPos1 - nPos - 1)
    End If
    If nPos2 <> 0 Then
      nPos3 = InStr(nPos2 + 1, strC, " ")
      'gstrCodazi = Mid$(strC, nPos1 + 1, nPos2 - nPos1 - 1)
    Else
      'gstrCodazi = Mid$(strC, nPos1 + 1)
    End If
    'Aggiunto : profilo
    If nPos3 <> 0 Then
      nPos4 = InStr(nPos3 + 1, strC, " ")
      CheckBusProfilo = Mid$(strC, nPos2 + 1, nPos3 - nPos2 - 1)
    Else
      If nPos2 <> 0 Then
        CheckBusProfilo = Mid$(strC, nPos2 + 1)
      End If
    End If
    'Aggiunto: programma
    'Aggiunto: restricted /R puo' esserci solo se c'e' programma,
    'Aggiunto: parametro/i con cui chiamare il programma, può esserci solo se c'è programma
    'NOTA:     se c'è restricted il/i parametro/i devono essere scritti dopo
    'Esempio: Roby . Prova business bs--paga /R Apri;3
    '  If nPos4 <> 0 Then
    '    gstrProgramma = Mid$(strC, nPos3 + 1, nPos4 - nPos3 - 1)
    '    nPos5 = InStr(nPos4 + 1, strC, " ")
    '    If nPos5 <> 0 Then
    '      gstrRestr = Mid$(strC, nPos4 + 1, nPos5 - nPos4 - 1)
    '      gstrParam = Mid$(strC, nPos5 + 1)
    '    Else
    '      strAppo = Mid$(strC, nPos4 + 1)
    '      If strAppo = "/R" Then
    '        gstrRestr = Mid$(strC, nPos4 + 1)
    '      Else
    '        gstrParam = Mid$(strC, nPos4 + 1)
    '      End If
    '    End If
    '  Else
    '    If nPos3 <> 0 Then
    '     gstrProgramma = Mid$(strC, nPos3 + 1)
    '    End If
    '  End If
    'Ogni campo con . viene considerato poi stringa vuota ...
    '  If gstrPasswd = "." Then gstrPasswd = ""
    '  If gstrOpnome = "." Then gstrOpnome = ""
    '  If gstrCodazi = "." Then gstrCodazi = ""
    If CheckBusProfilo = "." Or CheckBusProfilo = "" Then CheckBusProfilo = "Business"
    '  If gstrProgramma = "." Then gstrProgramma = ""
    '  If gstrRestr = "." Then gstrRestr = ""
    '  If gstrParam = "." Then gstrParam = ""
  End Function

  Private Function GetNewAggc(ByVal strBusDir As String, ByVal strDir As String, ByRef strCurrentVer As String) As Boolean
    'data la dir in cui cercala, verifica se c'è una nuova bs--aggc.dll: se la trova la scarica e la registra
    Dim strTmp As String
    Dim strBuffer As String

    Try
      '(confronto la versione della dll locale con quella remota)
      strTmp = strDir.Trim
      If strTmp.Length > 0 Then
        If strTmp.Substring(strTmp.Length - 1) <> "\" Then strTmp = strTmp & "\"
      End If
      strTmp = strTmp & "Busaggn.exe"
      If System.IO.File.Exists(strTmp) Then
        strBuffer = System.Diagnostics.FileVersionInfo.GetVersionInfo(strTmp).FileVersion
        Formatta(strBuffer)

        'contronto la versione installata con quella nuova: se quella nuova è migliore la scarico e la registro
        If IsNewVer((strCurrentVer), strBuffer) Then
          If strBusDir.Length > 0 Then
            If strBusDir.Substring(strBusDir.Length - 1) <> "\" Then strBusDir = strBusDir & "\"
          End If
          'tolgo l'eventuale attributo di sola lettura
          File.SetAttributes(strBusDir & "Busaggn.exe", FileAttributes.Normal)
          FileCopy(strTmp, strBusDir & "Busaggn.exe")
          strCurrentVer = strBuffer
        End If
      Else
        'sul server non ci sono aggiornamenti di Busaggn.exe (o la dir di ricerca sul server non è corretta)
      End If

      Return True
    Catch ex As Exception
      MsgBox("Error starting Bus.net - GetNewAggc: " & ex.ToString)
    End Try
  End Function

  Public Function AddQuotesToFN(ByVal strFilename As String) As String
    If InStr(strFilename, " ") Or InStr(strFilename, ",") Then
      AddQuotesToFN = """" & strFilename & """"
    Else
      AddQuotesToFN = strFilename
    End If
  End Function

  Private Sub Formatta(ByRef str As String)
    'str = str.Substring(0, InStr(1, str, ".") - 1).PadLeft(4, CChar("0")) & str.Substring(InStr(1, str, "."))
    str = Microsoft.VisualBasic.Right("0000" & Microsoft.VisualBasic.Left(str, InStr(1, str, ".") - 1), 4) & Mid(str, InStr(1, str, "."))
  End Sub

  Private Function IsNewVer(ByVal strCurVer As String, ByVal strNewVer As String) As Boolean
    Dim nNum1 As Integer
    Dim nNum2 As Integer
    Dim nNum3 As Integer

    Dim nNum1n As Integer
    Dim nNum2n As Integer
    Dim nNum3n As Integer

    Dim i As Integer

    IsNewVer = True

    On Error GoTo fine

    '-----------------------------------------------------------
    i = InStr(1, strCurVer, ".", vbTextCompare)
    If i > 0 Then
      nNum1 = CType(Microsoft.VisualBasic.Left(strCurVer, i), Integer)
      strCurVer = Mid(strCurVer, i + 1)
    End If

    i = InStr(1, strCurVer, ".", vbTextCompare)
    If i > 0 Then
      nNum2 = CType(Microsoft.VisualBasic.Left(strCurVer, i), Integer)
      strCurVer = Mid(strCurVer, i + 1)
    End If

    nNum3 = strCurVer


    '-----------------------------------------------------------
    i = InStr(1, strNewVer, ".", vbTextCompare)
    If i > 0 Then
      nNum1n = CType(Microsoft.VisualBasic.Left(strNewVer, i), Integer)
      strNewVer = Mid(strNewVer, i + 1)
    End If

    i = InStr(1, strNewVer, ".", vbTextCompare)
    If i > 0 Then
      nNum2n = CType(Microsoft.VisualBasic.Left(strNewVer, i), Integer)
      strNewVer = Mid(strNewVer, i + 1)
    End If

    nNum3n = strNewVer

    '-----------------------------------------------------------
    If nNum1 > nNum1n Then
      IsNewVer = False
    ElseIf nNum2 > nNum2n Then
      IsNewVer = False
    ElseIf nNum3 >= nNum3n Then
      IsNewVer = False
    End If

fine:
    Err.Clear()
  End Function

  Public Function GetSettingReg(ByVal strkey1 As String, ByVal strkey2 As String, ByVal strValuename As String, ByVal strDefault As String) As String
    'legge informazioni dal reg di windows

    ' prima legge se gestito o meno installazione per macchina
    Dim regRootKey As RegistryKey
    Dim strValue As String = "-1"
    Dim strRegRoot As String = ""
    Dim str64 As String = ""

    Try
      'per ora forziamo la compilazione busnet.exe come processo a 32 bit, per cui non serve
      'If Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\", False) Is Nothing And _
      '   Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\Wow6432Node\", False) Is Nothing Then
      '  str64 = ""
      'Else
      '  str64 = "\Wow6432Node"
      'End If

      'verifico se devo leggere da HKLM o DA HKCU
      regRootKey = Registry.LocalMachine.OpenSubKey("SOFTWARE" & str64 & "\NTS\" & strkey1, False)
      If regRootKey Is Nothing Then
        regRootKey.Close()
        Return strDefault
      End If
      strValue = CStr(regRootKey.GetValue("SystemInst"))
      regRootKey.Close()
      If strValue Is Nothing Then strValue = "0"

      If strValue = "-1" Then
        strRegRoot = "SOFTWARE" & str64 & "\NTS\" & strkey1 & "\" & strkey2
        regRootKey = Registry.LocalMachine.OpenSubKey(strRegRoot, False)
      Else
        strRegRoot = "Software\VB and VBA Program Settings\" & strkey1 & "\" & strkey2
        regRootKey = Registry.CurrentUser.OpenSubKey(strRegRoot, False)
      End If
      'adesso leggo la chiave cercata
      If regRootKey Is Nothing Then
        Return strDefault
      End If
      strValue = CStr(regRootKey.GetValue(strValuename))
      If strValue Is Nothing Then
        regRootKey.Close()
        Return strDefault
      End If
      regRootKey.Close()

      Return strValue

    Catch ex As Exception
      If bBatch = False Then
        MsgBox("Error reading Windows registry (System " & IIf(str64 <> "", "64", "32").ToString & " bit)" & vbCrLf & "Error: " & ex.ToString, MsgBoxStyle.Exclamation, "Messaggio da Bus.Net")
      Else
        WriteMsgBoxToLog("Error reading Windows registry (System " & IIf(str64 <> "", "64", "32").ToString & " bit)" & vbCrLf & "Error: " & ex.ToString, True)
      End If
      Return strDefault
    End Try

  End Function

#Region "Mutex"
  Public Function NtsMutexCrea(ByVal strMutexName As String, ByRef bExist As Boolean) As Boolean
    Dim SecurityDesc As New SECURITY_DESCRIPTOR
    Dim SecurityAttr As New SECURITY_ATTRIBUTES
    Dim lr As Long, lLastErr As Long

    NtsMutexCrea = False
    bExist = False
    glNtsMutexGlobal = 0
    glNtsMutexLocal = 0

    'Non crea il mutex se siamo in DEBUG con VB6
    If IsDebugMode() Then Return False

    Dim ptr As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(SecurityDesc))
    Marshal.StructureToPtr(SecurityDesc, ptr, False)

    lr = InitializeSecurityDescriptor(ptr, SECURITY_DESCRIPTOR_REVISION)
    lLastErr = Err.LastDllError
    If lr = 0 Then Return False

    lr = SetSecurityDescriptorDacl(ptr, 1, 0&, 0)
    lLastErr = Err.LastDllError
    If lr = 0 Then Return False

    SecurityAttr.lpSecurityDescriptor = ptr
    SecurityAttr.bInheritHandle = 1
    SecurityAttr.nLength = Len(CObj(SecurityAttr))

    'Crea il mutex Global\
    lr = CreateMutex(VarPtr(SecurityAttr), 1&, "Global\" & strMutexName)
    lLastErr = Err.LastDllError
    If lLastErr = ERROR_ALREADY_EXISTS Then
      bExist = True
    End If
    glNtsMutexGlobal = lr

    'Crea il mutex Local\
    lr = CreateMutex(VarPtr(SecurityAttr), 0&, strMutexName)
    lLastErr = Err.LastDllError
    glNtsMutexLocal = lr

    Return True
RoutineErr:
    'Non segnala nessun errore ed esce come se lo avesse creato (però non esistente)
    'MsgBox "Errore nella routine NtsMutexCrea: " & Err.Number & " - " & Err.Description & "(LastDllErro " & Err.LastDllError & ")", vbCritical, "Messaggio di Business"
    NtsMutexCrea = False
    bExist = False
    glNtsMutexGlobal = 0
    glNtsMutexLocal = 0

  End Function
  Public Function NtsMutexElimina() As Boolean
    NtsMutexElimina = False
    Try

      'Rilascia/chiude il mutex (aperto con CreateMutex) \Global
      If glNtsMutexGlobal <> 0 Then
        ReleaseMutex(glNtsMutexGlobal)
        CloseHandle(glNtsMutexGlobal)
        glNtsMutexGlobal = 0
      End If

      'Rilascia/chiude il mutex (aperto con CreateMutex) \Local
      If glNtsMutexLocal <> 0 Then
        ReleaseMutex(glNtsMutexLocal)
        CloseHandle(glNtsMutexLocal)
        glNtsMutexLocal = 0
      End If

      Return True
    Catch ex As Exception
      'Non segnala nessun errore ed esce come se NON avesse trovato il mutex da eliminare
      'Tanto viene comunque eliminato dal sistema operativo quando il processo viene chiuso
      'MsgBox "Errore nella routine NtsMutexElimina: " & Err.Number & " - " & Err.Description & "(LastDllError " & Err.LastDllError & ")", vbCritical, "Messaggio di Business"
      Return False
    End Try
  End Function
  Public Function NtsMutexTrova(ByVal strMutexName As String) As Boolean
    Try
      'Controlla il mutex Local\
      If OpenMutex(MUTEX_ALL_ACCESS, 0&, strMutexName) Then
        Return True
      End If

      'Controlla il mutex Global\
      If OpenMutex(MUTEX_ALL_ACCESS, 0&, "Global\" & strMutexName) Then
        Return True
      End If

      Return False
    Catch ex As Exception
      'Non segnala nessun errore ed esce come se NON avesse trovato il mutex
      'MsgBox "Errore nella routine TrovaNtsMutex: " & Err.Number & " - " & Err.Description & "(LastDllErro " & Err.LastDllError & ")", vbCritical, "Messaggio di Business"
      Return False
    End Try

  End Function

  Public Function VarPtr(ByVal e As Object) As Integer
    Dim GC As GCHandle = GCHandle.Alloc(e, GCHandleType.Pinned)
    Dim GC2 As Integer = GC.AddrOfPinnedObject.ToInt32
    GC.Free()
    Return GC2
  End Function
#End Region

#Region "Cripta/decripta"
  Public Enum busCryptoEncryptionType
    busBlockEncryption = 1
    busStreamEncryption = 2
  End Enum
  Public Enum busCryptoErrors
    busErrorAquiringContext = vbObjectError + 1056
    busErrorCreatingHash = vbObjectError + 1057
    busErrorCreatingHashData = vbObjectError + 1058
    busErrorDerivingKey = vbObjectError + 1059
    busErrorEncryptingData = vbObjectError + 1060
    busErrorDecryptingData = vbObjectError + 1061
    busErrorInvalidHexString = vbObjectError + 1062
    busErrorMissingParameter = vbObjectError + 1063
    busErrorBadEncryptionType = vbObjectError + 1064
  End Enum
  Private Const ERROR_AQUIRING_CONTEXT As String = "Could not acquire context"
  Private Const ERROR_CREATING_HASH As String = "Could not create hash"
  Private Const ERROR_CREATING_HASH_DATA As String = "Could not create hash data"
  Private Const ERROR_DERIVING_KEY As String = "Could not derive key"
  Private Const ERROR_ENCRYPTING_DATA As String = "Could not encrypt data"
  Private Const ERROR_DECRYPTING_DATA As String = "Could not decrypt data"
  Private Const ERROR_INVALID_HEX_STRING As String = "Not a valid hex string"
  Private Const ERROR_MISSING_PARAMETER As String = "Both a string and a key are required"
  Private Const ERROR_BAD_ENCRYPTION_TYPE As String = "Invalid encryption type specified"
  Private Const CRYPTO_PROVIDER As String = "Microsoft Base Cryptographic Provider v1.0"
  Private Const CRYPT_VERIFYCONTEXT As Integer = &HF0000000
  Private Const PROV_RSA_FULL As Integer = 1
  Private Const ALG_CLASS_HASH As Integer = 32768 ' (4 << 13)
  Private Const ALG_TYPE_ANY As Integer = 0
  Private Const ALG_SID_MD5 As Integer = 3
  Private Const CALG_MD5 As Integer = ALG_CLASS_HASH Or ALG_TYPE_ANY Or ALG_SID_MD5
  Private Const CRYPT_NO_SALT As Integer = &H10S
  Private Const ALG_CLASS_DATA_ENCRYPT As Integer = 24576 ' (3 << 13)
  Private Const ALG_TYPE_BLOCK As Integer = 1536 ' (3 << 9)
  Private Const ALG_TYPE_STREAM As Integer = 2048 ' (4 << 9)
  Private Const ALG_SID_RC2 As Integer = 2
  Private Const ALG_SID_RC4 As Integer = 1
  Private Const CALG_RC2 As Integer = ALG_CLASS_DATA_ENCRYPT Or ALG_TYPE_BLOCK Or ALG_SID_RC2
  Private Const CALG_RC4 As Integer = ALG_CLASS_DATA_ENCRYPT Or ALG_TYPE_STREAM Or ALG_SID_RC4

  Private Declare Function CryptAcquireContext Lib "advapi32.dll" Alias "CryptAcquireContextA" (ByRef phProv As Integer, ByVal pszContainer As String, ByVal pszProvider As String, ByVal dwProvType As Integer, ByVal dwFlags As Integer) As Integer
  Private Declare Function CryptCreateHash Lib "advapi32.dll" (ByVal hProv As Integer, ByVal algID As Integer, ByVal hKey As Integer, ByVal dwFlags As Integer, ByRef phHash As Integer) As Integer
  Private Declare Function CryptDeriveKey Lib "advapi32.dll" (ByVal hProv As Integer, ByVal algID As Integer, ByVal hBaseData As Integer, ByVal dwFlags As Integer, ByRef phKey As Integer) As Integer
  Private Declare Function CryptDestroyHash Lib "advapi32.dll" (ByVal hHash As Integer) As Integer
  Private Declare Function CryptDestroyKey Lib "advapi32.dll" (ByVal hKey As Integer) As Integer
  Private Declare Function CryptEncrypt Lib "advapi32.dll" (ByVal hKey As Integer, ByVal hHash As Integer, ByVal Final As Integer, ByVal dwFlags As Integer, ByVal pbData As String, ByRef pdwDataLen As Integer, ByVal dwBufLen As Integer) As Integer
  Private Declare Function CryptDecrypt Lib "advapi32.dll" (ByVal hKey As Integer, ByVal hHash As Integer, ByVal Final As Integer, ByVal dwFlags As Integer, ByVal pbData As String, ByRef pdwDataLen As Integer) As Integer
  Private Declare Function CryptHashData Lib "advapi32.dll" (ByVal hHash As Integer, ByVal pbData As String, ByVal dwDataLen As Integer, ByVal dwFlags As Integer) As Integer
  Private Declare Function CryptReleaseContext Lib "advapi32.dll" (ByVal hProv As Integer, ByVal dwFlags As Integer) As Integer
  Private Shared Function EncryptDecrypt(ByVal sText As String, ByVal sKeyRoot As String, ByVal bEncrypt As Boolean, ByVal enEncryptionType As busCryptoEncryptionType) As String

    Dim lResult As Integer
    Dim lHash As Integer
    Dim lKey As Integer
    Dim lHashPassword As Integer
    Dim lFlags As Integer
    Dim lData As Integer
    Dim lClear As Integer
    Dim lEncryptionType As Integer
    Dim m_lProvider As Integer

    Try
      CryptAcquireContext(m_lProvider, vbNullString, CRYPTO_PROVIDER, PROV_RSA_FULL, CRYPT_VERIFYCONTEXT)

      ' Create a handle to a hash object  using the MD5 algorithm
      lResult = CryptCreateHash(m_lProvider, CALG_MD5, 0, 0, lHash)
      If lResult = 0 Then
        Throw New Exception("CLN__STD.EncryptDecrypt error: " & ERROR_CREATING_HASH)
      End If

      ' Add some data to the hash object for use in generating our key
      ' sKeyRoot is, in effect, our key and should be fairly complex and
      ' not easily guessed
      lHashPassword = Len(sKeyRoot)
      lResult = CryptHashData(lHash, sKeyRoot, lHashPassword, 0)
      If lResult = 0 Then
        Throw New Exception("CLN__STD.EncryptDecrypt error: " & ERROR_CREATING_HASH_DATA)
      End If

      ' Select appropriate encryption method
      If enEncryptionType = busCryptoEncryptionType.busBlockEncryption Then
        lEncryptionType = CALG_RC2
      Else
        lEncryptionType = CALG_RC4
      End If

      ' Generate a session key for use with the cypher
      lFlags = CRYPT_NO_SALT
      lResult = CryptDeriveKey(m_lProvider, lEncryptionType, lHash, lFlags, lKey)
      If lResult = 0 Then
        Throw New Exception("CLN__STD.EncryptDecrypt error: " & ERROR_DERIVING_KEY)
      End If

      ' Encrypt or decrypt data as required
      lData = Len(sText)
      If bEncrypt Then
        lClear = lData
        ' Call with a null first to see how long the string needs to be
        lResult = CryptEncrypt(lKey, 0, 1, 0, vbNullString, lData, lClear)
        sText = sText & New String(" "c, lData - lClear)
        ' Encrypt some text
        lResult = CryptEncrypt(lKey, 0, 1, 0, sText, lClear, lData)
        If lResult = 0 Then
          Throw New Exception("CLN__STD.EncryptDecrypt error: " & ERROR_ENCRYPTING_DATA)
        End If
      Else
        ' You may want to slow down a hacker trying multiple passwords to decrypt
        ' by brut force with a sleep command like this...
        'Sleep 1000

        ' Decrypt the text
        lResult = CryptDecrypt(lKey, 0, 1, 0, sText, lData)
        sText = Microsoft.VisualBasic.Left(sText, lData)
        If lResult = 0 Then
          Throw New Exception("CLN__STD.EncryptDecrypt error: " & ERROR_DECRYPTING_DATA)
        End If
      End If

      EncryptDecrypt = sText

      If lHash <> 0 Then
        lResult = CryptDestroyHash(lHash)
      End If
      If lKey <> 0 Then
        lResult = CryptDestroyKey(lKey)
      End If
    Catch ex As Exception
      Throw ex
    Finally
      If m_lProvider <> 0 Then CryptReleaseContext(m_lProvider, 0)
    End Try
  End Function
  Private Shared Function ConvertStringFromHex(ByVal sText As String) As String
    Dim lCount As Integer = 0
    Dim sChar As String = ""
    Dim sResult As String = ""
    Dim lLength As Integer = 0

    ConvertStringFromHex = ""

    lLength = Len(sText)
    If lLength Mod 2 <> 0 Then
      Throw New Exception("CLN__STD.ConvertStringFromHex: Error " & ERROR_INVALID_HEX_STRING)
    End If

    For lCount = 1 To lLength
      sChar = Mid(sText, lCount, 1)
      If sChar < "0" Or sChar > "9" Then
        If sChar < "A" Or sChar > "F" Then
          Throw New Exception("CLN__STD.ConvertStringFromHex: Error " & ERROR_INVALID_HEX_STRING)
        End If
      End If
    Next

    For lCount = 1 To lLength Step 2
      sResult = sResult & Chr(CInt("&H" & Mid(sText, lCount, 2)))
    Next

    Return sResult
  End Function
  Private Shared Function ConvertStringToHex(ByVal sText As String) As String
    Dim lCount As Integer = 0
    Dim sHex As String = ""
    Dim sResult As String = ""

    For lCount = 1 To Len(sText)
      sHex = Hex(Asc(Mid(sText, lCount, 1)))
      If Len(sHex) = 1 Then
        sHex = "0" & sHex
      End If
      sResult = sResult & sHex
    Next
    Return sResult
  End Function
  Public Shared Function PwdCript(ByVal strData As String, ByRef strEncrypted As String) As Boolean
    Try
      If strData = "" Then
        strEncrypted = ""
        Return True
      End If
      strEncrypted = EncryptDecrypt(strData, "acT &%%15?éè\ Ygt<+[", True, busCryptoEncryptionType.busBlockEncryption)
      strEncrypted = ConvertStringToHex(strEncrypted)

      Return True
    Catch ex As Exception
      MsgBox("Error in PwdCript" & vbCrLf & "Error: " & ex.ToString, MsgBoxStyle.Exclamation, "Messaggio da Bus.Net")
    End Try
  End Function
  Public Shared Function PwdDeCript(ByRef strEncrypted As String, ByRef strDescripted As String) As Boolean
    Try
      If strEncrypted = "" Then
        strDescripted = ""
        Return True
      End If
      strEncrypted = ConvertStringFromHex(strEncrypted)
      strDescripted = EncryptDecrypt(strEncrypted, "acT &%%15?éè\ Ygt<+[", False, busCryptoEncryptionType.busBlockEncryption)
      Return True

    Catch ex As Exception
      MsgBox("Error in PwdDeCript" & vbCrLf & "Error: " & ex.ToString, MsgBoxStyle.Exclamation, "Messaggio da Bus.Net")
    End Try
  End Function
#End Region

  Public Shared Function IsDesignMode() As Boolean
    If Debugger.IsAttached Then
      Return False
    Else
      Return True
    End If
  End Function
  Public Shared Function IsDebugMode() As Boolean
    Return Not IsDesignMode()
  End Function

  Public Sub WriteMsgBoxToLog(ByVal strMsg As String, ByVal bAccoda As Boolean)
    'se sono stato avviato in modalità batch (tipo busbatch) i messaggi vengono a finire tutti qui (eccetto quelli du busnet.exe)
    Dim lw1 As New StreamWriter(DirP & "\BusNetBatch_" & System.Diagnostics.Process.GetCurrentProcess.Id.ToString & ".log", bAccoda)
    lw1.WriteLine(DateTime.Now.ToString & " " & strMsg)
    lw1.Flush()
    lw1.Close()
  End Sub
End Class
