﻿declare 
   @dd_current int

/* Ciclo per ogni destinazione diversa */
declare C2 cursor local fast_forward read_only FOR
  select 
     0 AS xx_an_dest
  union all
  select
     A.dd_coddest
  from
     destdiv A
  where 1=1
  and (A.an_conto = @conto@)

  open C2

  fetch next from C2 into @dd_current

  while @@fetch_status=0 
  begin
  fetch next from C2 into @dd_current

  /* Per ogni destinazione diversa creo un lead con */ 
  insert into leads (
		le_codlead,
		codditt,
		le_descr1,
		le_descr2,
		le_indir,
		le_cap,
		le_citta,
		le_prov,
		le_stato,
		le_codfis,
		le_pariva,
		le_telef,
		le_faxtlx,
		le_contatt,
		le_ultagg,
		le_zona,
		le_categ,
		le_agente,
		le_banc1,
		le_banc2,
		le_abi,
		le_cab,
		le_note,
		le_agente2,
		le_note2,
		le_email,
		le_website,
		le_usaem,
		le_codling,
		le_status,
		le_codcana,
		le_opnome,
		le_webuid,
		le_webpwd,
		le_conto,
		le_siglaric,
		le_cell,
		le_titolo,
		le_libstr1,
		le_libint1,
		le_coddest,
		le_clascon,
		le_listino,
		le_servdtua,
		le_servdeleted,
		le_clircodlprov,
		le_clirdeleted,
		le_clirupdated,
		le_clirdtua,
		le_idoutlo,
		le_privacy,
		le_contattato,
		le_nonint
  )
  select 
		@codlead@,
		codditt,
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
		an_contatt,
		getdate(),
		an_zona,
		an_categ,
		an_agente,
		an_banc1,
		an_banc2,
		an_abi,
		an_cab,
		an_note,
		an_agente2,
		an_note2,
		an_email,
		an_website,
		an_usaem,
		an_codling,
		an_status,
		an_codcana,
		an_opnome,
		an_webuid,
		an_webpwd,
		an_conto,
		an_siglaric,
		an_cell,
		an_titolo,
		an_libstr1,
		an_libint1,
	    @dd_current, 
		an_clascon,
		an_listino,
		getdate(),
		'N',
		0,
		'N',
		'N',
		'1900-01-01 00:00:00.000',
		0,
		' ',
		'S',
		'N'
  from 
	  anagra a
  where 1=1 
	and codditt = @ditta@
	and an_tipo = 'C' 
    and an_conto = @conto@


/* Fine ciclo */
end
close C2
deallocate C2
