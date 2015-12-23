-- Da usare per il debug in management studio
--DECLARE @ditta varchar(200)
--SELECT @ditta = 'CMATIC'

SELECT
  codditt + '§' + cast(tb_codhhse as varchar) as xx_chiave,
  tb_codhhse, 
  tb_deshhse
FROM 
  tabhhse
WHERE 1=1 
and codditt = 'CMATIC'
and tb_deshhse IS NOT NULL
