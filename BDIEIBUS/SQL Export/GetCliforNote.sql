/* Da usare per il debug in management studio
DECLARE @ditta varchar(200)
SELECT @ditta = 'CMATIC'
*/

SELECT  
       codditt + '§0§' + CAST(an_conto AS VARCHAR) AS xx_chiave,
	   an_tipo,
       an_conto,
	   0 as xx_progressivo,
       'Note generiche' as xx_codnote, 
	   anagra.an_note2 as xx_desnote,
	   anagra.an_ultagg as xx_ultagg
FROM   dbo.anagra WITH (NOLOCK)
WHERE  1=1
	AND codditt = @ditta
	AND an_status = 'A' 
	AND	( ( an_tipo = 'C' AND @tipocf = 'C' ) OR ( an_tipo = 'F' AND @tipocf = 'F' ) OR ( @tipocf = 'CF' ) ) 
	AND an_note2 IS NOT null 
UNION ALL
SELECT anagra.codditt + '§' + CAST(tb_codnote AS VARCHAR) + '§' + CAST(an_conto AS VARCHAR),
	   an_tipo, 
       an_conto, 
       tb_codnote, 
       tb_desnote, 
	   tb_testonot as le_note2,
	   anagra.an_ultagg
FROM   anagra WITH (NOLOCK)
       LEFT JOIN tabnote WITH (NOLOCK)
              ON anagra.codditt = tabnote.codditt 
                 AND an_conto = tb_contonot 
WHERE  anagra.codditt = @ditta
       AND an_tipo <> 'S' 
       AND NOT tb_codnote IS NULL 
       AND an_status = 'A' 
       AND ( ( an_tipo = 'C' AND @tipocf = 'C' ) OR ( an_tipo = 'F' AND @tipocf = 'F' ) OR ( @tipocf = 'CF' ) ) 
ORDER  BY 
	an_conto,
	an_tipo