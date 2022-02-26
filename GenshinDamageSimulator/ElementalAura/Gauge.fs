namespace GenshinDamageSimulator

// https://library.keqingmains.com/combat-mechanics/elemental-effects/elemental-gauge-theory
type Gauge = Gauge of float32 * float32

module Gauge =
    /// Unwraps the provided gauge into its constituent values.
    let unwrap g =
        let (Gauge (gu, dr)) = g
        gu, dr

    /// Wraps the provided values in a gauge.
    let wrap eu dr =
        (eu, dr) |> Gauge

    /// Gauge addition. Adding two gauges replaces the aura elemental unit value and maintains the decay rate.
    let add ag tg =
        let _, dr1 = ag |> unwrap
        let eu2, _ = tg |> unwrap
        (eu2, dr1) ||> wrap

    /// Gauge subtraction. Subtracting two gauges subtracts the trigger elemental unit value from the
    /// aura elemental unit value and maintains the decay rate. The result of the subtraction may not
    /// go below zero.
    let sub ag tg =
        let eu1, dr1 = ag |> unwrap
        let eu2, _ = tg |> unwrap
        (max 0f (eu1 - eu2), dr1) ||> wrap

    /// Scalar gauge subtraction. This operation does not modify the decay rate.
    let subs m g =
        let eu, dr = g |> unwrap
        (max 0f (eu - m), dr) ||> wrap

    /// Scalar gauge multiplication. This operation does not modify the decay rate.
    let muls m g =
        let eu, dr = g |> unwrap
        (eu * m, dr) ||> wrap

    /// Returns the elmental units for the provided gauge.
    let eu g =
        let eu, _ = g |> unwrap
        eu

    /// Returns the decay rate for the provided gauge.
    let dr g =
        let _, dr = g |> unwrap
        dr

    /// Returns the remaining duration for the provided gauge.
    let duration g =
        let eu, dr = g |> unwrap
        eu * dr

    /// Returns true if the provided gauge has an elemental unit value of less than or equal to 0.
    let isEmpty g =
        let eu, _ = g |> unwrap
        eu <= 0f

    /// Returns the initial duration for the provided raw elemental units, before the aura tax is applied.
    let initialDuration e =
        2.5f * e + 7f

    /// Returns the initial decay rate for the provided raw elemental units.
    let initialDecay e =
        (initialDuration e) / (e * 0.8f)

    /// Returns the gauge for the provided elemental unit value.
    let ofUnits e =
        (e, initialDecay e) ||> wrap

    /// Applies the aura tax to this gauge.
    let tax =
        muls 0.8f

    /// Decays the provided gauge over the specified time in seconds.
    let decay s g =
        let _, dr = g |> unwrap
        subs (s / dr) g

type Gauge with
    /// Gauge addition. Adding two gauges replaces the aura elemental unit value and maintains the decay rate.
    static member (+) (ag, tg) = Gauge.add ag tg

    /// Gauge subtraction. Subtracting two gauges subtracts the trigger elemental unit value from the
    /// aura elemental unit value and maintains the decay rate. The result of the subtraction may not
    /// go below zero.
    static member (-) (ag, tg) = Gauge.sub ag tg

    /// Scalar gauge subtraction. This operation does not modify the decay rate.
    static member (-) (g, m) = Gauge.subs m g

    /// Scalar gauge multiplication. This operation does not modify the decay rate.
    static member (*) (g, m) = Gauge.muls m g

    /// Scalar gauge multiplication. This operation does not modify the decay rate.
    static member (*) (m, g) = Gauge.muls m g