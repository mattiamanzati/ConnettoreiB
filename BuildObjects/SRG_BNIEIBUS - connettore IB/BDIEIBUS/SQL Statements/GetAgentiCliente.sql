/* Da usare per il debug in management studio
DECLARE @release varchar(200)
SELECT @release = '1.0'
*/

SELECT 
	an_agente, 
	an_agente2 
FROM 
	anagra WITH (NOLOCK)
WHERE 1=1
	AND codditt= @ditta
	AND an_conto = @conto