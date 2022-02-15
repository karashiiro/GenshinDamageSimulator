namespace GenshinDamageSimulator

open System.Reflection
open System.IO
open Newtonsoft.Json

// https://github.com/Dimbreath/GenshinData/blob/master/ExcelBinOutput/ElementCoeffExcelConfigData.json

module ElementalCoefficients =
    type CoeffDataRow =
        { [<JsonProperty("Level", NullValueHandling = NullValueHandling.Ignore)>]
          Level: uint32
          CrashCo: float32
          ElementLevelCo: float32
          PlayerElementLevelCo: float32
          PlayerShieldLevelCo: float32 }

    let private levelScalingData =
        "GenshinDamageSimulator.ElementCoeffExcelConfigData.json"
        |> Assembly.GetExecutingAssembly().GetManifestResourceStream
        |> fun data -> new StreamReader(data)
        |> fun reader -> reader.ReadToEnd()
        |> JsonConvert.DeserializeObject<CoeffDataRow array>

    let characterLevelMultipliers =
        levelScalingData
        |> Array.map (fun x -> (x.Level, x.PlayerElementLevelCo))
        |> Map.ofArray

    let characterCrystallizeLevelMultipliers =
        levelScalingData
        |> Array.map (fun x -> (x.Level, x.PlayerShieldLevelCo))
        |> Map.ofArray

    let enemyLevelMultipliers =
        levelScalingData
        |> Array.map (fun x -> (x.Level, x.ElementLevelCo))
        |> Map.ofArray