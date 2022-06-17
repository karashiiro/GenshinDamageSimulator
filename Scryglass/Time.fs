namespace Scryglass

open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames

[<AutoOpen>]
module Time =
    [<Measure>] type Frame

    let framesPerSecond = 60.0<Frame/second>
    let convertFramesToSeconds (x: float<Frame>) = x / framesPerSecond
    
    type Hitlag = float<second> * float // Real time and the slowdown factor

    // Output will be less than input; time-based effects will consume less GU/time
    type ApplyHitlag = float<second> -> Hitlag -> float<second>