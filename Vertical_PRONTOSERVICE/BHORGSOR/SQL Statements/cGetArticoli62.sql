-- Da usare per il debug in management studio
--DECLARE @ditta varchar(200)
--SELECT @ditta = 'Prs'

SELECT distinct 
   ar62_codart as ar62_codart,
   tb_tipopaga as tb_tipopaga
FROM 
  dbo.XM_ABBINAART62 WITH (NOLOCK)
       INNER JOIN dbo.XM_CATEGORIEART62 
	       ON dbo.XM_CATEGORIEART62.tb_codca62 = dbo.XM_ABBINAART62.ar62_codca62
WHERE 1=1
   AND tb_tipopaga= 'S'
