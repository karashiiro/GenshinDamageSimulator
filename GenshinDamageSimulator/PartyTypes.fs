namespace GenshinDamageSimulator

open EntityTypes

module PartyTypes =
    [<Struct>]
    type ElementalResonance =
        | FerventFlames
        | SoothingWater
        | HighVoltage
        | ShatteringIce
        | ImpetuousWinds
        | EnduringRock
        | ProtectiveCanopy

    [<Struct>]
    type Party =
        { Members: BattleNpc list }