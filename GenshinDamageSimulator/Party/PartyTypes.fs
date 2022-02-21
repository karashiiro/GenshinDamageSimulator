namespace GenshinDamageSimulator

type ElementalResonance =
    | FerventFlames
    | SoothingWater
    | HighVoltage
    | ShatteringIce
    | ImpetuousWinds
    | EnduringRock
    | ProtectiveCanopy

type Party = Map<EntityId, CharacterEntity>