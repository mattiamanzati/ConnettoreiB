-- DECLARE @ditta varchar(200), @gg_ultacqven VARCHAR(19)
-- SELECT @ditta = 'CASADEI', @gg_ultacqven = '180'

-- DECLARE @ditta varchar(200), @gg_ultacqven VARCHAR(19)
-- SELECT @ditta = 'CASADEI', @gg_ultacqven = '180'

-- DECLARE @ditta varchar(200), @gg_ultacqven VARCHAR(19)
-- SELECT @ditta = 'Prs', @gg_ultacqven = '9080'

SELECT	1                                            AS xx_tipo,           
    ar_codart + CASE WHEN ar_gesfasi = 'S' THEN '.' + Cast(af_fase AS VARCHAR (5)) ELSE '' END AS ar_codart,  
	CONVERT(VARCHAR(20), keymag.km_aammgg, 111)		 AS km_aammgg,
	Cast(( 1000000000 + km_numdoc ) AS VARCHAR(10))	 AS km_numdoc,
	km_tipork                                        AS km_tipork,
	CASE	WHEN testmag.tm_valuta = 0  THEN
					(	
						CASE WHEN mm_quant <> 0 THEN Round(mm_valore / mm_quant, 4) ELSE 0 END	
					) 
			ELSE	(	 
						CASE WHEN mm_quant <> 0 THEN Round(mm_valorev / mm_quant, 4) ELSE 0 END	 
					) 
	END AS mm_val,
	CASE 
	  WHEN mm_colli <> 0 THEN Round(mm_valore / mm_colli, 4)
	  ELSE 0
	END										 	     AS mm_valum2,  
	Cast(km_conto AS VARCHAR(9))					 AS km_conto,
	movmag.mm_unmis                                  AS mm_unmis, 
	movmag.mm_ump                                    AS mm_ump, 
	testmag.tm_valuta                                AS tm_valuta,
    getdate()                                        AS xx_ultagg
FROM keymag WITH (NOLOCK)
     INNER JOIN movmag movmag WITH (NOLOCK)
                 ON keymag.codditt = movmag.codditt 
				AND keymag.km_tipork = movmag.mm_tipork
                AND keymag.km_anno = movmag.mm_anno 
				AND keymag.km_serie = movmag.mm_serie
				AND keymag.km_numdoc = movmag.mm_numdoc
				AND keymag.km_riga = movmag.mm_riga
				AND keymag.km_codart = movmag.mm_codart
	 INNER JOIN testmag testmag WITH (NOLOCK)
				ON movmag.mm_numdoc = testmag.tm_numdoc
				AND movmag.mm_anno = testmag.tm_anno
				AND movmag.mm_serie = testmag.tm_serie 
				AND movmag.mm_tipork = testmag.tm_tipork
				AND movmag.codditt = testmag.codditt
     INNER JOIN tabcaum WITH (NOLOCK)
                ON keymag.km_causale = tabcaum.tb_codcaum  
	 INNER JOIN anagra  WITH (NOLOCK)
			    ON anagra.codditt = keymag.codditt
		       AND anagra.an_conto = keymag.km_conto          
	 LEFT JOIN artfasi   WITH (NOLOCK)                                                                                                                            
            ON artfasi.codditt = keymag.codditt
                AND artfasi.af_codart = keymag.km_codart
     INNER JOIN artico WITH (NOLOCK)
				ON artico.codditt = keymag.codditt 
				and artico.ar_codart = keymag.km_codart
			   AND   CASE WHEN artico.ar_gesfasi = 'S' THEN Cast(artfasi.af_fase AS VARCHAR(5)) ELSE '' END = CASE WHEN artico.ar_gesfasi = 'S' THEN Cast(keymag.km_fase AS VARCHAR(5)) ELSE '' END       				    
WHERE 1=1
  AND km_aammgg in (select MAX(km2.km_aammgg) from keymag as km2 inner join artico  WITH (NOLOCK) 
										on km_codart = ar_codart 
							   INNER JOIN tabcaum WITH (NOLOCK)
										ON km2.km_causale = tabcaum.tb_codcaum  
							WHERE 1=1
								AND km2.km_carscar = 1
								AND artico.codditt =  @ditta
								AND cast((GETDATE() - km2.km_aammgg ) as integer) < cast(@gg_ultacqven as integer)      
								AND ( ar_blocco != 'S')                                 
								AND ( ar_stainv = 'S' OR ar_codart = 'D' )                                    
								AND ar_codtagl = 0                                                            
								AND ( tb_carfor = 1  OR tb_carpro = 1  OR tb_scacli = 1 )                              
								AND ( ar_gesvar <> 'S' OR ( ar_gesvar = 'S' AND ar_codroot <> '' ) )   
								AND km2.km_conto = keymag.km_conto
								AND km2.km_codart = keymag.km_codart
								)
AND km_carscar = 1 
AND artico.codditt =  @ditta
AND cast((GETDATE() - keymag.km_aammgg ) as integer) < cast(@gg_ultacqven as integer)      
AND ( ar_blocco != 'S')                                 
AND ( ar_stainv = 'S' OR ar_codart = 'D' )                                    
AND ar_codtagl = 0                                                            
AND ( tb_carfor = 1  OR tb_carpro = 1  OR tb_scacli = 1 )                     
--AND ( ar_gesvar = 'N' OR ( ar_gesvar = 'S' AND ar_codroot <> '' ) )           
AND ( ar_gesvar <> 'S' OR ( ar_gesvar = 'S' AND ar_codroot <> '' ) )           
--AND mm_valore <> 0     
--AND artico.ar_codart='90384M'
--AND keymag.km_conto = 11010109
--ORDER BY keymag.km_aammgg
