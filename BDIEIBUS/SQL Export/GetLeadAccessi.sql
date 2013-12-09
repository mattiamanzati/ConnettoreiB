-- Da usare per il debug in management studio
--DECLARE @ditta varchar(50)
--SELECT @ditta = 'CMATIC'

SELECT 
    CAST(opcr_codlead AS VARCHAR) + '§' + codditt + '§' + opcr_opnome as xx_chiave,
	opcr_opnome, 
    opcr_codlead, 
    case opcr_crmvis when 'S' then '-1' else '0' end as opcr_crmvis, 
    case opcr_crmmod when 'S' then '-1' else '0' end as opcr_crmmod,
	'01011900000000' as xx_ultagg
FROM
	dbo.acclead WITH (NOLOCK)
WHERE  1=1
	AND codditt = @ditta 


	