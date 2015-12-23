﻿/* Da usare per il debug in management studio
DECLARE @ditta varchar(200), @giornistorico integer
SELECT @ditta = 'SONN', @giornistorico=180
versione 2
*/

SELECT                                                                                                         
    ar_codart + CASE WHEN ar_gesfasi = 'S' THEN '.' + Cast(af_fase AS VARCHAR (5)) ELSE '' END  AS ar_codart,    
    td_conto,                                                                                                    
    'O§' + CAST(mo_numord AS VARCHAR) + '§' + CAST(mo_anno AS VARCHAR) +'§' +  mo_serie + '§' +  mo_tipork + '§' + CAST(mo_riga AS VARCHAR) AS xx_code, 
    mo_descr AS mo_descr,                                                                                   
    mo_anno AS mo_anno,                                                                                     
    mo_serie AS mo_serie,                                                                                   
    mo_tipork AS mo_tipork,                                                                                  
    mo_riga AS mo_riga,                                                                                     
    mo_prezzo AS mo_prezzo,                                                                                 
    mo_ump AS mo_ump,                                                                                       
    mo_numord AS mo_numord,                                                                                 
    mo_quant AS mo_quant,                                                                                   
    td_coddest AS td_coddest,                                                                               
    td_datord AS td_datord,                                                                                 
    mo_scont1 AS mo_scont1,                                                                                 
    mo_scont2 AS mo_scont2,                                                                                 
    mo_scont3 AS mo_scont3,                                                                                 
    mo_scont4 AS mo_scont4,                                                                                 
    mo_scont5 AS mo_scont5,                                                                                 
    mo_scont6 AS mo_scont6,                                                                                 
    mo_ultagg AS mo_ultagg                                                                                    
    FROM   artico WITH (NOLOCK)                                                                                                 
            LEFT JOIN artfasi WITH (NOLOCK)                                                                                      
                    ON artico.codditt = artfasi.codditt                                                              
                    AND artico.ar_codart = artfasi.af_codart                                                      
            INNER JOIN movord WITH (NOLOCK)                                                                                      
                    ON artico.codditt = movord.codditt                                                              
              		    AND artico.ar_codart = movord.mo_codart 
                    AND CASE WHEN artico.ar_gesfasi = 'S' THEN Cast(artfasi.af_fase AS VARCHAR(5)) ELSE '' END =  CASE WHEN artico.ar_gesfasi = 'S' THEN  Cast(movord.mo_fase AS VARCHAR(5)) ELSE '' END  
            INNER JOIN testord WITH (NOLOCK)                                                                                     
                    ON testord.codditt = movord.codditt                                                             
                        AND testord.td_tipork = movord.mo_tipork                                                     
                        AND testord.td_anno = movord.mo_anno                                                         
                        AND testord.td_serie = movord.mo_serie                                                       
                        AND ( Getdate() - td_datord ) < Cast(@giornistorico as integer) 
                        AND testord.td_numord = movord.mo_numord   
			INNER JOIN anagra  WITH (NOLOCK)
				    ON anagra.codditt = testord.codditt
					    AND anagra.an_conto = td_conto
    WHERE  1=1                                                                                                     
            AND artico.codditt =  @ditta
            AND ( ar_stainv = 'S' OR ar_codart = 'D' )                                                              
            AND ar_codtagl = 0                                                                                      
            AND td_tipork IN ('Q', 'R', 'O' )                                                             
            --AND ( ar_gesvar = 'N'  OR ( ar_gesvar = 'S'  AND ar_codroot <> '' ) )
			AND ( ar_gesvar <> 'S'  OR ( ar_gesvar = 'S'  AND ar_codroot <> '' ) )
			AND (an_tipo = 'C')
			--AND an_conto IN ('10010875') AND  ar_codart IN ('91339L')
UNION                                                                                     
SELECT                                                                                                         
    ar_codart + CASE WHEN ar_gesfasi = 'S' THEN '.' + Cast(af_fase AS VARCHAR (5)) ELSE '' END  AS ar_codart,    
    tm_conto,                                                                                                    
    'F§' + CAST(mm_numdoc AS VARCHAR) + '§' + CAST(mm_anno AS VARCHAR) +'§' +  mm_serie + '§' +  mm_tipork + '§' + CAST(mm_riga AS VARCHAR) AS xx_code, 
    mm_descr AS mm_descr,                                                                                   
    mm_anno AS mm_anno,                                                                                     
    mm_serie AS mm_serie,                                                                                   
    mm_tipork AS mm_serie,                                                                                  
    mm_riga AS mm_riga,                                                                                     
    mm_prezzo AS mm_prezzo,                                                                                 
    mm_ump AS mm_ump,                                                                                       
    mm_numdoc AS mm_numord,                                                                                 
    mm_quant AS mm_quant,                                                                                   
    tm_coddest AS tm_coddest,                                                                               
    tm_datdoc AS tm_datord,                                                                                 
    mm_scont1 AS mm_scont1,                                                                                 
    mm_scont2 AS mm_scont2,                                                                                 
    mm_scont3 AS mm_scont3,                                                                                 
    mm_scont4 AS mm_scont4,                                                                                 
    mm_scont5 AS mm_scont5,                                                                                 
    mm_scont6 AS mm_scont6,                                                                                 
    mm_ultagg AS mm_ultagg                                                                                    
    FROM   artico WITH (NOLOCK)                                                                                                 
            LEFT JOIN artfasi WITH (NOLOCK)                                                                                      
                    ON artico.codditt = artfasi.codditt                                                              
                    AND artico.ar_codart = artfasi.af_codart                                                      
            INNER JOIN movmag WITH (NOLOCK)                                                                                      
                    ON artico.codditt = movmag.codditt                                                              
              		    AND artico.ar_codart = movmag.mm_codart 
                    AND CASE WHEN artico.ar_gesfasi = 'S' THEN Cast(artfasi.af_fase AS VARCHAR(5)) ELSE '' END =  CASE WHEN artico.ar_gesfasi = 'S' THEN  Cast(movmag.mm_fase AS VARCHAR(5)) ELSE '' END  
            INNER JOIN testmag WITH (NOLOCK)                                                                                     
                    ON testmag.codditt = movmag.codditt                                                             
                        AND testmag.tm_tipork = movmag.mm_tipork                                                     
                        AND testmag.tm_anno = movmag.mm_anno
                        AND testmag.tm_serie = movmag.mm_serie                                                       
                        AND ( Getdate() - tm_datdoc ) < Cast(@giornistorico as integer) 
                        AND testmag.tm_numdoc = movmag.mm_numdoc   
			INNER JOIN anagra  WITH (NOLOCK)
				    ON anagra.codditt = testmag.codditt
					    AND anagra.an_conto = tm_conto
    WHERE  1=1                                                                                                     
            AND artico.codditt =  @ditta
            AND ( ar_stainv = 'S' OR ar_codart = 'D' )                                                              
            AND ar_codtagl = 0       
		    AND an_tipo <> 'S'                                                
            AND an_status = 'A'                                                                                   
            --AND ( ar_gesvar = 'N'  OR ( ar_gesvar = 'S'  AND ar_codroot <> '' ) )  
			AND ( ar_gesvar <> 'S'  OR ( ar_gesvar = 'S'  AND ar_codroot <> '' ) )  
			AND tm_tipork in ('A','B','D')
			AND (an_tipo = 'C')
