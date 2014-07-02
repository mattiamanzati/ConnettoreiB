-- Da usare per il debug in management studio
--DECLARE @ditta varchar(50)
--SELECT @ditta = 'CMATIC'

SELECT 
 CAST(td_numord AS VARCHAR) + '§' + CAST(td_anno AS VARCHAR) + '§' +  td_serie + '§' +  td_tipork + '§' +  CAST(td_vers AS VARCHAR) + '§' + testoff.codditt as xx_chiave,
 testoff.codditt,
 td_codlead,
 td_anno,
 td_serie,
 td_numord,
 td_vers,
 td_datord,
 td_ca,
 td_riferim,
 td_oggetto,
 td_tipobf,
 tb_destpbf,
 td_totdoc,
 testoff.td_valuta,
 CASE td_codoppo WHEN 0 THEN NULL ELSE td_codoppo END AS td_codoppo,
 opportun.op_oggetto as op_oggetto,
 CASE td_validgg WHEN 0 THEN NULL ELSE td_validgg END AS td_validgg,
 CASE td_listino WHEN 0 THEN NULL ELSE td_listino END AS td_listino,
 dbo.tablist.tb_deslist as tb_deslist,
 CASE td_codpaga WHEN 0 THEN NULL ELSE td_codpaga END AS td_codpaga,
 tb_despaga,
 td_datcons,
 CASE td_consggconf WHEN 0 THEN NULL ELSE td_consggconf END AS td_consggconf,
 td_desconsx,
 testoff.td_note,
 testoff.td_rilasciato, 
 testoff.td_flstam, 
 testoff.td_confermato, 
 testoff.td_abband, 
 testoff.td_flevas, 
 testoff.td_annull,
 testoff.td_chiuso,
 testoff.td_ultagg as xx_ultagg
FROM testoff WITH (NOLOCK)
    INNER JOIN leads  WITH (NOLOCK)
	           ON leads.codditt = testoff.codditt
              AND leads.le_codlead = testoff.td_codlead
    LEFT OUTER JOIN opportun WITH (NOLOCK)
	           ON opportun.codditt = testoff.codditt
	          AND opportun.op_codoppo = testoff.td_codoppo
    LEFT OUTER JOIN tabtpbf WITH (NOLOCK)
	           ON tabtpbf.codditt = testoff.codditt
	          AND tabtpbf.tb_codtpbf = testoff.td_tipobf
    LEFT OUTER JOIN tablist WITH (NOLOCK)
	           ON tablist.codditt = testoff.codditt
	          AND tablist.tb_codlist = testoff.td_listino
    LEFT OUTER JOIN tabpaga WITH (NOLOCK)
	           ON tabpaga.tb_codpaga = testoff.td_codpaga
WHERE 1=1
 	   AND testoff.codditt = @ditta 
	   AND (GETDATE() - testoff.td_datord ) < cast(@gg_offerte as integer) 
	   AND (le_conto = '0' or @includi_lead_clienti <> '0')
	   AND leads.le_status <> 'I'
	   AND testoff.td_tipork = '!'
	