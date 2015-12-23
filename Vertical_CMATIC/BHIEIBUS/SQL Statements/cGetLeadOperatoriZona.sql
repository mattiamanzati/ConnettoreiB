-- Da usare per il debug in management studio
--DECLARE @ditta varchar(200)
--SELECT @ditta = 'CMATIC'

-- Recupera la lista degli operatori associati ai leads in base alla tabella associativa delle zone

SELECT DISTINCT 
  dbo.leads.codditt + '§' + cast(hh_opnome as varchar)  + '§' + CAST(le_codlead AS VARCHAR) as xx_chiave,
  HH_OperZone.hh_opnome AS xx_codoper,
  le_codlead AS xx_cod_lead
FROM dbo.leads 
   INNER JOIN dbo.HH_OperZone ON 
       (dbo.HH_OperZone.hh_codzona = dbo.leads.le_zona)
WHERE 1 = 1
AND dbo.leads.codditt = @ditta
except
SELECT
   dbo.acclead.codditt + '§' + cast(opcr_opnome as varchar)  + '§' + CAST(acclead.opcr_codlead AS VARCHAR),
   opcr_opnome, 
   acclead.opcr_codlead 
FROM acclead
WHERE 1 = 1
AND dbo.acclead.codditt = @ditta
