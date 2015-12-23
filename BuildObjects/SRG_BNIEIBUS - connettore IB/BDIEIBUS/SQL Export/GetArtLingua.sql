SELECT 
    codditt + '§' + cast(ax_codart as varchar) +  '§' + cast(ax_codvalu as varchar)  as xx_chiave,
    ax_codart as ax_codart, 
    ax_codvalu as ax_codvalu, 
    ax_descr as ax_descr,
    ax_desint as ax_desint,
    ax_note as ax_note,
	getdate() as xx_ultagg
FROM   artval WITH (NOLOCK)                                                                                                                                                  
WHERE  1=1                                                                                                       
	AND codditt = @ditta 
	AND ax_descr is not null