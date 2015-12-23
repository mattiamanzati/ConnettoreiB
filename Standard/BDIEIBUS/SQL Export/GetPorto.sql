/* Da usare per il debug in management studio
DECLARE @ditta varchar(200)
SELECT @ditta = 'SONN'
*/
SELECT
  tb_codport, 
  tb_desport
FROM 
  tabport 
WHERE 1=1 
and codditt = @ditta
and tb_desport IS NOT null
