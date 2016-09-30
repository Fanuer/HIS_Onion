using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using HIS.Testing.Base;
using HIS.WebApi.Auth.Base.Repositories;
using HIS.WebApi.Auth.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HIS.WebApi.Auth.Base.Test.Tests
{
  [TestClass]
  public class RefreshTokenTests
  {
    #region FIELDS
    #endregion

    #region CTOR
    #endregion

    #region METHODS

    [TestInitialize]
    public void Init()
    {
      this.SingleObject = new RefreshToken()
      {
        ClientId = "Test",
        ExpiresUtc = DateTime.UtcNow.AddMonths(6),
        IssuedUtc = DateTime.UtcNow.AddMinutes(-6),
        Id = "1",
        Subject = "TestUser",
        ProtectedTicket = ""
      };


      var first = (RefreshToken) this.SingleObject.Clone();
      var second = (RefreshToken) this.SingleObject.Clone();
      second.Id = "2";
      var third = (RefreshToken) this.SingleObject.Clone();
      third.Id = "3";

      this.MultipleObjects = new ObservableCollection<RefreshToken>()
      {
        first,
        second,
        third
      };

      this.DbSet = new Mock<DbSet<RefreshToken>>();
      DbSet.As<IQueryable<RefreshToken>>().Setup(m => m.ElementType).Returns(MultipleObjects.AsQueryable().ElementType);
      DbSet.As<IQueryable<RefreshToken>>().Setup(m => m.Expression).Returns(MultipleObjects.AsQueryable().Expression);
      DbSet.As<IQueryable<RefreshToken>>().Setup(m => m.GetEnumerator()).Returns(MultipleObjects.GetEnumerator());
      DbSet.Setup(t => t.FindAsync(It.IsAny<string>())).ReturnsAsync(this.SingleObject);

      DbSet.As<IDbAsyncEnumerable<RefreshToken>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<RefreshToken>(MultipleObjects.GetEnumerator()));

      DbSet.As<IQueryable<RefreshToken>>()
          .Setup(m => m.Provider)
          .Returns(new TestDbAsyncQueryProvider<RefreshToken>(MultipleObjects.AsQueryable().Provider));

      this.Context = new Mock<BearerDbContext>();
      this.Context.Setup(m => m.RefreshTokens).Returns(this.DbSet.Object);
      this.Context.Setup(m => m.Set<RefreshToken>()).Returns(this.DbSet.Object);
      this.Context.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1).Verifiable();
    }

    [TestMethod]
    public async Task TestFindOne()
    {
      using (var rep = new BearerUserRepository<IUser<string>>(this.Context.Object))
      {
        var element= await rep.RefreshTokens.FindAsync(this.SingleObject.Id);
        Assert.IsNotNull(element);
        Assert.AreEqual(element, this.SingleObject);
      }
    }

    [TestMethod]
    public async Task TestFindAll()
    {
      using (var rep = new BearerUserRepository<User>(this.Context.Object))
      {
        var elements = await rep.RefreshTokens.GetAllAsync();
        Assert.IsNotNull(elements);
        Assert.AreEqual(elements.Count(), MultipleObjects.Count);
      }
    }

    [TestMethod]
    public async Task TestExists()
    {
      using (var rep = new BearerUserRepository<User>(this.Context.Object))
      {
        var exists = await rep.RefreshTokens.ExistsAsync("3");
        Assert.IsTrue(exists);
        exists = await rep.RefreshTokens.ExistsAsync("4");
        Assert.IsFalse(exists);
      }
    }

    [TestMethod]
    public async Task TestAdd()
    {
      using (var rep = new BearerUserRepository<User>(this.Context.Object))
      {
        var newRT = (RefreshToken) this.SingleObject.Clone();
        newRT.Id = "4";
        var addedSuccessfully = await rep.RefreshTokens.AddAsync(newRT);

        this.DbSet.Verify(m => m.Add(It.IsAny<RefreshToken>()), Times.Once());
        try
        {
          this.Context.Verify(m => m.SaveChangesAsync(), Times.Once());
        }
        catch (Exception e)
        {
          
          throw e;
        }
        
        Assert.IsTrue(addedSuccessfully);
      }
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public async Task TestAdd_NullParameterCallCausesExeption()
    {
      using (var rep = new BearerUserRepository<User>(this.Context.Object))
      { 
        var addedSuccessfully = await rep.RefreshTokens.AddAsync(null);
        Assert.Fail();
      }
    }

    [TestMethod]
    public async Task TestRemove()
    {
      using (var rep = new BearerUserRepository<User>(this.Context.Object))
      {
        var removedSuccessfully = await rep.RefreshTokens.RemoveAsync(this.SingleObject);

        this.DbSet.Verify(m => m.Remove(It.IsAny<RefreshToken>()), Times.Once());
        this.Context.Verify(m => m.SaveChangesAsync(), Times.Once());
        Assert.IsTrue(removedSuccessfully);
      }
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public async Task TestRemove_NullParameterCallCausesExeption()
    {
      using (var rep = new BearerUserRepository<User>(this.Context.Object))
      {
        RefreshToken removeObject = null;
        await rep.RefreshTokens.RemoveAsync(removeObject);
        Assert.Fail();
      }
    }

    #endregion

    #region PROPERTIES

    private Mock<BearerDbContext> Context { get; set; }
    private Mock<DbSet<RefreshToken>> DbSet { get; set; }

    private RefreshToken SingleObject { get; set; }

    private ObservableCollection<RefreshToken> MultipleObjects { get; set; }

    #endregion

  }
}
