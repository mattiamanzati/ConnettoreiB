/* Da usare per il debug in management studio
DECLARE @release varchar(200)
SELECT @release = '1.0'
*/

SELECT  allole.codditt + '§' + ao_tipo + '§' + ao_strcod AS xx_chiave,   
        ao_strcod AS xx_ext_code,
        ao_nomedoc as xx_nome_doc,
		case ao_tipo
			when 'A' then 'Articoli'
			when 'C' then 'Clienti'
			else 'Altro'
		end as xx_l1,                                                                                                   
        isnull(ao_cartella,'Tutti')    AS xx_l2,
        NULL           AS xx_l3,
        NULL           AS xx_l4,
		getdate() as xx_ultagg
    FROM   allole WITH (NOLOCK)                                                                                                                                                                                             
    WHERE  1=1         
	 AND ao_nomedoc IS NOT null                                                                                                 
		AND allole.codditt = @ditta                              
