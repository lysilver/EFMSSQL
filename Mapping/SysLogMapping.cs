using Entity;
using System.Data.Entity.ModelConfiguration;

namespace Mapping
{
    public class SysLogMapping : EntityTypeConfiguration<SysLog>
    {
        public SysLogMapping()
        {
            this.ToTable("SysLog");
            this.HasKey(t => t.LogId); //主键
            this.Property(t => t.LogId).HasMaxLength(36).HasColumnType("nvarchar");
            this.Property(t => t.LogIp).HasMaxLength(20).HasColumnType("nvarchar");
            this.Property(t => t.LogType).HasMaxLength(20).HasColumnType("nvarchar");
            this.Property(t => t.LogMessage).HasMaxLength(500).HasColumnType("nvarchar");
            this.Property(t => t.LogStackTrace).HasMaxLength(500).HasColumnType("nvarchar");
            this.Property(t => t.LogUrl).HasMaxLength(50).HasColumnType("nvarchar");
            this.Property(t => t.Creator).HasMaxLength(32).HasColumnType("nvarchar");
            this.Property(t => t.ModifiedBy).HasMaxLength(32).HasColumnType("nvarchar");
        }
    }
}