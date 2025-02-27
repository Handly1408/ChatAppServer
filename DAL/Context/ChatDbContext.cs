using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Context
{
    using DAL.Entities;
    using Microsoft.EntityFrameworkCore;
    using System.Text.Json;

    public class ChatDbContext : DbContext
    {
        public DbSet<ChatContactEntity> ChatContactEntities { get; set; }
        public DbSet<ChatUserNotificationTokenEntity> ChatUserNotificationTokenEntities { get; set; }
        public DbSet<ChatUserProfileDataEntity> ChatUserProfileDataEntities { get; set; }

        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatContactEntity>().HasKey(u => u.Id); // Указываем ключ
            modelBuilder.Entity<ChatUserNotificationTokenEntity>().HasKey(u => u.Id); // Указываем ключ
            modelBuilder.Entity<ChatUserProfileDataEntity>().HasKey(u => u.Id); // Указываем ключ
            modelBuilder.Entity<ChatContactEntityBase>().HasKey(c => c.Id); // Предполагая, что Id есть в EntityBase

            // Или вручную с Newtonsoft.Json/System.Text.Json:
            modelBuilder.Entity<ChatContactEntityBase>()
                .Property(c => c.AvatarDefaultColor)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v,JsonSerializerOptions.Default), // Сериализация в JSON
                    v => v == null ? null : JsonSerializer.Deserialize<List<int>>(v,JsonSerializerOptions.Default) // Десериализация из JSON
                );
            modelBuilder.Entity<ChatContactEntity>().Property(c => c.Members).HasConversion(v => v == null ? null :
            JsonSerializer.Serialize(v, JsonSerializerOptions.Default)
            , v => v == null ? null : JsonSerializer.Deserialize<List<string>>(v, JsonSerializerOptions.Default));


        }
    }
}
