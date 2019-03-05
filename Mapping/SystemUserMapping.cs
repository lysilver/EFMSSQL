using Entity;
using System.Data.Entity.ModelConfiguration;

namespace Mapping
{
    public class SystemUserMapping : EntityTypeConfiguration<SystemUser>
    {
        public SystemUserMapping()
        {
            this.ToTable("SystemUser");
            this.HasKey(t => t.SystemUserId); //主键
            this.Property(t => t.SystemUserId).HasMaxLength(36).HasColumnType("nvarchar");
            this.Property(t => t.City).HasMaxLength(20).HasColumnType("nvarchar");
            this.Property(t => t.Province).HasMaxLength(20).HasColumnType("nvarchar");
            this.Property(t => t.Creator).HasMaxLength(32).HasColumnType("nvarchar");
            this.Property(t => t.LoginIp).HasMaxLength(20).HasColumnType("nvarchar");
            this.Property(t => t.LoginIp2).HasMaxLength(20).HasColumnType("nvarchar");
            this.Property(t => t.ModifiedBy).HasMaxLength(32).HasColumnType("nvarchar");
            this.Property(t => t.Pwd).HasMaxLength(32).HasColumnType("nvarchar");
            this.Property(t => t.Remark).HasMaxLength(500).HasColumnType("nvarchar");
            this.Property(t => t.RoleId).HasMaxLength(500).HasColumnType("nvarchar");
            this.Property(t => t.RoleName).HasMaxLength(20).HasColumnType("nvarchar");
            this.Property(t => t.Sex).HasMaxLength(10).HasColumnType("nvarchar");
            this.Property(t => t.SystemUserName).HasMaxLength(50).HasColumnType("nvarchar");
            this.Property(t => t.SystemUserNickName).HasMaxLength(50).HasColumnType("nvarchar");
            this.Property(t => t.Telephone).HasMaxLength(15).HasColumnType("nvarchar");
            this.Property(t => t.WeChat).HasMaxLength(32).HasColumnType("nvarchar");
        }
    }
}