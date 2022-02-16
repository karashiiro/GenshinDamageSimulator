namespace GenshinDamageSimulator

open System.Reflection
open System.IO
open Newtonsoft.Json

exception MappingNotFoundException of string

// https://github.com/Dimbreath/GenshinData/blob/master/ExcelBinOutput/ElementCoeffExcelConfigData.json
module ElementalCoefficients =
    type private CoeffDataRow =
        { [<JsonProperty("Level", NullValueHandling = NullValueHandling.Ignore)>]
          Level: uint32
          ElementLevelCo: float32
          PlayerElementLevelCo: float32
          PlayerShieldLevelCo: float32 }

    let private levelScalingData =
        "GenshinDamageSimulator.ElementCoeffExcelConfigData.json"
        |> Assembly.GetExecutingAssembly().GetManifestResourceStream
        |> fun data -> new StreamReader(data)
        |> fun reader -> reader.ReadToEnd()
        |> JsonConvert.DeserializeObject<CoeffDataRow array>

    let private characterLevelMultipliers =
        levelScalingData
        |> Array.map (fun x -> (x.Level, x.PlayerElementLevelCo))
        |> Map.ofArray

    let private characterCrystallizeLevelMultipliers =
        levelScalingData
        |> Array.map (fun x -> (x.Level, x.PlayerShieldLevelCo))
        |> Map.ofArray

    let private enemyLevelMultipliers =
        levelScalingData
        |> Array.map (fun x -> (x.Level, x.ElementLevelCo))
        |> Map.ofArray

    let getCharacterLevelMultiplier level =
        match Map.tryFind level characterLevelMultipliers with
        | Some x -> x
        | None -> raise (MappingNotFoundException($"No mapping found for level {level}"))

    let getCharacterCrystallizeLevelMultiplier level =
        match Map.tryFind level characterCrystallizeLevelMultipliers with
        | Some x -> x
        | None -> raise (MappingNotFoundException($"No mapping found for level {level}"))

    let getEnemyLevelMultiplier level =
        match Map.tryFind level enemyLevelMultipliers with
        | Some x -> x
        | None -> raise (MappingNotFoundException($"No mapping found for level {level}"))