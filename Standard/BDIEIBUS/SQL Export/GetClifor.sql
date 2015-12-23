/* Da usare per il debug in management studio
DECLARE @release varchar(200)
SELECT @release = '1.0', @tipocf = 'CF'
*/

-- Se si modifica questa query occorre modificare anche la GetCliforOld.sql per versioni di Business < 19

SELECT an_tipo, 
       an_conto, 
       an_descr1, 
       an_descr2, 
       an_indir, 
       ltrim(rtrim(an_citta)) as an_citta, 
       an_cap, 
       an_prov, 
       an_note2,
	   an_siglaric, 
       an_pariva, 
       an_codfis, 
       an_telef, 
       an_faxtlx, 
       an_cell, 
       an_email, 
       an_website, 
       an_ultagg, 
       an_blocco, 
	   CASE 
         WHEN an_fido = 0 THEN NULL 
         ELSE an_fido 
       END                        AS an_fido, 
       an_banc1, 
       an_banc2, 
       CASE 
         WHEN an_listino = 0 THEN NULL 
         ELSE an_listino 
       END                        AS an_listino, 
       an_codpag, 
       an_dtaper, 
       tb_deslist, 
       tb_despaga, 
       tb_scopaga, 
       tb_desvalu,
	   '-1' as xx_flg_mod_nel_disp, 
	   '-1' as xx_flg_deperibilita, 
       an_clascon,
	   an_valuta,
	   anagra.an_categ as xx_categ,
       tb_descate, 
       tb_deszone,
	   tb_desstat, 
       tb_descana,
       CASE 
         WHEN an_pariva = '99999999999' THEN '-1' 
         ELSE null 
       END                        AS xx_flg_new, 
       an_latitud  as an_latitud, 
       an_longitud as an_longitud,
	   (select max(tm_datdoc) from testmag where testmag.codditt = anagra.codditt and testmag.tm_conto = anagra.an_conto and tm_tipork in ( 'A', 'D','L','K') ) AS xx_ultfatt, 
       (SELECT TOP 1 td_datord 
        FROM   testord 
        WHERE  codditt = anagra.codditt 
               AND td_tipork IN ( 'R', 'O', 'H', 'Q' ) 
               AND td_conto = anagra.an_conto 
        ORDER  BY td_datord DESC) AS xx_ultord
FROM   anagra WITH (NOLOCK)
       LEFT JOIN tabpaga WITH (NOLOCK)
              ON anagra.an_codpag = tabpaga.tb_codpaga 
       LEFT JOIN tabvalu WITH (NOLOCK)
              ON anagra.an_valuta = tabvalu.tb_codvalu 
       LEFT JOIN tablist WITH (NOLOCK)
              ON anagra.codditt = tablist.codditt 
                 AND anagra.an_listino = tablist.tb_codlist 
       LEFT JOIN tabcana WITH (NOLOCK)
              ON anagra.codditt = tabcana.codditt 
                 AND anagra.an_codcana = tabcana.tb_codcana 
       LEFT JOIN tabzone WITH (NOLOCK)
              ON anagra.codditt = tabzone.codditt 
                 AND anagra.an_zona = tabzone.tb_codzone 
       LEFT JOIN tabcate WITH (NOLOCK)
              ON anagra.codditt = tabcate.codditt 
                 AND anagra.an_categ = tabcate.tb_codcate 
	   LEFT JOIN tabstat WITH (NOLOCK)
	          ON anagra.an_stato = tabstat.tb_codstat
WHERE 1=1
 	   AND anagra.codditt = @ditta 
       AND an_tipo <> 'S' 
       AND an_status = 'A' 
       AND ( ( an_tipo = 'C' AND @tipocf = 'C' ) OR ( an_tipo = 'F' AND @tipocf = 'F' ) OR ( an_tipo <> 'S' AND @tipocf = 'CF' ) ) 
	   AND (isnull(an_agente,0) <> 0 or cast(@filtra_con_agenti as integer) <>1)
ORDER  BY 
	an_tipo, 
    an_conto 