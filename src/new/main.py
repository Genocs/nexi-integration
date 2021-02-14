# -*- coding: utf-8 -*-

# Pagamento 3D Secure - Controllo 3D Secure

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

requestUrl = "https://int-ecommerce.nexi.it/ecomm/api/paga/pagaMOTO"

# Alias e chiave segreta
APIKEY = "ALIAS_MOTO_00030460" # Sostituire con il valore fornito da Nexi
CHIAVESEGRETA = "MVTZTZUAH6B2VOPQLDJTCDS42JV71W90" # Sostituire con il valore fornito da Nexi

# Parametri per calcolo MAC
codTrans = "TESTAP_" + datetime.utcnow().strftime('%Y%m%d%H%M%S%f')
importo = "15000" # 5000 = 50,00 EURO (indicare la cifra in centesimi)
divisa = "978" # divisa 978 indica EUR
timeStamp = int(time.time()*1000)

pan = "5255000000000001" # Pan della carta
scadenza = '203012' # Scadenza della carta (Formato aaaamm)
cvv = "123" # CVV della carta

transType = "01"

# Calcolo MAC
mac_str = 'apiKey=' + str(APIKEY) + \
    'codiceTransazione=' + str(codTrans) + \
    'pan=' + pan + \
    'scadenza=' + scadenza + \
    'cvv=' + cvv + \
    'importo=' + importo + \
    'divisa=' + divisa + \
    'timeStamp=' + str(timeStamp) + \
    CHIAVESEGRETA
     
mac =  hashlib.sha1(mac_str.encode('utf8')).hexdigest()

# Parametri di invio
requestParams = {
    'apiKey': APIKEY,
    'codiceTransazione': codTrans,
    'importo': importo,
    'divisa': divisa,
    'pan': pan,
    'scadenza': scadenza,
    'cvv': cvv,
    'timeStamp': str(timeStamp),
    'mac': mac
}

# Chiamata API
response = requests.post(requestUrl, json=requestParams)

# Parametri di ritorno
dataVerifica = response.json()

if dataVerifica['esito'] == "OK":
    macCalculated_str = 'esito=' + dataVerifica['esito'] + 'idOperazione=' + dataVerifica['idOperazione'] + 'timeStamp=' + dataVerifica['timeStamp'] + CHIAVESEGRETA
    macCalculated =  hashlib.sha1(macCalculated_str.encode('utf8')).hexdigest()
    if macCalculated != dataVerifica['mac']:
        print('Errore MAC: ' + macCalculated + ' non corrisponde a ' + dataVerifica['mac'])
    else:
        print('La transazione ' + codTrans + " è avvenuta con successo; codice autorizzazione: " + dataVerifica['codiceAutorizzazione'])
else:
    print('La transazione ' + codTrans + " è stata rifiutata; descrizione errore: " + dataVerifica['errore']['messaggio'])