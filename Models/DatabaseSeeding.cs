using DevGuide.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public static class DatabaseSeeding
    {
        public static void DataSeed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "HR", NormalizedName = "HR" },
                new IdentityRole { Name = "Developer", NormalizedName = "DEVELOPER" },
                new IdentityRole { Name = "Mentor", NormalizedName = "MENTOR" }
            );
            modelBuilder.Entity<Skill>().HasData(
                new Skill { Id = 1, Name = "Angular", Description = "A platform for building mobile and desktop web applications" },
                new Skill { Id = 2, Name = "React", Description = "A JavaScript library for building user interfaces" },
                new Skill { Id = 3, Name = "Vue.js", Description = "A progressive JavaScript framework for building user interfaces" },
                new Skill { Id = 4, Name = "Node.js", Description = "A JavaScript runtime built on Chrome's V8 JavaScript engine" },
                new Skill { Id = 5, Name = "ASP.NET Core", Description = "A cross-platform, high-performance framework for building modern, cloud-based applications" },
                new Skill { Id = 6, Name = "C#", Description = "A modern, object-oriented programming language developed by Microsoft" },
                new Skill { Id = 7, Name = "Java", Description = "A high-level programming language used for building large-scale applications" },
                new Skill { Id = 8, Name = "Python", Description = "A high-level, interpreted programming language known for its simplicity" },
                new Skill { Id = 9, Name = "Django", Description = "A high-level Python Web framework that encourages rapid development" },
                new Skill { Id = 10, Name = "Flask", Description = "A lightweight WSGI web application framework in Python" },
                new Skill { Id = 11, Name = "Ruby on Rails", Description = "A server-side web application framework written in Ruby" },
                new Skill { Id = 12, Name = "JavaScript", Description = "A versatile language for building interactive web applications" },
                new Skill { Id = 13, Name = "TypeScript", Description = "A strongly typed programming language that builds on JavaScript" },
                new Skill { Id = 14, Name = "HTML", Description = "The standard markup language for creating web pages" },
                new Skill { Id = 15, Name = "CSS", Description = "A style sheet language used for describing the look of a document" },
                new Skill { Id = 16, Name = "Sass", Description = "A preprocessor scripting language that is interpreted or compiled into CSS" },
                new Skill { Id = 17, Name = "Bootstrap", Description = "A popular front-end open-source toolkit for developing with HTML, CSS, and JS" },
                new Skill { Id = 18, Name = "SQL", Description = "A domain-specific language used for managing relational databases" },
                new Skill { Id = 19, Name = "MySQL", Description = "An open-source relational database management system" },
                new Skill { Id = 20, Name = "PostgreSQL", Description = "An advanced, open-source relational database system" },
                new Skill { Id = 21, Name = "MongoDB", Description = "A document-based, distributed database built for modern applications" },
                new Skill { Id = 22, Name = "Firebase", Description = "A platform developed by Google for creating mobile and web applications" },
                new Skill { Id = 23, Name = "Docker", Description = "A platform for developing, shipping, and running applications in containers" },
                new Skill { Id = 24, Name = "Kubernetes", Description = "An open-source container orchestration system for automating deployment" },
                new Skill { Id = 25, Name = "Git", Description = "A distributed version control system for tracking changes in source code" },
                new Skill { Id = 26, Name = "GitHub", Description = "A web-based interface for Git, offering source code management" },
                new Skill { Id = 27, Name = "Azure", Description = "Microsoft's cloud computing service" },
                new Skill { Id = 28, Name = "AWS", Description = "Amazon's cloud computing service platform" },
                new Skill { Id = 29, Name = "Google Cloud", Description = "Google's cloud computing service" },
                new Skill { Id = 30, Name = ".NET", Description = "A free, cross-platform, open-source developer platform for building apps" },
                new Skill { Id = 31, Name = "Swift", Description = "A powerful and intuitive programming language for macOS, iOS, watchOS, and tvOS" },
                new Skill { Id = 32, Name = "Kotlin", Description = "A modern programming language that makes developers happier" },
                new Skill { Id = 33, Name = "PHP", Description = "A popular general-purpose scripting language that is especially suited to web development" },
                new Skill { Id = 34, Name = "Laravel", Description = "A PHP framework for web artisans" },
                new Skill { Id = 35, Name = "Spring Boot", Description = "A Java-based framework used to create stand-alone applications" },
                new Skill { Id = 36, Name = "TensorFlow", Description = "An open-source machine learning framework" },
                new Skill { Id = 37, Name = "PyTorch", Description = "An open-source machine learning library based on the Torch library" },
                new Skill { Id = 38, Name = "Unity", Description = "A cross-platform game engine" },
                new Skill { Id = 39, Name = "Unreal Engine", Description = "A game engine developed by Epic Games" }
                );

        }
    }
}
