Om flowcharts/Fungus te gebruiken:

Open de scene 'Keuzebomen'

maak een leeg gameobject aan in de scene, noem dit gameobject 'Karakter Naam' + flowchart, bijvoorbeeld Lambertus Flowchart
plaats dit gameobject als child object in het gameObject 'Flowcharts', er staat als het goed is een object genaamd 'Placeholder Flowchart'.
zorg ervoor dat je flowchart als eerste child staat zodat deze straks te testen is
Op dit object, AddComponent -> Flowchart

Op het flowchart component zit in een knopje 'Open Flowchart Window' klik erop en de flowchart scherm gaat open.
(het is misschien handig om te kijken hoe de placeholder flowchart in elkaar zit)

In dit scherm staat een venster 'Variables'. Klik op het plusje om een variabele toe te voegen.
Klik op Int, er komt nu een nieuw variabele genaamd Var, verander Var naar Health, de 0 kan blijven, verander private naar public
Herhaal dit voor Money en Status.


in de flowchart scherm, rechtermuisknop -> Add Block. Dit is het eerste blok, deze moet Start heten (let op hoofdletter S).
Klik op het blok en onderaan de inspector zit een plusje, hier kunnen flowchart componenten op worden gezet

de belangrijkste:
Narrative -> say: 
	dit is een stukje tekst, bijvoorbeeld "Wil je werken voor de NSB"

Narrative -> Menu: 
	dit zijn dan de keuzes die je hebt om voor te kiezen. Deze moeten worden gekoppeld aan andere Blocks (kijk naar de placeholder flowchart voor duidelijkheid)
	Text: de tekst die in het knopje te zien is
	Target Block: het blok waar de flowchart heen gaat als je op deze knop klikt, er verschijnen lijntjes bij een verbonden Block

Variable -> Set Variable:
	Dit verandert 1 van de variabelen die in het venster staan. 
	Klik op Variabele en selecteer 1. Dit zullen Health, Money en Status zijn
	Operation, -= haalt het eraf, += erbij
	Integer is het getal hoeveel eraf of erbij gaat

Zuiderhuizen -> Detect New Event:
	bij elk nieuwe dag, waar dus een Narrative Menu op staat, moet het allereerste component Detect New Event zijn.

Zuiderhuizen -> UpdateVariables:
	Dit synchroniseert de variabelen van de flowchart met die van het script van het karakter, bijvoorbeeld Lambertus
	Dit moet altijd nadat variabelen zijn aangepast met Variable -> Set Variable anders worden de getallen in het spel niet aangepast


Vergeet niet een prefab te maken van je flowchart en deze in de Prefab -> Flowcharts folder te zetten en deze via SVN te comitten
Noem de blocks iets handigs, zoals Dag 1 Optie 1 zodat het leesbaar blijft. 
BLock description geven een handige en korte descrption aan de blocks. Dit helpt ook om overzicht te houden.