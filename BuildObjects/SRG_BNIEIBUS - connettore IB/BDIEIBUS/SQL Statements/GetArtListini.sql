/* Da usare per il debug in management studio
DECLARE @ditta varchar(200)
SELECT @ditta = 'SONN'
*/

SELECT artico.ar_codart + CASE WHEN artico.ar_gesfasi = 'S' THEN '.' + Cast(af_fase AS VARCHAR(5)) ELSE '' END AS ar_codart, 
       artico.ar_unmis, 
       CASE 
         WHEN lc_listino = 0 THEN NULL 
         ELSE lc_listino 
       END                     AS lc_listino, 
       lc_prezzo, 
       lc_daquant, 
       lc_aquant, 
       CASE 
         WHEN lc_conto = 0 THEN NULL 
         ELSE lc_conto 
       END                     AS lc_conto, 
       lc_netto, 
       lc_datagg, 
       lc_datscad, 
       lc_progr, 
       CASE 
         WHEN Isnull(lc_perqta, 0) = 0 THEN 1 
         ELSE lc_perqta 
       END                     AS lc_perqta, 
       lc_codtpro, 
       CASE 
         WHEN lc_codtpro > 0 
              AND lc_conto > 0 THEN 1 
         ELSE 
           CASE 
             WHEN lc_codtpro = 0 
                  AND lc_conto > 0 THEN 2 
             ELSE 
               CASE 
                 WHEN lc_codtpro > 0 
                      AND lc_conto = 0 THEN 3 
                 ELSE 4 
               END 
           END 
       END                     AS xx_prior,
	   getdate() as xx_ultagg   
FROM   artico WITH (NOLOCK)
       LEFT JOIN artfasi WITH (NOLOCK)
              ON artico.codditt = artfasi.codditt 
                 AND artico.ar_codart = artfasi.af_codart 
       INNER JOIN busvw_listini WITH (NOLOCK) 
               ON artico.codditt = busvw_listini.codditt 
                  AND artico.ar_codart = busvw_listini.ar_codart 
                  AND Isnull(artfasi.af_fase, 0) = busvw_listini.lc_fase 
WHERE  1=1
       AND artico.codditt = @ditta
	   AND ( ar_blocco != 'S')                                 
       AND ( ar_stainv = 'S' OR artico.ar_codart = 'D' ) 
       AND ar_codtagl = 0 
       AND lc_listino >= 0 
       AND lc_codvalu = 0 
	   AND convert(datetime,round(convert(float,  lc_datagg ),0,1)) -1  <= convert(datetime,round(convert(float,getdate()),0,1))
       AND lc_datscad >= Getdate() 
       AND ( lc_unmis = artico.ar_unmis OR lc_unmis = ' ' ) 
     --  AND ( artico.ar_gesvar = 'N' OR ( artico.ar_gesvar = 'S' AND artico.ar_codroot <> '' ) ) 
	   AND ( artico.ar_gesvar <> 'S' OR ( artico.ar_gesvar = 'S' AND artico.ar_codroot <> '' ) ) 


