-- Da usare per il debug in management studio
--DECLARE @ditta varchar(200)
--SELECT @ditta = 'CMATIC'


SELECT
     leads.codditt + '§' + cast(le_codlead as varchar) + '§' + cast(le_coddest as varchar) as xx_chiave
	,le_codlead
--	 le_hhcampagna_origine
	 ,tabcamp.tb_descamp AS xx_hhcampagna_origine
	,le_hhcodcana
	,le_hhcodese
	,tabpaga.tb_despaga as xx_hhcodpaga
	,le_hhcodtpbf
	,le_hhcondizioni_concorrente
	,tb_desport
	,tb_destpbf
	,tabciva.tb_desciva as xx_desciva
	,CAST(le_hhdtaper AS DATE) AS le_hhdtaper
	,CASE le_hhfascia_mercato 
--		WHEN 0 THEN ' ' 
		WHEN 1 THEN 'Produzione'
		WHEN 2 THEN 'Rivendita'
		WHEN 3 THEN 'Utente finale'
	 END AS le_hhfascia_mercato
	,le_hhfatturato_cmatic
	,le_hhfatturato_raccordi
	,CASE le_hhfonte 
--		WHEN 0 THEN ' ' 
		WHEN 1 THEN 'Annuncio pubblicitario'
		WHEN 2 THEN 'Segnalazione dipendente'
		WHEN 3 THEN 'Segnalazione esterna'
		WHEN 4 THEN 'Parner'
		WHEN 5 THEN 'Relazione pubbliche'
		WHEN 6 THEN 'Seminario'
		WHEN 7 THEN 'Fiera'
		WHEN 8 THEN 'Web'
		WHEN 9 THEN 'Passaparola'
		WHEN 10 THEN 'Altro'
	 END AS le_hhfonte
	,CAST(le_hhin_anagrafica AS DATE) AS le_hhin_anagrafica
	,CAST(le_hhin_potenziale AS DATE) AS le_hhin_potenziale
	,CASE le_hhlivello_interesse 
--		WHEN 0 THEN ' ' 
		WHEN 1 THEN 'Alto'
		WHEN 2 THEN 'Medio'
		WHEN 3 THEN 'Basso'
	 END AS le_hhlivello_interesse
	,le_hhporto
	,CASE le_hhsettore 
--		WHEN 0 THEN ' ' 
		WHEN 1 THEN 'Agricoltura,caccia, pesca e silvicoltura'
		WHEN 2 THEN 'Estrazione di minerali enegertici e non energetici'
		WHEN 3 THEN 'Industrie alimentari, delle bevande e del tabacco'
		WHEN 4 THEN 'Industrie tessili e dell''abbigliamento'
		WHEN 5 THEN 'Fabbricazione di prodotti chimici'
		WHEN 6 THEN 'Fabbricazione di prodotti farmaceutici e cosmetici'
		WHEN 7 THEN 'Fabbricazione di articoli in gomma e plastica'
		WHEN 8 THEN 'Fabbricazione macchine e apparecchi meccanici'
		WHEN 9 THEN 'Fabbricazione macchine per ufficio e sistemi informatici'
		WHEN 10 THEN 'Fabbricazione macchine e apparecchi elettrici'
		WHEN 11 THEN 'Fabbricazione apparecchi di precisione'
		WHEN 12 THEN 'Fabbricazione di mezzi di trasporto'
		WHEN 13 THEN 'Altre industrie manifatturiere'
		WHEN 14 THEN 'Prod. e distrib. di energia elettrica, gas e acqua'
		WHEN 15 THEN 'Edilizia'
		WHEN 16 THEN 'Commercio all''ingrosso, al dettaglio, riparazioni'
		WHEN 17 THEN 'Alberghi, ristoranti e pubblici esercizi'
		WHEN 18 THEN 'Trasporti, magazzinaggio'
		WHEN 19 THEN 'Poste e telecomunicazioni'
		WHEN 20 THEN 'Editoria e stampa'
		WHEN 21 THEN 'Intermediazione monetaria e finanziaria'
		WHEN 22 THEN 'Attività delle banche centrali'
		WHEN 23 THEN 'Attività legali e contabili'
		WHEN 24 THEN 'Consulenza amministrativo-gestionale'
		WHEN 25 THEN 'Pubblicità'
		WHEN 26 THEN 'Fabbricazione macchine e apparecchi meccanici'
		WHEN 27 THEN 'Fabbricazione macchine per ufficio e sistemi informatici'
		WHEN 28 THEN 'Fabbricazione macchine e apparecchi elettrici'
		WHEN 29 THEN 'Fabbricazione apparecchi di precisione'
		WHEN 30 THEN 'Fabbricazione di mezzi di trasporto'
		WHEN 31 THEN 'Trasporti, magazzinaggio'
	 END AS le_hhsettore,
	 getdate() as xx_ultagg
FROM leads  WITH (NOLOCK)
	  LEFT JOIN tabcamp  WITH (NOLOCK)
 			   ON leads.codditt = tabcamp.codditt 
			  AND leads.le_hhcampagna_origine = tabcamp.tb_codcamp
	  LEFT JOIN tabpaga  WITH (NOLOCK)
	           ON tabpaga.tb_codpaga = leads.le_hhcodpaga
	  LEFT JOIN tabport  WITH (NOLOCK)
	           ON tabport.tb_codport = leads.le_hhporto
	  LEFT JOIN tabciva  WITH (NOLOCK)
	           ON tabciva.tb_codciva = leads.le_hhcodese
	  LEFT JOIN tabtpbf  WITH (NOLOCK)
	           ON tabtpbf.tb_codtpbf = leads.le_hhcodtpbf
WHERE 1=1
	AND leads.codditt = @ditta
    --AND le_status <> 'I' 
    --TNET_MODIMAN: Gloria 24/03/13 chiesto di vedere i Lead con status=I
    AND (le_conto = '0' )
	AND le_coddest = 0
ORDER  BY 
    le_codlead
