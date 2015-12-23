/* Da usare per il debug in management studio
DECLARE @release varchar(200)
SELECT @release = '1.0'
CAST(testmag.tm_conto AS VARCHAR) + '§' + CAST(testmag.tm_numdoc AS VARCHAR) + '§' + testmag.codditt + '§' + testmag.an_tipo + '§' + testmag.tm_anno  as xx_numreg,
*/
--fatture e note di credito attive
--fatture e note di credito passive
--altri documenti di magazzino
--ordini e impegni

SELECT 
	'M' as xx_tipo,
	CAST(tm_note AS VARCHAR(3000)) as tm_note,
	tm_riferim,
	testmag.codditt + '§' + CAST(tm_conto AS VARCHAR)+ '§' + CAST(tm_anno as varchar) + '§' + tm_serie + '§' + CAST(tm_numdoc as varchar) + '§' + CAST(tm_numpar as varchar) as xx_numreg,
	an_tipo, 
	an_conto,
	tm_tipork,
	tm_anno, 
	tm_serie,
	tm_numdoc,
	tm_datdoc,
	-- Utilizzeremo questa quando gestiremo la valuta sulle testate documenti
	-- CASE tm_valuta WHEN 0 THEN tm_totdoc ELSE tm_totdocv END AS tm_totdoc,
	tm_totdoc - tm_totomag as tm_totdoc,
	tm_ultagg,  
	tb_desvalu,
	tm_magaz,
	tm_numfat, 
	tm_alffat, 
	tm_datfat,
	tm_annpar, 
	tm_alfpar,
	tm_numpar,
	tm_valuta,
	'0' as xx_flevas,  
	tm_tipobf as xx_tipobf, 
	tb_destpbf  
FROM testmag WITH (NOLOCK)
	INNER JOIN anagra WITH (NOLOCK)
	     ON testmag.codditt = anagra.codditt AND testmag.tm_conto = anagra.an_conto  
    INNER JOIN tabtpbf WITH (NOLOCK)
	     ON testmag.codditt = tabtpbf.codditt AND testmag.tm_tipobf = tabtpbf.tb_codtpbf  
    LEFT JOIN tabvalu WITH (NOLOCK)
	     ON testmag.tm_valuta = tabvalu.tb_codvalu 
WHERE 1=1
	AND anagra.codditt =  @ditta
    AND tm_anno >= Year(Getdate()) -2  
    AND an_tipo <> 'S' 
    AND an_status = 'A' 
    AND tm_tipork in ('A','D','N') 
	AND an_tipo = 'C' 
	AND (GETDATE() - testmag.tm_datdoc ) < cast(@gg_documenti as integer)    
UNION
SELECT 
	'M' as xx_tipo,
	CAST(tm_note AS VARCHAR(3000)),
	tm_riferim,
	testmag.codditt + '§' + CAST(tm_conto AS VARCHAR)+ '§' + CAST(tm_annpar as varchar) + '§' + tm_alfpar + '§' + CAST(tm_numdoc as varchar) + '§' + CAST(tm_numpar as varchar)  as xx_numreg,
--	testmag.codditt + '§' + CAST(tm_conto AS VARCHAR)+ '§' + CAST(tm_anno as varchar) + '§' + tm_serie + '§' + CAST(tm_numdoc as varchar) as xx_numreg,
	an_tipo, 
	an_conto,
	tm_tipork,
	tm_anno, 
	tm_serie,
	tm_numdoc,
	tm_datdoc,
	tm_totdoc - tm_totomag,
	tm_ultagg,  
	tb_desvalu,
	tm_magaz,
	tm_numfat, 
	tm_alffat, 
	tm_datfat,
	tm_annpar, 
	tm_alfpar,
	tm_numpar,
	tm_valuta,
	'0' as xx_flevas,  
	tm_tipobf as xx_tipobf, 
	tb_destpbf  
FROM testmag WITH (NOLOCK)
	INNER JOIN anagra WITH (NOLOCK)
	     ON testmag.codditt = anagra.codditt AND testmag.tm_conto = anagra.an_conto  
    INNER JOIN tabtpbf WITH (NOLOCK)
	     ON testmag.codditt = tabtpbf.codditt AND testmag.tm_tipobf = tabtpbf.tb_codtpbf  
    LEFT JOIN tabvalu WITH (NOLOCK)
	     ON testmag.tm_valuta = tabvalu.tb_codvalu 
