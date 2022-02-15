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

type Party = BattleNpc list