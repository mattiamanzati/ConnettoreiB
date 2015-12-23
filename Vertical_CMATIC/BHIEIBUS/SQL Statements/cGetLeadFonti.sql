-- Da usare per il debug in management studio
--DECLARE @ditta varchar(200)
--SELECT @ditta = 'CMATIC'

SELECT
  codditt + '§' + cast(tb_codhhfn as varchar) as xx_chiave,
  tb_codhhfn, 
  tb_deshhfn
FROM 
  tabhhfn
WHERE 1=1 
and codditt = @ditta
and tb_deshhfn IS NOT NULL
