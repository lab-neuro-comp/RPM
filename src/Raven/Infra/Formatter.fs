#light
module Infra.Formatter
open System
open System.Linq
open System.Collections.Generic

let Format (title: string) (stuff: Dictionary<string, string[]>): string[] =
    // n: Name
    // a: Age
    // s: Initial moment
    // p: Percentile
    // c: Correct Answers
    // i: Incorrect answers
    // x: Expected answers
    // a: Answers
    // T: Times
    // v: validity
    let inlet = [|
        stuff.["name"]
        stuff.["age"]
        stuff.["initial"]
        stuff.["percentile"]
        stuff.["correct"]
        stuff.["incorrect"]
        stuff.["expected"]
        stuff.["answers"]
        stuff.["times"]
        stuff.["validity"]
    |]
    let limit = 
        inlet
        |> Array.map(fun it -> it.Length)
        |> Array.min
    let build (i: int): string = 
        inlet
        |> Array.map(fun it -> it.ElementAt i)
        |> Array.reduce(fun box -> (fun it -> String.Format("{0};{1}", box, it)))
    let rec loop (box: string[]) (i: int): string[] =
        if i < limit
            then loop (Array.append box [|(build i)|]) (i+1)
            else box
    let start = [|title|]
    loop start 0