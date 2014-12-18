/* Da usare per il debug in management studio
DECLARE @release varchar(200)
SELECT @release = '1.0'
*/
-- strDittaCorrente & "§" & ConvStr(dtrT!an_conto) & "§" & ConvStr(dtrT!xx_agente) & "§" & ConvStr(dtrT!xx_prefer) & "|" & _

SELECT DISTINCT an_conto, xx_agente,  tb_descage, xx_ultagg, an_tipo, MAX(xx_prefer) as xx_prefer, MAX(xx_chiave) as xx_chiave
FROM 
(
SELECT 
       '1§' + anagra.codditt + '§' + anagra.an_tipo + '§' + cast(an_conto as varchar) + '§' + cast(an_agente as varchar) + '§' + 'A' as xx_chiave,
       an_tipo    AS an_tipo, 
       an_conto   AS an_conto, 
       an_agente  AS xx_agente, 
       tb_descage AS tb_descage, 
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
       '3§' + anagra.codditt + '§' + anagra.an_tipo + '§' + cast(anagra.an_conto as varchar) + '§' + cast(an_agente as varchar) + '§' + cast(destdiv.dd_coddest as varchar),
       anagra.an_tipo, 
       anagra.an_conto, 
       tabcage.tb_codcage, 
       tabcage.tb_descage, 
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
) AS agenti
GROUP BY an_conto, xx_agente, tb_descage, xx_ultagg, an_tipo