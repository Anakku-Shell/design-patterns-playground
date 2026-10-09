# Proxy

**Relevance:** ⭐⭐ Useful. `Lazy<T>`, EF Core lazy loading, mocking libraries and generated API clients are proxies.

**Intent:** provide a stand-in for another object to control access to it.

**Read first:** [`1-Classic/ProductImages.cs`](1-Classic/ProductImages.cs) (a virtual proxy) and [`1-Classic/PriceEditors.cs`](1-Classic/PriceEditors.cs) (a protection proxy).

| Level | What it shows |
|---|---|
| [`Common/`](Common/) | `ImageStore` (counts its reads) and `User`, shared by every level. |
| [`0-Problem/`](0-Problem/) | `ProductPage` reads every image up front; `PriceAdminPage` copies the admin check into each method. |
| [`1-Classic/`](1-Classic/) | `LazyImageProxy` reads on first use; `AdminOnlyPriceEditor` guards a `PriceEditor` that has no security code. |
| [`2-DotNet/`](2-DotNet/) | `Lazy<byte[]>` (`ProductImage`) and a `DispatchProxy` that logs every call to any interface (`LoggingProxy`). |

**Run it:** `dotnet run --project src/Patterns.Runner -- proxy`

**Tests:** `dotnet test --project tests/Patterns.Structural.Tests --filter-class "*.ProxyTests"`

**Guide:** [§5.7 Proxy](../../../docs/DESIGN_PATTERNS_GUIDE.md#57-proxy)
