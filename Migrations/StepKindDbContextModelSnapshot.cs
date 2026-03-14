using System;
using backend_stepkind.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pgvector;

#nullable disable

namespace backend_stepkind.Migrations;

[DbContext(typeof(StepKindDbContext))]
partial class StepKindDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "10.0.2")
            .HasAnnotation("Relational:MaxIdentifierLength", 63);

        modelBuilder.HasPostgresExtension("vector");

        modelBuilder.Entity("backend_stepkind.Models.Tutorial", b =>
        {
            b.Property<string>("Id").HasMaxLength(200).HasColumnType("character varying(200)");
            b.Property<DateTime>("CreatedAt").ValueGeneratedOnAdd().HasDefaultValueSql("timezone('utc', now())").HasColumnType("timestamp with time zone");
            b.Property<string>("Description").HasMaxLength(5000).HasColumnType("character varying(5000)");
            b.Property<string>("Name").IsRequired().HasMaxLength(300).HasColumnType("character varying(300)");
            b.Property<string>("SearchContent").IsRequired().HasColumnType("text");
            b.Property<string>("Transcript").HasColumnType("text");
            b.Property<DateTime>("UpdatedAt").ValueGeneratedOnAdd().HasDefaultValueSql("timezone('utc', now())").HasColumnType("timestamp with time zone");
            b.Property<string>("Url").IsRequired().HasMaxLength(1000).HasColumnType("character varying(1000)");
            b.HasKey("Id");
            b.HasIndex("CreatedAt");
            b.HasIndex("Name");
            b.ToTable("Tutorials");
        });

        modelBuilder.Entity("backend_stepkind.Models.TutorialChunk", b =>
        {
            b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uuid");
            b.Property<int>("ChunkIndex").HasColumnType("integer");
            b.Property<string>("Content").IsRequired().HasColumnType("text");
            b.Property<DateTime>("CreatedAt").ValueGeneratedOnAdd().HasDefaultValueSql("timezone('utc', now())").HasColumnType("timestamp with time zone");
            b.Property<Vector>("Embedding").IsRequired().HasColumnType("vector(1536)");
            b.Property<string>("TutorialId").IsRequired().HasMaxLength(200).HasColumnType("character varying(200)");
            b.HasKey("Id");
            b.HasIndex("TutorialId", "ChunkIndex").IsUnique();
            b.ToTable("TutorialChunks");
        });

        modelBuilder.Entity("backend_stepkind.Models.TutorialChunk", b =>
        {
            b.HasOne("backend_stepkind.Models.Tutorial", "Tutorial")
                .WithMany("Chunks")
                .HasForeignKey("TutorialId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("Tutorial");
        });

        modelBuilder.Entity("backend_stepkind.Models.Tutorial", b =>
        {
            b.Navigation("Chunks");
        });
#pragma warning restore 612, 618
    }
}
