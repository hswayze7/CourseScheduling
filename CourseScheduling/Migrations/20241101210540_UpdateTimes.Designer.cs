﻿// <auto-generated />
using System;
using CourseScheduling.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CourseScheduling.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241101210540_UpdateTimes")]
    partial class UpdateTimes
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CourseScheduling.Models.Course", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseId"));

                    b.Property<string>("CourseCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Credits")
                        .HasColumnType("int");

                    b.Property<string>("Professor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Time")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CourseId");

                    b.ToTable("Courses");

                    b.HasData(
                        new
                        {
                            CourseId = 1,
                            CourseCode = "CS101",
                            CourseName = "Introduction to Computer Science",
                            Credits = 3,
                            Professor = "Prof1",
                            Time = "M/W/F 10:00 AM - 12:00 PM"
                        },
                        new
                        {
                            CourseId = 2,
                            CourseCode = "MATH201",
                            CourseName = "Calculus II",
                            Credits = 4,
                            Professor = "Prof2",
                            Time = "T/TR 1:00 PM - 3:00 PM"
                        },
                        new
                        {
                            CourseId = 3,
                            CourseCode = "CS400",
                            CourseName = "Data Structures",
                            Credits = 4,
                            Professor = "Prof3",
                            Time = "T/TR 2:15 PM - 3:30 PM"
                        },
                        new
                        {
                            CourseId = 4,
                            CourseCode = "CS510",
                            CourseName = "Programming Language Concepts",
                            Credits = 3,
                            Professor = "Prof13",
                            Time = "T/TR 8:30 AM - 9:50 AM"
                        },
                        new
                        {
                            CourseId = 5,
                            CourseCode = "MATH231",
                            CourseName = "Discrete Math",
                            Credits = 3,
                            Professor = "Prof4",
                            Time = "M/W/F 1:00 PM - 3:00 PM"
                        },
                        new
                        {
                            CourseId = 6,
                            CourseCode = "PSY325",
                            CourseName = "Developmental Psychology",
                            Credits = 3,
                            Professor = "Prof5",
                            Time = "M/T/F 12:30 PM - 1:45 PM"
                        },
                        new
                        {
                            CourseId = 7,
                            CourseCode = "SOC338",
                            CourseName = "Health & Lifestyle",
                            Credits = 3,
                            Professor = "Prof6",
                            Time = "M/W/F 2:15 PM - 3:30 PM"
                        },
                        new
                        {
                            CourseId = 8,
                            CourseCode = "MKT690J",
                            CourseName = "Social Media Marketing",
                            Credits = 3,
                            Professor = "Prof7",
                            Time = "T/TR 9:30 AM - 10:45 AM"
                        },
                        new
                        {
                            CourseId = 9,
                            CourseCode = "MGMT681",
                            CourseName = "Strategic Management",
                            Credits = 3,
                            Professor = "Prof8",
                            Time = "M/F/W 3:30 PM - 4:45 PM"
                        },
                        new
                        {
                            CourseId = 10,
                            CourseCode = "NURS362",
                            CourseName = "Clinical Care Lab",
                            Credits = 1,
                            Professor = "Prof9",
                            Time = "F 1:00 PM - 4:00 PM"
                        },
                        new
                        {
                            CourseId = 11,
                            CourseCode = "GEOG235",
                            CourseName = "Meteorology",
                            Credits = 3,
                            Professor = "Prof10",
                            Time = "M/W/F 2:15 PM - 3:30 PM"
                        },
                        new
                        {
                            CourseId = 12,
                            CourseCode = "ENGR101",
                            CourseName = "Introduction to Engineering",
                            Credits = 3,
                            Professor = "Prof11",
                            Time = "T/TR 11:15 AM - 12:25 PM"
                        },
                        new
                        {
                            CourseId = 13,
                            CourseCode = "CHEM221",
                            CourseName = "General Chemistry I",
                            Credits = 5,
                            Professor = "Prof12",
                            Time = "T/TR 4:30 PM - 6:00 PM"
                        });
                });

            modelBuilder.Entity("CourseScheduling.Models.Enrollment", b =>
                {
                    b.Property<int>("EnrollmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EnrollmentId"));

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EnrollmentDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("EnrollmentId");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentId");

                    b.ToTable("Enrollments");

                    b.HasData(
                        new
                        {
                            EnrollmentId = 1,
                            CourseId = 1,
                            EnrollmentDate = new DateTime(2024, 11, 1, 16, 5, 39, 531, DateTimeKind.Local).AddTicks(1512),
                            StudentId = 1
                        });
                });

            modelBuilder.Entity("CourseScheduling.Models.Student", b =>
                {
                    b.Property<int>("StudentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StudentId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Major")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Year")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StudentId");

                    b.ToTable("Students");

                    b.HasData(
                        new
                        {
                            StudentId = 1,
                            Email = "test@student.com",
                            Major = "Computer Science",
                            Name = "Test Student",
                            Password = "test123",
                            Year = "Sophomore"
                        });
                });

            modelBuilder.Entity("CourseScheduling.Models.StudentCourse", b =>
                {
                    b.Property<int>("StudentCourseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StudentCourseId"));

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<DateTime>("RegisteredAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("StudentCourseId");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentId");

                    b.ToTable("StudentCourses");
                });

            modelBuilder.Entity("CourseScheduling.Models.Enrollment", b =>
                {
                    b.HasOne("CourseScheduling.Models.Course", "Course")
                        .WithMany("Enrollments")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CourseScheduling.Models.Student", "Student")
                        .WithMany("Enrollments")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("CourseScheduling.Models.StudentCourse", b =>
                {
                    b.HasOne("CourseScheduling.Models.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CourseScheduling.Models.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("CourseScheduling.Models.Course", b =>
                {
                    b.Navigation("Enrollments");
                });

            modelBuilder.Entity("CourseScheduling.Models.Student", b =>
                {
                    b.Navigation("Enrollments");
                });
#pragma warning restore 612, 618
        }
    }
}
