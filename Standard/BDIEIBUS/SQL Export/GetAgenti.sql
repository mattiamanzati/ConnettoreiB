/* Da usare per il debug in management studio
DECLARE @ditta varchar(200)
SELECT @ditta = 'SONN'
*/

SELECT
  codditt + '§' + cast(tb_codcage as varchar) as xx_chiave,
  tb_codcage, 
  tb_descage
FROM 
  tabcage
WHERE 1=1 
and codditt = @ditta
and tb_descage IS NOT null
