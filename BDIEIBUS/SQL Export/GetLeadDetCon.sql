/* Da usare per il debug in management studio
DECLARE @includi_lead_clienti varchar(200), @ditta VARCHAR(50)
SELECT @includi_lead_clienti = '0', @ditta = 'AEC'
*/

SELECT 
    organig.codditt + '§' + cast(og_codlead as varchar) + '§' + cast(og_progr as varchar) as xx_chiave,
	og_codlead,                                                  
	og_progr,                                                  
	og_descont,                                                
	og_descont2,                                               
	og_indir,                                                  
	og_cap,                                                    
	og_citta,                                                  
	og_prov,                                                   
	og_telef,                                                  
	og_cell,                                                   
	og_fax,                                                    
	og_email,                                                  
	og_rep,                                                    
	tabruaz.tb_desruaz AS tb_desruaz  ,
	getdate() as xx_ultagg                         
FROM   organig     WITH (NOLOCK)                                               
INNER JOIN leads    WITH (NOLOCK)                                      
       ON organig.codditt = leads.codditt                
            AND organig.og_codlead = leads.le_codlead   
LEFT JOIN tabruaz    WITH (NOLOCK)                                       
       ON organig.og_codruaz = tabruaz.tb_codruaz             
WHERE 1=1                                                         
	AND og_tipork = 'L'                                       
	AND leads.le_coddest=0
	AND leads.codditt = @ditta
	AND leads.le_servdeleted <> 'S'
	AND organig.og_servdeleted <> 'S'
	AND (leads.le_conto = '0' or @includi_lead_clienti <> '0')
	AND organig.og_servdeleted <> 'S'
ORDER  BY 
    og_conto,                                               
    og_progr        
                                  