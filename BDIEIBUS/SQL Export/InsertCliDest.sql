insert into destdiv
(
       codditt
      ,dd_conto
      ,dd_coddest
      ,dd_nomdest
      ,dd_inddest
      ,dd_capdest
      ,dd_locdest
      ,dd_prodest
      ,dd_telef
      ,dd_stato
      ,dd_codcomu
)
values
(
       @codditt@   
      ,@dd_conto@   
      ,@dd_coddest@
      ,@dd_nomdest@ 
      ,@dd_inddest@ 
      ,@dd_capdest@ 
      ,@dd_locdest@ 
      ,@dd_prodest@  
      ,@dd_telef@     
      ,@dd_stato@   
      ,@dd_codcomu@ 
)

update anagra 
set an_destin=@dd_coddest@
where 
     codditt=@codditt@
and  an_conto=@dd_conto@ 
and  an_tipo = 'C'