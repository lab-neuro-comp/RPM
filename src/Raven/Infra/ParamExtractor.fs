#light
module Infra.ParamExtractor
open System
open System.Linq
open System.Collections.Generic

(* Gets the image key for each question *)
let GetImages (inlet : string[]) : string[] =
    inlet
    |> Array.map(fun it -> it.Split ' ')
    |> Array.map(fun box -> box.ElementAt 0)

(* Gets the number of available options for each question *)
let GetNoOptions (inlet : string[]) : int[] =
    inlet
    |> Array.map(fun it -> it.Split ' ')
    |> Array.map(fun box -> box.ElementAt 1)
    |> Array.map(fun it -> int it)

(* Gets the correct options for each question *)
let GetCorrectOptions (inlet : string[]) : int[] =
    inlet
    |> Array.map(fun it -> it.Split ' ')
    |> Array.map(fun box -> box.ElementAt 2)
    |> Array.map(fun it -> int it)

(* Gets the series of each question *)
let GetSeries (inlet : string[]) : string[] = 
    inlet
    |> Array.map(fun it -> it.Split ' ')
    |> Array.map(fun box -> box.ElementAt 3)

(* Generates a table from a CSV file, separated by comma *)
let GenerateTableFromCsv (inlet : string[]) : string[][] =
    inlet
    |> Array.map(fun box -> box.Split ',')

(** Gets the maximum allowed result for a valid test *)
let GetTopResult (table : string[][]) : int = 
    table
    |> Array.map (fun it -> it.ElementAt 0)
    |> Array.filter (fun it -> it.Length > 0)
    |> Array.map int
    |> Array.max

(* Gets the minimum enabled result for a valid test *)
let GetFloorResult (table : string[][]) : int =
    table
    |> Array.map (fun it -> it.ElementAt 0)
    |> Array.filter (fun it -> it.Length > 0)
    |> Array.map int
    |> Array.min

(* Gets the series list from a validity table *)
let GetSeriesList (table : string[][]) : string[] =
    table.ElementAt 0
    |> Array.filter (fun it -> it.Length > 0)

(* Relates the series to the given answers *)
let RelateSeriesAndAnswers (series : string[]) (expected : int[]) (collected : int[]) : Map<string, int> =
    let limit = series.Length
    let rec loop (map : Map<string, int>) i =
        if i = limit
            then map
            else let current = series.ElementAt i
                 if map.ContainsKey current
                    then let correct = ((expected.ElementAt i) = (collected.ElementAt i))
                         let counting = map.[current] + if correct then 1 else 0
                         loop (map.Add(current, counting)) (i+1)
                    else loop (map.Add(current, 1)) (i+1)
    loop Map.empty 0

(* Gets the minimum enabled age for a test *)
let GetMinimumAge (percentile : string[][]) : int = 
    percentile.ElementAt(0).ElementAt(1).Split(' ')
    |> Array.map (fun it -> int(it))
    |> Array.min

(* Gets the maximum enabled age for a test *)
let GetMaximumAge (percentile : string[][]) : int = 
    percentile.ElementAt(0)
    |> Array.filter (fun it -> it.Length > 0)
    |> Array.map (fun it -> it.Split(' ')
                            |> Array.map (fun that -> int(that))
                            |> Array.max )
    |> Array.max

(* Gets what is expected for each series depending on the number of correct answers *)
let GetExpectedForEachSeries (validities : string[][]) (correct : int) : Map<string, int> =
    let series = validities.[0]
                 |> Array.filter (fun it -> it.Length > 0)
    let possibleLines = validities
                        |> Array.filter (fun it -> it.[0].Length > 0)
                        |> Array.filter (fun it -> int(it.[0]) = correct)
    let outlet = Map.empty
    let rec loop (outlet : Map<string, int>) (A : string[]) (B : string[]) (i : int) =
        if i = A.Length 
            then outlet
            else loop (outlet.Add(A.[i], int(B.[i]))) A B (i+1)
            
    if possibleLines.Length = 0
        then Map.empty
        else loop Map.empty series (possibleLines.[0].Skip(1).ToArray()) 0