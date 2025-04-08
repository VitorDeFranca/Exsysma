﻿// <auto-generated />
using ExsysmaAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ExsysmaAPI.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250408175401_new user class")]
    partial class newuserclass
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.3");

            modelBuilder.Entity("ExsysmaAPI.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Responsible")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("ExsysmaAPI.Models.Rule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ConclusionRuleItemId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProjectId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ConclusionRuleItemId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Rules");
                });

            modelBuilder.Entity("ExsysmaAPI.Models.RuleItem", b =>
                {
                    b.Property<int>("RuleItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Operator")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("RuleId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("VariableId")
                        .HasColumnType("INTEGER");

                    b.HasKey("RuleItemId");

                    b.HasIndex("RuleId");

                    b.HasIndex("VariableId");

                    b.ToTable("RuleItem");
                });

            modelBuilder.Entity("ExsysmaAPI.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ExsysmaAPI.Models.Variable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsGoalVariable")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.PrimitiveCollection<string>("PossibleValues")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ProjectId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("QuestionDescription")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Variables");
                });

            modelBuilder.Entity("ExsysmaAPI.Models.Rule", b =>
                {
                    b.HasOne("ExsysmaAPI.Models.RuleItem", "Conclusion")
                        .WithMany()
                        .HasForeignKey("ConclusionRuleItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ExsysmaAPI.Models.Project", null)
                        .WithMany("Rules")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Conclusion");
                });

            modelBuilder.Entity("ExsysmaAPI.Models.RuleItem", b =>
                {
                    b.HasOne("ExsysmaAPI.Models.Rule", null)
                        .WithMany("Conditions")
                        .HasForeignKey("RuleId");

                    b.HasOne("ExsysmaAPI.Models.Variable", "Variable")
                        .WithMany()
                        .HasForeignKey("VariableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Variable");
                });

            modelBuilder.Entity("ExsysmaAPI.Models.Variable", b =>
                {
                    b.HasOne("ExsysmaAPI.Models.Project", null)
                        .WithMany("Variables")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ExsysmaAPI.Models.Project", b =>
                {
                    b.Navigation("Rules");

                    b.Navigation("Variables");
                });

            modelBuilder.Entity("ExsysmaAPI.Models.Rule", b =>
                {
                    b.Navigation("Conditions");
                });
#pragma warning restore 612, 618
        }
    }
}
