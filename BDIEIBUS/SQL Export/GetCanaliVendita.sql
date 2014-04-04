/* Da usare per il debug in management studio
DECLARE @ditta varchar(200)
SELECT @ditta = 'SONN'
*/
SELECT
  tb_codcana, 
  tb_descana
FROM 
  tabcana 
WHERE 1=1 
and codditt = @ditta
and tb_descana IS NOT null
