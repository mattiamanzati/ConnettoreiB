
SELECT     
	'OFF' AS xx_tipo, 
	CAST(td_note AS VARCHAR(3000)) as tm_note, 
	td_riferim as tm_riferim,
	upper(td.codditt + '§' + CAST(td.td_conto AS VARCHAR)+ '§' + CAST(td.td_anno as varchar) + '§' + td.td_serie + '§' + CAST(td.td_numord as varchar)) + '§' + td.td_tipork AS xx_numreg,
	an.an_tipo, 
	an.an_conto,   
	td.td_tipork AS tm_tipork, 
	td.td_anno AS tm_anno, 
	td.td_serie AS tm_serie, 
	td.td_numord AS tm_numdoc,   
	td.td_datord AS tm_datdoc,   
	CASE td.td_valuta WHEN 0 THEN td_totdoc ELSE td_totdocv END AS tm_totdoc,  
	td.td_ultagg AS tm_ultagg,   
	tv.tb_desvalu, CAST(0 AS smallint) AS tm_magaz, 
	CAST(0 AS int) AS tm_numfat,   
	' ' AS tm_alffat, 
	CAST('1900-01-01' AS datetime) AS tm_datfat,   
	CAST(0 AS int) AS tm_annpar, ' ' AS tm_alfpar, 
	CAST(0 AS int) AS tm_numpar,   
	td.td_valuta as tm_valuta,  
	CASE WHEN td_flevas = 'S' THEN '1' ELSE '0' END AS xx_flevas, 
	td.td_tipobf AS xx_tipobf, 
	bf.tb_destpbf  
FROM    
        testoff AS td WITH (NOLOCK) 
		   LEFT OUTER JOIN leads AS le WITH (NOLOCK) ON td.codditt = le.codditt AND td.td_codlead = le.le_codlead 
		   LEFT OUTER JOIN anagra AS an WITH (NOLOCK) ON le.codditt = an.codditt AND le.le_conto = an.an_conto 
		   LEFT OUTER JOIN tabtpbf AS bf WITH (NOLOCK) ON td.codditt = bf.codditt AND td.td_tipobf = bf.tb_codtpbf 
		   LEFT OUTER JOIN tabvalu as tv WITH (NOLOCK) ON td.td_valuta = tv.tb_codvalu 
		   INNER JOIN  
            (
				SELECT     codditt, td_tipork, td_anno, td_numord, td_serie, MAX(td_vers) AS td_vers   
				FROM          testoff AS testoff_1   
				GROUP BY codditt, td_tipork, td_anno, td_numord, td_serie
			) AS MX ON MX.codditt = td.codditt AND MX.td_tipork = td.td_tipork AND MX.td_anno = td.td_anno AND  MX.td_serie = td.td_serie And MX.td_numord = td.td_numord And MX.td_vers = td.td_vers 
WHERE  (1 = 1) 
	AND td.td_tipork = '!' 
	AND an.codditt =  @ditta 
	AND (GETDATE() - td.td_datord ) < cast(@gg_offerte as integer) 
