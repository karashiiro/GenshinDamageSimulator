namespace GenshinDamageSimulator

type GaugeUnits = GaugeUnits of float32

module GaugeUnits =
    let unwrap (gu: GaugeUnits) =
        let (GaugeUnits gu') = gu
        gu'

    let wrap (gu: float32) =
        gu |> GaugeUnits

    let durationSeconds (gu: GaugeUnits) =
        2.5f * unwrap gu + 7f

    let decaySeconds (gu: GaugeUnits) =
        (durationSeconds gu) / unwrap gu

    let ofDurationSeconds (seconds: float32) =
        seconds / 2.5f - 7f |> wrap