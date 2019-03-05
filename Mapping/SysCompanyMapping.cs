using Entity;
using System.Data.Entity.ModelConfiguration;

namespace Mapping
{
    public class SysCompanyMapping : EntityTypeConfiguration<SysCompany>
    {
        public SysCompanyMapping()
        {
            this.ToTable("SysCompany");
            this.HasKey(t => t.CompanyId); //主键
            this.Property(t => t.Creator).HasMaxLength(32).HasColumnType("nvarchar");
            this.Property(t => t.ModifiedBy).HasMaxLength(32).HasColumnType("nvarchar");
            this.Property(t => t.CompanyAddress).HasMaxLength(20).HasColumnType("nvarchar");
            this.Property(t => t.CompanyBank).HasMaxLength(20).HasColumnType("nvarchar");
            this.Property(t => t.CompanyBankAccount).HasMaxLength(25).HasColumnType("nvarchar");
            this.Property(t => t.CompanyPerson).HasMaxLength(25).HasColumnType("nvarchar");
            this.Property(t => t.CompanyNum).HasMaxLength(25).HasColumnType("nvarchar");
        }
    }
}