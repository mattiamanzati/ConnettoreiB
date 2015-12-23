/* 
Query perconalizzata per l'estrazione dei dati custom di Adriatic 
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
    artico.codditt + '§' + cast(artico.ar_codart as varchar) as xx_chiave,
	mo_codart as xx_ext_code, 
	sum(mo_quant) as xx_tot_impegnato, 
	sum(case when mo_flevas = 'S' then mo_quant else mo_quaeva end) as xx_tot_evaso
from 
	artico 
		inner join movord 
			on artico.codditt = movord.codditt and artico.ar_codart = movord.mo_codart
		inner join testord	
			on testord.codditt = movord.codditt
			and testord.td_tipork = movord.mo_tipork
			and testord.td_anno = movord.mo_anno
			and testord.td_serie = movord.mo_serie
			and testord.td_numord = movord.mo_numord
WHERE 1=1
	and td_tipork = 'R'
--	and td_datord between #DaPeriodo# and #APeriodo#
	AND movord.codditt = @ditta
GROUP BY artico.codditt + '§' + cast(artico.ar_codart as varchar), mo_codart
ORDER BY mo_codart
 