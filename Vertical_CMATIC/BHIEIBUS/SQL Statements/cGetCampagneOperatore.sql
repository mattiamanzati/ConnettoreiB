-- Da usare per il debug in management studio
--DECLARE @ditta varchar(200)
--SELECT @ditta = 'CMATIC'

SELECT
  codditt + '§' + cast(hh_codcamp as varchar)  + '§' + hh_opnome as xx_chiave,
  hh_codcamp AS xx_cod_campagna, 
  hh_opnome AS xx_cod_operatore
FROM 
  [HH_OperCamp]
WHERE 1=1 
and codditt = @ditta
