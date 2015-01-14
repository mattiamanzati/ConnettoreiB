/* Da usare per il debug in management studio
DECLARE @ditta varchar(200)
SELECT @ditta = 'SONN'
*/

declare 
  @tb_ventil varchar(1), 
  @tb_azgesscad varchar(1)

select 
	@tb_ventil = case tb_ventil
		when 'S' then 'S'
		when 'C' then 'S'
		else'N'
	end,
		@tb_azgesscad = case tb_azgestscad
		when 'S' then 'S'
		when 'C' then 'S'
		else'N'
	end
from 
	tabanaz 
where 
     codditt = @codditt@


insert into anagra (
	 codditt
	,an_codcomu
	,an_partite
	,an_scaden
	,an_conto
	,an_tipo
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
	,an_codpcon
	,an_ultagg
)
values (
	 @codditt@
	,@an_codcomu@
	,@tb_ventil
	,@tb_azgesscad
	,@an_conto@
	,@an_tipo@
	,@an_codmast@
	,@an_cap@
	,@an_cell@
	,isnull(@an_codcana@,0)
	,isnull(@an_categ@, 0)
	,@an_citta@
	,isnull(@an_clascon@, 0)
	,isnull(@an_codpag@, 0)
	,@an_nazion1@
	,@an_porto@
	,@an_codfis@
 	,@an_email@
	,@an_faxtlx@
	,@an_iban@
	,@an_indir@  
 	,@an_note@
	,@an_note2@
	,@an_pariva@
	,@an_prov@
	,@an_descr1@
	,@an_telef@
	,@an_codpcon@
	,getdate()
)
