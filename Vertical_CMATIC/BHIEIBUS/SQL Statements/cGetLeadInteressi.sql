-- Da usare per il debug in management studio
--DECLARE @ditta varchar(200)
--SELECT @ditta = 'CMATIC'

SELECT
  codditt + '§' + cast(tb_codhhin as varchar) as xx_chiave,
  tb_codhhin, 
  tb_deshhin
FROM 
  tabhhin
WHERE 1=1 
and codditt = @ditta
and tb_deshhin IS NOT NULL
