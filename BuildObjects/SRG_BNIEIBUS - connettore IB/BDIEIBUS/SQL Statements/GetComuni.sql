/* Da usare per il debug in management studio
DECLARE @release varchar(200)
SELECT @release = '1.0'
*/

SELECT 
	co_codcomu,
	RTRIM(ltrim(co_denom)) as co_denom, 
	co_prov, 
	co_cap,
	getdate() as xx_ultagg  
 FROM 
	comuni WITH (NOLOCK)
 ORDER BY 
	co_codcomu