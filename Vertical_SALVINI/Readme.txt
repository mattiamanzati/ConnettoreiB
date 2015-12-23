Installazione Personalizzazione
--------------------------------
1. Nella cartella Bus\Scripts creare un file chiamato DLLMAP.INI in cui incollare questo (vedi file):

	*|BDIEIBUS|BHIEIBUS|NtsInformatica.CLHIEIBUS
	*|BEIEIBUS|BFIEIBUS|NtsInformatica.CLFIEIBUS

2. Nel registro di Business aggiungere nella root\OPZIONI la seguente sequenza chiave/valore

	CHILD_BNIEIBUS.FRMIEIBUS    BOIEIBUS;FROIEIBUS

NOTA:
L'export da Sonn e' pianificato in un file situato sotto Bus\Agg\schedulazione\exportIB.bat
Ecco il contenuto:

tipork=CLI;FOR;ART;LIS;SCO;DOC;CAT;MAG;CIT;PAG;COO;
ditta=SONN
