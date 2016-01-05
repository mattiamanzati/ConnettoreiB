/* Da usare per il debug in management studio
DECLARE @ditta varchar(200)
SELECT @ditta = 'SONN'
*/
SELECT
  cast(tb_codruaz as varchar) as xx_chiave,
  tb_codruaz, 
  tb_desruaz
FROM 
  tabruaz
WHERE 1=1 
and tb_desruaz IS NOT NULL
