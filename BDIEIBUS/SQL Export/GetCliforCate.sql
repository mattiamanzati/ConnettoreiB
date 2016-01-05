/* Da usare per il debug in management studio
DECLARE @ditta varchar(200)
SELECT @ditta = 'SONN'
*/
SELECT
  tb_codcate, 
  tb_descate
FROM 
  tabcate 
WHERE 1=1 
and codditt = @ditta
and tb_descate IS NOT null