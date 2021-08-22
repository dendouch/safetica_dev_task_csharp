# Properties-File-Editor

Safetica properties file editor (C++, C#) 
Vytvořte konzolovou aplikaci, která pracuje s patičkou souboru obsahující libovolné vlastnosti a 
jejich hodnoty.

Musí být dodržena tato pravidla: 
• Patička má následující formát:  
o hlavička: [SafeticaProperties] 
o oddělovač řádků: znak '\n' 
o vlastnosti a hodnoty: property1=value1 
o příklad: 
[SafeticaProperties]\nproperty1=value1\nproperty2=value2 

• Patička je uložena na konci souboru, soubor tedy bude vypadat např. následovně: 
 Lorem ipsum dolor sit amet, consectetuer adipiscing 
elit.[SafeticaProperties]\nproperty1=value1\nproperty2=value2 

• Soubor může být jakéhokoli typu, tzn. textový, binární, ... 

• Jednotlivé vlastnosti je možné přidávat, editovat i mazat (tzn. soubor může patičku již 
obsahovat – aktualizace patičky) 

• Celková velikost patičky nesmí přesáhnout 1024 znaků 

• Jednotlivé příkazy dostává aplikace přes argumenty příkazové řádky, tedy např: 
MyApp.exe add myFileName.txt property1=value1 
MyApp.exe edit myFileName.txt property1=value2 
MyApp.exe remove myFileName.txt property1 

Na co si dát pozor: 
• chybové stavy při práci se soubory 
• podpora velkých souborů
