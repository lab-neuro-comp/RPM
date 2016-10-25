#light
module Infra.Formatter
open System
open System.Linq

let Format (P: string) (C: string) (S: string) (XS : string[]) (AS: string[]) (TS: string[]): string[] =
    // p: Percentile
    // c: Correct Answers
    // s: Initial moment
    // x: Expected answers
    // a: Answers
    // T: Times
    let limit = Array.length AS
    let build i =
        String.Format("{0};{1};{2};{3};{4};{5}", 
                      S, C, P, (XS.ElementAt i), (AS.ElementAt i), (TS.ElementAt i))
    let rec loop box i =
        if i < limit
            then loop (Array.append box [|(build i)|]) (i+1)
            else box
    let start = [|"Começo;# Respostas Corretas;Percentil;Resposta Esperada;Resposta Dada;Tempo"|]
    loop start 0