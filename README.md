# TokoBukuAPI

REST API sederhana untuk manajemen toko buku, dibuat menggunakan ASP.NET Core Web API dan PostgreSQL. Project ini dibuat untuk tugas LKM 1 mata kuliah Pemrograman Antarmuka Aplikasi Universitas Jember.

Domain yang dipilih adalah toko buku dengan 3 entitas utama: kategori buku, buku, dan pesanan.

---

## Teknologi

- Bahasa: C#
- Framework: ASP.NET Core Web API (.NET 8)
- Database: PostgreSQL
- Library: Npgsql
- Tools: Visual Studio 2022, pgAdmin 4

---

## Instalasi dan Cara Menjalankan

1. Clone repository ini
   ```
   git clone https://github.com/Cleonixx/TokoBukuAPI.git
   ```

2. Import database (lihat bagian di bawah)

3. Buka file `appsettings.json`, sesuaikan connection string dengan konfigurasi PostgreSQL kamu
   ```json
   "DefaultConnection": "Host=localhost;Port=5432;Database=toko_buku;Username=postgres;Password=PASSWORDMU"
   ```

4. Buka project di Visual Studio 2022, tekan F5 untuk menjalankan. Swagger akan otomatis terbuka di browser.

---

## Cara Import Database

1. Buka pgAdmin 4
2. Buat database baru dengan nama `toko_buku`
3. Buka Query Tool pada database tersebut
4. Jalankan file `database.sql` yang ada di repository ini

---

## Endpoint

### Categories

| Method | URL | Keterangan |
|--------|-----|------------|
| GET | /api/categories | Ambil semua kategori |
| GET | /api/categories/{id} | Ambil kategori berdasarkan ID |
| POST | /api/categories | Tambah kategori baru |
| PUT | /api/categories/{id} | Update kategori |
| DELETE | /api/categories/{id} | Hapus kategori |

### Books

| Method | URL | Keterangan |
|--------|-----|------------|
| GET | /api/books | Ambil semua buku |
| GET | /api/books/{id} | Ambil buku berdasarkan ID |
| POST | /api/books | Tambah buku baru |
| PUT | /api/books/{id} | Update buku |
| DELETE | /api/books/{id} | Hapus buku |

### Orders

| Method | URL | Keterangan |
|--------|-----|------------|
| GET | /api/orders | Ambil semua order |
| GET | /api/orders/{id} | Ambil order berdasarkan ID |
| POST | /api/orders | Tambah order baru |
| PUT | /api/orders/{id} | Update order |
| DELETE | /api/orders/{id} | Hapus order |

---

## Video Presentasi

[https://youtu.be/9I_H9OnOw4A](https://youtu.be/9I_H9OnOw4A)
