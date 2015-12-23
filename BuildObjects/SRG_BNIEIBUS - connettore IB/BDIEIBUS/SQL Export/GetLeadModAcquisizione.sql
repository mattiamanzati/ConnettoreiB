/* Da usare per il debug in management studio
DECLARE @ditta varchar(200)
SELECT @ditta = 'SONN'
*/
SELECT
  codditt + '§' + cast(tb_codperv as varchar) as xx_chiave,
  tb_codperv, 
  tb_desperv
FROM 
  tabperv
WHERE 1=1 
and codditt = @ditta
and tb_desperv IS NOT NULL
