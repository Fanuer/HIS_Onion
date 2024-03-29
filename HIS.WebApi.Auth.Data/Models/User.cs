﻿using System;
using System.Collections.ObjectModel;
using System.Security.Claims;
using HIS.WebApi.Auth.Data.Interfaces.Models;

namespace HIS.WebApi.Auth.Data.Models
{
    public class User : IUser<string>
    {
        #region FIELDS

        private string _displayName;
        #endregion

        #region CTOR

        private User()
        {
            this.AdditionalClaims = new Collection<Claim>();
        }

        public User(int id) : this()
        {
            this.Id = id;
        }

        public User(int id, string userName, string displayName = "") : this(id)
        {
            if (String.IsNullOrWhiteSpace(userName)) { throw new ArgumentNullException(nameof(userName)); }

            UserName = userName;
            this.DisplayName = displayName;
        }
        #endregion

        #region METHODS

        #endregion

        #region PROPERTIES
        public int Id { get; set; }

        string IEntity<string>.Id => this.Id.ToString();

        

        public string UserName { get; set; }

        public string DisplayName
        {
            get
            {
                return this._displayName ?? this.UserName;
            }
            set
            {
                this._displayName = value;
            }
        }

        public string Password
        {
            internal get { return this.Password; }
            set { this.Password = value; }
        }

        public Collection<Claim> AdditionalClaims { get; set; }


        #endregion
    }
}