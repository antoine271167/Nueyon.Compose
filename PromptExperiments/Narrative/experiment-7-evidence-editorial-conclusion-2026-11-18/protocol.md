# Narrative Experiment 7 — Evidence versus Editorial Conclusion

Experimenten 5 en 6 behielden Endpoint. Zowel D als E zette in alle drie Digital-runs de gebruikscijfers centraal in het slot. E introduceerde in alle drie Webb-runs een te stellige volgorde van ontplooiing en afkoeling. Hypothese voor F: een eigen plaats voor bewijs geeft de redactionele conclusie ruimte om de Selected Idea uit te werken, met behoud van brontrouw.

Controle B en de twintig fixtures komen byte voor byte uit Experimenten 5 en 6. F verandert alleen de systemprompt van B zoals vastgelegd in prompts.diff. De brief blijft één string in Content. Selected Story, Context en Development blijven behouden. Evidence/Observed Results krijgt een eigen onderdeel, gevolgd door Editorial Conclusion. Critical Constraints blijft optioneel. Selected Idea bepaalt de focus en uitsluitend Synthesis ondersteunt inhoudelijke claims. De bron dient bij beoordeling als controle op overgeërfde ambiguïteit. Er worden geen nieuwe feiten, volgordes of causale verbanden afgeleid.

config.json bevriest Digital en Webb voor fase 1, Wolf en Lowell voor fase 2, elk met drie verse stateless B/F-paren. Elk stadium bevat evenveel B-eerst- als F-eerst-paren. Gebruik GPT-6 Luna (gpt-6-luna), dezelfde userprompt per paar en hetzelfde productieschema en instellingen. Narrative gebruikt maximaal 5000 completiontokens. Geen vervangende runs of selectie op kwaliteit.

## Handmatige rubric

0 is materieel onvoldoende, 1 is gedeeltelijk voldoende, 2 is volledig voldoende. Leg voor elke brief tekstbewijs en gepaarde dalingen vast. Een volledige brief scoort 2 op alle zes assen.

- Focus. Conclusie verdiept de Selected Idea. Digital moet inzicht geven in de werkwijze onder tijdsdruk, waaronder parallel leren, testen, bouwen of valideren. Gebruikscijfers zijn uitsluitend ondersteunende observaties.
- Structuur. Bewijs en redactionele gevolgtrekking hebben elk een herkenbare functie. Bij F moeten beide gelabelde onderdelen aanwezig zijn en die functie vervullen. B kan dezelfde functies vervullen binnen zijn eigen indeling. Een conclusie die hoofdzakelijk cijfers of het laatste chronologische feit levert, scoort maximaal 1. Een ontbrekende functie of F-sectie scoort 0.
- Progressie. Context oriënteert, Development werkt het centrale punt uit en de conclusie geeft een onderscheidend, onderbouwd inzicht zonder herhaling of zijpad.
- Details. Concrete, relevante details dragen de brief. Digital behoudt het parallelle werk en het onderscheid tussen beide sprintweken. Webb behoudt de reden van het vouwen.
- Brontrouw. Controleer identiteit, actoren, tijd, causaliteit, toeschrijving, scope en onzekerheid in de hele brief. Een materiële ongefundeerde claim scoort 0, ambiguïteit 1. Webb mag afkoeling nergens strikt na voltooide ontplooiing plaatsen, ook niet impliciet. Digital laat het startpunt van twaalf weken open. Wolf behoudt wapiti en de grenzen van ecosysteemclaims. Lowell onderscheidt bedrijven van bewoners en schrijft hergebruik geen onbewezen economische invloed toe.
- Handoff. De brief is samenhangend, concreet, formatagnostisch en rechtstreeks bruikbaar via het bestaande Compose-contract.

## Doorgang

Fase 1 vereist twaalf geldige calls met exact offline replay, 3/3 Digital op focus en structuur, en 3/3 Webb op brontrouw. Over de zes F-briefs geldt minstens 5/6 op volledige focus, 5/6 op volledige structuur en 5/6 volledig voldoende, met minstens 2/3 volledig voldoende per case. Focussom en brontrouwsom moeten beide stijgen, behalve bij een maximale B-score waar gelijkstand volstaat. Geen gepaarde daling naar 0, geen nieuw type materiële bronfout en maximaal 110% van B's totale tokens. Alle criteria zijn verplicht. Technisch onvolledig blokkeert de poort. Stop bij NO-GO.

Fase 2 herhaalt de generieke drempels. Focus- en brontrouwsom mogen gelijk blijven. Alleen bij GO volgen acht echte LinkedIn-calls via productie-Compose met de vaste paren Digital r3, Webb r1, Wolf r2 en Lowell r1. Verifieer de volledige Content-handoff en replay alle calls. Beoordeel aantrekkelijkheid, leesbaarheid, centrale boodschap tot het slot en feitelijke juistheid op dezelfde schaal. F vereist 4/4 op boodschap en juistheid, minstens 3/4 op aantrekkelijkheid en leesbaarheid en geen gepaarde daling naar 0. Controleer of het inzicht en bewijs uit de brief terechtkomen in hun respectieve rollen zonder interne labels in de post. Digital moet bij de werkwijze uitkomen. Gebruikscijfers mogen alleen als ondersteunende resultaten dienen of worden weggelaten. Geen automatische promotie, commit of push.
