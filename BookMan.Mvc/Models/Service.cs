﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace BookMan.Mvc.Models
{
    public class Service
    {
        private readonly string _dataFilePathRoot = @"Data";
        private readonly string _dataFile = @"Data\data.xml";
        private readonly XmlSerializer _serializer = new XmlSerializer(typeof(HashSet<Book>));
        public HashSet<Book> Books { get; set; }

        public Service(string fileName)
        {
            _dataFile = Path.Combine(_dataFilePathRoot, fileName + ".xml");
            if (!File.Exists(_dataFile))
            {
                Books = new HashSet<Book>() {
                    new Book{Id = 1, Name = "ASP.NET Core for dummy", Authors = "Trump D.", Publisher = "Washington", Year = 2020},
                    new Book{Id = 2, Name = "Pro ASP.NET Core", Authors = "Putin V.", Publisher = "Moscow", Year = 2020},
                    new Book{Id = 3, Name = "ASP.NET Core Video course", Authors = "Obama B.", Publisher = "Washington", Year = 2020},
                    new Book{Id = 4, Name = "Programming ASP.NET Core MVC", Authors = "Clinton B.", Publisher = "Washington", Year = 2020},
                    new Book{Id = 5, Name = "ASP.NET Core Razor Pages", Authors = "Yelstin B.", Publisher = "Moscow", Year = 2020},
                };
                SaveChanges();
            }
            else
            {
                using var stream = File.OpenRead(_dataFile);
                Books = _serializer.Deserialize(stream) as HashSet<Book>;
            }
        }

        public Book[] Get() => Books.ToArray();

        public Book Get(int id) => Books.FirstOrDefault(b => b.Id == id);

        public bool Add(Book book) => Books.Add(book);

        public Book Create()
        {
            var max = Books.Max(b => b.Id);
            var b = new Book()
            {
                Id = max + 1,
                Year = DateTime.Now.Year
            };
            return b;
        }

        public bool Update(Book book)
        {
            var b = Get(book.Id);
            return b != null && Books.Remove(b) && Books.Add(book);
        }

        public bool Delete(int id)
        {
            var b = Get(id);
            return b != null && Books.Remove(b);
        }

        public void SaveChanges()
        {
            using var stream = File.Create(_dataFile);
            _serializer.Serialize(stream, Books);
        }

        public string GetDataPath(string file) => $"Data\\{file}";

        public void Upload(Book book, IFormFile file)
        {
            if (file != null)
            {
                var path = GetDataPath(file.FileName);
                using var stream = new FileStream(path, FileMode.Create);
                file.CopyTo(stream);
                book.DataFile = file.FileName;
            }
        }
    }
}