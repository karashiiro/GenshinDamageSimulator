namespace Scryglass

module Time =
    type Seconds = float
    type Hitlag = Seconds * float // Real time and the slowdown factor
    
    // Output will be less than input; time-based effects will consume less GU/time
    type ApplyHitlag = Seconds -> Hitlag -> Seconds