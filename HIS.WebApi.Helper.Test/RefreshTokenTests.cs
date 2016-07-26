using System.Data.Entity;
using HIS.WebApi.Auth.Base.Models;
using HIS.WebApi.Auth.Base.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HIS.WebApi.Auth.Base.Test
{
  [TestClass]
  public class RefreshTokenTests
  {


    [TestMethod]
    public void AddToken()
    {
      var mockSet = new Mock<DbSet<RefreshToken>>();
      var mockContext = new Mock<AuthContext>();
      mockContext.Setup(m => m.RefreshTokens).Returns(mockSet.Object);

      
    }
  }
}
