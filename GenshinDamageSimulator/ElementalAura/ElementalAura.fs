namespace GenshinDamageSimulator

// https://docs.google.com/document/d/e/2PACX-1vSEovpheHaeum4Ba0MlNdfyOTsJ-wZzDmBof13bVztYtKDi6OQhLqNdwEkEApo6vvtAV0L_tMal2ZTN/pub#h.6crtulvx1dlt

type ElementalAuraData =
    { Element: Element
      ApplicationSkillId: uint32 // Used for comparing ICDs
      ApplicationSkillIcdMs: float32
      ElementalMastery: float32
      Gauge: Gauge
      Permanent: bool }

type ElementalAura =
    | PyroAura of ElementalAuraData
    | HydroAura of ElementalAuraData
    | ElectroAura of ElementalAuraData
    | CryoAura of ElementalAuraData
    | AnemoAura of ElementalAuraData
    | GeoAura of ElementalAuraData
    | DendroAura of ElementalAuraData

module ElementalAura =
    let element aura =
        match aura with
        | PyroAura _ -> Element.Pyro
        | HydroAura _ -> Element.Hydro
        | ElectroAura _ -> Element.Electro
        | CryoAura _ -> Element.Cryo
        | AnemoAura _ -> Element.Anemo
        | GeoAura _ -> Element.Geo
        | DendroAura _ -> Element.Dendro

    let wrap aura =
        match aura.Element with
        | Element.Pyro -> PyroAura(aura)
        | Element.Hydro -> HydroAura(aura)
        | Element.Electro -> ElectroAura(aura)
        | Element.Cryo -> CryoAura(aura)
        | Element.Anemo -> AnemoAura(aura)
        | Element.Geo -> GeoAura(aura)
        | Element.Dendro -> DendroAura(aura)

    let unwrap aura =
        match aura with
        | PyroAura a -> a
        | HydroAura a -> a
        | ElectroAura a -> a
        | CryoAura a -> a
        | AnemoAura a -> a
        | GeoAura a -> a
        | DendroAura a -> a

    let gauge aura =
        (aura |> unwrap).Gauge

    let isExpired aura =
        let data = (aura |> unwrap)
        not (data.Permanent) && (aura |> unwrap).Gauge |> Gauge.isEmpty

    let decay s aura =
        aura
        |> unwrap
        |> fun ad -> { ad with Gauge = ad.Gauge |> Gauge.decay s }
        |> wrap

    let oldIfPermanent aura trigger =
        if aura.Permanent then aura else trigger
        |> wrap
        |> Seq.singleton

    let reactionModifier reaction =
        match reaction with
        | StrongMelt | StrongVaporize -> 2f
        | Crystallize | Swirl | WeakMelt | WeakVaporize -> 0.5f
        | Overload | Superconduct -> 1f
        | _ -> 0f

    /// Resolves a gauge interaction when the result can have both the aura and trigger
    /// simultaneously (Electro-Charged, Burning).
    let resolveGaugeDouble aura trigger =
        // TODO: For now, I'm assuming the reaction modifier for these is 1x since it doesn't seem to
        // be stated on KQM. I should test this in-game.
        let newAura = wrap { trigger with Gauge = trigger.Gauge |> Gauge.tax }
        if aura.Permanent then
            [| wrap aura; newAura |]
        else
            let auraGauge = aura.Gauge - trigger.Gauge
            if auraGauge |> Gauge.isEmpty then
                [| newAura |]
            else
                [| wrap { aura with Gauge = auraGauge }; newAura |]

    let resolveGaugeAdd aura trigger =
        if aura.Permanent then
            Seq.singleton (wrap aura)
        elif trigger.Permanent then
            Seq.singleton (wrap trigger)
        else
            let newGauge = aura.Gauge + (Gauge.tax trigger.Gauge)
            if newGauge |> Gauge.isEmpty then Seq.empty else Seq.singleton (wrap { trigger with Gauge = newGauge })

    let resolveGauge aura trigger reaction =
        if aura.Permanent then
            Seq.singleton (wrap aura)
        elif trigger.Permanent then
            Seq.singleton (wrap trigger)
        else
            let newGauge = aura.Gauge - reactionModifier reaction * trigger.Gauge
            if newGauge |> Gauge.isEmpty then Seq.empty else Seq.singleton (wrap { aura with Gauge = newGauge })

    let interact aura trigger =
        match (aura, trigger) with
        // Same-element triggers
        | (PyroAura pOld, PyroAura pNew) -> resolveGaugeAdd pOld pNew, None
        | (HydroAura hOld, HydroAura hNew) -> resolveGaugeAdd hOld hNew, None
        | (ElectroAura eOld, ElectroAura eNew) -> resolveGaugeAdd eOld eNew, None
        | (CryoAura cOld, CryoAura cNew) -> resolveGaugeAdd cOld cNew, None
        | (AnemoAura aOld, AnemoAura aNew) -> resolveGaugeAdd aOld aNew, None
        | (GeoAura gOld, GeoAura gNew) -> resolveGaugeAdd gOld gNew, None
        | (DendroAura dOld, DendroAura dNew) -> resolveGaugeAdd dOld dNew, None
        // Pyro aura reactions
        | (PyroAura p, HydroAura h) -> resolveGauge p h StrongVaporize, Some(StrongVaporize)
        | (PyroAura p, ElectroAura e) -> resolveGauge p e Overload, Some(Overload)
        | (PyroAura p, CryoAura c) -> resolveGauge p c WeakMelt, Some(WeakMelt)
        | (PyroAura p, AnemoAura a) -> resolveGauge p a Swirl, Some(Swirl) // TODO multiple enemies
        | (PyroAura p, GeoAura g) -> resolveGauge p g Crystallize, Some(Crystallize)
        | (PyroAura p, DendroAura d) -> resolveGaugeDouble p d, Some(Burning 0.25f)
        // Hydro aura reactions
        | (HydroAura h, PyroAura p) -> resolveGauge h p WeakVaporize, Some(WeakVaporize)
        | (HydroAura h, ElectroAura e) -> resolveGaugeDouble h e, Some(ElectroCharged (ElectroCharged.wrap 1f e.ElementalMastery))
        | (HydroAura _, CryoAura c) -> Seq.singleton (CryoAura(c)), Some(Frozen) // TODO
        | (HydroAura h, AnemoAura a) -> resolveGauge h a Swirl, Some(Swirl) // TODO multiple enemies
        | (HydroAura h, GeoAura g) -> resolveGauge h g Crystallize, Some(Crystallize)
        | (HydroAura h, DendroAura d) -> [| wrap h; wrap { d with Gauge = d.Gauge |> Gauge.tax } |], None
        // Electro aura reactions
        | (ElectroAura e, HydroAura h) -> resolveGaugeDouble e h, Some(ElectroCharged (ElectroCharged.wrap 1f h.ElementalMastery))
        | (ElectroAura e, PyroAura p) -> resolveGauge e p Overload, Some(Overload)
        | (ElectroAura e, CryoAura c) -> resolveGauge e c Superconduct, Some(Superconduct)
        | (ElectroAura e, AnemoAura a) -> resolveGauge e a Swirl, Some(Swirl) // TODO multiple enemies
        | (ElectroAura e, GeoAura g) -> resolveGauge e g Crystallize, Some(Crystallize)
        | (ElectroAura e, DendroAura d) -> [| wrap e; wrap { d with Gauge = d.Gauge |> Gauge.tax } |], None
        // Cryo aura reactions
        | (CryoAura _, HydroAura h) -> Seq.singleton (HydroAura(h)), Some(Frozen) // TODO
        | (CryoAura c, PyroAura p) -> resolveGauge c p StrongMelt, Some(StrongMelt)
        | (CryoAura c, ElectroAura e) -> resolveGauge c e Superconduct, Some(Superconduct)
        | (CryoAura c, AnemoAura a) -> resolveGauge c a Swirl, Some(Swirl) // TODO multiple enemies
        | (CryoAura c, GeoAura g) -> resolveGauge c g Crystallize, Some(Crystallize)
        | (CryoAura c, DendroAura d) -> [| wrap c; wrap { d with Gauge = d.Gauge |> Gauge.tax } |], None
        // Anemo aura reactions
        | (AnemoAura a, PyroAura p) -> resolveGauge a p Swirl, Some(Swirl) // TODO multiple enemies
        | (AnemoAura a, HydroAura h) -> resolveGauge a h Swirl, Some(Swirl) // TODO multiple enemies
        | (AnemoAura a, ElectroAura e) -> resolveGauge a e Swirl, Some(Swirl) // TODO multiple enemies
        | (AnemoAura a, CryoAura c) -> resolveGauge a c Swirl, Some(Swirl) // TODO multiple enemies
        | (AnemoAura a, GeoAura _) -> [| wrap a |], None
        | (AnemoAura a, DendroAura d) -> [| wrap a; wrap { d with Gauge = d.Gauge |> Gauge.tax } |], None
        // Geo aura reactions
        | (GeoAura g, PyroAura p) -> resolveGauge g p Crystallize, Some(Crystallize)
        | (GeoAura g, HydroAura h) -> resolveGauge g h Crystallize, Some(Crystallize)
        | (GeoAura g, ElectroAura e) -> resolveGauge g e Crystallize, Some(Crystallize)
        | (GeoAura g, CryoAura c) -> resolveGauge g c Crystallize, Some(Crystallize)
        | (GeoAura g, AnemoAura _) -> [| wrap g |], None
        | (GeoAura g, DendroAura d) -> [| wrap g; wrap { d with Gauge = d.Gauge |> Gauge.tax } |], None
        // Dendro aura reactions
        | (DendroAura d, PyroAura p) -> resolveGaugeDouble d p, Some(Burning 0.25f)
        | (DendroAura d, HydroAura h) -> [| wrap d; wrap { h with Gauge = h.Gauge |> Gauge.tax } |], None
        | (DendroAura d, ElectroAura e) -> [| wrap d; wrap { e with Gauge = e.Gauge |> Gauge.tax } |], None
        | (DendroAura d, CryoAura c) -> [| wrap d; wrap { c with Gauge = c.Gauge |> Gauge.tax } |], None
        | (DendroAura d, AnemoAura _) -> [| wrap d |], None
        | (DendroAura d, GeoAura _) -> [| wrap d |], None