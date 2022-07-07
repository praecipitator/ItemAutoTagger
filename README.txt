Adds tags to items, for usage with inventory/sorting mods.
Supports FIS, VIS, VIS-G, LWIS, and Horizon (tags only).
Beta. Use at your own risk!

Issues:
- fails to recognize certain items, like "pa_comp_T60_LArm [MISC:0003C5AE]" as loose mod
    - this thing and similar are configured in a very weird way. Probably no point of adding code for that, either hardcode them or expect the main tagging mod to add proper tags.

- DLC05WorkshopFireworkWeapon* and DLC04FakeAlienBlasterTurret get processed. Blacklist them via EDID? Or just leave it in?

Other todos:
- add more heuristics for other mods
- add autopatching of mod-added INNRs
- add hard item type override via Synthesis UI
- Move itemtyper into it's own repo, then use it via nuget? I'll probably need this when I do a workbench organizer/recipe mover.
