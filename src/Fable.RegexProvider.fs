namespace Fable.Core

type EmitAttribute(macro: string) =
    inherit System.Attribute()

namespace Fable

module RegexProvider =

    open System.Text.RegularExpressions
    open FSharp.Quotations
    open FSharp.Core.CompilerServices
    open ProviderImplementation.ProvidedTypes

    [<TypeProvider>]
    type RegexProvider (config : TypeProviderConfig) as this =
        inherit TypeProviderForNamespaces(config)
        let asm = System.Reflection.Assembly.GetExecutingAssembly()
        let ns = "Fable.RegexProvider"

        let createTypes () =
            let newType = ProvidedTypeDefinition(asm, ns, "SafeRegex", Some typeof<obj>)
            let staticParams = [
                ProvidedStaticParameter("pattern", typeof<string>)
                ProvidedStaticParameter("ignoreCase", typeof<bool>, parameterDefaultValue=false)
                ProvidedStaticParameter("multiline", typeof<bool>, parameterDefaultValue=false)
            ]
            let methWithStaticParams =
                // ProvidedMethod(name, args, makeType typ, isStatic = isStatic, invokeCode = body)
                let m = ProvidedMethod("Create", [], typeof<Regex>, isStatic = true)
                m.DefineStaticParameters(staticParams, (fun nm args ->
                    let pattern = args.[0] :?> string
                    try
                        Regex(pattern, RegexOptions.ECMAScript) |> ignore
                    with
                    | _ -> failwith "Cannot compile regular expression"
                    let ignoreCase, multiline = args.[1] :?> bool, args.[2] :?> bool
                    let m2Body =
                        fun _ ->
                        <@@
                            let opts = if ignoreCase then RegexOptions.IgnoreCase else RegexOptions.None
                            let opts = if multiline then opts ||| RegexOptions.Multiline else opts
                            Regex(pattern, opts)
                        @@>
                    let m2 = ProvidedMethod(nm, [], typeof<Regex>, isStatic = true, invokeCode = m2Body)
                    newType.AddMember m2
                    m2))
                m
            newType.AddMember(methWithStaticParams)
            [newType]

        do this.AddNamespace(ns, createTypes())

    [<assembly:TypeProviderAssembly>]
    do ()