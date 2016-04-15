  select 
     0 AS xx_an_dest
  union all
  select
     A.dd_coddest
  from
     destdiv A
  where 1=1
  and (A.dd_conto = @conto@)