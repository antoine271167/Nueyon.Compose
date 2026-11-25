# Evidence review

### wolf — 2ff42e2fa4

- Narrative excerpt: “elandkalveren en volwassen mannelijke elanden”
- Archived evidence: “elk calves comprised 50% of wolf prey, and bull elk comprised 25%”
- Source review: Yellowstone elk are wapiti. The Dutch word eland changes the prey species.
- Evidence-first technical_failure (miss). Experiment 20 REVISE (confirmed). c12 quotes elandkalveren and elanden but normalizes both to elk. The true source anchor is present in raw c12. c14 joins noncontiguous source fragments.

### webb — 0d1f75b51c

- Narrative excerpt: “Daarna volgden weken van afkoeling”
- Archived evidence: “Note the major changes in the area marked as days 3-10 when the sunshield was deployed.”
- Source review: The brief sequences cooling after the telescope’s completed second-week deployment, while the source records cooling changes during deployment.
- Evidence-first technical_failure (miss). Experiment 20 GO (miss). c15 extracts weeks of cooling but loses Daarna and completion precedence. c05 treats cooling as part of commissioning. Other source quotations include synthetic ellipses or wording.

### faa — a027be447f28

- Narrative excerpt: “hotspots voor verkeerd baangebruik”
- Archived evidence: “a cylinder marks a wrong surface hot spot”
- Source review: The wording broadens wrong surface to improper use and narrows the surface to a runway. The named FAA category is central to the brief.
- Evidence-first UNCERTAIN (miss). Experiment 20 REVISE (confirmed). claim-009 equates verkeerd baangebruik and wrong surface. The UNCERTAIN finding concerns label continuity since 2022, not category identity.

### eia — 0f2553c79399

- Narrative excerpt: “42,4 van de 605,6 quadriljoen Btu”
- Archived evidence: “42.4 out of 605.6 quadrillion Btu”
- Source review: English quadrillion is 10^15, whereas Dutch quadriljoen is 10^24. The Dutch scale term for 10^15 is biljard.
- Evidence-first technical_failure (miss). Experiment 20 REVISE (confirmed). claim-009 quotes quadriljoen but normalizes to English quadrillion, erasing the factor of 10^9. Its exact evidence is present in raw output; other citations join separate paragraphs.

### digital — 166274757454

- Narrative excerpt: “wisselden tweewekelijkse sprints af tussen aanvragen aannemen en tijd besteden aan testen, onderzoek en verwerking”
- Archived evidence: “testing, researching and accepting applications in the first week and then suspending submissions to concentrate on testing, researching and processing applications the second”
- Source review: The alternation was between weeks within each sprint. Research and testing happened in both weeks.
- Evidence-first REVISE (confirmed). Experiment 20 GO (miss). c14 retains sprint-level alternation; original source documents two weeks within each sprint with testing and research in both.

### wolf — 1ae0c9278f

- Narrative excerpt: “volwassen mannelijke elanden”
- Archived evidence: “elk calves comprised 50% of wolf prey, and bull elk comprised 25%”
- Source review: Yellowstone elk are wapiti. The Dutch word eland changes the prey species.
- Evidence-first UNCERTAIN (miss). Experiment 20 REVISE (miss). claim-013 retains elanden but sourceMeaning translates elk as stierelanden. The candidate omission question concerns bison. The comparator rewrites contextual wolf prey as wolf kills without identifying the species error.

### bennu — bennu-challenge

- Narrative excerpt: “About 20 minutes after its release above Earth’s atmosphere, the capsule fired its engines to divert away from Earth.”
- Archived evidence: “About 20 minutes after the spacecraft released its sample capsule above Earth’s atmosphere, it fired its engines to divert its trajectory away from Earth.”
- Source review: The engine firing is attributed to the capsule instead of the spacecraft.
- Evidence-first technical_failure (miss). Experiment 20 REVISE (confirmed). Claims 002, 004 and 005 identify the actor error in raw comparison. claim-008 drops “its sample” from an unrelated source quotation; the entire response fails validation.

### earthquake — earthquake-challenge

- Narrative excerpt: “measured amplitude increases 32-fold, while energy release increases tenfold”
- Archived evidence: “While each whole number increase in magnitude represents a tenfold increase in the measured amplitude, it represents an 32 times more energy release.”
- Source review: The measured amplitude and energy multipliers are swapped.
- Evidence-first REVISE (confirmed). Experiment 20 REVISE (confirmed). The amended story swaps ×10 and ×32. Candidate flags the two swapped assertions separately. Both arms ground the correction in the exact USGS excerpt.

### coral — coral-challenge

- Narrative excerpt: “corals always recover when stress-caused bleaching is not severe”
- Archived evidence: “If the stress-caused bleaching is not severe, coral have been known to recover.”
- Source review: Documented possibility is changed into a guarantee.
- Evidence-first REVISE (confirmed). Experiment 20 REVISE (confirmed). The assertion always recover upgrades evidence of observed recovery to a guarantee. Both arms identify this. The separate candidate uncertainty about non-inevitability does not negate the blocker.

## Acceptable briefs

- 20108143a8. Coverage questions ask whether to add outcome metrics and service-channel detail. These are editorial omissions from an acceptable brief.
- 4071526efe. Cooling control withheld for nonverbatim Narrative excerpt; raw paraphrase combines a contrast into an unquoted string.
- 42e705d60d. Lowell endpoint frames the boom and subsequent slowdown. Later population and enterprise information is outside the chosen endpoint; related coverage question is overcautious.
- 4b4216205f. Valid concern about the locally ambiguous 46-lender wording, but the full brief gives 46 for the entire trial. A proposed second trial was outside the angle. Keep F1.
- bennu-control. No claim about the physical trajectory of the capsule. The brief calls for keeping two story threads distinct. Uncertainty arose by treating editorial direction as a physical comparison.
- earthquake-control. The correct amplitude and energy multipliers are preserved; both judges returned GO.
- coral-control. Candidate requires evidence that recovery is not inevitable despite the source giving an observation rather than a guarantee. No material contradiction in the full brief.

## Assessment independence

The operator checked archived target labels against the original source anchors, examined every blocker and disputed finding, and reviewed new mutation and control briefs before acceptance scoring. Six fresh review sessions on the three new subjects used the full inputs without the experimental verdict or label. These used the same GPT-6 Luna model, with previous editorial scores retained for exactly matched historical inputs. The independent reference tracks the known central errors; full-brief claim recall and general evidence alignment accuracy cannot be inferred from target-only review. Exact quote validation establishes lexical provenance, while semantic relevance and accuracy were adjudicated for the decision-bearing findings. No claims were added to the frozen truth after results arrived.
