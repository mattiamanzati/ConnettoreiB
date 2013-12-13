/* Da usare per il debug in management studio
DECLARE @release varchar(200)
SELECT @release = '1.0'
*/

SELECT top 500 
	an_conto, 
	an_indir, 
	an_cap, 
	an_citta, 
	an_prov, 
	an_stato, 
	ISNULL(tb_desstat, 'ITALIA') as xx_desstato
FROM anagra WITH (NOLOCK)
   LEFT JOIN tabstat WITH (NOLOCK)
       ON an_stato = tb_codstat 
WHERE 1=1
	AND anagra.codditt = @ditta
    AND  (isnull(an_hhlat_ib,0) = 0 OR isnull(an_hhlon_ib,0) = 0) 
	AND an_indir is not null
