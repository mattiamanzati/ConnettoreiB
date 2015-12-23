VARESE DISTRIBUZIONE
===
## Visualizzazione delle Promozioni

In questa personalizzazione vengono estratti 3 campi custom aggiunti sulla anagrafica degli articoli.
Vengono estratti tutti i dati di articoli che hanno la data di sistema compresa fra quella di inizio e quella di fine promozione.

### Prerequisiti

Sul database di Business devono essere creati i seguenti campi:

Nome campo | Tabella | Descrizione
-|
ar_hhdescrpromo  | artico | Descrizione della promozione
ar_hhiniziopromo | artico | Data di inizio della promozione.
ar_hhfinepromo   | artico | Data di fine della promozione

Ecco lo script di creazione:

```sql
ALTER TABLE artico ADD ar_hhdescrpromo VARCHAR(500)
GO

ALTER TABLE artico ADD ar_hhiniziopromo DATE
GO

ALTER TABLE artico ADD ar_hhfinepromo DATE
GO
```
