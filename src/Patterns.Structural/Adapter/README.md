# Adapter

**Relevance:** ⭐⭐⭐ Essential. Every vendor SDK, legacy system or external API you wrap behind your own interface is an adapter.

**Intent:** convert the interface of a class into the interface its clients expect, so classes that could not work together can.

**Read first:** [`1-Classic/LegacyCarrierAdapter.cs`](1-Classic/LegacyCarrierAdapter.cs), with the carrier's SDK it adapts, [`External/LegacyCarrierClient.cs`](External/LegacyCarrierClient.cs).

| Level | What it shows |
|---|---|
| [`External/`](External/) | The carrier's SDK (simulated): "someone else's code" with a text protocol, shared by every level. |
| [`0-Problem/`](0-Problem/) | `CheckoutSummary` and `OrderTracking` each build the payload and parse the answer: two copies of the carrier's format. |
| [`1-Classic/`](1-Classic/) | `LegacyCarrierAdapter` implements our `IShippingQuoteProvider`; `CheckoutSummary` only knows that interface. |
| [`2-DotNet/`](2-DotNet/) | `StreamReader` adapts a `Stream` of bytes to lines of text (`CarrierRateFile`). |

**Run it:** `dotnet run --project src/Patterns.Runner -- adapter`

**Tests:** `dotnet test --project tests/Patterns.Structural.Tests --filter-class "*.AdapterTests"`

**Guide:** [§5.1 Adapter](../../../docs/DESIGN_PATTERNS_GUIDE.md#51-adapter)
