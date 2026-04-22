# 📚 Toko Buku API

REST API Toko Buku menggunakan ASP.NET Core Web API dan PostgreSQL. Mencakup operasi CRUD untuk manajemen kategori buku, data buku, dan pesanan. Dibuat untuk tugas LKM 1 mata kuliah Pemrograman Antarmuka Aplikasi - Universitas Jember.

---

## 🛠️ Teknologi yang Digunakan

| Komponen | Teknologi |
|---|---|
| Bahasa | C# |
| Framework | ASP.NET Core Web API (.NET 8) |
| Database | PostgreSQL |
| Library DB | Npgsql |
| Tools | Visual Studio 2022, pgAdmin 4 |

---

## 📁 Struktur Folder

```
TokoBukuAPI/
├── Controllers/
│   ├── CategoriesController.cs
│   ├── BooksController.cs
│   └── OrdersController.cs
├── Models/
│   ├── Category.cs
│   ├── Book.cs
│   └── Order.cs
├── Data/
│   └── DbHelper.cs
├── appsettings.json
├── Program.cs
└── database.sql
```

---

## ⚙️ Langkah Instalasi & Cara Menjalankan

### Prasyarat
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/) + pgAdmin 4
- [Visual Studio 2022](https://visualstudio.microsoft.com/)

### 1. Clone Repository

```bash
git clone https://github.com/Cleonixx/TokoBukuAPI.git
cd TokoBukuAPI
```

### 2. Import Database

Lihat bagian **Cara Import Database** di bawah.

### 3. Konfigurasi Connection String

Buka file `appsettings.json`, sesuaikan dengan konfigurasi PostgreSQL kamu:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=toko_buku;Username=postgres;Password=PASSWORDMU"
  }
}
```

### 4. Jalankan Project

Buka project di Visual Studio 2022, lalu tekan **F5** atau klik tombol ▶. Swagger UI akan otomatis terbuka di browser.

---

## 🗄️ Cara Import Database

1. Buka **pgAdmin 4**
2. Klik kanan **Databases** → **Create** → **Database**
3. Isi nama database: `toko_buku` → Save
4. Klik database `toko_buku` → buka **Query Tool**
5. Buka file `database.sql` dari repo ini
6. Klik **Run (F5)**

---

## 🔗 Daftar Endpoint

### Categories

| Method | URL | Keterangan |
|---|---|---|
| GET | `/api/categories` | Ambil semua kategori |
| GET | `/api/categories/{id}` | Ambil kategori by ID |
| POST | `/api/categories` | Tambah kategori baru |
| PUT | `/api/categories/{id}` | Update kategori |
| DELETE | `/api/categories/{id}` | Hapus kategori |

### Books

| Method | URL | Keterangan |
|---|---|---|
| GET | `/api/books` | Ambil semua buku |
| GET | `/api/books/{id}` | Ambil buku by ID |
| POST | `/api/books` | Tambah buku baru |
| PUT | `/api/books/{id}` | Update buku |
| DELETE | `/api/books/{id}` | Hapus buku |

### Orders

| Method | URL | Keterangan |
|---|---|---|
| GET | `/api/orders` | Ambil semua order |
| GET | `/api/orders/{id}` | Ambil order by ID |
| POST | `/api/orders` | Tambah order baru (stock otomatis berkurang) |
| PUT | `/api/orders/{id}` | Update order (stock otomatis disesuaikan) |
| DELETE | `/api/orders/{id}` | Hapus order (stock otomatis dikembalikan) |

---

## 🎥 Video Presentasi

[Link video presentasi](https://youtu.be/-DqUz3-clJU)
