-- Da usare per il debug in management studio
--DECLARE @ditta varchar(50)
--SELECT @ditta = 'CMATIC'

SELECT 
    codditt + '§' + opcr_opnome + '§' + opcr_alopnome as xx_chiave,
	opcr_opnome, 
    opcr_alopnome, 
    case opcr_crmvis when 'S' then '-1' else '0' end as opcr_crmvis, 
    case opcr_crmmod when 'S' then '-1' else '0' end as opcr_crmmod,
	'01011900000000' as xx_ultagg
FROM
	dbo.acccrm WITH (NOLOCK)
WHERE  1=1
	AND codditt = @ditta 