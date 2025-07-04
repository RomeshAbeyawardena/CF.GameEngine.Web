﻿// <auto-generated />
using System;
using CF.Identity.Infrastructure.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CF.Identity.Infrastructure.SqlServer.Migrations
{
    [DbContext(typeof(CFIdentityDbContext))]
    partial class CFIdentityDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CF.Identity.Infrastructure.SqlServer.Models.DbAccessRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("RoleId");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DisplayName")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.HasIndex("ClientId", "Key");

                    b.ToTable("Role", "dbo");
                });

            modelBuilder.Entity("CF.Identity.Infrastructure.SqlServer.Models.DbAccessToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("AccessTokenId");

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ReferenceToken")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("RefreshToken")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("RevokeReason")
                        .HasMaxLength(600)
                        .HasColumnType("nvarchar(600)");

                    b.Property<string>("RevokedBy")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<DateTimeOffset?>("SuspendedTimestampUtc")
                        .HasColumnType("datetimeoffset(7)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("nvarchar(120)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("ValidFrom")
                        .HasColumnType("datetimeoffset(7)");

                    b.Property<DateTimeOffset?>("ValidTo")
                        .HasColumnType("datetimeoffset(7)");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("UserId");

                    b.ToTable("AccessToken", "dbo");
                });

            modelBuilder.Entity("CF.Identity.Infrastructure.SqlServer.Models.DbClient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ClientId");

                    b.Property<string>("DisplayName")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<bool>("IsSystem")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("nvarchar(120)");

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<string>("SecretHash")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<DateTimeOffset?>("SuspendedTimestampUtc")
                        .HasColumnType("datetimeoffset(7)");

                    b.Property<DateTimeOffset>("ValidFrom")
                        .HasColumnType("datetimeoffset(7)");

                    b.Property<DateTimeOffset?>("ValidTo")
                        .HasColumnType("datetimeoffset(7)");

                    b.HasKey("Id");

                    b.ToTable("Client", "dbo");
                });

            modelBuilder.Entity("CF.Identity.Infrastructure.SqlServer.Models.DbCommonName", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CommonNameId");

                    b.Property<bool>("IsAnonymisedMarker")
                        .HasColumnType("bit");

                    b.Property<string>("MetaData")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("ValueCI")
                        .IsRequired()
                        .HasMaxLength(344)
                        .HasColumnType("nvarchar(344)");

                    b.Property<string>("ValueHmac")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.HasKey("Id");

                    b.ToTable("CommonName", "dbo");
                });

            modelBuilder.Entity("CF.Identity.Infrastructure.SqlServer.Models.DbRoleScope", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("RoleScopeId");

                    b.Property<Guid>("AccessRoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("DbUserAccessRoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ScopeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasAlternateKey("AccessRoleId", "ScopeId");

                    b.HasIndex("DbUserAccessRoleId");

                    b.HasIndex("ScopeId");

                    b.ToTable("RoleScope", "dbo");
                });

            modelBuilder.Entity("CF.Identity.Infrastructure.SqlServer.Models.DbScope", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ScopeId");

                    b.Property<Guid?>("ClientId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ClientId");

                    b.Property<string>("Description")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<bool>("IsPrivileged")
                        .HasColumnType("bit");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Scope", "dbo");
                });

            modelBuilder.Entity("CF.Identity.Infrastructure.SqlServer.Models.DbUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("UserId");

                    b.Property<DateTimeOffset?>("AnonymisedTimestamp")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ClientId");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("EmailAddressCI")
                        .IsRequired()
                        .HasMaxLength(344)
                        .HasColumnType("nvarchar(344)");

                    b.Property<string>("EmailAddressHmac")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<Guid>("FirstCommonNameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<bool>("IsSystem")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<Guid>("LastCommonNameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Metadata")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<Guid?>("MiddleCommonNameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PreferredUsername")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("PreferredUsernameCI")
                        .IsRequired()
                        .HasMaxLength(344)
                        .HasColumnType("nvarchar(344)");

                    b.Property<string>("PreferredUsernameHmac")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("PrimaryTelephoneNumber")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("PrimaryTelephoneNumberHmac")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("UsernameCI")
                        .IsRequired()
                        .HasMaxLength(344)
                        .HasColumnType("nvarchar(344)");

                    b.Property<string>("UsernameHmac")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("FirstCommonNameId");

                    b.HasIndex("LastCommonNameId");

                    b.HasIndex("MiddleCommonNameId");

                    b.ToTable("User", "dbo");
                });

            modelBuilder.Entity("CF.Identity.Infrastructure.SqlServer.Models.DbUserAccessRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("UserRoleId");

                    b.Property<Guid>("AccessRoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("DbUserAccessRoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasAlternateKey("UserId", "AccessRoleId");

                    b.HasIndex("AccessRoleId");

                    b.HasIndex("DbUserAccessRoleId");

                    b.ToTable("UserRole", "dbo");
                });

            modelBuilder.Entity("CF.Identity.Infrastructure.SqlServer.Models.DbUserScope", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("UserScopeId");

                    b.Property<Guid>("ScopeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasAlternateKey("UserId", "ScopeId");

                    b.HasIndex("ScopeId");

                    b.ToTable("UserScope", "dbo");
                });

            modelBuilder.Entity("CF.Identity.Infrastructure.SqlServer.Models.DbAccessRole", b =>
                {
                    b.HasOne("CF.Identity.Infrastructure.SqlServer.Models.DbClient", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("CF.Identity.Infrastructure.SqlServer.Models.DbAccessToken", b =>
                {
                    b.HasOne("CF.Identity.Infrastructure.SqlServer.Models.DbClient", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CF.Identity.Infrastructure.SqlServer.Models.DbUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CF.Identity.Infrastructure.SqlServer.Models.DbRoleScope", b =>
                {
                    b.HasOne("CF.Identity.Infrastructure.SqlServer.Models.DbAccessRole", "Role")
                        .WithMany("Scopes")
                        .HasForeignKey("AccessRoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CF.Identity.Infrastructure.SqlServer.Models.DbUserAccessRole", null)
                        .WithMany("Scopes")
                        .HasForeignKey("DbUserAccessRoleId");

                    b.HasOne("CF.Identity.Infrastructure.SqlServer.Models.DbScope", "Scope")
                        .WithMany()
                        .HasForeignKey("ScopeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("Scope");
                });

            modelBuilder.Entity("CF.Identity.Infrastructure.SqlServer.Models.DbScope", b =>
                {
                    b.HasOne("CF.Identity.Infrastructure.SqlServer.Models.DbClient", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Client");
                });

            modelBuilder.Entity("CF.Identity.Infrastructure.SqlServer.Models.DbUser", b =>
                {
                    b.HasOne("CF.Identity.Infrastructure.SqlServer.Models.DbClient", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CF.Identity.Infrastructure.SqlServer.Models.DbCommonName", "FirstCommonName")
                        .WithMany()
                        .HasForeignKey("FirstCommonNameId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CF.Identity.Infrastructure.SqlServer.Models.DbCommonName", "LastCommonName")
                        .WithMany()
                        .HasForeignKey("LastCommonNameId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CF.Identity.Infrastructure.SqlServer.Models.DbCommonName", "MiddleCommonName")
                        .WithMany()
                        .HasForeignKey("MiddleCommonNameId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Client");

                    b.Navigation("FirstCommonName");

                    b.Navigation("LastCommonName");

                    b.Navigation("MiddleCommonName");
                });

            modelBuilder.Entity("CF.Identity.Infrastructure.SqlServer.Models.DbUserAccessRole", b =>
                {
                    b.HasOne("CF.Identity.Infrastructure.SqlServer.Models.DbAccessRole", "AccessRole")
                        .WithMany("UserRoles")
                        .HasForeignKey("AccessRoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CF.Identity.Infrastructure.SqlServer.Models.DbUserAccessRole", null)
                        .WithMany("UserRoles")
                        .HasForeignKey("DbUserAccessRoleId");

                    b.HasOne("CF.Identity.Infrastructure.SqlServer.Models.DbUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AccessRole");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CF.Identity.Infrastructure.SqlServer.Models.DbUserScope", b =>
                {
                    b.HasOne("CF.Identity.Infrastructure.SqlServer.Models.DbScope", "Scope")
                        .WithMany("UserScopes")
                        .HasForeignKey("ScopeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CF.Identity.Infrastructure.SqlServer.Models.DbUser", "User")
                        .WithMany("UserScopes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Scope");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CF.Identity.Infrastructure.SqlServer.Models.DbAccessRole", b =>
                {
                    b.Navigation("Scopes");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("CF.Identity.Infrastructure.SqlServer.Models.DbScope", b =>
                {
                    b.Navigation("UserScopes");
                });

            modelBuilder.Entity("CF.Identity.Infrastructure.SqlServer.Models.DbUser", b =>
                {
                    b.Navigation("UserScopes");
                });

            modelBuilder.Entity("CF.Identity.Infrastructure.SqlServer.Models.DbUserAccessRole", b =>
                {
                    b.Navigation("Scopes");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
