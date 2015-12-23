-- Da usare per il debug in management studio
-- DECLARE @ditta varchar(50), @includi_lead_clienti varchar(50)
-- SELECT @ditta = '001', @includi_lead_clienti = '0'

SELECT 
	   leads.codditt + '§' + cast(le_codlead as varchar) + '§' + cast(le_coddest as varchar) as xx_chiave,
       le_status,

--       case le_status 
--			when 'I' then 'Inattivo'
--			when 'P' then 'Prospect'
--			when 'A' then 'Attivo'
--			when 'C' then 'Potenziale'
--			else 'Non definito'
--	   end as xx_des_status,
--TNET_INIMOD
--TNET_RIGHE_COMMENTO	7

       case le_status 
			when 'I' then 'Inattivo'
			when 'P' then 'Lead/Prospect'
			when 'A' then 'Cliente'
			when 'C' then 'Potenziale'
			else 'Non definito'
	   end as xx_des_status,

--TNET_FINEMOD	   
	   
	   le_stato,
	   isnull(tb_desstat, 'ITALIA') as tb_desstat,
       le_privacy,
	   case le_privacy
	   	    when 'S' then 'Concessa'
			when 'N' then 'Non concessa'
			else 'Non definita'
	   end as xx_des_privacy,
	   le_contattato,
	   case le_contattato
	   	    when 'S' then 'Si'
			when 'N' then 'No'
			else 'Non definito'
	   end as xx_des_contattato,
	   le_nonint,
	   case le_nonint
	   	    when 'S' then 'Si'
			when 'N' then 'No'
			else 'Non definito'
	   end as xx_des_nonint,
       le_coddest,
	   le_codlead, 
       case le_conto when 0 then null else le_conto end as le_conto,  
	   le_banc1,
	   le_banc2, 
       le_descr1, 
       le_descr2, 
       le_indir, 
       le_citta, 
       le_cap, 
       le_prov,
	   le_note, 
       le_note2, 
       le_pariva, 
       le_codfis, 
       le_telef, 
       le_faxtlx, 
       le_cell, 
       le_email, 
       le_website, 
       le_ultagg, 
       le_banc1, 
       le_banc2, 
       CASE 
         WHEN le_listino = 0 THEN NULL 
         ELSE le_listino 
       END                        AS le_listino, 
	   le_listino,
       tb_deslist, 
	   tb_codcate,
       tb_descate, 
       tb_codzone,
       tb_deszone, 
       tb_descana, 
       '', --le_hhlat_ib, 
       '', --le_hhlon_ib, 
	   '', --xx_ultfatt
       (SELECT TOP 1 testoff.td_datord
        FROM   testoff 
        WHERE  codditt = leads.codditt 
               AND testoff.td_tipork IN ( 'R', 'O', 'H', 'Q' ) 
               AND testoff.td_codlead = leads.le_codlead 
        ORDER  BY td_datord DESC) AS xx_ultoff 
FROM   dbo.leads WITH (NOLOCK)
       LEFT JOIN tablist WITH (NOLOCK)
              ON leads.codditt = tablist.codditt 
                 AND leads.le_listino = tablist.tb_codlist 
       LEFT JOIN tabcana WITH (NOLOCK)
              ON leads.codditt = tabcana.codditt 
                 AND leads.le_codcana = tabcana.tb_codcana 
       LEFT JOIN tabzone WITH (NOLOCK)
              ON leads.codditt = tabzone.codditt 
                 AND leads.le_zona = tabzone.tb_codzone 
       LEFT JOIN tabcate WITH (NOLOCK)
              ON leads.codditt = tabcate.codditt 
                 AND leads.le_categ = tabcate.tb_codcate 
       LEFT JOIN tabstat WITH (NOLOCK)
              ON leads.le_stato = tabstat.tb_codstat
WHERE 1=1
 	   AND leads.codditt = @ditta 

--       AND le_status <> 'I' 
--TNET_INIMOD
--TNET_RIGHE_COMMENTO	1
		AND ( le_status <> 'I' AND le_status <> 'N' )
--TNET_FINEMOD	   

	   AND (le_conto = '0' or @includi_lead_clienti <> '0')
	   AND le_coddest = 0
ORDER  BY 
    le_codlead

	