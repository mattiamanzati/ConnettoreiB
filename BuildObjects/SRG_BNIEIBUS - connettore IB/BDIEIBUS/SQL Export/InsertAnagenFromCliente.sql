-- Inserimento in anagrafica generale

INSERT INTO anagen (  
		ag_codanag,
		ag_descr1,
		ag_descr2,
		ag_indir,
		ag_cap,
		ag_citta,
		ag_prov,
		ag_stato,
		ag_codfis,
		ag_pariva,
		ag_telef,
		ag_faxtlx,
		ag_ultagg,
		ag_note,
		ag_note2,
		ag_email,
		ag_website,
		ag_usaem,
		ag_codling,
		ag_opnome,
		ag_webuid,
		ag_webpwd,
		ag_siglaric,
		ag_cell,
		ag_titolo
)
select  
		@codanagen@,
		an_descr1,
		an_descr2,
		an_indir,
		an_cap,
		an_citta,
		an_prov,
		an_stato,
		an_codfis,
		an_pariva,
		an_telef,
		an_faxtlx,
		getdate(),
		an_note,
		an_note2,
		an_email,
		an_website,
		an_usaem,
		an_codling,
		an_opnome,
		an_webuid,
		an_webpwd,
		an_siglaric,
		an_cell,
		an_titolo
from 
	anagra a
where 1=1 
	and codditt = @ditta@
	and an_tipo = 'C' 
    and an_conto = @conto@

-- Imposto il collegamento con cod_anagen
update anagra
	set an_codanag = @codanagen@
where 1=1 
	and codditt = @ditta@
	and an_tipo = 'C' 
    and an_conto = @conto@

-- Inserisco una riga in anasto
INSERT INTO anasto (  
		as_codanag,
		as_descr1,
		as_descr2,
		as_indir,
		as_cap,
		as_citta,
		as_prov,
		as_stato,
		as_codfis,
		as_pariva,
		as_telef,
		as_faxtlx,
		as_note,
		as_note2,
		as_email,
		as_website,
		as_usaem,
		as_codling,
		as_webuid,
		as_webpwd,
		as_siglaric,
		as_cell,
		as_titolo,
		as_datini,
		as_datfin
)
select  
		@codanagen@,
		an_descr1,
		an_descr2,
		an_indir,
		an_cap,
		an_citta,
		an_prov,
		an_stato,
		an_codfis,
		an_pariva,
		an_telef,
		an_faxtlx,
		an_note,
		an_note2,
		an_email,
		an_website,
		an_usaem,
		an_codling,
		an_webuid,
		an_webpwd,
		an_siglaric,
		an_cell,
		an_titolo,
		DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())),
		convert(datetime, '2099/12/31', 111) 
from 
	anagra a
where 1=1 
	and codditt = @ditta@
	and an_tipo = 'C' 
    and an_conto = @conto@