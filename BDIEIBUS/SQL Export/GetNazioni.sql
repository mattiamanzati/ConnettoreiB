/* Da usare per il debug in management studio
DECLARE @release varchar(200)
SELECT @release = '1.0'
*/

SELECT 
	tb_codstat, 
	rtrim(ltrim(tb_desstat)) as tb_desstat
FROM 
	dbo.tabstat WITH (NOLOCK)
 ORDER BY 
	tb_desstat