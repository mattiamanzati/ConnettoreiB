/*
DECLARE @release varchar(200), @ditta varchar(50)
SELECT @release = '1.0', @ditta = 'GIOCHOTEL'
*/

SELECT DISTINCT an_conto, xx_agente,  tb_descage, xx_ultagg, an_tipo, MAX(xx_prefer) as xx_prefer, MAX(xx_chiave) as xx_chiave
FROM 
(

-- TNET_MODIMAN
--	query modificata per utente DARREN - implementato con Zona/Agente	Zona: 28 Usa -> Agente: 13 Darren

SELECT 
       --'1§' + anagra.codditt + '§' + anagra.an_tipo + '§' + cast(an_conto as varchar) + '§' + cast(an_agente as varchar) + '§' + 'A' as xx_chiave,
	   '1§' + anagra.codditt + '§' + anagra.an_tipo + '§' + cast(an_conto as varchar) + '§' + 
			--cast(an_agente as varchar) 
			CASE	WHEN  an_zona = @ZONAGE_ZONCOD  THEN  cast(@ZONAGE_AGECOD as varchar) 
					ELSE	cast(an_agente as varchar) 
			END 
			+ '§' + 'A' as xx_chiave,

       an_tipo    AS an_tipo, 
       an_conto   AS an_conto, 

			--	an_agente  AS xx_agente, 
			CASE	WHEN	an_zona = @ZONAGE_ZONCOD  THEN  @ZONAGE_AGECOD
					ELSE	an_agente  
			END AS xx_agente, 
		
			--tb_descage AS tb_descage, 
			CASE	WHEN	an_zona = @ZONAGE_ZONCOD  THEN  @ZONAGE_AGEDES
					ELSE	tb_descage 
			END AS tb_descage, 

       1          AS xx_prefer,
	   getdate()  AS xx_ultagg
FROM   tabcage WITH (NOLOCK)
       INNER JOIN anagra WITH (NOLOCK)
               ON tabcage.codditt = anagra.codditt 
                  AND tabcage.tb_codcage = anagra.an_agente 
WHERE  1=1
       AND anagra.codditt = @ditta 
       AND an_tipo <> 'S' 
       AND an_status = 'A' 

UNION 
SELECT 
       '2§' + anagra.codditt + '§' + anagra.an_tipo + '§' + cast(an_conto as varchar) + '§' + cast(an_agente2 as varchar) + '§' + 'B',
       an_tipo, 
       an_conto, 
       an_agente2, 
       tb_descage, 
       0          ,
	   getdate()  
FROM   tabcage WITH (NOLOCK)
       INNER JOIN anagra WITH (NOLOCK)
               ON tabcage.codditt = anagra.codditt 
                  AND tabcage.tb_codcage = anagra.an_agente2 
WHERE  1=1
       AND anagra.codditt = @ditta 
       AND an_tipo <> 'S' 
       AND an_status = 'A' 
UNION 
SELECT 
       --'3§' + anagra.codditt + '§' + anagra.an_tipo + '§' + cast(anagra.an_conto as varchar) + '§' + cast(an_agente as varchar) + '§' + cast(destdiv.dd_coddest as varchar),
       '3§' + anagra.codditt + '§' + anagra.an_tipo + '§' + cast(anagra.an_conto as varchar) + '§' +

			--cast(an_agente as varchar) 
			CASE	WHEN  an_zona = @ZONAGE_ZONCOD  THEN  cast(@ZONAGE_AGECOD as varchar) 
					ELSE	cast(an_agente as varchar) 
			END 
			+ '§' + cast(destdiv.dd_coddest as varchar),
			
       anagra.an_tipo, 
       anagra.an_conto, 
       
       --tabcage.tb_codcage, 
       --tabcage.tb_descage, 
			--	an_agente  AS xx_agente, 
			CASE	WHEN	an_zona = @ZONAGE_ZONCOD  THEN  @ZONAGE_AGECOD
					ELSE	tabcage.tb_codcage  
			END AS tb_codcage , 
		
			--tb_descage AS tb_descage, 
			CASE	WHEN	an_zona = @ZONAGE_ZONCOD  THEN  @ZONAGE_AGEDES
					ELSE	tabcage.tb_descage 
			END AS tb_descage , 
       
       0         ,
	   getdate()  
FROM   tabcage WITH (NOLOCK)
       INNER JOIN dbo.destdiv WITH (NOLOCK)
               ON tabcage.codditt = destdiv.codditt 
              AND tabcage.tb_codcage = destdiv.dd_agente 
	   INNER JOIN anagra WITH (NOLOCK)
	            ON anagra.codditt = destdiv.codditt 
                AND anagra.an_conto = destdiv.dd_conto 
WHERE  1=1
       AND destdiv.codditt = @ditta 
       AND an_tipo <> 'S' 
       AND an_status = 'A'
UNION 
SELECT 
       '4§' + anagra.codditt + '§' + anagra.an_tipo + '§' + cast(anagra.an_conto as varchar) + '§' + cast(anagra.an_agente2 as varchar) + '§' + cast(destdiv.dd_coddest as varchar),
       anagra.an_tipo, 
       anagra.an_conto, 
       tabcage.tb_codcage, 
       tabcage.tb_descage, 
       0        ,
	   getdate()
FROM   tabcage WITH (NOLOCK)
       INNER JOIN dbo.destdiv WITH (NOLOCK)
               ON tabcage.codditt = destdiv.codditt 
              AND tabcage.tb_codcage = destdiv.dd_agente2 
	   INNER JOIN anagra WITH (NOLOCK)
	            ON anagra.codditt = destdiv.codditt 
                AND anagra.an_conto = destdiv.dd_conto 
WHERE  1=1
       AND destdiv.codditt = @ditta 
       AND an_tipo <> 'S' 
       AND an_status = 'A'
UNION -- Se esistono clienti template metto in cross join la tabella clienti con gli agenti per ottenere tutte le combinazioni possibili
SELECT 
	   '5§' + anagra.codditt + '§' + anagra.an_tipo + '§' + cast(an_conto as varchar) + '§' + cast( tabcage.tb_codcage as varchar) + '§' + 'A' as xx_chiave,
       anagra.an_tipo, 
       anagra.an_conto, 
       tabcage.tb_codcage, 
       tabcage.tb_descage, 
       0        ,
	   getdate()
FROM   anagra WITH (NOLOCK), tabcage
WHERE  1=1
       AND tabcage.codditt = @ditta 
	   AND anagra.codditt =  @ditta 
	   AND anagra.an_pariva = '99999999999'
	   AND anagra.an_agente= 0
	   AND anagra.an_agente2 = 0
) AS agenti
GROUP BY an_conto, xx_agente, tb_descage, xx_ultagg, an_tipo