WHERE 1=1
	AND anagra.codditt =  @ditta
    AND tm_anno >= Year(Getdate()) -2  
    AND an_tipo <> 'S' 
    AND an_status = 'A' 
    AND tm_tipork in ('L','K','J') 
	AND (GETDATE() - testmag.tm_datdoc ) < cast(@gg_documenti as integer) 
	AND an_tipo = 'F' 
	AND tm_annpar > 0 AND tm_numpar > 0
UNION 
SELECT 
	'M' as xx_tipo, 
	CAST(tm_note AS VARCHAR(3000)),
	tm_riferim,
	testmag.codditt + '§' + CAST(tm_conto AS VARCHAR)+ '§' + CAST(tm_anno as varchar) + '§' + tm_serie + '§' + CAST(tm_numdoc as varchar) + '§' + CAST(tm_tipork as varchar) as xx_numreg,
	an_tipo, 
	an_conto,
	tm_tipork,
	tm_anno, 
	tm_serie,
	tm_numdoc,
	tm_datdoc,
	tm_totdoc - tm_totomag,
	tm_ultagg,  
	tb_desvalu,
	tm_magaz,
	tm_numfat, 
	tm_alffat, 
	tm_datfat,
	tm_annpar, 
	tm_alfpar,
	tm_numpar,
	tm_valuta,
	'0' as xx_flevas,  
	tm_tipobf as xx_tipobf, 
	tb_destpbf  
FROM testmag WITH (NOLOCK)
	INNER JOIN anagra WITH (NOLOCK)
	     ON testmag.codditt = anagra.codditt AND testmag.tm_conto = anagra.an_conto  
    INNER JOIN tabtpbf WITH (NOLOCK)
	     ON testmag.codditt = tabtpbf.codditt AND testmag.tm_tipobf = tabtpbf.tb_codtpbf  
    LEFT JOIN tabvalu WITH (NOLOCK)
	     ON testmag.tm_valuta = tabvalu.tb_codvalu 
WHERE 1=1
	AND anagra.codditt =  @ditta
    AND tm_anno >= Year(Getdate()) -2  
    AND an_tipo <> 'S' 
    AND an_status = 'A' 
    AND tm_tipork NOT IN ('Z', 'T', 'U') 
    AND tm_tipork not in ('L','K','J','A','D','N')
	AND (GETDATE() - testmag.tm_datdoc ) < cast(@gg_documenti as integer)                  
UNION 
SELECT 
	'O' as xx_tipo,
	CAST(td_note AS VARCHAR(3000)),
	td_riferim,
	testord.codditt + '§' + CAST(td_conto AS VARCHAR)+ '§' + CAST(td_anno as varchar) + '§' + td_serie + '§' + CAST(td_numord as varchar) + '§' + td_tipork as xx_numreg,
	an_tipo, 
	an_conto, 
	td_tipork, 
	td_anno,
	td_serie,
	td_numord, 
	td_datord, 
	td_totdoc - td_totomag,
	td_ultagg,  
	tb_desvalu,
	td_magaz,
	0, 
	' ', 
	null,
	0, 
	' ',
	0, 
	td_valuta,
	CASE WHEN td_flevas = 'C' THEN '1' ELSE '0' END, 
	td_tipobf, 
	tb_destpbf  
FROM testord WITH (NOLOCK)
	INNER JOIN anagra WITH (NOLOCK)
	     ON testord.codditt = anagra.codditt AND testord.td_conto = anagra.an_conto  
    INNER JOIN tabtpbf WITH (NOLOCK)
	     ON testord.codditt = tabtpbf.codditt AND testord.td_tipobf = tabtpbf.tb_codtpbf  
    LEFT JOIN tabvalu WITH (NOLOCK)
	     ON testord.td_valuta = tabvalu.tb_codvalu 
WHERE 1=1
	AND anagra.codditt =  @ditta
	AND an_tipo <> 'S' 
	AND ((an_tipo = 'C' and @tipocf = 'C') or (an_tipo = 'F' and @tipocf = 'F') or (an_tipo <> 'S' and @tipocf = 'CF'))  
	AND an_status = 'A' 
	AND td_tipork NOT IN ('X', 'H', 'Y') 
	AND cast((GETDATE() - testord.td_datord ) as integer) < cast(@gg_documenti as integer)
