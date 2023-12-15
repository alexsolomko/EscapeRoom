## EscapeRoom
Erstelle ein Projekt, in dem du einen rechteckigen Raum erstellst, der als Level dient. Darin kann sich eine Spielfigur bewegen, ein Item einsammeln und damit eine Tür öffnen.

Am Beginn der Laufzeit wird die Spielmechanik erklärt. Danach soll die Länge und die Breite des Raumes festgelegt werden können. Verwende hierfür Integer-Variablen. Nachdem die Eingabewerte auf Korrektheit und Sinnhaftigkeit geprüft wurden, wir der Raum erstellt.

Verwende für die Verwaltung und Darstellung des Raum ein zweidimensionales Array mit passenden Datentyp (z.B. char). Am Rand des Raums befinden sich Wände, der Rest ist begehbarer Boden.

Die Spielfigur und der Schlüssel werden an einer zufälligen Position innerhalb des Raums platziert. Die Position ist gleichbedeutend mit den Indizes der Elemente im Array. Die Tür soll an einer zufälligen Position in einer der Wände (am Rand des Raumes) platziert werden. Die Tür ist verschlossen.

Die Spielfigur kann mit den Pfeiltasten bewegt werden. Pro Schritt muss die entsprechende Taste einmal gedrückt werden. Bewegt sich die Spielfigur muss ihr Symbol an die passende Stelle im Array gesetzt und an der alten gelöscht werden. Für die Bewegung der Spielfigur, kannst du den Cursor in der Eingabeaufforderung mit der Methode Console.SetCursorPosition(Int32, Int32) an die passende Stelle setzen und das Symbol dort hinschreiben.

Achte darauf, dass die Spielfigur nicht durch Wände und verschlossene Türen laufen kann. Überprüfe ob das entsprechende Element im Array eine Wand oder eine Tür ist. Wenn ja wird die Spielfigur nicht bewegt.

Der Schlüssel wird eingesammelt in dem die Spielfigur über ihn läuft, also sich auf der Position des Schlüssel befindet. In diesem Fall kannst du an der entsprechenden Stelle später ein Leerzeichen schreiben und so den Schlüssel "verschwinden lassen". Gleiches gilt für die Tür die sich dadurch öffnet.

Wenn die Spielfigur sich in der offenen Tür befindet und sich nach draußen bewegt gilt das Spiel als geschafft. Ein kurzer Text gratuliert den Spielenden, danach wird das Programm beendet.
