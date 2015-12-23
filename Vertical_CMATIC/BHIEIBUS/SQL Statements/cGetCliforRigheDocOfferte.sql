
SELECT
        11  as query, 
        mo.mo_codart AS xx_codart,
        an.an_tipo, 
        an.an_conto,  
        upper(td.codditt + '§' + CAST(td.td_conto AS VARCHAR)+ '§' + CAST(td.td_anno as varchar) + '§' + td.td_serie + '§' + CAST(td.td_numord as varchar)) + '§' + td.td_tipork AS xx_numreg,
        td.td_tipork as tm_tipork,  
        td.td_anno as tm_anno,  
        td.td_serie as tm_serie,   
        td.td_numord as tm_numdoc,  
        '0' as tm_tipork1,   
        '0' as tm_serie1,  
        0 as tm_numdoc1,  
        right('00000' + CAST(mo.mo_riga AS NVARCHAR(50)), 4) AS mm_riga,  
        mo.mo_codart as mm_codart,  
        mo.mo_fase as mm_fase,  
        mo.mo_descr as mm_descr,  
        mo.mo_desint as mm_desint,  
        mo.mo_unmis as mm_unmis,  
        mo.mo_ump as mm_ump,  
        mo.mo_quant as mm_quant,  
        mo.mo_colli as mm_colli,    
        -- Calcolo del prezzo in valuta 
        --CASE    WHEN td_valuta=0    THEN    mo_valore 
        --        ELSE                        mo_valorev   SELECT CAST(field1 AS DECIMAL(10,2))
        -- END as mm_valore, 
		                   cast (
						       (
                                  --Qta
                                  CASE WHEN mo_umprz <> 'S' Then mo_quant Else mo_colli END
                                  --Prezzo
                                  *
                                  CASE WHEN td.td_valuta <> 0  THEN mo_prezvalc  ELSE 
                                        CASE WHEN td.td_scorpo = 'S' THEN mo_preziva    ELSE       mo_prezzo 
                                        END
                                  END
                                  --     Sconti
                                  *
                                  (100-mo_scont1)/100*(100-mo_scont2)/100*(100-mo_scont3)/100*(100-mo_scont4)/100*(100-mo_scont5)/100*(100-mo_scont6)/100
                                  --*(100-mo_scontp)/100-mo_scontv  sconti ulteriori presenti su MovMag / MovOrd (nella 2015 saranno presenti)
                                  -- Molt_Qta
                                  / mo_perqta
                           ) 
						   as decimal(16,6))as mm_valore,
        mo.mo_scont1 as mm_scont1, 
        mo.mo_scont2 as mm_scont2, 
        mo.mo_scont3 as mm_scont3,  
        -- CASE    WHEN mo_quant=0 THEN 0 
        --      ELSE     
        --         CASE    WHEN  td_valuta=0   THEN    Round( (mo_valore / mo_quant) * mo_perqta , 4)       
        --                 ELSE  Round( (mo_valorev / mo_quant) * mo_perqta , 4)         
        --         END 
        -- END AS xx_prezzo, 
		CASE	WHEN td_valuta=0  THEN
				(	mo_prezzo 
					* (100-mo_scont1)/100*(100-mo_scont2)/100*(100-mo_scont3)/100*(100-mo_scont4)/100*(100-mo_scont5)/100*(100-mo_scont6)/100 
				) 
		ELSE	(	mo_prezvalc
					* (100-mo_scont1)/100*(100-mo_scont2)/100*(100-mo_scont3)/100*(100-mo_scont4)/100*(100-mo_scont5)/100*(100-mo_scont6)/100 
				) 
		END AS xx_prezzo,
        td.td_valuta as tm_valuta,   
        mo.mo_stasino as mm_stasino, 
        td.td_ultagg as xx_ultagg  
FROM testoff as td WITH (NOLOCK) 
    INNER JOIN movoff as mo WITH (NOLOCK)   
       ON mo.codditt = td.codditt AND mo.mo_tipork = td.td_tipork AND mo.mo_anno = td.td_anno AND mo.mo_serie = td.td_serie AND mo.mo_numord = td.td_numord and mo.mo_vers=td.td_vers   
    INNER JOIN anagra as an WITH (NOLOCK)   
       ON td.codditt = an.codditt AND td.td_conto = an.an_conto   
    INNER JOIN (
	             select codditt,td_tipork,td_anno,td_numord,td_serie,MAX(td_vers) td_vers from testoff   
                 group by codditt,td_tipork,td_anno,td_numord,td_serie
			   ) MX   
       ON MX.codditt = td.codditt AND MX.td_tipork = td.td_tipork AND MX.td_anno = td.td_anno AND MX.td_serie = td.td_serie AND MX.td_numord = td.td_numord and MX.td_vers=td.td_vers 
WHERE  (1 = 1) 
        AND td.td_tipork = '!' 
        AND an.codditt =  @ditta
		AND (GETDATE() - td.td_datord ) < cast(@gg_offerte as integer) 
		-- test AND  mo_anno = 2014 AND  mo_numord = 247-- and mo_codart = '2V010052'
        