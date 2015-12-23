-- Da usare per il debug in management studio
--DECLARE @ditta varchar(200)
--SELECT @ditta = 'CMATIC'

SELECT 
	dbo.anagra.codditt + '§' + cast(hh_opnome as varchar)  + '§' + CAST(an_conto AS VARCHAR) as xx_chiave,
	hh_opnome as xx_cod_operatore, 
	an_conto  as xx_conto
FROM [HH_OperZone] 
    INNER JOIN  dbo.anagra 
	   ON ([HH_OperZone].hh_codzona =  anagra.an_zona) 
WHERE 1=1 
and dbo.anagra.codditt = @ditta