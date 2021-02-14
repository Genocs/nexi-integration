                                         
# -*- coding: utf-8 -*-

# Pagamento semplice - Avvio pagamento

'''
Pagamenti Telefonici - Vai alle specifiche 
Terminale 00030460  
DATI PER L'INTEGRAZIONE CON I PAGAMENTI:
Alias: ALIAS_MOTO_00030460
Chiave per il calcolo mac: MVTZTZUAH6B2VOPQLDJTCDS42JV71W90
Gruppo: GRP_60603

DATI PER L'ACCESSO AL BACK OFFICE:
Url di backoffice: https://int-ecommerce.nexi.it/ecomm/web/reporting/ReportLogin.jsp
Merchant: 00030460
User: ADMIN
Password: VYHOGOTMM1

PARAMETRI AGGIUNTIVI:
Sono i parametri personalizzabili che verranno gestiti/ritornati dalle operazioni di pagamento.
num_contratto: num_contratto
Per configurare l'elenco dei parametri aggiuntivi, usare la sezione dedicata nel backoffice.

CONFIGURAZIONE:
Mail per invio notifica pagamenti:
giovanni.nocco@gmail.com
Insegna da mostrare nelle pagine di pagamento:
AREA TEST
 Incasso Immediato:Con questa opzione le transazioni vengono incassate automaticamente, diversamente si deve provvedere a richiedere l'incasso da backoffice o tramite API.
 Dynamic Currency Conversion (DCC):Dynamic Currency Choice (DCC) è il servizio che permette ai titolari di carte di credito internazionali VISA e MASTERCARD, di fare acquisti nella propria valuta, utilizzando un tasso di cambio generato al momento del pagamento.
 Server To Server - vai alle specifiche:Per questa opzione i dati sensibili, relativi alla transazione, vengono gestiti direttamente dai server dell'esercente. Questo permette una completa personalizzazione dell'esperienza di pagamento, ma è subordinata all'ottenimento della certificazione di sicurezza PCI-DSS.
 
CARTE TEST
Con le seguenti carte puoi effettuare i test di pagamento:

Circuito	Numero carta	Scadenza	CVV2*	Esito Atteso	Messaggio Errore
VISA	4539970000000006 (EUR)	12/2030	***	Pagamento accettato	Message Ok
VISA	4539970000000014 (EUR)	12/2030	***	Pagamento rifiutato	Auth. Denied
MASTERCARD	5255000000000001 (EUR)	12/2030	***	Pagamento accettato	Message Ok
MASTERCARD	5255000000000019 (EUR)	12/2030	***	Pagamento rifiutato	Auth. Denied* qualsiasi combinazione di 3 numeri è accettata
'''

import sys
if sys.version_info >= (3,):
    from urllib.parse import urlencode
else:
    from urllib import urlencode
import hashlib
from datetime import datetime
import time
import requests

HTTP_HOST = "my-server.example.tdl"

requestUrl = "https://int-ecommerce.nexi.it/ecomm/ecomm/DispatcherServlet"
merchantServerUrl = "https://" + HTTP_HOST + "/xpay/pagamento_semplice_python/codice_base/"

# Alias e chiave segreta
APIKEY = "ALIAS_MOTO_00030460" # Sostituire con il valore fornito da Nexi
CHIAVESEGRETA = "MVTZTZUAH6B2VOPQLDJTCDS42JV71W90" # Sostituire con il valore fornito da Nexi

# Parametri per calcolo MAC
codTrans = "TESTAP_" + datetime.utcnow().strftime('%Y%m%d%H%M%S%f')

divisa = "EUR"
importo = 5000

# Calcolo MAC
mac_str = 'codTrans=' + str(codTrans) + 'divisa=' + str(divisa) + 'importo=' + str(importo) + str(CHIAVESEGRETA)
mac =  hashlib.sha1(mac_str.encode('utf8')).hexdigest()

# Parametri obbligatori
requestParams = {
    'alias': APIKEY,
    'importo': importo,
    'divisa': divisa,
    'codTrans': codTrans,
    'url': merchantServerUrl + "esito.py",
    'url_back': merchantServerUrl + "annullo.py",
    'mac': mac,
}

# Parametri facoltativi
facoltativi = [
]

# Creare un form html con metodo post verso requestUrl con campi hidden contenenti requestParams
# Chiamata API
print(requestParams)

#response = requests.post(requestUrl, json=requestParams)

# Parametri di ritorno
#print(response)