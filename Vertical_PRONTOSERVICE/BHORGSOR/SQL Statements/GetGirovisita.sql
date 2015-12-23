-- Da usare per il debug in management studio
--DECLARE @ditta varchar(200)
--SELECT @ditta = 'Prs'

SELECT
	CAST(gi_agente AS VARCHAR) + '§' + CAST(gi_conto AS VARCHAR) + '§' + CAST(gi_destin AS VARCHAR) + '§' + CAST(gi_giorno AS VARCHAR) AS xx_chiave, 
	gi_agente AS gi_agente,
	gi_conto AS gi_conto,
	CASE gi_destin WHEN 0 THEN NULL	ELSE gi_destin END	AS gi_destin,
    gi_giorno AS gi_giorno,
	gi_sequenza AS gi_sequenza,
	MAX(td_datord) AS td_datord,
	DATEPART(weekday, MAX(td_datord))  AS xx_weekday_datord,
	getdate() AS xx_ultagg
FROM XM_GIRI WITH (NOLOCK)
     LEFT JOIN dbo.testord WITH (NOLOCK) 
			ON testord.codditt = @ditta
	        AND testord.td_conto = XM_GIRI.gi_conto
	        AND testord.td_coddest = XM_GIRI.gi_destin
			AND testord.td_codagen = XM_GIRI.gi_agente
	INNER JOIN anagra  WITH (NOLOCK) 
	   ON anagra.codditt = @ditta
	   AND anagra.an_conto = gi_conto
	   AND anagra.an_agente = gi_agente
WHERE 1=1
       AND an_tipo <> 'S' 
       AND an_status = 'A' 
	   AND anagra.an_agente <> ''
GROUP BY  
	gi_agente, gi_conto, gi_destin, gi_giorno, gi_sequenza
