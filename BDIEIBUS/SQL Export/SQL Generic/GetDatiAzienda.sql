/* Da usare per il debug in management studio
DECLARE @release varchar(200)
SELECT @release = '1.0'
*/

SELECT 
  tb_azcodpcon,
  case tb_struttura
	when 'S' then '4'
	when 'A' then '5'
	when 'B' then '6'
	when 'C' then '6'
	when 'D' then '5'
  end as xx_num_cifre
FROM 
  tabanaz WITH (NOLOCK), 
  tabpcon WITH (NOLOCK)
WHERE  
  tabanaz.tb_azcodpcon = tabpcon.tb_codpcon
  AND tabanaz.codditt = @ditta
