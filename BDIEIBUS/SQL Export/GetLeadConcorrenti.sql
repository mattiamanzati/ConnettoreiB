/* Da usare per il debug in management studio
DECLARE @ditta varchar(200)
SELECT @ditta = 'SONN'
*/
SELECT
  codditt + '§' + cast(tb_codcptr as varchar) as xx_chiave,
  tb_codcptr, 
  tb_descptr
FROM 
  tabcptr
WHERE 1=1 
and codditt = @ditta
and tb_descptr IS NOT NULL