using Patterns.Modern.Result.Classic;
using Patterns.Shop;
using Xunit;
using Classic = Patterns.Modern.Result.Classic;
using DotNet = Patterns.Modern.Result.DotNet;
using Problem = Patterns.Modern.Result.Problem;

namespace Patterns.Modern.Tests;

public sealed class ResultTests
{
    [Fact]
    public void AllLevels_ReserveTwoBooks()
    {
        var problem = new Problem.StockService().Reserve(SampleData.Book, 2);
        var classic = Classic.StockService.Reserve(SampleData.Book, 2);
        var dotnet = DotNet.StockService.TryReserve(SampleData.Book, 2, out var reservation);

        Assert.Equal((SampleData.Book.Id, 2), (problem.ProductId, problem.Quantity));
        Assert.True(classic.IsSuccess);
        Assert.Equal((SampleData.Book.Id, 2), (classic.Value.ProductId, classic.Value.Quantity));
        Assert.True(dotnet);
        Assert.Equal((SampleData.Book.Id, 2), (reservation!.ProductId, reservation.Quantity));
    }

    [Fact]
    public void AllLevels_RefuseTwentyOneBooks()
    {
        var problem = Assert.Throws<Problem.InsufficientStockException>(() => new Problem.StockService().Reserve(SampleData.Book, 21));
        var classic = Classic.StockService.Reserve(SampleData.Book, 21);
        var dotnet = DotNet.StockService.TryReserve(SampleData.Book, 21, out var reservation);

        Assert.Equal("Only 20 left of Clean Code.", problem.Message);
        Assert.False(classic.IsSuccess);
        Assert.False(dotnet);
        Assert.Null(reservation);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void AllLevels_RefuseAQuantityBelowOne(int quantity)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Problem.StockService().Reserve(SampleData.Book, quantity));
        Assert.Equal("quantity.invalid", Classic.StockService.Reserve(SampleData.Book, quantity).Error.Code);
        Assert.False(DotNet.StockService.TryReserve(SampleData.Book, quantity, out _));
    }

    [Fact]
    public void Error_HasCodeAndMessage()
    {
        Assert.Equal(new BusinessError("stock.insufficient", "Only 20 left of Clean Code."), Classic.StockService.Reserve(SampleData.Book, 21).Error);
        Assert.Equal(new BusinessError("quantity.invalid", "Quantity must be at least 1."), Classic.StockService.Reserve(SampleData.Book, 0).Error);
    }

    [Fact]
    public void Value_OnFailure_Throws()
    {
        var failed = Classic.StockService.Reserve(SampleData.Book, 21);

        var error = Assert.Throws<InvalidOperationException>(() => failed.Value);

        Assert.Equal("A failed result has no value.", error.Message);
    }

    [Fact]
    public void Error_OnSuccess_Throws()
    {
        var reserved = Classic.StockService.Reserve(SampleData.Book, 2);

        var error = Assert.Throws<InvalidOperationException>(() => reserved.Error);

        Assert.Equal("A successful result has no error.", error.Message);
    }

    [Fact]
    public void Bind_StopsAtFirstFailure()
    {
        var charged = 0;
        Result<string> Charge(Reservation r)
        {
            charged++;
            return Classic.Result.Success($"charged {r.Quantity}");
        }

        var failed = Classic.StockService.Reserve(SampleData.Book, 21).Bind(Charge);
        var succeeded = Classic.StockService.Reserve(SampleData.Book, 2).Bind(Charge);

        Assert.Equal("stock.insufficient", failed.Error.Code);
        Assert.Equal("charged 2", succeeded.Value);
        Assert.Equal(1, charged); // the failed reservation never reached Charge
    }

    [Fact]
    public void Map_And_Match()
    {
        Assert.Equal(2, Classic.StockService.Reserve(SampleData.Book, 2).Map(r => r.Quantity).Value);
        Assert.Equal("stock.insufficient", Classic.StockService.Reserve(SampleData.Book, 21).Map(r => r.Quantity).Error.Code);

        Assert.Equal("Reserved 2", Classic.StockService.Reserve(SampleData.Book, 2).Match(r => $"Reserved {r.Quantity}", e => e.Message));
        Assert.Equal("Only 20 left of Clean Code.", Classic.StockService.Reserve(SampleData.Book, 21).Match(r => $"Reserved {r.Quantity}", e => e.Message));
    }
}
