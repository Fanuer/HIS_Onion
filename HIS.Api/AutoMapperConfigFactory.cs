﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web;
using AutoMapper;
using HIS.Api.Models;
using HIS.WebApi.Helper.Interfaces;


namespace HIS.Api
{
  public class AutoMapperConfigFactory
  {
    #region FIELDS

    private static AutoMapperConfigFactory _instance;
    #endregion

    #region CTOR

    private AutoMapperConfigFactory()
    {
      
    }
    #endregion

    #region METHODS

    public MapperConfiguration Create()
    {
      return new MapperConfiguration(cfg =>
      {
        cfg.CreateMap<Onion.Client.IUser, IUser<int>>().ConstructUsing(this.FromOnionUser);

      });
    }

    private IUser<int> FromOnionUser(Onion.Client.IUser user)
    {
      var claims = new List<Claim>
      {
        new Claim("admin", user.IsAdministrator.ToString()),
        new Claim("editor", user.IsEditor.ToString()),
        new Claim("live-editor", user.IsEditor.ToString()),
        new Claim("live-editor", user.IsLiveEditor.ToString()),
        new Claim("achived", user.IsArchived.ToString())
      };

      return new User(user.Id, user.Name, user.DisplayName, claims);
    }
    #endregion

    #region PROPERTIES

    public static AutoMapperConfigFactory Instance
    {
      get
      {
        if (_instance == null)
        {
          lock (typeof(AutoMapperConfigFactory))
          {
            if (_instance == null)
            {
              _instance = new AutoMapperConfigFactory();
            }
          }
        }
        return _instance;
        
      }
    }
    #endregion

  }
}