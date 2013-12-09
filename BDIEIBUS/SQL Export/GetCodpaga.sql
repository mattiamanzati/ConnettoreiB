/* Da usare per il debug in management studio
DECLARE @release varchar(200)
SELECT @release = '1.0'
*/

SELECT 
	tb_codpaga, 
	tb_despaga,
	getdate() as xx_ultagg
FROM 
	tabpaga WITH (NOLOCK)
ORDER BY 
	tb_codpaga