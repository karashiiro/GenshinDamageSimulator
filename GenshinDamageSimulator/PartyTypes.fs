namespace GenshinDamageSimulator

[<Struct>]
type ElementalResonance =
    | FerventFlames
    | SoothingWater
    | HighVoltage
    | ShatteringIce
    | ImpetuousWinds
    | EnduringRock
    | ProtectiveCanopy

type Party = Map<CombatantId, BattleNpc>