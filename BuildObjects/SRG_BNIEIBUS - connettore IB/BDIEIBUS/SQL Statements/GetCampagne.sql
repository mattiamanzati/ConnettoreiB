-- Da usare per il debug in management studio
--DECLARE @ditta varchar(50), @includi_lead_clienti varchar(50)
--SELECT @ditta = 'CMATIC', @includi_lead_clienti = '0'

SELECT
	codditt + '§' + cast(tb_codcamp as varchar)  as xx_chiave,
	codditt, 
	tb_codcamp, 
	tb_descamp,
	getdate() as xx_ultagg 
FROM 
	dbo.tabcamp WITH (NOLOCK)
WHERE 1=1
	AND codditt = @ditta 

	