#light
module Infra.ParamExtractor
open System
open System.Linq
open System.Collections.Generic

let GetImages (inlet : string[]) : string[] =
    inlet
    |> Array.map(fun it -> it.Split ' ')
    |> Array.map(fun box -> box.ElementAt 0)

let GetNoOptions (inlet : string[]) : int[] =
    inlet
    |> Array.map(fun it -> it.Split ' ')
    |> Array.map(fun box -> box.ElementAt 1)
    |> Array.map(fun it -> int it)

let GetCorrectOptions (inlet : string[]) : int[] =
    inlet
    |> Array.map(fun it -> it.Split ' ')
    |> Array.map(fun box -> box.ElementAt 2)
    |> Array.map(fun it -> int it)

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
    let rec loop i top = 
        if i = table.Length
            then top
            else let maybe = int(table.ElementAt(i).ElementAt(0))
                 if maybe > top then loop (i+1) maybe else loop (i+1) top
    loop 1 0

(* Gets the minimum enabled result for a valid test *)
let GetFloorResult (table : string[][]) : int = 
    let rec loop i top = 
        if i = table.Length
            then top
            else let maybe = int(table.ElementAt(i).ElementAt(0))
                 if maybe < top then loop (i+1) maybe else loop (i+1) top
    loop 1 100000

(* Gets the series list from a validity table *)
let GetSeriesList (table : string[][]) : string[] =
    table.ElementAt 0
    |> Array.filter (fun it -> it.Length > 0)

(* Relates the series to the given answers *)
let RelateSeriesAndAnswers (series : string[]) (expected : int[]) (collected : int[]) : Map<string, int> =
    let rec loop (map : Map<string, int>) limit i =
        if i = limit
            then map
            else let current = series.ElementAt i
                 if map.ContainsKey current
                    then let correct = ((expected.ElementAt i) = (collected.ElementAt i))
                         let counting = map.[current] + if correct then 1 else 0
                         loop (map.Add(current, counting)) limit (i+1)
                    else loop (map.Add(current, 1)) limit (i+1)
    loop Map.empty (series.Length) 0

(* Gets the minimum enabled age for a test *)
let GetMinimumAge (percentile : string[][]) : int = 
    percentile.ElementAt(0).ElementAt(1).Split(' ')
    |> Array.map(fun it -> int(it))
    |> Array.min

(* Gets the maximum enabled age for a test *)
let GetMaximumAge (percentile : string[][]) : int = 
    percentile.ElementAt(0)
    |> Array.filter(fun it -> it.Length > 0)
    |> Array.map(fun it -> it.Split(' ')
                           |> Array.map(fun that -> int(that))
                           |> Array.max )
    |> Array.max