﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RecordStore.Data.EF
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class RecordStoreEntities : DbContext
    {
        public RecordStoreEntities()
            : base("name=RecordStoreEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Album> Album { get; set; }
        public virtual DbSet<AlbumArtist> AlbumArtist { get; set; }
        public virtual DbSet<AlbumGenre> AlbumGenre { get; set; }
        public virtual DbSet<AlbumStatus> AlbumStatus { get; set; }
        public virtual DbSet<Artist> Artist { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Format> Format { get; set; }
        public virtual DbSet<Genre> Genre { get; set; }
        public virtual DbSet<Label> Label { get; set; }
        public virtual DbSet<UserDetails> UserDetails { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<AlbumRecording> AlbumRecording { get; set; }
        public virtual DbSet<Recording> Recording { get; set; }
        public virtual DbSet<Channel> Channel { get; set; }
    }
}
