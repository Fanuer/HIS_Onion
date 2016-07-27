using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using HIS.WebApi.Auth.Base.Models;
using HIS.WebApi.Auth.Base.Repositories;
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

      this.MultipleObjects = new List<RefreshToken>()
      {
        first,
        second,
        third
      };
      

      this.DbSet = new Mock<DbSet<RefreshToken>>();

      DbSet.As<IQueryable<RefreshToken>>().Setup(m => m.ElementType).Returns(MultipleObjects.AsQueryable().ElementType);
      DbSet.As<IQueryable<RefreshToken>>().Setup(m => m.Expression).Returns(MultipleObjects.AsQueryable().Expression);
      DbSet.As<IQueryable<RefreshToken>>().Setup(m => m.Provider).Returns(MultipleObjects.AsQueryable().Provider);
      DbSet.As<IQueryable<RefreshToken>>().Setup(m => m.GetEnumerator()).Returns(MultipleObjects.GetEnumerator());

      this.Context = new Mock<BearerDbContext>();
      this.Context.Setup(m => m.RefreshTokens).Returns(this.DbSet.Object);

    }

    [TestMethod]
    public async Task TestExists()
    {
      using (var rep = new BearerUserRepository<string, int>(this.Context.Object))
      {

      }
    }

    [TestMethod]
    public async Task AddExistDeleteToken()
    {
      using (var rep = new BearerUserRepository<string, int>(this.Context.Object))
      {
        try
        {
          var exists = await rep.RefreshTokens.ExistsAsync(this.SingleObject.Id);
          Assert.IsFalse(exists);

          var add = await rep.RefreshTokens.AddAsync(this.SingleObject);
          Assert.IsTrue(add);
          this.DbSet.Verify(m => m.Add(It.IsAny<RefreshToken>()), Times.Once());
          this.Context.Verify(m => m.SaveChanges(), Times.Once());

          exists = await rep.RefreshTokens.ExistsAsync(this.SingleObject.Id);
          Assert.IsTrue(exists);
        }
        finally
        {
          if (await rep.RefreshTokens.ExistsAsync(this.SingleObject.Id))
          {
            await rep.RefreshTokens.RemoveAsync(this.SingleObject.Id);
          }
        }
      }
    }

   
    #endregion

    #region PROPERTIES

    private Mock<BearerDbContext> Context { get; set; }
    private Mock<DbSet<RefreshToken>> DbSet { get; set; }

    private RefreshToken SingleObject { get; set; }

    private List<RefreshToken> MultipleObjects { get; set; }

    #endregion

  }
}
