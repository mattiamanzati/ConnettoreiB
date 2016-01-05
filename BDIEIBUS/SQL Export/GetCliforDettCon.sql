/* Da usare per il debug in management studio
DECLARE @release varchar(200)
SELECT @release = '1.0'
*/


SELECT 
	an_tipo,                                                   
	an_conto,                                                  
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
	tabruaz.tb_desruaz AS tb_desruaz,
	getdate() as xx_ultagg                           
FROM   organig   WITH (NOLOCK)                                                 
INNER JOIN anagra   WITH (NOLOCK)                                      
        ON organig.codditt = anagra.codditt                
            AND organig.og_conto = anagra.an_conto          
LEFT JOIN tabruaz WITH (NOLOCK)                                          
    ON organig.og_codruaz = tabruaz.tb_codruaz             
WHERE 1=1                                                         
	AND an_tipo <> 'S'                                         
	AND ((an_tipo = 'C' and @tipocf = 'C') or (an_tipo = 'F' and @tipocf = 'F') or (an_tipo <> 'S' and @tipocf = 'CF')) 
	AND an_status = 'A'                                       
	AND og_tipork <> 'L'                                       
	AND an_conto NOT IN ( 0 ) 
	AND anagra.codditt = @ditta
ORDER  BY an_tipo,                                                
    an_conto,                                               
    og_progr                                                