/* Da usare per il debug in management studio
DECLARE @release varchar(200)
SELECT @release = '1.0'
*/

SELECT 
	an_tipo, 
	dd_conto,
	dd_coddest,
	dd_nomdest, 
	dd_inddest, 
	dd_capdest, 
	ltrim(rtrim(dd_locdest)) as dd_locdest, 
	dd_prodest,
	dd_telef,
	dd_faxtlx,
	dd_email, 
	dd_note,
	getdate() as xx_ultagg
FROM 
	destdiv WITH (NOLOCK)
	    INNER JOIN anagra WITH (NOLOCK)
			ON destdiv.codditt = anagra.codditt 
		   AND destdiv.dd_conto = anagra.an_conto 
WHERE 1=1
	AND anagra.codditt =  @ditta 
	AND an_tipo <> 'S'
	AND an_status = 'A'
ORDER BY 
	an_tipo, 
	dd_conto