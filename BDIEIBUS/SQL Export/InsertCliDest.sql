/* Da usare per il debug in management studio
DECLARE @ditta varchar(200)
SELECT @ditta = 'SONN'
*/

/* Calcolo un progressivo valido che non sia fra quelli riservati (Da 1 a 1000)*/
declare 
   @dd_coddest int = 1,
   @v_VCOUNT1 int = 0,
   @v_VCOUNT2 int = 0,
   @v_VCOUNT3 int = 0,
   @v_VCOUNT4 int = 0


 while @dd_coddest < 1000
 begin

    /* Controllo se è gia' occupato come domicilio fiscale amministrazione*/
    set @v_VCOUNT1 = 0
    select
      @v_VCOUNT1 = COUNT(*)
    from
      tabinsg A
    where (A.tb_destcorr = @dd_coddest)

	/* Controllo se è gia' occupato come domicilio fiscale sede legale */
    set @v_VCOUNT2 = 0
    select
      @v_VCOUNT2 = COUNT(*)
    from
      tabinsg B
    where (B.tb_destsedel = @dd_coddest)

	/* Controllo se è gia' occupato come domicilio fiscale */
	set @v_VCOUNT3 = 0
    select
      @v_VCOUNT3 = COUNT(*)
    from
      tabinsg C
    where (C.tb_destdomf = @dd_coddest)

	/* Controllo se è gia' occupato come domicilio attiva estera */
	set @v_VCOUNT4 = 0
    select
      @v_VCOUNT4 = COUNT(*)
    from
      tabinsg D
    where (D.tb_destresan = @dd_coddest)


    if @v_VCOUNT1 = 0 AND @v_VCOUNT2 = 0 AND @v_VCOUNT3 = 0 AND @v_VCOUNT4 = 0 begin
      break
    end
    set @dd_coddest = @dd_coddest + 1
end


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
      ,@dd_coddest
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
set an_destin=1 
where 
     codditt=@codditt@
and  an_conto=@dd_conto@ 
and  an_tipo = 'C'
