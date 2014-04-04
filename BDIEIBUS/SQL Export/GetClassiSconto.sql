/* Da usare per il debug in management studio
DECLARE @ditta varchar(200)
SELECT @ditta = 'SONN'
*/
SELECT
  tb_codcscl, 
  tb_descscl
FROM 
  tabcscl 
WHERE 1=1 
and codditt = @ditta
and tb_descscl IS NOT null
