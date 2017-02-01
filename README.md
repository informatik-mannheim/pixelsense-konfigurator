Pixelsense-Konfigurator
=======================

Konfigurationsapplikation für den Microsoft PixelSense Touchtisch als Teildemonstrator im Forschungsprojekt SysPlace. Die Applikation ermöglicht Anwendern, ein 3D-Modell eines Autos zu betrachten und zu konfigurieren (Wechsel der Farbe). Durch Auflegen weiterer Geräte (Smartphone, Tablet, VR/AR-Geräte etc.) kann eine bestehende Konfiguration geladen bzw. die auf dem Tisch erstellte Konfiguration geteilt werden.

Voraussetzungen
-----
- Entwicklung: Windows 7 mit [Microsoft® Surface® 2.0 SDK and Runtime](https://www.microsoft.com/en-us/download/details.aspx?id=26716), .NET 4.0, WPF 4.0, Visual Studio 2010
- Ausführung: Microsoft PixelSense Tisch [Microsoft® Surface® 2.0 SDK and Runtime](https://www.microsoft.com/en-us/download/details.aspx?id=26716)

Detaillierte Informationen zur Entwicklung gibt es [in dieser Einführung von Daniel Litwin](http://services.informatik.hs-mannheim.de/~kohler/MMI/PixelsenseEinfuehrung.pdf), ertstellt im Rahmen der MMI-Vorlesung im WS 2014.

Die Byte Tags liegen im Repository als tags.pdf bei, da sie auf der offiziellen Microsoft-Seite nicht mehr verfügbar sind.

Konfiguration
-----
Zur Erkennung der Geräte werden sog. [Byte Tags](https://msdn.microsoft.com/en-us/library/ee804885(v=surface.10).aspx) verwendet, womit bis zu 256 Geräte eindeutig identifiziert werden können. Die Konfiguration des Demonstrators verwendet per default die Tags `0x01` (Laden der Konfiguration von getaggtem Tablet) und `0x02` (Teilen der Konfiguration mit einer getaggten VR-Brille), die Werte können über die Keys `tag-tablet` und `tag-glasses` konfiguriert werden. Durch die Keys `storage-ip` und `storate-port` kann der Server spezifiziert werden, über den die Konfigration unter bestimmten Schlüsseln (`storage-key-cas` zum Laden, `storage-key-3m5` zum Speichern) geteilt wird. Dabei wird ein einfaches JSON-Format verwendet.

Sämtliche Angaben sind über die Datei `app.config` anpassbar:

```
  <add key="storage-ip" value="37.61.204.167"/>
  <add key="storage-port" value="8080"/>
  <add key="storage-key-cas" value="config-cas" />
  <add key="storage-key-3m5" value="config-3m5" />
  <add key="tag-tablet" value="01" />
  <add key="tag-glasses" value="02" />

```

JSON-Format der Konfiguration (variabel ist im Grunde nur der Farbwert, der "Blau" oder "Grün" sein kann):

```
{
   "product":{
      "attributeGroups":[
         {
            "name":"Exterior",
            "attributes":[
               {
                  "name":"Farbe",
                  "selectedValues":[
                     "Blau"
                  ]
               },
               {
                  "name":"Scheibentönung",
                  "selectedValues":[

                  ]
               },
               {
                  "name":"Felgen",
                  "selectedValues":[

                  ]
               }
            ]
         },
         {
            "name":"Interior",
            "attributes":[
               {
                  "name":"Polster",
                  "selectedValues":[

                  ]
               },
               {
                  "name":"Navigation",
                  "selectedValues":[

                  ]
               }
            ]
         }
      ]
   },
   "timestamp":146278164764.9251
}

```