namespace GenshinDamageSimulator.Data

open System.Reflection
open System.IO
open Newtonsoft.Json

exception MappingNotFoundException of string * uint32

// https://github.com/Dimbreath/GenshinData/blob/master/ExcelBinOutput/ElementCoeffExcelConfigData.json
module ElementalCoefficients =
    type private CoeffDataRow () =
        [<JsonProperty("Level", NullValueHandling = NullValueHandling.Ignore)>]
        member val Level = 0u with get, set
        member val ElementLevelCo = 0f with get, set
        member val PlayerElementLevelCo = 0f with get, set
        member val PlayerShieldLevelCo = 0f with get, set

    let private levelScalingData =
        lazy (
            "GenshinDamageSimulator.Data.ElementCoeffExcelConfigData.json"
            |> Assembly.GetExecutingAssembly().GetManifestResourceStream
            |> fun data -> new StreamReader(data)
            |> fun reader -> reader.ReadToEnd()
            |> JsonConvert.DeserializeObject<CoeffDataRow array>
        )

    let private characterLevelMultipliers =
        lazy (
            levelScalingData.Force()
            |> Array.map (fun x -> (x.Level, x.PlayerElementLevelCo))
            |> Map.ofArray
        )

    let private characterCrystallizeLevelMultipliers =
        lazy (
            levelScalingData.Force()
            |> Array.map (fun x -> (x.Level, x.PlayerShieldLevelCo))
            |> Map.ofArray
        )

    let private enemyLevelMultipliers =
        lazy (
            levelScalingData.Force()
            |> Array.map (fun x -> (x.Level, x.ElementLevelCo))
            |> Map.ofArray
        )

    let loaded () =
        levelScalingData.Force().Length > 0

    /// Gets the character elemental coefficient for the provided level. Throws a MappingNotFoundException
    /// if no mapping for the provided level exists. Note that most functions in the Data namespace perform
    /// IO and can, in rare cases, fail with other exceptions.
    let getCharacterLevelMultiplier level =
        let cLevelMult = characterLevelMultipliers.Force()
        match Map.tryFind level cLevelMult with
        | Some x -> x
        | None -> raise (MappingNotFoundException("No mapping found for the provided level.", level))

    /// Gets the character crystallize coefficient for the provided level. Throws a MappingNotFoundException
    /// if no mapping for the provided level exists. Note that most functions in the Data namespace perform
    /// IO and can, in rare cases, fail with other exceptions.
    let getCharacterCrystallizeLevelMultiplier level =
        let cCrystallizeLevelMult = characterCrystallizeLevelMultipliers.Force()
        match Map.tryFind level cCrystallizeLevelMult with
        | Some x -> x
        | None -> raise (MappingNotFoundException("No mapping found for the provided level.", level))

    /// Gets the enemy elemental coefficient for the provided level. Throws a MappingNotFoundException
    /// if no mapping for the provided level exists. Note that most functions in the Data namespace perform
    /// IO and can, in rare cases, fail with other exceptions.
    let getEnemyLevelMultiplier level =
        let eLevelMult = enemyLevelMultipliers.Force()
        match Map.tryFind level eLevelMult with
        | Some x -> x
        | None -> raise (MappingNotFoundException("No mapping found for the provided level.", level))