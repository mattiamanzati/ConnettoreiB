/* Da usare per il debug in management studio
DECLARE @release varchar(200)
SELECT @release = '1.0'
*/

SELECT 
   'Tracciati' as descr , 
   @release as val, 
   getdate() as xx_ultagg
UNION
SELECT 'Rivenditore', '', getdate()
UNION
SELECT 'Clienti' , '', getdate()
UNION 
SELECT 'Release', CAST( MAX(rel_maior) + '-' + MAX(rel_minor) AS VARCHAR), getdate() from release
UNION
SELECT 'Estrazione', CAST( REPLACE(CONVERT(VARCHAR(10), GETDATE(), 103), '/', '')  + '-' + right('0' + cast(DATEPART(hour, getdate()) as varchar) ,2) + right('0' + cast(DATEPART(minute, getdate()) as varchar) ,2) as varchar), getdate()