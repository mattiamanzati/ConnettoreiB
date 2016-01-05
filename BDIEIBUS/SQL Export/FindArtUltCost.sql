-- DECLARE @ditta varchar(200), @gg_ultacqven VARCHAR(19)
-- SELECT @ditta = 'CASADEI', @gg_ultacqven = '180'

-- DECLARE @ditta varchar(200), @gg_ultacqven VARCHAR(19)
-- SELECT @ditta = 'CASADEI', @gg_ultacqven = '180'

SELECT TOP 1 
   apx_ultcos as ultcos
FROM 
   dbo.artprox 
WHERE 1=1
and apx_codart = @codart
and codditt =  @ditta