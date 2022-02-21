namespace GenshinDamageSimulator

type GaugeUnits = GaugeUnits of float32

module GaugeUnits =
    let durationSeconds (gu: GaugeUnits) =
        let (GaugeUnits gu') = gu
        2.5f * gu' + 7f

    let decaySeconds (gu: GaugeUnits) =
        let (GaugeUnits gu') = gu
        (durationSeconds gu) / gu'

    let ofDurationSeconds (seconds: float32) =
        seconds / 2.5f - 7f |> GaugeUnits

    let raw (gu: GaugeUnits) =
        let (GaugeUnits gu') = gu
        gu'