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
  public class ClientTests
  {
    #region FIELDS
    #endregion

    #region CTOR
    #endregion

    #region METHODS

    [TestInitialize]
    public void Init()
    {
      this.SingleObject = new Client()
      {
        Id = "1",
        Active = true,
        Name = "",
        AllowedOrigin = "*",
        Secret = ""
      };


      var first = (Client) this.SingleObject.Clone();
      var second = (Client) this.SingleObject.Clone();
      second.Id = "2";
      var third = (Client) this.SingleObject.Clone();
      third.Id = "3";

      this.MultipleObjects = new ObservableCollection<Client>()
      {
        first,
        second,
        third
      };

      this.DbSet = new Mock<DbSet<Client>>();
      DbSet.As<IQueryable<Client>>().Setup(m => m.ElementType).Returns(MultipleObjects.AsQueryable().ElementType);
      DbSet.As<IQueryable<Client>>().Setup(m => m.Expression).Returns(MultipleObjects.AsQueryable().Expression);
      DbSet.As<IQueryable<Client>>().Setup(m => m.GetEnumerator()).Returns(MultipleObjects.GetEnumerator());
      DbSet.Setup(t => t.FindAsync(It.IsAny<string>())).ReturnsAsync(this.SingleObject);

      DbSet.As<IDbAsyncEnumerable<Client>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<Client>(MultipleObjects.GetEnumerator()));

      DbSet.As<IQueryable<Client>>()
          .Setup(m => m.Provider)
          .Returns(new TestDbAsyncQueryProvider<Client>(MultipleObjects.AsQueryable().Provider));

      this.Context = new Mock<BearerDbContext>();
      //this.Context.Setup(m => m.Clients).Returns(this.DbSet.Object);
      this.Context.Setup(m => m.Set<Client>()).Returns(this.DbSet.Object);
      this.Context.Setup(c => c.SaveChangesAsync()).Returns(() => Task.Run(() => 1)).Verifiable();
    }

    [TestMethod]
    public async Task TestFindOne()
    {
      using (var rep = new BearerUserRepository<User>(this.Context.Object))
      {
        var element= await rep.Clients.FindAsync(this.SingleObject.Id);
        Assert.IsNotNull(element);
        Assert.AreEqual(element, this.SingleObject);
      }
    }

    [TestMethod]
    public async Task TestFindAll()
    {
      using (var rep = new BearerUserRepository<User>(this.Context.Object))
      {
        var elements = await rep.Clients.GetAllAsync();
        Assert.IsNotNull(elements);
        Assert.AreEqual(elements.Count(), MultipleObjects.Count);
      }
    }

    [TestMethod]
    public async Task TestExists()
    {
      using (var rep = new BearerUserRepository<User>(this.Context.Object))
      {
        var exists = await rep.Clients.ExistsAsync("3");
        Assert.IsTrue(exists);
        exists = await rep.Clients.ExistsAsync("4");
        Assert.IsFalse(exists);
      }
    }

    [TestMethod]
    public async Task TestAdd()
    {
      using (var rep = new BearerUserRepository<User>(this.Context.Object))
      {
        var newRT = (Client) this.SingleObject.Clone();
        newRT.Id = "4";
        var addedSuccessfully = await rep.Clients.AddAsync(newRT);

        this.DbSet.Verify(m => m.Add(It.IsAny<Client>()), Times.Once());
        this.Context.Verify(m => m.SaveChangesAsync(), Times.Once());
        Assert.IsNotNull(addedSuccessfully);
      }
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public async Task TestAdd_NullParameterCallCausesExeption()
    {
      using (var rep = new BearerUserRepository<User>(this.Context.Object))
      { 
        var addedSuccessfully = await rep.Clients.AddAsync(null);
        Assert.Fail();
      }
    }

    [TestMethod]
    public async Task TestRemove()
    {
      using (var rep = new BearerUserRepository<User>(this.Context.Object))
      {
        var removedSuccessfully = await rep.Clients.RemoveAsync(this.SingleObject);

        this.DbSet.Verify(m => m.Remove(It.IsAny<Client>()), Times.Once());
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
        Client removeObject = null;
        await rep.Clients.RemoveAsync(removeObject);
        Assert.Fail();
      }
    }

    #endregion

    #region PROPERTIES

    private Mock<BearerDbContext> Context { get; set; }
    private Mock<DbSet<Client>> DbSet { get; set; }
    private Client SingleObject { get; set; }
    private ObservableCollection<Client> MultipleObjects { get; set; }

    #endregion

  }
}
