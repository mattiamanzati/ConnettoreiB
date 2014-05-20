/* Da usare per il debug in management studio
DECLARE @ditta varchar(200)
SELECT @ditta = 'SONN'
*/

insert into anagra (
	 codditt
	,an_conto
	,an_codmast
	,an_cap
	,an_cell
	,an_codcana 
	,an_categ
	,an_citta
	,an_clascon
	,an_codpag 
	,an_nazion1
	,an_porto 
	,an_codfis
	,an_email 
	,an_faxtlx
	,an_iban
	,an_indir  
	,an_note 
	,an_note2 
	,an_pariva
	,an_prov
	,an_descr1
	,an_telef  
)
values (
	 @ditta
	,@an_conto
	,@an_codmast
	,@cap
	,@cellulare
	,isnull(@cod_canale_vendita,0)
	,isnull(@cod_categoria, 0)
	,@cod_citta
	,isnull(@cod_classe_sconto, 0)
	,isnull(@cod_cond_pag, 0)
	,@cod_nazione
	,@cod_porto_sped 
	,@codice_fiscale
 	,@email
	,@fax
	,@iban
	,@indirizzo  
 	,@titolo_note
	,@note 
	,@partita_iva
	,@provincia
	,@ragione_sociale
	,@telefono  
)
