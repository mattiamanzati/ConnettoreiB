-- Da usare per il debug in management studio
--DECLARE @ditta varchar(50)
--SELECT @ditta = 'CMATIC'

SELECT  
        movoff.codditt + '§' +cast(movoff.mo_numord as varchar) + '§' + movoff.mo_tipork + '§' + cast(movoff.mo_anno as varchar) + '§' + movoff.mo_serie + '§' + cast(movoff.mo_vers as varchar) + '§' + cast(movoff.mo_riga AS varchar) as xx_chiave,
		CAST(movoff.mo_numord AS VARCHAR) + '§' + CAST(movoff.mo_anno AS VARCHAR) + '§' +  movoff.mo_serie + '§' +  movoff.mo_tipork + '§' +  CAST(movoff.mo_vers AS VARCHAR) + '§' + movoff.codditt as xx_chiave_testata,
        td_codlead,                                                         
        mo_tipork,                                                
        td_anno,                                                  
        td_serie,                                                 
        td_numord,                                                
        mo_riga,                                                          
        mo_codart,                                                        
        mo_fase,                                                          
        mo_descr,                                                         
        mo_desint,                                                        
        mo_unmis,                                                         
        mo_ump,                                                           
        mo_quant,                                                         
        mo_colli,                                                         
        mo_valore,   
		td_datconf,
		mo_scont1, 
		mo_scont2, 
		mo_scont3, 
		mo_scont4, 
		mo_scont5, 
		mo_scont6,
        CASE                                                              
            WHEN mo_quant <> 0 THEN Round(mo_valore / mo_quant, 4)          
            ELSE 0                                                          
        END AS xx_prezzo,
		mo_ultagg AS xx_ultagg
    FROM   testoff WITH (NOLOCK)                                            
        INNER JOIN movoff WITH (NOLOCK)                                 
                ON movoff.codditt = testoff.codditt                            
                AND movoff.mo_tipork = testoff.td_tipork
                AND movoff.mo_anno = testoff.td_anno                           
                AND movoff.mo_serie = testoff.td_serie                         
                AND movoff.mo_numord = testoff.td_numord  
				AND movoff.mo_vers = testoff.td_vers
        INNER JOIN dbo.leads WITH (NOLOCK)                                   
                ON testoff.codditt = dbo.leads.codditt                       
                AND testoff.td_codlead = leads.le_codlead
    WHERE  dbo.leads.codditt =  @ditta
	    AND (GETDATE() - testoff.td_datord ) < cast(@gg_offerte as integer) 
        AND leads.le_status <> 'I'  
        AND (le_conto = '0' or @includi_lead_clienti <> '0')                                           
        AND testoff.td_tipork = '!'
