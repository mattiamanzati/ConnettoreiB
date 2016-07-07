/* Da usare per il debug in management studio
DECLARE @release varchar(200)
SELECT @release = '1.0'
*/

SELECT 
  OpPasswd as xx_passwd
FROM 
  OPERAT  
WHERE opnome = @opnome