/* 
Query perconalizzata per l'estrazione dei dati custom 
Importante:
  1. Non cambiare i nomi degli alias delle colonne
  2. La query deve contenere sempre
	a. xx_chiave: che identifica univocamente la riga
	b. xx_ext_code: che deve contenere la chiave del campo custom, ovvero il codice articolo
	c. xx_tot_impegnato: (quantità totale impegnata)
	d. xx_tot_evaso: (quantità totale evasa)

Questa query è contenuta nel file chiamato cGetCustomFieldsProdotti.sql

*/

select 
    artico.codditt + '§' + cast(artico.ar_codart as varchar) as xx_chiave, -- Non cambiare l'alias di questa colonna
	ar_codart        as xx_ext_code,                                       -- Non cambiare l'alias di questa colonna
	ar_hhdescrpromo  as xx_promozione, 
	ar_hhiniziopromo as xx_data_inizio,
	ar_hhfinepromo   as xx_data_fine
from 
	artico 
WHERE 1=1
	and getdate() between ar_hhiniziopromo and ar_hhfinepromo
	and ar_hhdescrpromo is not null
	AND artico.codditt = @ditta
    AND ( ar_blocco != 'S')                                                                                   
    AND ( ar_stainv = 'S' )
	AND ( ar_gesvar <> 'S' OR ( ar_gesvar = 'S' AND ar_codroot <> '' ))
	AND ( ar_codart is not null )
	AND ( ltrim(rtrim(ar_codart)) <> '')