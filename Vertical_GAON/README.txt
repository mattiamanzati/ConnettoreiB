Installazione Personalizzazione
--------------------------------
1. Nella cartella Bus\Scripts creare un file chiamato DLLMAP.INI in cui incollare questo (vedi file):

	*|BDIEIBUS|BHIEIBUS|NtsInformatica.CLHIEIBUS
	*|BEIEIBUS|BFIEIBUS|NtsInformatica.CLFIEIBUS

2. Nel registro di Business aggiungere nella root\OPZIONI la seguente sequenza chiave/valore

	CHILD_BNIEIBUS.FRMIEIBUS    BOIEIBUS;FROIEIBUS

