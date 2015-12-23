-- Da usare per il debug in management studio
--DECLARE @ditta varchar(200), @tipocf varchar(2)
--SELECT @ditta = 'CMATIC', @tipocf = 'CF'


SELECT  
       leads.codditt + '§0§' + CAST(leads.le_codlead AS VARCHAR) AS xx_chiave,
       leads.le_codlead,
	   0 as xx_progressivo,
       'Note generiche' as xx_codnote, 
       leads.le_note2 as xx_desnote,
	   leads.le_ultagg as xx_ultagg
FROM   dbo.leads WITH (NOLOCK)
WHERE  1=1
AND    codditt = @ditta 
AND    ISNULL(cast(le_note as varchar),'') <> '' 
--AND    le_status <> 'I' 
--TNET_MODIMAN: Gloria 24/03/13 chiesto di vedere i Lead con status=I
UNION ALL -- Personalizzazione. Accodo le note che sono associate ai Leads
SELECT leads.codditt + '§' + CAST(tb_codnote AS VARCHAR) + '§' + CAST(le_codlead AS VARCHAR),
       le_codlead, 
       tb_codnote, 
       tb_desnote, 
	   tb_testonot as le_note2,
	   leads.le_ultagg as xx_ulagg
FROM   leads WITH (NOLOCK)
       INNER JOIN tabnote WITH (NOLOCK)
              ON leads.codditt = tabnote.codditt 
                 AND leads.le_codlead = tb_contonot 
--				 AND leads.le_codlead < 1010001
WHERE  leads.codditt = @ditta
AND NOT tb_testonot IS NULL 
AND NOT tb_desnote IS NULL
ORDER  BY 
	le_codlead