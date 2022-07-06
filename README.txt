Adds tags to items, for usage with inventory/sorting mods.
Supports FIS, VIS, VIS-G, LWIS, and Horizon (tags only).
Early alpha. Do not use yet.

Issues:
- fails to recognize pa_comp_T60_LArm "[Valuable] T-60 Left Arm Armor" [MISC:0003C5AE] as loose mod

Other todos:
- add more heuristics for other mods
- add hard item type override via Synthesis UI
- Move itemtyper into it's own repo, then use it via nuget? I'll probably need this when I do a workbench organizer/recipe mover.