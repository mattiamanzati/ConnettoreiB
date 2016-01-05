/* Da usare per il debug in management studio
DECLARE @release varchar(200)
SELECT @release = '1.0', @tipocf = 'CF'
*/

SELECT an_tipo, 
       an_conto, 
       an_blocco, 
	   CASE 
         WHEN an_blocco = 'F' THEN 'Fuori fido' 
		 WHEN an_blocco = 'I' THEN 'Insoluti' 
		 WHEN an_blocco = 'R' THEN 'Rim. dirette scadute' 
		 WHEN an_blocco = 'B' THEN 'Blocco Fisso' 
       END              AS xx_blocco,
	   an_ultagg
FROM   anagra WITH (NOLOCK)
WHERE 1=1
 	   AND anagra.codditt = @ditta 
       AND an_tipo <> 'S' 
       AND an_status = 'A' 
       AND ( ( an_tipo = 'C' AND @tipocf = 'C' ) OR ( an_tipo = 'F' AND @tipocf = 'F' ) OR ( an_tipo <> 'S' AND @tipocf = 'CF' ) ) 
	   AND an_blocco <> 'N'
ORDER  BY 
	an_tipo, 
    an_conto 